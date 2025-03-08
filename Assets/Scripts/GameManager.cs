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
            Cursor.visible = true;
        }

        #endregion

        #region Public Methods

        public void StartGame()
        {
            
        }

        public void ShowControls()
        {
            
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