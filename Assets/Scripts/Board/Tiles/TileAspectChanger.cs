using UnityEngine;
using UnityEngine.SceneManagement;

public class TileAspectChanger : MonoBehaviour {
    
    [SerializeField]
    GameObject greyedPrefab;
    GreyedTileModel tileModel = null;
    Tile tile;

    public void SetAsGreyedModel()
    {
        
        tileModel.gameObject.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(false);
        if (!SceneManager.GetActiveScene().name.Contains("Tuto"))
            transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
    }

    public void SetAsBaseModel()
    {
        tileModel.gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        if (!SceneManager.GetActiveScene().name.Contains("Tuto"))
            transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    void UpdateModel()
    {
        if (tile.State == TileState.Greyed)
            SetAsGreyedModel();
        else if (tile.State == TileState.Discovered)
            SetAsBaseModel();
    }

    void Start () {
        tile = GetComponentInParent<Tile>();
        tileModel = GetComponentInChildren<GreyedTileModel>();
        if (tileModel == null)
        {
            GameObject go = Instantiate(greyedPrefab, transform);
            go.transform.SetAsLastSibling();
            go.transform.position = transform.position;
            if (GetComponentInChildren<GreyedTileModel>() == null)
                go.AddComponent<GreyedTileModel>();
            tileModel = go.GetComponent<GreyedTileModel>();
            tileModel.gameObject.SetActive(false);
        }
        

        UpdateModel();

        tile.StateChanged += UpdateModel;
	}

}
