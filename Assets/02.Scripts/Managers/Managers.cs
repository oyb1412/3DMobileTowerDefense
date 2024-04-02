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
    private Data _data = new Data();
    private PoolManager _pool = new PoolManager();
    private CreatorManager _creator = new CreatorManager();
    private InputManager _input = new InputManager();
    private ResourcesManager _resources = new ResourcesManager();
    private SceneManagerEX _scene = new SceneManagerEX();
 
    public UICreator creator => GameObject.Find("UI_Creator").GetComponent<UICreator>();

    public static CreatorManager Creator => _instance._creator;
    public static PoolManager Pool => _instance._pool;
    public static Data Data => _instance._data;
    public static SceneManagerEX Scene => _instance._scene;
    public static InputManager Input => Instance._input;
    public static ResourcesManager Resources => Instance._resources;


    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        Input.OnUpdate();
    }

  
    private static void Init()
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
            
            Pool.Init();
            Data.Init();
        }
    }
}
