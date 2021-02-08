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
using Rosiness.Tween;

public class RSScrollRect : ScrollRect {
    public enum State {
        Unknown,
        Moving,
        Stable
    }

    public delegate void OnStateChange(State state);
    private OnStateChange _onStateChange;
    public OnStateChange onStateChange {
        get {
            return _onStateChange;
        }
        set {
            _onStateChange = value;
        }
    }
    protected virtual void fireStateChange(State state) {
        RosinessLog.Log("RSScrollRect.onStateChange " + state + " scalePosition=" + getScalePosition());
        if (_onStateChange != null) {
            _onStateChange(State.Stable);
        }
    }

    public delegate void OnInertiaBeforeStart(Vector2 velocity, Vector2 acceleration);
    public delegate void OnInertiaAfterEnd(Vector2 velocity, Vector2 acceleration);
    private OnInertiaBeforeStart _onInertiaBeforeStart;
    private OnInertiaAfterEnd _onInertiaAfterEnd;
    public OnInertiaBeforeStart onInertiaBeforeStart {
        get {
            return _onInertiaBeforeStart;
        }
        set {
            _onInertiaBeforeStart = value;
        }
    }
    public OnInertiaAfterEnd onInertiaAfterEnd {
        get {
            return _onInertiaAfterEnd;
        }
        set {
            _onInertiaAfterEnd = value;
        }
    }

    protected Vector2 overflow = Vector2.zero;
    protected Vector2 minPos = Vector2.zero;
    protected Vector2 maxPos = Vector2.zero;
    public Vector2 minPosition {
        get {
            return minPos;
        }
    }
    public Vector2 maxPosition {
        get {
            return maxPos;
        }
    }

    private struct TouchMove {
        public Vector2 delta;
        public float time;
    }
    private float touchMovePreviousTimestamp = 0;
    private Queue<TouchMove> touchMoves = new Queue<TouchMove>();

    protected void resetTouchMoves() {
        touchMovePreviousTimestamp = Time.time;
        touchMoves.Clear();
    }
    
    protected void gatherTouchMove(Vector2 delta) {
        if (touchMoves.Count >= 5) {
            touchMoves.Dequeue();
        }
        float nowTime = Time.time;
        TouchMove move = new TouchMove();
        move.delta = delta;
        move.time = nowTime - touchMovePreviousTimestamp;
        touchMoves.Enqueue(move);
        touchMovePreviousTimestamp = nowTime;
    }

    protected Vector2 calculateTouchMoveVelocity() {
        TouchMove[] moves = touchMoves.ToArray();
        float totalTime = 0;
        Vector2 totalDelta = Vector2.zero;
        int count = 0;
        for (int i = moves.Length; --i >= 0;) {
            if (horizontal) {
                if (i < moves.Length - 1) {
                    if ((moves[i].delta.x > 0 && moves[i + 1].delta.x < 0) || (moves[i].delta.x < 0 && moves[i + 1].delta.x > 0)) {
                        break;
                    }
                }
                totalDelta.x += moves[i].delta.x;
            } else if (vertical) {
                if (i < moves.Length - 1) {
                    if ((moves[i].delta.y > 0 && moves[i + 1].delta.y < 0) || (moves[i].delta.y < 0 && moves[i + 1].delta.y > 0)) {
                        break;
                    }
                }
                totalDelta.y += moves[i].delta.y;
            }
            totalTime += moves[i].time;
            count++;
        }
        Vector2 v = Vector2.zero;
        if (totalTime >= 0 && totalTime < 0.5) {
            float brake = 0.5f;
            v = new Vector2(totalDelta.x * (1 - brake) / totalTime,
                totalDelta.y * (1 - brake) / totalTime);
        }
        RosinessLog.Log("RSScrollRect.calculateTouchMoveVelocity count=" + count + " totalTime=" + totalTime + " v=" + v.ToString());
        return v;
    }

    public virtual void Start() {
        base.Start();
        this.movementType = MovementType.Unrestricted;
        this.inertia = false;
        if (this.vertical == this.horizontal) {
            this.vertical = !this.horizontal;
            RosinessLog.Warning("RSScrollRect仅支持水平或者垂直一个方向滚动!");
        }
    }

