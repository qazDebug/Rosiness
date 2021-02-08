/****************************************************
	文件：StringFormat.cs
	作者：世界和平
	日期：2020/12/4 9:33:57
	功能：Nothing
*****************************************************/
using System;
using System.Text;

namespace Rosiness.IO
{
    public static class StringFormat 
    {
        [ThreadStatic]
        private static StringBuilder _cacheBuilder = new StringBuilder(1024);

        public static string Format(string format, object arg0)
        {
            if (string.IsNullOrEmpty(format))
                throw new ArgumentNullException();

            _cacheBuilder.Length = 0;
            _cacheBuilder.AppendFormat(format, arg0);
			return _cacheBuilder.ToString();
        }
		public static string Format(string format, object arg0, object arg1)
		{
			if (string.IsNullOrEmpty(format))
				throw new ArgumentNullException();

			_cacheBuilder.Length = 0;
			_cacheBuilder.AppendFormat(format, arg0, arg1);
			return _cacheBuilder.ToString();
		}
		public static string Format(string format, object arg0, object arg1, object arg2)
		{
			if (string.IsNullOrEmpty(format))
				throw new ArgumentNullException();

			_cacheBuilder.Length = 0;
			_cacheBuilder.AppendFormat(format, arg0, arg1, arg2);
			return _cacheBuilder.ToString();
		}
		public static string Format(string format, params object[] args)
		{
			if (string.IsNullOrEmpty(format))
				throw new ArgumentNullException();

			if (args == null)
				throw new ArgumentNullException();

			_cacheBuilder.Length = 0;
			_cacheBuilder.AppendFormat(format, args);
			return _cacheBuilder.ToString();
		}
	}
}