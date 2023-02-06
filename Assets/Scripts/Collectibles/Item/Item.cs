using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Subtegral.DialogueSystem.DataContainers;

public abstract class Item : Collectible
{
    private StoryContainer story; 

    public virtual void Use()
    {

    }

    public override Collectible Create(CollectibleId id)
    {
        Item item;
        string path = "DialogueSystem/Dialogue/";
        switch (id) 
        {
            case CollectibleId.Stars:
                item = ScriptableObject.CreateInstance<Item>();
                Debug.Log(path + id.ToString());
                story = Resources.Load<StoryContainer>(path+id.ToString());
                return item;
        }
        return null;
    }
}
