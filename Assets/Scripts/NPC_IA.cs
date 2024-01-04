using System;
using Proyecto.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Proyecto.IA
{
    public class NPC_IA : MonoBehaviour
    {
        #region Inspector Variables
        
        #endregion

        #region Public Variables

        #endregion

        #region Private Variables

        private NavMeshAgent _navMeshAgent;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _navMeshAgent.speed = IAManager.Instance.Speed;
            _navMeshAgent.SetDestination(IAManager.Instance.getRandomSalaPoint());
        }

        private void Update()
        {
            if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}