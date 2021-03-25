using System;
using System.Collections;
using System.Collections.Generic;

namespace Rosiness.Patch
{
	/// <summary>
	/// 变体规则
	/// </summary>
	public class VariantRule
	{
		public const string DefaultTag = "default";

		/// <summary>
		/// 变体组
		/// </summary>
		public List<string> VariantGroup;

		/// <summary>
		/// 目标变体
		/// </summary>
		public string TargetVariant;
	}
}