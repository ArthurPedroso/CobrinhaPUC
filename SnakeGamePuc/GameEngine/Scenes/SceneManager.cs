using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Scenes
{
    public class SceneManager
    {
        private GameScene[] m_gameScenes;
        private Dictionary<string, GameScene> m_nameToScene;
        public GameScene CurrentScene { get; private set; }
        public SceneManager(GameScene[] _gameScenes, string _firstSceneToLoad) 
        {
            m_gameScenes = _gameScenes;
            m_nameToScene = new Dictionary<string, GameScene>(_gameScenes.Length);
            foreach (GameScene scene in _gameScenes) m_nameToScene.Add(scene.SceneName, scene);
        }

        public bool LoadScene(string _name)
        {
            if (m_nameToScene.ContainsKey(_name))
            {
                CurrentScene = m_nameToScene[_name];
                return true;
            }
            else { return false; }
        }
    }
}
