using System;
using Script.InteractionSystem.Interface;
using Script.InteractionSystem.Struct;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Script
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MouseAI : MonoBehaviour, IFreeze, IInteractable
    {
        public InteractionData Data { get; }

        private Vector3 _targetLocation;
        private NavMeshAgent _agent;
        private bool _isFrozen = false;

        [SerializeField] private GameObject passwordCodePanel;
        [SerializeField] private Door door;

        [SerializeField] private AudioClip[] _audioClip;

        [SerializeField]
        private Subtitle[] experimentSubtitles = new Subtitle[]
        {
        new Subtitle { title = "Öğğğğkkk elini çabuk tut biraz. Benim midem kaldırmıyor öğğğğğkkk" }, // Index 0
        new Subtitle { title = "Tamam artık kapıya git" }, // Index 1
        };

        [Header("Settings")] [SerializeField] private float _patrolRadius = 10f;
        [SerializeField] private float _arrivalDistance = 1f;
        private IInteractable _ınteractableImplementation;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            SetNewRandomDestination();
        }

        private void Update()
        {
            if (passwordCodePanel.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Q)) 
                {
                    passwordCodePanel.SetActive(false);
                }
            }
            if (_isFrozen) return;
            CheckArrivalAndMove();
        }

        private void CheckArrivalAndMove()
        {
            if (!_agent.isOnNavMesh) return;

            if (Vector3.Distance(transform.position, _targetLocation) <= _arrivalDistance)
            {
                SetNewRandomDestination();
            }
        }

        private void SetNewRandomDestination()
        {
            Vector3 newPoint = GetRandomLocationOnNavMesh(transform.position, _patrolRadius);

            if (newPoint != Vector3.zero)
            {
                _targetLocation = newPoint;
                _agent.SetDestination(_targetLocation);
            }
        }

        public Vector3 GetRandomLocationOnNavMesh(Vector3 center, float radius)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection += center;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }

            return Vector3.zero;
        }


        public void Freeze()
        {
            _isFrozen = true;

            if (_agent.isActiveAndEnabled)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
            }
        }

        public void UnFreeze()
        {
        }

        public bool IsFreezed() => _isFrozen;

        public void Interact(GameObject interactor)
        {
            if (!_isFrozen) return;
            if (passwordCodePanel == null) return;
            passwordCodePanel.SetActive(true);
            if (!door.isOpen())
            {
                VoiceManager.Instance.PlaySequence(_audioClip, experimentSubtitles);
                door.OpenTheDoor();
            }
        }
    }
}