using StarterAssets;
using UnityEngine;
using System.Collections;

public class Mask : MonoBehaviour
{
    public bool IsActive { get; private set; }

    [SerializeField] private GameObject _maskGameObject;
    private Vector3 _targetFacePosition = new Vector3(0, 1.375f, 00);
    private Vector3 _restingPosition = new Vector3(0, 0.567f, 2.689f);
    [SerializeField] private float _transitionSpeed = 5f;

    [SerializeField] private float _stopDistanceThreshold = 1f;

    protected StarterAssetsInputs _input;
    private Coroutine _moveCoroutine;

    protected virtual void Awake()
    {
        _input = GetComponentInParent<StarterAssetsInputs>();
    }

    protected virtual void Update()
    {
        if (_input != null && _input.maskAction)
        {
            HandleMaskToggle();
        }
    }

    private void HandleMaskToggle()
    {
        _input.maskAction = false;
        IsActive = !IsActive;

        if (IsActive)
        {
            EquipMask();
        }
        else
        {
            UnEquipMask();
        }
    }

    public virtual void EquipMask()
    {
        MoveToPosition(_targetFacePosition);
    }

    public virtual void UnEquipMask()
    {
        MoveToPosition(_restingPosition);
    }

    private void MoveToPosition(Vector3 target)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
            _moveCoroutine = null;
        }


        _maskGameObject.SetActive(true);

        _moveCoroutine = StartCoroutine(LerpMask(target));
    }

    private IEnumerator LerpMask(Vector3 target)
    {
        while (Vector3.Distance(_maskGameObject.transform.localPosition, target) > _stopDistanceThreshold)
        {
            _maskGameObject.transform.localPosition = Vector3.Lerp(
                _maskGameObject.transform.localPosition,
                target,
                Time.deltaTime * _transitionSpeed
            );

            yield return null;
        }

        _maskGameObject.transform.localPosition = target;
        _moveCoroutine = null;
        _maskGameObject.SetActive(false);
    }

    public void SetMask(GameObject mask)
    {
        this._maskGameObject = mask.gameObject;
    }
}