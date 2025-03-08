using System;
using Manager;
using Proyecto.Utilities;
using UnityEngine;

namespace Proyecto.Controller
{
    public class FirstPersonController : Singlenton<FirstPersonController>
    {
        #region Inspector Variables

        [Header("Move")]
        [SerializeField] private float MoveSpeed;
        [SerializeField] private float LimitMove;
        

        [Header("Rotation")] [SerializeField] private float RotationSpeed;
        [SerializeField] private float LimitRotation;

        #endregion

        #region Public Variables
        public bool IsJaulaUp { private get; set; }
        
        #endregion

        #region Private Variables

        private float _RotationY;
        private float _RotationX;
        private Rigidbody _rb;
        private Vector3 _startPosition;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _RotationY = 0;
            Cursor.lockState = CursorLockMode.Locked;
            IsJaulaUp = true;
            _startPosition = transform.position;
            //_rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(UIManager.Instance.Paused) return;
            CalculateRotation();
            CalculateMove();
        }

        #endregion
        
        #region Private Methods
        
        private void CalculateRotation()
        {
            _RotationY += -Input.GetAxis("Mouse Y") * RotationSpeed;
            var rotationX = Input.GetAxis("Mouse X") * RotationSpeed;
            _RotationY = Mathf.Clamp(_RotationY, -LimitRotation, LimitRotation);
            Camera.main.transform.localRotation = Quaternion.Euler(_RotationY, 0, 0);
            transform.rotation *= Quaternion.Euler(0, rotationX, 0);
        }

        private void CalculateMove()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            transform.position += transform.forward * (v * MoveSpeed * Time.deltaTime);
            transform.position += Quaternion.AngleAxis(90, Vector3.up) * transform.forward *
                                  (h * MoveSpeed * Time.deltaTime);
            if(IsJaulaUp) calculateLimits();
        }
        
        private void calculateLimits()
        {
            transform.LimitZ(_startPosition.z-LimitMove, _startPosition.z+LimitMove);
            transform.LimitX(_startPosition.x-LimitMove, _startPosition.x+LimitMove);
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("KillZone"))
            { 
                UIManager.Instance.ShowEndPanel();
            }
        }
    }
    
}