    public virtual void Update() {
        adjustUpdate();
    }

    public override void OnBeginDrag(PointerEventData eventData) {
        RosinessLog.Log("RSScrollRect.OnBeginDrag: " + eventData.position + " " + eventData.delta + "@" + eventData.clickTime);
        //base.OnBeginDrag(eventData);
        stopAdjust();
        resetTouchMoves();
    }

    public override void OnDrag(PointerEventData eventData) {
        //BELog.info("RSScrollRect.OnDrag: " + eventData.position + " " + eventData.delta + "@" + eventData.clickTime);
        //base.OnDrag(eventData);
        Vector2 delta = RSUI.transScreenToWorld(eventData.delta);
        doScroll(delta, true);
        gatherTouchMove(delta);
    }

    public override void OnEndDrag(PointerEventData eventData) {
        RosinessLog.Log("RSScrollRect.OnEndDrag: " + eventData.position + " " + eventData.delta + "@" + eventData.clickTime + ",");
        //base.OnEndDrag(eventData);
        Vector2 delta = RSUI.transScreenToWorld(eventData.delta);
        gatherTouchMove(delta);
        startAdjust(delta);
    }
    
    protected virtual void prepare() {
        overflow = this.viewport.rect.size * 0.1f;
        if (horizontal) {
            minPos.x = 0 - (this.content.rect.width > this.viewport.rect.width ? this.content.rect.width - this.viewport.rect.width : 0);
            maxPos.x = 0;
        } else if (vertical) {
            minPos.y = 0;
            maxPos.y = 0 + (this.content.rect.height > this.viewport.rect.height ? this.content.rect.height - this.viewport.rect.height : 0);
        }
        //BELog.warn("RSScrollRect.prepare viewport=" + this.viewport.rect + " " + this.viewport.localPosition);
        //BELog.warn("RSScrollRect.prepare content=" + this.content.rect + " " + this.content.localPosition);
        //BELog.warn("RSScrollRect.prepare overflow=" + overflow + " minPos=" + minPos + " maxPos=" + maxPos);
    }

    public Vector2 getPosition() {
        return this.content.localPosition;
    }

    public Rect getBoundRect() {
        return new Rect(minPos.x, minPos.y, maxPos.x - minPos.x, maxPos.y - minPos.y);
    }

    public float getScalePosition() {
        if (horizontal) {
            if (minPos.x != maxPos.x) {
                return 1 - (this.content.localPosition.x - minPos.x) / (maxPos.x - minPos.x);
            }
        } else if (vertical) {
            if (minPos.y != maxPos.y) {
                return (this.content.localPosition.y - minPos.y) / (maxPos.y - minPos.y);
            }
        }
        return 0;
    }

    public bool doScrollX(float delta, bool canOverflow = false) {
        return doScroll(new Vector2(delta, 0), canOverflow);
    }

    public bool doScrollY(float delta, bool canOverflow = false) {
        return doScroll(new Vector2(0, delta), canOverflow);
    }

    public bool doScroll(Vector2 delta, bool canOverflow = false, bool overflowRevert = false) {
        if (this.horizontal && delta.x != 0) {
            prepare();
            Vector3 pos = this.content.localPosition;
            if (delta.x > 0) { // right
                float max = overflowRevert ? minPos.x : (canOverflow ? maxPos.x + overflow.x : maxPos.x);
                if (pos.x < max) {
                    pos.x = Math.Min(max, pos.x + delta.x);
                    this.content.localPosition = pos;
                    return true;
                }
            } else if (delta.x < 0) { // left
                float min = overflowRevert ? maxPos.x : (canOverflow ? minPos.x - overflow.x : minPos.x);
                if (pos.x > min) {
                    pos.x = Math.Max(min, pos.x + delta.x);
                    this.content.localPosition = pos;
                    return true;
                }
            }
        } else if (vertical && delta.y != 0) {
            prepare();
            Vector3 pos = this.content.localPosition;
            if (delta.y > 0) { // up
                float max = overflowRevert ? minPos.y : (canOverflow ? maxPos.y + overflow.y : maxPos.y);
                if (pos.y < max) {
                    pos.y = Math.Min(max, pos.y + delta.y);
                    this.content.localPosition = pos;
                    return true;
                }
            } else if (delta.y < 0) { // down
                float min = overflowRevert ? maxPos.y : (canOverflow ? minPos.y - overflow.y : minPos.y);
                if (pos.y > min) {
                    pos.y = Math.Max(min, pos.y + delta.y);
                    this.content.localPosition = pos;
                    return true;
                }
            }
        }
        return false;
    }

