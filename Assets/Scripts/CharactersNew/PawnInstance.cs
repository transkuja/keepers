using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnInstance : MonoBehaviour {

    private Color red;
    private Color green;

    public class AscendingFeedback
    {
        public Sprite sprite;
        public string txt;
        public Color txtColor;
        
        public AscendingFeedback(Sprite _sprite, string _txt, Color _color)
        {
            sprite = _sprite;
            txt = _txt;
            txtColor = _color;
        }

        public AscendingFeedback(string _txt, Color _color)
        {
            txt = _txt;
            txtColor = _color;
        }
    }

    public void Start()
    {

        red = new Color32(0xBE, 0x18, 0x18, 0xFF);
        green = new Color32(0x1F, 0xB8, 0x66, 0xFF);

    }

    List<AscendingFeedback> feedBackQueue = new List<AscendingFeedback>();
    [SerializeField] GameObject goPanelAscendingFeedback;
    bool bIsInFeedback = false;

    [SerializeField]
    PawnData data;

    // Need to be initialized in charactersInitializer and changed in moveCharacter
    Tile currentTile;

    #region Accessors
    public PawnData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }

    [System.Obsolete("Use interaction component instead")]
    public InteractionImplementer Interactions
    {
        get
        {
            return GetComponent<Interactable>().Interactions;
        }

        set
        {
            GetComponent<Interactable>().Interactions = value;
        }
    }

    public Tile CurrentTile
    {
        get
        {
            return currentTile;
        }

        set
        {
            //Maybe shouldn't be there
            PloufPloufEmissionAdapter ppea = GetComponentInChildren<PloufPloufEmissionAdapter>();
            if (ppea != null && value.Type == TileType.Beach)
            {
                ppea.play = true;
            }
            else
            {
                if(ppea != null)
                {
                    ppea.play = false;
                }
            }
            

            currentTile = value;
            if (GetComponent<Behaviour.Prisoner>() != null)
                TileManager.Instance.PrisonerTile = value;
            if(EventManager.OnPawnMove != null)
            {
                EventManager.OnPawnMove(this, value);
            }

        }
    }
    #endregion

    #region Ascending feedback functions
    public void AddFeedBackToQueue(Sprite sprite, int amount)
    {
        // Used in tuto, allow to show the ascending feedback only when necessary
        if (!goPanelAscendingFeedback.transform.parent.gameObject.activeInHierarchy)
            return;

        string str = (amount < 0) ? "- ": "+ ";
        str += Mathf.Abs(amount);

        Color c = (amount < 0) ? Color.red : Color.green;

        feedBackQueue.Add(new AscendingFeedback(sprite, str, c));

        if (!bIsInFeedback)
        {
            StartAscendingFeedback(feedBackQueue[0]);
        }
    }

    public void AddFeedBackToQueue(int amount)
    {
        // Used in tuto, allow to show the ascending feedback only when necessary
        if (!goPanelAscendingFeedback.transform.parent.gameObject.activeInHierarchy)
            return;

        string str = (amount < 0) ? "- " : "+ ";
        str += Mathf.Abs(amount);

        Color c = (amount < 0) ? red : green;

        feedBackQueue.Add(new AscendingFeedback(str, c));

        if (!bIsInFeedback)
        {
            StartAscendingFeedback(feedBackQueue[0]);
        }
    }

    void StartAscendingFeedback(AscendingFeedback af)
    {
        Vector3 ascFeedbackOffset = Vector3.down * 0.3f;
        Vector3 txtWithImgPosition = Vector3.right * 0.1f;

        goPanelAscendingFeedback.gameObject.SetActive(true);
        goPanelAscendingFeedback.transform.localPosition = Vector3.zero + ascFeedbackOffset;

        if (af.sprite == null)
        {
            goPanelAscendingFeedback.transform.GetChild(0).gameObject.SetActive(false);
            goPanelAscendingFeedback.transform.GetChild(1).gameObject.SetActive(false);
            goPanelAscendingFeedback.transform.GetChild(2).localPosition = Vector3.zero;
        }
        else
        {
            goPanelAscendingFeedback.transform.GetChild(0).gameObject.SetActive(true);
            goPanelAscendingFeedback.transform.GetChild(1).gameObject.SetActive(true);
            goPanelAscendingFeedback.transform.GetChild(2).localPosition = txtWithImgPosition;
        }

        goPanelAscendingFeedback.transform.GetChild(1).GetComponent<Image>().sprite = af.sprite;
        goPanelAscendingFeedback.transform.GetChild(1).GetComponent<Image>().color = af.txtColor;
        goPanelAscendingFeedback.GetComponentInChildren<Text>().color = af.txtColor;
        goPanelAscendingFeedback.GetComponentInChildren<Text>().text = af.txt;

        goPanelAscendingFeedback.GetComponentInParent<WorldspaceCanvasCameraAdapter>().RecalculateActionCanvas(Camera.main);

        StartCoroutine(ProcessAscendingFeedback());
        bIsInFeedback = true;
    }

    private IEnumerator ProcessAscendingFeedback()
    {
        for (float f = 3.0f; f >= 0; f -= 0.1f)
        {
            Vector3 decal = new Vector3(0.0f, f, 0.0f) * 0.01f;
            goPanelAscendingFeedback.transform.localPosition += decal;
            yield return null;
        }

        feedBackQueue.Remove(feedBackQueue[0]);

        if(feedBackQueue.Count > 0)
        {
            StartAscendingFeedback(feedBackQueue[0]);
        }
        else
        {
            bIsInFeedback = false;
            goPanelAscendingFeedback.gameObject.SetActive(false);
        }

    }
    #endregion
}
