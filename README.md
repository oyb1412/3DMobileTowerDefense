## **📃핵심 기술**

### ・모바일 & PC 크로스 플랫폼 인풋 시스템

🤔**WHY?**

모바일 & PC 각각에 알맞는 인풋 시스템을 구현해, 어떤 기기에서도 게임을 즐길 수 있는 환경을 만들기 위해 사용하였습니다.

🤔**HOW?**

 관련 코드

- CameraController
    
    ```csharp
        private void CameraMove() {
    #if UNITY_EDITOR || UNITY_STANDALONE_WIN  //PC환경에서의 카메라 이동 처리
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
    
    #elif UNITY_ANDROID  //안드로이드 환경에서의 카메라 이동 처리
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
    
    ```
    
- CameraController
    
    ```csharp
        private void CameraMove() {
    #if UNITY_EDITOR || UNITY_STANDALONE_WIN  //PC환경에서의 카메라 이동 처리
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
    
    #elif UNITY_ANDROID  //안드로이드 환경에서의 카메라 이동 처리
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
    
    ```
    

🤓**Result!**

각 플랫폼에 맞게 자동으로 전환되는 인풋 시스템으로, 모바일, PC양방향 기기에서 모두 플레이가 가능하게 되었습니다.

### ・JSON을 이용한 데이터 관리

🤔**WHY?**

게임상의 모든 데이터를 Data라는 정적 클래스에 저장하여 보안이 취약하고 유지보수가 힘들다고 느껴 외부에서의 관리의 필요성을 느꼈습니다.

🤔**HOW?**

 관련 코드

- TowerData.json
    
    ```csharp
    {
      "TowerIconPath": [
        [
          "Sprites/Towers/ArcherTower/ArcherTower_Lvl1",
          "Sprites/Towers/ArcherTower/ArcherTower_Lvl2",
          "Sprites/Towers/ArcherTower/ArcherTower_Lvl3",
          "Sprites/Towers/ArcherTower/ArcherTower_Lvl4"
        ],
        [
          "Sprites/Towers/CanonTower/CanonTower_Lvl1",
          "Sprites/Towers/CanonTower/CanonTower_Lvl2",
          "Sprites/Towers/CanonTower/CanonTower_Lvl3",
          "Sprites/Towers/CanonTower/CanonTower_Lvl4"
        ],
        [
          "Sprites/Towers/MagicTower/MagicTower_Lvl1",
          "Sprites/Towers/MagicTower/MagicTower_Lvl2",
          "Sprites/Towers/MagicTower/MagicTower_Lvl3",
          "Sprites/Towers/MagicTower/MagicTower_Lvl4"
        ],
        [
          "Sprites/Towers/DeathTower/DeathTower_Lvl1",
          "Sprites/Towers/DeathTower/DeathTower_Lvl2",
          "Sprites/Towers/DeathTower/DeathTower_Lvl3",
          "Sprites/Towers/DeathTower/DeathTower_Lvl4"
        ]
      ],
      "_towerCost": [
        [
          70,
          140,
          210,
          280
        ],
        [
          80,
          160,
          240,
          320
        ],
        [
          75,
          150,
          230,
          300
        ],
        [
          100,
          170,
          240,
          310
        ]
      ],
      "_sellCost": [
        [
          35,
          75,
          110,
          150
        ],
        [
          40,
          85,
          125,
          175
        ],
        [
          33,
          80,
          115,
          160
        ],
        [
          50,
          90,
          150,
          210
        ]
      ],
      "_towerCreateTime": [
        [
          3.0,
          5.0,
          7.0,
          9.0
        ],
        [
          4.0,
          6.0,
          8.0,
          10.0
        ],
        [
          3.0,
          5.0,
          7.0,
          9.0
        ],
        [
          6.0,
          8.0,
          10.0,
          10.0
        ]
      ],
      "_towerDamage": [
        [
          8,
          20,
          50,
          80
        ],
        [
          12,
          30,
          55,
          105
        ],
        [
          15,
          40,
          65,
          95
        ],
        [
          7,
          18,
          40,
          55
        ]
      ],
      "_towerAttackDelay": [
        [
          1.2,
          1.1,
          1.0,
          1.0
        ],
        [
          2.5,
          2.4,
          2.3,
          2.2
        ],
        [
          1.6,
          1.5,
          1.4,
          1.4
        ],
        [
          0.8,
          0.7,
          0.6,
          0.5
        ]
      ],
      "_towerAttackRange": [
        [
          10.0,
          10.0,
          12.0,
          12.0
        ],
        [
          6.0,
          6.0,
          8.0,
          8.0
        ],
        [
          8.0,
          8.0,
          10.0,
          10.0
        ],
        [
          10.0,
          10.0,
          12.0,
          12.0
        ]
      ]
    }
    ```
    

