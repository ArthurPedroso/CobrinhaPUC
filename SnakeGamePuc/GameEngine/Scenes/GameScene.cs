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

        public string SceneName { get; private set; }
        public IReadOnlyCollection<GameObject> SceneObjs { get => m_sceneObjs; }
        public GameScene(string _name, GameObject[] _objs) 
        {
            m_sceneObjs = new HashSet<GameObject>(_objs);
            SceneName = _name;
        }
        public GameScene(string _name)
        {
            m_sceneObjs = new HashSet<GameObject>();
            SceneName = _name;
        }

        public void AddObj(GameObject _obj) => m_sceneObjs.Add(_obj);
        public void RemoveObj(GameObject _obj) => m_sceneObjs.Remove(_obj);
    }
}
