using UnityEngine;

public class CrankSystem : MonoBehaviour, IPowerProducer
{
    public int Produce => 1;
    private UserInputActions _actions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _actions = new UserInputActions();
        _actions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //if (_actions.UI.Click.triggered)
        //{
        //    Debug.Log("Clicked");
        //}
    }
}
