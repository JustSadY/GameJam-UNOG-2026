using UnityEngine;
using System.Collections.Generic;
using Script.InteractionSystem.Interface;
using System;

namespace Script.InteractionSystem
{
    [Serializable]
    public abstract class InteractionUIController
    {
        protected GameObject ownerGameObject;
        protected LayerMask interactionLayer;

        [Header("Detection Settings")] [SerializeField]
        protected float detectionRadius = 5f;

        [Header("UI Settings")] [SerializeField]
        protected GameObject uiPrefab;

        [SerializeField] protected Vector3 uiOffset = new Vector3(0, 1.2f, 0);

        protected Dictionary<IInteractable, GameObject> activeUIs = new Dictionary<IInteractable, GameObject>();
        protected HashSet<IInteractable> foundThisFrame = new HashSet<IInteractable>();
        protected List<IInteractable> toRemove = new List<IInteractable>();

        public virtual void Initialize(GameObject owner, LayerMask layer)
        {
            ownerGameObject = owner;
            interactionLayer = layer;
        }

        public abstract void UpdateTick();

        protected virtual void RefreshUI(IEnumerable<IInteractable> detectedInteractables)
        {
            foundThisFrame.Clear();
            foreach (var interactable in detectedInteractables)
            {
                foundThisFrame.Add(interactable);
                if (!activeUIs.ContainsKey(interactable))
                {
                    if (interactable is MonoBehaviour mono)
                    {
                        GameObject ui = GameObject.Instantiate(uiPrefab, mono.transform.position + uiOffset,
                            Quaternion.identity);
                        activeUIs.Add(interactable, ui);
                    }
                }
            }

            toRemove.Clear();
            foreach (var pair in activeUIs)
            {
                if (!foundThisFrame.Contains(pair.Key))
                {
                    GameObject.Destroy(pair.Value);
                    toRemove.Add(pair.Key);
                }
                else if (pair.Key is MonoBehaviour mono)
                {
                    pair.Value.transform.position = mono.transform.position + uiOffset;
                    if (Camera.main != null)
                    {
                        pair.Value.transform.LookAt(Camera.main.transform);
                        pair.Value.transform.Rotate(0, 180, 0);
                    }
                }
            }

            foreach (var key in toRemove) activeUIs.Remove(key);
        }
    }
}