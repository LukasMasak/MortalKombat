using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default CharacterList", menuName = "Choose Character/CharactersList")]

public class CharacterMenu : ScriptableObject
{
    public List<CharacterSlots> Container = new List<CharacterSlots>();
    public void AddCharacter(CharacterPref _character, int _amount)
    {
        bool newCharacter = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].character == _character)
            {
                Container[i].AddAmount(_amount);
                newCharacter = true;
                break;
            }
        }
        if (!newCharacter)
        {
            Container.Add(new CharacterSlots(_character, _amount));
        }
    }
    
}

[System.Serializable]
public class CharacterSlots
{
    public CharacterPref character;
    public int amount;
    public CharacterSlots(CharacterPref _character, int _amount)
    {
        character = _character;
        amount = _amount;
    }

    public void AddAmount(int Value)
    {
        amount += Value;
    }
}
