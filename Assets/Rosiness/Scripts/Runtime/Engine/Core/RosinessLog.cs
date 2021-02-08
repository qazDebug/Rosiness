/****************************************************
	文件：RoseLog.cs
	作者：世界和平
	日期：2020/11/27 10:20:58
	功能：Nothing
*****************************************************/
using System;
using System.Diagnostics;

namespace Rosiness
{
    public static class RosinessLog
    {
        private static Action<ELogLevel, string> _action;

        /// <summary> 监听日志 </summary>
        public static void RegisterCallback(Action<ELogLevel, string> action) => _action += action;

        /// <summary> 日志 </summary>
        [Conditional("DEBUG")]
        public static void Log(string info) => _action?.Invoke(ELogLevel.Log, info);

        /// <summary> 警告 </summary>
        public static void Warning(string info) => _action?.Invoke(ELogLevel.Warning, info);

        /// <summary> 错误 </summary>
        public static void Error(string info) => _action?.Invoke(ELogLevel.Error, info);

        /// <summary> 异常 </summary>
        public static void Exception(string info) => _action?.Invoke(ELogLevel.Exception, info);
    }
}