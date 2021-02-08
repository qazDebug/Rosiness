/****************************************************
	文件：GameObjectPoolManager.cs
	作者：世界和平
	日期：2020/12/1 17:4:21
	功能：Nothing
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rosiness.Pool
{
    internal class GameObjectCollector : IEnumerator
    {
        private readonly Queue<SpawnGameObject> _collector;


        object IEnumerator.Current => throw new System.NotImplementedException();

        bool IEnumerator.MoveNext()
        {
            throw new System.NotImplementedException();
        }

        void IEnumerator.Reset()
        {
            throw new System.NotImplementedException();
        }

        internal void Restore(SpawnGameObject spawnGameObject)
        {
            throw new NotImplementedException();
        }
    }
}