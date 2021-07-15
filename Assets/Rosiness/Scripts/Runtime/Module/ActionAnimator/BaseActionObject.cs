using System.Collections;
using UnityEngine;

namespace Rosiness.ActionAnimator
{
    public sealed class BaseActionObject : MonoBehaviour, IAction
    {
        public bool IsPlaying { get; set; }
        public bool Loop { get; set; }
        public bool AutoRun { get; set; }
        public bool AutoPlayNextAction { get; set; }
        public float PlayTime { get; set; }
        public float DelayTime { get; set; }
        public float AutoStartDelayTime { get; set; }
        public ActionCurve ActionCurveEase { get; set; }
        public ActionCallBack DelayCallBack { get; set; }
        public ActionCallBack PlayCallBack { get; set; }
        public BaseActionSequence ParentAction { get; set; }

        void Start()
        {
            Init();
            if (AutoRun)
            {
                StartCoroutine(DelayTimeAction(AutoStartDelayTime, PlayLoop));
            }
        }

        public void Init()
        {
            IsPlaying = false;
            ParentAction = null;
            DelayCallBack = new ActionCallBack();
            DelayCallBack.SetCompleteMethod(PlayLoop);
            PlayCallBack = new ActionCallBack();
            PlayCallBack.SetCompleteMethod(() =>
            {
                PlayCallBack.OnStepCompleteMethod?.Invoke();
                PlayNext();
                if (Loop)
                    Delay();
                else
                    IsPlaying = false;
            });
        }

        public void PlayLoop()
        {
            PlayCallBack.OnStartMethod?.Invoke();
        }

        public void Play()
        {
            IsPlaying = true;
            if (DelayTime > 0)
            {
                Delay();
            }
            else
            {
                PlayLoop();
            }
        }

        public void Delay()
        {
            DelayCallBack.OnStartMethod?.Invoke();
            if (DelayTime > 0)
            {
                StartCoroutine(DelayTimeAction(DelayTime, () => { DelayCallBack.OnCompleteMethod(); }));
            }
            else
            {
                DelayCallBack.OnCompleteMethod();
            }
        }

        public void PlayNext()
        {
            ParentAction?.PlayNext(this);
        }

        public void RemoveFromParent()
        {
            ParentAction?.RemoveAction(this);
        }

        public void Kill()
        {
        }

        private IEnumerator DelayTimeAction(float delayTime, System.Action a)
        {
            yield return new WaitForSeconds(delayTime);
            a();
        }
    }
}