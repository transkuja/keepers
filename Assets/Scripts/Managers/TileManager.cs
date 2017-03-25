﻿using System.Collections;
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

    Dictionary<Tile, List<MonsterInstance>> monstersOnTile = new Dictionary<Tile, List<MonsterInstance>>();
    Dictionary<Tile, List<KeeperInstance>> keepersOnTile = new Dictionary<Tile, List<KeeperInstance>>();
    Dictionary<KeeperInstance, Tile> getTileFromKeeper = new Dictionary<KeeperInstance, Tile>();
    Tile prisonerTile;
    GameObject tiles;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            InitializeState();
        }
        else if (instance != this)
        {
            instance.ResetTileManager();
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

    /// <summary>
    /// Move keeper and its followers from a tile to another.
    /// </summary>
    /// <param name="keeper">The keeper to move</param>
    /// <param name="from">The origin tile</param>
    /// <param name="direction">The direction of the movement from the origin tile</param>
    public void MoveKeeper(KeeperInstance keeper, Tile from, Direction direction, int costAction)
    {
        Tile destination = from.Neighbors[(int)direction];
        if (destination == null)
            return;

        RemoveKeeperFromTile(from, keeper);
        AddKeeperOnTile(destination, keeper);
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
                RemoveKeeperFromTile(from, goCurrentCharacter.GetComponent<KeeperInstance>());
                AddKeeperOnTile(destination, goCurrentCharacter.GetComponent<KeeperInstance>());
                goCurrentCharacter.GetComponent<KeeperInstance>().ActionPoints = 0;
                goCurrentCharacter.GetComponent<KeeperInstance>().StartBetweenTilesAnimation(spawnPoints[i + 1 % spawnPoints.Length].position);
            }

        }
    }

    /// <summary>
    /// Move a monster to another tile.
    /// </summary>
    /// <param name="monster">The monster to move</param>
    /// <param name="from">The origin tile of the monster</param>
    /// <param name="direction">The direction of the movement from the origin tile</param>
    public void MoveMonster(MonsterInstance monster, Tile from, Direction direction)
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
        MonsterInstance[] removeIndex = new MonsterInstance[monstersOnTile[tile].Count];
        short nbrOfElementsToRemove = 0;

        List<ItemContainer> lootList = new List<ItemContainer>();
        Transform lastMonsterPosition = null;
        foreach (MonsterInstance mi in monstersOnTile[tile])
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
            ItemManager.AddItemOnTheGround(tile, lastMonsterPosition, lootList);
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

    /// <summary>
    /// Remove a killed keeper from play
    /// </summary>
    /// <param name="keeper">The keeper to remove</param>
    public void RemoveKilledKeeper(KeeperInstance keeper)
    {
        RemoveKeeperFromTile(getTileFromKeeper[keeper], keeper);
        getTileFromKeeper.Remove(keeper);
    }
    
    /// <summary>
    /// Add a monster on a tile
    /// </summary>
    /// <param name="tile">A Tile</param>
    /// <param name="monster">A new MonsterInstance</param>
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


    /// <summary>
    /// Update tile references for a keeper.
    /// </summary>
    /// <param name="tile">New tile</param>
    /// <param name="keeper">KeeperInstance</param>
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

    /// <summary>
    /// Remove monster from a tile after a movement.
    /// </summary>
    /// <param name="tile">Old tile</param>
    /// <param name="monster">MonsterInstance</param>
    private void RemoveMonsterFromTile(Tile tile, MonsterInstance monster)
    {
        monstersOnTile[tile].Remove(monster);
    }

    /// <summary>
    /// Remove keeper from a tile after a movement.
    /// </summary>
    /// <param name="tile">Old tile</param>
    /// <param name="keeper">KeeperInstance</param>
    private void RemoveKeeperFromTile(Tile tile, KeeperInstance keeper)
    {
        keepersOnTile[tile].Remove(keeper);
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

    public void ResetTileManager()
    {
        instance.monstersOnTile.Clear();
        instance.keepersOnTile.Clear();
        instance.getTileFromKeeper.Clear();

        // Re-initialize
        instance.InitializeState();
    }

    private void InitializeState()
    {
        GameObject beginTile = GameObject.FindGameObjectWithTag("BeginTile");
        if (beginTile == null)
        {
            Debug.Log("No tag BeginTile on the first tile has been set.");
            return;
        }
        instance.prisonerTile = beginTile.GetComponentInParent<Tile>();
        foreach (KeeperInstance ki in GameManager.Instance.AllKeepersList)
        {
            instance.AddKeeperOnTile(instance.prisonerTile, ki);
        }
        instance.InitializeMonsters();
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
        foreach (MonsterInstance mi in instance.tiles.GetComponentsInChildren<MonsterInstance>())
        {
            instance.AddMonsterOnTile(mi.GetComponentInParent<Tile>(), mi);
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
