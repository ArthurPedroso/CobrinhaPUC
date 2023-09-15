using GameEngine.Input;
using GameEngine.Patterns;
using GameEngine.Physics;
using GameEngine.Rendering;
using GameEngine.Scenes;

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

        public GameInstance()
        {
            m_physics2D = new Physics2D();
            m_sceneManager = new SceneManager();
            m_inputSystem = new InputSystem();
            //m_renderer = new GameEngineASCIIRE
            EngineLoop();
        }

        private void EngineLoop()
        {
            
        }
    }
}