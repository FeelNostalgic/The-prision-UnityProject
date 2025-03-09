using System.Collections.Generic;
using Proyecto.Utilities;
using UnityEngine;

namespace Proyecto.Manager
{
    public class IAManager : Singlenton<IAManager>
    {
        #region Inspector Variables

        [SerializeField] private float IASpeed;
        
        [SerializeField] private List<Transform> SalaPoints;

        #endregion

        #region Public Variables

        public float Speed => IASpeed;


        #endregion

        #region Private Variables

        #endregion

        #region Unity Methods

        #endregion

        #region Public Methods

        public Vector3 getRandomRoomPoint()
        {
            return SalaPoints[Random.Range(0, SalaPoints.Count)].position;
        }
        #endregion

        #region Private Methods

        #endregion
    }
}