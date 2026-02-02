using Script.InteractionSystem.Struct;
using UnityEngine;

namespace Script.InteractionSystem.Interface
{
    public interface IInteractable
    {
        InteractionData Data { get; }
        public void Interact(GameObject interactor);
    }
}