using System;
using Proyecto.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Manager
{
    public class AudioManager : Singlenton<AudioManager>
    {
        #region Inspector Variables

        [FormerlySerializedAs("MainAudioSource")] [SerializeField] private AudioSource mainAudioSource;
        [FormerlySerializedAs("SfxAudioSource")] [SerializeField] private AudioSource sfxAudioSource;
        [FormerlySerializedAs("SfxDoorAudioSource")] [SerializeField] private AudioSource sfxDoorAudioSource;

        [FormerlySerializedAs("SFXClips")] [SerializeField] private AudioClip[] sfxClips;

        #endregion

        #region Public Variables

        public enum SFX_Type
        {
            TickSound,
            HydraulicSound,
            DoorSoundOpen,
            DoorSoundClose,
            ClickSound
        }

        #endregion

        #region Public Methods

        public void StartMusic()
        {
            mainAudioSource.Play();
        }
        
        public void PlayClip(SFX_Type type)
        {
            switch (type)
            {
                case SFX_Type.TickSound:
                case SFX_Type.HydraulicSound:
                    sfxAudioSource.pitch = Random.Range(1, 1.5f);
                    PlaySFXClip((int)type);
                    break;
                case SFX_Type.DoorSoundOpen:
                case SFX_Type.DoorSoundClose:
                    sfxDoorAudioSource.Stop();
                    sfxDoorAudioSource.pitch = Random.Range(1, 1.5f);
                    PlaySFXClip((int)type, true);
                    break;
                case SFX_Type.ClickSound:
                    sfxAudioSource.pitch = Random.Range(0.85f, 1.15f);
                    PlaySFXClip((int)type);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public float GetClipDuration(SFX_Type type)
        {
            return type switch
            {
                SFX_Type.TickSound => sfxClips[(int)type].length,
                SFX_Type.HydraulicSound => sfxClips[(int)type].length,
                SFX_Type.DoorSoundOpen => sfxClips[(int)type].length,
                SFX_Type.DoorSoundClose => sfxClips[(int)type].length,
                _ => 0
            };
        }

        public void PauseClip(bool isPaused)
        {
            if (isPaused)
            {
                sfxAudioSource.Pause();
                sfxDoorAudioSource.Pause();
                mainAudioSource.Pause();
            }
            else
            {
                sfxAudioSource.UnPause();
                sfxDoorAudioSource.UnPause();
                mainAudioSource.UnPause();
            }
        }

        public void StopAudio()
        {
            sfxAudioSource.Stop();
            sfxDoorAudioSource.Stop();
            mainAudioSource.Stop();
        }
        
        #endregion

        #region Private Methods

        private void PlaySFXClip(int clip, bool door = false)
        {
            if (door)
            {
                sfxDoorAudioSource.PlayOneShot(sfxClips[clip]);
            }
            else
            {
                sfxAudioSource.PlayOneShot(sfxClips[clip]);
            }
        }
        
        #endregion
    }
}