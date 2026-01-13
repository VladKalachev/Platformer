using UnityEngine;

namespace PixelCrew.Utils
{
    public class AudioUtils
    {
        public const string  SfxSourceTag = "SfxAudioSource";

        public static AudioSource FindSfxSound()
        {
            return GameObject.FindWithTag(SfxSourceTag).GetComponent<AudioSource>();    
        }
    }
}