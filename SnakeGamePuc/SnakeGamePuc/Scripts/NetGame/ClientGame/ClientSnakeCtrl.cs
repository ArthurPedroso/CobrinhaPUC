using GameEngine;
using GameEngine.Components;
using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame.ClientGame
{
    internal class ClientSnakeCtrl : MPlayerSnakeCtrl
    {
        public ClientSnakeCtrl(GameObject _attachedGameObject) : base(_attachedGameObject, new Vector2(0.0f, 5.0f))
        {
        }

        protected override void OnCollision(Collider _collider)
        {
        }
    }
}
