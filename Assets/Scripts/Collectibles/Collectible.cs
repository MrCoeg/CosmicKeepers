using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : ScriptableObject
{
    
    public delegate void CollectibleEffect();
    public CollectibleEffect effect;
    public Sprite icon;

    public virtual Collectible Take()  
    {
        return this;
    }
}

public enum ItemId
{
    ArtefactFragment,
    Cosmic,
}

public enum WeaponId
{
    Bow,
    Arrow,
}
