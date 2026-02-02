using System.Collections.Generic;
using Script.InteractionSystem.Interface;
using Script.InteractionSystem.Struct;
using UnityEngine;

namespace Script
{
    public class PasswdButton : MonoBehaviour, IInteractable
    {
        private PasswdDoor _passwdDoor;
        private string _color;
        [SerializeField] private char _colorButton;

        public InteractionData Data { get; }

        private void Awake()
        {
            _passwdDoor = GetComponentInParent<PasswdDoor>();

            if (_passwdDoor == null)
            {
                Debug.LogError($"PasswdDoor not found in parents of {gameObject.name}");
            }

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                string hexColor = ColorUtility.ToHtmlStringRGB(meshRenderer.material.color);
                _color = "#" + hexColor;
            }
        }

        public void Interact(GameObject interactor)
        {
            if (_passwdDoor != null)
            {
                _passwdDoor.AddKey(_colorButton, gameObject.name[0], _color);
            }
        }
    }
}