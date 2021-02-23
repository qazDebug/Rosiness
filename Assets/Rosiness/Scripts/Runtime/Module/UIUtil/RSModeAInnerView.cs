using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Rosiness;

public class DDModeAInnerView : RSScrollRect {
    private DDModeAView rootView;
    private ScrollRect outerScrollRect;

    private ScrollRect currentRect;


	// Use this for initialization
	void Start () {
        base.Start();

        if (this.horizontal) {
            RosinessLog.Warning("ModeAScrollRect 只支持竖直方向滚动!");
        }
        this.horizontal = false;
        this.vertical = true;

        rootView = this.GetComponentInParent<DDModeAView>();
        if (!rootView) {
            RosinessLog.Error("ModeAScrollRect 未找到DDModeAView的对象");
        }
        rootView.init();
        this.outerScrollRect = rootView.body.gameObject.GetComponent<ScrollRect>();

        RectTransform rtf = this.transform as RectTransform;
        rtf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rootView.body.rect.height);

        this.onInertiaBeforeStart = new OnInertiaBeforeStart(this.onInnerScrollRectInertiaBeforeStart);
        this.onInertiaAfterEnd = new OnInertiaAfterEnd(this.onInnerScrollRectInertiaAfterEnd);
	}

    public override void OnBeginDrag(PointerEventData eventData) {
        RosinessLog.Log("ModeAScrollRect.OnBeginDrag: " + eventData.position + " " + eventData.delta + "@" + eventData.clickTime);
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) {
            this.currentRect = outerScrollRect;
        } else {
            this.currentRect = this;
        }

        if (this.currentRect == this) {
            //base.OnBeginDrag(eventData);
            stopAdjust();
            resetTouchMoves();
        } else if (currentRect) {
            currentRect.OnBeginDrag(eventData);
        }
    }

    public override void OnDrag(PointerEventData eventData) {
        //BELog.debug("ModeAScrollRect.OnDrag: " + eventData.position + " " + eventData.delta + "@" + eventData.clickTime);
        if (this.currentRect == this) {
            //base.OnDrag(eventData);
            Vector2 delta = RSUI.transScreenToWorld(eventData.delta);
            doMoveAsLinked(delta.y);
            gatherTouchMove(delta);
        } else if (currentRect) {
            currentRect.OnDrag(eventData);
        }
    }

    public override void OnEndDrag(PointerEventData eventData) {
        RosinessLog.Log("ModeAScrollRect.OnEndDrag: " + eventData.position + " " + eventData.delta + "@" + eventData.clickTime);
        if (this.currentRect == this) {
            //base.OnEndDrag(eventData);
            Vector2 delta = RSUI.transScreenToWorld(eventData.delta);
            gatherTouchMove(delta);
            startAdjust(delta);
        } else if (currentRect) {
            currentRect.OnEndDrag(eventData);
        }
        currentRect = null;
    }

    private bool doMoveAsLinked(float dy) {
        if (dy == 0) {
            return false;
        }
        //BELog.debug("ModeAScrollRect.doMove " + this.viewport.rect + " " + this.content.rect);
        //BELog.debug("ModeAScrollRect.doMove " + this.verticalNormalizedPosition + " = " + Math.Round(this.verticalNormalizedPosition, 2));
        Vector3 pos = rootView.content.localPosition;
        if (dy > 0) { // up
            if (pos.y < rootView.maxPosY) {
                pos.y = Math.Min(rootView.maxPosY, pos.y + dy);
                rootView.content.localPosition = pos;
                return true;
            } else {
                return this.doScrollY(dy, true);
            }
        } else { // down
            if (this.doScrollY(dy, false)) {
                return true;
            } else if (pos.y > rootView.minPosY) {
                pos.y = Math.Max(rootView.minPosY, pos.y + dy);
                rootView.content.localPosition = pos;
                return true;
            } else if (this.doScrollY(dy, true)) {
                return true;
            }
        }
        return false;
    }

    private bool doMoveRootView(float dy) {
        if (dy == 0) {
            return false;
        }
        Vector3 pos = rootView.content.localPosition;
        if (dy > 0) { // up
            if (pos.y < rootView.maxPosY) {
                pos.y = Math.Min(rootView.maxPosY, pos.y + dy);
                rootView.content.localPosition = pos;
                return true;
            }
        } else { // down
            if (pos.y > rootView.minPosY) {
                pos.y = Math.Max(rootView.minPosY, pos.y + dy);
                rootView.content.localPosition = pos;
                return true;
            }
        }
        return false;
    }


    public override void stopAdjust() {
        base.stopAdjust();
        stopAdjustRootInertia();
    }

    protected override void adjustUpdate() {
        if (Input.GetMouseButton(0)) {
            stopAdjustInertia();
            stopAdjustRootInertia();
            return;
        }
        updateAdjustOverflow();
        updateAdjustInertia();
        updateAdjustRootInertia();
    }

    private void onInnerScrollRectInertiaBeforeStart(Vector2 velocity, Vector2 acceleration) {
        //BELog.debug("ModeAScrollRect.onInnerScrollRectInertiaBeforeStart " + velocity + acceleration);
        if (velocity.y > 0) {
            startAdjustRootInertia(velocity, acceleration);
        } else {
            this.startAdjustInertia(velocity, acceleration);
        }
    }

    private void onInnerScrollRectInertiaAfterEnd(Vector2 velocity, Vector2 acceleration) {
        //BELog.debug("ModeAScrollRect.onInnerScrollRectInertiaAfterEnd " + velocity + acceleration);
        if (velocity.y < 0) {
            startAdjustRootInertia(velocity, acceleration);
        }
    }

    private Vector2 rootInertiaVelocity = Vector2.zero;
    private Vector2 rootInertiaAcceleration = Vector2.zero;
    public virtual bool startAdjustRootInertia(Vector2 velocity, Vector2 acceleration) {
        if (velocity.y != 0) {
            rootInertiaVelocity = velocity;
            rootInertiaAcceleration = acceleration;
            return true;
        }
        return false;
    }

    public virtual void stopAdjustRootInertia() {
        rootInertiaVelocity = Vector2.zero;
    }

    public virtual void stopAdjustRootInertiaAtEnd() {
        if (rootInertiaVelocity.y > 0) {
            this.startAdjustInertia(rootInertiaVelocity, rootInertiaAcceleration);
        }
        rootInertiaVelocity = Vector2.zero;
    }

    protected virtual void updateAdjustRootInertia() {
        if (rootInertiaVelocity == Vector2.zero) {
            return;
        }
        if (horizontal) {
        } else if (vertical) {
            //BELog.debug("DDScrollRect.updateAdjustInertia: " + rootInertiaVelocity.y + ", " + Time.deltaTime);
            if (!doMoveRootView(computeInetiaMoveDistance(rootInertiaVelocity, rootInertiaAcceleration, Time.deltaTime).y)) {
                stopAdjustRootInertiaAtEnd();
                return;
            }
            computeInetiaVelocityAfterMove(ref rootInertiaVelocity, rootInertiaAcceleration, Time.deltaTime);
        }
    }

}
