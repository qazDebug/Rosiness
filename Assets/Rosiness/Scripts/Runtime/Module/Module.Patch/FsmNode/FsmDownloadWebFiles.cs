using System.Collections;
using Rosiness.AI;

namespace Rosiness.Patch
{
    internal class FsmDownloadWebFiles : IFsmNode
	{
		private readonly PatchManagerImpl _patcher;
		public string Name { private set; get; }

		public FsmDownloadWebFiles(PatchManagerImpl patcher)
		{
			_patcher = patcher;
			Name = EPatchStates.DownloadWebFiles.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.DownloadWebFiles);
			RosinessEngine.StartCoroutine(Download());
		}
		void IFsmNode.OnUpdate()
		{
			if (_patcher.InternalDownloader != null)
				_patcher.InternalDownloader.Update();
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}

		private IEnumerator Download()
		{
			var downloader = _patcher.InternalDownloader;

			// 注册下载回调
			downloader.OnPatchFileCheckFailedCallback = PatchEventDispatcher.SendWebFileCheckFailedMsg;
			downloader.OnPatchFileDownloadFailedCallback = PatchEventDispatcher.SendWebFileDownloadFailedMsg;
			downloader.OnPatchFileDownloadSucceedCallback = PatchEventDispatcher.SendDownloadFilesProgressMsg;
			downloader.Download();
			yield return downloader;

			// 检测下载结果
			if (downloader.DownloadStates != EDownloaderStates.Succeed)
				yield break;

			// 缓存本地资源版本号
			_patcher.CacheLocalResourceVersion();
			_patcher.SwitchNext();
		}
	}
}