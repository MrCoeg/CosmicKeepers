using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] public int slotsCapacity { get; private set; } = 20;
    private List<Collectible> collectibles = new List<Collectible>();
    private List<Seed> seeds = new List<Seed>();
    private GameObject UI;

    private int seedsMaxIndex;
    private int activeSeedsIndex;

    private int slotsMaxIndex;
    private int activeSlotsIndex;

    public void Initialize(GameObject inventoryUI)
    {
        UI = inventoryUI;
        seeds = UI.GetComponentsInChildren<Seed>().ToList();
/*        seeds.ForEach(x => x.slots.ForEach(y => y.SetSelectedImageColor(120, 120, 120, 255)));*/
        seeds.ForEach(x => x.gameObject.SetActive(false));

        activeSeedsIndex = 0;
        seedsMaxIndex = seeds.Count - 1;

        slotsMaxIndex = seeds[0].slots.Count - 1;
        activeSlotsIndex = 0;

        seeds[activeSeedsIndex].gameObject.SetActive(true);
/*        GetSelectedSlots().SetSelectedImageColor(120, 120, 120, 255);*/

    }

    public void DisplayInventory(bool displayMode)
    {
        UI.gameObject.SetActive(displayMode);
    }

    public void ChangeSlots(int i)
    {
        GetSelectedSlots().SetUnselected();
        activeSlotsIndex += i;
        if (activeSlotsIndex > slotsMaxIndex) activeSlotsIndex = 0;
        if (activeSlotsIndex < 0) activeSlotsIndex = slotsMaxIndex;

        GetSelectedSlots().SetSelected();
    }

    public Inventory ChangeSeeds(int i)
    {
        GetSelectedSlots().SetUnselected();
        seeds[activeSeedsIndex].gameObject.SetActive(false);

        activeSeedsIndex += i;
        if (activeSeedsIndex > seedsMaxIndex) activeSeedsIndex = 0;
        if (activeSeedsIndex < 0) activeSeedsIndex = seedsMaxIndex;

        seeds[activeSeedsIndex].gameObject.SetActive(true);
        GetSelectedSlots().SetSelected();
        return this;
    }

    

    public void GetCollectible()
    {

    }

    public void UseCollectible()
    {
        Debug.Log("Use Collectible");
        GetSelectedSlots().UseCollectible();
    }

    public void AddCollectible(Collectible newCollectible)
    {
        Slot slot = new Slot();
        if(collectibles.Count < slotsCapacity && newCollectible != null)
        {
            seeds.Find(x => slot = x.slots.Find(y => y.CheckEmpty()));
        }

        if(slot != null)
        {
            slot.setCollectible(newCollectible, this);
        }

        
    }

    public void RemoveCollectible(Collectible oldCollectible)
    {
        collectibles.Remove(oldCollectible);
    }

    private Slot GetSelectedSlots()
    {
        return seeds[activeSeedsIndex].slots[activeSlotsIndex];
    }
}
