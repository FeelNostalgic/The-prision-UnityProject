using System;
using UnityEngine;
using Cinemachine;

public class OrbitCamera : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    public Transform target;
    public float rotationSpeed = 10f;

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (target != null)
        {
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}