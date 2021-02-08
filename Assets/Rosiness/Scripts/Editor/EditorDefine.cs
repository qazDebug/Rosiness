/****************************************************
	文件：EditorDefine.cs
	作者：世界和平
	日期：2020/11/27 10:46:6
	功能：Nothing
*****************************************************/

namespace Rosiness.Editor
{
    public class EditorDefine
    {
        /// <summary>
		/// 资源导入工具的配置文件存储路径
		/// </summary>
		public const string AssetImporterSettingFilePath = "Assets/RosinessSetting/AssetImporterSetting.asset";

        /// <summary>
        /// 资源包收集工具的配置文件存储路径
        /// </summary>
        public const string AssetBundleCollectorSettingFilePath = "Assets/RosinessSetting/AssetBundleCollectorSetting.asset";

        /// <summary>
        /// UI面板的配置文件存储路径
        /// </summary>
        public const string UIPanelSettingFilePath = "Assets/RosinessSetting/UIPanelSetting.asset";
    }

    /// <summary>
    /// 资源搜索类型
    /// </summary>
    public enum EAssetSearchType
    {
        All,
        AnimationClip,
        AudioClip,
        AudioMixer,
        Font,
        Material,
        Mesh,
        Model,
        PhysicMaterial,
        Prefab,
        Scene,
        Script,
        Shader,
        Sprite,
        Texture,
        VideoClip,
    }

    /// <summary>
    /// 资源文件格式
    /// </summary>
    public enum EAssetFileExtension
    {
        prefab, //预制体
        unity, //场景
        fbx, //模型
        anim, //动画
        png, //图片
        jpg, //图片
        mat, //材质球
        shader, //着色器
        ttf, //字体
        cs, //脚本
    }
}