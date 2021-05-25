using DG.Tweening;
using UnityEngine;

namespace Rosiness.ActionAnimator
{
    public class ActionAnimator
    {
        public static Tweener moveTo(GameObject obj, float time, Vector3 target, bool isLocal,
            ActionCurve vkDurType = ActionCurve.AnimationCurve, ActionCallBack completeObj = null)
        {
            Tweener tw = null;
            tw = isLocal
                ? obj.transform.DOLocalMove(target, time, false).SetEase((Ease) vkDurType)
                : obj.transform.DOMove(target, time, false).SetEase((Ease) vkDurType);
            if (completeObj == null) return tw;
            tw.OnComplete(completeObj.OnCompleteMethod);
            tw.OnStart(completeObj.OnStartMethod);
            tw.OnStepComplete(completeObj.OnStepCompleteMethod);

            return tw;
        }

        public static Tweener moveFrom(GameObject obj, float time, Vector3 target, bool isLocal,
            ActionCurve vkDurType = ActionCurve.AnimationCurve, ActionCallBack completeObj = null)
        {
            Tweener tw = null;
            tw = isLocal
                ? obj.transform.DOLocalMove(target, time, false).SetEase((Ease) vkDurType).From()
                : obj.transform.DOMove(target, time, false).SetEase((Ease) vkDurType).From();
            if (completeObj == null) return tw;
            tw.OnComplete(completeObj.OnCompleteMethod);
            tw.OnStart(completeObj.OnStartMethod);
            tw.OnStepComplete(completeObj.OnStepCompleteMethod);
            return tw;
        }

        public static Tweener moveBy(GameObject obj, float time, Vector3 offset, bool isLocal,
            ActionCurve vkDurType = ActionCurve.AnimationCurve, ActionCallBack completeObj = null)
        {
            Tweener tw = null;
            tw = isLocal
                ? obj.transform.DOLocalMove(offset, time, false).SetEase((Ease) vkDurType).SetRelative(true)
                : obj.transform.DOMove(offset, time, false).SetEase((Ease) vkDurType).SetRelative(true);
            if (completeObj == null) return tw;
            tw.OnComplete(completeObj.OnCompleteMethod);
            tw.OnStart(completeObj.OnStartMethod);
            tw.OnStepComplete(completeObj.OnStepCompleteMethod);
            return tw;
        }

        public static DelayAction delayTo(float time, System.Action completeObj = null)
        {
            DelayAction skyDelayAnimation = new DelayAction();
            skyDelayAnimation.PlayTime = time;
            if (completeObj != null)
            {
                skyDelayAnimation.PlayCallBack.AddCompleteMethod(completeObj);
            }

            return skyDelayAnimation;
        }
    }
}