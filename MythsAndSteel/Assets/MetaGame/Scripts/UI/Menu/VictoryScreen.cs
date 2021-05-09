using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VictoryScreen : MonoBehaviour
{
    [Header("Infos globales")]
    [SerializeField] private GameObject _playTimeM;
    public GameObject playTimeM => _playTimeM;
    [SerializeField] private GameObject _playTimeS;
    public GameObject PlayTimeS => _playTimeS;
    [SerializeField] private GameObject _turnCounter;
    public GameObject TurnCounter => _turnCounter;


    [Space]
    [Header("Info Red Army")]
    [SerializeField] private GameObject _redDeadUnits;
    public GameObject RedDeadUnits => _redDeadUnits;
    [SerializeField] private GameObject _redEventUsed;
    public GameObject RedEventUsed => _redEventUsed;
    [SerializeField] private GameObject _redResourcesUsed;
    public GameObject RedResourcesUsed => _redResourcesUsed;
    [SerializeField] private GameObject _redWin;
    public GameObject RedWin => _redWin;


    [Space]
    [Header("Info Blue Army")]
    [SerializeField] private GameObject _blueDeadUnits;
    public GameObject BlueDeadUnits => _blueDeadUnits;
    [SerializeField] private GameObject _blueResourcesUsed;
    public GameObject BlueResourcesUsed => _blueResourcesUsed;
    [SerializeField] private GameObject _blueEventUsed;
    public GameObject BlueEventUsed => _blueEventUsed;
    [SerializeField] private GameObject _blueWin;
    public GameObject BlueWin => _blueWin;
}
