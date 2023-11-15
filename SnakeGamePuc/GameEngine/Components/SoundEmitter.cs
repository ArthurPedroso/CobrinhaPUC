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
        readonly bool r_loop;
        public SoundEmitter(GameObject _attachedGameObject, Stream? _sound, bool _loop = false) : base(_attachedGameObject)
        {
            m_sound = new SoundPlayer(_sound);
            r_loop = _loop;
        }

        public void Play()
        { 
            if(r_loop) m_sound.PlayLooping();
            else m_sound.Play();
        }
        public void Stop() 
        {
            m_sound.Stop();
        }
    }
}
