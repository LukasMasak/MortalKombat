using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideAttackSettings : MonoBehaviour
{
    [SerializeField] private GameObject _attackSetting;
    [SerializeField] private GameObject _attackPoint;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_Dropdown>().onValueChanged.AddListener(SetVisibility);
    }

    void SetVisibility(int value)
    {
        if (value == 1) _attackSetting.SetActive(true);
        else            _attackSetting.SetActive(false);

        if (value == 1) _attackPoint.SetActive(true);
        else            _attackPoint.SetActive(false);
    }
}
