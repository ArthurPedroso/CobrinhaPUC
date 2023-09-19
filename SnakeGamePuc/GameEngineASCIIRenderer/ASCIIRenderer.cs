using GameEngine.Components.Sprites;
using GameEngine.Rendering;
using System.Diagnostics;
using System.Windows.Controls;

namespace GameEngineASCIIRenderer
{
    public class ASCIIRenderer : IRenderer
    {
        private const char k_defaultSprite = '.';
        private readonly float r_targetFrameRenderTime;

        //Local Thread Read Write
        private Thread m_renderThread;
        private List<AsciiFrag> m_interThreadSpriteBuffer;
        private Stopwatch m_timer = new Stopwatch();

        //Game Thread Write - Render Thread Read
        private bool m_run;

        //Game Thread Read - Render Thread Write
        public float DeltaTime { get; private set; }
        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }

        public ASCIIRenderer(int _width, int _height, int _fps)
        {
            WindowWidth = _width;
            WindowHeight = _height;
            m_run = false;
            r_targetFrameRenderTime = 1000 / _fps;
            m_interThreadSpriteBuffer = new List<AsciiFrag>();
            m_renderThread = new Thread(new ThreadStart(RenderThreadLoop));
        }

        private void LoadSpriteBuffer(ImageSprite[]? _imageSprites, ASCIISprite[] _asciiSprites)
        {
            lock (m_interThreadSpriteBuffer) 
            {
                m_interThreadSpriteBuffer.Clear();
                if (_imageSprites != null)
                {
                    throw new NotImplementedException();
                }
                if (_asciiSprites != null)
                {
                    foreach (ASCIISprite sprite in _asciiSprites) m_interThreadSpriteBuffer.Add(new AsciiFrag(sprite));
                }                    
            }
        }
        private bool GameCoordsToFrameCoords(float _x, float _y, out int _result)
        {
            int y = (int)Math.Round((_y + (WindowHeight / 2.0f)) * -1, 0);
            int x = (int)Math.Round(_x + (WindowWidth / 2.0f), 0);

            if (y >= 0 && y < WindowHeight && x >= 0 && x < WindowWidth)
            {
                _result = (y * WindowWidth) + x;
                return true;
            }
            else
            {
                _result = 0;
                return false;
            }

        }
        private void InitFrame(char[] _frame)
        {
            for (int i = 0, y = 0; i < _frame.Length; i++)
            {
                if (i == _frame.Length - 1) _frame[i] = '\0';
                else if (i == ((WindowWidth + 1) * y) + WindowWidth) { _frame[i] = '\n'; y++; }
                else _frame[i] = k_defaultSprite;
            }
        }
        private void ClearFrame(char[] _frame)
        {
            for (int i = 0, y = 0; i < _frame.Length; i++)
            {
                if (i == ((WindowWidth + 1) * y) + WindowWidth) y++;
                else if (i != (_frame.Length - 1))
                {
                    _frame[i] = k_defaultSprite;
                }
            }
        }
        private void RenderThreadLoop()
        {
            m_run = true;
            AsciiFrag[] localSpriteBuffer;
            char[] frame = new char[(WindowHeight * WindowWidth) + WindowHeight + 1];
            int index;

            InitFrame(frame);
            while (m_run)
            {
                m_timer.Restart();

                Console.Clear();
                ClearFrame(frame);
                lock (m_interThreadSpriteBuffer) { localSpriteBuffer = m_interThreadSpriteBuffer.ToArray(); }

                foreach (AsciiFrag frag in localSpriteBuffer)
                {
                    if (GameCoordsToFrameCoords(frag.ModelMatrix.GetElement(2, 0), 
                                                frag.ModelMatrix.GetElement(2, 1), 
                                                out index))
                    {
                        frame[index] = frag.Value;
                    }
                }

                Console.WriteLine(frame);

                while (m_timer.ElapsedMilliseconds < r_targetFrameRenderTime)
                { 

                }
            }
        }

        public void RenderSprites(ImageSprite[] _imageSprites)
        {
            throw new NotImplementedException();
        }

        public void RenderSprites(ASCIISprite[] _asciiSprites)
        {
            LoadSpriteBuffer(null, _asciiSprites);
        }

        public void RenderSprites(ImageSprite[] _imageSprites, ASCIISprite[] _asciiSprites)
        {
            throw new NotImplementedException();
        }

        public void StartRenderLoop()
        {
            if (!m_run)
            {
                m_renderThread.Start();   
            }
        }

        public void StopRenderLoop()
        {
            if (m_run)
            {
                m_run = false;
                m_renderThread.Join();
            }
        }

        //public void RegisterOnFrameRendered(Action _onFrameRender)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
