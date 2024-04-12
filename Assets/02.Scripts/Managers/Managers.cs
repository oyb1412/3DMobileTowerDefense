using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers _instance;
    public static Managers Instance {
        get {
            Init();
            return _instance;
        }
    }

    public static CameraController MainCamera;
    public static AudioManager Audio;
    private Data _data = new Data();
    private CursorManager _cursor = new CursorManager();
    private SpawnManager _spawn = new SpawnManager();
    private PoolManager _pool = new PoolManager();
    private CreatorManager _creator = new CreatorManager();
    private InputManager _input = new InputManager();
    private ResourcesManager _resources = new ResourcesManager();
    private SceneManagerEX _scene = new SceneManagerEX();
    private LanguageManager _language = new LanguageManager();
 
    public UICreator creator => GameObject.Find("UI_Creator").GetComponent<UICreator>();

    public static CursorManager Cursor => _instance._cursor;
    public static CreatorManager Creator => _instance._creator;
    public static SpawnManager Spawn => _instance._spawn;
    public static PoolManager Pool => _instance._pool;
    public static Data Data => _instance._data;
    public static SceneManagerEX Scene => _instance._scene;
    public static LanguageManager Language => _instance._language;
    public static InputManager Input => Instance._input;
    public static ResourcesManager Resources => Instance._resources;


    private void Awake()
    {
    }

    private void Update()
    {
        Input.OnUpdate();
    }

    public void Clear() {
        Language.Clear();
    }
  
    public static void Init()
    {
        if (_instance == null)
        {
            GameObject managers = GameObject.Find("@Managers");
            if (managers == null)
            {
                managers = new GameObject("@Managers");
                managers.AddComponent<Managers>();
            }
            
            DontDestroyOnLoad(managers);
            _instance = managers.GetComponent<Managers>();

            Audio = GameObject.Find("@AudioManager").GetComponent<AudioManager>();
            Data.Init();
        }
        Scene.Init();
        Pool.Init();
        MainCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();

    }


}
