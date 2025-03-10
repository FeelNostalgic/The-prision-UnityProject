using Proyecto.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Proyecto.IA
{
    public class NPC_IA : MonoBehaviour
    {
        #region Private Variables

        private NavMeshAgent _navMeshAgent;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _navMeshAgent.enabled = true;
        }

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _navMeshAgent.speed = IAManager.Instance.Speed;
            _navMeshAgent.SetDestination(IAManager.Instance.getRandomRoomPoint());
        }

        private void Update()
        {
            if (_navMeshAgent.remainingDistance < _navMeshAgent.stoppingDistance)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}