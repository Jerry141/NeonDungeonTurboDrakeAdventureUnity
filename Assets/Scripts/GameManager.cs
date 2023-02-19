using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    public static GameManager instance;

    [Header("Time")]
    [SerializeField] private float baseTime = 0.1f;
    [SerializeField] private float delayTime; // read only
    [SerializeField] private bool isPlayerTurn = true; // Player turn - true as game starts with players turn


    [Header("Entities")]
    [SerializeField] private int actorNum = 0;
    [SerializeField] private List<Entity> entities;
    [SerializeField] private List<Actor> actors;

    [Header("Death")]
    [SerializeField] private Sprite deadSprite;

    public bool IsPlayerTurn { get => isPlayerTurn; }

    public List<Entity> Entities { get => entities; }

    public List<Actor> Actors { get => actors; }

    public Sprite DeadSprite { get => deadSprite; }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneState sceneState = SaveManager.instance.Save.Scenes.Find(x => x.FloorNumber == SaveManager.instance.CurrentFloor);

        if (sceneState is not null)
        {
            LoadState(sceneState.GameState);
        }
        else
        {
            entities = new List<Entity>();
            actors = new List<Actor>();
        }
    }

    private void StartTurn() 
    {
        // Start the game
        if (actors[actorNum].GetComponent<Player>())
        {
            isPlayerTurn = true;
        }
        else
        {
            if (actors[actorNum].AI != null)
            {
                actors[actorNum].AI.RunAI();
            }
            else
            {
                Action.WaitAction();
            }
        }
    }

    public void EndTurn() 
    {
        if (actors[actorNum].GetComponent<Player>())
        {
            isPlayerTurn = false;
        }

        if (actorNum == actors.Count - 1)
        {
            actorNum = 0;
        }
        else
        {
            actorNum++;
        }

        StartCoroutine(TurnDelay());
    }

    private IEnumerator TurnDelay()
    {
        yield return new WaitForSeconds(delayTime);
        StartTurn();
    }

    public void AddEntity(Entity entity)
    {
        if (!entity.gameObject.activeSelf)
        {
            entity.gameObject.SetActive(true);
        }

        entities.Add(entity);
    }

    public void InsertEntity(Entity entity, int index)
    {
        if (!entity.gameObject.activeSelf)
        {
            entity.gameObject.SetActive(true);
        }
        entities.Insert(index, entity);
    }

    public void RemoveEntity(Entity entity)
    {
        entity.gameObject.SetActive(true);
        entities.Remove(entity);
    }

    public void AddActor(Actor actor)
    {
        actors.Add(actor);
        delayTime = SetTime();
    }

    public void InsertActor(Actor actor, int index)
    {
        actors.Insert(index, actor);
        delayTime = SetTime();
    }

    public void RemoveActor(Actor actor)
    {
        actors.Remove(actor);
        delayTime = SetTime();
    }

    // blocking movement by NPC
    public Actor GetActorAtLocation(Vector3 location)
    {
        foreach (Actor actor in actors)
        {
            if (actor.BlocksMovement && actor.transform.position == location)
            {
                return actor;
            }
        }
        return null;
    }

    private float SetTime() => baseTime / actors.Count;


    public GameState SaveState()
    {
        foreach (Item item in actors[0].Inventory.Items)
        {
            if (entities.Contains(item))
            {
                continue;
            }
            AddEntity(item);
        }

        GameState gameState = new GameState(entities: entities.ConvertAll(x => x.SaveState()));

        foreach (Item item in actors[0].Inventory.Items)
        {
            RemoveEntity(item);
        }

        return gameState;
    }

    public void LoadState(GameState state)
    {
        isPlayerTurn = false; // preventing player to move during loading

        if (entities.Count > 0)
        {
            foreach (Entity entity in entities)
            {
                Destroy(entity.gameObject);
            }

            entities.Clear();
            actors.Clear();
        }

        StartCoroutine(LoadEntityStates(state.Entities));
    }

    private IEnumerator LoadEntityStates(List<EntityState> entityStates)
    {
        int entityState = 0;

        while (entityState < entityStates.Count)
        {
            yield return new WaitForEndOfFrame();

            string entityName = entityStates[entityState].Name.Contains("'s rotting corpse.") ?
                entityStates[entityState].Name.Substring(entityStates[entityState].Name.LastIndexOf(' ') + 1) : entityStates[entityState].Name;

            if (entityStates[entityState].Type == EntityState.EntityType.Actor)
            {
                ActorState actorState = entityStates[entityState] as ActorState;
                Actor actor = MapManager.instance.CreateEntity(entityName, actorState.Position).GetComponent<Actor>();

                actor.LoadState(actorState);
            }
            else if (entityStates[entityState].Type == EntityState.EntityType.Item)
            {
                ItemState itemState = entityStates[entityState] as ItemState;
                Item item = MapManager.instance.CreateEntity(entityName, itemState.Position).GetComponent<Item>();

                item.LoadState(itemState);
            }

            entityState++;
        }

        isPlayerTurn = true; // allows player to move after loading
    }

}

[System.Serializable]

public class GameState
{
    [SerializeField] private List<EntityState> entities;

    public List<EntityState> Entities { get => entities; set => entities = value; }

    public GameState(List<EntityState> entities)
    {
        this.entities = entities;
    }
}
