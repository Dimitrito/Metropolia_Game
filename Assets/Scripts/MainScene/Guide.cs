using UnityEngine;
using UnityEngine.UIElements; // Äëÿ UI Toolkit

public class GuideController : MonoBehaviour
{
    private Button _understoodButton;
    private VisualElement _root;
    private Label _textPlace;
    private string _guideText =
@"Congratulations! You have become one of two investors in a new park. The main thing is to remember to pay your taxes on time.

Your goal is simple: make your competitor go bankrupt before you. Every two minutes you both have to pay a tax, and the one who fails to do so first loses.

To build new buildings, press E - the construction menu will open. Not all buildings are available immediately, new ones will become available after purchasing the next level of the park.

To improve existing buildings, click on them. You can increase income, the frequency of its receipt, and protection from attacks from a competitor.

You can also spend money to harm your opponent. Click on his buildings and choose an attack method: either worsen one of the parameters (cannot be lower than lvl 1), or completely demolish the building.

Strategically develop the park, combine growth and attacks on the competitor, and become the most successful investor!";

    void OnEnable()
    {
        Time.timeScale = 0f;
        var uiDocument = GetComponent<UIDocument>();
        _root = uiDocument.rootVisualElement;

        _understoodButton = _root.Q<Button>("guide-button");
        _textPlace = _root.Q<Label>("textPlace");
        
        if (_textPlace != null)
        {
            _textPlace.text = _guideText;
        }

        if (_understoodButton != null)
        {
            _understoodButton.clicked += OnUnderstoodClicked;
        }
    }

    void OnDisable()
    {
        if (_understoodButton != null)
        {
            _understoodButton.clicked -= OnUnderstoodClicked;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
        }
    }

    private void OnUnderstoodClicked()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
