using GameEngine;
using GameEngine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGamePuc.Scripts.NetGame
{
    internal class ShadowSnakeCtrl : Script
    {
        GameObject[] m_body;
        Transform m_headTr;
        public ShadowSnakeCtrl(GameObject _attachedGameObject) : base(_attachedGameObject)
        {
        }

        public override void Start()
        {
        }

        public override void Update()
        {
        }

        public virtual void SetShadow(byte[] _serializedSnake)
        {

        }
    }
}
