using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Collectible collectible;

    [SerializeField] Image select;
    [SerializeField] Image icon;


    private void Awake()
    {
        select = GetComponent<Image>();
        icon = GetComponentsInChildren<Image>()[1];

        select.color = new Color32(120, 120, 120, 255);
    }

    public bool CheckEmpty()
    {
        return collectible == null; 
    }

    public void SetSelected()
    {
        select.color = new Color32(255, 255, 255, 255);
    }

    public void SetUnselected()
    {
        select.color = new Color32(120, 120, 120, 120);
    }

    public void SetSelectedImageColor(byte r, byte g, byte b, byte a)
    {
        select.color = new Color32(r,g,b,a);
    }

    public void setCollectible(Collectible newCollectible, Inventory inventory)
    {
        collectible = newCollectible;
        icon.sprite = newCollectible.icon;

        Button button = icon.gameObject.AddComponent<Button>();
        button.onClick.AddListener(() => {
            var playersInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            if (!inventory.Equals(playersInventory))
            {
                Debug.Log("Success Input not players");
            }
            else
            {
                Debug.Log("Success Input players");

            }
        });
    }

    public void UseCollectible()
    {
        Debug.Log("Use Effect");
        collectible.effect();
    }

}
