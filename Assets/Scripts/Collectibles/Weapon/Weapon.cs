using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Collectible
{
    [SerializeField] public float damage { get; protected set; }
    public virtual void Attack()
    {

    }
}
