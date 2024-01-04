using Proyecto.Utilities;
using UnityEngine;

namespace Proyecto.Manager
{
    public class AudioManager : Singlenton<AudioManager>
    {
        #region Inspector Variables
        
        [SerializeField] private AudioSource MainAudioSource;
        [SerializeField] private AudioSource SfxAudioSource;
        [SerializeField] private AudioSource SfxDoorAudioSource;

        [SerializeField] private AudioClip[] SFXClips;

        #endregion

        #region Public Variables

        public enum SFX_Type
        {
            tickSound, hidraulicSound, doorSoundOpen, doorSoundClose
        }
        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        #endregion  

        #region Public Methods

        public void PlayClip(SFX_Type type)
        {
            switch (type)
            {
                case SFX_Type.tickSound:
                    SfxAudioSource.pitch = Random.Range(1, 1.5f);
                    PlaySFXClip((int) type);
                    break;
                case SFX_Type.hidraulicSound:
                    SfxAudioSource.pitch = Random.Range(1, 1.5f);
                    PlaySFXClip((int) type);
                    break;
                case SFX_Type.doorSoundOpen:
                    SfxDoorAudioSource.Stop();
                    SfxDoorAudioSource.pitch = Random.Range(1, 1.5f);
                    PlaySFXClip((int) type, true);
                    break;
                case SFX_Type.doorSoundClose:
                    SfxDoorAudioSource.Stop();
                    SfxDoorAudioSource.pitch = Random.Range(1, 1.5f);
                    PlaySFXClip((int) type,true);
                    break;
            }
        }

        public float getClipDuration(SFX_Type type)
        {
            switch (type)
            {
                case SFX_Type.tickSound:
                    return SFXClips[(int)type].length;
                case SFX_Type.hidraulicSound:
                    return SFXClips[(int)type].length;
                case SFX_Type.doorSoundOpen:
                    return SFXClips[(int)type].length;
                case SFX_Type.doorSoundClose:
                    return SFXClips[(int)type].length;
            }

            return 0;
        }

        #endregion

        #region Private Methods
        private void PlaySFXClip(int clip)
        {
            SfxAudioSource.PlayOneShot(SFXClips[clip]);
        } 
        private void PlaySFXClip(int clip, bool door)
        {
            SfxDoorAudioSource.PlayOneShot(SFXClips[clip]);
        }
        
        #endregion
    }
}