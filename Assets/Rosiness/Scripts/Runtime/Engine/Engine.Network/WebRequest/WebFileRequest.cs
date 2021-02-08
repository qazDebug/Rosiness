/****************************************************
	文件：WebFileRequest.cs
	作者：世界和平
	日期：2021/1/12 11:54:36
	功能：Nothing
*****************************************************/
using UnityEngine.Networking;

namespace Rosiness.Network
{
    public sealed class WebFileRequest : WebRequestBase
    {
		/// <summary>
		/// 文件存储路径
		/// </summary>
		public string SavePath { private set; get; }

		public WebFileRequest(string url, string savePath) : base(url)
		{
			SavePath = savePath;
		}
		public override void DownLoad()
		{
			if (CacheRequest != null)
				return;

			// 下载文件
			CacheRequest = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);
			DownloadHandlerFile handler = new DownloadHandlerFile(SavePath);
			handler.removeFileOnAbort = true;
			CacheRequest.downloadHandler = handler;
			CacheRequest.disposeDownloadHandlerOnDispose = true;
			CacheRequest.timeout = Timeout;
			AsyncOperationHandle = CacheRequest.SendWebRequest();
		}
		public override void ReportError()
		{
			if (CacheRequest != null)
				RosinessLog.Warning($"{nameof(WebFileRequest)} : {URL} Error : {CacheRequest.error}");
		}
	}
}