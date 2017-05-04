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
        if (_targetType == TargetType.FoeSingle)
            isTargetExpectedAFriend = false;
        else
            isTargetExpectedAFriend = true;

        if (isTargetExpectedAFriend)
        {
            Cursor.SetCursor(GameManager.Instance.Texture2DUtils.buffCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(GameManager.Instance.Texture2DUtils.attackCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    void Update()
    {
        transform.position = Input.mousePosition + new Vector3(offsetX, offsetY);
    }
}
