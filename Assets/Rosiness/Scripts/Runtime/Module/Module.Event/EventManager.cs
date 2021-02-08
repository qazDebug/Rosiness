/****************************************************
	文件：EventManager.cs
	作者：世界和平
	日期：2020/12/1 15:31:37
	功能：事件管理器
*****************************************************/
using Rosiness.Console;
using Rosiness.Reference;
using System;
using System.Collections.Generic;

namespace Rosiness.Event
{
    /// <summary>
	/// 事件管理器
	/// </summary>
    public sealed class EventManager : ModuleSingleton<EventManager>, IModule
    {
        private readonly Dictionary<int, List<Action<IEventMessage>>> _listeners = new Dictionary<int, List<Action<IEventMessage>>>();

        void IModule.OnCreate(object createParam)
        {
        }
        void IModule.OnUpdate()
        {
        }
        void IModule.OnGUI()
        {
            ConsoleGUI.Lable($"[{nameof(EventManager)}] Listener total count : {GetAllListenerCount()}");
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
        {
            AddListener(typeof(TEvent), listener);
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        public void AddListener(System.Type eventType, System.Action<IEventMessage> listener)
        {
            int eventId = eventType.GetHashCode();
            if (_listeners.ContainsKey(eventId) == false)
                _listeners.Add(eventId, new List<Action<IEventMessage>>());
            if (_listeners[eventId].Contains(listener) == false)
                _listeners[eventId].Add(listener);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
        {
            RemoveListener(typeof(TEvent), listener);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        public void RemoveListener(System.Type eventType, System.Action<IEventMessage> listener)
        {
            int eventId = eventType.GetHashCode();
            if (_listeners.ContainsKey(eventId))
            {
                if (_listeners[eventId].Contains(listener))
                    _listeners[eventId].Remove(listener);
            }
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        public void SendMessage(IEventMessage message)
        {
            int eventId = message.GetType().GetHashCode();
            if (_listeners.ContainsKey(eventId) == false)
                return;

            List<Action<IEventMessage>> actions = _listeners[eventId];
            for (int i = 0; i < actions.Count; i++)
            {
                actions[i].Invoke(message);
            }

            // 回收引用对象
            IReference reference = message as IReference;
            if (reference != null)
                ReferencePool.Release(reference);

        }

        /// <summary>
        /// 清空所有监听
        /// </summary>
        public void ClearListeners()
        {
            foreach (int eventId in _listeners.Keys)
            {
                _listeners[eventId].Clear();
            }
            _listeners.Clear();
        }

        /// <summary>
        /// 获取监听者总数
        /// </summary>
        private object GetAllListenerCount()
        {
            int count = 0;
            foreach (var list in _listeners)
            {
                count += list.Value.Count;
            }
            return count;
        }



    }
}