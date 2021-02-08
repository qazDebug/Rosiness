/****************************************************
	文件：Rosiness.cs
	作者：世界和平
	日期：2020/11/27 11:14:0
	功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness
{
    public static class RosinessEngine
    {
        private class ModuleWrapper
        {
            public int Priority { private set; get; }
			public IModule Module { private set; get; }

			public ModuleWrapper(IModule module, int priority)
			{
				Module = module;
				Priority = priority;
			}
		}

		private static readonly List<ModuleWrapper> _moduleWrappers = new List<ModuleWrapper>(100);
		private static MonoBehaviour _behaviour;
		private static bool _isDirty = false;

		/// <summary>
		/// 初始化框架
		/// </summary>
		public static void Initialize(MonoBehaviour behaviour, Action<ELogLevel,string> logCallback)
        {
			if (behaviour == null)
				throw new Exception("Rosiness behaviour is null.");
			if (_behaviour != null)
				throw new Exception($"{nameof(RosinessEngine)} is already initialized.");

			UnityEngine.Object.DontDestroyOnLoad(behaviour.gameObject);
			_behaviour = behaviour;

			// 注册日志回调
			if (logCallback != null)
				RosinessLog.RegisterCallback(logCallback);
		}

		/// <summary>
		/// 更新框架
		/// </summary>
		public static void Update()
		{
			// 如果有新模块需要重新排序
			if (_isDirty)
			{
				_isDirty = false;
				_moduleWrappers.Sort((left, right) =>
				{
					if (left.Priority > right.Priority)
						return -1;
					else if (left.Priority == right.Priority)
						return 0;
					else
						return 1;
				});
			}

			// 轮询所有模块
			for (int i = 0; i < _moduleWrappers.Count; i++)
			{
				_moduleWrappers[i].Module.OnUpdate();
			}
		}

		/// <summary>
		/// 绘制所有模块的GUI内容
		/// </summary>
		internal static void DrawModulesGUIContent()
		{
			for (int i = 0; i < _moduleWrappers.Count; i++)
			{
				_moduleWrappers[i].Module.OnGUI();
			}
		}

		/// <summary>
		/// 查询游戏模块是否存在
		/// </summary>
		public static bool Contains(Type moduleType)
		{
			for (int i = 0; i < _moduleWrappers.Count; i++)
			{
				if (_moduleWrappers[i].Module.GetType() == moduleType)
					return true;
			}
			return false;
		}

		/// <summary>
		/// 创建游戏模块
		/// </summary>
		/// <typeparam name="T">模块类</typeparam>
		/// <param name="priority">运行时的优先级，优先级越大越早执行。如果没有设置优先级，那么会按照添加顺序执行</param>
		public static T CreateModule<T>(int priority = 0) where T:class, IModule
        {
			return CreateModule<T>(null, priority);
        }

		/// <summary>
		/// 创建游戏模块
		/// </summary>
		/// <typeparam name="T">模块类</typeparam>
		/// <param name="createParam">创建参数</param>
		/// <param name="priority">运行时的优先级，优先级越大越早执行。如果没有设置优先级，那么会按照添加顺序执行</param>
		private static T CreateModule<T>(object p, int priority = 0) where T : class, IModule
        {
            if(priority < 0)            
				throw new Exception("The priority can not be negative");
			if (Contains(typeof(T)))
				throw new Exception($"Game module {typeof(T)} is already existed");

			if(priority == 0)
            {
				int minPriority = GetMinPriority();
				priority = --minPriority;
			}

			RosinessLog.Log($"Create game module : {typeof(T)}");
			T module = Activator.CreateInstance<T>();
			ModuleWrapper wrapper = new ModuleWrapper(module, priority);
			wrapper.Module.OnCreate(p);
			_moduleWrappers.Add(wrapper);
			_isDirty = true;
			return module;
		}

		/// <summary>
		/// 获取游戏模块
		/// </summary>
		/// <typeparam name="T">模块类</typeparam>
		public static T GetModule<T>() where T : class, IModule
		{
			System.Type type = typeof(T);
			for (int i = 0; i < _moduleWrappers.Count; i++)
			{
				if (_moduleWrappers[i].Module.GetType() == type)
					return _moduleWrappers[i].Module as T;
			}

			RosinessLog.Warning($"Not found game module {type}");
			return null;
		}

		/// <summary>
		/// 获取当前模块里最小的优先级
		/// </summary>
		private static int GetMinPriority()
        {
			int minPriority = 0;
			for (int i = 0; i < _moduleWrappers.Count; i++)
			{
				if (_moduleWrappers[i].Priority < minPriority)
					minPriority = _moduleWrappers[i].Priority;
			}
			return minPriority; //小于等于零
		}

		#region 协程相关
		/// <summary>
		/// 开启一个协程
		/// </summary>
		public static Coroutine StartCoroutine(IEnumerator coroutine)
		{
			if (_behaviour == null)
				throw new Exception($"{nameof(RosinessEngine)} is not initialize. Use MotionEngine.Initialize");
			return _behaviour.StartCoroutine(coroutine);
		}

		/// <summary>
		/// 停止一个协程
		/// </summary>
		/// <param name="coroutine"></param>
		public static void StopCoroutine(Coroutine coroutine)
		{
			if (_behaviour == null)
				throw new Exception($"{nameof(RosinessEngine)} is not initialize. Use MotionEngine.Initialize");
			_behaviour.StopCoroutine(coroutine);
		}

		/// <summary>
		/// 停止所有协程
		/// </summary>
		public static void StopAllCoroutines()
		{
			if (_behaviour == null)
				throw new Exception($"{nameof(RosinessEngine)} is not initialize. Use MotionEngine.Initialize");
			_behaviour.StopAllCoroutines();
		}
		#endregion
	}
}