using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBattleCharactersPanelHandler : MonoBehaviour {

    Tile activeTile;

    [SerializeField]
    GameObject imagePrefab;

    bool isPrisonerOnTile = false;

    public Tile ActiveTile
    {
        get
        {
            return activeTile;
        }

        set
        {
            activeTile = value;
        }
    }

    private void Awake()
    {
        int j = 0;
        // TODO load keepers on tile in UI
        foreach (KeeperInstance ki in TileManager.Instance.KeepersOnTile[activeTile])
        {
            GameObject kiImage = Instantiate(imagePrefab, transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).GetChild(j));
            kiImage.GetComponent<UIKeeperInstance>().keeperInstance = ki;
            kiImage.transform.localScale = Vector3.one;
            kiImage.transform.localPosition = Vector3.zero;
            kiImage.GetComponent<Image>().sprite = ki.Keeper.AssociatedSprite;
            j++;
        }

        if (TileManager.Instance.PrisonerTile != null && TileManager.Instance.PrisonerTile == activeTile)
        {
            isPrisonerOnTile = true;
        }
        // TODO load monsters on tile in UI (sprites only)
    }

    public void CloseSelectBattleCharactersPanel()
    {
        List<KeeperInstance> selected = new List<KeeperInstance>();

        if (transform.GetChild((int)SelectBattleCharactersScreenChildren.FirstCharacter).GetChild(0) != null)
            selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.FirstCharacter).GetComponentInChildren<KeeperInstance>());

        if (transform.GetChild((int)SelectBattleCharactersScreenChildren.SecondCharacter).GetChild(0) != null)
            selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.SecondCharacter).GetComponentInChildren<KeeperInstance>());

        if (isPrisonerOnTile)
        {
        }
        else
        {
            if (transform.GetChild((int)SelectBattleCharactersScreenChildren.ThirdCharacter).GetChild(0) != null)
                selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.ThirdCharacter).GetComponentInChildren<KeeperInstance>());
        }

        BattleHandler.LaunchBattle(activeTile, selected);
        activeTile = null;
        gameObject.SetActive(false);
    }

}

public enum SelectBattleCharactersScreenChildren
{
    Header,
    FirstCharacter,
    SecondCharacter,
    ThirdCharacter,
    Go,
    CharactersOnTile
}