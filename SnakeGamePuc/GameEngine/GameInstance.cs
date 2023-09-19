using GameEngine.Input;
using GameEngine.Patterns;
using GameEngine.Physics;
using GameEngine.Rendering;
using GameEngine.Scenes;
using System.Windows.Input;

namespace GameEngine
{
    public class GameInstance : Singleton<GameInstance>
    {
        private InputSystem m_inputSystem;
        private Physics2D m_physics2D;
        private SceneManager m_sceneManager;
        private IRenderer m_renderer;

        public static Physics2D Physics { get => Instance.m_physics2D; }
        public static SceneManager SceneMan { get => Instance.m_sceneManager; }
        public static InputSystem Input { get => Instance.m_inputSystem; }

        public GameInstance()
        {
            m_physics2D = new Physics2D();
            m_sceneManager = new SceneManager();
            m_inputSystem = new InputSystem();
            //m_renderer = new GameEngineASCIIRE
            InitEngine();
            EngineLoop();

            Application.Run();
        }

        private void InitEngine()
        {
            string message;
            var hookId = Input.RegisterInput(
                new List<Key> {
            Key.A,
            Key.B
                },
                () =>
                {
                    Console.WriteLine("a-b");
                },
                out message);
        }
        private void EngineLoop()
        {
            
        }
        public class Program
        {
            [STAThread]
            static void Main()
            {
                GameInstance instance = new GameInstance();
            }
        }
    }
}