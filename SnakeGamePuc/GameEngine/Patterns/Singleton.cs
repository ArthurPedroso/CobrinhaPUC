using GameEngine.Exceptions;

namespace GameEngine.Patterns
{
    public abstract class Singleton<SingletonType> where SingletonType : Singleton<SingletonType>
    {
        private static SingletonType s_instance;

        public static SingletonType Instance
        {
            get
            {
                if (s_instance == null)
                {
                    throw new GameEngineException("Nenhuma Instancia encontrada!");
                }
                return s_instance;
            }
        }

        public Singleton()
        {
            if (s_instance != null)
            {
                throw new GameEngineException("More than one instance found!!!");
                return;
            }
            s_instance = (SingletonType)this;
        }
    }
}
