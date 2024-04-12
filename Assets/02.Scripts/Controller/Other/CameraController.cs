using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ī�޶� Ȯ��,�̵��� ���� ��Ʈ�ѷ�
/// </summary>
public class CameraController : MonoBehaviour
{
    private const float CAMERA_SHAKE_TIME = 0.5f;  //�⺻ ī�޶� ����ũ �ð�
    private const float CAMERA_SHAKE_VALUE = 1.5f;  //�⺻ ī�޶� ����ũ Value
    private const float DEFAULT_CAMERA_MOVE_VALUE = 50f;  //�������� ���� ���� ��, �⺻�� 1.0f�� ������ ��
    private const float DEFAULT_CAMERA_ZOOM_VALUE = 10f;  //�������� ���� ���� ��, �⺻�� 1.0f�� ������ ��
    private const float CAMERA_MOVEAREA_PLUS = 0.9f;  //����� / ��ũ�� ũ�� ��ġ�� ���콺 ��ġ �� ī�޶� +�̵�
    private const float CAMERA_MOVEAREA_MINUS = 0.1f;  //����� / ��ũ�� ũ�� ��ġ�� ���콺 ��ġ �� ī�޶� -�̵�
    private const float TOUCH_MOVEVALUE_MINUS = 4f;  //���콺 �̵��� ��ġ�̵��� �� ����
    private const float LIMIT_POS = 40f;  //���� ��ġ�� �������� �߰��� �̵� ������ �ִ� �Ÿ�
    private const float TOUCH_ZOOMINOUT_VALUE = 0.02f;  //���콺 �ܰ� ��ġ���� �� ����
    private const float CAMERA_ZOOMIN_LIMIT = 40f;  //���콺 �ܰ� ��ġ���� �� ����
    private const float CAMERA_ZOOMOUT_LIMIT = 80f;  //���콺 �ܰ� ��ġ���� �� ����
    private float _limitRight;  //�߰��� �̵� ������ RightPos
    private float _limitLeft;  //�߰��� �̵� ������ LeftPos
    private float _limitUp;  //�߰��� �̵� ������ UpPos
    private float _limitDown;  //�߰��� �̵� ������ DownPos
    private float _initialDistance;  //��ġ �� ��,�ƿ� �� �� ��ġ ������ �� 
    private Vector2 _touchStartPos;  //��ġ�� ������ ��ġ

    private Camera _camera;  //���� ī�޶�
    private float _ZoomInOutValue = 5;  //Ȯ�� �� ��� Value
    private float _moveValue = 25;  //�̵� Value

    public Camera Camera => _camera; //ī�޶� ������Ƽ
 
    /// <summary>
    /// �⺻ ���� �ʱ�ȭ
    /// </summary>
    void Start()
    {
        float saveSens = PlayerPrefs.GetFloat(Define.SENSITYVITY);  //���࿡, �ٸ� ������ ���������� �����ߴٸ�
        if(saveSens != float.MaxValue)
            SetCameraSens(saveSens);  //���� ����

        _camera = GetComponent<Camera>();  //ī�޶� Get

        Vector3 defaultPos = transform.position;  //���� ��ġ�� �������, �ִ� �̵� ������ ��ġ�� ���
        _limitRight = defaultPos.x + LIMIT_POS;
        _limitLeft = defaultPos.x - LIMIT_POS;
        _limitUp = defaultPos.z + LIMIT_POS;
        _limitDown = defaultPos.z - LIMIT_POS;
    }

