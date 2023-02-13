using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collectible
{
    public float damage { get; protected set; }
    private Health[] target;

    public Weapon Create(WeaponId id)
    {
        var weapon = ScriptableObject.CreateInstance<Weapon>();
        switch (id)
        {
            case WeaponId.Bow:
                damage = 10;
                return weapon;
        }
        return null;
    }
}
