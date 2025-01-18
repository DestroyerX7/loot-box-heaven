using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [System.Serializable]
    private struct SelectionSaveData
    {
        public string CharacterSelectionDataSOName;
        public string WeaponSelectionDataSOName;
        public string PetSelectionDataSOName;

        public SelectionSaveData(string characterSelectionDataSOName, string weaponSelectionDataSOName, string petSelectionDataSOName)
        {
            CharacterSelectionDataSOName = characterSelectionDataSOName;
            WeaponSelectionDataSOName = weaponSelectionDataSOName;
            PetSelectionDataSOName = petSelectionDataSOName;
        }
    }

    public static SelectionManager Instance { get; private set; }

    [field: SerializeField] public SelectionDataSO CharacterSelectionDataSO { get; private set; }
    [field: SerializeField] public SelectionDataSO WeaponSelectionDataSO { get; private set; }
    [field: SerializeField] public SelectionDataSO PetSelectionDataSO { get; private set; }

    [SerializeField] private SelectionDataSO[] _selectionDataSOs;
    [SerializeField] private Dictionary<string, SelectionDataSO> _selectionDataSOsDictionary = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SetupDictionary();

        SelectionSaveData selectionSaveData = SaveDataManager.LoadData<SelectionSaveData>("/selection-data.json");

        if (selectionSaveData.CharacterSelectionDataSOName != null)
        {
            CharacterSelectionDataSO = _selectionDataSOsDictionary[selectionSaveData.CharacterSelectionDataSOName];
            WeaponSelectionDataSO = _selectionDataSOsDictionary[selectionSaveData.WeaponSelectionDataSOName];

            if (!string.IsNullOrEmpty(selectionSaveData.PetSelectionDataSOName))
            {
                PetSelectionDataSO = _selectionDataSOsDictionary[selectionSaveData.PetSelectionDataSOName];
            }
        }
    }

    private void OnDestroy()
    {
        SelectionSaveData selectionSaveData = new(CharacterSelectionDataSO.name, WeaponSelectionDataSO.name, PetSelectionDataSO != null ? PetSelectionDataSO.name : null);
        selectionSaveData.SaveData("/selection-data.json");
    }

    private void SetupDictionary()
    {
        foreach (SelectionDataSO selectionDataSO in _selectionDataSOs)
        {
            _selectionDataSOsDictionary.Add(selectionDataSO.name, selectionDataSO);
        }
    }

    public void Select(SelectionType selectionType, SelectionDataSO selectionDataSO)
    {
        switch (selectionType)
        {
            case SelectionType.Character:
                CharacterSelectionDataSO = selectionDataSO;
                break;
            case SelectionType.Weapon:
                WeaponSelectionDataSO = selectionDataSO;
                break;
            case SelectionType.Pet:
                PetSelectionDataSO = selectionDataSO;
                break;
        }

        MainMenuManager.Instance.Select(selectionType, selectionDataSO.InventoryItemSO);
    }
}

public enum SelectionType
{
    Character,
    Weapon,
    Pet,
}
