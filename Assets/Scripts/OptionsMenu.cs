using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] _resolutions;


    private void Awake()
    {
        _resolutions = Screen.resolutions;
        var options = _resolutions.Select(resolution => $"{resolution.width} x {resolution.height}").ToList();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = options.IndexOf($"{Screen.currentResolution.width} x {Screen.currentResolution.height}");
        resolutionDropdown.RefreshShownValue();
    }
    private void Start()
    {
        var result = audioMixer.GetFloat("volume", out var value);
        if (result)
        {
            slider.value = value;
        }
    }

    public void ChangeVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
