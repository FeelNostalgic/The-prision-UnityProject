using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private GameObject controlsPanel;
        [SerializeField] private Image blackScreen;
        
        #endregion
        
        #region Unity Methods

        private void Start()
        {
            controlsPanel.SetActive(false);
            Time.timeScale = 1;
        }

        #endregion

        #region Public Methods

        public void StartGame()
        {
            ClickSound();
            blackScreen.DOFade(1f, 1.5f).SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    MySceneManager.LoadScene(MySceneManager.Scenes.GameScene);
                })
                .Play();
        }

        public void ShowControls()
        {
            ClickSound();
            controlsPanel.SetActive(true);
        }

        public void HideControls()
        {
            ClickSound();

            controlsPanel.SetActive(false);
        }

        public void ExitToMainMenu()
        {
            ClickSound();
            MySceneManager.LoadScene(MySceneManager.Scenes.MainMenu);
        }

        public void ExitGame()
        {
            ClickSound();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }

        #endregion

        #region Private Methods

        private static void ClickSound()
        {
            AudioManager.Instance.PlayClip(AudioManager.SFX_Type.ClickSound);
        }

        #endregion
    }
}