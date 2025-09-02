using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Building : MonoBehaviour
{
    private Stats stats;

    [Header ("Params")]
    [SerializeField] private int income;
    [SerializeField] private float frequency;
    [SerializeField] private float protection;

    [Header("Technical")]
    [SerializeField] private int price;
    [SerializeField] private float priceIncrease;
    [SerializeField] private int maxLevel;

    [Header("Improve")]
    [SerializeField] private int improveIncome;
    [SerializeField] private float improveFrequency;
    [SerializeField] private float improveProtection;

    private int _priceIncome;
    private int _priceFrequency;
    private int _priceProtection;

    private int _incomeLevel = 1;
    private int _frequencyLevel = 1;
    private int _protectionLevel = 1;

    private GameObject _uiObject;

    private Label _incomeText;
    private Label _frequencyText;
    private Label _protectionText;

    private Button _incomeBtn;
    private Button _frequencyBtn;
    private Button _protectionBtn;

    private bool _isUiOpen = false;

    private void Awake()
    {
        _priceIncome = price;
        _priceFrequency = price;
        _priceProtection = price;

        GameObject StatsObject = GameObject.Find("Stats");
        stats = StatsObject.GetComponent<Stats>();

        //uiObject = GameObject.Find("BuildingUpgrade");

        StartCoroutine(AddIncome());
    }
    private void Update()
    {
        if (_isUiOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseBtn();
        }
    }
    private void OnMouseDown()
    {
        if (_isUiOpen)
            return;

        _isUiOpen = true;

        Time.timeScale = 0f;
        _uiObject.SetActive(true);

        UIDocument buildingUIDocument = _uiObject.GetComponent<UIDocument>();
        var root = buildingUIDocument.rootVisualElement;

        _incomeBtn = root.Q<Button>("incomeBtn");
        _frequencyBtn = root.Q<Button>("frequencyBtn");
        _protectionBtn = root.Q<Button>("protectionBtn");

        UpdatePrice();

        _incomeBtn.clicked += ImproveIncome;
        _frequencyBtn.clicked += ImproveFrequency;
        _protectionBtn.clicked += ImproveProtection;

        Button closeBtn = root.Q<Button>("closeBtn");
        closeBtn.clicked += CloseBtn;

        Button deleteBtn = root.Q<Button>("deleteBtn");
        deleteBtn.clicked += DeleteBtn;

        _incomeText = root.Q<Label>("incomeText");
        _frequencyText = root.Q<Label>("frequencyText");
        _protectionText = root.Q<Label>("protectionText");

        UpdateText();
    }

    private void UpdateText() 
    {
        if (_incomeLevel >= maxLevel)
            _incomeText.text = income.ToString();
        else
            _incomeText.text = $"{income} -> {income + improveIncome}";

        if (_frequencyLevel >= maxLevel)
            _frequencyText.text = frequency.ToString();
        else
            _frequencyText.text = $"{frequency} -> {frequency - improveFrequency}";

        if (_protectionLevel >= maxLevel)
            _protectionText.text = protection.ToString();
        else
            _protectionText.text = $"{protection} -> {protection + improveProtection}";
    }
    private void UpdatePrice()
    {
        if (_incomeLevel >= maxLevel)
            _incomeBtn.text = $"max";
        else
            _incomeBtn.text = $"{_priceIncome} coin";
        
        if (_frequencyLevel >= maxLevel)
            _frequencyBtn.text = $"max";
        else
            _frequencyBtn.text = $"{_priceFrequency} coin";

        if (_protectionLevel >= maxLevel)
            _protectionBtn.text = $"max";
        else
            _protectionBtn.text = $"{_priceProtection} coin";
    }

    private void CloseBtn() 
    {
        _uiObject.SetActive(false);
        Time.timeScale = 1f;
        _isUiOpen = false;
    }
    private void DeleteBtn()
    {
        GameObject uiManagerObject = GameObject.Find("UI");
        UIManager uiManager = uiManagerObject.GetComponent<UIManager>();
        uiManager.RemoveBuild();

        GetComponent<GridLocator>().DeleteObject();
        CloseBtn();
    }
    private void ImproveIncome() 
    {
        if (stats.WasteMoney(_priceIncome) && _incomeLevel < maxLevel) 
        {
            income += improveIncome;
            _priceIncome = (int)(_priceIncome * priceIncrease);
            _incomeLevel++;
            UpdatePrice();
            UpdateText();
        }
    }

    private void ImproveFrequency()
    {
        if (stats.WasteMoney(_priceFrequency) && _frequencyLevel < maxLevel)
        { 
            frequency -= improveFrequency;
            _priceFrequency = (int)(_priceFrequency * priceIncrease);
            _frequencyLevel++;
            UpdatePrice();
            UpdateText();
        }
    }

    private void ImproveProtection()
    {
        if (stats.WasteMoney(_priceProtection) && _protectionLevel < maxLevel)
        { 
            protection += improveProtection;
            _priceProtection = (int)(_priceProtection * priceIncrease);
            _protectionLevel++;
            UpdatePrice();
            UpdateText();
        }
    }

    private IEnumerator AddIncome()
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            stats.AddMoney(income);
        }
    }

    public void SetUIObject(GameObject ui) 
    {
        _uiObject = ui;
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
        frequency = frequency - improveFrequency * (_frequencyLevel - 1);
        protection = protection + improveProtection * (_protectionLevel - 1);
    }

}
