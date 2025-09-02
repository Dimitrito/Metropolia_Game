using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Timer : MonoBehaviour
{
    public Stats stats;
    public Enemy enemy;

    [Header ("Params")]
    public int time;
    public int tax;
    public float taxIncrease;

    private int _currentTime;
    private Label _timerLabel;
    private Label _taxLabel;

    void Start()
    {
        _currentTime = time;

        // Получаем UI
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        _timerLabel = root.Q<Label>("timerLabel");
        _taxLabel = root.Q<Label>("taxLabel");

        _taxLabel.text = $"-{tax}";

        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (true)
        {
            int minutes = _currentTime / 60;
            int seconds = _currentTime % 60;
            _timerLabel.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            _currentTime--;

            if (_currentTime < 0)
            {
                TakeTax();
                _currentTime = time; 
            }
        }
    }

    private void TakeTax() 
    {
        if (stats.WasteMoney(tax)) 
        {
            tax = Mathf.CeilToInt(tax * taxIncrease);
            _taxLabel.text = $"-{tax}";
        }
        else
            SceneManager.LoadScene("GameOver");

        if (!enemy.WasteMoney(tax))
            SceneManager.LoadScene("Victory");
    }

    public int GetCurrentTime() => _currentTime;

    public void LoadFromData(TimerData data)
    {
        _currentTime = data.currentTime;
        tax = data.tax;
    }

}
