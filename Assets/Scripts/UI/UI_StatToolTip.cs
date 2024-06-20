using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriotion;

    public void ShowStatToopTip(string _text)
    {
        descriotion.text = _text;
        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        descriotion.text = "";
        gameObject.SetActive(false);
    }
}
