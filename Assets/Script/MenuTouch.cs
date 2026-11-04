#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

public class MenuTouch : MonoBehaviour
{
    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            if (t.phase != TouchPhase.Began) continue;

            Button[] buttons = FindObjectsByType<Button>();
            foreach (Button b in buttons)
            {
                RectTransform r = b.GetComponent<RectTransform>();
                if (b.interactable && RectTransformUtility.RectangleContainsScreenPoint(r, t.position))
                {
                    b.onClick.Invoke();
                    return;
                }
            }
        }
    }
}
#endif