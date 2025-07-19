using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class UISystem : MonoBehaviour
{
    private Button upgradeCrankButton;
    private Button upgradeLightButton;
    private Button createGeneratorButton;
    private Button testButton;

    public UnityEvent UpgradeCrankClicked;
    public UnityEvent UpgradeLightClicked;
    public UnityEvent CreateGeneratorClicked;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        var upgradeCrankButtonRoot = root.Q<TemplateContainer>("upgrade-crank");
        var upgradeCrankButton = upgradeCrankButtonRoot?.Q<Button>("Button");

        var upgradeLightButtonRoot = root.Q<TemplateContainer>("upgrade-light");
        var upgradeLightButton = upgradeLightButtonRoot?.Q<Button>("Button");

        var createGeneratorButtonRoot = root.Q<TemplateContainer>("add-gen");
        var createGeneratorButton = createGeneratorButtonRoot?.Q<Button>("Button");

        if (UpgradeCrankClicked == null)
        {
            UpgradeCrankClicked = new UnityEvent();
        }

        if (UpgradeLightClicked == null)
        {
            UpgradeLightClicked = new UnityEvent();
        }

        if (CreateGeneratorClicked == null)
        {
            CreateGeneratorClicked = new UnityEvent();
        }

        upgradeCrankButton.clicked += OnUpgradeCrankButtonClicked;
        upgradeLightButton.clicked += OnUpgradeLightButtonClicked;
        createGeneratorButton.clicked += OnCreateGeneratorButtonClicked;
    }

    private void OnCreateGeneratorButtonClicked()
    {
        Debug.Log("Generator Button Clicked");
        CreateGeneratorClicked?.Invoke();
    }

    private void OnUpgradeLightButtonClicked()
    {
        Debug.Log("Light Button Clicked");
        UpgradeLightClicked?.Invoke();
    }

    private void OnUpgradeCrankButtonClicked()
    {
        Debug.Log("Crank Button Clicked");
        UpgradeCrankClicked?.Invoke();
    }
}
