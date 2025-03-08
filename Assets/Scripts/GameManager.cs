using System;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private GameObject controlsPanel;
        
        #endregion

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        private void Start()
        {
            controlsPanel.SetActive(false);
        }

        #endregion

        #region Public Methods

        public void StartGame()
        {
            MySceneManager.LoadScene(MySceneManager.Scenes.GameScene);
        }

        public void ShowControls()
        {
            controlsPanel.SetActive(true);
        }

        public void HideControls()
        {
            controlsPanel.SetActive(false);
        }

        public void ExitToMainMenu()
        {
            MySceneManager.LoadScene(MySceneManager.Scenes.MainMenu);
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }

        #endregion

        #region Private Methods

        #endregion
    }
}