using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arrow;
using DG.Tweening;
using Proyecto.Controller;
using Proyecto.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager
{
    public class SetupManager : Singlenton<SetupManager>
    {
        #region Inspector Variables

        [SerializeField] private List<Transform> positions;

        [SerializeField] private List<GameObject> npcGameObjects;

        #endregion

        #region Public Variables

        public NPCJailPosition PlayerPosition { get; private set; }

        #endregion

        #region Private Variables

        private GameObject _player;

        private readonly List<Transform> _availablePositions = new();
        private readonly List<NPCJailPosition> _npcStartPoints = new();

        #endregion

        #region Unity Methods

        //

        #endregion

        #region Public Methods

        public NPCJailPosition GetNextNPC()
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
            _player = GameObject.FindGameObjectWithTag("Player");

            InicializateListAux(positions, _availablePositions);

            var selectedPosition = GetRandomPosition(out var index);
            PlayerPosition = new NPCJailPosition
            {
                NPC = _player,
                Index = index,
                Jail = positions.FirstOrDefault(x => x.position == selectedPosition.position),
                IsPlayer = true
            };
            
            _player.GetComponent<FirstPersonController>().SetStartPositionAndRotation(selectedPosition.position);
            _player.transform.AddY(2f);
            _player.GetComponent<Rigidbody>().useGravity = true;

            Debug.Log($"Player nas been started in position {selectedPosition.position}. Positions left: {_availablePositions.Count}");

            InitializeNPCsPosition();
        }

        #endregion

        #region Private Methods

        private IEnumerator SetupScene()
        {
            yield return new WaitForSeconds(Random.Range(0.75f, 1f));

            foreach (var npc in npcGameObjects)
            {
                npc.transform.DOPunchScale(new Vector3(.2f, -0.3f, .2f), 2f, vibrato: 0).OnPlay(() => { npc.SetActive(true); }).Play();

                yield return new WaitForSeconds(Random.Range(0.75f, 1.25f));
            }

            StartCoroutine(SpinBehaviour());
        }

        private Transform GetRandomPosition(out int index)
        {
            index = Random.Range(0, _availablePositions.Count);
            var currentPosition = _availablePositions[index];
            _availablePositions.RemoveAt(index);
            index = positions.IndexOf(currentPosition);
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

        private void InitializeNPCsPosition()
        {
            foreach (var npc in npcGameObjects)
            {
                var selectedPosition = GetRandomPosition(out var index);

                npc.transform.position = selectedPosition.position;
                npc.transform.AddY(2f);

                _npcStartPoints.Add(
                    new NPCJailPosition
                    {
                        NPC = npc,
                        Index = index,
                        Jail = positions.FirstOrDefault(x => x.position == selectedPosition.position)
                    });

                Debug.Log($"Npc {npc.name}-'{index}' has been started in position {selectedPosition.position}. Positions left: {_availablePositions.Count}");
            }
        }

        private static void InicializateListAux<T>(List<T> from, List<T> to)
        {
            to.AddRange(from);
        }

        #endregion
    }

    public class NPCJailPosition
    {
        public GameObject NPC;
        public Transform Jail;
        public int Index;
        public bool IsPlayer;
    }
}