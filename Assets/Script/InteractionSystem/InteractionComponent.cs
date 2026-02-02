using Script.InteractionSystem.Interface;
using UnityEngine;

namespace Script.InteractionSystem
{
    public class InteractionComponent : MonoBehaviour
    {
        [SubclassSelector] [SerializeReference]
        private InteractionRaySettings raySettings;

        [SubclassSelector] [SerializeReference]
        private InteractionUIController interactionUIController;

        [SerializeField] private LayerMask interactionLayer = ~0;

        private void Awake()
        {
            interactionUIController?.Initialize(gameObject, interactionLayer);
        }

        void Update()
        {
            interactionUIController?.UpdateTick();
        }

        public void PerformInteraction()
        {
            if (raySettings == null) return;

            if (raySettings.TryGetInteractable(gameObject, interactionLayer, out IInteractable interactable))
            {
                interactable.Interact(this.gameObject);
            }
        }
    }
}