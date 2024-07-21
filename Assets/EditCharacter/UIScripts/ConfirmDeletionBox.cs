using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConfirmDeletionBox : MonoBehaviour
{
    [SerializeField] private TMP_Text _tmpText;
    [SerializeField] private string _textToPrint;

    private CharacterListManager _characterListManager;
    private CharacterData _characterToBeDecided;

    public void Initialize(CharacterListManager listManager, CharacterData data)
    {
        _characterListManager = listManager;
        _characterToBeDecided = data;
        _tmpText.text = _textToPrint + data.name + "?";
    }

    public void Delete()
    {
        CharacterLoader.DeleteCharacterFolder(_characterToBeDecided.name);
        GlobalState.AllCharacters.Remove(_characterToBeDecided);
        _characterListManager.RefreshCharactersMenu();
        Destroy(gameObject);
    }

    public void Deny()
    {
        Destroy(gameObject);
    }

}
