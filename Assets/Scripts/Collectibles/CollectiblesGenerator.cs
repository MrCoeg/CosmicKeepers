using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesGenerator : MonoBehaviour
{
    [SerializeField] List<ItemId> itemIds = new List<ItemId>();
    [SerializeField] List<WeaponId> weaponIds = new List<WeaponId>();
    [SerializeField] Inventory inventory;

    [SerializeField] bool inventoryDisplayMode;
    [SerializeField] GameObject invetoryUI;
    private void Awake()
    {
        gameObject.tag = "CollectibleGenerator";
        inventory = gameObject.AddComponent<Inventory>();
        inventory.Initialize(invetoryUI);
        inventory.DisplayInventory(inventoryDisplayMode);

        itemIds.ForEach(x => inventory.AddCollectible(CreateItem(x)));
        weaponIds.ForEach(x => inventory.AddCollectible(CreateWeapon(x)));
    }

    public void DisplayInventory()
    {
        inventoryDisplayMode = !inventoryDisplayMode;
        inventory.DisplayInventory(!inventoryDisplayMode);
    }

    private Item CreateItem(ItemId id)
    {
        return ScriptableObject.CreateInstance<Item>().Create(id);
    }

    private Weapon CreateWeapon(WeaponId id)
    {
        return ScriptableObject.CreateInstance<Weapon>().Create(id);
    }

   
}
