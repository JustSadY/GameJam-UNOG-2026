using System.Collections.Generic;
using Script;
using UnityEngine;

public class FreezeMask : Mask, IAttackAction
{
    private List<IFreeze> _currentlyFrozenObjects = new List<IFreeze>();
    private Camera _mainCamera;

    [Header("Raycast Settings")] [SerializeField]
    private float _maxDistance = 100f;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("FreezeMask: No Main Camera found!");
        }
    }

    public void OnAttack()
    {
        if (!IsActive) return;
        _input.attackAction = false;
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out var hit, _maxDistance))
        {
            if (hit.collider.TryGetComponent<IFreeze>(out IFreeze freezeTarget))
            {
                if (!freezeTarget.IsFreezed())
                {
                    freezeTarget.Freeze();
                    _currentlyFrozenObjects.Add(freezeTarget);
                }
            }
        }
    }

    public override void UnEquipMask()
    {
        base.UnEquipMask();
        foreach (IFreeze freezeTarget in _currentlyFrozenObjects)
        {
            if (freezeTarget != null)
            {
                freezeTarget.UnFreeze();
            }
        }

        _currentlyFrozenObjects.Clear();
    }
}