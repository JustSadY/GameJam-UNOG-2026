using Script.InteractionSystem.Interface;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.InteractionSystem
{
    public class MouseForwardSource : InteractionRaySettings
    {
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        public override bool TryGetInteractable(GameObject actor, LayerMask layer, out IInteractable interactable)
        {
            interactable = null;

            if (_mainCamera == null) return false;

            Vector2 mousePosition = Mouse.current.position.ReadValue();

            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, layer))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out interactable))
                {
                    Debug.Log($"Interactable found: {hit.collider.gameObject.name}");
                    return true;
                }
            }

            return false;
        }
    }
}