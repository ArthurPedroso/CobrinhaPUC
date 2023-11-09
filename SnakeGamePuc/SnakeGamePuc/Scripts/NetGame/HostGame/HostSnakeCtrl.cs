using GameEngine;
using GameEngine.Components;
using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame.HostGame
{
    internal class HostSnakeCtrl : MPlayerSnakeCtrl
    {
        public HostGameCtrl GameCtrl { private get; set; }
        public HostSnakeCtrl(GameObject _attachedGameObject) : base(_attachedGameObject, new Vector2(0.0f, -5.0f))
        {
        }
        public override void Start()
        {
            base.Start();
            AttachedGameObject.GetComponent<Collider>().RegisterOnCollisionEnterCB(OnCollision);
        }

        protected override void OnCollision(Collider _collider)
        {
            base.OnCollision(_collider);
            if (_collider.AttachedGameObject.Name == "Apple")
            {
                EatApple();
                GameCtrl.OnHostApple();
            }
            else
            {
                GameCtrl.OnHostDeath();
                GameInstance.SceneMan.LoadScene("LoseScene");
            }
        }
    }
}
