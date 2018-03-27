using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayRow : MonoBehaviour {

    private Text statText;
    private Image statIcon;

    private void Awake()
    {
        statText = GetComponentInChildren<Text>();
        statIcon = GetComponentInChildren<Image>();
    }

    public void Set(int number, Sprite icon) {
        statText.text = "" + number;
        statIcon.sprite = icon;
    }
}
