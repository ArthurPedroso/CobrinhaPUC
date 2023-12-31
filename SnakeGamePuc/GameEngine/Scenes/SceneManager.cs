﻿using System;
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
        private Action<GameScene> m_onSceneLoadCB;
        public GameScene CurrentScene { get; private set; }
        public SceneManager(GameScene[] _gameScenes, string _firstSceneToLoad, Action<GameScene> _onNewSceneLoad) 
        {
            m_gameScenes = _gameScenes;
            m_nameToScene = new Dictionary<string, GameScene>(_gameScenes.Length);
            foreach (GameScene scene in _gameScenes) m_nameToScene.Add(scene.SceneName, scene);
            m_onSceneLoadCB = _onNewSceneLoad;
            LoadScene(_firstSceneToLoad);
        }

        public bool LoadScene(string _name)
        {
            if (m_nameToScene.ContainsKey(_name))
            {
                CurrentScene = m_nameToScene[_name];
                CurrentScene.LoadScene();
                m_onSceneLoadCB(CurrentScene);
                GameInstance.Debug.LogMsg("Scene " + _name + " loaded!");
                return true;
            }
            else { return false; }
        }
    }
}
