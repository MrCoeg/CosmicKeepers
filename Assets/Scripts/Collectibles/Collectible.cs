using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectible : ScriptableObject
{
    public virtual Collectible Take()  
    {
        return this;
    }

    public virtual Collectible Create(CollectibleId id)
    {
        return this;
    }
}

public enum CollectibleId
{
    Stars
}
