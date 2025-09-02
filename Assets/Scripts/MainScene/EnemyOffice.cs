using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyOffice : MonoBehaviour
{
    private Stats stats;
    private Enemy enemy;

    [Header("Params")]
    [SerializeField] private int income;
    [SerializeField] private float frequency;
    [SerializeField] private int protection;

    [Header("Technical")]
    [SerializeField] private int price;
    [SerializeField] private int harmPrice;
    [SerializeField] private float priceIncrease;
    [SerializeField] private int maxLevel;

    [Header("Improve")]
    [SerializeField] private int improveIncome;
    [SerializeField] private float improveFrequency;
    [SerializeField] private int improveProtection;

    private int _priceIncome;
    private int _priceFrequency;
    private int _priceProtection;

    private int _incomeLevel = 1;
    private int _frequencyLevel = 1;
    private int _protectionLevel = 1;

    [Header("UI")]
    private GameObject _uiObject;

    private Label _incomeLabel;
    private Label _frequencyLabel;
    private Label _protectionLabel;
    private Label _harmPriceLabel;
    private Label _demolishPriceLabel;

    private Button _harmButton;
    private Button _demolishButton;
    private Button _closeButton;

    private bool _isUiOpen = false;
    void Awake()
    {
        _priceIncome = price;
        _priceFrequency = price;
        _priceProtection = price;

        StartCoroutine(AddIncome());
    }

    private void Update()
    {
        if (_isUiOpen && Input.GetKeyDown(KeyCode.Escape)) 
        {
            CloseHarmUI();
        }
    }
    private IEnumerator AddIncome()
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            enemy.AddMoney(income);
        }
    }

    public void ImproveIncome() 
    {
        _incomeLevel++;
        income += improveIncome;
    }

    public void ImproveFrequency()
    {
        _frequencyLevel++;
        frequency -= improveFrequency;
    }

    public void ImproveProtection()
    {
        _protectionLevel++;
        protection += improveProtection;
    }

    public void IncreasePrice(int index)
    {
        switch (index)
        {
            case 0:
                _priceIncome = Mathf.CeilToInt(_priceIncome * priceIncrease);
                break;
            case 1:
                _priceFrequency = Mathf.CeilToInt(_priceFrequency * priceIncrease);
                break;
            case 2:
                _priceProtection = Mathf.CeilToInt(_priceProtection * priceIncrease);
                break;
        }
    }
    public int[] GetParamsLevel() 
    {
        return new int[] { _incomeLevel, _frequencyLevel, _protectionLevel };
    }

    public int[] GetParamsPrice()
    {
        return new int[] { _priceIncome, _priceFrequency, _priceProtection };
    }

    public void SetEnemy(Enemy enemy) 
    {
        this.enemy = enemy;
    }

    public void Damage()
    {
        List<int> upgradable = new List<int>();
        if (_incomeLevel > 1) upgradable.Add(0);
        if (_frequencyLevel > 1) upgradable.Add(1);
        if (_protectionLevel > 1) upgradable.Add(2);

        if (upgradable.Count == 0)
            return;

        int choice = upgradable[Random.Range(0, upgradable.Count)];

        switch (choice)
        {
            case 0:
                _incomeLevel--;
                income -= improveIncome;
                _priceIncome = Mathf.FloorToInt(_priceIncome / priceIncrease);
                break;
            case 1:
                _frequencyLevel--;
                frequency -= improveFrequency;
                _priceFrequency = Mathf.FloorToInt(_priceFrequency / priceIncrease);
                break;
            case 2:
                _protectionLevel--;
                protection -= improveProtection;
                _priceProtection = Mathf.FloorToInt(_priceProtection / priceIncrease);
                break;
        }
    }


    public void Demolish() 
    {
        GameObject uiManagerObject = GameObject.Find("UI");
        UIManager uiManager = uiManagerObject.GetComponent<UIManager>();
        uiManager.RemoveBuild();

        GetComponent<GridLocator>().DeleteObject();
        enemy.RemoveOffice(this);
    }

    public int GetPriceDamage()
    {
        return harmPrice * protection * 5;
    }
    public int GetPriceDemolish()
    {
        return harmPrice * protection * 10;
    }
    private void OnMouseDown()
    {
        if (!_isUiOpen)
            OpenHarmUI();
    }
    private void OpenHarmUI()
    {
        _isUiOpen = true;
        Time.timeScale = 0f;
        _uiObject.SetActive(true);

        UIDocument doc = _uiObject.GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        _incomeLabel = root.Q<Label>("incomeValue");
        _frequencyLabel = root.Q<Label>("frequencyValue");
        _protectionLabel = root.Q<Label>("protectionValue");

        _harmPriceLabel = root.Q<Label>("harmPriceLabel");
        _demolishPriceLabel = root.Q<Label>("demolishPriceLabel");

        _harmButton = root.Q<Button>("harmButton");
        _demolishButton = root.Q<Button>("demolishButton");
        _closeButton = root.Q<Button>("closeHarmButton");

        UpdateHarmUI();

        _harmButton.clicked += () =>
        {
            if (_incomeLevel + _frequencyLevel + _protectionLevel > 3) 
            {
                if (stats.WasteMoney(GetPriceDamage()))
                {
                    Damage();
                    UpdateHarmUI();
                }
            }
        };

        _demolishButton.clicked += () =>
        {
            if (stats.WasteMoney(GetPriceDemolish()))
            {
                Demolish();
                CloseHarmUI();
            }
        };

        _closeButton.clicked += CloseHarmUI;
    }

    private void UpdateHarmUI()
    {
        _incomeLabel.text = $"{_incomeLevel} level"; 
        _frequencyLabel.text = $"{_frequencyLevel} level";
        _protectionLabel.text = $"{_protectionLevel} level";

        _harmPriceLabel.text = GetPriceDamage().ToString();
        _demolishPriceLabel.text = GetPriceDemolish().ToString();
    }

    private void CloseHarmUI()
    {
        _uiObject.SetActive(false);
        Time.timeScale = 1f;
        _isUiOpen = false;
    }

    public void SetUiObject(GameObject uiObject) 
    {
        this._uiObject = uiObject;
    }
    public void SetStats(Stats stats)
    {
        this.stats = stats;
    }

    public int GetIncomeLevel() => _incomeLevel;
    public int GetFrequencyLevel() => _frequencyLevel;
    public int GetProtectionLevel() => _protectionLevel;

    public void SetLevels(int incomeLvl, int freqLvl, int protLvl)
    {
        _incomeLevel = incomeLvl;
        _frequencyLevel = freqLvl;
        _protectionLevel = protLvl;

        income = income + improveIncome * (_incomeLevel - 1);
        frequency = frequency + improveFrequency * (_frequencyLevel - 1);
        protection = protection + improveProtection * (_protectionLevel - 1);
    }

}