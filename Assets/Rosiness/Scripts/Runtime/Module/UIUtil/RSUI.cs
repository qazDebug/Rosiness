using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
using Object = UnityEngine.Object;

namespace Rosiness {
    public class RSUI {
        private static Vector2 _screenResolution = Vector2.zero;
        private static Vector2 _designedResolution = Vector2.zero;
        private static Vector2 _realResolution = Vector2.zero;
        private static Vector2 _scaleForScreen = new Vector2(1, 1); // world / local
        private static float _adapterSizeRatioForMatchHeight = 1;
        private static Rect _safeArea = Rect.zero;

        public static Vector2 ScreenResolution {
            get {
                ComputeResolution();
                return _screenResolution;
            }
        }

        public static Vector2 DesignedResolution {
            get {
                ComputeResolution();
                return _designedResolution;
            }
        }

        public static Vector2 RealResolution {
            get {
                ComputeResolution();
                return _realResolution;
            }
        }

        public static Vector2 ScaleForScreen {
            get {
                ComputeResolution();
                return _scaleForScreen;
            }
        }

        public static float AdapterSizeRatioForMatchHeight {
            get {
                ComputeResolution();
                return _adapterSizeRatioForMatchHeight;
            }
        }

        public static Rect SafeArea {
            get {
                ComputeResolution();
                return _safeArea;
            }
        }

        private static void ComputeResolution(bool recompute = false) {
            if (_designedResolution == Vector2.zero || recompute) {
                CanvasScaler canvas = Object.FindObjectOfType<CanvasScaler>();
                if (!canvas) {
                    RosinessLog.Error("DDUI.computeResolution could not find Canvas");
                    return;
                }
                _designedResolution = canvas.referenceResolution;
                _realResolution = new Vector2(_designedResolution.x, Screen.height * _designedResolution.x / Screen.width);

                _scaleForScreen = new Vector2(Screen.width / RealResolution.x, Screen.height / RealResolution.y);
                //if (Screen.height / Screen.width < _designedResolution.y / _designedResolution.x) {
                if ((float)Screen.height / Screen.width < 960f / 640) {
                    _adapterSizeRatioForMatchHeight = _designedResolution.x / (_designedResolution.y * Screen.width / Screen.height);
                } else {
                    _adapterSizeRatioForMatchHeight = 1;
                }

                //Rect rect = Device.getSafeAreaInNDC();
                ////rect = new Rect(0, 44 / 812f, 1, (812 - 44 - 34) / 812f);
                //rect.y = 1 - rect.yMax;
                //_safeArea = new Rect(rect.x * _realResolution.x, rect.y * _realResolution.y, rect.width * _realResolution.x, rect.height * _realResolution.y);

                RosinessLog.Log("RSUI.computeResolution Screen: " + Screen.width + "x" + Screen.height + " scale: " + _scaleForScreen
                    + " adapterSizeRatioForMatchHeight: " + _adapterSizeRatioForMatchHeight
                    + " Canvas: " + _designedResolution + " -> " + _realResolution + " SafeArea: " + _safeArea);
            }
        }

        public static Vector2 TransScreenToWorld(Vector2 p) {
            ComputeResolution();
            return new Vector2(p.x / _scaleForScreen.x, p.y / _scaleForScreen.y);
        }

        public static Rect GetBoundingRectToWorld(GameObject gameObject, bool designed = true) {
            return GetBoundingRectToWorld(gameObject.transform);
        }

        public static Rect GetBoundingRectToWorld(Transform tf, bool designed = true) {
            RectTransform rtf = tf as RectTransform;
            //BELog.debug("DUHelper.getGameObjectWorldRect " + rtf.position);
            //BELog.debug("DUHelper.getGameObjectWorldRect " + obj.transform.parent.TransformPoint(rtf.localPosition));

            Vector3[] pos = new Vector3[4];
            rtf.GetWorldCorners(pos);
            //BELog.debug("DUHelper.getGameObjectWorldRect " + pos[0] + " " + pos[2]);
            Rect rect = new Rect(pos[0].x, pos[0].y, pos[2].x - pos[0].x, pos[2].y - pos[0].y);

            //Vector3 pMin = obj.transform.parent.TransformPoint(new Vector3(rtf.localPosition.x + rtf.rect.xMin, rtf.localPosition.y + rtf.rect.yMin, rtf.localPosition.z));
            //Vector3 pMax = obj.transform.parent.TransformPoint(new Vector3(rtf.localPosition.x + rtf.rect.xMax, rtf.localPosition.y + rtf.rect.yMax, rtf.localPosition.z));
            //BELog.debug("DUHelper.getGameObjectWorldRect " + pMin + " " + pMax);
            //rect = new Rect(pMin.x, pMin.y, pMax.x - pMin.x, pMax.y - pMin.y);

            //BELog.debug("DUHelper.getGameObjectWorldRect " + rect);
            //BELog.debug("DUHelper.getGameObjectWorldRect " + Screen.width + ", " + Screen.height);

            if (designed) {
                ComputeResolution();
                rect = new Rect(rect.x / _scaleForScreen.x, rect.y / _scaleForScreen.y, rect.width / _scaleForScreen.x, rect.height / _scaleForScreen.y);
            }
            return rect;
        }

    }
}
