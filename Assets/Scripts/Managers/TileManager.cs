using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Return the position TilePrefab's children in hierarchy.
/// TilePrefab is the first child of a Tile
/// </summary>
public enum TilePrefabChildren
{
    Model,
    PortalTriggers,
    SpawnPoints,
    BeginPositions // Only on begin tile
}

/// <summary>
/// Handle tile specific behaviours like movements to new tiles. Has the knowledge of who is where.
/// </summary>
public class TileManager : MonoBehaviour {

    private static TileManager instance = null;

    Dictionary<Tile, List<PawnInstance>> monstersOnTile = new Dictionary<Tile, List<PawnInstance>>();
    Dictionary<Tile, List<PawnInstance>> keepersOnTile = new Dictionary<Tile, List<PawnInstance>>();
    Dictionary<PawnInstance, Tile> getTileFromKeeper = new Dictionary<PawnInstance, Tile>();

    Tile prisonerTile;
    GameObject tiles;
    [SerializeField]
    Tile beginTile;
    [SerializeField]
    Tile endTile;

    [SerializeField]
    int levelWidth;
    [SerializeField]
    int levelHeight;

    // Tags
    string beginTileTag = "BeginTile";
    string endTileTag = "EndTile";

    // Called after tiles instantiation as TileManager must be on the gameobject containing all tiles
    void Start()
    {
        Init();
    }

    public void Init()
    {
        instance = this;
        InitializeState();
        GameManager.Instance.RegisterTileManager(this);
    }

    public void MoveKeeper(PawnInstance keeper, Tile from, Direction direction, int costAction)
    {
        Tile destination = from.Neighbors[(int)direction];
        if (destination == null)
        {
            Debug.Log("Destination Unknown");
            return;
        }


        RemoveKeeperFromTile(from, keeper);
        AddKeeperOnTile(destination, keeper);
        Transform[] spawnPoints = GetSpawnPositions(destination, direction);

        // Physical movement
        keeper.GetComponent<Behaviour.AnimatedPawn>().StartBetweenTilesAnimation(spawnPoints[0].position);

        Behaviour.Keeper keeperComponent = keeper.GetComponent<Behaviour.Keeper>();
        keeperComponent.ActionPoints -= (short)costAction;
        keeperComponent.getPawnInstance.CurrentTile = destination;

        GameObject goCurrentCharacter;

        for (int i = 0; i < keeperComponent.GoListCharacterFollowing.Count; i++)
        {
            goCurrentCharacter = keeperComponent.GoListCharacterFollowing[i];

            if (goCurrentCharacter.GetComponent<Behaviour.Escortable>() != null)
            {
                goCurrentCharacter.GetComponent<PawnInstance>().CurrentTile = destination;
                goCurrentCharacter.GetComponent<Behaviour.AnimatedPawn>().StartBetweenTilesAnimation(spawnPoints[i + 1 % spawnPoints.Length].position);
            }

        }
    }

    public void MoveMonster(PawnInstance monster, Tile from, Direction direction)
    {
        RemoveMonsterFromTile(from, monster);
        AddMonsterOnTile(from.Neighbors[(int)direction], monster);
    }

    /// <summary>
    /// Destroy monsters beaten in battle and remove them from dictionaries.
    /// </summary>
    /// <param name="tile">The tile concerned</param>
    public void RemoveDefeatedMonsters(Tile tile)
    {
        PawnInstance[] removeIndex = new PawnInstance[monstersOnTile[tile].Count];
        short nbrOfElementsToRemove = 0;

        List<ItemContainer> lootList = new List<ItemContainer>();
        Transform lastMonsterPosition = null;
        foreach (PawnInstance pi in monstersOnTile[tile])
        {
            if (pi.GetComponent<Behaviour.Mortal>().CurrentHp == 0)
            {
                lastMonsterPosition = pi.transform;
                if (pi.GetComponent<Behaviour.Mortal>().DeathParticles != null)
                {
                    ParticleSystem ps = Instantiate(pi.GetComponent<Behaviour.Mortal>().DeathParticles, pi.transform.parent);
                    ps.transform.position = pi.transform.position;
                    ps.Play();
                    Destroy(ps.gameObject, ps.main.duration);
                }
                pi.GetComponent<Behaviour.Inventory>().ComputeItems();
                lootList.AddRange(pi.GetComponent<Behaviour.Inventory>().Items);

                removeIndex[nbrOfElementsToRemove] = pi;
                nbrOfElementsToRemove++;
            }
        }


        if (lootList.Count > 0)
        {
            ItemManager.AddItemOnTheGround(tile, lastMonsterPosition, lootList.ToArray());
        }

        int elementsRemoved = 0;
        for (int j = 0; j < monstersOnTile[tile].Count; j++)
        {
            for (int i = 0; i < removeIndex.Length; i++)
            {
                if (elementsRemoved == nbrOfElementsToRemove)
                    break;

                if (monstersOnTile[tile][j] == removeIndex[i])
                {
                    monstersOnTile[tile].RemoveAt(j);
                    elementsRemoved++;
                    break;
                }
            }
            if (elementsRemoved == nbrOfElementsToRemove)
                break;
        }

        for (int i = 0; i < nbrOfElementsToRemove; i++)
        {
            Destroy(removeIndex[i].gameObject, 0.1f);
        }

        if (monstersOnTile[tile].Count == 0)
            monstersOnTile.Remove(tile);
    }

    public void RemoveKilledKeeper(PawnInstance keeper)
    {
        RemoveKeeperFromTile(getTileFromKeeper[keeper], keeper);
        getTileFromKeeper.Remove(keeper);
    }

