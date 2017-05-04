using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour {

    enum ScaleState
    {
        unscale = -1,
        idle = 0,
        scale = 1,
    }

    public enum ChatMode
    {
        mute = -1,
        pickme,
        picked,
    }

    RectTransform trBox;
    Text txt;

    public Transform trTarget;    
    public List<string>[] tabEmotes;
    public bool bEnable = true;

    bool bIsShown;
    ScaleState state = ScaleState.idle;
    ChatMode mode = ChatMode.mute;
    float fTimer = 0;
    float fLerp = 0;

    // Parametres
    public float fShowLength = 3;
    public float fDelayMin = 1, fDelayMax = 2;
    public float fScaleSpeed = 10;

	// Use this for initialization

    public void Start()
    {
        trBox = GetComponent<RectTransform>();
        txt = GetComponentInChildren<Text>();
        
        trBox.transform.localScale = Vector3.zero;

        tabEmotes = new List<string>[2];

        tabEmotes[0] = new List<string>();
        tabEmotes[0].Add("Hey!");
        tabEmotes[0].Add("Pick me!");

        tabEmotes[1] = new List<string>();
        tabEmotes[1].Add("Let's go!");
        tabEmotes[1].Add("Yeah!");

        fTimer = Random.Range(fDelayMin, fDelayMax);
    }

	// Update is called once per frame
	void Update () {
        if (!bIsShown && mode != ChatMode.mute && bEnable)
        {
            if(fTimer <= 0)
            {
                txt.text = tabEmotes[(int)mode][Random.Range(0, tabEmotes[(int)mode].Count)];
                TriggerScale();
            }
            else
            {
                fTimer -= Time.unscaledDeltaTime;
            }

        }else
        {
            fTimer -= Time.unscaledDeltaTime;

            if(fTimer <= 0)
            {
                TriggerUnscale();
            }
        }

        if (bIsShown)
        {
            UpdatePosition();
        }

        if(state != ScaleState.idle)
        {
            UpdateScale();
        }
	}

    void UpdatePosition()
    {
        trBox.position = Camera.main.WorldToScreenPoint(trTarget.position);
    }

    public void TriggerScale()
    {
        fLerp = 0;
        fTimer = fShowLength;
        bIsShown = true;
        state = ScaleState.scale;
    }

    public void TriggerUnscale()
    {
        fLerp = 1;
        bIsShown = false;
        fTimer = Random.Range(fDelayMin, fDelayMax);
        state = ScaleState.unscale;
    }

    void UpdateScale()
    {
        fLerp += Time.deltaTime * (int)state * fScaleSpeed;

        trBox.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, fLerp);

        if(state == ScaleState.scale && fLerp >= 1)
        {
            state = ScaleState.idle;
        }
        else if (state == ScaleState.unscale && fLerp <= 0)
        {
            state = ScaleState.idle;
        }
    }

    public void SetMode(ChatMode _mode)
    {
        mode = _mode;
        switch (_mode)
        {
            case ChatMode.mute:
                bIsShown = false;
                trBox.transform.localScale = Vector3.zero;
                fLerp = 0;
                break;
            case ChatMode.pickme:
                break;
            case ChatMode.picked:
                break;
            default:
                break;
        }
    }

    public void SetEnable(bool status)
    {
        bEnable = status;
        if(status == false)
        {
            TriggerUnscale();
        }
    }
}
