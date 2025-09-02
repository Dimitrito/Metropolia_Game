using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Stats : MonoBehaviour
{
    [Header("Level Params")]
    public int maxLevel = 5;
    public float baseLevelCost = 100;
    public float percentageIncrease = 1.5f;

    [Header("Money Params")]
    public int money = 10;

    public Action<bool> LevelInfo;

    private int _current_level = 1;

    private Label _levelLabel;
    private Label _coinsLabel;
    private VisualElement _progressFill;

    private float _currentLevelCost;
    private bool _canLevelUpgrade = false;
    void Start()
    {
        _currentLevelCost = baseLevelCost;

        var root = GetComponent<UIDocument>().rootVisualElement;

        _levelLabel = root.Q<Label>(""); 
        _coinsLabel = root.Q<Label>(className: "coins-label");
        _progressFill = root.Q<VisualElement>(className: "progress-fill");

        UpdateUI();
    }

    public bool IncreaseLevel()
    {
        if (_current_level >= maxLevel)
            return false;

        if (money < _currentLevelCost)
            return false;

        money -= (int)_currentLevelCost;
        
        _current_level++;
        
        _currentLevelCost *= percentageIncrease;
        print(percentageIncrease);
        print(_currentLevelCost);
        UpdateUI();
        UpdateLevelInfo();
        return true;
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
        UpdateLevelInfo();
    }

    public bool WasteMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            UpdateUI();
            UpdateLevelInfo();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        if (_coinsLabel == null || _levelLabel == null || _progressFill == null) return;

        _coinsLabel.text = money.ToString();

        if (_current_level >= maxLevel)
        {
            _levelLabel.text = $"Level {_current_level} (max)";
            _progressFill.style.width = Length.Percent(100f);
        }
        else
        {
            _levelLabel.text = $"Level {_current_level}";
            float progress = Mathf.Clamp01(money / _currentLevelCost);
            _progressFill.style.width = Length.Percent(progress * 100f);
        }
    }

    private void UpdateLevelInfo() 
    {
        if (money >= _currentLevelCost && _current_level < maxLevel)
        {
            _canLevelUpgrade = true;
            LevelInfo?.Invoke(true);
        }
        else if (money < _currentLevelCost) 
        {
            _canLevelUpgrade = false;
            LevelInfo?.Invoke(false);
        }
    }

    public bool GetLevelInfo()
    {
        if (_current_level >= maxLevel)
            return false;
        return _canLevelUpgrade;
    }

    public int GetCurrentLevel()
    {
        return _current_level;
    }

    public int GetCurrentMoney() => money;

    public void LoadFromData(StatsData data)
    {
        money = data.money;
        _current_level = data.level;
        _currentLevelCost = baseLevelCost * Mathf.Pow(percentageIncrease, _current_level - 1);
        UpdateUI();
    }

}