    public void AddMonsterOnTile(Tile tile, PawnInstance monster)
    {
        if (monster.GetComponent<Behaviour.Monster>() == null)
        {
            Debug.Log("Can't add monster to tile, missing component Monster.");
            return;
        }

        if (MonstersOnTile.ContainsKey(tile))
        {
            MonstersOnTile[tile].Add(monster);
        }
        else
        {
            List<PawnInstance> newList = new List<PawnInstance>();
            newList.Add(monster);
            MonstersOnTile.Add(tile, newList);
        }
    }

    public void AddMonsterOnTile(PawnInstance monster)
    {
        Tile tile = monster.GetComponentInParent<Tile>();
        if (monster.GetComponent<Behaviour.Monster>() == null)
        {
            Debug.Log("Can't add monster to tile, missing component Monster.");
            return;
        }

        if (MonstersOnTile.ContainsKey(tile))
        {
            MonstersOnTile[tile].Add(monster);
        }
        else
        {
            List<PawnInstance> newList = new List<PawnInstance>();
            newList.Add(monster);
            MonstersOnTile.Add(tile, newList);
        }
    }

    public void AddKeeperOnTile(Tile tile, PawnInstance keeper)
    {
        if (keeper.GetComponent<Behaviour.Keeper>() == null)
        {
            Debug.Log("Can't add keeper to tile, missing component Keeper.");
            return;
        }

        if (KeepersOnTile.ContainsKey(tile))
        {
            KeepersOnTile[tile].Add(keeper);
        }
        else
        {
            List<PawnInstance> newList = new List<PawnInstance>();
            newList.Add(keeper);
            KeepersOnTile.Add(tile, newList);
        }

        if (GetTileFromKeeper.ContainsKey(keeper))
            GetTileFromKeeper[keeper] = tile;
        else
            GetTileFromKeeper.Add(keeper, tile);

        keeper.CurrentTile = tile;
    }

    private void RemoveMonsterFromTile(Tile tile, PawnInstance monster)
    {
        if (monster.GetComponent<Behaviour.Monster>() != null)
            MonstersOnTile[tile].Remove(monster);
        else
            Debug.Log("Could not add monster because Monster component missing.");
    }

    private void RemoveKeeperFromTile(Tile tile, PawnInstance keeper)
    {
        if (keeper.GetComponent<Behaviour.Keeper>() != null)
            KeepersOnTile[tile].Remove(keeper);
        else
            Debug.Log("Could not add keeper because Keeper component missing.");
    }

    /// <summary>
    /// Return the spawn positions based on destination tile and the direction of the movement.
    /// </summary>
    /// <param name="destinationTile">Destination Tile</param>
    /// <param name="moveDirection">Movement direction</param>
    /// <returns></returns>
    private Transform[] GetSpawnPositions(Tile destinationTile, Direction moveDirection)
    {
        Transform[] spawnPositions = new Transform[2];
        Transform[] tmp = new Transform[3];
        Transform allSpawnPoints = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.SpawnPoints);

        tmp = allSpawnPoints.GetChild((int)Utils.GetOppositeDirection(moveDirection)).GetComponentsInChildren<Transform>();
      
        spawnPositions[0] = tmp[1];
        spawnPositions[1] = tmp[2];
        return spawnPositions;
    }

    private void InitializeState()
    {
        tiles = gameObject;
        if (beginTile == null)
        {
            Debug.Log("Reference to begin tile is null.");
            return;
        }
        if (endTile == null)
        {
            Debug.Log("Reference to end tile is null.");
            return;
        }

    }

    public void RegisterPrisonerPosition(PawnInstance _prisoner)
    {
        prisonerTile = beginTile;
        _prisoner.CurrentTile = beginTile;
    }

    public void ChangeBeginTile(string _newTileName)
    {
        for (int i = 0; i < tiles.transform.childCount; i++)
        {
            if (tiles.transform.GetChild(i).name.Equals(_newTileName))
            {
                beginTile = tiles.transform.GetChild(i).GetComponent<Tile>();
                break;
            }
        }
    }

    public void ChangeEndTile(string _newTileName)
    {
        for (int i = 0; i < tiles.transform.childCount; i++)
        {
            if (tiles.transform.GetChild(i).name.Equals(_newTileName))
            {
                endTile = tiles.transform.GetChild(i).GetComponent<Tile>();
                break;
            }
        }
    }

    #region Accessors
    public static TileManager Instance
    {
        get
        {
            return instance;
        }
    }

    public Dictionary<Tile, List<PawnInstance>> MonstersOnTile
    {
        get
        {
            return monstersOnTile;
        }
    }

    public Dictionary<Tile, List<PawnInstance>> KeepersOnTile
    {
        get
        {
            return keepersOnTile;
        }
    }

    public Dictionary<PawnInstance, Tile> GetTileFromKeeper
    {
        get
        {
            return getTileFromKeeper;
        }
    }

    public Tile PrisonerTile
    {
        get
        {
            return prisonerTile;
        }

        set
        {
            prisonerTile = value;
        }
    }

    public Tile BeginTile
    {
        get
        {
            return beginTile;
        }

        set
        {
            beginTile = value;
        }
    }

    public Transform[] GetBeginPositions
    {
        get
        {
            int beginPositionsCount = beginTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.BeginPositions).childCount;
            Transform[] beginPositions = new Transform[beginPositionsCount];
            for (int i = 0; i < beginPositionsCount; i++)
                beginPositions[i] = beginTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.BeginPositions).GetChild(i);
            return beginPositions;
        }
    }

    public Tile EndTile
    {
        get
        {
            return endTile;
        }

        set
        {
            endTile = value;
        }
    }
    #endregion
}
