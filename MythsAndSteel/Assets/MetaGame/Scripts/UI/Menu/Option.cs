using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    //---------Audio--------------
    public AudioMixer audioMixer;

    //--------Résolution----------
    public Dropdown ResolutionDropdown;
    Resolution[] resolutions;

    //-------Avertissement--------
    bool isAvertissement;

    #region Résolution
    private void Start()
    {
        resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i=0; i<resolutions.Length; i++)
        {
           string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    #endregion

    #region Audio
    public void ActiveVolume(bool isVolumeOn)
    {
        if (isVolumeOn)
        {
            audioMixer.SetFloat("Master", 0);
        }
        else
        {
            audioMixer.SetFloat("Master", -80);
        }
    }
    public void SetEffectvolume (float volume)
    {
        audioMixer.SetFloat("Effect", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", volume);
    }
    #endregion

    #region Avertissement
    public void SetAvertissement(bool IsAverti)
    {
        isAvertissement = IsAverti;
    }
    #endregion

}
