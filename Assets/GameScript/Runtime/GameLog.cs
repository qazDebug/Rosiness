/****************************************************
	文件：GameLog.cs
	作者：世界和平
	日期：2021/3/5 11:7:11
	功能：Nothing
*****************************************************/
using UnityEngine;

public static class GameLog
{
	public static void Log(string content)
	{
		Debug.Log("[Game] " + content);
	}

	public static void Warning(string content)
	{
		Debug.LogWarning("[Game] " + content);
	}

	public static void Error(string content)
	{
		Debug.LogError("[Game] " + content);
	}
}