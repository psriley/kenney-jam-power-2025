using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
    private int Timer;

    public PowerSystem powerSystem;

    private PowerStorage storage = new PowerStorage();
    private List<IPowerProducer> powerProducers = new List<IPowerProducer>();
    private List<IPowerConsumer> powerConsumers = new List<IPowerConsumer>();

    public GameObject ClickGeneratorPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerSystem = new PowerSystem(storage, powerProducers, powerConsumers);
        SetupClickGenerator();
    }

    private void SetupClickGenerator()
    {
        GameObject clickGenerator = Instantiate(ClickGeneratorPrefab);
        clickGenerator.transform.position = Vector3.zero;
        CrankSystem crankSystem = clickGenerator.GetComponent<CrankSystem>();
        crankSystem.PowerStorage = storage;
    }

    // Update is called once per frame
    void Update()
    {
        Timer += 1;
        if (Timer >= 60)
        {
            powerSystem.Tick();
            Timer = 0;
        }
    }
}
