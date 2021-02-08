/****************************************************
	文件：AssetLoaderBase.cs
	作者：世界和平
	日期：2020/12/4 14:34:0
	功能：Nothing
*****************************************************/
using UnityEngine;

namespace Rosiness.Resource
{
	/// <summary>
	/// 扩展的场景实例对象
	/// </summary>
    public class SceneInstance
    {
		private readonly AsyncOperation _asyncOp;

		public SceneInstance(AsyncOperation op)
        {
			_asyncOp = op;
        }

		/// <summary>
		/// UnityEngine场景对象
		/// </summary>
		public UnityEngine.SceneManagement.Scene Scene { internal set; get; }

		/// <summary>
		/// 激活场景
		/// 注意：如果传入的参数SceneInstanceParam.ActivateOnLoad=false，需要手动激活场景
		/// </summary>
		public void Activate()
		{
			_asyncOp.allowSceneActivation = true;
		}
	}
	/// <summary>
	/// 加载场景实体对象需要提供的参数类
	/// </summary>
	public class SceneInstanceParam : IAssetParam
	{
		/// <summary>
		/// 是否是附加场景
		/// </summary>
		public bool IsAdditive { set; get; }

		/// <summary>
		/// 加载完毕时是否主动激活
		/// </summary>
		public bool ActivateOnLoad { set; get; }
	}
}