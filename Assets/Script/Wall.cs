using System;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private float _speed = 2.0f;

    private float _movementProgress = 0f;
    private bool _movingToEnd = true;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float step = _speed * Time.deltaTime;

        if (_movingToEnd)
        {
            _movementProgress += step;
            if (_movementProgress >= 1f) _movingToEnd = false;
        }
        else
        {
            _movementProgress -= step;
            if (_movementProgress <= 0f) _movingToEnd = true;
        }

        transform.position = Vector3.Lerp(_startPosition, _endPosition, _movementProgress);
    }
}