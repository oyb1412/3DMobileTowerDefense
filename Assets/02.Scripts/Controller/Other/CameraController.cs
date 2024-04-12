using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 카메라 확대,이동을 위한 컨트롤러
/// </summary>
public class CameraController : MonoBehaviour
{
    private const float CAMERA_SHAKE_TIME = 0.5f;  //기본 카메라 셰이크 시간
    private const float CAMERA_SHAKE_VALUE = 1.5f;  //기본 카메라 셰이크 Value
    private const float DEFAULT_CAMERA_MOVE_VALUE = 50f;  //설정에서 감도 설정 시, 기본값 1.0f에 곱해줄 값
    private const float DEFAULT_CAMERA_ZOOM_VALUE = 10f;  //설정에서 감도 설정 시, 기본값 1.0f에 곱해줄 값
    private const float CAMERA_MOVEAREA_PLUS = 0.9f;  //상수값 / 스크린 크기 위치에 마우스 위치 시 카메라 +이동
    private const float CAMERA_MOVEAREA_MINUS = 0.1f;  //상수값 / 스크린 크기 위치에 마우스 위치 시 카메라 -이동
    private const float TOUCH_MOVEVALUE_MINUS = 4f;  //마우스 이동과 터치이동의 값 조절
    private const float LIMIT_POS = 40f;  //현재 위치를 기준으로 추가로 이동 가능한 최대 거리
    private const float TOUCH_ZOOMINOUT_VALUE = 0.02f;  //마우스 줌과 터치줌의 값 조절
    private const float CAMERA_ZOOMIN_LIMIT = 40f;  //마우스 줌과 터치줌의 값 조절
    private const float CAMERA_ZOOMOUT_LIMIT = 80f;  //마우스 줌과 터치줌의 값 조절
    private float _limitRight;  //추가로 이동 가능한 RightPos
    private float _limitLeft;  //추가로 이동 가능한 LeftPos
    private float _limitUp;  //추가로 이동 가능한 UpPos
    private float _limitDown;  //추가로 이동 가능한 DownPos
    private float _initialDistance;  //터치 줌 인,아웃 시 각 터치 사이의 값 
    private Vector2 _touchStartPos;  //터치를 시작한 위치

    private Camera _camera;  //메인 카메라
    private float _ZoomInOutValue = 5;  //확대 및 축소 Value
    private float _moveValue = 25;  //이동 Value

    public Camera Camera => _camera; //카메라 프로퍼티
 
    /// <summary>
    /// 기본 설정 초기화
    /// </summary>
    void Start()
    {
        float saveSens = PlayerPrefs.GetFloat(Define.SENSITYVITY);  //만약에, 다른 씬에서 감도설정을 변경했다면
        if(saveSens != float.MaxValue)
            SetCameraSens(saveSens);  //감도 설정

        _camera = GetComponent<Camera>();  //카메라 Get

        Vector3 defaultPos = transform.position;  //현재 위치를 기반으로, 최대 이동 가능한 위치를 계산
        _limitRight = defaultPos.x + LIMIT_POS;
        _limitLeft = defaultPos.x - LIMIT_POS;
        _limitUp = defaultPos.z + LIMIT_POS;
        _limitDown = defaultPos.z - LIMIT_POS;
    }

    /// <summary>
    /// 카메라 이동및 줌 인,아웃
    /// </summary>
    void Update()
    {
        if (Managers.Scene.CurrentScene is not GameScene)  //인게임이 아니라면 return
            return;

        if (!GameSystem.Instance.IsPlay())  //게임이 종료되었으면 return
            return;

        ZoomInandOut();  //카메라 줌 인,아웃
        CameraMove();  //카메라 이동
    }

    /// <summary>
    /// 감도 조절
    /// </summary>
    /// <param name="sens">감도값</param>
    public void SetCameraSens(float sens) {
        float wheel = sens * DEFAULT_CAMERA_ZOOM_VALUE;  //최대값 1.0f에 맞춰 재계산
        float move = sens * DEFAULT_CAMERA_MOVE_VALUE;  //최대값 1.0f에 맞춰 재계산
        _ZoomInOutValue = wheel;  //계산한 값으로 감도 변경
        _moveValue = move;  //계산한 값으로 감도 변경
    }

    /// <summary>
    /// 카메라 셰이크
    /// </summary>
    public void CameraShake() => transform.DOShakeRotation(CAMERA_SHAKE_TIME, CAMERA_SHAKE_VALUE);  //카메라 셰이크

    /// <summary>
    /// 카메라 이동
    /// </summary>
    private void CameraMove() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN  //PC환경에서의 이동 처리
        var mousePos = Input.mousePosition;

        if (EventSystem.current.IsPointerOverGameObject())  //UI위치에 마우스 위치 시 return
            return;

