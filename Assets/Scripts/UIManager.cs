using System;
using Proyecto.Utilities;
using UnityEngine;

namespace Manager
{ 
	public class UIManager : Singlenton<UIManager>
	{
		#region Inspector Variables

		[SerializeField] private GameObject pausePanel;
		[SerializeField] private GameObject endPanel;
		
		#endregion
	
		#region Public Variables
		
		public bool Paused { get; private set; }
		
		#endregion
		
		#region Unity Methods

		private void Awake()
		{
			pausePanel!.SetActive(false);
			endPanel!.SetActive(false);
		}

		private void Update()
		{
			if (!Input.GetKeyDown(KeyCode.Escape)) return;
			Paused = !Paused;
			Time.timeScale = Paused ? 0 : 1;
			pausePanel.SetActive(Paused);
		}

		#endregion

		#region Public Methods
		
		public void HidePause()
		{
			pausePanel.SetActive(false);
			Paused = false;
		}
		
		public void ShowEndPanel()
		{
			throw new System.NotImplementedException();
		}
		
		#endregion
	}
}