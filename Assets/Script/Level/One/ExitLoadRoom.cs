using UnityEngine;
using System.Collections;
using Script;
using UnityEngine.InputSystem;

public class ExitLoadRoom : MonoBehaviour
{
    public bool CanTeleport = true;

    [Header("Door Settings")] [SerializeField]
    private float openDistance = 2f;

    [SerializeField] private float openSpeed = 2f;

    [Header("Movement Settings")] [SerializeField]
    private float rotationSpeed = 5f;

    [SerializeField] private float WaitForSecendNextLevel = 2f;
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float arrivalThreshold = 0.5f;
    [SerializeField] private float delayBeforeLoad = 2f;

    private Vector3 _targetPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CanTeleport)
        {
            Transform targetTransform = transform.Find("TargetLocation");

            if (targetTransform != null)
            {
                _targetPosition = targetTransform.position;
            }
            else
            {
                Debug.LogWarning("TargetLocation child object not found!");
                return;
            }

            if (other.TryGetComponent(out PlayerInput playerInput))
            {
                playerInput.enabled = false;
            }

            // Start move and look sequence
            StartCoroutine(SmoothMoveAndLook(other.transform));
            StartCoroutine(LoadLevelAfterDelay());
        }
    }

    private IEnumerator SmoothMoveAndLook(Transform playerTransform)
    {
        Transform cameraTransform = playerTransform.GetComponentInChildren<Camera>()?.transform;

        while (playerTransform != null)
        {
            float distanceToTarget = Vector3.Distance(playerTransform.position, _targetPosition);

            // Distance Check: Log message when close enough
            if (distanceToTarget <= arrivalThreshold)
            {
                Debug.Log("Player reached the target threshold.");
            }

            // 1. Handle Rotation
            Vector3 direction = (_targetPosition - playerTransform.position).normalized;
            Vector3 horizontalDirection = new Vector3(direction.x, 0, direction.z);

            if (horizontalDirection != Vector3.zero)
            {
                Quaternion targetBodyRotation = Quaternion.LookRotation(horizontalDirection);
                playerTransform.rotation = Quaternion.Slerp(
                    playerTransform.rotation,
                    targetBodyRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // 2. Handle Camera Vertical Rotation
            if (cameraTransform != null)
            {
                Vector3 camDirection = (_targetPosition - cameraTransform.position).normalized;
                Quaternion targetCamRotation = Quaternion.LookRotation(camDirection);
                cameraTransform.rotation = Quaternion.Slerp(
                    cameraTransform.rotation,
                    targetCamRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // 3. Handle Movement
            playerTransform.position = Vector3.MoveTowards(
                playerTransform.position,
                _targetPosition,
                movementSpeed * Time.deltaTime
            );

            // Break if we are very close to the target
            if (distanceToTarget < 0.1f) break;

            yield return null;
        }
    }

    private IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(WaitForSecendNextLevel);
        LevelManager.Instance.NextLevel();
    }
}