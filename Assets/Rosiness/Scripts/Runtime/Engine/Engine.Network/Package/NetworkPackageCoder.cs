/****************************************************
	文件：NetworkPackageCoder.cs
	作者：世界和平
	日期：2021/1/12 11:52:57
	功能：网络包编码解码器
*****************************************************/
using Rosiness.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Rosiness.Network
{
    /// <summary>
    /// 网络包编码解码器
    /// </summary>
    public abstract class NetworkPackageCoder
    {
		private TcpChannel _channel;

		/// <summary>
		/// 包体的最大尺寸
		/// </summary>
		public int PackageBodyMaxSize { private set; get; }

		/// <summary>
		/// 初始化编码解码器
		/// </summary>
		public void InitCoder(TcpChannel channel, int packageBodyMaxSize)
		{
			_channel = channel;
			PackageBodyMaxSize = packageBodyMaxSize;
		}

		/// <summary>
		/// 获取包头的尺寸
		/// </summary>
		public abstract int GetPackageHeaderSize();

		/// <summary>
		/// 编码
		/// </summary>
		/// <param name="sendBuffer">编码填充的字节缓冲区</param>
		/// <param name="packageObj">需要编码的包裹对象</param>
		public abstract void Encode(ByteBuffer sendBuffer, System.Object packageObj);

		/// <summary>
		/// 解码
		/// </summary>
		/// <param name="receiveBuffer">解码需要的字节缓冲区</param>
		/// <param name="outputResult">解码成功后的包裹对象列表</param>
		public abstract void Decode(ByteBuffer receiveBuffer, List<System.Object> outputResult);

		/// <summary>
		/// 捕捉错误异常
		/// </summary>
		protected void HandleError(bool isDispose, string error)
		{
			_channel.HandleError(isDispose, error);
		}
	}
}