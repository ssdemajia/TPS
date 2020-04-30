using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [System.Serializable]
    public class ContainerItem
    {
        public System.Guid ID;
        public string Name;
        public int Maximum;

        public int amountTaken = 0;

        public ContainerItem()
        {
            ID = System.Guid.NewGuid();
        }

        public int Get(int amount)
        {
            if (amountTaken >= Maximum)
                return 0;
            if (amountTaken + amount > Maximum)
            {
                int toMuch = Maximum - amountTaken;
                amountTaken = Maximum;
                return toMuch;
            }
            amountTaken += amount;
            return amount;
        }
        
        public int Remaining
        {
            get
            {
                return Maximum - amountTaken;
            }
        }
    }

    [SerializeField]
    List<ContainerItem> items;

    private void Awake()
    {
        items = new List<ContainerItem>();
    }
    public System.Guid Add(string name, int maximum)
    {
        var item = new ContainerItem
        {
            Name = name,
            Maximum = maximum
        };
        items.Add(item);
        return item.ID;
    }
    
    public int TakeFromContainer(System.Guid id, int amount)
    {
        var item = items.Find(x => x.ID == id);
        if (item == null)
            return -1;

        return item.Get(amount);
    }

    public ContainerItem GetItem(System.Guid id)
    {
        return items.Find(x => x.ID == id);
    }
}
