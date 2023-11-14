using GameEngine.Components;
using GameEngine.Components.Sprites;
using GameEngine.Debug;
using GameEngine.Input;
using GameEngine.Net;
using GameEngine.Patterns;
using GameEngine.Physics;
using GameEngine.Rendering;
using GameEngine.Scenes;
using System.Diagnostics;
using System.Windows.Input;

namespace GameEngine
{
    public sealed class GameInstance : Singleton<GameInstance>
    {
        private enum EngineState { FirstFrame, Running, Exiting, ChangeScene }

        //Engine systems
        private GameEngineDebuggerBase m_debugger;
        private InputSystem m_inputSystem;
        private Physics2D m_physics2D;
        private SceneManager m_sceneManager;
        private IRenderer m_renderer;

        //Engine state
        private EngineState m_engineState;
        private int m_frameCount;
        private TcpHost m_tcpHost;
        private TcpClient m_tcpClient;
        private UdpReceive m_udpHost;
        private UdpSend m_udpClient;

        //Game components

        public static IDebugger Debug { get => Instance.m_debugger; }
        public static Physics2D Physics { get => Instance.m_physics2D; }
        public static SceneManager SceneMan { get => Instance.m_sceneManager; }
        public static InputSystem Input { get => Instance.m_inputSystem; }
        public static IRenderer Renderer { get => Instance.m_renderer; }
        public static TcpHost HostTCP { get => Instance.m_tcpHost; }
        public static TcpClient ClientTCP { get => Instance.m_tcpClient; }
        public static UdpReceive UDPReceive { get => Instance.m_udpHost; }
        public static UdpSend UDPSend { get => Instance.m_udpClient; }

        public GameInstance(IRenderer _renderer, GameScene[] _gameScenes, string _firstSceneToLoad, bool _debbug = false)
        {
            m_debugger = _debbug ? new GameEngineDebugger() : new MockDebugger();
            m_tcpHost = new TcpHost();
            m_tcpClient = new TcpClient();
            m_udpHost = new UdpReceive();
            m_udpClient = new UdpSend();
            m_sceneManager = new SceneManager(_gameScenes, _firstSceneToLoad, OnSceneLoad);
            m_inputSystem = new InputSystem();
            m_physics2D = new Physics2D();
            m_renderer = _renderer;

            InitEngine();
            EngineLoop();
        }

        private void InitEngine()
        {
            m_debugger.StartModuleThread();
            m_renderer.StartRenderLoop();
            m_inputSystem.StartModuleThread();
            m_tcpHost.StartModuleThread();
            m_tcpClient.StartModuleThread();
            m_udpHost.StartModuleThread();
            m_udpClient.StartModuleThread();
            m_engineState = EngineState.FirstFrame;
        }
        private void WaitForRenderThread()
        {
            bool renderThreadDone = false;
            while (!(m_renderer.CheckIfFrameFinishedRendering(out renderThreadDone) && renderThreadDone)) ;
        }

        private void UpdateInput()
        {
            m_inputSystem.UpdateInputState();
        }
        private void UpdateScripts()
        {
            Script[] scripts = m_sceneManager.CurrentScene.SceneScripts.ToArray();
            foreach (Script script in scripts)
            {
                if (m_engineState == EngineState.FirstFrame) script.Start();
                else if(m_engineState == EngineState.Running) script.Update();
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
                ShipSpritesToRenderer();
                UpdateInput();
                UpdatePhysics();
                UpdateScripts();
                WaitForRenderThread();
                if (m_engineState == EngineState.FirstFrame)
                    m_engineState = EngineState.Running;
                else if (m_engineState == EngineState.ChangeScene)
                    m_engineState = EngineState.FirstFrame;
            }
        }
        private void StopEngine()
        {
            m_debugger.StopModuleThread();
            m_renderer.StopRenderLoop();
            m_inputSystem.StopModuleThread();
            m_tcpHost.StopModuleThread();
            m_tcpClient.StopModuleThread();
            m_udpHost.StopModuleThread();
            m_udpClient.StopModuleThread();
            m_engineState = EngineState.Exiting;
        }

        private void OnSceneLoad(GameScene _scene)
        {
            m_engineState = EngineState.ChangeScene;
        }
        public static void Instantiate(GameObject _obj)
        {
            Instance.m_sceneManager.CurrentScene.AddObj(_obj);
            Script script = _obj.GetComponent<Script>();
            if (script != null) 
                script.Start();
        }
        public static void RemoveObj(GameObject _obj)
        {
            Instance.m_sceneManager.CurrentScene.RemoveObj(_obj);
        }
        public static void QuitGame() { Instance.StopEngine(); }

    }
}