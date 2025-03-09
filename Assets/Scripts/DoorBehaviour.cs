using System;
using DG.Tweening;
using Manager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Door
{
    public class DoorBehaviour : MonoBehaviour
    {
        #region Inspector Variables

        [SerializeField] private GameObject doorPositiveX;
        [SerializeField] private GameObject doorNegativeX;

        #endregion

        #region private Variables

        private Sequence _openSequence;
        private Sequence _closeSequence;

        #endregion

        #region Unity Methods

        private void Start()
        {
            BuildCloseSequence();
        }

        private void OnTriggerEnter(Collider other)
        {
            BuildStartSequence();

            if (_closeSequence.IsActive())
            {
                _closeSequence.Kill();
            }
            
            _openSequence.Play();
        }
        
        private void OnTriggerExit(Collider other)
        {
            BuildCloseSequence();
            _closeSequence.Play();
        }
        
        #endregion

        private void BuildStartSequence()
        {
            _openSequence = DOTween.Sequence();
            _openSequence.Append(doorPositiveX.transform.DOLocalMoveX(1.6f, 1).SetEase(Ease.Linear))
                .Join(doorNegativeX.transform.DOLocalMoveX(-1.6f, 1).SetEase(Ease.Linear))
                .OnPlay(() => AudioManager.Instance.PlayClip(AudioManager.SFX_Type.DoorSoundOpen));
        }

        private void BuildCloseSequence()
        {
            _closeSequence = DOTween.Sequence();
            _closeSequence.Append(doorPositiveX.transform.DOLocalMoveX(0.5f, 1).SetEase(Ease.Linear))
                .Join(doorNegativeX.transform.DOLocalMoveX(-0.5f, 1).SetEase(Ease.Linear))
                .OnPlay(() => AudioManager.Instance.PlayClip(AudioManager.SFX_Type.DoorSoundClose));
        }
    }
}