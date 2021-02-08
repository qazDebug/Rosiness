
namespace Rosiness.Tween
{
	public class TweenGroup
	{
		/// <summary>
		/// 播放一个补间动画
		/// </summary>
		public long Play(ITweenNode tweenRoot, UnityEngine.Object safeObject = null)
		{
			int groupID = this.GetHashCode();
			return TweenManager.Instance.Play(tweenRoot, safeObject, groupID);
		}

		/// <summary>
		/// 播放一个补间动画
		/// </summary>
		public long Play(ITweenChain tweenChain, UnityEngine.Object safeObject = null)
		{
			int groupID = this.GetHashCode();
			return TweenManager.Instance.Play(tweenChain, safeObject, groupID);
		}

		/// <summary>
		/// 关闭组内所有的补间动画
		/// </summary>
		public void KillAll()
		{
			int groupID = this.GetHashCode();
			TweenManager.Instance.Kill(groupID);
		}
	}
}