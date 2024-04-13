using UnityEngine;

/// <summary>
/// 사운드 관리
/// </summary>
public class AudioManager : MonoBehaviour {

    private AudioClip[] bgmClip;  //게임에 존재하는 모든 bgm 목록
    private const float DefaultBgmVolume = 0.5f;  //bgm기본 볼륨
    private AudioSource bgmPlayer;  //bgm은 한 번에 하나만 출력 가능

    private const int SFX_CHANNELS = 10;  //한번에 최대 출력 가능한 효과음 수
    private AudioClip[] sfxClips;  //게임에 존재하는 모든 sfx 목록
    private const float DefaultSfxVolume = 0.5f;  //sfx기본 볼륨
    AudioSource[] sfxPlayers;  //여러 sfx를 동시에 출력하기 위한 배열

    private void Start() {
        InitBgm();  //bgm 초기화
        InitSfx();  //sfx 초기화
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// bgm 초기화
    /// </summary>
    void InitBgm() {
        GameObject bgmObject = new GameObject("BgmPlayer");  //bgm player 생성
        bgmObject.transform.parent = transform;  //부모를 manager로 지정
        bgmPlayer = bgmObject.AddComponent<AudioSource>();  //오디오 소스 추가
        bgmPlayer.playOnAwake = false;  //즉시 재생 해제
        bgmPlayer.loop = true;  //loop 설정
        bgmPlayer.volume = DefaultBgmVolume;  //기본 볼륨 설정
        bgmClip = new AudioClip[(int)Define.BgmType.Count];  //

        for (int i = 0; i < bgmClip.Length; i++)
            bgmClip[i] = Resources.Load<AudioClip>(Managers.Data.GetBgmPath(i));  //bgm clip들의 path를 초기화
    }

    /// <summary>
    /// sfx 초기화
    /// </summary>
    void InitSfx() {
        GameObject sfxObject = new GameObject("SfxPlayer");  //sfx player 생성
        sfxObject.transform.parent = transform; //부모를 manager로 지정
        sfxPlayers = new AudioSource[SFX_CHANNELS];  //채널 수 만큼 player 생성

        for (int i = 0; i < sfxPlayers.Length; i++) {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();  //각 player에 오디오 소스 추가
            sfxPlayers[i].playOnAwake = false;  //즉시 재생 해제
            sfxPlayers[i].volume = DefaultSfxVolume;  //기본 볼륨 설정
        }

        sfxClips = new AudioClip[(int)Define.SfxType.Count];  //클립을 sfx의 수 만큼 초기화

        for (int i = 0; i < sfxClips.Length; i++)
            sfxClips[i] = Resources.Load<AudioClip>(Managers.Data.GetSfxPath(i));  //sfx clip들의 path 초기화
    }

    /// <summary>
    /// bgm 볼륨 설정
    /// </summary>
    /// <param name="volume">볼륨</param>
    public void SetBgmVolume(float volume) =>  bgmPlayer.volume = volume;

    /// <summary>
    /// sfx 볼륨 설정
    /// </summary>
    /// <param name="volume">볼륨</param>
    public void SetSfxVolume(float volume) {
        for (int i = 0; i < sfxPlayers.Length; i++)
            sfxPlayers[i].volume = volume;
    }

    /// <summary>
    /// bgm 재생 및 정지
    /// </summary>
    /// <param name="islive">재생 or 정지</param>
    /// <param name="bgm">재생할 bgm 타입</param>
    public void SetBgm(bool islive, Define.BgmType bgm = Define.BgmType.Main) {
        bgmPlayer.Stop();  //어떤 상황에서든지 현재의 bgm 정지
        bgmPlayer.clip = bgmClip[(int)bgm];  //클립 교체
        if (islive)
            bgmPlayer.Play();  //재생
        else
            bgmPlayer.Stop();  //정지
    }

    /// <summary>
    /// sfx 재생
    /// </summary>
    /// <param name="sfx">재생할 sfx 타입</param>
    public void PlaySfx(Define.SfxType sfx) {
        for (int i = 0; i < sfxPlayers.Length; i++) {
            if (sfxPlayers[i].isPlaying)  //플레이어중 사용 가능한 플레이어를 서치
                continue;

            sfxPlayers[i].clip = sfxClips[(int)sfx];  //사용가능한 플레이어를 서치하면, 클립 설정 후 플레이
            sfxPlayers[i].Play();
            break;
        }
    }
}