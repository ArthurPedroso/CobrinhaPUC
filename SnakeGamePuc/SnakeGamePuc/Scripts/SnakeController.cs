﻿using GameEngine;
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
        private const float k_moveAfter = 1;
        private const int k_startingSize = 4;

        private Transform m_transform;
        private SnakeDirection m_direction;
        private float m_elapsedTime;
        private SnakeBody m_snakeBody;

        public SnakeController(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
            m_direction = SnakeDirection.Right;
        }

        public override void Start()
        {
            m_transform = AttachedGameObject.GetComponent<Transform>();
            m_elapsedTime = 0;
            BuildBodyParts();
        }

        public override void Update()
        {
            UpdateTimer();
            if ((GameInstance.Input.KeysReleased & InputKey.A) != 0) 
                m_transform.Position += Vector2.Left;
            if ((GameInstance.Input.KeysReleased & InputKey.W) != 0) 
                m_transform.Position += Vector2.Up;
            if ((GameInstance.Input.KeysReleased & InputKey.D) != 0) 
                m_transform.Position += Vector2.Right;
            if ((GameInstance.Input.KeysReleased & InputKey.S) != 0)
                m_transform.Position += Vector2.Down;
        }
        private void UpdateTimer()
        {
            m_elapsedTime += GameInstance.Renderer.GetDeltaTime();
            if (m_elapsedTime >= k_moveAfter)
            {
                m_elapsedTime = 0;
                Move();
            }
        }
        private void BuildBodyParts()
        {
            Vector2 newPos = Vector2.Zero;
            GameObject obj;
            for (int i = 0; i < k_startingSize; i++)
            {
                obj = ObjsBuilders.BuildSnakeBody();
                newPos += Vector2.Right;
                obj.GetComponent<Transform>().Position += newPos;
                GameInstance.Instantiate(obj);
            }
        }

        private void Move()
        {
            switch (m_direction)
            {
                case SnakeDirection.Up:
                    m_transform.Position += Vector2.Up;
                    break;
                case SnakeDirection.Down:
                    m_transform.Position += Vector2.Down;
                    break;
                case SnakeDirection.Right:
                    m_transform.Position += Vector2.Right;
                    break;
                case SnakeDirection.Left:
                    m_transform.Position += Vector2.Left;
                    break;
            }
            m_snakeBody.Move();
            m_snakeBody.Direction = m_direction;
        }
    }
}
