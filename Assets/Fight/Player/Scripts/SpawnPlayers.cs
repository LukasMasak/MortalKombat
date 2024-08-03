using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayers : MonoBehaviour
{
    // TODO obselete
    [SerializeField] private GameObject[] _playerPrefabs;
    
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
        var leftPlayer = Instantiate(_playerPrefabs[(int)GlobalState.Player1Character], _leftSpawnTransform);
        var rightPlayer = Instantiate(_playerPrefabs[(int)GlobalState.Player2Character], _rightSpawnTransform);
        
        // Flip the left player
        Vector3 _leftScale = leftPlayer.transform.localScale;
        _leftScale.x *= -1;
        leftPlayer.transform.localScale = _leftScale;

        // Get necessary components
        var lefthealth = leftPlayer.GetComponent<Health>();
        var leftMovement = leftPlayer.GetComponent<PlayerController>();
        var righthealth = rightPlayer.GetComponent<Health>();
        var rightMovement = rightPlayer.GetComponent<PlayerController>();

        // Set the health bar references
        lefthealth.slider = _sliderLeft;
        lefthealth.fill = _fillLeft;
        righthealth.slider = _sliderRight;
        righthealth.fill = _fillRight;

        // Initialize the characters
        leftMovement.Initialize(GlobalState.Player.one, true, LayerMask.GetMask("Player2"));
        rightMovement.Initialize(GlobalState.Player.two, false, LayerMask.GetMask("Player1"));

        // Set the tags and layers of players
        leftPlayer.layer = LayerMask.NameToLayer("Player1");
        leftPlayer.tag = "Player1";
        rightPlayer.layer = LayerMask.NameToLayer("Player2");
        rightPlayer.tag = "Player2";


        // Set targets to MultipleTargetCamera
        MultipleTargetCamera multipleTargetCamera = Camera.main.GetComponent<MultipleTargetCamera>();
        multipleTargetCamera.targets.Add(leftPlayer.transform);
        multipleTargetCamera.targets.Add(rightPlayer.transform);


    }

}
