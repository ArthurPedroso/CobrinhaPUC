using GameEngine;
using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                GameCtrl.OnHostApple();
            }
            else
            {
                GameCtrl.OnClientDeath();

            }
        }
    }
}
