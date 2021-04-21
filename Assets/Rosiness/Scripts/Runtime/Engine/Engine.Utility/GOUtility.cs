using UnityEngine;

namespace Rosiness.Utility
{
    public class GOUtility
    {
        public delegate IterationArguments UnitProcess(Transform unit, string path);

        /// <summary>
        /// child 迭代器
        /// 迭代当前root的每一个孩子，以及孩子的孩子。。。。
        /// 并且把每个孩子交给processor进行处理
        /// </summary>
        /// <param name="r"></param>当前迭代过程的根节点
        /// <param name="path"></param>当前迭代节点的节点目录
        /// <param name="controller"></param>节点控制器
        public static void IterateChild(Transform r, string path, UnitProcess controller)
        {
            IterationArguments ctrler = controller(r, path);
            if (ctrler == IterationArguments.StopAll)
            {
                return;
            }
            else if (ctrler == IterationArguments.StopCurrent)
            {
            }
            else if (ctrler == IterationArguments.Continue)
            {
                foreach (Transform child in r)
                {
                    IterateChild(child, path + child.name + "/", controller);
                }
            }
        }
        
        public static T FindObject<T>(Transform transform, string path) where T:UnityEngine.Object
        {
            //Debug.Log(path);
            if (string.IsNullOrEmpty(path)) 
                return default(T);

            Transform child = transform.Find(path);
            if (child == null) 
                return default(T);

            if (typeof(T) == typeof(GameObject))
                return child.gameObject as T;

            T com = child.GetComponent<T>();
            if (com == null) 
                return default(T);

            return com;
        }
    }

    public enum IterationArguments
    {
        Continue, //继续迭代
        StopCurrent, //跳过当前节点的迭代
        StopAll //停止所有迭代
    }
}