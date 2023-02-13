using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Seed : MonoBehaviour
{
    [SerializeField] public List<Slot> slots { get; private set; } = new List<Slot>();

    private void Awake()
    {
        slots = GetComponentsInChildren<Slot>().ToList();

    }
}
