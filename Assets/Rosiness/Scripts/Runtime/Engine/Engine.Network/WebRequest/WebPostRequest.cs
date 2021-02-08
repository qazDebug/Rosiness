/****************************************************
	文件：WebPostRequest.cs
	作者：世界和平
	日期：2021/1/12 11:55:4
	功能：Nothing
*****************************************************/
using System;
using UnityEngine.Networking;

namespace Rosiness.Network
{
	public sealed class WebPostRequest : WebRequestBase
	{
		public string PostData { private set; get; }

		public WebPostRequest(string url, string post) : base(url)
		{
			PostData = post;
		}
		public override void DownLoad()
		{
			if (CacheRequest != null)
				return;

			// Check error
			if (string.IsNullOrEmpty(PostData))
				throw new Exception($"{nameof(WebPostRequest)} post content is null or empty : {URL}");

			// 下载文件
			CacheRequest = UnityWebRequest.Post(URL, PostData);
			DownloadHandlerBuffer downloadhandler = new DownloadHandlerBuffer();
			CacheRequest.downloadHandler = downloadhandler;
			CacheRequest.disposeDownloadHandlerOnDispose = true;
			CacheRequest.timeout = Timeout;
			AsyncOperationHandle = CacheRequest.SendWebRequest();
		}
		public override void ReportError()
		{
			if (CacheRequest != null)
				RosinessLog.Warning($"{nameof(WebPostRequest)}  : {URL} Error : {CacheRequest.error}");
		}

		public string GetResponse()
		{
			if (IsDone() && HasError() == false)
				return CacheRequest.downloadHandler.text;
			else
				return null;
		}
	}
}