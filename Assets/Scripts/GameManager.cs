using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private float Timer;

    private ClickHandler clickHandler;
    private PowerSystem powerSystem;
    private BuildSystem buildSystem;

    private List<IPowerProducer> powerProducers = new List<IPowerProducer>();
    private List<IPowerConsumer> powerConsumers = new List<IPowerConsumer>();

    [SerializeField] private GameObject light;

    [SerializeField] private GameObject ClickGeneratorPrefab;
    [SerializeField] private PowerStorage Storage;
    [SerializeField] private float Tick = 1; // How many seconds per tick?

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickHandler = gameObject.AddComponent<ClickHandler>();
        buildSystem = gameObject.AddComponent<BuildSystem>();
        clickHandler.buildEvent.AddListener(BuildSomething);
        powerSystem = new PowerSystem(Storage, powerProducers, powerConsumers);
        SetupClickGenerator();
    }

    private void SetupClickGenerator()
    {
        GameObject clickGenerator = Instantiate(ClickGeneratorPrefab);
        clickGenerator.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= Tick)
        {
            powerSystem.Tick();
            Timer = 0;
        }
    }

    private void OnApplicationQuit()
    {
        Storage.ResetPower();
    }

    void BuildSomething(GridCell cell)
    {
        Debug.Log("Building something " + cell);
        buildSystem.Build(light, cell);
    }
}
