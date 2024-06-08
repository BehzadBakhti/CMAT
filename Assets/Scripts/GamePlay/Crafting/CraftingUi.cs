using System;
using System.Collections;
using System.Collections.Generic;
using Crafting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUi : MonoBehaviour
{
    [SerializeField] private Dropdown _partSelection;
    [SerializeField] private Button _loadBtn;
    [SerializeField] private Button _newBtn;
    [SerializeField] private Button _saveBtn;
    [SerializeField] private Button _deleteBtn;
    [SerializeField] private TextMeshProUGUI _partName;


    

    private void Awake()
    {
        InitUi();

    }

    private void InitUi()
    {
        _saveBtn.onClick.AddListener(Save);
        _partSelection.options = new List<Dropdown.OptionData>();
    }

    private void NewObject()
    {
    }

    private void Clear()
    {

    }


    private void Save()
    {

    }




}
[Serializable]
public class CraftingData
{
    public Dictionary<Guid, CraftedObject> CraftedObjects;
    public Dictionary<Guid, CraftingPart> CraftedParts;
}