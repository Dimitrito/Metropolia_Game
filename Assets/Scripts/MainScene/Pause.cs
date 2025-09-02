using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Pause : MonoBehaviour
{
    public GameObject gameSaverObject; 
    public UIManager uiManager;

    private VisualElement _root;
    private GameSaver _gameSaver;

    void OnEnable()
    {
        Time.timeScale = 0f;
        _gameSaver = GameSaver.Instance;
        var uiDoc = GetComponent<UIDocument>();
        _root = uiDoc.rootVisualElement;

        _root.Q<Button>("ResumeButton").clicked += ResumeGame;
        _root.Q<Button>("SaveButton").clicked += SaveGame;
        _root.Q<Button>("MenuButton").clicked += QuitGame;
    }

    private void ResumeGame()
    {
        uiManager.ClosePause();
    }

    private void SaveGame()
    {
        _gameSaver.Save();
        ResumeGame();
    }

    private void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}