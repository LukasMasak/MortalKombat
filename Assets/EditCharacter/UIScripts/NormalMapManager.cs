using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class NormalMapManager : MonoBehaviour
{
    [SerializeField] private Slider _edgesStrengthSlider;
    [SerializeField] private Slider _edgeBlurSlider;
    [SerializeField] private Slider _borderStrengthSlider;
    [SerializeField] private Slider _borderBlurSlider;
    [SerializeField] private Slider _borderSoftenSlider;

    [SerializeField] private Slider _borderSlopePercentageSlider;
    [SerializeField] private Slider _finalBlurSlider;
    [SerializeField] private Toggle _autoGenerateToggle;

    [SerializeField] private Toggle _showNormalMapToggle;

    [SerializeField] private FajtovPlayerAnimator _previewAnimator;
    [SerializeField] private TMP_Dropdown _animationSelectDropdown;

    private NormalMapGenerator _normalMapGenerator;
    private CharacterData _selectedCharacter;
    private Vector2 _offscreenPosition;

    // Get references
    void Start()
    {
        _normalMapGenerator = GetComponent<NormalMapGenerator>();
        _offscreenPosition = transform.position;

        // TODO remove
        if (GlobalState.AllCharacters.Count == 0) CharacterLoader.LoadAllCharacters(GlobalState.AllCharacters);
        InitializeMenu(GlobalState.AllCharacters[0]);
    }


    // Initializes the normal map menu with the selected character
    public void InitializeMenu(CharacterData selectedCharacter)
    {
        _selectedCharacter = selectedCharacter;
        _previewAnimator.Initialize(_selectedCharacter);
        _previewAnimator.ShowPreviewIcon();

        // Update the normal toggle
        OnShowNormalMapToggle();
    }


    // Callback for the showNormalMap toggle button
    public void OnShowNormalMapToggle()
    {
        if (_showNormalMapToggle.isOn) 
        {
            if (_selectedCharacter.previewNormalMap == null) OnGeneratePreviewButtonDown();
            else _previewAnimator.ShowPreviewIcon(previewNormal: true);
        }
        else
        {
            _previewAnimator.ShowPreviewIcon(previewNormal: false);
        }
    }


    // Callback for any parameter change
    public void OnSliderChange()
    {
        if (_autoGenerateToggle.isOn) OnGeneratePreviewButtonDown();
    }


    // Callback for generate preview button
    public void OnGeneratePreviewButtonDown()
    {
        Debug.Log("Calling with " + _edgesStrengthSlider.value + " strenght edge " + (int)_edgeBlurSlider.value + " edge blur");
        Texture2D texture = _normalMapGenerator.GenerateNormalMap(_selectedCharacter.preview.texture, _edgesStrengthSlider.value, (int)_edgeBlurSlider.value,
                            _borderStrengthSlider.value, (int)_borderBlurSlider.value, (int)_borderSoftenSlider.value, _borderSlopePercentageSlider.value, (int)_finalBlurSlider.value);
        _selectedCharacter.previewNormalMap = texture;                                     
        OnShowNormalMapToggle();
    }


    // Saves and generates all normal maps for a character
    public void OnGenerateAndSaveButtonDown()
    {
        List<Texture2D> normalMaps = new List<Texture2D>();

        GenerateNormalMapsForAnimation(normalMaps, ref _selectedCharacter.attackAnim);
        _selectedCharacter.attackAnim.normalMapframes = normalMaps.ToArray();
        normalMaps.Clear();
        // TODO generate all normal maps and set


        int charIdx = GlobalState.AllCharacters.IndexOf(_selectedCharacter);

        GlobalState.AllCharacters[charIdx] = _selectedCharacter;
        CharacterLoader.SaveConfigOfCharacter(_selectedCharacter);
    }


    private void GenerateNormalMapsForAnimation(List<Texture2D> resultList, ref FajtovAnimationClip anim)
    {
        foreach (Sprite sprite in anim.frames)
        {
            Texture2D texture = _normalMapGenerator.GenerateNormalMap(sprite.texture, _edgesStrengthSlider.value, (int)_edgeBlurSlider.value,
                            _borderStrengthSlider.value, (int)_borderBlurSlider.value, (int)_borderSoftenSlider.value, _borderSlopePercentageSlider.value, (int)_finalBlurSlider.value);
            resultList.Add(texture);                                                        
        }
    }


    public void OnDeleteButtonDown()
    {
        _selectedCharacter.attackAnim.normalMapframes = null;
        _selectedCharacter.hurtAnim.normalMapframes = null;
        _selectedCharacter.idleAnim.normalMapframes = null;
        _selectedCharacter.jumpAnim.normalMapframes = null;
        _selectedCharacter.walkAnim.normalMapframes = null;
        _selectedCharacter.blockAnim.normalMapframes = null;
        _selectedCharacter.deathAnim.normalMapframes = null;

        // TODO
        //CharacterLoader.DeleteNormalMaps(_selectedCharacter.name);
    }

    public void OnBackButtonDown()
    {
        // TODO go back
        // TODO reload normal maps on current character
    }
}