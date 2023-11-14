using GameEngine;
using GameEngine.Scenes;

namespace SnakeGamePuc.Scenes
{
    internal class DisconnectScene : GameScene
    {
        public DisconnectScene() : base("DisconnectScene")
        {
        }

        public override void BuildScene()
        {
            foreach (GameObject obj in ObjsBuilders.BuildDisconnectScene())
            {
                AddObj(obj);
            }
        }
    }
}