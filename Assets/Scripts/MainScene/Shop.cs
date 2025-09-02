using System;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    public Texture2D[] images;
    public GameObject building;
    public Stats stats;
    public UIManager uiManager;
    public GameObject[] shops;
    public int[] prices;

    private Button[] _buyButtons;
    private Button _levelUpButton;
    private bool _canIncrease;

    void OnEnable()
    {
        print("Activate");
        var root = GetComponent<UIDocument>().rootVisualElement;
        _levelUpButton = root.Q<Button>(className: "level-up-button");

        _buyButtons = new Button[5];
        for (int i = 0; i < 5; i++)
        {
            _buyButtons[i] = root.Q<Button>(name: $"Button{i}");
        }

        string[] imageNames = { "BarImage", "CandyImage", "PizzaImage", "MarketImage", "CasinoImage" };
        for (int i = 0; i < imageNames.Length; i++)
        {
            if (i >= images.Length) break; // безопасная проверка

            var visualElement = root.Q<VisualElement>(imageNames[i]);
            if (visualElement != null)
            {
                Image img = visualElement.Q<Image>();
                if (img == null)
                {
                    img = new Image();
                    visualElement.Add(img);
                }

                img.image = images[i]; // назначаем Texture2D
                img.scaleMode = ScaleMode.ScaleToFit; // можно задать Fit или Stretch
            }
        }


        LevelUpHandler(stats.GetLevelInfo());
        stats.LevelInfo += LevelUpHandler;
    }

    private void ButtonHandler(int index) 
    {
        if (stats.WasteMoney(prices[index]) && uiManager.AddBuild()) 
        {
            GameObject newShop = Instantiate(shops[index], new Vector3(-2.5f, 0f, 0.5f), Quaternion.identity);
            newShop.GetComponent<Building>().SetUIObject(building);
            GameSaver.Instance.playerBuildings.Add(newShop.GetComponent<Building>());
            uiManager.CloseShop();
        }
    }

    private void UpdateButtons()
    {
        int currentLevel = stats.GetCurrentLevel() - 1;
        print("currentLevel" + currentLevel);
        for (int i = 0; i < _buyButtons.Length; i++)
        {
            int index = i;
            _buyButtons[index].clicked -= () => ButtonHandler(index);

            if (i <= currentLevel)
            {
                _buyButtons[index].RemoveFromClassList("disabled");
                _buyButtons[index].text = $"Buy building";
                _buyButtons[index].clicked += () => ButtonHandler(index);
            }
            else
            {
                _buyButtons[index].AddToClassList("disabled");
                _buyButtons[index].text = $"Requires Level {index + 1}";
                _buyButtons[index].clicked -= () => ButtonHandler(index);
            }
        }
    }

    private void OnLevelUpClicked()
    {
        stats.IncreaseLevel();
    }

    private void LevelUpHandler(bool status)
    {
        if (!isActiveAndEnabled)
            return;
        print("called");

        _levelUpButton.clicked -= OnLevelUpClicked;
        if (!status) 
        {
            //canIncrease = false;
            _levelUpButton.clicked -= OnLevelUpClicked;
            _levelUpButton.AddToClassList("disabled");
            UpdateButtons();
        }
        else
        {
            //canIncrease = true;
            _levelUpButton.clicked += OnLevelUpClicked;
            _levelUpButton.RemoveFromClassList("disabled");
            UpdateButtons();
        }
    }
}
