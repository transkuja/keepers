using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;
using Behaviour;

public class GlowObjectCmd : MonoBehaviour
{
	public Color GlowColor;
	public float LerpFactor = 10;

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
        if (GetComponent<Monster>() == null)
		    GlowController.RegisterObject(this);
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
