namespace Rosiness.Tween
{
	public static partial class TweenChainExtension
	{
		/// <summary>
		/// 添加节点
		/// </summary>
		public static ITweenChain Append(this ITweenChain chain, ITweenNode node)
		{
			return chain.Append(node);
		}

		/// <summary>
		/// 条件等待节点
		/// </summary>
		public static ITweenChain Until(this ITweenChain chain, System.Func<bool> condition)
		{
			return chain.Append(UntilNode.Allocate(condition));
		}

		/// <summary>
		/// 执行节点
		/// </summary>
		public static ITweenChain Execute(this ITweenChain chain, System.Action execute)
		{
			return chain.Append(ExecuteNode.Allocate(execute));
		}


		/// <summary>
		/// 延迟计时节点
		/// </summary>
		public static ITweenChain Delay(this ITweenChain chain, float delay, System.Action triggerCallback = null)
		{
			return chain.Append(TimerNode.AllocateDelay(delay, triggerCallback));
		}

		/// <summary>
		/// 重复计时节点
		/// 注意：该节点为无限时长
		/// </summary>
		public static ITweenChain Repeat(this ITweenChain chain, float delay, float interval, System.Action triggerCallback = null)
		{
			return chain.Append(TimerNode.AllocateRepeat(delay, interval, triggerCallback));
		}

		/// <summary>
		/// 重复计时节点
		/// </summary>
		public static ITweenChain Repeat(this ITweenChain chain, float delay, float interval, float duration, System.Action triggerCallback = null)
		{
			return chain.Append(TimerNode.AllocateRepeat(delay, interval, duration, triggerCallback));
		}

		/// <summary>
		/// 重复计时节点
		/// </summary>
		public static ITweenChain Repeat(this ITweenChain chain, float delay, float interval, long maxTriggerCount, System.Action triggerCallback = null)
		{
			return chain.Append(TimerNode.AllocateRepeat(delay, interval, maxTriggerCount, triggerCallback));
		}

		/// <summary>
		/// 持续计时节点
		/// </summary>
		public static ITweenChain Duration(this ITweenChain chain, float delay, float duration, System.Action triggerCallback = null)
		{
			return chain.Append(TimerNode.AllocateDuration(delay, duration, triggerCallback));
		}
	}
}