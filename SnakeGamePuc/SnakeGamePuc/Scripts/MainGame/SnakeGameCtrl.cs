using GameEngine;
using GameEngine.Components;
using GameEngine.GEMath;
using GameEngine.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts
{
    internal class SnakeGameCtrl : Script
    {
        private List<Vector2> m_possibleSpawnPos;
        private int m_gameAreaX;
        private int m_gameAreaY;
        public SnakeController SnakeCtrl { private get; set; }
        public SnakeGameCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            m_possibleSpawnPos = new List<Vector2>();
        }

        private void InstantiateApple()
        {
            m_possibleSpawnPos.Clear();
            Vector2[] snakePos = SnakeCtrl.GetSnakePos();
            Vector2 possiblePos;
            bool posIsPossible;
            for (int y = 0; y < m_gameAreaY; y++)
            {
                possiblePos.Y = -(m_gameAreaY / 2.0f) + y;
                for (int x = 0; x < m_gameAreaX; x++)
                {
                    possiblePos.X = -(m_gameAreaX / 2.0f) + x;
                    posIsPossible = true;
                    foreach (Vector2 sPos in snakePos)
                    {
                        if ((sPos - possiblePos).Magnitude() <= 0.0001f)
                        {
                            posIsPossible = false;
                            break;
                        }
                    }
                    if (posIsPossible) m_possibleSpawnPos.Add(possiblePos);
                }
            }

            if (m_possibleSpawnPos.Count > 0)
            {
                GameObject apple = ObjsBuilders.BuildApple(InstantiateApple);
                Random random = new Random();

                apple.GetComponent<Transform>().Position = m_possibleSpawnPos[random.Next(m_possibleSpawnPos.Count)];

                GameInstance.Instantiate(apple);
            }
            else
            {
                GameInstance.SceneMan.LoadScene("MainMenu");
            }
        }

        public override void Start()
        {
            GameObject[] walls = ObjsBuilders.BuildWalls();
            foreach (GameObject wall in walls) { GameInstance.Instantiate(wall); }
            m_gameAreaX = GameInstance.Renderer.GetWidth() - 2;
            m_gameAreaY = GameInstance.Renderer.GetHeight() - 2;
            InstantiateApple();
        }

        public override void Update()
        {
            if (GameInstance.Input.KeyPressed(InputKey.Esc))
                GameInstance.QuitGame();
        }


    }
}
