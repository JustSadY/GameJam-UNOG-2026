using UnityEngine;
using System.Collections;

public class EnterLoadRoom : MonoBehaviour
{
    [SerializeField] private GameObject _rightDoor;
    [SerializeField] private GameObject _leftDoor;

    [Header("Door Settings")] [SerializeField]
    private float openDistance = 2f;

    [SerializeField] private float openSpeed = 2f;

    private bool _isOpening = false;

    private void Awake()
    {
        StartCoroutine(OpenDoors());
    }

    private IEnumerator OpenDoors()
    {
        if (_leftDoor == null || _rightDoor == null)
        {
            Debug.LogError("Door references are missing in the inspector!");
            yield break;
        }

        _isOpening = true;

        Vector3 leftTargetPos = _leftDoor.transform.localPosition + Vector3.forward * openDistance;
        Vector3 rightTargetPos = _rightDoor.transform.localPosition + Vector3.back * openDistance;

        while (Vector3.Distance(_leftDoor.transform.localPosition, leftTargetPos) > 0.01f)
        {
            _leftDoor.transform.localPosition = Vector3.MoveTowards(
                _leftDoor.transform.localPosition,
                leftTargetPos,
                openSpeed * Time.deltaTime
            );

            _rightDoor.transform.localPosition = Vector3.MoveTowards(
                _rightDoor.transform.localPosition,
                rightTargetPos,
                openSpeed * Time.deltaTime
            );

            yield return null;
        }

        _leftDoor.transform.localPosition = leftTargetPos;
        _rightDoor.transform.localPosition = rightTargetPos;

        _isOpening = false;
    }
}