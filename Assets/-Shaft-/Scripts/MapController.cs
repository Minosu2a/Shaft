using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private int _currentMapNumber = 0;
    private int _maxMapNumber = 2;

    [SerializeField] private GameObject[] _maps = null;

    public void MapSwitch()
    {
        _maps[_currentMapNumber].SetActive(false); //Desactivate Old Map

        _currentMapNumber++;
        if(_currentMapNumber == _maxMapNumber)
        {
            _currentMapNumber = 0;
        }

        _maps[_currentMapNumber].SetActive(true); //Activate New Map
    }

    public Transform GetMap()
    {
        return _maps[_currentMapNumber].transform;
    }
}
