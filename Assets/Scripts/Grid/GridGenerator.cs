using UnityEngine;
using System;
using System.Collections.Generic;
using Random = System.Random;
using System.Linq;
using UnityEngine.Events;

public class GridGenerator : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int walkLength = 50;
    [SerializeField] private Vector3Int initialGridSize = new Vector3Int(5, 0, 5);

    [Header("Seed Control")]
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private int seed = 0;

    [Header("Generated Prefabs")]
    [SerializeField] private GameObject fogTilePrefab;
    [SerializeField] private GameObject floorTilePrefab;
    [SerializeField] private GameObject metalTilePrefab;
    [SerializeField] private int metalTileChance = 20;


    public Dictionary<Vector3Int, bool> gridLights = new Dictionary<Vector3Int, bool>();

    private Grid grid;
    private HashSet<Vector3Int> visitedTiles = new HashSet<Vector3Int>();
    private GameObject fogRootGameObject;

    public Dictionary<GridCell, GameObject> gridFogTiles = new Dictionary<GridCell, GameObject>();
    public Dictionary<Vector3, GridCell> gridCellPositions = new Dictionary<Vector3, GridCell>();

    public UnityEvent gridCreated = new UnityEvent();

    void Start()
    {
        grid = GetComponent<Grid>();
        Generate();
        gridCreated?.Invoke();
        Debug.Log("Grid has been created");
    }

    private void ClearGrid()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        gridLights.Clear();
    }

    private int GetSeed()
    {
        int finalSeed = useRandomSeed ? DateTime.Now.GetHashCode() : seed;
        Debug.Log("Using Seed: " + finalSeed);
        return finalSeed;
    }

    private void WalkTiles(System.Random rand)
    {
        Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.right,
            Vector3Int.left,
            Vector3Int.forward,
            Vector3Int.back
        };

        Vector3Int current = GenerateInitialTiles();
        visitedTiles.Add(current);

        for (int i = 0; i < walkLength; i++)
        {
            Vector3Int dir = directions[rand.Next(directions.Length)];
            Vector3Int next = current + dir;
            current = next;

            // If the tile was already visited just skip it
            if (visitedTiles.Contains(current))
            {
                continue;
            }

            visitedTiles.Add(current);
            GenerateTiles(current);
        }
    }

    private Vector3Int GenerateInitialTiles() {
        Vector3Int current = new Vector3Int(0, 0, 0);
        
        // Generate based on some grid initial size
        for (int i = -initialGridSize.x/2; i < initialGridSize.x; i ++)
        {
            for (int j = -initialGridSize.z / 2; j < initialGridSize.z; j ++)
            {
                Vector3Int coords = new Vector3Int(i, 0, j);
                GenerateTiles(coords);
                visitedTiles.Add(coords);
                current = coords;
            }
        }

        return current; // Returns end of coords
    }

    void ReplaceMetalTile(GridCell cell)
    {
        GameObject createdObject = Instantiate(floorTilePrefab, cell.transform.position, Quaternion.identity, transform);
        GridCell gridCell = createdObject.AddComponent<GridCell>();

        Destroy(cell.gameObject);
    }


    void GenerateTiles(Vector3Int coordinate)
    {
        var position = grid.GetCellCenterWorld(coordinate);

        // Hey is the chanc
        Random random = new Random();
        int randomNumberInRange = random.Next(101);

        GameObject prefabToGenerate = floorTilePrefab;

        bool isMetalTile = randomNumberInRange < metalTileChance;

        if (isMetalTile)
        {
            prefabToGenerate = metalTilePrefab;
        }

        Vector3 gridPosition = new Vector3(position.x, 0, position.z);
        GameObject createdObject = Instantiate(prefabToGenerate, gridPosition, Quaternion.identity, transform);

        GridCell gridCell = createdObject.AddComponent<GridCell>();

        gridCellPositions.Add(gridPosition, gridCell);

        GameObject fog = Instantiate(fogTilePrefab, gridCell.transform.position + Vector3.up * 0.1f, Quaternion.Euler(90f, 0f, 0f), fogRootGameObject.transform);
        gridFogTiles[gridCell] = fog;

        if (isMetalTile)
        {
            gridCell.SetOccupant(createdObject);
            createdObject.GetComponent<MineableItem>().OreDepleated.AddListener(ReplaceMetalTile);
        }

        gridLights[coordinate] = false;
    }

    private void Generate()
    {
        ClearGrid();

        fogRootGameObject = new GameObject("Fog");
        fogRootGameObject.transform.parent = grid.transform;

        // fogRootGameObject = Instantiate(fogRootGameObject, grid.transform);

        int randSeed = GetSeed();
        var rand = new System.Random(randSeed);

        WalkTiles(rand);
    }
}
