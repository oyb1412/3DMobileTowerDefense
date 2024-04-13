using UnityEngine;

/// <summary>
/// ���� ��
/// </summary>
public class MainScene : BaseScene {
    public override void Clear() { }

    public override void Init() {
        base.Init();
        Managers.Init();
        SceneType = Define.SceneType.Main;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        Cursor.lockState = CursorLockMode.Confined;  //pcȯ���� ��� Ŀ�� Ż�� �Ұ� ����
#endif
    }

    protected override void Start() {
        base.Start();
        Managers.Audio = GameObject.Find("@AudioManager").GetComponent<AudioManager>();
        Managers.Audio.SetBgm(true, Define.BgmType.Main);
    }
}
