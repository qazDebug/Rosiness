/****************************************************
	文件：StringLoggingExtensions.cs
	作者：世界和平
	日期：2020/11/28 9:51:11
	功能：Nothing
*****************************************************/

public static class StringLoggingExtensions
{
	/// <summary>
	/// Sets the color of the text according to the parameter value.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="color">Color.</param>
	public static string Colored(this string message, Colors color)
	{
		return string.Format("<color={0}>{1}</color>", color.ToString(), message);
	}

	/// <summary>
	/// Sets the color of the text according to the traditional HTML format parameter value.
	/// </summary>
	/// <param name="message">Message</param>
	/// <param name="color">Color</param>
	public static string Colored(this string message, string colorCode)
	{
		return string.Format("<color={0}>{1}</color>", colorCode, message);
	}

	/// <summary>
	/// Sets the size of the text according to the parameter value, given in pixels.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="size">Size.</param>
	public static string Sized(this string message, int size)
	{
		return string.Format("<size={0}>{1}</size>", size, message);
	}

	/// <summary>
	/// Renders the text in boldface.
	/// </summary>
	/// <param name="message">Message.</param>
	public static string Bold(this string message)
	{
		return string.Format("<b>{0}</b>", message);
	}

    /// <summary>
    /// Renders the text in italics.
    /// </summary>
    /// <param name="message">Message.</param>
    public static string Italics(this string message)
    {
        return string.Format("<i>{0}</i>", message);
    }

	/// <summary>
	/// 移除首个字符
	/// </summary>
	public static string RemoveFirstChar(this string str)
	{
		if (string.IsNullOrEmpty(str))
			return str;
		return str.Substring(1);
	}

	/// <summary>
	/// 移除末尾字符
	/// </summary>
	public static string RemoveLastChar(this string str)
	{
		if (string.IsNullOrEmpty(str))
			return str;
		return str.Substring(0, str.Length - 1);
	}
}

public enum Colors
{
	aqua,
	black,
	blue,
	brown,
	cyan,
	darkblue,
	fuchsia,
	green,
	grey,
	lightblue,
	lime,
	magenta,
	maroon,
	navy,
	olive,
	purple,
	red,
	silver,
	teal,
	white,
	yellow
}

