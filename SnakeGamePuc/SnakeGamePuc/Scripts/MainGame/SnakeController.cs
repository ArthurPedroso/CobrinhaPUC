﻿using GameEngine;
using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Input;
using GameEngine.GEMath;
using GameEngine.Scenes;
using System.Net.Quic;

namespace SnakeGamePuc.Scripts
{
    internal class SnakeController : Script
    {
        private const float k_moveAfter = 0.3f;
        private const int k_startingSize = 4;

        private Transform m_transform;
        private SnakeDirection m_direction;
        private SnakeDirection m_lastDirection;
        private float m_elapsedTime;
        private SnakeBody m_snakeBody;
        private SoundEmitter m_onAppleEatSound;
        private SoundEmitter m_failureSound;
        private SoundEmitter m_winSound;

        public SnakeController(GameObject _attachedGameObject, SoundEmitter _soundEmitter, SoundEmitter _failureSound, SoundEmitter _winSound) : base(_attachedGameObject)
        {
            m_direction = SnakeDirection.Left;
            m_lastDirection = SnakeDirection.Left;
            m_onAppleEatSound = _soundEmitter;
            m_failureSound = _failureSound;
            m_winSound = _winSound;
        }

        public override void Start()
        {
            m_transform = AttachedGameObject.GetComponent<Transform>();
            AttachedGameObject.GetComponent<Collider>().RegisterOnCollisionEnterCB(OnCollision);
            m_elapsedTime = 0;
            BuildBodyParts();
        }

        public override void Update()
        {
            UpdateTimer();
            CheckInput();
        }
        private void EatApple()
        {
            m_snakeBody.AddBodyPiece();
            m_onAppleEatSound.Play();
        }
        private void Die()
        {
            m_failureSound.Play();
            GameInstance.SceneMan.LoadScene("LoseScene");
        }
        private void OnCollision(Collider _collider)
        {
            if (_collider.AttachedGameObject.Name == "Apple") EatApple();
            else Die();
        }
        private void CheckInput()
        {
            if (((GameInstance.Input.KeyPressed(InputKey.A) || GameInstance.Input.KeyPressed(InputKey.ArrowLeft)) 
                && m_direction != SnakeDirection.Right && m_lastDirection != SnakeDirection.Right))
                m_direction = SnakeDirection.Left;
            else if (((GameInstance.Input.KeyPressed(InputKey.W) || GameInstance.Input.KeyPressed(InputKey.ArrowUp)) 
                && m_direction != SnakeDirection.Down && m_lastDirection != SnakeDirection.Down))
                m_direction = SnakeDirection.Up;
            else if (((GameInstance.Input.KeyPressed(InputKey.D) || GameInstance.Input.KeyPressed(InputKey.ArrowRight)) 
                && m_direction != SnakeDirection.Left && m_lastDirection != SnakeDirection.Left))
                m_direction = SnakeDirection.Right;
            else if (((GameInstance.Input.KeyPressed(InputKey.S) || GameInstance.Input.KeyPressed(InputKey.ArrowDown)) 
                && m_direction != SnakeDirection.Up && m_lastDirection != SnakeDirection.Up))
                m_direction = SnakeDirection.Down;

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
            SnakeBody oldsnake = null;
            GameObject obj;
            SnakeBody snakeBody;
            for (int i = 0; i < k_startingSize; i++)
            {
                obj = ObjsBuilders.BuildSnakeBody();
                newPos += Vector2.Right;
                obj.GetComponent<Transform>().Position += newPos;
                snakeBody = obj.GetComponent<SnakeBody>();
                if(oldsnake != null) oldsnake.NextBodyPiece = snakeBody;
                GameInstance.Instantiate(obj);

                if (i == 0) m_snakeBody = snakeBody;
                oldsnake = snakeBody;
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
            m_lastDirection = m_direction;
            m_snakeBody.Move();
            m_snakeBody.Direction = m_direction;
        }

        public Vector2[] GetSnakePos()
        {
            List<Vector2> pos = new List<Vector2>();
            if (m_snakeBody != null)
            {
                pos.AddRange(m_snakeBody.BodyPositions());
            }
            pos.Add(m_transform.Position);

            return pos.ToArray();
        }
    }
}
