using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TilePrefab is the first child of a Tile
public enum TilePrefabChildren
{
    Model,
    PortalTriggers,
    SpawnPoints
}

public class TileManager : MonoBehaviour {

    private static TileManager instance = null;

    Dictionary<Tile, List<MonsterInstance>> monstersOnTile = new Dictionary<Tile, List<MonsterInstance>>();
    Dictionary<Tile, List<KeeperInstance>> keepersOnTile = new Dictionary<Tile, List<KeeperInstance>>();
    Dictionary<KeeperInstance, Tile> getTileFromKeeper = new Dictionary<KeeperInstance, Tile>();
    Tile prisonerTile;
    public PrisonerInstance prisoner;

    // For testing, to delete
    public Tile monsterTileTest;
    public MonsterInstance monsterInstanceTest;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            prisonerTile = GameObject.FindGameObjectWithTag("BeginTile").GetComponentInParent<Tile>();
            foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
            {
                AddKeeperOnTile(prisonerTile, ki);
            }
            AddMonsterOnTile(monsterTileTest, monsterInstanceTest);
        }
        else if (instance != this)
        {
            Destroy(gameObject);

        }
        DontDestroyOnLoad(gameObject);
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

    public void MoveKeeper(KeeperInstance keeper, Tile from, Direction direction)
    {
        Tile destination = from.Neighbors[(int)direction];
        if (destination == null)
            return;

        RemoveKeeperFromTile(from, keeper);
        AddKeeperOnTile(destination, keeper);
        Transform[] spawnPoints = GetSpawnPositions(destination, direction);


        // Physical movement
        NavMeshAgent agent = keeper.GetComponent<NavMeshAgent>();

        agent.enabled = false;
        keeper.transform.position = spawnPoints[0].position;
        agent.enabled = true;

        GameObject goCurrentCharacter;

        for (int i = 0; i < keeper.Keeper.GoListCharacterFollowing.Count; i++)
        {
            goCurrentCharacter = keeper.Keeper.GoListCharacterFollowing[i];

            if (goCurrentCharacter.GetComponent<PrisonerInstance>() != null)
            {
                prisonerTile = destination;
            }
            else
            {
                RemoveKeeperFromTile(from, goCurrentCharacter.GetComponent<KeeperInstance>());
                AddKeeperOnTile(destination, goCurrentCharacter.GetComponent<KeeperInstance>());
            }

            agent = goCurrentCharacter.GetComponent<NavMeshAgent>();

            agent.enabled = false;
            goCurrentCharacter.transform.position = spawnPoints[i+1 % spawnPoints.Length].position;
            agent.enabled = true;
        }


        // Handle prisoner
        /*if (prisoner.KeeperFollowed != null && prisoner.KeeperFollowed == keeper)
        prisonerTile = destination;*/
    }

    public void MoveMonster(MonsterInstance monster, Tile from, Direction direction)
    {
        RemoveMonsterFromTile(from, monster);
        AddMonsterOnTile(from.Neighbors[(int)direction], monster);        
    }

    public void RemoveDefeatedMonsters(Tile tile)
    {
        foreach (MonsterInstance mi in monstersOnTile[tile])
        {
            Destroy(mi.gameObject);
        }
        monstersOnTile.Remove(tile);
    }

    private void AddMonsterOnTile(Tile tile, MonsterInstance monster)
    {
        if (monstersOnTile.ContainsKey(tile))
        {
            monstersOnTile[tile].Add(monster);
        }
        else
        {
            List<MonsterInstance> newList = new List<MonsterInstance>();
            newList.Add(monster);
            monstersOnTile.Add(tile, newList);
        }
    }

    private void AddKeeperOnTile(Tile tile, KeeperInstance keeper)
    {
        if (keepersOnTile.ContainsKey(tile))
        {
            keepersOnTile[tile].Add(keeper);
        }
        else
        {
            List<KeeperInstance> newList = new List<KeeperInstance>();
            newList.Add(keeper);
            keepersOnTile.Add(tile, newList);
        }

        if (getTileFromKeeper.ContainsKey(keeper))
            getTileFromKeeper[keeper] = tile;
        else
            getTileFromKeeper.Add(keeper, tile);
    }

    private void RemoveMonsterFromTile(Tile tile, MonsterInstance monster)
    {
        monstersOnTile[tile].Remove(monster);
    }

    private void RemoveKeeperFromTile(Tile tile, KeeperInstance keeper)
    {
        keepersOnTile[tile].Remove(keeper);
    }

    private Transform[] GetSpawnPositions(Tile destinationTile, Direction moveDirection)
    {
        Transform[] spawnPositions = new Transform[2];
        Transform[] tmp = new Transform[3];
        Transform allSpawnPoints = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.SpawnPoints);

        switch (moveDirection)
        {
            case Direction.North:
                tmp = allSpawnPoints.GetChild((int)Direction.South).GetComponentsInChildren<Transform>();
                break;
            case Direction.North_East:
                tmp = allSpawnPoints.GetChild((int)Direction.South_West).GetComponentsInChildren<Transform>();
                break;
            case Direction.North_West:
                tmp = allSpawnPoints.GetChild((int)Direction.South_East).GetComponentsInChildren<Transform>();
                break;
            case Direction.South:
                tmp = allSpawnPoints.GetChild((int)Direction.North).GetComponentsInChildren<Transform>();
                break;
            case Direction.South_East:
                tmp = allSpawnPoints.GetChild((int)Direction.North_West).GetComponentsInChildren<Transform>();
                break;
            case Direction.South_West:
                tmp = allSpawnPoints.GetChild((int)Direction.North_East).GetComponentsInChildren<Transform>();
                break;
        }

        spawnPositions[0] = tmp[1];
        spawnPositions[1] = tmp[2];
        return spawnPositions;
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

    public Dictionary<Tile, List<MonsterInstance>> MonstersOnTile
    {
        get
        {
            return monstersOnTile;
        }

        private set { }

    }

    public Dictionary<Tile, List<KeeperInstance>> KeepersOnTile
    {
        get
        {
            return keepersOnTile;
        }
        private set { }

    }

    public Dictionary<KeeperInstance, Tile> GetTileFromKeeper
    {
        get
        {
            return getTileFromKeeper;
        }
        private set { }

    }
}
