using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _speed;

    private void Update()
    {
        Vector3 direction = _target.position - transform.position;

        Vector3 desiredDirection = direction + _offset;

        transform.position += (desiredDirection * _speed * Time.deltaTime);
    }
}
