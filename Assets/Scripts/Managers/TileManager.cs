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
    SpawnPoints
}

/// <summary>
/// Handle tile specific behaviours like movements to new tiles. Has the knowledge of who is where.
/// </summary>
public class TileManager : MonoBehaviour {

    private static TileManager instance = null;

    [System.Obsolete]
    Dictionary<Tile, List<MonsterInstance>> monstersOnTileOld = new Dictionary<Tile, List<MonsterInstance>>();
    [System.Obsolete]
    Dictionary<Tile, List<KeeperInstance>> keepersOnTileOld = new Dictionary<Tile, List<KeeperInstance>>();
    [System.Obsolete]
    Dictionary<KeeperInstance, Tile> getTileFromKeeperOld = new Dictionary<KeeperInstance, Tile>();

    Dictionary<Tile, List<PawnInstance>> monstersOnTile = new Dictionary<Tile, List<PawnInstance>>();
    Dictionary<Tile, List<PawnInstance>> keepersOnTile = new Dictionary<Tile, List<PawnInstance>>();
    Dictionary<PawnInstance, Tile> getTileFromKeeper = new Dictionary<PawnInstance, Tile>();

    Tile prisonerTile;
    GameObject tiles;
    GameObject beginTile;
    GameObject endTile;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            InitializeStateOld();
        }
        else if (instance != this)
        {
            instance.ResetTileManagerOld();
            Destroy(gameObject);
        }
        DontDestroyOnLoad(instance.gameObject);
    
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

    [System.Obsolete]
    /// <summary>
    /// Move keeper and its followers from a tile to another.
    /// </summary>
    /// <param name="keeper">The keeper to move</param>
    /// <param name="from">The origin tile</param>
    /// <param name="direction">The direction of the movement from the origin tile</param>
    public void MoveKeeperOld(KeeperInstance keeper, Tile from, Direction direction, int costAction)
    {
        Tile destination = from.Neighbors[(int)direction];
        if (destination == null)
            return;

        RemoveKeeperFromTileOld(from, keeper);
        AddKeeperOnTileOld(destination, keeper);
        Transform[] spawnPoints = GetSpawnPositions(destination, direction);

        // Physical movement
        keeper.StartBetweenTilesAnimation(spawnPoints[0].position);

        keeper.ActionPoints -= (short)costAction;
        GameObject goCurrentCharacter;

        for (int i = 0; i < keeper.Keeper.GoListCharacterFollowing.Count; i++)
        {
            goCurrentCharacter = keeper.Keeper.GoListCharacterFollowing[i];

            if (goCurrentCharacter.GetComponent<PrisonerInstance>() != null)
            {
                prisonerTile = destination;
                goCurrentCharacter.GetComponent<PrisonerInstance>().StartBetweenTilesAnimation(spawnPoints[i + 1 % spawnPoints.Length].position);

            }
            else
            {
                RemoveKeeperFromTileOld(from, goCurrentCharacter.GetComponent<KeeperInstance>());
                AddKeeperOnTileOld(destination, goCurrentCharacter.GetComponent<KeeperInstance>());
                goCurrentCharacter.GetComponent<KeeperInstance>().ActionPoints = 0;
                goCurrentCharacter.GetComponent<KeeperInstance>().StartBetweenTilesAnimation(spawnPoints[i + 1 % spawnPoints.Length].position);
            }

        }
    }

    public void MoveKeeper(PawnInstance keeper, Tile from, Direction direction, int costAction)
    {
        Tile destination = from.Neighbors[(int)direction];
        if (destination == null)
            return;

        RemoveKeeperFromTile(from, keeper);
        AddKeeperOnTile(destination, keeper);
        Transform[] spawnPoints = GetSpawnPositions(destination, direction);

        // Physical movement
        keeper.GetComponent<Behaviour.AnimatedPawn>().StartBetweenTilesAnimation(spawnPoints[0].position);

        // TODO remove this line when animated pawn is implemented
        keeper.transform.position = spawnPoints[0].position;

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

                // TODO remove this line when animated pawn is implemented
                keeper.transform.position = spawnPoints[i + 1 % spawnPoints.Length].position;
            }

        }
    }

    [System.Obsolete]
    /// <summary>
    /// Move a monster to another tile.
    /// </summary>
    /// <param name="monster">The monster to move</param>
    /// <param name="from">The origin tile of the monster</param>
    /// <param name="direction">The direction of the movement from the origin tile</param>
    public void MoveMonsterOld(MonsterInstance monster, Tile from, Direction direction)
    {
        RemoveMonsterFromTileOld(from, monster);
        AddMonsterOnTileOld(from.Neighbors[(int)direction], monster);        
    }

    [System.Obsolete]
    /// <summary>
    /// Destroy monsters beaten in battle and remove them from dictionaries.
    /// </summary>
    /// <param name="tile">The tile concerned</param>
    public void RemoveDefeatedMonstersOld(Tile tile)
    {
        MonsterInstance[] removeIndex = new MonsterInstance[monstersOnTileOld[tile].Count];
        short nbrOfElementsToRemove = 0;

        List<ItemContainer> lootList = new List<ItemContainer>();
        Transform lastMonsterPosition = null;
        foreach (MonsterInstance mi in monstersOnTileOld[tile])
        {
            if (mi.CurrentHp == 0)
            {
                lastMonsterPosition = mi.transform;
                if (mi.deathParticles != null)
                {
                    ParticleSystem ps = Instantiate(mi.deathParticles, mi.transform.parent);
                    ps.transform.position = mi.transform.position;
                    ps.Play();
                    Destroy(ps.gameObject, ps.main.duration);
                }

                lootList.AddRange(mi.ComputeLoot());

                removeIndex[nbrOfElementsToRemove] = mi;
                nbrOfElementsToRemove++;
            }
        }


        if (lootList.Count > 0)
        {
            ItemManager.AddItemOnTheGround(tile, lastMonsterPosition, lootList.ToArray());
        }

        int elementsRemoved = 0;
        for (int j = 0; j < monstersOnTileOld[tile].Count; j++)
        {
            for (int i = 0; i < removeIndex.Length; i++)
            {
                if (elementsRemoved == nbrOfElementsToRemove)
                    break;

                if (monstersOnTileOld[tile][j] == removeIndex[i])
                {
                    monstersOnTileOld[tile].RemoveAt(j);
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

        if (monstersOnTileOld[tile].Count == 0)
            monstersOnTileOld.Remove(tile);
    }

    [System.Obsolete]
    /// <summary>
    /// Remove a killed keeper from play
    /// </summary>
    /// <param name="keeper">The keeper to remove</param>
    public void RemoveKilledKeeperOld(KeeperInstance keeper)
    {
        RemoveKeeperFromTileOld(getTileFromKeeperOld[keeper], keeper);
        getTileFromKeeperOld.Remove(keeper);
    }

    [System.Obsolete]
    /// <summary>
    /// Add a monster on a tile
    /// </summary>
    /// <param name="tile">A Tile</param>
    /// <param name="monster">A new MonsterInstance</param>
    private void AddMonsterOnTileOld(Tile tile, MonsterInstance monster)
    {
        if (monstersOnTileOld.ContainsKey(tile))
        {
            monstersOnTileOld[tile].Add(monster);
        }
        else
        {
            List<MonsterInstance> newList = new List<MonsterInstance>();
            newList.Add(monster);
            monstersOnTileOld.Add(tile, newList);
        }
    }

    private void AddMonsterOnTile(Tile tile, PawnInstance monster)
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

    [System.Obsolete]
    /// <summary>
    /// Update tile references for a keeper.
    /// </summary>
    /// <param name="tile">New tile</param>
    /// <param name="keeper">KeeperInstance</param>
    private void AddKeeperOnTileOld(Tile tile, KeeperInstance keeper)
    {
        if (keepersOnTileOld.ContainsKey(tile))
        {
            keepersOnTileOld[tile].Add(keeper);
        }

        else
        {
            List<KeeperInstance> newList = new List<KeeperInstance>();
            newList.Add(keeper);
            keepersOnTileOld.Add(tile, newList);
        }

        if (getTileFromKeeperOld.ContainsKey(keeper))
            getTileFromKeeperOld[keeper] = tile;
        else
            getTileFromKeeperOld.Add(keeper, tile);
    }

    private void AddKeeperOnTile(Tile tile, PawnInstance keeper)
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
    }

    [System.Obsolete]
    /// <summary>
    /// Remove monster from a tile after a movement.
    /// </summary>
    /// <param name="tile">Old tile</param>
    /// <param name="monster">MonsterInstance</param>
    private void RemoveMonsterFromTileOld(Tile tile, MonsterInstance monster)
    {
        monstersOnTileOld[tile].Remove(monster);
    }

    private void RemoveMonsterFromTile(Tile tile, PawnInstance monster)
    {
        if (monster.GetComponent<Behaviour.Monster>() != null)
            MonstersOnTile[tile].Remove(monster);
        else
            Debug.Log("Could not add monster because Monster component missing.");
    }

    [System.Obsolete]
    /// <summary>
    /// Remove keeper from a tile after a movement.
    /// </summary>
    /// <param name="tile">Old tile</param>
    /// <param name="keeper">KeeperInstance</param>
    private void RemoveKeeperFromTileOld(Tile tile, KeeperInstance keeper)
    {
        keepersOnTileOld[tile].Remove(keeper);
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

    [System.Obsolete]
    public void ResetTileManagerOld()
    {
        instance.monstersOnTileOld.Clear();
        instance.keepersOnTileOld.Clear();
        instance.getTileFromKeeperOld.Clear();

        // Re-initialize
        instance.InitializeStateOld();
    }

    public void ResetTileManager()
    {
        instance.MonstersOnTile.Clear();
        instance.KeepersOnTile.Clear();
        instance.GetTileFromKeeper.Clear();

        // Re-initialize
        instance.InitializeState();
    }

    [System.Obsolete]
    private void InitializeStateOld()
    {
        beginTile = GameObject.FindGameObjectWithTag("BeginTile");
        endTile = GameObject.FindGameObjectWithTag("EndTile");
        if (beginTile == null)
        {
            Debug.Log("No tag BeginTile on the first tile has been set.");
            return;
        }
        if (endTile == null)
        {
            Debug.Log("No tag EndTile on the last tile has been set.");
            return;
        }
        instance.prisonerTile = beginTile.GetComponentInParent<Tile>();
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersListOld)
        {
            instance.AddKeeperOnTileOld(instance.prisonerTile, ki);
        }
        instance.InitializeMonstersOld();
    }

    private void InitializeState()
    {
        beginTile = GameObject.FindGameObjectWithTag("BeginTile");
        endTile = GameObject.FindGameObjectWithTag("EndTile");
        if (beginTile == null)
        {
            Debug.Log("No tag BeginTile on the first tile has been set.");
            return;
        }
        if (endTile == null)
        {
            Debug.Log("No tag EndTile on the last tile has been set.");
            return;
        }
        instance.prisonerTile = beginTile.GetComponentInParent<Tile>();
        foreach (PawnInstance pi in GameManager.Instance.AllKeepersList)
        {
            instance.AddKeeperOnTile(instance.prisonerTile, pi);
        }
        instance.InitializeMonsters();
    }

    [System.Obsolete]
    private void InitializeMonstersOld()
    {
        HelperRoot helperRoot = FindObjectOfType<HelperRoot>();
        if (helperRoot == null)
        {
            Debug.Log("No helper root found in scene, can't retrieve tiles.");
            return;
        }
        instance.tiles = helperRoot.gameObject;
        foreach (MonsterInstance mi in instance.tiles.GetComponentsInChildren<MonsterInstance>())
        {
            instance.AddMonsterOnTileOld(mi.GetComponentInParent<Tile>(), mi);
        }
    }

    private void InitializeMonsters()
    {
        HelperRoot helperRoot = FindObjectOfType<HelperRoot>();
        if (helperRoot == null)
        {
            Debug.Log("No helper root found in scene, can't retrieve tiles.");
            return;
        }
        instance.tiles = helperRoot.gameObject;
        foreach (Behaviour.Monster m in instance.tiles.GetComponentsInChildren<Behaviour.Monster>())
        {
            instance.AddMonsterOnTile(m.GetComponentInParent<Tile>(), m.getPawnInstance);
        }
    }

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------- Accessors ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */

    public static TileManager Instance
    {
        get
        {
            return instance;
        }
    }

    [System.Obsolete]
    public Dictionary<Tile, List<MonsterInstance>> MonstersOnTileOld
    {
        get
        {
            return monstersOnTileOld;
        }

        private set { }

    }

    [System.Obsolete]
    public Dictionary<Tile, List<KeeperInstance>> KeepersOnTileOld
    {
        get
        {
            return keepersOnTileOld;
        }
        private set { }

    }

    [System.Obsolete]
    public Dictionary<KeeperInstance, Tile> GetTileFromKeeperOld
    {
        get
        {
            return getTileFromKeeperOld;
        }
        private set { }

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
}
