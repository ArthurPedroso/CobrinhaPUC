using GameEngine.Components;
using GameEngine.Components.Sprites;
using GameEngine.Input;
using GameEngine.Patterns;
using GameEngine.Physics;
using GameEngine.Rendering;
using GameEngine.Scenes;
using System.Diagnostics;
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

        public GameInstance(IRenderer _renderer)
        {
            m_physics2D = new Physics2D();
            m_sceneManager = new SceneManager();
            m_inputSystem = new InputSystem();
            m_renderer = _renderer;

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
            m_renderer.StartRenderLoop();
            EngineLoop();
        }
        private int engineLoopCounts = 0;
        private void EngineLoop()
        {
            GameObject obj = new GameObject();
            obj.AttachComponent(new Transform(obj, new GEMath.Matrix3x3()));
            obj.AttachComponent(new ASCIISprite(obj, 'X'));
            while (true)
            {
                m_renderer.RenderSprites(new ASCIISprite[] { obj.GetComponent<ASCIISprite>() });
                engineLoopCounts++;
            }
        }
        private void StopEngine()
        {
            m_renderer.StopRenderLoop();
        }

        public static void QuitGame() { Instance.StopEngine(); }
    }
}