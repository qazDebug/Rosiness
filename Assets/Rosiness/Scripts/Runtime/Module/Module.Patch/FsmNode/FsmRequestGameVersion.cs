using System.Collections;
using System.Collections.Generic;
using Rosiness.AI;
using Rosiness.Network;

namespace Rosiness.Patch
{
    internal class FsmRequestGameVersion : IFsmNode
	{
		private readonly PatchManagerImpl _patcher;
		public string Name { private set; get; }

		public FsmRequestGameVersion(PatchManagerImpl patcher)
		{
			_patcher = patcher;
			Name = EPatchStates.RequestGameVersion.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.RequestGameVersion);
			RosinessEngine.StartCoroutine(Download());
		}
		void IFsmNode.OnUpdate()
		{
		}
		void IFsmNode.OnExit()
		{
		}
		void IFsmNode.OnHandleMessage(object msg)
		{
		}

		public IEnumerator Download()
		{
			// 获取最新的游戏版本号
			{
				string url = _patcher.GetWebServerIP();
				string post = _patcher.GetWebPostData();
				RosinessLog.Log($"Beginning to request from web : {url} {post}");
				WebPostRequest download = new WebPostRequest(url, post);
				download.DownLoad();
				yield return download;

				//Check fatal
				if (download.HasError())
				{
					download.ReportError();
					download.Dispose();
					PatchEventDispatcher.SendGameVersionRequestFailedMsg();
					yield break;
				}

				string response = download.GetResponse();
				RosinessLog.Log($"Succeed get response from web : {url} {response}");
				_patcher.ParseWebResponseData(response);
				download.Dispose();
			}

			// 检测强更安装包
			if(_patcher.ForceInstall)
			{
				string requestedGameVersion = _patcher.RequestedGameVersion.ToString();
				RosinessLog.Log($"Found new APP can be install : {requestedGameVersion}");
				PatchEventDispatcher.SendFoundForceInstallAPPMsg(requestedGameVersion, _patcher.AppURL);
				yield break;
			}

			// 检测资源版本是否变化
			int newResourceVersion = _patcher.RequestedResourceVersion;
			int oldResourceVersion = _patcher.LocalResourceVersion;
			if (newResourceVersion == oldResourceVersion)
			{
				RosinessLog.Log($"Resource version is not change.");
				//_patcher.Switch(EPatchStates.DownloadOver.ToString());
				_patcher.Switch(EPatchStates.PatchDone.ToString());
			}
			else
			{
				RosinessLog.Log($"Resource version is change : {oldResourceVersion} -> {newResourceVersion}");
				_patcher.SwitchNext();
			}
		}
	}
}