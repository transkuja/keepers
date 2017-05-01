using UnityEngine;
using System.Collections.Generic;
using Behaviour;

public class GlowObjectCmd : MonoBehaviour
{
	public Color GlowColor;
	public float LerpFactor = 10;

    float timerDoubleClick;
    [SerializeField]
    private float doubleClickCoolDown = 1.2f;
    private int nbrOfClicks = 0;
    private bool startDCTimer = false;

    public Renderer[] Renderers
	{
		get;
		private set;
	}

	public Color CurrentColor
	{
		get { return _currentColor; }
	}

	private Color _currentColor;
	private Color _targetColor;

	void Start()
	{
		Renderers = GetComponentsInChildren<Renderer>();
        if (GetComponent<Behaviour.Monster>() == null)
		    GlowController.RegisterObject(this);

        timerDoubleClick = doubleClickCoolDown;
        nbrOfClicks = 0;
        startDCTimer = false;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.CurrentState == GameState.Normal)
        {
            if (GetComponent<Keeper>() != null)
            {
                nbrOfClicks++;
                Keeper clickedKeeper = GetComponent<Keeper>();
                GameManager.Instance.ClearListKeeperSelected();
                GameManager.Instance.AddKeeperToSelectedList(clickedKeeper.getPawnInstance);
                GameManager.Instance.Ui.HideInventoryPanels();
                clickedKeeper.IsSelected = true;

                if (nbrOfClicks == 1)
                {
                    timerDoubleClick = doubleClickCoolDown;
                    startDCTimer = true;
                }

                if (timerDoubleClick > 0.0f)
                {
                    if (nbrOfClicks >= 2)
                    {
                        Camera.main.GetComponent<CameraManager>().UpdateCameraPosition(GetComponent<PawnInstance>());
                        timerDoubleClick = 0.0f;
                        nbrOfClicks = 0;
                        startDCTimer = false;
                    }
                }
                else
                {
                    startDCTimer = false;
                    nbrOfClicks = 0;
                }
            }
        }
    }

    private void UpdateDoubleCick()
    {
        if (timerDoubleClick > 0.0f && nbrOfClicks >= 1)
        {
            timerDoubleClick -= Time.deltaTime;
        }
    }

    private void OnMouseEnter()
	{
        if (GetComponent<DieFeedback>() == null && GetComponent<RightMouseClickExpected>() == null)
        {
            _targetColor = GlowColor;
            enabled = true;
        }     
	}

	private void OnMouseExit()
	{
        if (GetComponent<DieFeedback>() == null && GetComponent<RightMouseClickExpected>() == null)
        {
            _targetColor = Color.black;
            enabled = true;
        }
	}

    public void UpdateColor(bool _enable)
    {
        if (_enable)
            _targetColor = GlowColor;
        else
            _targetColor = Color.black;
    }

    /// <summary>
    /// Update color, disable self if we reach our target color.
    /// </summary>
    private void Update()
	{
		_currentColor = Color.Lerp(_currentColor, _targetColor, Time.deltaTime * LerpFactor);

		if (_currentColor.Equals(_targetColor))
		{
			enabled = false;
		}

        if (startDCTimer)
            UpdateDoubleCick();
	}

    private void OnDestroy()
    {
        GlowController.UnregisterObject(this);
    }

    //private void OnDisable()
    //{
    //    GlowController.UnregisterObject(this);
    //}
}
