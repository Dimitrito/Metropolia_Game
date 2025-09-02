using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private Button _menuButton;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _menuButton = root.Q<Button>("MenuButton");

        if (_menuButton != null)
            _menuButton.clicked += OnMenuClicked;
    }

    private void OnDisable()
    {
        if (_menuButton != null)
            _menuButton.clicked -= OnMenuClicked;
    }

    private void OnMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
