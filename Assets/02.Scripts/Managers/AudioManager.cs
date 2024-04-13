using UnityEngine;

/// <summary>
/// ���� ����
/// </summary>
public class AudioManager : MonoBehaviour {

    private AudioClip[] bgmClip;  //���ӿ� �����ϴ� ��� bgm ���
    private const float DefaultBgmVolume = 0.5f;  //bgm�⺻ ����
    private AudioSource bgmPlayer;  //bgm�� �� ���� �ϳ��� ��� ����

    private const int SFX_CHANNELS = 10;  //�ѹ��� �ִ� ��� ������ ȿ���� ��
    private AudioClip[] sfxClips;  //���ӿ� �����ϴ� ��� sfx ���
    private const float DefaultSfxVolume = 0.5f;  //sfx�⺻ ����
    AudioSource[] sfxPlayers;  //���� sfx�� ���ÿ� ����ϱ� ���� �迭

    private void Start() {
        InitBgm();  //bgm �ʱ�ȭ
        InitSfx();  //sfx �ʱ�ȭ
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// bgm �ʱ�ȭ
    /// </summary>
    void InitBgm() {
        GameObject bgmObject = new GameObject("BgmPlayer");  //bgm player ����
        bgmObject.transform.parent = transform;  //�θ� manager�� ����
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  //����� �ҽ� �߰�
        bgmPlayer.playOnAwake = false;  //��� ��� ����
        bgmPlayer.loop = true;  //loop ����
        bgmPlayer.volume = DefaultBgmVolume;  //�⺻ ���� ����
        bgmClip = new AudioClip[(int)Define.BgmType.Count];  //

        for (int i = 0; i < bgmClip.Length; i++)
            bgmClip[i] = Resources.Load<AudioClip>(Managers.Data.GetBgmPath(i));  //bgm clip���� path�� �ʱ�ȭ
    }

    /// <summary>
    /// sfx �ʱ�ȭ
    /// </summary>
    void InitSfx() {
        GameObject sfxObject = new GameObject("SfxPlayer");  //sfx player ����
        sfxObject.transform.parent = transform; //�θ� manager�� ����
        sfxPlayers = new AudioSource[SFX_CHANNELS];  //ä�� �� ��ŭ player ����

        for (int i = 0; i < sfxPlayers.Length; i++) {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();  //�� player�� ����� �ҽ� �߰�
            sfxPlayers[i].playOnAwake = false;  //��� ��� ����
            sfxPlayers[i].volume = DefaultSfxVolume;  //�⺻ ���� ����
        }

        sfxClips = new AudioClip[(int)Define.SfxType.Count];  //Ŭ���� sfx�� �� ��ŭ �ʱ�ȭ

        for (int i = 0; i < sfxClips.Length; i++)
            sfxClips[i] = Resources.Load<AudioClip>(Managers.Data.GetSfxPath(i));  //sfx clip���� path �ʱ�ȭ
    }

    /// <summary>
    /// bgm ���� ����
    /// </summary>
    /// <param name="volume">����</param>
    public void SetBgmVolume(float volume) =>  bgmPlayer.volume = volume;

    /// <summary>
    /// sfx ���� ����
    /// </summary>
    /// <param name="volume">����</param>
    public void SetSfxVolume(float volume) {
        for (int i = 0; i < sfxPlayers.Length; i++)
            sfxPlayers[i].volume = volume;
    }

    /// <summary>
    /// bgm ��� �� ����
    /// </summary>
    /// <param name="islive">��� or ����</param>
    /// <param name="bgm">����� bgm Ÿ��</param>
    public void SetBgm(bool islive, Define.BgmType bgm = Define.BgmType.Main) {
        bgmPlayer.Stop();  //� ��Ȳ�������� ������ bgm ����
        bgmPlayer.clip = bgmClip[(int)bgm];  //Ŭ�� ��ü
        if (islive)
            bgmPlayer.Play();  //���
        else
            bgmPlayer.Stop();  //����
    }

    /// <summary>
    /// sfx ���
    /// </summary>
    /// <param name="sfx">����� sfx Ÿ��</param>
    public void PlaySfx(Define.SfxType sfx) {
        for (int i = 0; i < sfxPlayers.Length; i++) {
            if (sfxPlayers[i].isPlaying)  //�÷��̾��� ��� ������ �÷��̾ ��ġ
                continue;

            sfxPlayers[i].clip = sfxClips[(int)sfx];  //��밡���� �÷��̾ ��ġ�ϸ�, Ŭ�� ���� �� �÷���
            sfxPlayers[i].Play();
            break;
        }
    }
}