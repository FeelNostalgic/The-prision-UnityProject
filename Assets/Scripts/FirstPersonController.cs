using System;
using Gate;
using Manager;
using Proyecto.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Proyecto.Controller
{
    public class FirstPersonController : Singlenton<FirstPersonController>
    {
        #region Inspector Variables

        [SerializeField] private bool debugPlayer;

        [Header("Move")] [SerializeField] private float moveSpeed;
        [SerializeField] private float limitMove;

        [Header("Rotation")] [SerializeField] private float rotationSpeed;
        [SerializeField] private float limitRotation;

        #endregion

        #region Public Variables

        public bool IsJaulaUp { private get; set; }

        #endregion

        #region Private Variables

        private float _rotationY;
        private float _rotationX;
        private Rigidbody _rb;
        private CapsuleCollider _capsuleCollider;
        private Vector3 _startPosition;

        #endregion

        #region Unity Methods

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            _rotationY = 0;
            IsJaulaUp = !debugPlayer;
            _rb = GetComponent<Rigidbody>();
            _capsuleCollider = GetComponent<CapsuleCollider>();
            _rb.useGravity = debugPlayer;
        }

        private void Update()
        {
            if (UIManager.Instance.Paused) return;
            CalculateRotation();
            CalculateMove();
        }

        #endregion

        #region Public Methods

        public void SetStartPositionAndRotation(Vector3 position)
        {
            transform.SetPosition(position);
            var direction = -transform.position;
            direction.y = 0;
            transform.SetRotation(Quaternion.LookRotation(direction));
            _startPosition = position;
        }

        public void SetBiggerCapsuleColliderRadius()
        {
            _capsuleCollider.radius = 0.8f;
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
            if (IsJaulaUp) CalculateLimits();
        }

        private void CalculateLimits()
        {
            transform.LimitZ(_startPosition.z - limitMove, _startPosition.z + limitMove);
            transform.LimitX(_startPosition.x - limitMove, _startPosition.x + limitMove);
        }
        
        #endregion
    }
}