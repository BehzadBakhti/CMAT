using System;
using System.Collections.Generic;
using MonstersDataManagement;
using UiMenus;
using UnityEngine;

namespace Inventory
{
    public class InventoryManagerUi : BaseUiView
    {
        [SerializeField] private RectTransform _weaponParent;
        [SerializeField] private RectTransform _toolsParent;
        [SerializeField] private RectTransform _ammoParent;
        private InventoryData _inventoryData;

        public override void Init()
        {
            _inventoryData ??= new InventoryData();

            for (int i = 0; i < _inventoryData.Load().weapons.Count; i++)
            {
                
            }
        }
    }

    public class InventoryData : BaseDataClass<InventoryDataModel>
    {


    }



    [Serializable]
    public class InventoryDataModel
    {
        public List<Weapon> weapons;
        public List<Tool> tools;
        public List<Ammo> ammos;

    }



}