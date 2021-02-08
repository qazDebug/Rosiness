/****************************************************
	文件：ConvertUtil.cs
	作者：世界和平
	日期：2021/1/13 18:34:22
	功能：Nothing
*****************************************************/
using System;

namespace Rosiness.Editor
{
    public class ConvertUtil
    {
        public static long Str2Long(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return Convert.ToInt64(Convert.ToDecimal(str));
        }

        public static int Str2Int(string str)
        {
            return Convert.ToInt32(Convert.ToDecimal(str));
        }

        public static double Str2Double(string str)
        {
            return Convert.ToDouble(Convert.ToDecimal(str));
        }
    }
}