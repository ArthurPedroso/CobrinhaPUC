using GameEngine;
using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts
{
    internal class Apple : Script
    {
        private Transform m_transform;
        private Action m_onAppleEaten;

        public Apple(GameObject _attachedGameObject, Action _onAppleEaten) : base(_attachedGameObject)
        {
            m_onAppleEaten = _onAppleEaten;
        }
        private void OnCollision(Collider _collider)
        {
            GameInstance.RemoveObj(AttachedGameObject);
            m_onAppleEaten();
        }

        public override void Start()
        {
            m_transform = AttachedGameObject.GetComponent<Transform>();
            AttachedGameObject.GetComponent<Collider>().RegisterOnCollisionEnterCB(OnCollision);
        }


        public override void Update()
        {
        }
    }
}
