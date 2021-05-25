namespace Rosiness.ActionAnimator
{
    public class DelayAction : BaseActionNormal
    {
        public DelayAction()
        {
            this.Init();
        }
        
        public DelayAction(float delayTime,System.Action startAction,System.Action callback){
            this.Init ();
            this.PlayTime = delayTime;
            this.PlayCallBack.AddCompleteMethod (callback);
            this.PlayCallBack.AddStartMethod (startAction);
        }
    }
}