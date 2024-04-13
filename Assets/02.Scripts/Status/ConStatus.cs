using UnityEngine;

/// <summary>
/// Ÿ�� Con �ɷ�ġ
/// </summary>
public class ConStatus : MonoBehaviour {
    [SerializeField]private int _level;
    [SerializeField]protected Define.TowerType _towerType;
    public Define.TowerType TowerType => _towerType;

    private float _maxBuildingAmout;  //con ���� �ð�
    private int _killNumber;

    public int KillNumber { get { return _killNumber; } set {_killNumber = value; } }
    public float CurrentBuildingAmout { get; set; } = 0f;
    public float MaxBuildingAmout => _maxBuildingAmout;

    public int Level => _level;
    private void Awake() {
        _maxBuildingAmout = Managers.Data.GetTowerCreateTime((int)_towerType, _level);
    }


}
