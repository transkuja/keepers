using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum TilePrefabChildren
{
    Model,
    NorthCollider,
    NorthEastCollider,
    SouthEastCollider,
    SouthCollider,
    SouthWestCollider,
    NorthWestCollider
}

public class TileManager : MonoBehaviour {

    private static TileManager instance = null;

    Dictionary<Tile, List<MonsterInstance>> monstersOnTile = new Dictionary<Tile, List<MonsterInstance>>();
    Dictionary<Tile, List<KeeperInstance>> keepersOnTile = new Dictionary<Tile, List<KeeperInstance>>();
    Dictionary<KeeperInstance, Tile> getTileFromKeeper = new Dictionary<KeeperInstance, Tile>();
    Tile prisonerTile;
    public PrisonerInstance prisoner;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            prisonerTile = GameObject.FindGameObjectWithTag("BeginTile").GetComponentInParent<Tile>();
            keepersOnTile.Add(prisonerTile, GameManager.Instance.AllKeepersList);
        }
        else if (instance != this)
        {
            Destroy(gameObject);

        }
        DontDestroyOnLoad(gameObject);
    }

    public static TileManager Instance
    {
        get
        {
            return instance;
        }
    }

    public void MoveKeeper(KeeperInstance keeper, Tile from, Direction direction)
    {
        if (from.Neighbors[(int)direction] == null)
            return;

        RemoveKeeperFromTile(from, keeper);
        AddKeeperOnTile(from.Neighbors[(int)direction], keeper);

        // Handle prisoner
        if (prisoner.KeeperFollowed != null && prisoner.KeeperFollowed == keeper)
            prisonerTile = from.Neighbors[(int)direction];
    }

    public void MoveMonster(MonsterInstance monster, Tile from, Direction direction)
    {
        RemoveMonsterFromTile(from, monster);
        AddMonsterOnTile(from.Neighbors[(int)direction], monster);        
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
        switch (moveDirection)
        {
            case Direction.North:
                tmp = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.SouthCollider).GetComponentsInChildren<Transform>();
                break;
            case Direction.North_East:
                tmp = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.SouthWestCollider).GetComponentsInChildren<Transform>();
                break;
            case Direction.North_West:
                tmp = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.SouthEastCollider).GetComponentsInChildren<Transform>();
                break;
            case Direction.South:
                tmp = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.NorthCollider).GetComponentsInChildren<Transform>();
                break;
            case Direction.South_East:
                tmp = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.NorthWestCollider).GetComponentsInChildren<Transform>();
                break;
            case Direction.South_West:
                tmp = destinationTile.transform.GetChild(0).GetChild((int)TilePrefabChildren.NorthEastCollider).GetComponentsInChildren<Transform>();
                break;
        }

        spawnPositions[0] = tmp[1];
        spawnPositions[1] = tmp[2];
        return spawnPositions;
    }

}
