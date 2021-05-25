using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace libx
{
    public static class MenuItems
    {
        private const string KApplyBuildRules = "Rosiness/Bundles/Build Rules";
        private const string KBuildAssetBundles = "Rosiness/Bundles/Build Bundles";
        private const string KBuildPlayer = "Rosiness/Bundles/Build Player";
		private const string KViewDataPath = "Rosiness/Bundles/View Bundles";
        private const string KCopyBundles = "Rosiness/Bundles/Copy Bundles";

        [MenuItem("Assets/Apply Rule/Text", false, 1)]
        private static void ApplyRuleText()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternText);
        }

        [MenuItem("Assets/Apply Rule/Prefab", false, 1)]
        private static void ApplyRulePrefab()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternPrefab);
        }

        [MenuItem("Assets/Apply Rule/Png", false, 1)]
        private static void ApplyRulePng()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternPng);
        }

        [MenuItem("Assets/Apply Rule/Material", false, 1)]
        private static void ApplyRuleMaterial()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternMaterial);
        }

        [MenuItem("Assets/Apply Rule/Controller", false, 1)]
        private static void ApplyRuleController()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternController);
        }

        [MenuItem("Assets/Apply Rule/Asset", false, 1)]
        private static void ApplyRuleAsset()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternAsset);
        }

        [MenuItem("Assets/Apply Rule/Scene", false, 1)]
        private static void ApplyRuleScene()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternScene);
        }

        [MenuItem("Assets/Apply Rule/Directory", false, 1)]
        private static void ApplyRuleDir()
        {
            var rules = BuildScript.GetBuildRules();
            AddRulesForSelection(rules, rules.searchPatternDir);
        }

        private static void AddRulesForSelection(BuildRules rules, string searchPattern)
        {
            var isDir = rules.searchPatternDir.Equals(searchPattern);
            foreach (var item in Selection.objects)
            {
                var path = AssetDatabase.GetAssetPath(item);
                var rule = new BuildRule
                {
                    searchPath = path,
                    searchPattern = searchPattern,
                    nameBy = isDir ? NameBy.Directory : NameBy.Path
                };
                ArrayUtility.Add(ref rules.rules, rule);
            }

            EditorUtility.SetDirty(rules);
            AssetDatabase.SaveAssets();
        }

        [MenuItem(KApplyBuildRules)]
        private static void ApplyBuildRules()
        {
            var watch = new Stopwatch();
            watch.Start();
            BuildScript.ApplyBuildRules();
            watch.Stop();
            Debug.Log("ApplyBuildRules " + watch.ElapsedMilliseconds + " ms.");
        }

        [MenuItem(KBuildAssetBundles)]
        private static void BuildAssetBundles()
        {
            var watch = new Stopwatch();
            watch.Start();
			BuildScript.ApplyBuildRules ();
            BuildScript.BuildAssetBundles();
            watch.Stop();
            Debug.Log("BuildAssetBundles " + watch.ElapsedMilliseconds + " ms.");
        } 

        [MenuItem(KBuildPlayer)]
        private static void BuildStandalonePlayer()
        {
            BuildScript.BuildStandalonePlayer();
        }

		[MenuItem(KViewDataPath)]
        private static void ViewDataPath()
        {
			EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
        }

        [MenuItem(KCopyBundles)]
        private static void CopyAssetBundles()
        {
            BuildScript.CopyAssetBundlesTo(Application.streamingAssetsPath);
        } 

#if !UNITY_2018_1_OR_NEWER
        private const string KCopyPath = "Assets/Copy Path";
        [MenuItem(KCopyPath)]
        private static void CopyPath()
        {
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            EditorGUIUtility.systemCopyBuffer = assetPath;
            Debug.Log(assetPath);
        }
#endif
        private const string KToJson = "Assets/ToJson";

        [MenuItem(KToJson)]
        private static void ToJson()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            var json = JsonUtility.ToJson(Selection.activeObject);
            File.WriteAllText(path.Replace(".asset", ".json"), json);
            AssetDatabase.Refresh();
        }

        #region Tools 
        [MenuItem("Tools/View CRC")]
        private static void GetCRC()
        {
            var path = EditorUtility.OpenFilePanel("OpenFile", Environment.CurrentDirectory, "");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            using (var fs = File.OpenRead(path))
            {
                var crc = Utility.GetCRC32Hash(fs);
                Debug.Log(crc);
            }
        }

        [MenuItem("Tools/View MD5")]
        private static void GetMD5()
        {
            var path = EditorUtility.OpenFilePanel("OpenFile", Environment.CurrentDirectory, "");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            using (var fs = File.OpenRead(path))
            {
                var crc = Utility.GetMD5Hash(fs);
                Debug.Log(crc);
            }
        }

        [MenuItem("Tools/Take a Screenshot")]
        private static void Screenshot()
        {
            var path = EditorUtility.SaveFilePanel("截屏", null, "screenshot_", "png");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ScreenCapture.CaptureScreenshot(path);

        }
        #endregion 
    }
}