using System.Collections;
using System.Collections.Generic;
using Rosiness.AI;
using Rosiness.Network;

namespace Rosiness.Patch
{
    internal class FsmGetWebPatchManifest : IFsmNode
	{
		private readonly PatchManagerImpl _patcher;
		public string Name { private set; get; }

		public FsmGetWebPatchManifest(PatchManagerImpl patcher)
		{
			_patcher = patcher;
			Name = EPatchStates.GetWebPatchManifest.ToString();
		}
		void IFsmNode.OnEnter()
		{
			PatchEventDispatcher.SendPatchStepsChangeMsg(EPatchStates.GetWebPatchManifest);
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

		private IEnumerator Download()
		{
			// 从远端下载最新的补丁清单
			int newResourceVersion = _patcher.RequestedResourceVersion;
			string url = _patcher.GetWebDownloadURL(newResourceVersion.ToString(), PatchDefine.PatchManifestFileName);
			WebGetRequest download = new WebGetRequest(url);
			download.DownLoad();
			yield return download;

			// Check fatal
			if (download.HasError())
			{
				download.ReportError();
				download.Dispose();
				PatchEventDispatcher.SendWebPatchManifestDownloadFailedMsg();
				yield break;
			}
			
			// 保存补丁清单到本地
			_patcher.SaveRemotePatchManifest(download.GetText());
			download.Dispose();
			_patcher.SwitchNext();
		}
	}
}