    public bool doScroll(Vector2 delta, Vector2 dest) {
        if (this.horizontal && delta.x != 0) {
            Vector3 pos = this.content.localPosition;
            if (pos.x < dest.x) { // right
                pos.x = Math.Min(dest.x, pos.x + Math.Abs(delta.x));
                this.content.localPosition = pos;
                return true;
            } else if (pos.x > dest.x) { // left
                pos.x = Math.Max(dest.x, pos.x - Math.Abs(delta.x));
                this.content.localPosition = pos;
                return true;
            }
        } else if (vertical && delta.y != 0) {
            Vector3 pos = this.content.localPosition;
            if (pos.y < dest.y) { // up
                pos.y = Math.Min(dest.y, pos.y + Math.Abs(delta.y));
                this.content.localPosition = pos;
                return true;
            } else if (pos.y > dest.y) { // down
                pos.y = Math.Max(dest.y, pos.y - Math.Abs(delta.y));
                this.content.localPosition = pos;
                return true;
            }
        }
        return false;
    }

    public void doScollToBegin() {
        doScrollTo(horizontal ? maxPos : minPos);
    }

    public void doScollToEnd() {
        doScrollTo(horizontal ? minPos : maxPos);
    }

    public void doScrollTo(Vector2 dest) {
        Vector3 pos = this.content.localPosition;
        if (this.horizontal) {
            pos.x = dest.x;
            this.content.localPosition = pos;
        } else if (vertical) {
            pos.y = dest.y;
            this.content.localPosition = pos;
        }
    }
    public void doAnimScrollTo(Vector2 dest)
    {
        Vector3 pos = this.content.localPosition;
        ITweenNode tween = default;
        if (this.horizontal)
        {
            print(pos);
            print(dest);
            pos.x = dest.x;
            //this.content.localPosition = pos;
            tween = this.content.transform.TweenPositionTo(0.2f, pos);

        }
        else if (vertical)
        {
            pos.y = dest.y;
            //this.content.localPosition = pos;
            tween = this.content.transform.TweenPositionTo(0.2f, pos);
        }
        TweenManager.Instance.Play(tween);
    }


    public virtual void startAdjust(Vector2 delta) {
        prepare();
        if (!startAdjustOverflow()) {
            Vector2[] v = computeInetiaVelocityByTouch(delta);
            if (v[0] != Vector2.zero) {
                if (_onInertiaBeforeStart != null) {
                    _onInertiaBeforeStart(v[0], v[1]);
                } else {
                    startAdjustInertia(v[0], v[1]);
                }
            } else {
                fireStateChange(State.Stable);
            }
        }
    }

    public virtual void stopAdjust() {
        stopAdjustOverflow();
        stopAdjustInertia();
    }

    protected virtual void adjustUpdate() {
        if (Input.GetMouseButton(0)) {
            stopAdjustInertia();
            return;
        }
        updateAdjustOverflow();
        updateAdjustInertia();
    }

    private Vector2 adjustOverflowVelocity = Vector2.zero;
    public virtual bool startAdjustOverflow() {
        Vector3 pos = this.content.localPosition;
        if (horizontal) {
            if (pos.x < minPos.x) {
                adjustOverflowVelocity.x = this.viewport.rect.width;
            } else if (pos.x > maxPos.x) {
                adjustOverflowVelocity.x = -this.viewport.rect.width;
            }
            return adjustOverflowVelocity.x != 0;
        } else if (vertical) {
            if (pos.y < minPos.y) {
                adjustOverflowVelocity.y = this.viewport.rect.height;
            } else if (pos.y > maxPos.y) {
                adjustOverflowVelocity.y = -this.viewport.rect.height;
            }
            return adjustOverflowVelocity.y != 0;
        }
        return false;
    }

    public virtual void stopAdjustOverflow() {
        adjustOverflowVelocity = Vector2.zero;
    }

