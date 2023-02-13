using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float health { get; private set; }
    [SerializeField] public float maxHealth { get; private set; }

    private void Start()
    {
        health = maxHealth;
    }

    public void Heal(float amount)
    {
        health += amount;
    }

    public bool Damaged(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            return true;
        }
        return false;
    }
}
