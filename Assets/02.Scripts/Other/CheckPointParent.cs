using CartoonFX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointParent : MonoBehaviour {

    [System.Serializable]
    public class EnemyList :IComparable<int> {
        public EnemyList(int number, GameObject go) {
            Number = number;
            enemy = go;
        }
        public int Number;
        public GameObject enemy;

        public int CompareTo(int other) {
            if (Number == other) return 0;
            else if (Number > other) return 1;
            else return -1;
        }

        public bool Contain(GameObject go) {
            if (enemy == go) return true;
            return false;
        }
    }


    [SerializeField]private List<EnemyList> _enemys = new List<EnemyList>();

    public List<EnemyList> Enemys => _enemys;

    public void AddnSort(EnemyList enemy) {
        _enemys.Add(enemy);
        Sort();
    }

    public void DestroyEnemyList(GameObject go) {
        foreach(var item in _enemys) {
            if(item.Contain(go)) {
                _enemys.Remove(item);
                break;
            }
        }
    }

    private void Start() {
        for(int i = 0; i< transform.childCount; i++) {
            transform.GetChild(i).GetComponent<CheckPoint>().MyNumber = i;
        }
    }

    public void Sort() {
        _enemys.Sort((x,y) =>  x.Number.CompareTo(y.Number));
    }
}