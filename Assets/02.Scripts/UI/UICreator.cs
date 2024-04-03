using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICreator : UIBase
{
    private RectTransform _rectTransform;
    private ISelectedObject _selectObject;
    private GameObject _centerImage;
    private GameObject[] _towersIcon = new GameObject[(int)Define.TowerType.Count];
    void Start()
    {
        Init();
    }

    private void Init() {
        _centerImage = Util.FindChild(gameObject, "Center", false);
        _towersIcon[(int)Define.TowerType.ArcherTower] = Util.FindChild(_centerImage, "ArcherTower", false);
        _towersIcon[(int)Define.TowerType.CanonTower] = Util.FindChild(_centerImage, "CanonTower", false);
        _towersIcon[(int)Define.TowerType.MagicTower] = Util.FindChild(_centerImage, "MagicTower", false);
        _towersIcon[(int)Define.TowerType.DeathTower] = Util.FindChild(_centerImage, "DeathTower", false);


        for (int i = 0; i < _towersIcon.Length; i++) {
            int index = i;
            _towersIcon[i].GetComponent<Button>().onClick.AddListener(() => CreateTower((Define.TowerType)index, _selectObject.MyTransform.position));
        }

        _rectTransform = _centerImage.GetComponentInChildren<RectTransform>();
        _centerImage.SetActive(false);
    }

    private void CreateTower(Define.TowerType type, Vector3 createPos) {
        int cost = Managers.Data.GetTowerCost((int)type, 1);
        if (!GameSystem.Instance.EnoughGold(cost))
            return;

        string name = type.ToString();
        GameSystem.Instance.SetGold(-cost);
        Managers.Creator.CreateTower(name, createPos);
        _centerImage.SetActive(false);
        _selectObject.OnDeSelect();
        _selectObject = null;
    }



    public void SelectNode(bool trigger, Vector3 pos ,ISelectedObject obj = null) {
        _centerImage.SetActive(trigger);
        _selectObject = obj;

        if (!trigger)
            return;

        Util.RectToWorldPosition(pos, _rectTransform);
        ShowTowerIcon();
    }

    private void ShowTowerIcon() {
        for(int i = 0; i< _towersIcon.Length; i++)
            if (GameSystem.Instance.EnoughGold(Managers.Data.GetTowerCost(i,1)))
                _towersIcon[i].SetActive(true);
            else
                _towersIcon[i].SetActive(false);
    }


}