    protected virtual void updateAdjustOverflow() {
        if (adjustOverflowVelocity == Vector2.zero) {
            return;
        }
        //BELog.info("ModeAScrollRect.adjustContentUpdate: " + adjustContentType + " " + Time.deltaTime);
        if (!doScroll(adjustOverflowVelocity * Time.deltaTime, false, true)) {
            stopAdjustOverflow();
            fireStateChange(State.Stable);
        }
    }

    private Vector2 inertiaVelocity = Vector2.zero;
    private Vector2 inertiaAcceleration = Vector2.zero;
    public virtual bool startAdjustInertia(Vector2 velocity, Vector2 acceleration) {
        inertiaVelocity = velocity;
        inertiaAcceleration = acceleration;
        if (inertiaVelocity == Vector2.zero) {
            fireStateChange(State.Stable);
            return false;
        }
        return true;
    }

    public virtual void stopAdjustInertia() {
        inertiaVelocity = Vector2.zero;
    }

    protected virtual void stopAdjustInertiaAtEnd() {
        fireStateChange(State.Stable);
        if (_onInertiaAfterEnd != null) {
            _onInertiaAfterEnd(inertiaVelocity, inertiaAcceleration);
        }
        stopAdjustInertia();
    }

    protected virtual void updateAdjustInertia() {
        if (inertiaVelocity == Vector2.zero) {
            return;
        }
        if (!doScroll(computeInetiaMoveDistance(inertiaVelocity, inertiaAcceleration, Time.deltaTime))) {
            stopAdjustInertiaAtEnd();
            return;
        }
        computeInetiaVelocityAfterMove(ref inertiaVelocity, inertiaAcceleration, Time.deltaTime);
        if (inertiaVelocity == Vector2.zero) {
            stopAdjustInertiaAtEnd();
        }
    }

    protected Vector2[] computeInetiaVelocityByTouch(Vector2 delta) {
        Vector2 _v = calculateTouchMoveVelocity();
        Vector2[] v = { Vector2.zero, Vector2.zero };
        if (horizontal && _v.x != 0) {
            float vsrc = _v.x, p = this.viewport.rect.width;
            if (Math.Abs(vsrc) > p * 0.1) {
                float vmin = p * 0.2f, vmax = p * 4.2f;
                float vdest = Math.Min(vmax, Math.Max(vmin, Math.Abs(vsrc)));
                v[0].x = (vsrc > 0 ? vdest : -vdest);
                v[1].x = (vsrc > 0 ? -p : p) * 0.35f;
            }
        } else if (vertical && _v.y != 0) {
            float vsrc = _v.y, p = this.viewport.rect.height;
            if (Math.Abs(vsrc) > p * 0.1) {
                float vmin = p * 0.2f, vmax = p * 4.2f;
                float vdest = Math.Min(vmax, Math.Max(vmin, Math.Abs(vsrc)));
                v[0].y = (vsrc > 0 ? vdest : -vdest);
                v[1].y = (vsrc > 0 ? -p : p) * 0.35f;
            }
        }
        RosinessLog.Log("RSScrollRect.computeInetiaVelocityByTouch " + (delta / Time.deltaTime) + " -> " + v[0]);
        return v;
    }

    protected Vector2 computeInetiaMoveDistance(Vector2 velocity, Vector2 acceleration, float dt) {
        Vector2 d = Vector2.zero;
        if (velocity.x != 0) {
            d.x = velocity.x * dt + acceleration.x * dt * dt / 2;
        } else if (velocity.y != 0) {
            d.y = velocity.y * dt + acceleration.y * dt * dt / 2;
        }
        return d;
    }

    protected void computeInetiaVelocityAfterMove(ref Vector2 velocity, Vector2 acceleration, float dt) {
        if (velocity.x != 0) {
            float v = velocity.x + acceleration.x * dt;
            velocity.x = (velocity.x < 0 && v < 0) || (velocity.x > 0 && v > 0) ? v : 0;
        } else if (velocity.y != 0) {
            float v = velocity.y + acceleration.y * dt;
            velocity.y = (velocity.y < 0 && v < 0) || (velocity.y > 0 && v > 0) ? v : 0;
        }
    }
}
