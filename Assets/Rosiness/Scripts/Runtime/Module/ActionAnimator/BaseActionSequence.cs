using System.Collections.Generic;

namespace Rosiness.ActionAnimator
{
    public class BaseActionSequence : BaseActionNormal
    {
        public List<IAction> AnimationSequence;

        public override void Init()
        {
            base.Init();
            AnimationSequence = new List<IAction>();
            PlayTime = 0;
        }

        public BaseActionSequence()
        {
            this.Init();
        }

        public virtual void AppendAction(IAction skyAction)
        {
        }

        public virtual void Insert(IAction skyAction, int index)
        {
        }
        
        public virtual void RemoveAction (IAction skyAction)
        {
        }

        public void RemoveAll()
        {
            foreach (var skyAction in AnimationSequence)
            {
                skyAction.ParentAction = null;
            }
            AnimationSequence.Clear();
            PlayTime = 0;
            ParentAction?.ReComputePlaytime();
        }
        
        public  virtual void PlayNext (IAction skyAction)
        {
        }

        public virtual void ReComputePlaytime ()
        {
        }

        public virtual  void AddHead (IAction skyAction)
        {
        }
    }
}