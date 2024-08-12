using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private float invincibilityTime = 0.2f;
    [SerializeField] private Gradient hpSliderGradient;

    // private vars
    private int _maxHealth = 100;
    private int _currentHealth;
    private bool _canTakeDamage = true;

    // References
    private PlayerController playerController;
    private FajtovPlayerAnimator fajtovAnimator;
    private Slider _hpSlider;
    private Image _hpSliderfill;

    
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        fajtovAnimator = GetComponent<FajtovPlayerAnimator>();
    }

    public void Initialize(Slider hpSlider, Image hpSliderFill, int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
        _hpSlider = hpSlider;
        _hpSliderfill = hpSliderFill;
        _hpSlider.value = _currentHealth / (float)_maxHealth;
        _hpSliderfill.color = hpSliderGradient.Evaluate(0f);
    }

    public void TakeDamage(int damage)
    {
        if (!_canTakeDamage) return;

        if (_currentHealth > 0)
        {
            _currentHealth = Mathf.Max(_currentHealth - damage, 0);
            _hpSlider.value = _hpSlider.value = _currentHealth / (float)_maxHealth;;
            _hpSliderfill.color = hpSliderGradient.Evaluate(1-_hpSlider.value);

            if (_currentHealth <= 0)
            {
                Die();
                return;
            }

            fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Hurt);
            _canTakeDamage = false;
            StartCoroutine(InvincibilityAfterHit());
        }
    }

    private IEnumerator InvincibilityAfterHit()
    {
        playerController.FreezePlayer();
        yield return new WaitForSeconds(invincibilityTime);
        _canTakeDamage = true;
        playerController.UnFreezePlayer();
    }

    public void Die()
    {
        fajtovAnimator.ChangeState(FajtovPlayerAnimator.FajtovAnimationStates.Death, true);
        playerController.enabled = false;
        FightManager.Instance.ShowWinUI(playerController.GetWhichPlayer());
        enabled = false;
    }
    
}
