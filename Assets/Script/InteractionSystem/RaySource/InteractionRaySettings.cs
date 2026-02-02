using UnityEngine;
using Script.InteractionSystem.Interface;

namespace Script.InteractionSystem
{
    [System.Serializable]
    public abstract class InteractionRaySettings
    {
        public abstract bool TryGetInteractable(GameObject actor, LayerMask layer, out IInteractable interactable);
    }
}