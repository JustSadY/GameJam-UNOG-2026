using Script.InteractionSystem.Interface;
using Script.InteractionSystem.Struct;
using UnityEngine;

namespace Script
{
    [RequireComponent(typeof(Rigidbody))]
    public class FireBall : MonoBehaviour, IInteractable
    {
        [SerializeField] private float _pushForce = 10f;
        [SerializeField] private float _unequipMaskSpeed = 5f;
        [SerializeField] private float _velocityThreshold = 2f;

        public InteractionData Data { get; }
        private Rigidbody _rigidbody;
        private Mask _mask;
        private bool _speedBoostApplied = false; // Bayrak ekledik

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            ApplySpeedBoostIfMaskNotActive();
        }

        private void ApplySpeedBoostIfMaskNotActive()
        {
            bool isMaskNotEffecting = _mask == null || !_mask.IsActive;

            if (isMaskNotEffecting && !_speedBoostApplied) // Bayrağa bakıyoruz
            {
                Vector3 horizontalVelocity = new Vector3(_rigidbody.linearVelocity.x, 0, _rigidbody.linearVelocity.z);

                if (horizontalVelocity.magnitude > _velocityThreshold)
                {
                    Vector3 speedBoostForce = horizontalVelocity.normalized * _unequipMaskSpeed;
                    _rigidbody.AddForce(speedBoostForce, ForceMode.VelocityChange);
                    _speedBoostApplied = true; // Bayrağı true yapıyoruz
                }
            }
        }

        public void Interact(GameObject interactor)
        {
            _mask = interactor.GetComponent<Mask>();
            _speedBoostApplied = false; // Bayrağı resetliyoruz

            if (_mask == null || !_mask.IsActive)
            {
                Debug.Log("zort1");
                return;
            }

            Debug.Log("zort2");
            ApplyRepellingForce(interactor.transform.position);
        }

        private void ApplyRepellingForce(Vector3 interactorPosition)
        {
            Vector3 pushDirection = (transform.position - interactorPosition).normalized;
            _rigidbody.AddForce(pushDirection * _pushForce, ForceMode.Impulse);
        }
    }
}
    