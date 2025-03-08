using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Proyecto.Behaviour;
using Proyecto.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Proyecto.Manager
{
    public class SetupManager : Singlenton<SetupManager>
    {
        #region Inspector Variables

        [FormerlySerializedAs("Positions")] [SerializeField]
        private List<Transform> positions;

        [FormerlySerializedAs("Player")] [SerializeField]
        private GameObject player;

        [FormerlySerializedAs("NPCGameObjects")] [SerializeField]
        private List<GameObject> npcGameObjects;

        #endregion

        #region Public Variables

        public int PlayerIndex { get; private set; }

        #endregion

        #region Private Variables

        private readonly List<Transform> _positionsAux = new ();
        private readonly List<NPC_Position> _npcStartPoints = new ();

        #endregion

        #region Unity Methods

        // Empty

        #endregion

        #region Public Methods

        public NPC_Position GetNextNPC()
        {
            var npcClass = _npcStartPoints[Random.Range(0, _npcStartPoints.Count)]; //Random target
            _npcStartPoints.Remove(npcClass);
            return npcClass;
        }

        public void StartGame()
        {
            StartCoroutine(SetupScene());
        }
        
        public void InitializePlayer()
        {
            InicializateListAux(positions, _positionsAux);

            var selectedPosition = GetRandomPosition(out var index);
            PlayerIndex = index;

            player.transform.position = selectedPosition.position + Vector3.up * 1.5f;
            player.GetComponent<Rigidbody>().useGravity = true;

            Debug.Log($"Player nas been started in position {selectedPosition.position}. Positions left: {_positionsAux.Count}");
            
            InitializeNPCsPosition();
        }

        #endregion

        #region Private Methods

        private IEnumerator SetupScene()
        {
            yield return new WaitForSeconds(Random.Range(0.75f, 1f));

            foreach (var npc in npcGameObjects)
            {
                npc.transform.DOPunchScale(new Vector3(.2f, -0.3f, .2f), 1.5f, vibrato: 0).OnPlay(() => { npc.SetActive(true); }).Play();

                yield return new WaitForSeconds(Random.Range(0.75f, 1f));
            }

            StartCoroutine(SpinBehaviour());
        }

        private Transform GetRandomPosition(out int index)
        {
            index = Random.Range(0, _positionsAux.Count);
            var currentPosition = _positionsAux[index];
            _positionsAux.Remove(currentPosition);
            return currentPosition;
        }

        private static IEnumerator SpinBehaviour()
        {
            for (var i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(Random.Range(3, 5));
                ArrowBehaviour.Instance.Spin();
                yield return new WaitWhile(() => !ArrowBehaviour.Instance.SpinIsCompleted);
            }
        }

        private static void InicializateListAux<T>(List<T> from, List<T> to)
        {
            to.AddRange(from);
        }
        
        private void InitializeNPCsPosition()
        {
            foreach (var npc in npcGameObjects)
            {
                var selectedPosition = GetRandomPosition(out var index);
                
                npc.transform.position = selectedPosition.position + Vector3.up * 1.5f;
                
                _npcStartPoints.Add(new NPC_Position { NPC = npc, Index = index });
                
                Debug.Log($"Npc {npc.name} has been started in position {selectedPosition.position}. Positions left: {_positionsAux.Count}");

            }
        }
        
        #endregion
    }

    public class NPC_Position
    {
        public GameObject NPC;
        public int Index;
    }
}