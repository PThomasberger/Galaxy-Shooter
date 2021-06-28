using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float _shakeAmount = 0.2f;
    [SerializeField]
    private float _shakeDuration = 0f;
    [SerializeField]
    private float _decreaseFactor = 1f;
    private Vector3 _originalPos;

    private void Update()
    {
        MainCameraShake();
    }

    private void MainCameraShake()
    {
        _originalPos = new Vector3(0, 1, -10);

        if (_shakeDuration > 0)
        {
            transform.position = _originalPos + Random.insideUnitSphere * _shakeAmount;
            _shakeDuration -= _decreaseFactor * Time.deltaTime;
        }
        else
        {
            _shakeDuration = 0f;
            transform.position = _originalPos;
        }
    }

    public void CameraShakeActive()
    {
        _shakeDuration = 0.2f;
    }
}
