using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffectBox : MonoBehaviour {

    public void Add(List<Sprite> icons) {
        foreach (Sprite s in icons) {
            GameObject imageObject = new GameObject("Icon", typeof(RectTransform));
            imageObject.transform.SetParent(transform);
            Image image = imageObject.AddComponent<Image>();
            image.sprite = s;
        }

        GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
    }
}
