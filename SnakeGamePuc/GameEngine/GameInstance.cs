using GameEngine.Components;
using GameEngine.Components.Sprites;
using GameEngine.Debug;
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
        private enum EngineState { FirstFrame, Running, Exiting }

        //Engine systems
        private GameEngineDebugger m_debugger;
        private InputSystem m_inputSystem;
        private Physics2D m_physics2D;
        private SceneManager m_sceneManager;
        private IRenderer m_renderer;

        //Engine state
        private EngineState m_engineState;
        private int m_frameCount;
        //Game components

        public static IDebugger Debug { get => Instance.m_debugger; }
        public static Physics2D Physics { get => Instance.m_physics2D; }
        public static SceneManager SceneMan { get => Instance.m_sceneManager; }
        public static InputSystem Input { get => Instance.m_inputSystem; }

        public GameInstance(IRenderer _renderer, GameScene[] _gameScenes, string _firstSceneToLoad)
        {
            m_debugger = new GameEngineDebugger();
            m_sceneManager = new SceneManager(_gameScenes, _firstSceneToLoad, OnSceneLoad);
            m_inputSystem = new InputSystem();
            m_physics2D = new Physics2D();
            m_renderer = _renderer;

            InitEngine();
            EngineLoop();
        }

        private void InitEngine()
        {
            m_debugger.StartDebugger();
            m_renderer.StartRenderLoop();
            m_inputSystem.StartInputLoop();
            m_engineState = EngineState.FirstFrame;
        }
        private void WaitForRenderThread()
        {
            bool renderThreadDone = false;
            while(!(m_renderer.CheckIfFrameFinishedRendering(out renderThreadDone) && renderThreadDone))
            { 
                Thread.Sleep(10);
            }
        }

        private void UpdateInput()
        {
            m_inputSystem.UpdateInputState();
        }
        private void UpdateScripts()
        {
            foreach (Script script in m_sceneManager.CurrentScene.SceneScripts)
            {
                if (m_engineState == EngineState.FirstFrame) script.Start();
                else script.Update();
            }
        }

        private void UpdatePhysics()
        {
            m_physics2D.CalculateCollisions(m_sceneManager.CurrentScene.SceneColliders.ToArray());
        }

        private void ShipSpritesToRenderer()
        {
            m_renderer.RenderSprites(m_sceneManager.CurrentScene.SceneSprites.ToArray());
        }

        private void EngineLoop()
        {
            while (m_engineState != EngineState.Exiting)
            {
                UpdateInput();
                UpdatePhysics();
                UpdateScripts();
                ShipSpritesToRenderer();
                WaitForRenderThread();
                if (m_engineState == EngineState.FirstFrame) m_engineState = EngineState.Running;
            }
        }
        private void StopEngine()
        {
            m_debugger.StopDebugger();
            m_renderer.StopRenderLoop();
            m_inputSystem.StopInputLoop();
            m_engineState = EngineState.Exiting;
        }

        private void OnSceneLoad(GameScene _scene)
        {
            m_engineState = EngineState.FirstFrame;
        }
        public static void Instantiate(GameObject _obj)
        {
            Instance.m_sceneManager.CurrentScene.AddObj(_obj);
        }
        public static void QuitGame() { Instance.StopEngine(); }
    }
}