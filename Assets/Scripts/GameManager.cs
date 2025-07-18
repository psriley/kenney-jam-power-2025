using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
    private float Timer;

    private PowerSystem powerSystem;
    private List<IPowerProducer> powerProducers = new List<IPowerProducer>();
    private List<IPowerConsumer> powerConsumers = new List<IPowerConsumer>();

    [SerializeField] private GameObject ClickGeneratorPrefab;
    [SerializeField] private PowerStorage Storage;
    [SerializeField] private float Tick = 1; // How many seconds per tick?

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
}
