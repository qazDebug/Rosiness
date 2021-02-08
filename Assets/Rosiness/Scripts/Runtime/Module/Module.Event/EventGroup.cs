/****************************************************
	文件：EventGroup.cs
	作者：世界和平
	日期：2020/12/1 15:31:26
	功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Event
{
    public class EventGroup 
    {
		private readonly Dictionary<System.Type, List<Action<IEventMessage>>> _cachedListener = new Dictionary<System.Type, List<Action<IEventMessage>>>();

		/// <summary>
		/// 添加一个监听
		/// </summary>
		public void AddListener<TEvent>(System.Action<IEventMessage> listener) where TEvent : IEventMessage
		{
			System.Type eventType = typeof(TEvent);
			if (_cachedListener.ContainsKey(eventType) == false)
				_cachedListener.Add(eventType, new List<Action<IEventMessage>>());

			if (_cachedListener[eventType].Contains(listener) == false)
			{
				_cachedListener[eventType].Add(listener);
				EventManager.Instance.AddListener(eventType, listener);
			}
			else
			{
				RosinessLog.Warning($"Event listener is exist : {eventType}");
			}
		}

		/// <summary>
		/// 移除所有缓存的监听
		/// </summary>
		public void RemoveAllListener()
		{
			foreach (var pair in _cachedListener)
			{
				System.Type eventType = pair.Key;
				for (int i = 0; i < pair.Value.Count; i++)
				{
					EventManager.Instance.RemoveListener(eventType, pair.Value[i]);
				}
				pair.Value.Clear();
			}
			_cachedListener.Clear();
		}
	}
}