using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro _characterLabel;


    private List<CharacterData> _allCharacters = new List<CharacterData>();

    // Start is called before the first frame update
    void Start()
    {
        CharacterLoader.LoadAllCharacters(_allCharacters);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
