/****************************************************
	文件：ReferencePool.cs
	作者：世界和平
	日期：2020/12/1 11:58:1
	功能：引用池
*****************************************************/
using System;
using System.Collections.Generic;

namespace Rosiness.Reference
{
    /// <summary>
    /// 引用池
    /// </summary>
    public static class ReferencePool
    {

        /// <summary>
        /// 对象池初始初始容量
        /// </summary>
        public static int InitCapacity { get; set; } = 100;

        /// <summary>
        /// 对象池的数量
        /// </summary>
        public static int Count
        {
            get
            {
                return GetAllCollectors.Count;
            }
        }

        /// <summary>
		/// 清除所有对象池
		/// </summary>
		public static void ClearAll()
        {
            foreach (KeyValuePair<Type, ReferenceCollector> pair in GetAllCollectors)
            {
                pair.Value.Clear();
            }
            GetAllCollectors.Clear();
        }

        /// <summary>
        /// 申请引用对象
        /// </summary>
        public static T Spawn<T>() where T : class, IReference, new()
        {
            Type type = typeof(T);
            return Spawn(type) as T;
        }


        /// <summary>
		/// 申请引用对象
		/// </summary>
		public static IReference Spawn(Type type)
        {
            if (GetAllCollectors.ContainsKey(type) == false)
            {
                GetAllCollectors.Add(type, new ReferenceCollector(type, InitCapacity));
            }
            return GetAllCollectors[type].Spawn();
        }

        /// <summary>
		/// 回收引用对象
		/// </summary>
		public static void Release(IReference item)
        {
            Type type = item.GetType();
            if (GetAllCollectors.ContainsKey(type) == false)
            {
                GetAllCollectors.Add(type, new ReferenceCollector(type, InitCapacity));
            }
            GetAllCollectors[type].Release(item);
        }

        /// <summary>
        /// 批量回收列表集合
        /// </summary>
        public static void Release<T>(List<T> items) where T : class, IReference, new()
        {
            Type type = typeof(T);
            if (GetAllCollectors.ContainsKey(type) == false)
            {
                GetAllCollectors.Add(type, new ReferenceCollector(type, InitCapacity));
            }

            for (int i = 0; i < items.Count; i++)
            {
                GetAllCollectors[type].Release(items[i]);
            }
        }

        #region 调试专属方法
        internal static Dictionary<Type, ReferenceCollector> GetAllCollectors { get; } = new Dictionary<Type, ReferenceCollector>();
        #endregion
    }
}