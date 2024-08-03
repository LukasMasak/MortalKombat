using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// A popup box which is used to confirm the deletion of a character
public class ConfirmDeletionBox : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private string _textToPrint;

    private CharacterListManager _characterListManager;
    private CharacterData _characterToBeDecided;


    // Initialization of the popup with the manager and character to be deleted
    public void Initialize(CharacterListManager listManager, CharacterData data)
    {
        _characterListManager = listManager;
        _characterToBeDecided = data;
        _tmpText.text = _textToPrint + data.name + "?";
    }


    // Callback for the delete button, which deletes the character
    public void Delete()
    {
        CharacterLoader.DeleteCharacterFolder(_characterToBeDecided.name);
        GlobalState.AllCharacters.Remove(_characterToBeDecided);
        _characterListManager.RefreshCharactersMenu();
        Destroy(gameObject);
    }


    // Callback for the deny button, which only closes the popup
    public void Deny()
    {
        Destroy(gameObject);
    }

}
