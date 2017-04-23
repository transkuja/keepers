using UnityEngine;
using UnityEngine.UI;

public class MouseFollower : MonoBehaviour {

    [SerializeField]
    float offsetX = 10.0f;
    [SerializeField]
    float offsetY = 0.0f;
    [SerializeField]
    Color friendlyColor;
    [SerializeField]
    Color foeColor;

    bool isTargetExpectedAFriend;

    public bool IsTargetExpectedAFriend
    {
        get
        {
            return isTargetExpectedAFriend;
        }

        set
        {
            isTargetExpectedAFriend = value;
        }
    }

    public void ExpectedTarget(TargetType _targetType)
    {
        if (_targetType == TargetType.Foe)
            isTargetExpectedAFriend = false;
        else
            isTargetExpectedAFriend = true;

        if (isTargetExpectedAFriend)
        {
            GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteSupportSymbol;
            GetComponent<Image>().color = friendlyColor;
        }
        else
        {
            GetComponent<Image>().sprite = GameManager.Instance.SpriteUtils.spriteAttackSymbol;
            GetComponent<Image>().color = foeColor;
        }
    }

    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(offsetX, offsetY);
    }
}
