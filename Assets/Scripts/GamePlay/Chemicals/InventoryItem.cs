using System;
using System.Collections.Generic;
using MonstersDataManagement;
using UnityEngine;

namespace Inventory
{

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

    public abstract class Weapon : FixedAsset
    {
        private List<Ammo> _loadedAmmos;
    }

    public abstract class Tool : FixedAsset
    {

    }

    public class Food : Consumable
    {

    }

    public class Water : Consumable
    {

    }
}