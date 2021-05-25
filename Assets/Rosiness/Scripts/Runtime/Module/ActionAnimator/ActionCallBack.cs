using DG.Tweening;

namespace Rosiness.ActionAnimator
{
    public class ActionCallBack
    {
        public TweenCallback OnCompleteMethod;
        public TweenCallback OnStartMethod;
        public TweenCallback OnStepCompleteMethod;
        
        public ActionCallBack ()
        {
        }

        public void SetCompleteMethod(System.Action action)
        {
            if (action != null) OnCompleteMethod = new TweenCallback(action);
        }
        
        public void SetStartMethod (System.Action action)
        {
            if (action != null) OnStartMethod = new TweenCallback (action);
        }

        public void SetStepCompleteMethod(System.Action action)
        {
            if (action != null) OnStepCompleteMethod = new TweenCallback(action);
        }
        
        public void AddCompleteMethod (System.Action action)
        {
            if (action != null) OnCompleteMethod += new TweenCallback (action);
        }

        public void AddStartMethod (System.Action action)
        {
            if (action != null) OnStartMethod += new TweenCallback (action);
        }

        public void AddStepCompleteMethod (System.Action action)
        {
            if (action != null) OnStepCompleteMethod += new TweenCallback (action);
        }
    }
}