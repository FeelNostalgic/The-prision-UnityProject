using System;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using Proyecto.Controller;
using Proyecto.IA;
using Proyecto.Manager;
using Proyecto.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Proyecto.Behaviour
{
    public class FlechaBehaviour : Singlenton<FlechaBehaviour>
    {
        #region Inspector Variables

        [SerializeField] private float SpinDuration;

        [SerializeField] private List<GameObject> JaulasList;

        #endregion

        #region Public Variables

        public bool SpinIsCompleted { get; private set; }

        #endregion

        #region Private Variables

        private int _currentNPCs;
        private float _pieceAngle;
        private float _halfPieceAngle;
        private int _indice;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _currentNPCs = 0;
            _pieceAngle = 360f / 8;
            _halfPieceAngle = _pieceAngle / 2;
        }

        #endregion

        #region Public Methods

        public void Spin()
        {
            SpinIsCompleted = false;
            var NPC = GetIndexOfSpin();

            float targetAngle = _indice * _pieceAngle;
            float rightOffset = (targetAngle + _halfPieceAngle) % 360;
            float leftOffset = (targetAngle - _halfPieceAngle) % 360;

            float randomAngle = Random.Range(leftOffset, rightOffset);

            Vector3 targetRotation = Vector3.up * (randomAngle + 1 * 360 * SpinDuration);

            float prevAngle, currentAngle;
            prevAngle = currentAngle = transform.rotation.eulerAngles.y;

            bool isIndicatorOnTheLine = false;

            transform.DORotate(targetRotation, SpinDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart)
                .OnUpdate(() =>
                {
                    float diff = Mathf.Abs(prevAngle - currentAngle);
                    if (diff >= _halfPieceAngle) //Suena cuando la flecha apunta a una jaula
                    {
                        if (isIndicatorOnTheLine)
                        {
                            AudioManager.Instance.PlayClip(AudioManager.SFX_Type.tickSound);
                        }

                        prevAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }

                    currentAngle = transform.rotation.eulerAngles.y;
                })
                .OnComplete(() =>
                {
                    Debug.Log(_indice + ": Completed");
                    if(NPC != null) JailDown(NPC);
                    else JailDown();
                })
                .Play();
        }

        #endregion

        #region Private Methods
        private NPC_Position GetIndexOfSpin()
        {
            if (_currentNPCs < 7)
            {
                var target = SetupManager.Instance.GetNextNPC();
                _indice = target.Indice;
                _currentNPCs++;
                return target;
            }
            else
            {
                _indice = SetupManager.Instance.PlayerIndex;
                return null;
            }
        }
        
        private void JailDown(NPC_Position npc = null)
        {
            JaulasList[_indice].transform
                .DOMoveY(-2.55f, AudioManager.Instance.getClipDuration(AudioManager.SFX_Type.hidraulicSound))
                .OnComplete(() =>
                {
                    SpinIsCompleted = true;
                    if (npc != null) npc.NPC.GetComponent<NPC_IA>().enabled = true;
                    else FirstPersonController.Instance.IsJaulaUp = false;
                })
                .OnPlay(() => AudioManager.Instance.PlayClip(AudioManager.SFX_Type.hidraulicSound))
                .Play();
        }

        #endregion
    }
}