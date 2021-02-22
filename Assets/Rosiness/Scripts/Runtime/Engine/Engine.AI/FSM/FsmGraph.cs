/****************************************************
	文件：FsmGraph.cs
	作者：世界和平
	日期：2021/2/21 17:29:45
	功能：转换关系图
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.AI
{
    public class FsmGraph
    {
		private readonly Dictionary<string, List<string>> _graph = new Dictionary<string, List<string>>();
		private readonly string _globalNode;

		public FsmGraph(string globalNode)
        {
			_globalNode = globalNode;
        }

		/// <summary>
		/// 添加转换关系
		/// </summary>
		/// <param name="nodeName">节点名称</param>
		/// <param name="transitionNodes">可以转换到的节点列表</param>
		public void AddTransition(string nodeName, List<string> transitionNodes)
		{
			if (transitionNodes == null)
				throw new ArgumentNullException();

			if (_graph.ContainsKey(nodeName))
			{
				RosinessLog.Warning($"Graph node {nodeName} already existed.");
				return;
			}

			_graph.Add(nodeName, transitionNodes);
		}

		/// <summary>
		/// 检测转换关系
		/// </summary>
		public bool CanTransition(string from, string to)
		{
			if (_graph.ContainsKey(from) == false)
			{
				RosinessLog.Warning($"Not found graph node {from}");
				return false;
			}

			if (to == _globalNode)
				return true;

			return _graph[from].Contains(to);
		}
	}
}