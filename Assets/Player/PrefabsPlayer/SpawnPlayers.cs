using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject[] _prefs;
    
    public GameObject LeftSpawn;
    public GameObject RightSpawn;
    [Space]
    public Camera cam;
    public Slider sliderLeft;
    public Image fillLeft;

    public Slider sliderRight;
    public Image fillRight;

    public MultipleTargetCamera targetCam;


    private void Awake()
    {
        var Left = Instantiate(_prefs[(int)GlobalState.Player1Character], LeftSpawn.transform);
        var Right = Instantiate(_prefs[(int)GlobalState.Player2Character], RightSpawn.transform);
        Vector3 _left = Left.transform.localScale;
        _left.x *= -1;
        Left.transform.localScale = _left;

        var LeftSprite = Left.GetComponent<SpriteRenderer>();
        var lefthealth = Left.GetComponent<Health>();
        var leftMovement = Left.GetComponent<PlayerMovement>();

        var RightSprite = Right.GetComponent<SpriteRenderer>();
        var righthealth = Right.GetComponent<Health>();
        var rightMovement = Right.GetComponent<PlayerMovement>();

    
        lefthealth.slider = sliderLeft;
        lefthealth.fill = fillLeft;
        leftMovement.playerCamera = cam;

        righthealth.slider = sliderRight;
        righthealth.fill = fillRight;
        rightMovement.playerCamera = cam;

        targetCam.targets[0] = Left.transform;
        targetCam.targets[1] = Right.transform;

        //RightSprite.flipX = false;

        rightMovement.playerRight = true;
        Left.layer = LayerMask.NameToLayer("Player1");
        Left.tag = "Player1";
        Right.layer = LayerMask.NameToLayer("Player2");
        Right.tag = "Player2";

        rightMovement.enemyMask = LayerMask.GetMask("Player1");
        leftMovement.enemyMask = LayerMask.GetMask("Player2");

        leftMovement.OnEnable2();
        rightMovement.OnEnable2();

        rightMovement.facingRight = false;

    }

}
