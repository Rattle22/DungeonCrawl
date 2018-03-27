using RatStudios.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class ItemDisplay : MonoBehaviour {

    public GameObject RowPrefab;
    public GameObject EffectBoxPrefab;
    public List<Sprite> SpriteMap;

    private Text itemName;
    private int rowCount = 0;

    private void Awake()
    {
        itemName = GetComponentInChildren<Text>();
    }

    public void displayItem(Item item) {
        //Reset display
        rowCount = 0;
        for (int i = 0; i < transform.childCount; i++) {
            GameObject go = transform.GetChild(i).gameObject;
            if (go.name != "ItemName") {
                Destroy(go);
            }
        }
        
        itemName.text = item.GetName();

        //Create new row for every stat
        int damage = item.GetDamage();
        if (damage > 0)
        {
            addRow(damage, SpriteMap[0]);
        }
        int defense = item.GetDefense();
        if (defense > 0)
        {
            addRow(defense, SpriteMap[1]);
        }
        int health = item.GetHealth();
        if (health > 0)
        {
            addRow(health, SpriteMap[2]);
        }

        GameObject effectBox = Instantiate(EffectBoxPrefab);
        effectBox.transform.SetParent(transform);
        effectBox.GetComponent<ItemEffectBox>().Add(item.GetEffectIcons());
    }

    private void addRow(int number, Sprite icon)
    {
        GameObject newRow = Instantiate(RowPrefab, transform);
        newRow.GetComponent<ItemDisplayRow>().Set(number, icon);


        rowCount++;
    }
}
