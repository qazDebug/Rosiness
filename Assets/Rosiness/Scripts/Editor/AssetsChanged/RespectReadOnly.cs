/****************************************************
	文件：RespectReadOnly.cs
	作者：世界和平
	日期：2020/11/27 10:18:14
	功能：监听将要进行 创建  删除  移动 保存 的操作
*****************************************************/

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

namespace Rosiness.Editor
{
    public class RespectReadOnly : UnityEditor.AssetModificationProcessor
    {
        // Known issues:
        // You can still apply changes to prefabs of locked files (but the prefabs wont be saved)
        // You can add add components to prefabs (but the prefabs wont be saved)
        // IsOpenForEdit might get called a few too many times per object selection, so try and cache the result for performance (i.e called in same frame)

        public static void OnWillCreateAsset(string path)
        {
            Debug.Log(("OnWillCreateAsset " + path).Colored(Colors.green));

            ScriptsInfoRecoder(path);
        }

        /// <summary>
        /// ScriptsInfoRecoder 替换脚本模板内容
        /// </summary>
        static void ScriptsInfoRecoder(string path)
        {
            path = path.Replace(".meta", "");
            if (path.EndsWith(".cs"))
            {
                string str = File.ReadAllText(path);
                str = str.Replace("#CreateAuthor#", Environment.UserName).Replace(
                    "#CreateTime#", string.Concat(DateTime.Now.Year, "/", DateTime.Now.Month, "/",
                    DateTime.Now.Day, " ", DateTime.Now.Hour, ":", DateTime.Now.Minute, ":", DateTime.Now.Second));
                File.WriteAllText(path, str);
            }
        }

        public static string[] OnWillSaveAssets(string[] paths)
        {
            List<string> result = new List<string>();
            foreach (var path in paths)
            {
                if (IsUnlocked(path))
                    result.Add(path);
                else
                    Debug.LogError(path + " is read-only.");
            }
            Debug.Log(("OnWillSaveAssets".Colored(Colors.cyan)));
            return result.ToArray();
        }

        public static AssetMoveResult OnWillMoveAsset(string oldPath, string newPath)
        {
            AssetMoveResult result = AssetMoveResult.DidNotMove;
            if (IsLocked(oldPath))
            {
                Debug.LogError(string.Format("Could not move {0} to {1} because {0} is locked!", oldPath, newPath));
                result = AssetMoveResult.FailedMove;
            }
            else if (IsLocked(newPath))
            {
                Debug.LogError(string.Format("Could not move {0} to {1} because {1} is locked!", oldPath, newPath));
                result = AssetMoveResult.FailedMove;
            }
            Debug.Log(("OnWillMoveAsset  from" + oldPath + " to " + newPath).Colored(Colors.darkblue));
            return result;
        }

        public static AssetDeleteResult OnWillDeleteAsset(string assetPath, RemoveAssetOptions option)
        {
            if (IsLocked(assetPath))
            {
                Debug.LogError(string.Format("Could not delete {0} because it is locked!", assetPath));
                return AssetDeleteResult.FailedDelete;
            }

            Debug.Log(("OnWillDeleteAsset" + assetPath).Colored(Colors.red));
            return AssetDeleteResult.DidNotDelete;
        }

        public static bool IsOpenForEdit(string assetPath, out string message)
        {
            if (IsLocked(assetPath))
            {
                message = "File is locked for editing!";
                return false;
            }
            else
            {
                message = null;
                return true;
            }
        }

        static bool IsUnlocked(string path)
        {
            return !IsLocked(path);
        }

        static bool IsLocked(string path)
        {
            if (!File.Exists(path))
                return false;
            FileInfo fi = new FileInfo(path);
            return fi.IsReadOnly;
        }
    }
}