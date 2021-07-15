using DG.Tweening;
using UnityEngine;

namespace Rosiness.ActionAnimator
{
    public class BaseActionNormal : IAction
    {
        public BaseActionNormal()
        {
            this.Init();
        }

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

        public virtual void Init()
        {
            IsPlaying = false;
            AutoPlayNextAction = true;
            ActionCurveEase = ActionCurve.Linear;
            ParentAction = null;
            DelayCallBack = new ActionCallBack();
            DelayCallBack.AddCompleteMethod(PlayLoop);
            PlayCallBack = new ActionCallBack();
            PlayCallBack.AddCompleteMethod(() =>
            {
                PlayCallBack?.OnStepCompleteMethod();
                if (AutoPlayNextAction) PlayNext();
                if (Loop) Delay();
                else IsPlaying = false;
            });
        }

        public virtual void PlayLoop()
        {
            PlayCallBack.OnStartMethod?.Invoke();        
            DelayTimeAction(PlayTime, PlayCallBack);
        }

        public void Play()
        {
            IsPlaying = true;
            if (DelayTime > 0) {
                Delay ();
            } else {
                PlayLoop ();
            }
        }

        public virtual void Delay()
        {
            DelayTimeAction (DelayTime, DelayCallBack);
        }

        public virtual void PlayNext()
        {
            ParentAction.PlayNext(this);
        }

        public virtual void RemoveFromParent()
        {
            ParentAction?.RemoveAction (this);
        }

        public virtual void Kill()
        {
            _tw?.Kill();
        }

        private float _time;
        private Tweener _tw;

        public void DelayTimeAction(float delayTime, ActionCallBack skyAniCallBack)
        {
            _tw = null;
            _tw = RunDelayTime(delayTime, delayTime);
            _tw.OnComplete(skyAniCallBack.OnCompleteMethod);
        }

        private Tweener RunDelayTime(float delayTime, float f)
        {
            _time = 0;
            return DOTween.To(() => _time, (float x) => { _time = x; }, delayTime, f).SetTarget(this)
                .SetEase(Ease.Linear);
        }
        
        public float GetLeftTime(){
            return PlayTime - _time;
        }

        public virtual void Stop()
        {
            _tw?.Complete ();
        }
        public virtual void Pause()
        {
            _tw?.Pause ();
        }
        public virtual void Resume()
        {
            _tw?.Play();
        }
    }
}