🤓**Result!**

모든 데이터를 외부 파일에 별도 저장 후 게임 실행시 파일을 로드하는 방식으로 변경해 보다 높아진 보안성과 함께 데이터의 추가 / 변경 / 제거 등 유지보수가 보다 쉬워졌습니다.

### ・게임 자동저장 기능

🤔**WHY?**

게임 도중 게임 종료 시, 게임 데이터가 저장되지 않고 모두 삭제되어, 재 시작 시 처음부터 시작해야 한다는 문제가 발생했기에 추가했습니다.

🤔**HOW?**

 관련 코드

- GameSystem
    
    ```csharp
    public class GameSystem : MonoBehaviour
    {
     public void SaveGameData() {
            int round = GameRound - 1;
            int gold = CurrentGold;
            int hp = GameHp;
            int score = GameScore;
            var conData = ConObjects;
            var towerData = TowerObjects;
    
            foreach (var tower in towerData)   //현재 위치를 저장
                tower.Value.SetPosition();
            foreach (var con in conData) 
                con.Value.SetPosition();
    
            var savedata = new Data.GameSystemData(round, gold, hp, score, conData, towerData);  //데이터 생성
    
            string gamedata = JsonConvert.SerializeObject(savedata, Formatting.Indented);
            Managers.Data.SaveJson(Application.persistentDataPath, "SaveData", gamedata);  //Json으로 저장
        }
    }
    ```
    
- GameScene
    
    ```csharp
    public class GameScene : BaseScene
    {
    	public override void Init() {
        base.Init();
        Managers.Init();
    
        SceneType = Define.SceneType.InGame;
        Managers.Spawn.Init();
        Managers.Audio.SetBgm(true, Define.BgmType.Ingame);
    
        if(Managers.Scene.isContinue) {  //이어하기를 했을 경우, 데이터를 불러옴
            Dictionary<int, GameSystem.ConData> conData = Managers.Data.LoadJson<Data.GameSystemData>(Application.persistentDataPath, "SaveData").ConData;
            Dictionary<int, GameSystem.TowerData> towerData = Managers.Data.LoadJson<Data.GameSystemData>(Application.persistentDataPath, "SaveData").TowerData;
    
            foreach(var item in conData) {
                GameSystem.ConData loadData = item.Value;
                int level = loadData.Level;
                Vector3 pos = new Vector3(loadData.PosX, -0.5f, loadData.PosZ);
    
                Define.TowerType type = (Define.TowerType)loadData.Type;
                string path = $"Towers/{type.ToString()}/{type.ToString()}_Lvl{level}Cons";
                ConBase go = Managers.Resources.Instantiate(path, null).GetComponent<ConBase>();
                go.Init(pos, loadData.KillNumber);
                GameSystem.Instance.AddConObject(go, type, level, loadData.KillNumber);
            }
    
            foreach(var item in towerData) {
                GameSystem.TowerData loadData = item.Value;
                int level = loadData.Level;
                int kill = loadData.KillNumber;
                Vector3 pos = new Vector3(loadData.PosX, -0.5f, loadData.PosZ);
    
                Define.TowerType type = (Define.TowerType)loadData.Type;
                var towerdata = loadData as GameSystem.TowerData;
                string path = $"Towers/{type.ToString()}/{type.ToString()}_Lvl{level}";
                TowerBase go = Managers.Resources.Instantiate(path, null).GetComponent<TowerBase>();
                go.Init();
                go.TowerStatus.Init(kill, level, pos, type);
                GameSystem.Instance.AddTowerObject(go, type, level);
            }
            GameSystem.Instance.Continue();
            return;
        }
        GameSystem.Instance.Init();
    }
    }
    ```
    

🤓**Result!**

게임 도중 게임 종료 시, 자동으로 이전 라운드의 모든 데이터를 저장. 이후 게임 재 시작 시, 기존의 게임 시작 버튼이 아닌 이어하기 버튼이 활성화되고, 이어하기를 선택해 이전 게임의 내용 그대로 게임을 재개할 수 있게 되었습니다.

### ・적의 생성을 JSON데이터로 관리 및 자동 생성

