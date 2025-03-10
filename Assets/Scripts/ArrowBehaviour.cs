using DG.Tweening;
using Manager;
using Proyecto.Controller;
using Proyecto.IA;
using Proyecto.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Arrow
{
    public class ArrowBehaviour : Singlenton<ArrowBehaviour>
    {
        #region Inspector Variables

        [SerializeField] private float spinDuration;
        
        #endregion

        #region Public Variables

        public bool SpinIsCompleted { get; private set; }

        #endregion

        #region Private Variables

        private int _currentNPCs;
        private float _pieceAngle;
        private float _halfPieceAngle;

        private const int NPC_COUNT = 8;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _currentNPCs = 0;
            _pieceAngle = 360f / NPC_COUNT;
            _halfPieceAngle = _pieceAngle / 2;
        }
        
        #endregion

        #region Public Methods

        public void Spin()
        {
            SpinIsCompleted = false;
            var npc = GetIndexOfSpin();

            var targetAngle = npc.Index * _pieceAngle;
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
                            AudioManager.Instance.PlayClip(AudioManager.SFX_Type.TickSound);
                        }

                        prevAngle = currentAngle;
                        isIndicatorOnTheLine = !isIndicatorOnTheLine;
                    }

                    currentAngle = transform.rotation.eulerAngles.y;
                })
                .OnComplete(() =>
                {
                    Debug.Log(npc.Index + ": Completed");
                    JailDown(npc);
                })
                .Play();
        }

        #endregion

        #region Private Methods
        private NPCJailPosition GetIndexOfSpin()
        {
            if (_currentNPCs >= 7) return SetupManager.Instance.PlayerPosition;
            
            var target = SetupManager.Instance.GetNextNPC();
            _currentNPCs++;
            return target;
        }
        
        private void JailDown(NPCJailPosition npcJail)
        {
            npcJail.Jail
                .DOMoveY(-2.55f, AudioManager.Instance.GetClipDuration(AudioManager.SFX_Type.HydraulicSound))
                .OnComplete(() =>
                {
                    SpinIsCompleted = true;
                    if (!npcJail.IsPlayer) npcJail.NPC.GetComponent<NPC_IA>().enabled = true;
                    else
                    {
                        FirstPersonController.Instance.IsJaulaUp = false;
                        FirstPersonController.Instance.SetBiggerCapsuleColliderRadius();
                    }
                })
                .OnPlay(() => AudioManager.Instance.PlayClip(AudioManager.SFX_Type.HydraulicSound))
                .Play();
        }

        #endregion
    }
}