/****************************************************
	文件：GameLauncher.cs
	作者：世界和平
	日期：2020/11/27 11:42:28
	功能：Nothing
*****************************************************/
using System.Collections;
using UnityEngine;
using Rosiness;
using System;
using Rosiness.Console;
using Rosiness.Event;
using Rosiness.Tween;

public class GameLauncher : MonoBehaviour
{
    [Tooltip("在编辑器下模拟运行")]
    public bool SimulationOnEditor = true;
    // Start is called before the first frame update
    void Awake()
    {
#if !UNITY_EDITOR
        SimulationOnEditor = false;
#endif
        RosinessEngine.Initialize(this, HandleRosinessLog);

        if (Application.isEditor || Debug.isDebugBuild)
            DeveloperConsole.Initialize();
    }

    private void HandleRosinessLog(ELogLevel logLevel, string log)
    {
        if (logLevel == ELogLevel.Log)
        {
            Debug.Log(log);
        }
        else if (logLevel == ELogLevel.Error)
        {
            Debug.LogError(log);
        }
        else if (logLevel == ELogLevel.Warning)
        {
            Debug.LogWarning(log);
        }
        else if (logLevel == ELogLevel.Exception)
        {
            Debug.LogError(log);
        }
        else
        {
            throw new NotImplementedException($"{logLevel}");
        }
    }

    private void Start()
    {
        RosinessLog.Log("Rosiness Start...");
        RosinessEngine.StartCoroutine(CreateGameModules());
       
    }

    private IEnumerator CreateGameModules()
    {
        RosinessEngine.CreateModule<EventManager>();
        RosinessEngine.CreateModule<TweenManager>();


        // 创建游戏业务逻辑相关的自定义模块
        RosinessEngine.CreateModule<BattleManager>();

        RosinessEngine.GetModule<BattleManager>().Print();

        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        RosinessEngine.Update();
    }

    private void OnGUI()
    {
        // 绘制控制台
        if (Application.isEditor || Debug.isDebugBuild)
            DeveloperConsole.Draw();
    }
}
