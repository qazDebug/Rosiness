
namespace Rosiness.Tween
{
	/// <summary>
	/// 执行节点
	/// </summary>
	public class ExecuteNode : ITweenNode
	{
		public static ExecuteNode Allocate(System.Action execute)
		{
			ExecuteNode node = new ExecuteNode
			{
				Execute = execute,
			};
			return node;
		}

		public bool IsDone { private set; get; } = false;
		public System.Action Execute { set; get; }

		void ITweenNode.OnUpdate()
		{
			Execute.Invoke();
			IsDone = true;
		}
		void ITweenNode.OnDispose()
		{
		}
		void ITweenNode.Kill()
		{
			IsDone = true;
		}
	}
}