using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private GameObject uiObject; 

    private Slider _volumeSlider;
    private Label _volumeLabel;

    private DropdownField _resolutionDropdown;
    private Label _resolutionLabel;

    private Toggle _fullscreenToggle;
    private Button _backButton;

    private Resolution[] _availableResolutions;

    private void OnEnable()
    {
        if (uiObject == null)
        {
            Debug.LogError("UI Object is not assigned!");
            return;
        }

        var root = uiObject.GetComponent<UIDocument>().rootVisualElement;

        // Находим элементы
        _volumeSlider = root.Q<Slider>("VolumeSlider");
        _volumeLabel = root.Q<Label>("VolumeValueLabel");

        _resolutionDropdown = root.Q<DropdownField>("ResolutionDropdown");
        _resolutionLabel = root.Q<Label>("ResolutionValueLabel");

        _fullscreenToggle = root.Q<Toggle>("FullscreenToggle");

        _backButton = root.Q<Button>("BackButton");
        _backButton.clicked += CloseSettings;

        // Заполняем список разрешений
        _availableResolutions = Screen.resolutions;
        List<string> resStrings = new List<string>();
        int currentIndex = 0;
        for (int i = 0; i < _availableResolutions.Length; i++)
        {
            string res = _availableResolutions[i].width + " x " + _availableResolutions[i].height;
            resStrings.Add(res);

            if (_availableResolutions[i].width == Screen.width &&
                _availableResolutions[i].height == Screen.height)
            {
                currentIndex = i;
            }
        }

        _resolutionDropdown.choices = resStrings;
        _resolutionDropdown.value = resStrings[currentIndex];
        _resolutionLabel.text = _resolutionDropdown.value;

        // Загружаем сохранённые настройки
        _volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", AudioListener.volume);
        _volumeLabel.text = Mathf.RoundToInt(_volumeSlider.value * 100) + "%";

        _fullscreenToggle.value = PlayerPrefs.GetInt("Fullscreen", Screen.fullScreen ? 1 : 0) == 1;

        // Подписка на события
        _volumeSlider.RegisterValueChangedCallback(evt =>
        {
            AudioListener.volume = evt.newValue;
            _volumeLabel.text = Mathf.RoundToInt(evt.newValue * 100) + "%";
            PlayerPrefs.SetFloat("MasterVolume", evt.newValue);
        });

        _fullscreenToggle.RegisterValueChangedCallback(evt =>
        {
            Screen.fullScreen = evt.newValue;
            PlayerPrefs.SetInt("Fullscreen", evt.newValue ? 1 : 0);
        });

        _resolutionDropdown.RegisterValueChangedCallback(evt =>
        {
            int index = _resolutionDropdown.choices.IndexOf(evt.newValue);
            if (index >= 0)
            {
                Resolution res = _availableResolutions[index];
                Screen.SetResolution(res.width, res.height, Screen.fullScreen);
                _resolutionLabel.text = evt.newValue;
            }
        });
    }

    public void OpenSettings()
    {
        uiObject.SetActive(true);
        Time.timeScale = 0f;
    }

    private void CloseSettings()
    {
        uiObject.SetActive(false);
        Time.timeScale = 1f;
        PlayerPrefs.Save();
    }
}
