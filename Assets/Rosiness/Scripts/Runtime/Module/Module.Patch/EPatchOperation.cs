
namespace Rosiness.Patch
{
	/// <summary>
	/// 用户层反馈的操作方式
	/// </summary>
	public enum EPatchOperation
	{
		BeginingDownloadWebFiles,
		TryRequestGameVersion,
		TryDownloadWebPatchManifest,
		TryDownloadWebFiles,
	}
}