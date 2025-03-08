using System;
using DG.Tweening;
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

        #endregion

        #region Unity Methods

        private void Awake()
        {
            pausePanel!.SetActive(false);
            //endPanel!.SetActive(false);
        }

        private void Start()
        {
            blackScreen.DOFade(0f, 1.5f).SetEase(Ease.InSine)
                .OnPlay(() =>
                {
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

        public void ShowEndPanel()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void PauseGame()
        {
            Paused = !Paused;
            Time.timeScale = Paused ? 0 : 1;
            pausePanel.SetActive(Paused);
            Cursor.visible = Paused;
            Cursor.lockState = Paused ? CursorLockMode.Confined : CursorLockMode.Locked;
            AudioManager.Instance.PauseClip(Paused);
        }
        
        private static void ClickSound()
        {
            AudioManager.Instance.PlayClip(AudioManager.SFX_Type.ClickSound);
        }
        
        #endregion
    }
}