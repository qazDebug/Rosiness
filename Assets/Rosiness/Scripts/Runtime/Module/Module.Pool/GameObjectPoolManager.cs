/****************************************************
	文件：GameObjectPoolManager.cs
	作者：世界和平
	日期：2020/12/1 17:4:21
	功能：Nothing
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Rosiness.Pool
{
    /// <summary>
    /// 游戏对象池管理器
    /// </summary>
    public sealed class GameObjectPoolManager : ModuleSingleton<GameObjectPoolManager>, IModule
    {
        private readonly Dictionary<string, GameObjectCollector> _collectors = new Dictionary<string, GameObjectCollector>();
        private GameObject _root;

        void IModule.OnCreate(object createParam)
        {
            _root = new GameObject("[PoolManager]");
            _root.transform.position = Vector3.zero;
            _root.transform.eulerAngles = Vector3.zero;
            Object.DontDestroyOnLoad(_root);
        }

        void IModule.OnGUI()
        {
        }

        void IModule.OnUpdate()
        {
        }

        
    }
}