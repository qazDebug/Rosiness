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

public class RSModeAView : MonoBehaviour {
    public RectTransform content;
    public RectTransform banner;
    public RectTransform body;
    public float offsetTop = 0;
    public float offsetBottom = 0;
    public bool screenAdapted = true; // 屏幕适配，全屏模式下使用
    public bool extendViewport = true; // 将显示扩展至屏幕上部的非安全区域

    private bool _init = false;
    
    [System.NonSerialized]
    public float minPosY, maxPosY;

    void Start() {
        init();
	}

    public void init() {
        if (_init) {
            return;
        }
        if (!content) {
            content = this.transform.Find("Content") as RectTransform;
            if (!content) {
                RosinessLog.Warning("DDModeAView 未找到对象content");
            }
        }
        if (!banner) {
            banner = this.transform.Find("Banner") as RectTransform;
            if (!banner) {
                RosinessLog.Warning("DDModeAView 未找到对象banner");
            }
        }
        if (!body) {
            for (int i = this.transform.childCount; --i >= 0;) {
                Transform tf = this.transform.GetChild(i);
                if (tf.gameObject.GetComponent<RSPageScrollRect>()) {
                    body = tf as RectTransform;
                    break;
                }
            }
            if (!body) {
                RosinessLog.Warning("DDModeAView 未找到对象body");
            }
        }
        adapterScreen();
        _init = true;
    }

    void adapterScreen() {
        float bannerHeight = banner ? banner.rect.height : 0;
        float othersHeight = 0;
        banner.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, banner.rect.height * RSUI.AdapterSizeRatioForMatchHeight);
        if (body) {
            for (int i = this.content.childCount; --i >= 0; ) {
                RectTransform _rtf = this.content.GetChild(i) as RectTransform;
                if (_rtf != banner && _rtf != body) {
                    _rtf.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _rtf.rect.height * RSUI.AdapterSizeRatioForMatchHeight);
                    othersHeight += _rtf.rect.height;
                }
            }
        }
        //BELog.info("DDModeAView: bannerHeight=" + bannerHeight + " othersHeight=" + othersHeight);

        float safeAreaTop = 0;
        if (screenAdapted) {
            safeAreaTop = RSUI.RealResolution.y - RSUI.SafeArea.yMax;
        }
        
        RectTransform rtf = this.transform as RectTransform;
        rtf.anchorMin = new Vector2(0, 0);
        rtf.anchorMax = new Vector2(1, 1);
        rtf.pivot = new Vector2(0, 1);
        rtf.offsetMax = new Vector2(rtf.offsetMax.x, extendViewport ? 0 : -safeAreaTop);
        rtf.offsetMin = new Vector2(rtf.offsetMin.x, RSUI.SafeArea.yMin + offsetBottom * RSUI.AdapterSizeRatioForMatchHeight);

        content.anchorMin = new Vector2(0, 1);
        content.anchorMax = new Vector2(1, 1);
        content.pivot = new Vector2(0, 1);
        //content.offsetMax = new Vector2(content.offsetMax.x, 0);
        //content.localPosition = Vector2.zero;
        //content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rtf.rect.height + bannerHeight);
        content.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, rtf.rect.height - (extendViewport ? safeAreaTop : 0) + bannerHeight);

        body.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rtf.rect.height - (extendViewport ? safeAreaTop : 0) - othersHeight);
        RosinessLog.Log("DDModeAView: " + RSUI.GetBoundingRectToWorld(this.transform) 
            + " content:" + RSUI.GetBoundingRectToWorld(content) + " body: " + RSUI.GetBoundingRectToWorld(body));


        minPosY = content.localPosition.y;
        maxPosY = minPosY + (banner ? banner.rect.height : 0) - (extendViewport ? safeAreaTop : 0);
    }
}
