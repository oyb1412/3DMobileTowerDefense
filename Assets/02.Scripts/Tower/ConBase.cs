using System;
using UnityEngine;

/// <summary>
/// Con �⺻ ���� ����
/// </summary>
public class ConBase : MonoBehaviour
{
    private const float CREATE_SOUND_DELAY = 1f;

    private ConStatus _status;  //Con�ɷ�ġ
    public Action<float> OnCreatEvent;  //Ÿ�� ���� �� �̺�Ʈ

    private string _createTowerPath;  //���� �Ϸ�� ������ Ÿ���� path
    private float _soundAmout;

    public ConStatus Status { get { return _status; } }
    public int ConHandle { get; set; }  //Con�� �ο��Ǵ� ���� �ڵ�

    /// <summary>
    /// �ʱ�ȭ
    /// </summary>
    /// <param name="pos">��ġ</param>
    /// <param name="killNumber">���� Ÿ���� kill�� ���</param>
    public void Init(Vector3 pos, int killNumber = 0) {
        transform.position = pos;
        _status = GetComponent<ConStatus>();
        _status.CurrentBuildingAmout = 0;
        _status.KillNumber = killNumber;
        string name = gameObject.name;
        _createTowerPath = name.Substring(0, name.Length - 4);
    }

    private void Update() {
        if (!GameSystem.Instance.IsPlay())  //���� ���� �� ����
            return;

        _status.CurrentBuildingAmout += Time.deltaTime;
        _soundAmout += Time.deltaTime;
        if(_soundAmout >= CREATE_SOUND_DELAY) {  //create ���� ȣ��
            Managers.Audio.PlaySfx(Define.SfxType.Build);
            _soundAmout = 0;
        }
            
        if (_status.CurrentBuildingAmout >= _status.MaxBuildingAmout)
            CreateTower();

        OnCreatEvent?.Invoke(_status.CurrentBuildingAmout / _status.MaxBuildingAmout);
    }

    /// <summary>
    /// Ÿ�� ����
    /// </summary>
    private void CreateTower() {
        TowerStatus tower = Managers.Resources.Instantiate($"Towers/{_status.TowerType.ToString()}/{_createTowerPath}", null).GetComponent<TowerStatus>();
        tower.Init(_status.KillNumber, _status.Level, transform.position, _status.TowerType);
        var towerbase = tower.GetComponent<TowerBase>();
        towerbase.Init();
        towerbase.TowerHandle = GameSystem.Instance.AddTowerObject(towerbase, tower.TowerType, tower.Level);
        Managers.Audio.PlaySfx(Define.SfxType.BuildCompleted);
        GameSystem.Instance.RemoveConObject(ConHandle);
        Managers.Resources.Destroy(gameObject);
    }
}
