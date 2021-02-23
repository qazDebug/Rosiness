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

public class RSScreenAdapter : MonoBehaviour {
    public enum AlignMode {
        Top,
        Bottom,
        Left,
        Right
    }
    public bool safeArea = true;
    public float offset = 0;
    public AlignMode alignMode = AlignMode.Top;
    public bool scaleToFit = true;

	void Start () {
        RectTransform rtf = this.transform as RectTransform;
        if (scaleToFit) {
            CanvasScaler canvas = this.GetComponentInParent<CanvasScaler>();
            if (canvas) {
                if (canvas.matchWidthOrHeight == 0) {
                    rtf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rtf.rect.height * RSUI.adapterSizeRatioForMatchHeight);
                }
            }
        }

        Rect bound = RSUI.getBoundingRectToWorld(rtf);
        Vector2 pos = rtf.localPosition;
        switch (alignMode) {
            case AlignMode.Top:
                float top = RSUI.realResolution.y;
                if (safeArea) {
                    top = RSUI.safeArea.yMax;
                }
                pos.y += top - offset - bound.yMax;
                break;
            case AlignMode.Bottom:
                float bottom = 0;
                if (safeArea) {
                    bottom = RSUI.safeArea.yMin;
                }
                pos.y += bottom - offset - bound.yMin;
                break;
        }
        //BELog.debug("DDScreenAdapter " + rtf.rect + " " + bound);
        rtf.localPosition = pos;
	}

}
