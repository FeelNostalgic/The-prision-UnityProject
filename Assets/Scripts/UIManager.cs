using System;
using DG.Tweening;
using Proyecto.Controller;
using Proyecto.Manager;
using Proyecto.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : Singlenton<UIManager>
    {
        #region Inspector Variables

        [SerializeField] private Image blackScreen;
        
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject endPanel;

        #endregion

        #region Public Variables

        public bool Paused { get; private set; }
        public bool Started { get; private set; }
        
        public bool GameEnded { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            pausePanel!.SetActive(false);
            endPanel!.SetActive(false);
            
            blackScreen.DOFade(0f, 1.5f).SetEase(Ease.InSine)
                .OnPlay(() =>
                {
                    blackScreen.gameObject.SetActive(true);
                    SetupManager.Instance.InitializePlayer();
                })
                .OnComplete(() =>
                {
                    Started = true;
                    SetupManager.Instance.StartGame();
                    AudioManager.Instance.StartMusic();
                })
                .Play();
        }
        
        private void Update()
        {
            if (GameEnded) return;
            if (!Started) return;
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            PauseGame();
        }

        #endregion

        #region Public Methods

        public void HidePause()
        {
            ClickSound();
            PauseGame();
        }

        public void FadeBlackScreen()
        {
            blackScreen.DOFade(1f, 3f).SetEase(Ease.InSine)
                .OnPlay(() =>
                {
                    blackScreen.gameObject.SetActive(true);
                })
                .OnComplete(() =>
                {
                    var player = GameObject.FindGameObjectWithTag("Player");
                    player.GetComponent<FirstPersonController>().enabled = false;
                    player.GetComponent<Rigidbody>().useGravity = false;
                    AudioManager.Instance.StopAudio();
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    endPanel.SetActive(true);
                    GameEnded = true;
                    
                })
                .Play();
        }
        
        public void ExitToMainMenu()
        {
            ClickSound();
            MySceneManager.LoadScene(MySceneManager.Scenes.MainMenu);
        }

        #endregion

        #region Private Methods

        private void PauseGame()
        {
            Paused = !Paused;
            Time.timeScale = Paused ? 0 : 1;
            pausePanel.SetActive(Paused);
            Cursor.visible = Paused;
            Cursor.lockState = Paused ? CursorLockMode.None : CursorLockMode.Locked;
            AudioManager.Instance.PauseClip(Paused);
        }
        
        private static void ClickSound()
        {
            AudioManager.Instance.PlayClip(AudioManager.SFX_Type.ClickSound);
        }
        
        #endregion
    }
}