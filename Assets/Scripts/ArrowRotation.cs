
using System;
using DG.Tweening;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arrow
{ 
	public class ArrowRotation : MonoBehaviour
	{
		#region Public Variables
		
		#endregion

		#region Private Variables

		private const int SPIN_DURATION = 4;
		
		#endregion

		#region Unity Methods

		private void Start()
		{
			StartTween();
		}

		#endregion

		#region Private Methods

		private void StartTween()
		{
			const float pieceAngle = 360f / 8;
			const float halfPieceAngle = pieceAngle / 2;
			var targetAngle = Random.Range(0,8) * pieceAngle;
			var rightOffset = (targetAngle + halfPieceAngle) % 360;
			var leftOffset = (targetAngle - halfPieceAngle) % 360;

			var randomAngle = Random.Range(leftOffset, rightOffset);
			var targetRotation = Vector3.up * (randomAngle + 1 * 360 * SPIN_DURATION);
			transform.DORotate(targetRotation, SPIN_DURATION, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart)
				.OnComplete(StartTween)
				.Play();
		}
		
		#endregion
	}
}