using UnityEngine;

public class Stairs : MonoBehaviour, IFreeze
{
    [Header("Movement Settings")] [SerializeField]
    private Vector3 _startPosition;

    [SerializeField] private Vector3 _endPosition;
    [SerializeField] private float _speed = 2.0f;

    private bool _isFrozen = false;
    private float _movementProgress = 0f;
    private bool _movingToEnd = true;

    private void Update()
    {
        if (_isFrozen) return;

        MoveStairs();
    }

    private void MoveStairs()
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

    public void Freeze()
    {
        _isFrozen = true;
    }

    public void UnFreeze()
    {
        _isFrozen = false;
    }

    public bool IsFreezed() => _isFrozen;

    private void SetPositions()
    {
        _startPosition = transform.position;
        _endPosition = transform.position + Vector3.up * 5f;
    }
}