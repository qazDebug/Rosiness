/****************************************************
	文件：IModule.cs
	作者：世界和平
	日期：2020/11/27 11:11:4
	功能：Nothing
*****************************************************/

namespace Rosiness
{
    public interface IModule
    {
		/// <summary>
		/// 创建模块
		/// </summary>
		void OnCreate(System.Object createParam);

		/// <summary>
		/// 轮询模块
		/// </summary>
		void OnUpdate();

		/// <summary>
		/// GUI绘制
		/// </summary>
		void OnGUI();
	}
}