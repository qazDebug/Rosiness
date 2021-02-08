/****************************************************
	文件：CSVLoader.cs
	作者：世界和平
	日期：2021/1/13 14:39:55
	功能：CSV
*****************************************************/
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Rosiness.Editor
{
    public class CSVLoader
    {
        public CSVLoader()
        {
            this.Initialize();
        }

        private void Initialize()
        {
        }

        public void Execution()
        {
        }

        public CSVTable LoadCSV(TextAsset csvAsset)
        {
            return LoadCSVFromContent(csvAsset.text);
        }

        public CSVTable LoadCSV(string t_csv_path)
        {
            // 加载CSV为文本
            TextAsset csvTextAsset = Resources.Load(t_csv_path) as TextAsset;
            Debug.Log(string.Format("<color=cyan>[LoadCSV] : {0} </color>", t_csv_path));
            return LoadCSVFromContent(csvTextAsset.text);
        }

        public CSVTable LoadCSVAsset(string csvAssetPath)
        {
            TextAsset csvTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(csvAssetPath);
            return LoadCSVFromContent(csvTextAsset.text);
        }

        private CSVTable LoadCSVFromContent(string csvContent)
        {
            //数据表类的生成
            CSVTable csvTable = new CSVTable();
            string csvText = csvContent.Replace(Environment.NewLine, "\r");
            //从文本数据的前后去除CR
            csvText = csvText.Trim('\r');
            csvText = csvText.Replace("\r\r", "\r");

            //将CR作为分段字符进行分割，转换成排列
            string[] csv = csvText.Split('\r');
            //基于多行生成列表。
            List<string> rows = new List<string>(csv);
            //分类
            string[] headers = rows[0].Split(',');
            //分类的存储
            foreach (string header in headers)
            {
                csvTable.AddHeaders(header);
            }

            //移除第一行和第二行空行
            rows.RemoveAt(0);
            rows.RemoveAt(0);

            foreach (string row in rows)
            {
                string[] fields = row.Split(',');
                
                csvTable.AddRecord(CreateRecord(headers, fields));
            }
            return csvTable;
        }

        /// <summary>
        /// 以分类名称为键输入项目生成记录的函数
        /// </summary>
        /// <param name="headers"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        private CSVRecord CreateRecord(string[] headers, string[] fields)
        {
            CSVRecord record = new CSVRecord();

            for (int i = 0; i < headers.Length; i++)
            {
                record.AddField(headers[i], fields[i]);
            }

            return record;
        }
    }
}