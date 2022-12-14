/*****************************************************************************
// File Name :         OptionsMenu.cs
// Author :            Rudy Wolfer
// Creation Date :     November 9th, 2022
//
// Brief Description : A script controlling Options Menu functionality. 
*****************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer _audioMixer;
    private float _curMasterVolume;
    private bool _currentlyMuted;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMP_Dropdown _weatherDropdown;
    private Resolution[] _resolutions;

    /// <summary>
    /// Runs PrepareResolutions
    /// </summary>
    private void Start()
    {
        PrepareResolutions();
    }

    /// <summary>
    /// Gathers Resolultion Information
    /// </summary>
    private void PrepareResolutions()
    {
        _resolutions = Screen.resolutions;
        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == 1920 && _resolutions[i].height == 1080)
            {
                currentResolutionIndex = i;
            }
        }
        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.value = currentResolutionIndex;
        _resolutionDropdown.RefreshShownValue();
        _weatherDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Sets the Music Volume
    /// </summary>
    /// <param name="volume">-80 to 0</param>
    public void SetMusicVolume(float volume)
    {
        _audioMixer.SetFloat("MusicVolume", volume);
    }

    /// <summary>
    /// Sets the Music Volume
    /// </summary>
    /// <param name="volume">-80 to 0</param>
    public void SetSoundVolume(float volume)
    {
        _audioMixer.SetFloat("SoundEffectsVolume", volume);
    }

    /// <summary>
    /// Sets the Music Volume
    /// </summary>
    /// <param name="volume">-80 to 0</param>
    public void SetMasterVolume(float volume)
    {
        _curMasterVolume = volume;

        if (_currentlyMuted)
        {
            _audioMixer.SetFloat("MasterVolume", -80);
        }
        else
        {
            _audioMixer.SetFloat("MasterVolume", _curMasterVolume);
        }
    }

    /// <summary>
    /// Sets the Weather
    /// </summary>
    /// <param name="weatherIndex">0 = All, 1 = Day & Night, 2 = Just Day</param>
    public void SetWeather(int weatherIndex)
    {
        MultiSceneData.s_WeatherOption = weatherIndex;
        _weatherDropdown.RefreshShownValue();
    }

    /// <summary>
    /// Adjusts the Quality Level from Low to High. (Low is actually Medium, and High is actually Ultra)
    /// </summary>
    /// <param name="qualityIndex">0 Thru 2</param>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// Updates Screen Resolution
    /// </summary>
    /// <param name="resolutionIndex">Based on Array Size</param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = _resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    /// <summary>
    /// Mutes Master Volume
    /// </summary>
    /// <param name="isMuted">Y or N</param>
    public void SetMuted(bool isMuted)
    {
        if(isMuted)
        {
            _audioMixer.SetFloat("MasterVolume", -80);
            _currentlyMuted = true;
        }
        else
        {
            _audioMixer.SetFloat("MasterVolume", _curMasterVolume);
            _currentlyMuted = false;
        }
    }

    /// <summary>
    /// Toggles fullscreen.
    /// </summary>
    /// <param name="isFullscreen"></param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// Toggles help boxes.
    /// </summary>
    /// <param name="helpEnabled"></param>
    public void SetHelpToggle(bool helpEnabled)
    {
        if (helpEnabled)
        {
            MultiSceneData.s_HelpEnabled = true;
        }
        else
        {
            MultiSceneData.s_HelpEnabled = false;
        }
    }
}
