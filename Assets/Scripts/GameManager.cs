using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private float Timer;


    public InventoryObject inventory;
    private ClickHandler clickHandler;
    private PowerSystem powerSystem;
    private BuildSystem buildSystem;
    private CursorChangerSystem cursorChangerSystem;

    private List<IPowerProducer> powerProducers = new List<IPowerProducer>();
    private List<IPowerConsumer> powerConsumers = new List<IPowerConsumer>();

    [SerializeField] private GameObject light;

    [SerializeField] private GameObject ClickGeneratorPrefab;
    [SerializeField] private PowerStorage Storage;
    [SerializeField] private float Tick = 1; // How many seconds per tick?

    [SerializeField] private Texture2D PickaxeCursor;
    [SerializeField] private Texture2D NormalCursor;
    [SerializeField] private Texture2D InteractCursor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buildSystem = gameObject.AddComponent<BuildSystem>();
        powerSystem = new PowerSystem(Storage, powerProducers, powerConsumers);

        SetupCursorChangingSystem();
        SetupClickHandler();
        SetupClickGenerator();
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
        if (Storage.Power >= 5)
        {
            GameObject newObject = buildSystem.Build(light, cell);
            cell.SetOccupant(newObject);

            GridGenerator gg = cell.GetComponentInParent<GridGenerator>();

            if (gg.gridFogTiles.TryGetValue(cell, out GameObject fogTile))
            {
                Destroy(fogTile);

                List<Vector3> neighborPositions = new List<Vector3>
                {
                    cell.transform.position + new Vector3(1, 0, 0),
                    cell.transform.position + new Vector3(-1, 0, 0),
                    cell.transform.position + new Vector3(0, 0, 1),
                    cell.transform.position + new Vector3(0, 0, -1)
                };

                foreach (Vector3 neighborPos in neighborPositions)
                {
                    Debug.Log(neighborPos);
                    if (gg.gridCellPositions.TryGetValue(neighborPos, out GridCell neighborCell))
                    {
                        if (gg.gridFogTiles.TryGetValue(neighborCell, out GameObject nfogTile))
                        {
                            Destroy(nfogTile);
                        }
                    }
                }
                    // Vector3 neighborPos = cell.transform.position + new Vector3(1, 0, 0);
                
            }

            Storage.Drain(5);
            IPowerConsumer consumer = newObject.GetComponent<IPowerConsumer>();
            powerConsumers.Add(consumer);
        }
        else
        {
            Debug.Log("Don't have enough power");
        }

        // foreach (var invSlot in inventory.Container.Items)
        //     {
        //         if (invSlot.Item.Name == "Metal" && invSlot.Amount >= 20)
        //         {
        //             GameObject newObject = buildSystem.Build(light, cell);
        //             cell.SetOccupant(newObject);
        //             invSlot.RemoveAmount(20);
        //             IPowerConsumer consumer = newObject.GetComponent<IPowerConsumer>();
        //             powerConsumers.Add(consumer);
        //         }
        //         else
        //         {
        //             Debug.Log("Don't have enough materials");
        //         }
        //     }
    }
}
