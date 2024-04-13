using UnityEngine;

/// <summary>
/// 메인 씬
/// </summary>
public class MainScene : BaseScene {
    public override void Clear() { }

    public override void Init() {
        base.Init();
        Managers.Init();
        SceneType = Define.SceneType.Main;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Confined;  //pc환경일 경우 커서 탈출 불가 설정
#endif
    }

    protected override void Start() {
        base.Start();
        Managers.Audio = GameObject.Find("@AudioManager").GetComponent<AudioManager>();
        Managers.Audio.SetBgm(true, Define.BgmType.Main);
    }
}
