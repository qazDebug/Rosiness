/****************************************************
	文件：CsvController.cs
	作者：世界和平
	日期：2021/1/13 14:44:31
	功能：Nothing
*****************************************************/
using System.IO;
using System.Collections.Generic;

namespace Rosiness.Editor
{
    public class CsvController
    {
		static CsvController csv;
		public List<string[]> arrayData;

		private CsvController()   //单例，构造方法为私有
		{
			arrayData = new List<string[]>();
		}

		public static CsvController GetInstance()
        {
			if(csv == null)
            {
				csv = new CsvController();
            }
			return csv;
        }

		public void loadFile(string path, string fileName)
		{
			arrayData.Clear();
			StreamReader sr = null;
			try
			{
				string file_url = path + "//" + fileName;    //根据路径打开文件
				sr = File.OpenText(file_url);
				//Debug.Log("File Find in " + file_url);
			}
			catch
			{
				//Debug.Log("File cannot find ! ");
				return;
			}

			string line;
			while ((line = sr.ReadLine()) != null)   //按行读取
			{
				arrayData.Add(line.Split(','));   //每行逗号分隔,split()方法返回 string[]
			}
			sr.Close();
			sr.Dispose();
		}

		public string getString(int row, int col)
		{
			return arrayData[row][col];
		}
		public int getInt(int row, int col)
		{
			return int.Parse(arrayData[row][col]);
		}
	}
}