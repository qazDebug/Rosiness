namespace Rosiness.ActionAnimator
{
    public class ActionSequence : BaseActionSequence
    {
        private IAction currentAction;

        public override void AppendAction(IAction skyAction)
        {
            AnimationSequence.Add(skyAction);
            setAction(skyAction);
        }

        public override void AddHead(IAction skyAction)
        {
            AnimationSequence.Insert(0, skyAction);
            setAction(skyAction);
        }

        public override void Insert(IAction skyAction, int index)
        {
            AnimationSequence.Insert(index, skyAction);
            setAction(skyAction);
        }


        public override void Kill()
        {
            currentAction?.Kill();
            base.Kill();
            RemoveAll();
        }

        public void AddAfterCurrent(IAction skyAction)
        {
            int index = 0;
            if (currentAction != null)
            {
                index = AnimationSequence.IndexOf(currentAction) + 1;
            }

            AnimationSequence.Insert(index, skyAction);
            setAction(skyAction);
        }

        private void setAction(IAction skyAction)
        {
            skyAction.ParentAction = this;
            PlayTime += skyAction.PlayTime;
            ParentAction?.ReComputePlaytime();
        }

        public override void RemoveAction(IAction skyAction)
        {
            AnimationSequence.Remove(skyAction);
            skyAction.ParentAction = null;
            PlayTime -= skyAction.PlayTime;
            ParentAction?.ReComputePlaytime();
        }

        public override void PlayLoop()
        {
            base.PlayLoop();
            if (AnimationSequence.Count > 0)
            {
                currentAction = AnimationSequence[0];
                currentAction.Play();
            }
            else
            {
                PlayCallBack.OnCompleteMethod();
            }
        }

        public void PlayNextChildAction()
        {
            if (currentAction != null)
            {
                PlayNext(currentAction);
            }
        }

        public override void PlayNext(IAction skyAction)
        {
            if (AnimationSequence.Contains(skyAction))
            {
                int index = AnimationSequence.IndexOf(skyAction);
                if (index < AnimationSequence.Count - 1)
                {
                    currentAction = AnimationSequence[index + 1];
                    currentAction.Play();
                }
                else
                {
                    PlayCallBack.OnCompleteMethod();
                }
            }
        }

        public override void ReComputePlaytime()
        {
            PlayTime = 0;
            foreach (IAction skyAction in AnimationSequence)
            {
                PlayTime += skyAction.PlayTime;
            }
        }
    }
}