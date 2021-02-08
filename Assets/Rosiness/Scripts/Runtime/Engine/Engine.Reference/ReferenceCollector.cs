/****************************************************
	文件：ReferenceCollector.cs
	作者：世界和平
	日期：2020/12/1 11:57:52
	功能：Nothing
*****************************************************/
using System;
using System.Collections.Generic;

namespace Rosiness.Reference
{
    internal class ReferenceCollector
    {
        private readonly Queue<IReference> _references;

        /// <summary>
        /// 引用类型
        /// </summary>
        public Type ClassType { private set; get; }

        /// <summary>
		/// 内部缓存总数
		/// </summary>
        public int Count
        {
            get { return _references.Count; }
        }

        /// <summary>
		/// 外部使用总数
		/// </summary>
		public int SpawnCount { private set; get; }

		public ReferenceCollector(Type type, int capacity)
		{
			ClassType = type;

			// 创建缓存池
			_references = new Queue<IReference>(capacity);

			// 检测是否继承了专属接口
			Type temp = type.GetInterface(nameof(IReference));
			if (temp == null)
				throw new Exception($"{type.Name} must inherit from {nameof(IReference)}");
		}

		/// <summary>
		/// 申请引用对象
		/// </summary>
		public IReference Spawn()
		{
			IReference item;
			if (_references.Count > 0)
			{
				item = _references.Dequeue();
			}
			else
			{
				item = Activator.CreateInstance(ClassType) as IReference;
			}
			SpawnCount++;
			return item;
		}

		/// <summary>
		/// 回收引用对象
		/// </summary>
		public void Release(IReference item)
		{
			if (item == null)
				return;

			if (item.GetType() != ClassType)
				throw new Exception($"Invalid type {item.GetType()}");

			if (_references.Contains(item))
				throw new Exception($"The item {item.GetType()} already exists.");

			SpawnCount--;
			item.OnRelease();
			_references.Enqueue(item);
		}

		/// <summary>
		/// 清空集合
		/// </summary>
		public void Clear()
		{
			_references.Clear();
			SpawnCount = 0;
		}
	}
}