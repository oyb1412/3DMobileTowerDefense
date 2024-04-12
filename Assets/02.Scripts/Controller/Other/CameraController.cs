using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    private const float _limitPos = 40f;
    [SerializeField] private float _limitRight;
    [SerializeField] private float _limitLeft;
    [SerializeField] private float _limitUp;
    [SerializeField] private float _limitDown;
    private Vector3 _defaultPos;

    private Camera _camera;
    private float initialDistance;
    private Vector2 _touchStartPos;
    private float _wheelSpeed = 5;
    private float _moveSpeed = 25;

    public Camera Camera => _camera;
    void Start()
    {
        _camera = GetComponent<Camera>();
        _defaultPos = transform.position;
        _limitRight = _defaultPos.x + _limitPos;
        _limitLeft = _defaultPos.x - _limitPos;
        _limitUp = _defaultPos.z + _limitPos;
        _limitDown = _defaultPos.z - _limitPos;
    }

    void Update()
    {
        if (Managers.Scene.CurrentScene is not GameScene)
            return;

        if (!GameSystem.Instance.IsPlay())
            return;



        ZoomInandOut();
        CameraMove();
    }


    public void SetCameraSens(float sens) {
        int wheel = (int)sens * 10;
        int move = (int)sens * 50;
        _wheelSpeed = wheel;
        _moveSpeed = move;
    }

    public void CameraShake() {
        transform.DOShakeRotation(0.5f, 3f);
    }
    private void CameraMove() {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        var mousePos = Input.mousePosition;
        if (mousePos.x >= Screen.width - Screen.width * .1f && _limitRight > transform.position.x) {
            transform.position += Vector3.right * _moveSpeed * Time.deltaTime;
        }
        if (mousePos.x <= Screen.width - Screen.width * .9f && _limitLeft < transform.position.x) {
            transform.position += Vector3.left * _moveSpeed * Time.deltaTime;
        }

        if (mousePos.y >= Screen.height - Screen.height * .1f && _limitUp > transform.position.z) {
            transform.position += Vector3.forward * _moveSpeed * Time.deltaTime;
        }
        if (mousePos.y <= Screen.height - Screen.height * .9f && _limitDown < transform.position.z) {
            transform.position += Vector3.back * _moveSpeed * Time.deltaTime;
        }


#elif UNITY_ANDROID
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Began) {
                _touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) {
                Vector2 delta = (Vector2)touch.position - _touchStartPos;
                Vector3 dir = new Vector3(-delta.x, 0f, -delta.y) * _moveSpeed / 4f * Time.deltaTime;
                transform.position += dir;

                if (_limitRight < transform.position.x) {
                    transform.position = new Vector3(_limitRight, transform.position.y, transform.position.z);
                } else if (_limitLeft > transform.position.x) {
                    transform.position = new Vector3(_limitLeft, transform.position.y, transform.position.z);
                }
                if (_limitUp < transform.position.z) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, _limitUp);
                } else if (_limitDown > transform.position.z) {
                    transform.position = new Vector3(transform.position.x, transform.position.y, _limitDown);
                }

                _touchStartPos = touch.position;
            }
        }
#endif
    }
    private void ZoomInandOut() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        float scroll = Input.GetAxis("Mouse ScrollWheel") * _wheelSpeed;

        _camera.fieldOfView -= scroll;

        if (_camera.fieldOfView >= 80f)
            _camera.fieldOfView = 80f;
        else if (_camera.fieldOfView <= 40f)
            _camera.fieldOfView = 40f;
#elif UNITY_ANDROID
if(Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (EventSystem.current.IsPointerOverGameObject(touchZero.fingerId))
                return;

            if (EventSystem.current.IsPointerOverGameObject(touchOne.fingerId))
                return;
            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began) {
                initialDistance = Vector2.Distance(touchZero.position, touchOne.position);
            }
            else if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved) {
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);
                if (Mathf.Approximately(initialDistance, 0))
                    return;

                var factor = (currentDistance - initialDistance) * 0.01f;

                _camera.fieldOfView -= factor;
            }

            
            if (_camera.fieldOfView >= 80f)
                _camera.fieldOfView = 80f;
            else if (_camera.fieldOfView <= 40f)
                _camera.fieldOfView = 40f;
        }
#endif
    }
}
