/****************************************************
	文件：UIPanelMonitor.cs
	作者：世界和平
	日期：2021/3/19 18:14:8
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;
using Rosiness.Editor;
using UnityEditor;
using UnityEngine;

namespace MotionFramework.Editor
{
	public class UIPanelMonitor : UnityEditor.Editor
	{
		[InitializeOnLoadMethod]
		static void StartInitializeOnLoadMethod()
		{
#if UNITY_2018_4_OR_NEWER
			// 监听新的Prefab系统
			UnityEditor.Experimental.SceneManagement.PrefabStage.prefabSaving += OnPrefabSaving;
#else
			// 监听Inspector的Apply事件
			PrefabUtility.prefabInstanceUpdated = delegate (GameObject go)
			{
				UIManifest manifest = go.GetComponent<UIManifest>();
				if (manifest != null)
					UIPanelModifier.Refresh(manifest);
			};
#endif
		}

#if UNITY_2018_4_OR_NEWER
		static void OnPrefabSaving(GameObject go)
		{
			UnityEditor.Experimental.SceneManagement.PrefabStage stage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
			if (stage != null)
			{
				UnityEngine.UI.UIManifest manifest = go.GetComponent<UnityEngine.UI.UIManifest>();
				if (manifest != null)
					UIPanelModifier.Refresh(manifest);
			}
		}
#endif
	}
}