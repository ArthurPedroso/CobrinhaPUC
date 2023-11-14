using GameEngine;
using GameEngine.Components;
using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace SnakeGamePuc.Scripts.NetGame.HostGame
{
    internal class HostShadowSnake : ShadowSnakeCtrl
    {
        public HostGameCtrl GameCtrl { private get; set; }
        public HostShadowSnake(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }
        public override void Start()
        {
            base.Start();
            AttachedGameObject.GetComponent<Collider>().RegisterOnCollisionEnterCB(OnCollision);
        }

        private void OnCollision(Collider _col)
        {
            if (_col.AttachedGameObject.Name == "Apple")
            {
                GameCtrl.OnClientApple();
            }
            else
            {
                GameCtrl.OnClientDeath();
            }
        }

        public override void SetShadow(byte[] _serializedSnake)
        {
            if (_serializedSnake.Length % 2 != 0) throw new Exception("Wrong Data!");
            if (!m_ready) return;
            ClearOldBody();

            List<GameObject> objs = new List<GameObject>();


            Vector2 pos = new Vector2(_serializedSnake[_serializedSnake.Length - 2], _serializedSnake[_serializedSnake.Length - 1]);
            if (pos.X > 128) pos.X -= 256;
            if (pos.Y > 128) pos.Y -= 256;
            GameObject newObj;

            m_headTr.Position = pos;

            int i = 0;
            for (; i < _serializedSnake.Length - 2; i += 2)
            {
                for (int j = 0; j < _serializedSnake[i + 1]; j++)
                {
                    switch (_serializedSnake[i])
                    {
                        case 1: //left
                            pos += Vector2.Left;
                            break;
                        case 2: //right
                            pos += Vector2.Right;
                            break;
                        case 3: //up
                            pos += Vector2.Up;
                            break;
                        case 4: //down
                            pos += Vector2.Down;
                            break;
                        default:
                            break;
                    }

                    newObj = ObjsBuilders.BuildShadowSnakeBody();
                    newObj.GetComponent<Transform>().Position = pos;
                    newObj.AttachComponent(new Collider(newObj));
                    GameInstance.Instantiate(newObj);

                    objs.Add(newObj);
                }
            }

            m_body = objs.ToArray();
        }
    }
}
