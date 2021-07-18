using System.Collections.Generic;
using UiMenus;
using UnityEngine;

namespace Inventory
{


    public class InventoryManagerUi : BaseUiView
    {

    }
    

    

    public abstract class InventoryItem : MonoBehaviour
    {
        [SerializeField] private float _weight;

        public float weight => _weight;

        public virtual void Collect()
        {
        }
    }

    public abstract class Consumable : InventoryItem
    {

    }

    public abstract class FixedAsset : InventoryItem
    {

    }

    public class Weapon : FixedAsset
    {
        private List<Ammo> _loadedAmmos;
    }

    public class Tool : FixedAsset
    {

    }

    public class Ammo : Consumable
    {

    }

    public class Food : Consumable
    {

    }

    public class Water : Consumable
    {

    }
}