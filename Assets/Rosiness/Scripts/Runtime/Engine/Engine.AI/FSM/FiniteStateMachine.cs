/****************************************************
	文件：FiniteStateMachine.cs
	作者：世界和平
	日期：2021/2/21 17:29:33
	功能：有限状态机
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.AI
{
	public class FiniteStateMachine
	{
		private readonly List<IFsmNode> _nodes = new List<IFsmNode>();
		private IFsmNode _curNode;
		private IFsmNode _preNode;

		/// <summary>
		/// 节点转换关系图
		/// 注意：如果为NULL则不检测转换关系
		/// </summary>
		public FsmGraph Graph;

		/// <summary>
		/// 当前运行的节点名称
		/// </summary>
		public string CurrentNodeName
		{
			get { return _curNode != null ? _curNode.Name : string.Empty; }
		}

		/// <summary>
		/// 之前运行的节点名称
		/// </summary>
		public string PreviousNodeName
		{
			get { return _preNode != null ? _preNode.Name : string.Empty; }
		}

		/// <summary>
		/// 启动状态机
		/// </summary>
		/// <param name="entryNode">入口节点</param>
		public void Run(string entryNode)
		{
			_curNode = GetNode(entryNode);
			_preNode = GetNode(entryNode);

			if (_curNode != null)
				_curNode.OnEnter();
			else
				RosinessLog.Error($"Not found entry node : {entryNode}");
		}

		private IFsmNode GetNode(string nodeName)
		{
			for (int i = 0; i < _nodes.Count; i++)
			{
				if (_nodes[i].Name == nodeName)
					return _nodes[i];
			}
			return null;
		}

		/// <summary>
		/// 更新状态机
		/// </summary>
		public void Update()
		{
			if (_curNode != null)
				_curNode.OnUpdate();
		}

		/// <summary>
		/// 加入一个节点
		/// </summary>
		public void AddNode(IFsmNode node)
		{
			if (node == null)
				throw new ArgumentNullException();

			if (_nodes.Contains(node) == false)
			{
				_nodes.Add(node);
			}
			else
			{
				RosinessLog.Warning($"Node {node.Name} already existed");
			}
		}

		/// <summary>
		/// 转换节点
		/// </summary>
		public void Transition(string nodeName)
		{
			if (string.IsNullOrEmpty(nodeName))
				throw new ArgumentNullException();

			IFsmNode node = GetNode(nodeName);
			if (node == null)
			{
				RosinessLog.Error($"Can not found node {nodeName}");
				return;
			}

            //检测转换关系
            if (Graph != null)
            {
				if(Graph.CanTransition(_curNode.Name,node.Name) == false)
                {
					RosinessLog.Error($"Can not transition {_curNode} to {node}");
					return;
				}
			}

			RosinessLog.Log($"Transition {_curNode.Name} to {node.Name}");
			_preNode = _curNode;
			_curNode.OnExit();
			_curNode = node;
			_curNode.OnEnter();
		}

		/// <summary>
		/// 返回到之前的节点
		/// </summary>
		public void RevertToPreviousNode()
		{
			Transition(PreviousNodeName);
		}

		/// <summary>
		/// 接收消息
		/// </summary>
		public void HandleMessage(object msg)
		{
			if (_curNode != null)
				_curNode.OnHandleMessage(msg);
		}

		private bool IsContains(string nodeName)
		{
			for (int i = 0; i < _nodes.Count; i++)
			{
				if (_nodes[i].Name == nodeName)
					return true;
			}
			return false;
		}
	}
}