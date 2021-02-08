/****************************************************
	文件：CSVTable.cs
	作者：世界和平
	日期：2021/1/13 14:42:56
	功能：管理记录的数据表类
*****************************************************/
using System.Collections.Generic;

namespace Rosiness.Editor
{
    public class CSVTable
    {
        public List<string> Headers { get; } = new List<string>();

        public List<CSVRecord> Records { get; } = new List<CSVRecord>();

        public CSVTable()
        {
            this.Initialize();
        }

        public void Initialize()
        {
        }

        public void Execution()
        {
        }

        public void AddHeaders(string t_header)
        {
            Headers.Add(t_header);
        }

        public void AddRecord(CSVRecord t_record)
        {
            Records.Add(t_record);
        }

        public CSVRecord GetRecord(int t_record_number)
        {
            return Records[t_record_number];
        }
    }
}