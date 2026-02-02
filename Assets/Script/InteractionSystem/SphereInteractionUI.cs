using UnityEngine;
using System.Collections.Generic;
using Script.InteractionSystem.Interface;
using System;

namespace Script.InteractionSystem
{
    [Serializable]
    public class SphereInteractionUI : InteractionUIController
    {
        private Collider[] results = new Collider[10];

        public override void UpdateTick()
        {
            if (ownerGameObject == null || uiPrefab == null) return;

            int numFound = Physics.OverlapSphereNonAlloc(ownerGameObject.transform.position, detectionRadius, results,
                interactionLayer);

            List<IInteractable> foundList = new List<IInteractable>();
            for (int i = 0; i < numFound; i++)
            {
                if (results[i].TryGetComponent(out IInteractable interactable))
                    foundList.Add(interactable);
            }

            RefreshUI(foundList);
        }
    }
}