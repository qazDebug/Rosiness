/****************************************************
	文件：IAssetProcessor.cs
	作者：世界和平
	日期：2021/2/25 15:34:39
	功能：Nothing
*****************************************************/
using UnityEditor;

namespace Rosiness.Editor
{

    /// <summary>
    /// 资源处理器接口
    /// </summary>
    public interface IAssetProcessor
    {
        void OnPreprocessModel(string importAssetPath, AssetImporter assetImporter);
        void OnPreprocessTexture(string importAssetPath, AssetImporter assetImporter);
        void OnPreprocessAudio(string importAssetPath, AssetImporter assetImporter);
        void OnPreprocessSpriteAtlas(string importAssetPath, AssetImporter assetImporter);
    }
}