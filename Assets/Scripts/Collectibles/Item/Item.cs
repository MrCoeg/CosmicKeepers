using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Collectible
{
    public void Use()
    {
        effect();
    }

    public Item Create(ItemId id)
    {
        var item = ScriptableObject.CreateInstance<Item>();
        switch (id) 
        {
            case ItemId.Cosmic:
                item.name = "Cosmic";
                item.icon = Resources.Load<Sprite>("Icon_Cosmic");
                item.effect = () => {
                    var healthStats = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
                    Debug.Log(item.name);
                };
                return item;
            case ItemId.ArtefactFragment:
                item.name = "Artefact Fragment";
                item.icon = Resources.Load<Sprite>("Icon_Fragment");
                item.effect = () =>
                {
                    Debug.Log(item.name);
                };
                return item;
        }
        return null;
    }
}
