using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{

    public static MapManager instance;

    [Header("Map Settings")]
    [SerializeField] private int width = 80;
    [SerializeField] private int height = 45;
    [SerializeField] private int roomMaxSize = 10;
    [SerializeField] private int roomMinSize = 6;
    [SerializeField] private int maxRooms = 30;
    [SerializeField] private int maxMonstersPerRoom = 2;
    [SerializeField] private int maxItemsPerRoom = 2;


    [Header("Tiles")]
    [SerializeField] private TileBase floorTile;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase fogTile;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap floorMap;
    [SerializeField] private Tilemap obstacleMap;
    [SerializeField] private Tilemap fogMap;


    [Header("Features")]
    [SerializeField] private List<RectangularRoom> rooms = new List<RectangularRoom>();
    [SerializeField] private List<Vector3Int> visibleTiles = new List<Vector3Int>();

    private Dictionary<Vector3Int, TileData> tiles = new Dictionary<Vector3Int, TileData>();
    private Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();


    public int Width { get => width; }
    public int Height { get => height; }

    public TileBase FloorTile { get => floorTile; }
    public TileBase WallTile { get => wallTile; }

    public Tilemap FloorMap { get => floorMap; }
    public Tilemap ObstacleMap { get => obstacleMap; }
    public Tilemap FogMap { get => fogMap; }

    public List<RectangularRoom> Rooms { get => rooms; }
    public Dictionary<Vector2Int, Node> Nodes { get => nodes; set => nodes = value; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        ProcGen procGen = gameObject.AddComponent<ProcGen>();
        procGen.GenerateDungeon(width, height, roomMaxSize, roomMinSize, maxRooms, maxMonstersPerRoom, maxItemsPerRoom, rooms);

        AddTileMapToDictionary(floorMap);
        AddTileMapToDictionary(obstacleMap);

        SetupFogMap();

        // Setting up camera to view the game better
        Camera.main.transform.position = new Vector3(40, 20.25f, -10);
        Camera.main.orthographicSize = 27;
    }

    /// <summary>Return True if x and y are inside of the bounds of this map</summary>
    public bool InBounds(int x, int y) => 0 <= x && x < width && 0 <= y && y < height;


    public void CreateEntity(string entity, Vector2 position)
    {
        switch(entity)
        {
            case "Player":
                Instantiate(Resources.Load<GameObject>("Player"),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Player";
                break;
            case "Commoner":
                Instantiate(Resources.Load<GameObject>("Commoner"),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Commoner";
                break;
            case "Templar":
                Instantiate(Resources.Load<GameObject>("Templar"),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Templar";
                break;
            case "Neon Blood Vial":
                Instantiate(Resources.Load<GameObject>("Neon Blood Vial"),
                    new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Neon Blood Vial";
                break;
            default:
                Debug.Log("Entity not found");
                break;

        }
    }

    public void UpdateFogMap(List<Vector3Int> playerFOV)
    {
        foreach (Vector3Int pos in visibleTiles)
        {
            if (!tiles[pos].IsExplored)
                tiles[pos].IsExplored = true;

            tiles[pos].IsVisible = false;
            fogMap.SetColor(pos, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        }

        visibleTiles.Clear();

        foreach (Vector3Int pos in playerFOV)
        {
            tiles[pos].IsVisible = true;
            fogMap.SetColor(pos, Color.clear);
            visibleTiles.Add(pos);
        }
    }

    public void SetEntitiesVisibilities()
    {
        foreach (Entity entity in GameManager.instance.Entities)
        {
            if (entity.GetComponent<Player>())
            {
                continue;
            }

            Vector3Int entityPosition = floorMap.WorldToCell(entity.transform.position);

            if (visibleTiles.Contains(entityPosition))
            {
                entity.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                entity.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void AddTileMapToDictionary(Tilemap tilemap)
    {
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos))
            {
                continue;
            }

            TileData tile = new TileData();
            tiles.Add(pos, tile);
        }
    }

    private void SetupFogMap()
    {
        foreach (Vector3Int pos in tiles.Keys)
        {
            fogMap.SetTile(pos, fogTile);
            fogMap.SetTileFlags(pos, TileFlags.None);
        }
    }
}   
