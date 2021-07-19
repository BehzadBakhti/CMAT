using UnityEngine;

namespace Inventory
{
    public class Ammo : Consumable
    {
        [SerializeField] private AmmoType _type;
    }

    public enum AmmoType
    {
        killer,
        Anesthetic,
        Chemical
    }
}