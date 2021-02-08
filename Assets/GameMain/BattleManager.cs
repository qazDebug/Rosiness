/****************************************************
	文件：BattleManager.cs
	作者：世界和平
	日期：2020/11/27 14:19:31
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rosiness;

public class BattleManager : IModule
{
    void IModule.OnCreate(object createParam)
    {
        RosinessLog.Log("OnCreate");
    }

    void IModule.OnGUI()
    {
        RosinessLog.Log("OnGUI");

    }

    void IModule.OnUpdate()
    {
        //RosinessLog.Log("OnUpdate");

    }

    public void Print()
    {
        RosinessLog.Log("Hello world");
    }

}