    /// <summary>
    /// ī�޶� �̵��� �� ��,�ƿ�
    /// </summary>
    void Update()
    {
        if (Managers.Scene.CurrentScene is not GameScene)  //�ΰ����� �ƴ϶�� return
            return;

        if (!GameSystem.Instance.IsPlay())  //������ ����Ǿ����� return
            return;

        ZoomInandOut();  //ī�޶� �� ��,�ƿ�
        CameraMove();  //ī�޶� �̵�
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="sens">������</param>
    public void SetCameraSens(float sens) {
        float wheel = sens * DEFAULT_CAMERA_ZOOM_VALUE;  //�ִ밪 1.0f�� ���� ����
        float move = sens * DEFAULT_CAMERA_MOVE_VALUE;  //�ִ밪 1.0f�� ���� ����
        _ZoomInOutValue = wheel;  //����� ������ ���� ����
        _moveValue = move;  //����� ������ ���� ����
    }

    /// <summary>
    /// ī�޶� ����ũ
    /// </summary>
    public void CameraShake() => transform.DOShakeRotation(CAMERA_SHAKE_TIME, CAMERA_SHAKE_VALUE);  //ī�޶� ����ũ

    /// <summary>
    /// ī�޶� �̵�
    /// </summary>
    private void CameraMove() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN  //PCȯ�濡���� �̵� ó��
        var mousePos = Input.mousePosition;

        if (EventSystem.current.IsPointerOverGameObject())  //UI��ġ�� ���콺 ��ġ �� return
            return;

        //���� ���콺�� x��ġ���� ������� ī�޶� �¿� �̵�
        if (mousePos.x >= Screen.width - Screen.width * CAMERA_MOVEAREA_MINUS && _limitRight > transform.position.x)
            transform.position += Vector3.right * _moveValue * Time.deltaTime;
        if (mousePos.x <= Screen.width - Screen.width * CAMERA_MOVEAREA_PLUS && _limitLeft < transform.position.x)
            transform.position += Vector3.left * _moveValue * Time.deltaTime;

        //���� ���콺�� y��ġ���� ������� ī�޶� ���� �̵�
        if (mousePos.y >= Screen.height - Screen.height * CAMERA_MOVEAREA_MINUS && _limitUp > transform.position.z)
            transform.position += Vector3.forward * _moveValue * Time.deltaTime;
        if (mousePos.y <= Screen.height - Screen.height * CAMERA_MOVEAREA_PLUS && _limitDown < transform.position.z)
            transform.position += Vector3.back * _moveValue * Time.deltaTime;


#elif UNITY_ANDROID  //�ȵ���̵� ȯ�濡���� �̵� ó��
        if (Input.touchCount == 1) {  //�ϳ��� ��ġ�� �ԷµǾ�����
            Touch touch = Input.GetTouch(0);  //��ġ�� ����

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))  //UI ��ġ�� return
                return;

            if (touch.phase == TouchPhase.Began) {  //��ġ��
                _touchStartPos = touch.position;  //��ġ�� ��ġ ����
            }
            else if (touch.phase == TouchPhase.Moved) {  //��ġ �� �̵� ��
                Vector2 delta = (Vector2)touch.position - _touchStartPos;  //��ġ�� ��ġ���� �̵��� ��ġ������ ���� ���
                Vector3 dir = new Vector3(-delta.x, 0f, -delta.y) * _moveValue / TOUCH_MOVEVALUE_MINUS * Time.deltaTime;  //delta���Ͱ��� ������� �̵��� ���� ���� ���
                transform.position += dir;  //�̵�

                //�̵� ��ġ ����
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

                _touchStartPos = touch.position;  //��ġ �� �̵��� ��ġ�� ���ο� ���� ��ġ ��ġ�� ����
            }
        }
#endif
    }

    /// <summary>
    /// ī�޶� �� ��,�ƿ�
    /// </summary>
    private void ZoomInandOut() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN  //PC ȯ�濡���� �� ��,�ƿ� ó��
        float scroll = Input.GetAxis("Mouse ScrollWheel") * _ZoomInOutValue;  //���콺 �� ���� ȣ��

        _camera.fieldOfView -= scroll;  //���콺 �� ���� ������� �� ��,�ƿ�

        //�� ��,�ƿ� �� ����
        if (_camera.fieldOfView >= CAMERA_ZOOMOUT_LIMIT)
            _camera.fieldOfView = CAMERA_ZOOMOUT_LIMIT;
        else if (_camera.fieldOfView <= CAMERA_ZOOMIN_LIMIT)
            _camera.fieldOfView = CAMERA_ZOOMIN_LIMIT;

#elif UNITY_ANDROID  //�ȵ���̵� ȯ�濡���� �� ��,�ƿ� ó��
if(Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch(0);  //ù ��ġ ����
            Touch touchOne = Input.GetTouch(1);  //�ι�° ��ġ ����

            if (EventSystem.current.IsPointerOverGameObject(touchZero.fingerId) ||  //� ��ġ���� UI�� ��ġ������ return
                EventSystem.current.IsPointerOverGameObject(touchOne.fingerId))
                return;

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)  //ù ��ġ �� �� ��ġ ������ �Ÿ��� ���
                _initialDistance = Vector2.Distance(touchZero.position, touchOne.position);

            else if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved) {  //��ġ �� �̵� �� �� ��ġ ������ �Ÿ��� ���
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                if (Mathf.Approximately(_initialDistance, 0))  //���� ���� ��ġ�� ��ġ �� return
                    return;

                var factor = (currentDistance - _initialDistance) * _ZoomInOutValue * TOUCH_ZOOMINOUT_VALUE;  //��ġ �� �̵����� �� �� ���� ���

                _camera.fieldOfView -= factor;  //�� �� ��ŭ �� ��,�ƿ�
            }

            //�� ��,�ƿ� ����
            if (_camera.fieldOfView >= CAMERA_ZOOMOUT_LIMIT)
                _camera.fieldOfView = CAMERA_ZOOMOUT_LIMIT;
            else if (_camera.fieldOfView <= CAMERA_ZOOMIN_LIMIT)
                _camera.fieldOfView = CAMERA_ZOOMIN_LIMIT;
        }
#endif
    }
}
