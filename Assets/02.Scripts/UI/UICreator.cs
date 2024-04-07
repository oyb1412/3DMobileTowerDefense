using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreator : UIBase
{
    private Data _data;
    public UI_EnterInfo _creatorInfoPanel;
    private RectTransform _rectTransform;
    private ISelectedObject _selectObject;
    private GameObject _centerImage;
    private GameObject[] _towersIcon = new GameObject[(int)Define.TowerType.Count];
    void Start()
    {
        Init();
    }

    private void Init() {
        _data = Managers.Data;
        _centerImage = Util.FindChild(gameObject, "Center", false);
        _towersIcon[(int)Define.TowerType.ArcherTower] = Util.FindChild(_centerImage, "ArcherTower", false);
        _towersIcon[(int)Define.TowerType.CanonTower] = Util.FindChild(_centerImage, "CanonTower", false);
        _towersIcon[(int)Define.TowerType.MagicTower] = Util.FindChild(_centerImage, "MagicTower", false);
        _towersIcon[(int)Define.TowerType.DeathTower] = Util.FindChild(_centerImage, "DeathTower", false);


        for (int i = 0; i < _towersIcon.Length; i++) {
            int index = i;
            _towersIcon[i].GetComponent<Button>().onClick.AddListener(() => SelecteCreator((Define.TowerType)index));
        }

        _rectTransform = _centerImage.GetComponentInChildren<RectTransform>();
        _centerImage.SetActive(false);
    }

    private void SelecteCreator(Define.TowerType type) {
        _creatorInfoPanel.gameObject.SetActive(true);
        _creatorInfoPanel.SetPosition(_rectTransform);

        string name = $"{type.ToString()} Lvl{1}";
        string info = _data.GetTowerInfo((int)type);
        int damage = _data.GetTowerAttackDamage((int)type, 1);
        float delay = _data.GetTowerAttacnDelay((int)type, 1);
        float range = _data.GetTowerAttacmRange((int)type, 1);
        int cost = _data.GetTowerCost((int)type, 1);
        foreach (var item in _towersIcon) {
            Util.SetOutLine(item, false);
        }
        Util.SetOutLine(_towersIcon[(int)type], true);

        _creatorInfoPanel.SetEnterInfoUI(name, info, damage, delay, range, cost);
        _creatorInfoPanel.SetBtn(() => CreateTower(type, _selectObject.MyTransform.position, cost, name), cost);
    }

    private void CreateTower(Define.TowerType type, Vector3 createPos, int cost, string name) {
        if (!GameSystem.Instance.EnoughGold(cost))
            return;

        GameSystem.Instance.SetGold(-cost);
        Managers.Creator.CreateTower(name, createPos);
        Util.SetOutLine(_towersIcon[(int)type], false);
        _centerImage.SetActive(false);
        _creatorInfoPanel.gameObject.SetActive(false);
        _selectObject.OnDeSelect();
        _selectObject = null;
    }



    public void SelectNode(bool trigger, Vector3 pos, ISelectedObject obj = null) {
        _creatorInfoPanel.gameObject.SetActive(false);
        foreach (var item in _towersIcon) {
            Util.SetOutLine(item, false);
        }
        _centerImage.SetActive(trigger);
        _selectObject = obj;

        if (!trigger)
            return;

        Util.RectToWorldPosition(pos, _rectTransform);
    }
}
