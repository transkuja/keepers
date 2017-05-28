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

    private void OnEnable()
    {
        int j = 0;
        transform.GetChild((int)SelectBattleCharactersScreenChildren.Header).GetComponentInChildren<Text>().text = Translater.BattleCharacterSelection();

        foreach (PawnInstance ki in TileManager.Instance.KeepersOnTile[activeTile])
        {
            if (ki.Data.Behaviours[(int)BehavioursEnum.Archer] == true)
                continue;

            GameObject kiImage = Instantiate(imagePrefab, transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).GetChild(j));
            kiImage.AddComponent<UIKeeperInstance>();
            kiImage.GetComponent<UIKeeperInstance>().keeperInstance = ki;
            kiImage.AddComponent<DragHandler>();
            kiImage.AddComponent<CanvasGroup>();

            kiImage.transform.localScale = Vector3.one;
            kiImage.transform.localPosition = Vector3.zero;
            kiImage.GetComponent<Image>().sprite = ki.Data.AssociatedSprite;
            
            j++;
        }

        PawnInstance archer = GameManager.Instance.ArcherInstance;
        if (archer != null && archer.CurrentTile == activeTile)
        {
            GameObject kiImage = Instantiate(imagePrefab, transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).GetChild(j));
            kiImage.AddComponent<UIKeeperInstance>();
            kiImage.GetComponent<UIKeeperInstance>().keeperInstance = archer;
            kiImage.AddComponent<DragHandler>();
            kiImage.AddComponent<CanvasGroup>();

            kiImage.transform.localScale = Vector3.one;
            kiImage.transform.localPosition = Vector3.zero;
            kiImage.GetComponent<Image>().sprite = archer.Data.AssociatedSprite;
        }
        
        if (TileManager.Instance.PrisonerTile != null && TileManager.Instance.PrisonerTile == activeTile)
        {
            isPrisonerOnTile = true;
            GameObject kiImage = Instantiate(imagePrefab, transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(2));
            kiImage.transform.localScale = Vector3.one;
            kiImage.transform.localPosition = Vector3.zero;
            kiImage.GetComponent<Image>().sprite = GameManager.Instance.PrisonerInstance.Data.AssociatedSprite;
        }
        else
        {
            isPrisonerOnTile = false;
        }

        // TODO load monsters on tile in UI (sprites only)
    }

    public void CloseSelectBattleCharactersPanel()
    {
        List<PawnInstance> selected = new List<PawnInstance>();

        if (transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(0).childCount > 0)
            selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetComponentInChildren<UIKeeperInstance>().keeperInstance);

        if (transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(1).childCount > 0)
            selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(1).GetComponentInChildren<UIKeeperInstance>().keeperInstance);

        if (!isPrisonerOnTile)
        {
            if (transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(2).childCount > 0)
                selected.Add(transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(2).GetComponentInChildren<UIKeeperInstance>().keeperInstance);
        }

        BattleHandler.isPrisonerOnTile = isPrisonerOnTile;

        if (selected.Count == 0)
        {
            Debug.Log("At least one keeper must be selected");
            return;
        }
        gameObject.SetActive(false);

        BattleHandler.LaunchBattle(activeTile, selected);
        activeTile = null;

        for (int i = 0; i < transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).childCount; i++)
        {
            if (transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(i).childCount > 0)
                Destroy(transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(i).GetChild(0).gameObject);
        }

        for (int i = 0; i < transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).childCount; i++)
        {
            if (transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).GetChild(i).childCount > 0)
                Destroy(transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).GetChild(i).GetChild(0).gameObject);
        }
    }

    public int FindFreeSlotInSelection()
    {
        for (int i = 0; i < transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).childCount; i++)
            if (transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersSelected).GetChild(i).childCount == 0)
                return i;
        
        return -1;
    }

    public int FindFreeSlotInCharactersOnTile()
    {
        for (int i = 0; i < transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).childCount; i++)
            if (transform.GetChild((int)SelectBattleCharactersScreenChildren.CharactersOnTile).GetChild(i).childCount == 0)
                return i;

        return -1;
    }

}

public enum SelectBattleCharactersScreenChildren
{
    Header,
    CharactersSelected,
    Go,
    CharactersOnTile
}