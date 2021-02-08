/****************************************************
	文件：AssetOperationHandle.cs
	作者：世界和平
	日期：2020/12/4 9:26:43
	功能：Nothing
*****************************************************/
using System.Collections;
using UnityEngine;

namespace Rosiness.Resource
{
    public struct AssetOperationHandle : IEnumerator
    {
        private IAssetProvider _provider;

        internal AssetOperationHandle(IAssetProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
		/// 句柄是否有效（AssetFileLoader销毁会导致所有句柄失效）
		/// </summary>
		public bool IsValid
        {
            get
            {
                return _provider != null && _provider.IsValid;
            }
        }

        /// <summary>
        /// 当前的加载状态
        /// </summary>
        public EAssetStates States
        {
            get
            {
                if (!IsValid)
                    return EAssetStates.None;
                return _provider.States;
            }
        }

        /// <summary>
		/// 加载进度
		/// </summary>
		public float Progress
        {
            get
            {
                if (IsValid == false)
                    return 0;
                return _provider.Progress;
            }
        }

        /// <summary>
		/// 是否加载完毕
		/// </summary>
		public bool IsDone
        {
            get
            {
                if (IsValid == false)
                    return false;
                return _provider.IsDone;
            }
        }

        /// <summary>
		/// 完成委托
		/// </summary>
		public event System.Action<AssetOperationHandle> Completed
        {
            add
            {
                if (IsValid == false)
                    throw new System.Exception($"{nameof(AssetOperationHandle)} is invalid");
                if (_provider.IsDone)
                    value.Invoke(this);
                else
                    _provider.Callback += value;
            }
            remove
            {
                if (IsValid == false)
                    throw new System.Exception($"{nameof(AssetOperationHandle)} is invalid");
                _provider.Callback -= value;
            }            
        }

        /// <summary>
		/// 资源对象
		/// </summary>
        public Object AssetObject
        {
            get
            {
                if (!IsValid)
                    return null;
                return _provider.AssetObject;
            }
        }

        /// <summary>
		/// 资源场景
		/// </summary>
		public SceneInstance AssetScene
        {
            get
            {
                if (IsValid == false)
                    return null;
                return _provider.AssetScene;
            }
        }

        /// <summary>
        /// 初始化的游戏对象（只限于请求的资源对象类型为GameObject）
        /// </summary>
        public GameObject InstantiateObject
        {
            get
            {
                if (IsValid == false)
                    return null;
                if (_provider.AssetObject == null)
                    return null;
                return UnityEngine.Object.Instantiate(_provider.AssetObject as GameObject);
            }
        }

        /// <summary>
		/// 释放资源句柄
		/// </summary>
		public void Release()
        {
            if (IsValid == false)
                return;
            _provider.Release();
            _provider = null;
        }

        #region 异步操作相关
        /// <summary>
        /// 异步操作任务
        /// </summary>
        public System.Threading.Tasks.Task<object> Task
        {
            get { return _provider.Task; }
        }

        // 协程相关
        bool IEnumerator.MoveNext()
        {
            return !IsDone;
        }
        void IEnumerator.Reset()
        {
        }
        object IEnumerator.Current
        {
            get { return AssetObject; }
        }
        #endregion
    }
}