using UnityEngine;

public class Fil : MonoBehaviour
{
    [Header("Boyut Ayarları")]
    [SerializeField] private float _scaleDecreaseAmount = 0.2f;
    [SerializeField] private float _minScale = 0.2f;
    [SerializeField] private float _scaleRestoreTime = 1.0f;
    [SerializeField] private float _scaleDecreaseTime = 0.1f;

    [Header("Büyüme Ayarları")]
    [SerializeField] private float _growthSpeed = 0.05f;

    private Vector3 _originalScale;
    private Vector3 _currentScale;
    private Vector3 _targetScale;
    private bool _isRestoring = false;
    private bool _isDecreasing = false;
    private float _restoreProgress = 0f;
    private float _decreaseProgress = 0f;

    private void Start()
    {
        _originalScale = transform.localScale;
        _currentScale = _originalScale;
    }

    private void Update()
    {
        if (_isRestoring)
        {
            RestoreScale();
        }
        else
        {
            if (transform.localScale.x < _originalScale.x)
            {
                transform.localScale += Vector3.one * _growthSpeed * Time.deltaTime;

                if (transform.localScale.x > _originalScale.x)
                {
                    transform.localScale = _originalScale;
                }

                _currentScale = transform.localScale;
            }

            if (_isDecreasing)
            {
                DecreaseScaleSmooth();
            }
        }
    }

    public void IncreaseScale()
    {
        _isRestoring = true;
        _restoreProgress = 0f;
        _isDecreasing = false;
    }

    public void DecreaseScale()
    {
        _isRestoring = false;

        float sizeRatio = _currentScale.x / _originalScale.x;
        float dynamicDecrease = _scaleDecreaseAmount * sizeRatio;

        _targetScale = _currentScale - Vector3.one * dynamicDecrease;

        float minLimit = _originalScale.x * _minScale;
        if (_targetScale.x < minLimit) _targetScale = Vector3.one * minLimit;

        _isDecreasing = true;
        _decreaseProgress = 0f;
    }

    private void DecreaseScaleSmooth()
    {
        _decreaseProgress += Time.deltaTime / _scaleDecreaseTime;

        if (_decreaseProgress >= 1f)
        {
            _decreaseProgress = 1f;
            _isDecreasing = false;
        }

        _currentScale = Vector3.Lerp(_currentScale, _targetScale, _decreaseProgress);
        transform.localScale = _currentScale;
    }

    private void RestoreScale()
    {
        _restoreProgress += Time.deltaTime / _scaleRestoreTime;

        if (_restoreProgress >= 1f)
        {
            _restoreProgress = 1f;
            _isRestoring = false;
            _currentScale = _originalScale;
        }

        _currentScale = Vector3.Lerp(_currentScale, _originalScale, _restoreProgress);
        transform.localScale = _currentScale;
    }
}