🤔**WHY?**

적을 생성할 때 마다 적의 레벨, 공격력 등 수치를 수동으로 정의하고, 그 수치에 맞춰 적을 생성해, 적 생성 메서드의 종류가 늘어나고 코드가 길어지는 문제가 발생해 자동생성 로직을 생각했습니다.

🤔**HOW?**

 관련 코드

- SpawnManager
    
    ```csharp
    public class SpawnManager
    {
    	IEnumerator CoSpwan(int currentGameLevel, ParticleSystem effect) {
        List<Data.EnemySpawnData> spawnData = _data.GetEnemySpawnData(currentGameLevel);  //현재 레벨에 맞는 데이터 추출
        effect.Play();
    
        foreach (var data in spawnData) {
            for(int i = 0; i<data.Count; i++) {
                if (!GameSystem.Instance.IsPlay())  //게임 종료시 정지
                    GameSystem.Instance.StopAllCoroutines();
    
                _enemyNumber++;
    
                int level = (int)data.EnemyLevel;
                int type = (int)data.EnemyType;
                EnemyController enemy = Managers.Resources.Instantiate(_enemyObject[type, level], null).GetComponent<EnemyController>();
                enemy.Init(_spawnPoint.position, _enemyNumber);
                yield return new WaitForSeconds(SPAWN_DELAY);
            }
        }
        effect.Stop();
      }
    }
    ```
    

🤓**Result!**

미리 JSON에 적의 각종 수치 데이터를 모두 정의 후, 그 데이터를 기반으로 자동으로 적을 생성하는 로직을 구현해, 데이터의 변경만으로 각종 적을 생성할 수 있게 되어 코드 압축 및 유지보수의 용이성이 증가했습니다.

### ・공통된 ‘선택’ 이라는 기능의 추상화

🤔**WHY?**

게임 내에서 클릭이나 터치로 선택할 수 있는 오브젝트(적, 타워, 땅 등)를 모두 별개의 객체로 취급하여, 같은 ‘선택’이라는 기능이지만 모두 별개의 코드로 제어하게 되어 코드의 양이 증가하고 코드의 수정이 필요해졌을 때 모든 부분을 수정해 유지보수가 힘든 문제가 발생했기에 사용했습니다.

🤔**HOW?**

 관련 코드

- SelectSystem
    
    ```csharp
    public class SelectSystem : MonoBehaviour
    {
        [SerializeField]private LayerMask SelectLayer;  //선택할 오브젝트들의 레이어
        private ISelectedObject _lastSelectObject;  //선택한 오브젝트
        private float _touchTime;  
        private bool _isLongTouch;
        private bool _isTouch;
    
        private void Update() {
    
    #if UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Input.GetMouseButtonDown(0)) {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
    
                Select();
            }
    #elif UNITY_ANDROID
            if (Input.touchCount == 1 ) {
                Touch touch = Input.GetTouch(0);
    
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;
    
                switch (touch.phase) {
                    case TouchPhase.Began:
                        if(!_isTouch) {
                            _touchTime = Time.time;
                            _isLongTouch = false;
                            _isTouch = true;
                        }
                        
                        break;
    
                    case TouchPhase.Stationary:
                        if (!_isTouch) return;
    
                        if(Time.time - _touchTime > 0.1f && !_isLongTouch) {
                            _isLongTouch = true;
                        }
                        break;
    
                    case TouchPhase.Ended:
                        if(!_isTouch) return;
    
                        if (!_isLongTouch) {
                            Select();
                        }
    
                        _isTouch = false;
                    break;
                }
            }
    
    #endif
    
        }
    
        /// <summary>
        /// 오브젝트 선택
        /// </summary>
        private void Select() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool col = Physics.Raycast(ray, out var hit, float.MaxValue, SelectLayer);
            DeSelect();  //선택 해제
            
            if (col) {  //감지된 오브젝트 선택
                Managers.Audio.PlaySfx(Define.SfxType.ObjectSelect);
                if (hit.collider.TryGetComponent<ISelectedObject>(out _lastSelectObject))
                    _lastSelectObject.OnSelect();
            }
        }
    
        /// <summary>
        /// 선택 해제
        /// </summary>
        private void DeSelect() {
            if (_lastSelectObject == null)
                return;
    
            if (!_lastSelectObject.IsValid())
                return;
    
            _lastSelectObject.OnDeSelect();  //선택된 오브젝트가 존재하면, 선택 해제
            _lastSelectObject = null;
        }
    
    }
    ```
    

