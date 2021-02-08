/****************************************************
	文件：ModuleSingleton.cs
	作者：世界和平
	日期：2020/11/27 12:6:11
	功能：Nothing
*****************************************************/

namespace Rosiness
{
    public class ModuleSingleton<T> where T : class, IModule
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    RosinessLog.Error($"{typeof(T)} is not create. Use {nameof(RosinessEngine)}.{nameof(RosinessEngine.CreateModule)} create.");
                return instance;
            }
        }

        protected ModuleSingleton()
        {
            if (instance != null)
                throw new System.Exception($"{typeof(T)} instance already created.");
            instance = this as T;
        }
    }
}