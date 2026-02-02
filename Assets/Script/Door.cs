using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    private float _openDistance = 5f;
    [SerializeField] private float _openSpeed = 2f;
    private bool _isOpen= false;

    public bool isOpen()
    {
        return _isOpen; 
    }
    public void OpenTheDoor()
    {
        _isOpen = true;
        StopAllCoroutines();
        StartCoroutine(MoveDoorRoutine());
    }

    private IEnumerator MoveDoorRoutine()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + Vector3.down * _openDistance;

        float elapsedTime = 0f;
        float duration = _openDistance / _openSpeed;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
}