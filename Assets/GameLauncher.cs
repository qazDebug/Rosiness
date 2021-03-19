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
using Rosiness.Audio;
using Rosiness.Resource;
using Rosiness.Scene;
using Rosiness.Pool;
using Rosiness.Patch;
using Rosiness.Network;

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
        // 创建事件管理器
        RosinessEngine.CreateModule<EventManager>();

        // 创建网络管理器
        var networkCreateParam = new NetworkManager.CreateParameters {PackageCoderType = typeof(ProtoPackageCoder)};
        RosinessEngine.CreateModule<NetworkManager>(networkCreateParam);

        // 本地资源服务接口
        LocalBundleServices bundleServices = new LocalBundleServices();
        yield return bundleServices.InitializeAsync(SimulationOnEditor);

        // 创建资源管理器
        //// 直接使用通用的补丁管理器
        //IBundleServices bundleServices = MotionEngine.GetMoudle<PatchManager>();
        var resourceCreateParam = new ResourceManager.CreateParameters
        {
            LocationRoot = "Assets/GameRes",
            SimulationOnEditor = SimulationOnEditor,
            BundleServices = bundleServices,
            DecryptServices = null,
            AutoReleaseInterval = 10f
        };
        RosinessEngine.CreateModule<ResourceManager>(resourceCreateParam);

        // 创建音频管理器
        RosinessEngine.CreateModule<AudioManager>();

        
        RosinessEngine.CreateModule<TweenManager>();
        
        // 创建场景管理器
        RosinessEngine.CreateModule<SceneManager>();

        // 创建对象池管理器
        RosinessEngine.CreateModule<GameObjectPoolManager>();

        // 最后创建游戏业务逻辑相关的自定义模块
        // 直接进入游戏
        var luaCreateParam = new LuaManager.CreateParameters {SimulationOnEditor = SimulationOnEditor};
        RosinessEngine.CreateModule<LuaManager>(luaCreateParam);
        LuaManager.Instance.StartGame();
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
