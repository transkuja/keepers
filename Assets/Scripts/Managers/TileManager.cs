using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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
