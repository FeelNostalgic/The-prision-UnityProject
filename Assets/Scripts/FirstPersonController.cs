using System;
using Manager;
using Proyecto.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Proyecto.Controller
{
    public class FirstPersonController : Singlenton<FirstPersonController>
    {
        #region Inspector Variables

        [FormerlySerializedAs("MoveSpeed")]
        [Header("Move")]
        [SerializeField] private float moveSpeed;
        [FormerlySerializedAs("LimitMove")] [SerializeField] private float limitMove;
        

        [FormerlySerializedAs("RotationSpeed")]
        [Header("Rotation")] 
        [SerializeField] private float rotationSpeed;
        [FormerlySerializedAs("LimitRotation")] [SerializeField] private float limitRotation;

        #endregion

        #region Public Variables
        public bool IsJaulaUp { private get; set; }
        
        #endregion

        #region Private Variables

        private float _rotationY;
        private float _rotationX;
        private Rigidbody _rb;
        private Vector3 _startPosition;

        #endregion

        #region Unity Methods
        
        private void Start()
        {
            _rotationY = 0;
            Cursor.lockState = CursorLockMode.Locked;
            IsJaulaUp = true;
        }

        private void Update()
        {
            if(UIManager.Instance.Paused) return;
            CalculateRotation();
            CalculateMove();
        }

        #endregion

        #region Public Methods

        public void SetStartPositionAndRotation(Vector3 position)
        {
            transform.SetPosition(position);
            var direction = - transform.position;
            direction.y = 0;
            transform.SetRotation(Quaternion.LookRotation(direction));
            _startPosition = position;
            // transform.LookAt(Vector3.zero);
            // transform.SetRotationX(0);
            // transform.SetRotationZ(0);
        }

        #endregion
        
        #region Private Methods
        
        private void CalculateRotation()
        {
            _rotationY += -Input.GetAxis("Mouse Y") * rotationSpeed;
            var rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            _rotationY = Mathf.Clamp(_rotationY, -limitRotation, limitRotation);
            Camera.main.transform.localRotation = Quaternion.Euler(_rotationY, 0, 0);
            transform.rotation *= Quaternion.Euler(0, rotationX, 0);
        }

        private void CalculateMove()
        {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");

            transform.position += transform.forward * (v * moveSpeed * Time.deltaTime);
            transform.position += Quaternion.AngleAxis(90, Vector3.up) * transform.forward *
                                  (h * moveSpeed * Time.deltaTime);
            if(IsJaulaUp) calculateLimits();
        }
        
        private void calculateLimits()
        {
            transform.LimitZ(_startPosition.z-limitMove, _startPosition.z+limitMove);
            transform.LimitX(_startPosition.x-limitMove, _startPosition.x+limitMove);
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