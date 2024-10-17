using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const string HorizontalAxisName = "Horizontal";
    private const string VerticalAxisName = "Vertical";

    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;

    private float _deathZone = 0.1f;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 direction = new Vector3(-Input.GetAxisRaw(VerticalAxisName), 0 , Input.GetAxisRaw(HorizontalAxisName));

        if (direction.magnitude < _deathZone)
            return;

        _characterController.Move(direction.normalized * _moveSpeed * Time.deltaTime);
        
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        float step = _rotateSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, step);
    }
}
