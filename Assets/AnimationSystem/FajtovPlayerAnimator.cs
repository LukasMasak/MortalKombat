using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FajtovPlayerAnimator : MonoBehaviour
{
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] public UnityEvent animationStarted;
    [SerializeField] public UnityEvent animationLooped;
    [SerializeField] public UnityEvent animationEnded;

    public enum FajtovAnimationStates 
    {
        Idle,
        Move,
        Jump,
        Attack,
        Block,
        Hurt,
        Death,
    }

    // References
    private SpriteRenderer _spriteRenderer;

    // Private vars
    private CharacterData _characterData;
    private bool [,] animTransMatrix = 
    { 
        // From ROW to COL
        //Idle  Move   Jump   Atk    Block  Hurt   Death
        {false, true,  true,  true,  true,  true,  true}, // Idle
        {true,  false, true,  true,  true,  true,  true}, // Move
        {true,  false, false, true,  true,  true,  true}, // Jump
        {true,  true,  true,  false, true,  true,  true}, // Atk
        {true,  false, true,  true,  false, true,  true}, // Block
        {true,  false, true,  true,  true,  false, true}, // Hurt
        {false, false, false, false, false, false, false},// Death

    };
    //private bool isMoving = false;
    //private bool isBlocking = false;

    private FajtovAnimationStates _currentState = FajtovAnimationStates.Idle;
    private FajtovAnimationClip _currentAnim;
    private Coroutine _runningCoroutine;
    private bool _currentAnimComplete;
    private int _currentFrameNum = 0;
    private bool _animationDisabled = false;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void Initialize(ref CharacterData characterData)
    {
        if (_spriteRenderer == null) Start();
        _characterData = characterData;
        if (_runningCoroutine != null) StopCoroutine(_runningCoroutine);    
        ChangeState(FajtovAnimationStates.Idle, true);        
    }


    public void ChangeState(FajtovAnimationStates newState, bool forceChange = false)
    {
        // Check if state can be changed
        if (!forceChange)
        {
            if (!animTransMatrix[(int)_currentState, (int)newState]) return;
            if (newState == _currentState) return;
            if (!_currentAnimComplete && !_currentAnim.canBeInterupted) return;
        }

        _animationDisabled = false;
        _currentState = newState;
        _currentAnimComplete = false;
        StopAnimation();
        _runningCoroutine = StartCoroutine(PlayAnimation());
    }

    //--------------------Edit character menu functions--------------------
    public void ShowBubbleIcon()
    {
        StopAnimation();
        _animationDisabled = true;
        _spriteRenderer.sprite = _characterData.bubbleIcon;
    }


    public void ShowPreviewIcon()
    {
        StopAnimation();
        _animationDisabled = true;
        _spriteRenderer.sprite = _characterData.preview;
    }


    public void ContinueAnimation()
    {
        if (_animationDisabled) return;

        StopAnimation();
        if (_currentFrameNum == _currentAnim.frames.Length - 1) _currentFrameNum = 0;
        StartCoroutine(PlayAnimation(_currentFrameNum));
    }


    public void ReplayAnimation()
    {
        if (_animationDisabled) return;

        StopAnimation();
        _currentFrameNum = 0;
        StartCoroutine(PlayAnimation(0));
    }


    public void StopAnimation()
    {
        if (_animationDisabled) return;

        if (_runningCoroutine == null) return;
        StopCoroutine(_runningCoroutine);
    }


    public void NextFrame()
    {
        if (_animationDisabled) return;

        _currentFrameNum = Mathf.Min(_currentFrameNum + 1, _currentAnim.frames.Length - 1);
        Sprite frame = _currentAnim.frames[_currentFrameNum];
        _spriteRenderer.sprite = frame;
    }


    public void PrevFrame()
    {
        if (_animationDisabled) return;

        _currentFrameNum = Mathf.Max(_currentFrameNum - 1, 0);
        Sprite frame = _currentAnim.frames[_currentFrameNum];
        _spriteRenderer.sprite = frame;
    }


    public int GetCurrentFrameNum()
    {
        return _currentFrameNum;
    }
    //--------------------Edit character menu functions end--------------------


    private IEnumerator PlayAnimation(int startFrame = 0)
    {
        _currentAnim = GetAnimationBasedOnState();
        _currentFrameNum = startFrame;

        // Check if anim exists
        if (_currentAnim.frames == null || _currentAnim.frames.Length == 0) 
        {
            _spriteRenderer.sprite = _defaultSprite;
            yield return null;
        }
        animationStarted.Invoke();
        
        if (_currentAnim.frames != null)
        {
            for (int i = startFrame; i < _currentAnim.frames.Length; i++)
            {
                Sprite frame = _currentAnim.frames[i];
                _spriteRenderer.sprite = frame;
                _currentFrameNum = i;
                yield return new WaitForSeconds(CharacterLoader.FRAME_DELAY);
            }
        }

        _currentAnimComplete = true;
        if (_currentAnim.isLooping)
        {
            _runningCoroutine = StartCoroutine(PlayAnimation());
            animationLooped.Invoke();
        }
        else
        {
            animationEnded.Invoke();
        }
    }


    private ref FajtovAnimationClip GetAnimationBasedOnState()
    {
        if (_currentState == FajtovAnimationStates.Idle)
        {
            return ref _characterData.idleAnim;
        }
        else if (_currentState == FajtovAnimationStates.Attack)
        {
            return ref _characterData.attackAnim;
        }
        else if (_currentState == FajtovAnimationStates.Block)
        {
            return ref _characterData.blockAnim;
        }
        else if (_currentState == FajtovAnimationStates.Move)
        {
            return ref _characterData.walkAnim;
        }
        else if (_currentState == FajtovAnimationStates.Hurt)
        {
            return ref _characterData.hurtAnim;
        }
        else if (_currentState == FajtovAnimationStates.Death)
        {
            return ref _characterData.deathAnim;
        }
        else if (_currentState == FajtovAnimationStates.Jump)
        {
            return ref _characterData.jumpAnim;
        }
        else
        {
            Debug.LogError("Unknown animation state! " + _currentState);
            return ref _characterData.idleAnim;
        }
    }
}
