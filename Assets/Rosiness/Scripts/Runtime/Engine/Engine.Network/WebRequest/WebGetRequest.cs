/****************************************************
	文件：WebGetRequest.cs
	作者：世界和平
	日期：2021/1/12 11:54:57
	功能：Nothing
*****************************************************/
using UnityEngine.Networking;

namespace Rosiness.Network
{
	public sealed class WebGetRequest : WebRequestBase
	{
		public WebGetRequest(string url) : base(url)
		{
		}
		public override void DownLoad()
		{
			if (CacheRequest != null)
				return;

			// 下载文件
			CacheRequest = new UnityWebRequest(URL, UnityWebRequest.kHttpVerbGET);
			DownloadHandlerBuffer handler = new DownloadHandlerBuffer();
			CacheRequest.downloadHandler = handler;
			CacheRequest.disposeDownloadHandlerOnDispose = true;
			CacheRequest.timeout = Timeout;
			AsyncOperationHandle = CacheRequest.SendWebRequest();
		}
		public override void ReportError()
		{
			if (CacheRequest != null)
				RosinessLog.Warning($"{nameof(WebGetRequest)} : {URL} Error : {CacheRequest.error}");
		}

		public byte[] GetData()
		{
			if (IsDone() && HasError() == false)
				return CacheRequest.downloadHandler.data;
			else
				return null;
		}
		public string GetText()
		{
			if (IsDone() && HasError() == false)
				return CacheRequest.downloadHandler.text;
			else
				return null;
		}
	}
}