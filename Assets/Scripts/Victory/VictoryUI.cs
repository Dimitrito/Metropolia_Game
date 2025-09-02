using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class VictoryUI : MonoBehaviour
{
    private Button menuButton;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        menuButton = root.Q<Button>("MenuButton");

        if (menuButton != null)
            menuButton.clicked += OnMenuClicked;
    }

    private void OnDisable()
    {
        if (menuButton != null)
            menuButton.clicked -= OnMenuClicked;
    }

    private void OnMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
