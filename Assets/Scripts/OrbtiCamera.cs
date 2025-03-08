using System;
using UnityEngine;
using Cinemachine;

public class OrbitCamera : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform target;
    [SerializeField] [Range(0,50f)] private float rotationSpeed = 10f;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (target == null) return;
        transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        transform.LookAt(target.position);
    }
}