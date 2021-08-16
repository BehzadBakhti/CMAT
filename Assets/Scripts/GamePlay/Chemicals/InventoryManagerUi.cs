using System;
using System.Collections.Generic;
using MonstersDataManagement;
using TMPro;
using UiMenus;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryManagerUi : BaseUiView
    {
        [SerializeField] private RectTransform _weaponParent;
        [SerializeField] private RectTransform _toolsParent;
        [SerializeField] private RectTransform _ammoParent;
        [SerializeField] private GameObject _itemsPrefab;// TODO Proper pooling System required
        
        private InventoryData _inventoryData;

        public override void Init()
        {
            _inventoryData ??= new InventoryData();
            var invData = _inventoryData.Load();
            for (int i = 0; i < invData.weapons.Count; i++)
            {
                 Instantiate(_itemsPrefab, _weaponParent).GetComponent<InventoryItemUi>().Init(invData.weapons[i], ItemClicked);

            }
            for (int i = 0; i < invData.tools.Count; i++)
            {
                Instantiate(_itemsPrefab, _toolsParent).GetComponent<InventoryItemUi>().Init(invData.tools[i], ItemClicked);

            }
            for (int i = 0; i < invData.ammos.Count; i++)
            {
                Instantiate(_itemsPrefab, _ammoParent).GetComponent<InventoryItemUi>().Init(invData.ammos[i], ItemClicked);

            }
        }

        private void ItemClicked(InventoryItem item)
        {

        }


    }

    public class InventoryItemUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameTag;
        [SerializeField] private TextMeshProUGUI _countTag;
        [SerializeField] private Image _icon;

        private InventoryItem item;
        private int _count;
        private Action<InventoryItem> _onClick;

        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OcClicked);
        }

        private void OcClicked()
        {
            _onClick?.Invoke(item);
        }

        public void Init(InventoryItem item, Action<InventoryItem> callBack)
        {
            _nameTag.text = item.nameTag;
            _icon.sprite = item.icon;
            _countTag.text = item.count.ToString();
            _count = item.count;
            _onClick = callBack;
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