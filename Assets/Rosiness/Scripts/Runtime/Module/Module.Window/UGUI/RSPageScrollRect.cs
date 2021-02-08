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

public class RSPageScrollRect : RSScrollRect {
    protected int pageCount = 9;
    protected float pageWidth = 0;
    protected float pageHeight = 0;

    void Start() {
        base.Start();
	}

    protected override void prepare() {
        base.prepare();

        if (horizontal) {
            pageWidth = this.viewport.rect.width;
            pageCount = this.content.rect.width == 0 ? 1 : (int)Math.Ceiling(this.content.rect.width / pageWidth);
        } else {
            pageHeight = this.viewport.rect.height;
            pageCount = this.content.rect.height == 0 ? 1 : (int)Math.Ceiling(this.content.rect.height / pageHeight);
        }
        //BELog.warn("RSPageScrollRect.prepare pageCount=" + pageCount);
    }

    public int getPageIndex() {
        if (pageCount > 1) {
            Vector2 pos = this.content.localPosition;
            if (this.horizontal) {
                return Math.Min(pageCount, (int)((maxPos.x - pos.x) / pageWidth));
            } else if (this.vertical) {
                return Math.Min(pageCount, (int)((pos.y - minPos.y) / pageHeight));
            }
        }
        return 0;
    }

    public void doScrollToPage(int pageIndex) {
        if (pageIndex >= pageCount) {
            RosinessLog.Warning("RSPageScrollRect.doScrollToPage 页码错误! pageIndex= " + pageIndex + " pageCount=" + pageCount);
        }
        prepare();
        Vector2 dest = this.content.localPosition;
        if (this.horizontal) {
            dest.x = maxPos.x - pageIndex * pageWidth;
        } else if (this.vertical) {
            dest.y = minPos.y + pageIndex * pageHeight;
        }
        this.doScrollTo(dest);
    }

    public void doScrollAnimToPage(int pageIndex)
    {
        if (pageIndex >= pageCount)
        {
            RosinessLog.Warning("RSPageScrollRect.doScrollToPage 页码错误! pageIndex= " + pageIndex + " pageCount=" + pageCount);
        }
        prepare();
        Vector2 dest = this.content.localPosition;
        if (this.horizontal)
        {
            dest.x = maxPos.x - pageIndex * pageWidth;
        }
        else if (this.vertical)
        {
            dest.y = minPos.y + pageIndex * pageHeight;
        }
        this.doAnimScrollTo(dest);
    }

    private Vector2 destPosition = Vector2.zero;
    private Vector2 adjustVelocity = Vector2.zero;

    public override void startAdjust(Vector2 delta) {
        prepare();
        Vector3 pos = this.content.localPosition;
        if (horizontal) {
            if (pos.x < minPos.x) {
                destPosition.x = minPos.x;
            } else if (pos.x > maxPos.x) {
                destPosition.x = maxPos.x;
            } else {
                destPosition.x = pos.x;
                if (delta.x < -1) {
                    destPosition.x = Math.Max(minPos.x, destPosition.x - pageWidth * 0.7f);
                } else if (delta.x > 1) {
                    destPosition.x = Math.Min(maxPos.x, destPosition.x + pageWidth * 0.7f);
                }
                destPosition.x = (float)Math.Round(destPosition.x / pageWidth, 0) * pageWidth;
            }
            if (destPosition.x != pos.x) {
                adjustVelocity.x = this.viewport.rect.width / 0.4f;
            } else {
                fireStateChange(State.Stable);
            }
        }
        RosinessLog.Log("RSPageScrollRect startAdjust: page count=" + pageCount + " " + pos + " -> " + destPosition);
    }

    public override void stopAdjust() {
        destPosition = Vector2.zero;
        adjustVelocity = Vector2.zero;
    }

    protected override void adjustUpdate() {
        if (Input.GetMouseButton(0)) {
            return;
        }

        if (adjustVelocity == Vector2.zero) {
            return;
        }
        //BELog.info("PageScrollRect.adjustUpdate: " + this.horizontalNormalizedPosition + ", " + destPosition);
        if (!this.doScroll(adjustVelocity * Time.deltaTime, destPosition)) {
            stopAdjust();
            fireStateChange(State.Stable);
        }
    }
}