🤓**Result!**

‘선택’이라는 특징으로 공통되는 모든 객체들을 하나로 묶어 추상화해, 이들 모두를 하나의 특징으로 묶고, 실제 ‘선택’의 기능은 각 자식 클래스에서 정의해 코드의 양 감소 및 유지보수가 용이하게 되었습니다.

### ・옵저버 패턴을 이용한 UI 시스템

🤔**WHY?**

 데이터의 변경이 없음에도 주기적으로 UI를 Update해, 필요없는 메소드 호출이 계속 반복되어 퍼포먼스가 하락했기에 사용했습니다.

🤔**HOW?**

 관련 코드

- PlayerController
    
    ```csharp
    public class PlayerController : UnitBase, ITakeDamage
    {
    	#region Event // 특정 상황에 호출되어야 하는 이벤트들을 정의
        public Action ShotEvent;
        public Action<float> CrossValueEvent;
        public Action<int, int, int> HpEvent;
        public Action<int, int, int> BulletEvent;
        public Action<Player.WeaponController> ChangeEvent;
        public Action<Transform, Transform> HurtEvent;
        public Action DeadEvent;
        public Action<int> KillEvent;
        public Action RespawnEvent;
        public Action<bool> HurtShotEvent;
        public Action<bool> ShowScoreboardEvent;
        public Action ShowMenuEvent;
        public Action ShowSettingEvent;
        public Action<bool> CollideItemEvent;
        public Action<bool> SetAimEvent;
        public Action<int> ChangeCrosshairEvent;
        public Action<DirType, string, bool, bool> ShowKillAndDeadTextEvent;
        #endregion
        
        public override void ChangeWeapon(WeaponType type) {
            
            base.ChangeWeapon(type);
            CrossValueEvent.Invoke(CurrentWeapon.CrossValue); // 특정 행동에 맞춰 이벤트 발동
            ChangeEvent.Invoke(CurrentWeapon);
        }
     }
    ```
    
- UI_Base
    
    ```csharp
    public class UI_Base : MonoBehaviour {
        protected PlayerController _player;
        public void SetPlayer(PlayerController playerController) {
            _player = playerController; //플레이어를 생성함과 동시에 플레이어 변수에 할당
        }
        private void Start() {
            Init();
        }
        protected virtual void Init() { }
    }
    ```
    
- UI_
    
    ```csharp
    public class UI_Crosshair : UI_Base
    {
    		protected override void Init() {
            base.Init();
            _player.CrossValueEvent -= SetCross; // 모든 UI 컴포넌트에서 이벤트 구독
            _player.CrossValueEvent += SetCross;
        }
     }
    ```
    

🤓**Result!**

타워나 적이 생성되는 순간 이벤트에 UI를 구독시켜, 데이터의 변화가 발생할 때에만 UI를 Update해, 필요없는 메소드 호출을 억제해 퍼포먼스가 상승했습니다.

### ・씬 전환 페이드 시스템

🤔**WHY?**

씬 전환시 아무 연출없이 즉각적으로 화면이 전환되어 화면이 갈아끼워지는듯한 느낌을 받는다는 피드백을 받아, 보다 극적인 연출을 위해 사용하였습니다.

🤔**HOW?**

 관련 코드

- UI_Fade
    
    ```csharp
    public class UI_Fade : MonoBehaviour
    {
        private const float FADE_TIME = 1f;
        private Image _fadeImage;
    
        private void Awake() {
            _fadeImage = GetComponentInChildren<Image>();
            _fadeImage.color = Color.black;
        }
    
        /// <summary>
        /// trigger판정에 맞춰 페이드 실행
        /// </summary>
        public Tween SetFade(bool trigger) {
            if(trigger) {
                return _fadeImage.DOFade(1f, FADE_TIME);
            } else
                return _fadeImage.DOFade(0, FADE_TIME);
        }
    }
    ```
    
- SceneManagerEX
    
    ```csharp
    public class SceneManagerEX
    {
        
        public void LoadScene(Define.SceneType type)
    	 {
         var tween = _fade.SetFade(true);
         if(type == Define.SceneType.Exit) {
             tween.OnComplete(DoNextGame);
         }
         else
             tween.OnComplete(() => DoNextScene(type));
    	 }
    
    }
    ```
    

