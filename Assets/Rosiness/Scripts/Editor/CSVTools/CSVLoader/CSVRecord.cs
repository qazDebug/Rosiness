/****************************************************
	文件：CSVRecord.cs
	作者：世界和平
	日期：2021/1/13 14:44:24
	功能：Nothing
*****************************************************/
using System.Collections;
using System.Collections.Generic;

namespace Rosiness.Editor
{
    public class CSVRecord
    {
        private Dictionary<string, string> m_record = new Dictionary<string, string>();

		public CSVRecord()
		{
			this.Initialize();
		}

		public void Initialize()
		{
		}

		public void AddField(string t_header, string t_field)
		{
			if (!m_record.ContainsKey(t_header))
			{
				m_record.Add(t_header, t_field);
			}
		}

		public string GetField(string t_header)
		{
			if (m_record.ContainsKey(t_header))
			{
				return m_record[t_header];
			}
			return null;
		}

		public override string ToString()
		{
			string result = "";
			foreach (KeyValuePair<string, string> entry in m_record)
			{
				result += string.Format(" [{0}  {1} ]", entry.Key, entry.Value);
			}
			return result;
		}
	}
}