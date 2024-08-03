using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Custom scrollable list manager for characters menu
public class CharacterListManager : MonoBehaviour
{
    [SerializeField] private MenuManager _menuManager;
    [SerializeField] private GameObject _characterSelectMenu;
    [SerializeField] private RectTransform _contentTransform;
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _deleteConfirmationPrefab;

    [SerializeField] private string _addCharacterText = "+ Add Character";

    private float _itemHeight = 60;


    // Get the size of a single character menu item
    private void Start()
    {
        _itemHeight = _itemPrefab.GetComponent<RectTransform>().rect.height;
    }


    // Toggles on and off the character select menu
    public void ToggleCharacterSelect()
    {
        // Turn off the menu upon toggle
        if (_characterSelectMenu.activeSelf) 
        {
            _characterSelectMenu.SetActive(false);
            EraseCharactersMenu();
            return;
        }

        // else populate the menu and turn it on
        _characterSelectMenu.SetActive(true);
        EraseCharactersMenu();
        PopulateCharactersMenu();
    }


    // Populates the character menu with + Add character button and all characters
    public void PopulateCharactersMenu()
    {
        // Create the first button and delete the delete button from it
        GameObject addCharacterItem = Instantiate(_itemPrefab, _contentTransform);
        addCharacterItem.GetComponentInChildren<TMP_Text>().text = _addCharacterText;
        RectTransform addCharacterDeleteRectTransform = addCharacterItem.transform.GetChild(0).GetComponent<RectTransform>();
        RectTransform addCharacterTextRectTransform = addCharacterItem.transform.GetChild(1).GetComponent<RectTransform>();
        addCharacterTextRectTransform.sizeDelta += new Vector2(addCharacterDeleteRectTransform.sizeDelta.x, 0);
        Destroy(addCharacterDeleteRectTransform.gameObject);

        // Add a listener to create a new character
        addCharacterTextRectTransform.GetComponent<Button>().onClick.AddListener(delegate{_menuManager.SwitchCharacterToIdx(0); ToggleCharacterSelect();});

        // Add all buttons for characters
        int characterIdx = 1;
        foreach (CharacterData character in GlobalState.AllCharacters)
        {
            Transform characterItem = Instantiate(_itemPrefab, _contentTransform).transform;
            characterItem.localPosition += new Vector3(0, -_itemHeight * characterIdx, 0);
            characterItem.GetComponentInChildren<TMP_Text>().text = character.name;
            Button characterDeleteButton = characterItem.GetChild(0).GetComponent<Button>();
            Button characterTextButton = characterItem.GetChild(1).GetComponent<Button>();
            characterTextButton.GetComponent<Button>().onClick.AddListener(delegate{_menuManager.SwitchCharacterToIdx(characterItem.GetSiblingIndex()); ToggleCharacterSelect();});
            characterDeleteButton.GetComponent<Button>().onClick.AddListener(delegate{AskToDeleteACharacter(characterItem.GetSiblingIndex() - 1);});

            characterIdx += 1;
        }

        // Resize the content to be scrollable
        _contentTransform.sizeDelta = new Vector2(0, characterIdx * _itemHeight);
    }


    // Deletes all entries from the menu and resize the content back
    public void EraseCharactersMenu()
    {
        foreach (Transform child in _contentTransform)
        {
            Destroy(child.gameObject);
        }
        _contentTransform.sizeDelta = new Vector2(0, _itemHeight);
    }


    // Erases and again populates the character menu
    public void RefreshCharactersMenu()
    {
        EraseCharactersMenu();
        PopulateCharactersMenu();
    }


    // Deletes the selected character
    public void AskToDeleteACharacter(int idx)
    {
        if (idx >= GlobalState.AllCharacters.Count)
        {
            Debug.LogWarning("Tried to delete character of idx " + idx + " but all characters only have " + GlobalState.AllCharacters.Count + " characters.");
            return;
        }

        // Create a confirm box
        GameObject deleteConfirmationInstance = Instantiate(_deleteConfirmationPrefab, transform.parent);
        ConfirmDeletionBox confirmDeletionBox = deleteConfirmationInstance.GetComponent<ConfirmDeletionBox>();
        confirmDeletionBox.Initialize(this, GlobalState.AllCharacters[idx]);
    }
}
