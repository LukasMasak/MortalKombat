using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _universalPlayerPrefabs;
    
    // The spawn points of the players
    [SerializeField] private Transform _leftSpawnTransform;
    [SerializeField] private Transform _rightSpawnTransform;

    // UI pass references
    [SerializeField] private Slider _sliderLeft;
    [SerializeField] private Image _fillLeft;

    [SerializeField] private Slider _sliderRight;
    [SerializeField] private Image _fillRight;

    private void Start()
    {
        // Create two player instances form prefabs
        GameObject leftPlayer = Instantiate(_universalPlayerPrefabs, _leftSpawnTransform);
        GameObject rightPlayer = Instantiate(_universalPlayerPrefabs, _rightSpawnTransform);
        
        // Flip the right player
        Vector3 rightScale = rightPlayer.transform.localScale;
        rightScale.x *= -1;
        rightPlayer.transform.localScale = rightScale;

        // Get necessary components
        Health lefthealth = leftPlayer.GetComponent<Health>();
        PlayerController leftMovement = leftPlayer.GetComponent<PlayerController>();
        FajtovPlayerAnimator leftAnimator = leftPlayer.GetComponent<FajtovPlayerAnimator>();
        Health righthealth = rightPlayer.GetComponent<Health>();
        PlayerController rightMovement = rightPlayer.GetComponent<PlayerController>();
        FajtovPlayerAnimator rightAnimator = rightPlayer.GetComponent<FajtovPlayerAnimator>();

        // Set the health bar references
        lefthealth.Initialize(_sliderLeft, _fillLeft, GlobalState.Player1Character.health);
        righthealth.Initialize(_sliderRight, _fillRight, GlobalState.Player2Character.health);

        // Initialize the characters
        leftMovement.Initialize(GlobalState.Player.one, true, LayerMask.GetMask("Player2"));
        rightMovement.Initialize(GlobalState.Player.two, false, LayerMask.GetMask("Player1"));

        // Initialize the animators
        leftAnimator.Initialize(GlobalState.Player1Character);
        leftAnimator.Initialize(GlobalState.Player2Character);

        // Set the tags and layers of players
        leftPlayer.layer = LayerMask.NameToLayer("Player1");
        leftPlayer.tag = "Player1";
        rightPlayer.layer = LayerMask.NameToLayer("Player2");
        rightPlayer.tag = "Player2";

        // Make second player in front of the first on to reduce z-fighting
        rightPlayer.GetComponent<SpriteRenderer>().sortingOrder = 3;

        // Set targets to MultipleTargetCamera
        MultipleTargetCamera multipleTargetCamera = Camera.main.GetComponent<MultipleTargetCamera>();
        multipleTargetCamera.targets.Add(leftPlayer.transform);
        multipleTargetCamera.targets.Add(rightPlayer.transform);
    }

}
