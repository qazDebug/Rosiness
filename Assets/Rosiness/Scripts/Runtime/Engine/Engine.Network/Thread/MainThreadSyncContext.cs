/****************************************************
	文件：MainThreadSyncContext.cs
	作者：世界和平
	日期：2021/1/12 11:53:16
	功能：Nothing
*****************************************************/
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Rosiness.Network
{
    /// <summary>
    /// 同步其它线程里的回调到主线程里
    /// 注意：Unity3D中需要设置Scripting Runtime Version为.NET4.6
    /// </summary>
    internal sealed class MainThreadSyncContext : SynchronizationContext
    {
		/// <summary>
		/// 同步队列
		/// </summary>
		private readonly ConcurrentQueue<Action> _safeQueue = new ConcurrentQueue<Action>();

		public void Update()
		{
			while (true)
			{
				if (_safeQueue.TryDequeue(out Action action) == false)
					return;
				action.Invoke();
			}
		}

		public override void Post(SendOrPostCallback callback, object state)
		{
			Action action = new Action(() => { callback(state); });
			_safeQueue.Enqueue(action);
		}
	}
}