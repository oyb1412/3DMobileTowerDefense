using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] float _wheelSpeed;

    public Camera Camera => _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        if (!GameSystem.Instance.IsPlay())
            return;

        ZoomInandOut();
        ClickPosToMove();
    }

    public void CameraShake() {
        transform.DOShakeRotation(0.5f, 3f);
    }
    private void ClickPosToMove() {

    }
    private void ZoomInandOut() {
        float scroll = Input.GetAxis("Mouse ScrollWheel") * _wheelSpeed;

        _camera.fieldOfView -= scroll;

        if (_camera.fieldOfView >= 60f)
            _camera.fieldOfView = 60f;
        else if (_camera.fieldOfView <= 40f)
            _camera.fieldOfView = 40f;
    }
}
