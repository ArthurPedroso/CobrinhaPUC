using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Components
{
    public class SoundEmitter : Component
    {
        SoundPlayer m_sound;
        public SoundEmitter(GameObject _attachedGameObject, Stream? _sound) : base(_attachedGameObject)
        {
            m_sound = new SoundPlayer(_sound);
        }

        public void Play()
        { 
            m_sound.Play();
        }
    }
}
