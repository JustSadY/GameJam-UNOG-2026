using UnityEngine;

public class ScaleFil : Mask
{
    [Header("Target Settings")]
    [SerializeField] private Fil _targetFil;

    private Camera _mainCamera;
    private bool _isScaled = false;

    [Header("Raycast Settings")]
    [SerializeField] private float _maxDistance = 100f;

    protected override void Awake()
    {
        base.Awake();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        base.Update();

        if (!IsActive || _targetFil == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            PerformRaycast();
        }
    }

    private void PerformRaycast()
    {
        Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _maxDistance))
        {
            Fil hitFil = hit.collider.GetComponentInParent<Fil>();

            if (hitFil != null && hitFil == _targetFil)
            {
                _targetFil.DecreaseScale();
                _isScaled = true;
            }
        }
    }

    public override void UnEquipMask()
    {
        base.UnEquipMask();

        if (_isScaled && _targetFil != null)
        {
            _targetFil.IncreaseScale();
            _isScaled = false;
        }
    }
}