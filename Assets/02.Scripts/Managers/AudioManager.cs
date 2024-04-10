using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [Header("--Bgm--")]
    private AudioClip[] bgmClip;
    private const float DefaultBgmVolume = 0.5f;
    private AudioSource bgmPlayer;

    [Header("--Sfx--")]
    int sfxChannels = 10;
    private AudioClip[] sfxClips;
    private const float DefaultSfxVolume = 0.5f;
    AudioSource[] sfxPlayers;

    private void Start() {
        InitBgm();
        InitSfx();
        DontDestroyOnLoad(gameObject);
    }

    void InitBgm() {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = DefaultBgmVolume;
        bgmClip = new AudioClip[(int)Define.BgmType.Count];

        for (int i = 0; i < bgmClip.Length; i++)
            bgmClip[i] = Resources.Load<AudioClip>(Managers.Data.GetBgmPath(i));
    }

    void InitSfx() {
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for (int i = 0; i < sfxPlayers.Length; i++) {
            sfxPlayers[i] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[i].playOnAwake = false;
            sfxPlayers[i].volume = DefaultSfxVolume;
        }
        sfxClips = new AudioClip[(int)Define.SfxType.Count];

        for (int i = 0; i < sfxClips.Length; i++)
            sfxClips[i] = Resources.Load<AudioClip>(Managers.Data.GetSfxPath(i));
    }
    public void SetBgmVolume(float volume) {
        bgmPlayer.volume = volume;
    }
    public void SetSfxVolume(float volume) {
        for (int i = 0; i < sfxPlayers.Length; i++) {
            sfxPlayers[i].volume = volume;
        }
    }
    public void PlayBgm(bool islive, Define.BgmType bgm = Define.BgmType.Main) {
        bgmPlayer.Stop();
        bgmPlayer.clip = bgmClip[(int)bgm];
        if (islive)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    public void PlaySfx(Define.SfxType sfx) {
        for (int i = 0; i < sfxPlayers.Length; i++) {
            if (sfxPlayers[i].isPlaying)
                continue;

            sfxPlayers[i].clip = sfxClips[(int)sfx];
            sfxPlayers[i].Play();
            break;
        }
    }
}