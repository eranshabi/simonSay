using UnityEngine;
using UnityEngine.UI;

public class GameplayButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Sprite hoverImage;
    [SerializeField] private Sprite normalImage;

    public void SetSizePositionAndColor(int size, int position, Color color)
    {
        image.fillAmount = 1f / size;
        transform.rotation = Quaternion.Euler(0f, 0f, 180f + 360f / size * position);
        SetColor(color);
    }

    public void SetColor(Color color)
    {
        image.color = color;
    }

    public void Hover()
    {
        image.sprite = hoverImage;
    }

    public void PointerExit()
    {
        image.sprite = normalImage;
    }
}
