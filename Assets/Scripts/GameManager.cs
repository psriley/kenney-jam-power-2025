using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int Timer;

    public PowerSystem powerSystem;
    public PowerStorage storage;

    public List<IPowerProducer> powerProducers;
    public List<IPowerConsumer> powerConsumers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        storage = new PowerStorage();
        powerProducers = new List<IPowerProducer>();
        powerConsumers = new List<IPowerConsumer>();

        powerSystem = new PowerSystem(storage, powerProducers, powerConsumers);
    }

    // Update is called once per frame
    void Update()
    {
        Timer += 1;
        if (Timer >= 60)
        {
            powerSystem.Tick();
        }
    }
}
