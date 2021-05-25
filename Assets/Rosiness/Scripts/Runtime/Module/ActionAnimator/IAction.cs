namespace Rosiness.ActionAnimator
{
    public interface IAction
    {
        /// <summary>
        /// 是否 playing
        /// </summary>
        bool IsPlaying { get; set; }

        /// <summary>
        /// 是否 loop
        /// </summary>
        bool Loop { get; set; }

        /// <summary>
        /// 是否 自动运行
        /// </summary>
        bool AutoRun { get; set; }

        /// <summary>
        /// 是否 自动运行下一个
        /// </summary>
        bool AutoPlayNextAction { get; set; }

        /// <summary>
        /// 播放时间
        /// </summary>
        float PlayTime { get; set; }

        /// <summary>
        /// 延迟时间
        /// </summary>
        float DelayTime { get; set; }

        /// <summary>
        /// 自动运行延迟时间
        /// </summary>
        float AutoStartDelayTime { get; set; }
        
        /// <summary>
        /// 动作曲线
        /// </summary>
        ActionCurve ActionCurveEase { get; set; }
        
        ActionCallBack DelayCallBack { get; set; }
        
        ActionCallBack PlayCallBack { get; set; }

        BaseActionSequence  ParentAction { get; set; }
        
        /// <summary>
        /// 初始化
        /// </summary>
        void Init ();

        /// <summary>
        /// 循环播放
        /// </summary>
        void PlayLoop ();

        /// <summary>
        /// 播放
        /// </summary>
        void Play ();

        /// <summary>
        /// 延迟播放
        /// </summary>
        void Delay ();

        /// <summary>
        /// 播放下一个
        /// </summary>
        void PlayNext ();
        
        void RemoveFromParent ();

        void Kill();
    }
}