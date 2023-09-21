using GameEngine.Components.Sprites;
using GameEngine.GEMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components.Scripts
{
    public class UI : Script
    {
        private Transform m_transform;
        private GameObject[] m_uiTexts;
        private string m_text;
        public string UiText { get => m_text; set { m_text = value; UpdateUi(); } }
        public bool CenterText { get; set; }
        public UI(GameObject _attachedGameObject, string _text, bool _centerText = true) : base(_attachedGameObject)
        {
            m_text = _text;
            CenterText = _centerText;
        }

        private void UpdateUi()
        {
            if (m_uiTexts != null && m_uiTexts.Length > 0)
            {
                foreach (GameObject obj in m_uiTexts)
                {
                    if(obj != null) GameInstance.RemoveObj(obj);
                }
            }
            GameObject newUiObj;
            Vector2 posOffset = CenterText ? new Vector2(-((m_text.Length - 1) / 2), 0.0f) : Vector2.Zero;
            m_uiTexts = new GameObject[m_text.Length];

            for (int i = 0; i < m_uiTexts.Length; i++)
            {
                newUiObj = new GameObject(m_text[i].ToString());
                newUiObj.AttachComponent(new Transform(newUiObj));
                newUiObj.AttachComponent(new ASCIISprite(newUiObj, m_text[i]));

                newUiObj.GetComponent<Transform>().Position = m_transform.Position + posOffset + (Vector2.Right * i);

                GameInstance.Instantiate(newUiObj);
            }
        }

        public override void Start()
        {
            m_transform = AttachedGameObject.GetComponent<Transform>();
            UpdateUi();
        }

        public override void Update()
        {
        }
    }
}
