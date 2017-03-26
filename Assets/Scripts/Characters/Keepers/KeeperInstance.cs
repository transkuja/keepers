using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class KeeperInstance : MonoBehaviour, ITradable {

    [Header("Keeper Info")]
    [SerializeField]
    private Keeper keeper = null;
    private bool isSelected = false;

    public GameObject keeperInventoryPanel;

    private bool isStarving = false;
    private bool isMentalHealthLow = false;

    [SerializeField]
    private short currentHunger;

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
    Animator anim;
    Vector3 v3AgentDirectionTemp;

    // Rotations
    float fLerpRotation = 0.666f;
    Quaternion quatTargetRotation;
    Quaternion quatPreviousRotation;
    bool bIsRotating = false;
    [SerializeField]
    float fRotateSpeed = 1.0f;

    // Aggro
    bool isTargetableByMonster = true;

    // Movement between tile
    // Prevents action poping when arriving on a tile
    Direction arrivingTrigger = Direction.None;
    bool isMovingBetweenTiles = false;
    float lerpMoveParam = 0.0f;
    Vector3 lerpStartPosition;
    Vector3 lerpEndPosition;
    Quaternion lerpStartRotation;
    Quaternion lerpEndRotation;

    // TODO handle buffs better
    public bool isLowMentalHealthBuffApplied = false;

    public short CurrentHunger
    {
        get { return currentHunger; }
        set
        {
            currentHunger = value;
            if (currentHunger < 0)
            {
                currentHunger = 0;
                IsStarving = true;
            }
            else if (currentHunger > keeper.MaxHunger)
            {
                currentHunger = keeper.MaxHunger;
                IsStarving = false;
            }
            else
            {
                IsStarving = false;
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
            else if (currentHp <= 0)
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
        anim.SetTrigger("triggerDeath");

        // Try to fix glow bug
        Destroy(GetComponent<GlowObjectCmd>());

        GameManager.Instance.Ui.HideSelectedKeeperPanel();
        GameManager.Instance.CheckGameState();

        // Deactivate pawn
        DeactivatePawn();
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
                IsMentalHealthLow = true;
            }
            else if (currentMentalHealth > keeper.MaxMentalHealth)
            {
                currentMentalHealth = keeper.MaxMentalHealth;
                IsMentalHealthLow = false;
            }
            else
            {
                IsMentalHealthLow = false;
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
            GameManager.Instance.ShortcutPanel_NeedUpdate = true;   
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
        anim = GetComponentInChildren<Animator>();
        fRotateSpeed = 5.0f;
        isEscortAvailable = true;
        InteractionImplementer = new InteractionImplementer();
        if(GameManager.Instance.Ui == null)
        {
            InteractionImplementer.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.MenuUi.spriteTrade);
            if (isAbleToImproveMoral) InteractionImplementer.Add(new Interaction(MoralBuff), 1, "Moral", GameManager.Instance.MenuUi.spriteMoral);
        }
        else
        {
            InteractionImplementer.Add(new Interaction(Trade), 0, "Trade", GameManager.Instance.Ui.spriteTrade);
            if (isAbleToImproveMoral) InteractionImplementer.Add(new Interaction(MoralBuff), 1, "Moral", GameManager.Instance.Ui.spriteMoral);
        }

        currentHp = keeper.MaxHp;
        currentHunger = keeper.MaxHunger;
        currentMentalHealth = keeper.MaxMentalHealth;
        actionPoints = keeper.MaxActionPoints;
        currentMp = keeper.MaxMp;
        isAlive = true;

        equipment = new ItemContainer[3];
    }

    private void Update()
    {
        GameObject goDestinationTemp = gameObject;
        for (int i = 0; i < keeper.GoListCharacterFollowing.Count; i++)
        {
            if (keeper.GoListCharacterFollowing[i].GetComponent<PrisonerInstance>() != null
                && !keeper.GoListCharacterFollowing[i].GetComponent<PrisonerInstance>().IsMovingBetweenTiles)
            {
                keeper.GoListCharacterFollowing[i].GetComponentInParent<NavMeshAgent>().destination = goDestinationTemp.transform.position;
                goDestinationTemp = keeper.GoListCharacterFollowing[i];
            }
        }

        if (bIsRotating)
        {
            Rotate();
        }

        if (IsMovingBetweenTiles)
        {
            lerpMoveParam += Time.deltaTime;
            if (lerpMoveParam >= 1.0f)
            {
                IsMovingBetweenTiles = false;
            }
            transform.position = Vector3.Lerp(lerpStartPosition, lerpEndPosition, Mathf.Clamp(lerpMoveParam,0,1));
            transform.rotation = Quaternion.Lerp(lerpStartRotation, lerpEndRotation, Mathf.Clamp(lerpMoveParam, 0, 1));
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

    public bool IsTargetableByMonster
    {
        get
        {
            return isTargetableByMonster;
        }

        set
        {
            isTargetableByMonster = value;
            foreach (GameObject follower in keeper.GoListCharacterFollowing)
            {
                if (follower.GetComponent<PrisonerInstance>() != null)
                    follower.GetComponent<PrisonerInstance>().IsTargetableByMonster = value;
            }
        }
    }

    public Direction ArrivingTrigger
    {
        get
        {
            return arrivingTrigger;
        }

        set
        {
            arrivingTrigger = value;
        }
    }

    public bool IsMovingBetweenTiles
    {
        get
        {
            return isMovingBetweenTiles;
        }

        set
        {
            isMovingBetweenTiles = value;
            agent.enabled = !value;
        }
    }

    public bool IsStarving
    {
        get
        {
            return isStarving;
        }

        set
        {
            isStarving = value;
        }
    }

    public bool IsMentalHealthLow
    {
        get
        {
            return isMentalHealthLow;
        }

        set
        {
            isMentalHealthLow = value;
        }
    }

    public void TriggerRotation(Vector3 v3Direction)
    {
        if (agent.enabled == false)
        {
            Debug.Log("Agent is not active!");
            return;
        }
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
        if (GameManager.Instance.ListOfSelectedKeepers.Count > 0)
        {
            int costAction = interactionImplementer.Get("Moral").costAction;
            if (GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints >= costAction)
            {
                GameManager.Instance.ListOfSelectedKeepers[0].ActionPoints -= (short)costAction;
                short amountMoralBuff = (short)Random.Range(minMoralBuff, maxMoralBuff);
                GameManager.Instance.GoTarget.GetComponentInParent<KeeperInstance>().CurrentMentalHealth += amountMoralBuff;
                GameManager.Instance.ShortcutPanel_NeedUpdate = true;
                GameManager.Instance.SelectedKeeperNeedUpdate = true;
                GameManager.Instance.Ui.MoralBuffActionTextAnimation(amountMoralBuff);
            }
            else
            {
                GameManager.Instance.Ui.ZeroActionTextAnimation();
            }
        }
    }

    private void DeactivatePawn()
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = false;
        enabled = false;

        // Deactivate gameobject after a few seconds
        StartCoroutine(DeactivateGameObject());
    }

    IEnumerator DeactivateGameObject()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.SetActive(false);
        foreach (Collider c in GetComponentsInChildren<Collider>())
            c.enabled = true;
        enabled = true;
    }

    public void StartBetweenTilesAnimation(Vector3 newPosition)
    {
        lerpMoveParam = 0.0f;
        lerpStartPosition = transform.position;
        lerpEndPosition = newPosition;
        Vector3 direction = newPosition - transform.position;
        lerpStartRotation = Quaternion.LookRotation(transform.forward);
        lerpEndRotation = Quaternion.LookRotation(direction);

        anim.SetTrigger("moveBetweenTiles");

        IsMovingBetweenTiles = true;
    }
}
