using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using System.Linq;
using System;
using UnityEngine.Events;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private float Timer;
    private float GameOverTimer;


    public InventoryObject inventory;
    private ClickHandler clickHandler;
    private PowerSystem powerSystem;
    private BuildSystem buildSystem;
    private CursorChangerSystem cursorChangerSystem;
    private UpgradeSystem upgradeSystem;
    private GeneratorSystem generator;
    private CrankSystem crankSystem;

    private List<IPowerProducer> powerProducers = new List<IPowerProducer>();
    private List<IPowerConsumer> powerConsumers = new List<IPowerConsumer>();

    private List<Light> lights = new List<Light>();

    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private GameObject ClickGeneratorPrefab;
    [SerializeField] private PowerStorage Storage;
    [SerializeField] private CostObject generatorCostObject;
    [SerializeField] private float Tick = 1; // How many seconds per tick?
    [SerializeField] private UIScriptableObject UIScriptableObject;

    [SerializeField] private Texture2D PickaxeCursor;
    [SerializeField] private Texture2D NormalCursor;
    [SerializeField] private Texture2D InteractCursor;
    [SerializeField] private GridGenerator gridGameObject;

    public UnityEvent TriggerLowPowerUI;
    public UnityEvent TriggerGameOver;
    private bool lowPower = false;


    void Awake()
    {
        buildSystem = gameObject.AddComponent<BuildSystem>();
        powerSystem = new PowerSystem(Storage, powerProducers, powerConsumers);
        gridGameObject.gridCreated.AddListener(SetupInitialGridObjects);
        generator = gameObject.AddComponent<GeneratorSystem>();
        Storage.Power = 1;
        Storage.LowPower.AddListener(LowPower);

        SetupGenerator();
        SetupCursorChangingSystem();
        SetupClickHandler();
        SetupUpgradeSystem();
    }

    private void LowPower()
    {
        TriggerLowPowerUI?.Invoke();
        lowPower = true;
    }

    private void SetupGenerator()
    {
        generator = gameObject.AddComponent<GeneratorSystem>();
        generator.costObject = generatorCostObject;
        powerProducers.Add(generator);
    }

    private void SetupUpgradeSystem()
    {
        upgradeSystem = gameObject.AddComponent<UpgradeSystem>();
        upgradeSystem.inventoryObject = inventory;
        upgradeSystem.powerStorage = Storage;
    }

    public void AddGenerator()
    {
        Debug.Log("Upgraing Generator");
        upgradeSystem.UpgradeItem(generator);
        UIScriptableObject.NumGen = generator.numGenerators;
    }

    public void UpgradeLights()
    {
        Debug.Log("Upgrading Lights");
        foreach (Light light in lights)
        {
            if (light != null)
            {
                upgradeSystem.UpgradeItem(light);
                RemoveFogPanels(light);
            }
        }
    }

    public void UpgradeCrank()
    {
        Debug.Log("Upgrading Crank!");
        upgradeSystem.UpgradeItem(crankSystem);
    }

    private void SetupCursorChangingSystem()
    {
        cursorChangerSystem = gameObject.AddComponent<CursorChangerSystem>();
        cursorChangerSystem.PickaxeCursor = PickaxeCursor;
        cursorChangerSystem.InteractCursor = InteractCursor;
        cursorChangerSystem.NormalCursor = NormalCursor;
    }

    private void SetupClickHandler()
    {
        clickHandler = gameObject.AddComponent<ClickHandler>();
        clickHandler.buildEvent.AddListener(BuildSomething);
    }

    private void SetupInitialGridObjects()
    {
        Debug.Log("Setup called");
        SetupClickGenerator();
        SetupInitialLights();
    }

    private void SetupClickGenerator()
    {
        GameObject clickGenerator = Instantiate(ClickGeneratorPrefab);
        crankSystem = clickGenerator.GetComponent<CrankSystem>();
        clickGenerator.transform.position = gridGameObject.transform.GetChild(1).position;
        GridCell initCell = gridGameObject.transform.GetChild(1).GetComponent<GridCell>();

        initCell.SetOccupant(clickGenerator);
        Destroy(gridGameObject.gridFogTiles[initCell]);
    }

    private void SetupInitialLights()
    {
        GameObject initialLight = Instantiate(lightPrefab);
        initialLight.transform.position = gridGameObject.transform.GetChild(2).position;
        GridCell initCell = gridGameObject.transform.GetChild(2).GetComponent<GridCell>();

        initCell.SetOccupant(initialLight);
        Light lightScript = initialLight.GetComponent<Light>();
        lights.Add(lightScript);
        RemoveFogPanels(initCell);
        UIScriptableObject.NumLights = 1;
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

        if (lowPower && Storage.Power <= 0)
        {
            GameOverTimer += Time.deltaTime;
            if (GameOverTimer > 5)
            {
                Debug.Log("GAME OVER!");
                TriggerGameOver?.Invoke();
            }
        }
    }

    private void OnApplicationQuit()
    {
        Storage.ResetPower();
        UIScriptableObject.Reset();
    }

    void BuildSomething(GridCell cell)
    {
        if (Storage.Power >= 5)
        {
            GameObject newObject = buildSystem.Build(lightPrefab, cell);
            cell.SetOccupant(newObject);

            RemoveFogPanels(cell);

            Storage.Drain(5);
            IPowerConsumer consumer = newObject.GetComponent<IPowerConsumer>();
            powerConsumers.Add(consumer);
            lights.Add(newObject.GetComponent<Light>());
            UIScriptableObject.NumLights = lights.Count();
        }
        else
        {
            Debug.Log("Don't have enough power");
        }
    }

    private void RemoveFogPanels(Light light)
    {
        Collider[] hitColliders = Physics.OverlapSphere(light.transform.position, light.lightRadius);
        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Fog"))
            {
                Destroy(hit.gameObject);
            }
        }
    }

    private void RemoveFogPanels(GridCell cell)
    {
        GridGenerator gg = cell.GetComponentInParent<GridGenerator>();
        Light lightOccupant = cell.GetOccupant().GetComponent<Light>();

        Collider[] hitColliders = Physics.OverlapSphere(cell.transform.position, lightOccupant.lightRadius);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Fog"))
            {
                Destroy(hit.gameObject);
            }
        }

        // if (gg.gridFogTiles.TryGetValue(cell, out GameObject fogTile))
        // {
        //     Destroy(fogTile);

        //     List<Vector3> neighborPositions = new List<Vector3>
        //     {
        //         cell.transform.position + new Vector3(1, 0, 0),
        //         cell.transform.position + new Vector3(-1, 0, 0),
        //         cell.transform.position + new Vector3(0, 0, 1),
        //         cell.transform.position + new Vector3(0, 0, -1)
        //     };

        //     foreach (Vector3 neighborPos in neighborPositions)
        //     {
        //         Debug.Log(neighborPos);
        //         if (gg.gridCellPositions.TryGetValue(neighborPos, out GridCell neighborCell))
        //         {
        //             if (gg.gridFogTiles.TryGetValue(neighborCell, out GameObject nfogTile))
        //             {
        //                 Destroy(nfogTile);
        //             }
        //         }
        //     }

        // }
    }
}
