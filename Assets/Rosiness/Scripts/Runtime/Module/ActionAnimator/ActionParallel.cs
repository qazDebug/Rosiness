namespace Rosiness.ActionAnimator
{
    public class ActionParallel: BaseActionSequence
    {
        public override void AppendAction(IAction skyAction)
        {
            AnimationSequence.Add(skyAction);
            setAction(skyAction);
        }

        public override void AddHead(IAction skyAction)
        {
            AnimationSequence.Insert(0,skyAction);
            setAction(skyAction);
        }

        private void setAction(IAction skyAction)
        {
            skyAction.ParentAction = this;
            if (!(skyAction.PlayTime > PlayTime)) return;
            PlayTime = skyAction.PlayTime;
            ParentAction?.ReComputePlaytime ();
        }
    }
}