        //현재 마우스의 x위치값을 기반으로 카메라 좌우 이동
        if (mousePos.x >= Screen.width - Screen.width * CAMERA_MOVEAREA_MINUS && _limitRight > transform.position.x)
            transform.position += Vector3.right * _moveValue * Time.deltaTime;
        if (mousePos.x <= Screen.width - Screen.width * CAMERA_MOVEAREA_PLUS && _limitLeft < transform.position.x)
            transform.position += Vector3.left * _moveValue * Time.deltaTime;

        //현재 마우스의 y위치값을 기반으로 카메라 상하 이동
        if (mousePos.y >= Screen.height - Screen.height * CAMERA_MOVEAREA_MINUS && _limitUp > transform.position.z)
            transform.position += Vector3.forward * _moveValue * Time.deltaTime;
        if (mousePos.y <= Screen.height - Screen.height * CAMERA_MOVEAREA_PLUS && _limitDown < transform.position.z)
            transform.position += Vector3.back * _moveValue * Time.deltaTime;


#elif UNITY_ANDROID  //안드로이드 환경에서의 이동 처리
        if (Input.touchCount == 1) {  //하나의 터치만 입력되었을시
            Touch touch = Input.GetTouch(0);  //터치값 저장

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))  //UI 터치시 return
                return;

            if (touch.phase == TouchPhase.Began) {  //터치시
                _touchStartPos = touch.position;  //터치한 위치 저장
            }
            else if (touch.phase == TouchPhase.Moved) {  //터치 중 이동 시
                Vector2 delta = (Vector2)touch.position - _touchStartPos;  //터치한 위치에서 이동한 위치까지의 벡터 계산
                Vector3 dir = new Vector3(-delta.x, 0f, -delta.y) * _moveValue / TOUCH_MOVEVALUE_MINUS * Time.deltaTime;  //delta벡터값을 기반으로 이동을 위한 벡터 계산
                transform.position += dir;  //이동

                //이동 위치 제한
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

                _touchStartPos = touch.position;  //터치 후 이동한 위치를 새로운 시작 터치 위치로 지정
            }
        }
#endif
    }

    /// <summary>
    /// 카메라 줌 인,아웃
    /// </summary>
    private void ZoomInandOut() {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN  //PC 환경에서의 줌 인,아웃 처리
        float scroll = Input.GetAxis("Mouse ScrollWheel") * _ZoomInOutValue;  //마우스 휠 값을 호출

        _camera.fieldOfView -= scroll;  //마우스 휠 값을 계산으로 줌 인,아웃

        //줌 인,아웃 값 제한
        if (_camera.fieldOfView >= CAMERA_ZOOMOUT_LIMIT)
            _camera.fieldOfView = CAMERA_ZOOMOUT_LIMIT;
        else if (_camera.fieldOfView <= CAMERA_ZOOMIN_LIMIT)
            _camera.fieldOfView = CAMERA_ZOOMIN_LIMIT;

#elif UNITY_ANDROID  //안드로이드 환경에서의 줌 인,아웃 처리
if(Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch(0);  //첫 터치 정보
            Touch touchOne = Input.GetTouch(1);  //두번째 터치 정보

            if (EventSystem.current.IsPointerOverGameObject(touchZero.fingerId) ||  //어떤 터치던지 UI를 터치했을시 return
                EventSystem.current.IsPointerOverGameObject(touchOne.fingerId))
                return;

            if (touchZero.phase == TouchPhase.Began || touchOne.phase == TouchPhase.Began)  //첫 터치 시 각 터치 사이의 거리를 계산
                _initialDistance = Vector2.Distance(touchZero.position, touchOne.position);

            else if(touchZero.phase == TouchPhase.Moved || touchOne.phase == TouchPhase.Moved) {  //터치 후 이동 시 각 터치 사이의 거리를 계산
                var currentDistance = Vector2.Distance(touchZero.position, touchOne.position);

                if (Mathf.Approximately(_initialDistance, 0))  //거의 같은 위치를 터치 시 return
                    return;

                var factor = (currentDistance - _initialDistance) * _ZoomInOutValue * TOUCH_ZOOMINOUT_VALUE;  //터치 후 이동했을 시 그 차를 계산

                _camera.fieldOfView -= factor;  //그 차 만큼 줌 인,아웃
            }

            //줌 인,아웃 제한
            if (_camera.fieldOfView >= CAMERA_ZOOMOUT_LIMIT)
                _camera.fieldOfView = CAMERA_ZOOMOUT_LIMIT;
            else if (_camera.fieldOfView <= CAMERA_ZOOMIN_LIMIT)
                _camera.fieldOfView = CAMERA_ZOOMIN_LIMIT;
        }
#endif
    }
}
