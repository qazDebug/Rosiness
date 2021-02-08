/****************************************************
	文件：StringOperate.cs
	作者：世界和平
	日期：2020/12/4 9:34:5
	功能：Nothing
*****************************************************/
using System;

namespace Rosiness.IO
{
	/// </summary>
	/// 字符串操作类
	/// 注意：Span<T>需要C# 7.2支持
	/// </summary>
	public static class StringOperate
	{
		private static string _operateString;
		private static int _operateIndex = 0;

		/// <summary>
		/// 设置要处理的字符串
		/// </summary>
		public static void SetOperateString(string content)
		{
			_operateString = content;
			_operateIndex = 0;
		}

		public static bool NextFloat(char separator, out float value)
		{
			value = 0;

			string span = MoveNext(separator);
			if (span == null)
			{
				return false;
			}
			else
			{
				value = float.Parse(span);
				return true;
			}
		}

		public static bool NextDouble(char separator, out double value)
		{
			value = 0;

			string span = MoveNext(separator);

			if (span == null)
			{
				return false;
			}
			else
			{
				value = double.Parse(span);
				return true;
			}
		}

		public static bool NextInt(char separator, out int value)
		{
			value = 0;

			string span = MoveNext(separator);

			if (span == null)
			{
				return false;
			}
			else
			{
				value = int.Parse(span);
				return true;
			}
		}

		public static bool NextLong(char separator, out long value)
		{
			value = 0;

			string span = MoveNext(separator);

			if (span == null)
			{
				return false;
			}
			else
			{
				value = long.Parse(span);
				return true;
			}
		}
		public static bool NextString(char separator, out string value)
		{
			value = null;

			string span = MoveNext(separator);

			if (span == null)
			{
				return false;
			}
			else
			{
				value = span.ToString();
				return true;
			}
		}

		private static string MoveNext(char separator)
		{
			int beginIndex = _operateIndex;

			for (int i = _operateIndex; i < _operateString.Length; i++)
			{
				bool isLastChar = _operateIndex == _operateString.Length - 1;
				bool isSeparatorChar = _operateString[i] == separator;

				if (isSeparatorChar || isLastChar)
				{
					if (isLastChar && isSeparatorChar == false)
						_operateIndex++;

					int charCount = _operateIndex - beginIndex;
					if (charCount == 0)
					{
						throw new InvalidOperationException($"Invalid operate string : {_operateString}");
					}

					_operateIndex++;
					return _operateString.Substring(beginIndex, charCount);
				}
				else
				{
					_operateIndex++;
				}
			}

			return null; //移动失败返回NULL
		}
	}
}