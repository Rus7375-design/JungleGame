#if UNITY_EDITOR
using UnityEngine;

public class RemoteTouch : MonoBehaviour
{
    public Player2D player;
    public RectTransform stick;        
    public RectTransform knob;       
    public RectTransform jumpButton;  
    public RectTransform fireButton;   
    public float deadZone = 0.25f;   

    int stickId = -1;
    int fireId = -1;

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            bool ended = t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled;

            if (t.phase == TouchPhase.Began)
            {
                if (stickId == -1 && Hit(stick, t.position)) stickId = t.fingerId;
                else if (Hit(jumpButton, t.position)) player.Jump();
                else if (fireId == -1 && Hit(fireButton, t.position)) fireId = t.fingerId;
            }

            if (t.fingerId == stickId)
            {
                if (ended) { StopStick(); continue; }
                DriveStick(t.position);
            }

            if (t.fingerId == fireId && ended) fireId = -1;
        }

        if (fireId != -1) player.Fire();  
    }

    void DriveStick(Vector2 touchPos)
    {
        Vector2 center = stick.TransformPoint(stick.rect.center);
        float half = stick.rect.width * stick.lossyScale.x / 2f;
        float dx = Mathf.Clamp((touchPos.x - center.x) / half, -1f, 1f);

        float value = Mathf.Abs(dx) < deadZone ? 0f : Mathf.Sign(dx);
        player.SetMoveX(value);

        if (knob != null)
            knob.position = new Vector3(center.x + dx * half * 0.6f, center.y, 0f);
    }

    void StopStick()
    {
        stickId = -1;
        player.SetMoveX(0f);
        if (knob != null) knob.anchoredPosition = Vector2.zero;
    }

    bool Hit(RectTransform r, Vector2 p)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(r, p);
    }
}
#endif