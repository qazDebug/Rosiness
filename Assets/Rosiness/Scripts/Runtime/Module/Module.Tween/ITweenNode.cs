
namespace Rosiness.Tween
{
	public interface ITweenNode
	{
		bool IsDone { get; }

		void OnUpdate();
		void OnDispose();
		void Kill();
	}
}