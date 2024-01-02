using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCharacterMenu : MonoBehaviour
{
    public CharacterMenu characterMenu;

    public int X_START;
    public int Y_START;

    public int X_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<CharacterPref, GameObject> itemsDisplayed = new Dictionary<CharacterPref, GameObject>();

    private void Start()
    {
        CreateDisplay();
    }

    private void Update()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        for (int i = 0; i < characterMenu.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(characterMenu.Container[i].character))
            {
                return;
            }
            else
            {
                var obj = Instantiate(characterMenu.Container[i].character.prefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                itemsDisplayed.Add(characterMenu.Container[i].character, obj);

            }
        }
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < characterMenu.Container.Count; i++)
        {
            var obj = Instantiate(characterMenu.Container[i].character.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            itemsDisplayed.Add(characterMenu.Container[i].character, obj);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEMS) * (i / NUMBER_OF_COLUMN), 0f);
    }

    
}
