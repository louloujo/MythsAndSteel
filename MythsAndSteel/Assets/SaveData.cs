using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveData : MonoBehaviour
{
    [SerializeField] private Slider Unlockslider;
    public int unlockCampaign;
    public int redPlayerVictories;
    public int bluePlayerVictories;

    private void Awake()
    {
        
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("UnlockCampaign"))
        {
            unlockCampaign = PlayerPrefs.GetInt("UnlockCampaign");
        }
        if (PlayerPrefs.HasKey("RedPlayerVictories"))
        {
            redPlayerVictories = PlayerPrefs.GetInt("RedPlayerVictories");
        }
        if (PlayerPrefs.HasKey("BluePlayerVictories"))
        {
            bluePlayerVictories = PlayerPrefs.GetInt("BluePlayerVictories");
        }
    }
}