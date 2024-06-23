using TMPro;
using UnityEngine;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 150;
    [SerializeField] private float yOffset = 150;

    public virtual void AdjustPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        //print(mousePosition);

        float new_xOffset = 0;
        float new_yOffset = 0;

        if (mousePosition.x > xLimit)
            new_xOffset = -xOffset;
        else
            new_xOffset = xOffset;

        if (mousePosition.y > yLimit)
            new_yOffset = -yOffset;
        else
            new_yOffset = yOffset;

        transform.position = new Vector2(mousePosition.x + new_xOffset, mousePosition.y + new_yOffset);
    }

    public void AdjustFontSize(TextMeshProUGUI _text)
    {
        if (_text.text.Length > 12)
            _text.fontSize = _text.fontSize * .8f;
    }
}
