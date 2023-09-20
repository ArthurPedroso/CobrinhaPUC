using GameEngine;
using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Input;
using GameEngine.GEMath;

namespace SnakeGamePuc.Scripts
{
    internal class SnakeController : Script
    {
        Transform m_transform;
        public SnakeController(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        public override void Start()
        {
            m_transform = AttachedGameObject.GetComponent<Transform>();
        }

        public override void Update()
        {
            if ((GameInstance.Input.KeysReleased & InputKey.A) != 0) 
                m_transform.Position += Vector2.Left;
            if ((GameInstance.Input.KeysReleased & InputKey.W) != 0) 
                m_transform.Position += Vector2.Up;
            if ((GameInstance.Input.KeysReleased & InputKey.D) != 0) 
                m_transform.Position += Vector2.Right;
            if ((GameInstance.Input.KeysReleased & InputKey.S) != 0)
                m_transform.Position += Vector2.Down;
        }
    }
}
