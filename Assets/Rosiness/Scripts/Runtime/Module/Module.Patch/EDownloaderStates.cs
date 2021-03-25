/****************************************************
	文件：EDownloaderStates.cs
	作者：世界和平
	日期：2021/3/19 18:59:25
	功能：Nothing
*****************************************************/

namespace Rosiness.Patch
{
    /// <summary>
    /// 下载状态
    /// </summary>
    public enum EDownloaderStates
    {
        None,
        Loading,
        Failed,
        Succeed,
        Forbid,
    }
}