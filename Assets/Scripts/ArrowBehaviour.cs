using System.Collections.Generic;
using DG.Tweening;
using Proyecto.Controller;
using Proyecto.IA;
using Proyecto.Manager;
using Proyecto.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Proyecto.Behaviour
{
    public class ArrowBehaviour : Singlenton<ArrowBehaviour>
    {
        #region Inspector Variables

        [FormerlySerializedAs("SpinDuration")] [SerializeField] private float spinDuration;

        [FormerlySerializedAs("JailsList")] [SerializeField] private List<GameObject> jailsList;

        #endregion

        #region Public Variables

        public bool SpinIsCompleted { get; private set; }

        #endregion

        #region Private Variables

        private int _currentNPCs;
        private float _pieceAngle;
        private float _halfPieceAngle;
        private int _index;

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
            var npc = GetIndexOfSpin();

            var targetAngle = _index * _pieceAngle;
            var rightOffset = (targetAngle + _halfPieceAngle) % 360;
            var leftOffset = (targetAngle - _halfPieceAngle) % 360;

            var randomAngle = Random.Range(leftOffset, rightOffset);

            var targetRotation = Vector3.up * (randomAngle + 1 * 360 * spinDuration);

            float prevAngle, currentAngle;
            prevAngle = currentAngle = transform.rotation.eulerAngles.y;

            var isIndicatorOnTheLine = false;

            transform.DORotate(targetRotation, spinDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuart)
                .OnUpdate(() =>
                {
                    var diff = Mathf.Abs(prevAngle - currentAngle);
                    if (diff >= _halfPieceAngle) //Sound when arrow points to a jail 
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
                    Debug.Log(_index + ": Completed");
                    if(npc != null) JailDown(npc);
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
                _index = target.Indice;
                _currentNPCs++;
                return target;
            }
            else
            {
                _index = SetupManager.Instance.PlayerIndex;
                return null;
            }
        }
        
        private void JailDown(NPC_Position npc = null)
        {
            jailsList[_index].transform
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