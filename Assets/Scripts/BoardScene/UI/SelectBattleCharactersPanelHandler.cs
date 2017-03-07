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

        foreach (KeeperInstance ki in TileManager.Instance.KeepersOnTile[activeTile])
        {
            GameObject kiImage = Instantiate(imagePrefab, transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).GetChild(j));
            kiImage.AddComponent<UIKeeperInstance>();
            kiImage.GetComponent<UIKeeperInstance>().keeperInstance = ki;
            kiImage.AddComponent<DragHandler>();
            kiImage.AddComponent<CanvasGroup>();

            kiImage.transform.localScale = Vector3.one;
            kiImage.transform.localPosition = Vector3.zero;
            kiImage.GetComponent<Image>().sprite = ki.Keeper.AssociatedSprite;
            
            j++;
        }

        if (TileManager.Instance.PrisonerTile != null && TileManager.Instance.PrisonerTile == activeTile)
        {
            isPrisonerOnTile = true;
            GameObject kiImage = Instantiate(imagePrefab, transform.GetChild((int)SelectBattleCharactersScreenChildren.ThirdCharacter));
            kiImage.transform.localScale = Vector3.one;
            kiImage.transform.localPosition = Vector3.zero;
            kiImage.GetComponent<Image>().sprite = GameManager.Instance.PrisonerInstance.Prisoner.AssociatedSprite;
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
            BattleHandler.isPrisonerOnTile = true;
        }
        else
        {
            if (transform.GetChild((int)SelectBattleCharactersScreenChildren.ThirdCharacter).GetChild(0) != null)
                selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.ThirdCharacter).GetComponentInChildren<KeeperInstance>());
        }

        if (selected.Count == 0)
        {
            return;
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