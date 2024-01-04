using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Proyecto.Behaviour;
using Proyecto.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Proyecto.Manager
{
    public class SetupManager : Singlenton<SetupManager>
    {
        #region Inspector Variables

        [SerializeField] private List<Transform> Positions;
        [SerializeField] private GameObject Player;
        [SerializeField] private List<GameObject> NPCGameObjects;

        #endregion

        #region Public Variables

        public int PlayerIndex => _playerIndex;
        
        #endregion

        #region Private Variables

        private List<Transform> _positionsAux;
        private List<GameObject> _npcListAux;
        private List<NPC_Position> _npcStartPoints;
        private int _playerIndex;

        #endregion

        #region Unity Methods
        private void Start()
        {
            _npcListAux = new List<GameObject>();
            _positionsAux = new List<Transform>();
            _npcStartPoints = new List<NPC_Position>();
            StartCoroutine(SetupScene());
        }

        #endregion

        #region Public Methods

        public NPC_Position GetNextNPC()
        {
            var npcClass = _npcStartPoints[Random.Range(0, _npcStartPoints.Count)]; //Target aleatorio
            _npcStartPoints.Remove(npcClass);
            return npcClass;
        }

        #endregion

        #region Private Methods
        private IEnumerator SetupScene()
        {
            InicializateListAux(Positions, _positionsAux);

            var index = Random.Range(0, _positionsAux.Count);
            var currentPosition = _positionsAux[index];
            _positionsAux.Remove(currentPosition);
            _playerIndex = index;

            Player.transform.position = currentPosition.position + Vector3.up * 1.5f;
            Player.SetActive(true);
            
            yield return new WaitForSeconds(Random.Range(0.75f,1f));
            
            foreach (var currentNPC in NPCGameObjects)
            {
                index = Random.Range(0, _positionsAux.Count); //Posicion aleatoria
                currentPosition = _positionsAux[index];
                _positionsAux.Remove(currentPosition);
                
                currentNPC.transform.position = currentPosition.position + Vector3.up * 1.5f;
                currentNPC.transform.DOPunchScale(new Vector3(.2f, -0.3f, .2f), 1.5f, vibrato: 0).Play();
                currentNPC.SetActive(true);
                
                _npcStartPoints.Add(new NPC_Position(){NPC = currentNPC, Indice = Positions.IndexOf(currentPosition)});
                
                yield return new WaitForSeconds(Random.Range(0.75f,1f));
            }
            
            StartCoroutine(SpinBehaviour());
        }

        private IEnumerator SpinBehaviour()
        {
            InicializateListAux(NPCGameObjects, _npcListAux);
            for (int i = 0; i < 8; i++)
            {
                yield return new WaitForSeconds(Random.Range(3, 5));
                FlechaBehaviour.Instance.Spin();
                yield return new WaitWhile(() => !FlechaBehaviour.Instance.SpinIsCompleted);
            }
        }
        
        private void InicializateListAux<T>(List<T> from, List<T> to)
        {
            foreach (var p in from)
            {
                to.Add(p);
            }
        }

        #endregion
    }

    public class NPC_Position
    {
        public GameObject NPC;
        public int Indice;
    }
}