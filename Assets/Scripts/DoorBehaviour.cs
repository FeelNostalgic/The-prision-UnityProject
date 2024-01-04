using System;
using DG.Tweening;
using Proyecto.Manager;
using UnityEngine;

namespace Proyecto.Behaviour
{
    public class DoorBehaviour : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private GameObject DoorPositiveX;
        [SerializeField] private GameObject DoorNegativeX;

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(DoorPositiveX.transform.DOLocalMoveX(1.6f,
                        1)
                    .SetEase(Ease.Linear));
            sq.Join( DoorNegativeX.transform.DOLocalMoveX(-1.6f,
                        1)
                    .SetEase(Ease.Linear));

            sq.Play().OnPlay(() => AudioManager.Instance.PlayClip(AudioManager.SFX_Type.doorSoundOpen));
        }

        private void OnTriggerExit(Collider other)
        {
            Sequence sq = DOTween.Sequence();
            sq.Append(DoorPositiveX.transform.DOLocalMoveX(0.5f,
                    1)
                .SetEase(Ease.Linear));
            sq.Join(DoorNegativeX.transform.DOLocalMoveX(-0.5f,
                        1)
                    .SetEase(Ease.Linear));

            sq.Play().OnPlay(() => AudioManager.Instance.PlayClip(AudioManager.SFX_Type.doorSoundClose));

        }
    }
}