🤓**Result!**

  씬 전환 시 즉각적인 전환이 아닌, 화면이 가려지고 씬이 전환되고 화면이 밝아지고 게임이 시작되는 등 페이드 연출을 추가해, 사용자가 어색한 느낌을 받지 않도록 하였습니다.

### ・풀링 오브젝트 시스템

🤔**WHY?**

각종 오브젝트를 필요할 때 마다 생성, 필요가 없어지면 제거해 짧은 시간 내에 다량의 객체를 생성하고 제거하는 상황이 반복되 퍼포먼스가 크게 하락하였기에 사용했습니다.

🤔**HOW?**

 관련 코드

- PoolManager
    
    ```csharp
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;
    
    public class PoolManager
    {
        class Pool
        {
            public GameObject Original { get; private set; }
            public Transform Root { get; private set; }
            private Stack<Poolable> _poolStack = new Stack<Poolable>();
    
            public void Init(GameObject original, int count = 5)
            {
                Original = original;
                Root = new GameObject().transform;
                Root.name = original.name + "_root";
    
                for (int i = 0; i < count; i++)
                {
                    Release(Create());
                }
            }
    
            
    
            private Poolable Create()
            {
                GameObject go = Object.Instantiate(Original);
                go.name = Original.name;
                return go.GetOrAddComponent<Poolable>();
            }
    
            public void Release(Poolable poolable)
            {
                if (poolable == null)
                    return;
    
                poolable.transform.parent = Root;
                poolable.gameObject.SetActive(false);
                _poolStack.Push(poolable);
            }
    
            public Poolable Activation()
            {
                Poolable poolable;
    
                if (_poolStack.Count > 0)
                    poolable = _poolStack.Pop();
                else
                    poolable = Create();
                
                poolable.gameObject.SetActive(true);
    
                poolable.transform.parent = Managers.Scene.CurrentSceneManager.transform;
    
                poolable.transform.parent = Root;
    
                return poolable;
            }
    
        }
        
        private Transform _root;
        private Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    
        public void Init()
        {
            if (_root == null)
            {
                _root = new GameObject("@Pool_root").transform;
                Object.DontDestroyOnLoad(_root.GameObject());
            }
        }
    
        public void Release(Poolable poolable)
        {
            string name = poolable.gameObject.name;
    
            if (_pools.ContainsKey(name) == false)
            {
                Object.Destroy(poolable.gameObject);
                return;
            }
            
            _pools[name].Release(poolable);
        }
    
        public void Clear() {
            foreach(Transform t in _root) {
                Managers.Resources.Destroy(t.gameObject);
            }
            _root = null;
            _pools.Clear();
        }
    
        public Poolable Activation(GameObject original)
        {
            if(_pools.ContainsKey(original.name) == false)
                CreatePool(original);
    
            return _pools[original.name].Activation();
        }
    
        private void CreatePool(GameObject original, int count = 5)
        {
            Pool pool = new Pool();
            pool.Init(original,count);
            pool.Root.parent = _root;
            
            _pools.Add(original.name, pool);
        }
    
        public GameObject GetOriginal(string name)
        {
            if (_pools.ContainsKey(name) == false)
                return null;
    
            return _pools[name].Original;
        }
    }
    ```
    

🤓**Result!**

  시스템에 큰 부하를 주는 객체의 직접적인 생성 및 파괴를 최대한 피하고 풀링 시스템을 이용, 이미 생성된 객체를 재사용하는 과정을 통해 객체의 생성에 들어가는 비용을 줄여 퍼포먼스가 크게 상승했습니다.

## 📈보완점

🤔**문제점**

모바일 환경에서 평균 30프레임 가량의 낮은 퍼포먼스로 게임 플레이동안 이질감이 크게 느껴짐

🤔**문제의 원인**

에셋 스토어에서 구매한 발사체 에셋에 과도한 파티클 효과 및 쉐이더 효과, 또한 스크립트 내부에서 과도하게 많은 충돌계산을 하고있어 시스템에 큰 부하를 입히고 있었음

🤓**해결방안**

쉐이더를 모바일 환경에 알맞는 쉐이더로 교체 후 파티클 효과에서 큰 비중을 차지하는 Max Partcle수를 줄이고, 과도한 충돌계산을 피하기 위해 각종 조건체크로 쓸데없는 충돌체크를 줄임. 그 후 마지막으로 충돌 후 생기는 Hit이펙트 또한 동일하게 최적화 해 모바일 환경에서도 최소 60프레임 이상의 퍼포먼스 유지 성공
