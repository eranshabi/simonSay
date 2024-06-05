using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameplayButtonsContainer : MonoBehaviour, IPointerMoveHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    public event Action<int> onPointerMoveCallback;
    public event Action<int> onPointerClickCallback;
    public event Action<int> onPointerDownCallback;
    public event Action<int> onPointerUpCallback;
    public int buttonsCount { get; set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        int hoveredButtonIndex = GetPointerEventButtonIndex(eventData);
        onPointerMoveCallback.Invoke(hoveredButtonIndex);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        int downedButtonIndex = GetPointerEventButtonIndex(eventData);
        onPointerDownCallback.Invoke(downedButtonIndex);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        int pointerButtonIndex = GetPointerEventButtonIndex(eventData);
        onPointerUpCallback.Invoke(pointerButtonIndex);
    }

    // I could have used buttons for all these events, but it was too limited to my desired generic implementation (progrematically dividing a circle sprite)
    public void OnPointerClick(PointerEventData eventData)
    {
        int clickedButtonIndex = GetPointerEventButtonIndex(eventData);
        onPointerClickCallback.Invoke(clickedButtonIndex);
    }

    // This is a function that understand on which button the pointer is hovering, based on circle calculations
    // It is generic and works with different number of buttons
    private int GetPointerEventButtonIndex(PointerEventData eventData) {
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localPosition);

        Vector3 direction = localPosition - new Vector2(0, 0);
        float angleRadians = Mathf.Atan2(direction.y, direction.x); // Get angle between the pointer and the center of the circle
        float angleDegrees = Mathf.Rad2Deg * angleRadians; // Convert angle to degrees

        angleDegrees = (angleDegrees - 90f + (360f / buttonsCount) + 360f) % 360f; // Normalize the angle to be between 0 and 360 degrees
        int hoveredButtonIndex = Mathf.FloorToInt(angleDegrees / (360f / buttonsCount));

        return hoveredButtonIndex;
    }
}
