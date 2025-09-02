using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument _menuUi;
    [SerializeField] private UIDocument _settingUi;
    [SerializeField] private GameObject _settingsPanel;

    private void Awake()
    {
        ActivateMenu();
    }

    private void ActivateSettings() 
    {
        _settingsPanel.SetActive(true);

        var settingsRoot = _settingUi.rootVisualElement;
        var backButton = settingsRoot.Q<Button>("BackButton");

        backButton.clicked += () => ActivateMenu();
    }

    private void ActivateMenu()
    {
        _settingsPanel.SetActive(false);

        var menuRoot = _menuUi.rootVisualElement;

        var playButton = menuRoot.Q<Button>("PlayButton");
        var settingsButton = menuRoot.Q<Button>("SettingsButton");
        var quitButton = menuRoot.Q<Button>("QuitButton");

        playButton.clicked += () => SceneManager.LoadScene("Main");
        settingsButton.clicked += () => ActivateSettings();
        quitButton.clicked += () => Application.Quit();

        Button loadButton = menuRoot.Q<Button>("LoadButton");

        loadButton.clicked += () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnMainSceneLoaded;
        };
    }

    private void OnMainSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            GameSaver.Instance.Load(); // загружаем сохранение
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnMainSceneLoaded;
        }
    }
}