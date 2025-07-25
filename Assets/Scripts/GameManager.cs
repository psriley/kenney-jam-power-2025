using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    [SerializeField] private CostObject lightCostObject;
    [SerializeField] private CostObject crankCostObject;
    [SerializeField] private float Tick = 1; // How many seconds per tick?
    [SerializeField] private UIScriptableObject UIScriptableObject;
    [SerializeField] private ItemObject metalObejct;

    [SerializeField] private Texture2D PickaxeCursor;
    [SerializeField] private Texture2D NormalCursor;
    [SerializeField] private Texture2D InteractCursor;
    [SerializeField] private GridGenerator gridGameObject;
    [SerializeField] private AudioSystem audioSystem;

    public UnityEvent TriggerLowPowerUI;
    public UnityEvent TriggerGameOver;
    public UnityEvent ResetErrorUI;
    private bool lowPower = false;
    private bool GameOver = false;

    public UnityEvent StartAlarmSoundEvent;
    public UnityEvent StopAlarmSoundEvent;
    public UnityEvent PlaceSoundEvent;

    void Awake()
    {
        SetupInventory();

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

    private void SetupInventory()
    {
        inventory.Clear();
        inventory.AddItem(metalObejct.CreateItem(), 0);
    }

    private void LowPower()
    {
        TriggerLowPowerUI?.Invoke();
        StartAlarmSoundEvent?.Invoke();
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
        if (!upgradeSystem.HasFunds(generator)) return;

        upgradeSystem.UpgradeItem(generator, true);
        UIScriptableObject.NumGen = generator.numGenerators;
        UIScriptableObject.NumMetal = inventory.GetItemAmount(0);
    }

    public void UpgradeLights()
    {
        if (!upgradeSystem.HasFunds(lightCostObject)) return;

        foreach (Light light in lights)
        {
            if (light != null)
            {
                // We don't want to charge lights for each light
                upgradeSystem.UpgradeItem(light, false);
                RemoveFogPanels(light);
            }
        }

        Storage.Drain(lightCostObject.Cost);
        upgradeSystem.IncreaseCost(lightCostObject);
    }

    public void UpgradeCrank()
    {
        if (!upgradeSystem.HasFunds(crankSystem)) return;

        upgradeSystem.UpgradeItem(crankSystem, true);
        upgradeSystem.IncreaseCost(crankCostObject);
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
        crankSystem.CrankSoundEvent.AddListener(audioSystem.PlayCrankSound);
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
        if (!GameOver)
        {
            Timer += Time.deltaTime;
            if (Timer >= Tick)
            {
                powerSystem.Tick();
                Timer = 0;
            }
        }

        if (lowPower)
        {
            if (Storage.Power <= 0)
            {
                GameOverTimer += Time.deltaTime;
                if (GameOverTimer > 5)
                {
                    Debug.Log("GAME OVER!");
                    SceneManager.LoadScene(1);
                    // TriggerGameOver?.Invoke();
                    GameOver = true;
                    // StartCoroutine(QuitAfterDelay(5f));
                }
            }
            else
            {
                ResetErrorUI?.Invoke();
                GameOverTimer = 0;
                StopAlarmSoundEvent?.Invoke();
            }
        }
    }

    private IEnumerator<WaitForSeconds> QuitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Quitting!");
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        Storage.ResetPower();
        UIScriptableObject.Reset();
        inventory.Clear();

        generatorCostObject.Cost = 20;
        lightCostObject.Cost = 50;
        crankCostObject.Cost = 100;
    }

    void BuildSomething(GridCell cell)
    {
        if (Storage.Power >= 5)
        {
            GameObject newObject = buildSystem.Build(lightPrefab, cell);
            cell.SetOccupant(newObject);

            Storage.Drain(5);
            IPowerConsumer consumer = newObject.GetComponent<IPowerConsumer>();
            powerConsumers.Add(consumer);
            Light lightScript = newObject.GetComponent<Light>();

            // We need to get the current level of the lights. We should just be able to get it from the first light
            int level = lights[0].lightRadius;
            lightScript.lightRadius = level;
            lights.Add(lightScript);

            RemoveFogPanels(cell);

            UIScriptableObject.NumLights = lights.Count();
            PlaceSoundEvent?.Invoke();
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
    }
}
