using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowHideAttackSettings : MonoBehaviour
{
    [SerializeField] private GameObject _attackSetting;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(SetVisibility);
    }

    void SetVisibility(int value)
    {
        if (value == 1) _attackSetting.SetActive(true);
        else            _attackSetting.SetActive(false);
    }
}
