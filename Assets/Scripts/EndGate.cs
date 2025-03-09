using DG.Tweening;
using Manager;
using UnityEngine;

namespace Gate
{ 
	public class EndGate : MonoBehaviour
	{
		#region Inspector Variables

		[SerializeField] private GameObject gatePositiveZ;
		[SerializeField] private GameObject gateNegativeZ;
		
		#endregion
		
		#region Public Methods

		public void OpenGate()
		{
			var sq = DOTween.Sequence();
			sq.Append(gatePositiveZ.transform.DOLocalMoveZ(4f, 8f).SetEase(Ease.Linear))
				.Join(gateNegativeZ.transform.DOLocalMoveZ(-4f, 8f).SetEase(Ease.Linear))
				.OnPlay(() => AudioManager.Instance.PlayClip(AudioManager.SFX_Type.HydraulicSound))
				.Play();
		}
		
		#endregion
	}
}