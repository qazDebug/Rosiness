/****************************************************
	文件：ELoaderStates.cs
	作者：世界和平
	日期：2020/12/4 14:34:27
	功能：Nothing
*****************************************************/

namespace Rosiness.Resource
{
	/// <summary>
	/// 文件加载状态
	/// </summary>
	internal enum ELoaderStates
	{
		None = 0,
		Download,
		CheckDownload,
		CheckDepends,
		LoadFile,
		CheckFile,
		Success,
		Fail,
	}
}