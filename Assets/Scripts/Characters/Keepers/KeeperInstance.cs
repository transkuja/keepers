using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class KeeperInstance : MonoBehaviour, ITradable {

    [Header("Keeper Info")]
    [SerializeField]
    private Keeper keeper = null;
    private bool isSelected = false;

    public GameObject keeperInventoryPanel;

    private bool isStarving = false;
    private bool isMentalHealthLow = false;

    [SerializeField]
    private short currentHunger = 0;

    [SerializeField]
    private short currentMentalHealth;

    [SerializeField]
    private int currentHp;

    [SerializeField]
    private int currentMp;

    [SerializeField]
    private GameObject goSelectionAura;

    // Used only in menu. Handles selection in main menu.
    [SerializeField]
    private bool isSelectedInMenu = false;
    public MeshRenderer meshToHighlight;

    private bool isAlive = true;
    // Inventory
    private ItemContainer[] equipment;


    // Actions
    [Header("Actions")]
    [SerializeField]
    private short actionPoints = 3;

    private InteractionImplementer interactionImplementer;
    //  Escort
    public bool isEscortAvailable = true;
    // Moral
    public bool isAbleToImproveMoral = true;
    public int minMoralBuff = -10;
    public int maxMoralBuff = 20;

    // Mouvement
    [Header("Mouvement")]
    // Update variables
    NavMeshAgent agent;
    Vector3 v3AgentDirectionTemp;

    // Rotations
    float fLerpRotation = 0.666f;
    Quaternion quatTargetRotation;
    Quaternion quatPreviousRotation;
    bool bIsRotating = false;
    [SerializeField]
    float fRotateSpeed = 1.0f;


    public short CurrentHunger
    {
        get { return currentHunger; }
        set
        {
            currentHunger = value;
            if (currentHunger > keeper.MaxHunger)
            {
                currentHunger = keeper.MaxHunger;
                isStarving = true;
            }
            else if (currentHunger < 0)
            {
                currentHunger = 0;
                isStarving = false;
            }
            else
            {
                isStarving = false;
            }

        }
    }

    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            if (currentHp > keeper.MaxHp)
            {
                currentHp = keeper.MaxHp;
                isAlive = true;
            }
            else if (currentHp < 0)
            {
                currentHp = 0;
                
                isAlive = false;
                Die();
            }
            else
            {
                isAlive = true;
            }

        }
    }

    public int CurrentMp
    {
        get { return currentMp; }
        set
        {
            currentMp = value;
            if (currentMp > keeper.MaxMp)
            {
                currentMp = keeper.MaxMp;
            }
            else if (currentMp < 0)
            {
                currentMp = 0;
            }

        }
    }

    void Die()
    {
        Debug.Log("Blaeuurgh... *dead*");
        Tile currentTile = TileManager.Instance.GetTileFromKeeper[this];

        // Drop items
        ItemManager.AddItemOnTheGround(currentTile, transform, GetComponent<Inventory>().List_inventaire);

        // Remove reference from tiles
        TileManager.Instance.RemoveKilledKeeper(this);

        // Death operations
        GameManager.Instance.ShortcutPanel_NeedUpdate = true;
        GlowController.UnregisterObject(GetComponent<GlowObjectCmd>());
        gameObject.SetActive(false);

        GameManager.Instance.CheckGameState();
    }

    public short CurrentMentalHealth
    {
        get { return currentMentalHealth; }
        set
        {
            currentMentalHealth = value;
            if (currentMentalHealth < 0)
            {
                currentMentalHealth = 0;
                isMentalHealthLow = true;
            }
            else if (currentMentalHealth > keeper.MaxMentalHealth)
            {
                currentMentalHealth = keeper.MaxMentalHealth;
                isMentalHealthLow = false;
            }
            else
            {
                isMentalHealthLow = false;
            }
        }
    }

    public short ActionPoints
    {
        get
        {
            return actionPoints;
        }

        set
        {

            if (value < actionPoints) GameManager.Instance.Ui.DecreaseActionTextAnimation(actionPoints - value);
            actionPoints = value;
            GameManager.Instance.Ui.UpdateActionText();
            if (actionPoints > keeper.MaxActionPoints)
                actionPoints = keeper.MaxActionPoints;
            if (actionPoints < 0)
                actionPoints = 0;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }

        set
        {
            isAlive = value;
        }
    }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fRotateSpeed = 5.0f;
        isEscortAvailable = true;
        InteractionImplementer = new InteractionImplementer();
        if(GameManager.Instance.Ui == null)
        {
            InteractionImplementer.Add(new Interaction(Trade), "Trade", GameManager.Instance.MenuUi.spriteTrade);
            if (isAbleToImproveMoral) InteractionImplementer.Add(new Interaction(MoralBuff), "Moral", GameManager.Instance.MenuUi.spriteMoral);
        }
        else
        {
            InteractionImplementer.Add(new Interaction(Trade), "Trade", GameManager.Instance.Ui.spriteTrade);
            if (isAbleToImproveMoral) InteractionImplementer.Add(new Interaction(MoralBuff), "Moral", GameManager.Instance.Ui.spriteMoral);
        }

        currentHp = keeper.MaxHp;
        currentHunger = 0;
        currentMentalHealth = keeper.MaxMentalHealth;
        actionPoints = keeper.MaxActionPoints;
        currentMp = keeper.MaxMp;
        isAlive = true;

        equipment = new ItemContainer[3];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MonsterInstance>())
        {
            agent.SetDestination(transform.position);
            BattleHandler.StartBattleProcess(TileManager.Instance.GetTileFromKeeper[this]);
        }
    }


    private void Update()
    {
        GameObject goDestinationTemp = gameObject;
        for (int i = 0; i < keeper.GoListCharacterFollowing.Count; i++)
        {
            keeper.GoListCharacterFollowing[i].GetComponent<NavMeshAgent>().destination = goDestinationTemp.transform.position;
            goDestinationTemp = keeper.GoListCharacterFollowing[i];
        }

        if (bIsRotating)
        {
            Rotate();
        }
    }

    private void ToggleHighlightOnMesh(bool isSelected)
    {
        if (meshToHighlight != null)
        {
            if (isSelected)
            {
                meshToHighlight.material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
                meshToHighlight.material.SetColor("_OutlineColor", Color.blue);
            }
            else
            {
                meshToHighlight.material.shader = Shader.Find("Diffuse");
            }
        }
    }

    public KeeperInstance(KeeperInstance from)
    {
        keeper = from.keeper;
        goSelectionAura = from.goSelectionAura;

    }

    /* ------------------------------------------------------------------------------------ */
    /* ------------------------------------- Accessors ------------------------------------ */
    /* ------------------------------------------------------------------------------------ */

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }

        set
        {
            isSelected = value;
            GoSelectionAura.SetActive(value);
            if (agent != null)
                agent.avoidancePriority = value == true ? 80 : 50;
        }
    }



    public bool IsSelectedInMenu
    {
        get
        {
            return isSelectedInMenu;
        }

        set
        {
            isSelectedInMenu = value;
            ToggleHighlightOnMesh(isSelectedInMenu);
        }
    }

    public Keeper Keeper
    {
        get
        {
            return keeper;
        }

        set
        {
            keeper = value;
        }
    }

    public GameObject GoSelectionAura
    {
        get
        {
            return goSelectionAura;
        }

        set
        {
            goSelectionAura = value;
        }
    }

    public ItemContainer[] Equipment
    {
        get
        {
            return equipment;
        }

        set
        {
            equipment = value;
        }
    }

    public InteractionImplementer InteractionImplementer
    {
        get
        {
            return interactionImplementer;
        }

        set
        {
            interactionImplementer = value;
        }
    }

    public void TriggerRotation(Vector3 v3Direction)
    {
        agent.angularSpeed = 0.0f;

        quatPreviousRotation = transform.rotation;

        Vector3 v3PosTemp = transform.position;
        v3PosTemp.y = 0;
        v3Direction.y = 0.0f;

        quatTargetRotation.SetLookRotation(v3Direction - v3PosTemp);

        bIsRotating = true;

        agent.Stop();

        v3AgentDirectionTemp = v3Direction;

        fLerpRotation = 0.0f;
    }

    void Rotate()
    {
        if(fLerpRotation >= 1.0f)
        {
            transform.rotation = quatTargetRotation;
            bIsRotating = false;
            agent.Resume();
            fLerpRotation = 0.0f;

            agent.destination = v3AgentDirectionTemp;
            agent.angularSpeed = 100.0f;
        }
        else
        {
            fLerpRotation += fRotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(quatPreviousRotation, quatTargetRotation, fLerpRotation);
        }
    }


    public void Trade(int _i = 0)
    {
        keeperInventoryPanel.SetActive(true);
        GameManager.Instance.Ui.UpdateInventoryPanel(gameObject);
    }
    public void MoralBuff(int _i = 0)
    {
        if (GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints > 0)
        {
            GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints -= 1;
            short amountMoralBuff = (short)Random.Range(minMoralBuff, maxMoralBuff);
            GameManager.Instance.GoTarget.GetComponent<KeeperInstance>().CurrentMentalHealth += amountMoralBuff;
            GameManager.Instance.ShortcutPanel_NeedUpdate = true;
            GameManager.Instance.Ui.MoralBuffActionTextAnimation(amountMoralBuff);
        }
        else
        {
            GameManager.Instance.Ui.ZeroActionTextAnimation();
        }
    }
}
