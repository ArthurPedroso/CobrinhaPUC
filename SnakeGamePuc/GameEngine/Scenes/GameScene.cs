using GameEngine.Components;
using GameEngine.Components.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Scenes
{
    public class GameScene
    {
        private HashSet<GameObject> m_sceneObjs;
        private HashSet<Script> m_sceneScripts;
        private HashSet<Collider> m_sceneColliders;
        private HashSet<ASCIISprite> m_sceneSprites;

        public string SceneName { get; private set; }
        internal IReadOnlyCollection<GameObject> SceneObjs { get => m_sceneObjs; }
        internal IReadOnlyCollection<Script> SceneScripts { get => m_sceneScripts; }
        internal IReadOnlyCollection<Collider> SceneColliders { get => m_sceneColliders; }
        internal IReadOnlyCollection<ASCIISprite> SceneSprites { get => m_sceneSprites; }
        public GameScene(string _name, GameObject[] _objs) 
        {
            m_sceneObjs = new HashSet<GameObject>(_objs);
            m_sceneScripts = new HashSet<Script>();
            m_sceneColliders = new HashSet<Collider>();
            m_sceneSprites = new HashSet<ASCIISprite>();
            SceneName = _name;
            FindComponents();
        }
        public GameScene(string _name)
        {
            m_sceneObjs = new HashSet<GameObject>();
            m_sceneScripts = new HashSet<Script>();
            m_sceneColliders = new HashSet<Collider>();
            m_sceneSprites = new HashSet<ASCIISprite>();
            SceneName = _name;
            FindComponents();
        }

        private void FindComponents()
        {
            foreach (GameObject obj in m_sceneObjs)
            {
                m_sceneScripts.UnionWith(obj.GetComponents<Script>());
                m_sceneColliders.UnionWith(obj.GetComponents<Collider>());
                m_sceneSprites.UnionWith(obj.GetComponents<ASCIISprite>());
            }
        }

        public void AddObj(GameObject _obj)
        {
            m_sceneObjs.Add(_obj);
            m_sceneScripts.UnionWith(_obj.GetComponents<Script>());
            m_sceneColliders.UnionWith(_obj.GetComponents<Collider>());
            m_sceneSprites.UnionWith(_obj.GetComponents<ASCIISprite>());
        }
        public void RemoveObj(GameObject _obj)
        {
            m_sceneObjs.Remove(_obj);
            foreach(Script script in _obj.GetComponents<Script>()) m_sceneScripts.Remove(script);
            foreach (Collider collider in _obj.GetComponents<Collider>()) m_sceneColliders.Remove(collider);
            foreach (ASCIISprite sprite in _obj.GetComponents<ASCIISprite>()) m_sceneSprites.Remove(sprite);
        }
    }
}
