using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player3D_Sightseeing : MonoBehaviour
{

    float moveSpeed = 1.3f;
    float dashSpeed = 2.6f;
    float rotationSpeed = 400f;

    float jumpPower = 5f;
    float jumpInterval = 1.3f;
    float gravity = 14f;

    float maxRotation = 20f;

    private Quaternion currentDirection;
    private float pastDirectionY;


    QuerySoundController.QueryChanSoundType[] jumpSounds = {
        QuerySoundController.QueryChanSoundType.ONE_TWO,
        QuerySoundController.QueryChanSoundType.GO_AHEAD,
    };

    //--------------------------

    CharacterController controller;
    Animator animator;
    QuerySoundController querySound;
    QueryMechanimController queryMechanim;

    //--------------------------

    Vector3 moveDirection = Vector3.zero;
    float nextAllowedJumpTime = 0;



    //========================================================

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        querySound = GetComponent<QuerySoundController>();
        queryMechanim = GetComponent<QueryMechanimController>();

        queryMechanim.ChangeAnimation(QueryMechanimController.QueryChanAnimationType.IDLE, false);

        pastDirectionY = 0;
    }


    //=========================================================

    void Update()
    {
        updateMove();
    }


    void updateMove()
    {
        //float inputX = Input.GetAxis("Horizontal");
        //float inputY = Input.GetAxis("Vertical");
        float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
        float inputY = CrossPlatformInputManager.GetAxis("Vertical");
        bool dash = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        bool inputJump = CrossPlatformInputManager.GetButton("Jump");
        bool jumped = false;


        // movement -----------------------------------


        //moveDirection = new Vector3(inputX, 0, inputY);
        //moveDirection = transform.TransformDirection(moveDirection);
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        moveDirection = cameraForward * inputY + Camera.main.transform.right * inputX;
        if (moveDirection.magnitude > 1)
            moveDirection = moveDirection.normalized;
        if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0)
            if (moveDirection != Vector3.zero)
                currentDirection = Quaternion.LookRotation(moveDirection);


        if (dash)
        {
            moveDirection *= dashSpeed;
        }
        else
        {
            moveDirection *= moveSpeed;
        }

        moveDirection.y = pastDirectionY;

        if (controller.isGrounded)
        {

            if (inputJump && animator.IsInTransition(0) == false && nextAllowedJumpTime <= Time.time)
            {
                moveDirection.y = jumpPower;
                jumped = true;
                nextAllowedJumpTime = Time.time + jumpInterval;

                PlayJumpSound();
            }

        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        pastDirectionY = moveDirection.y;

        controller.Move(moveDirection * Time.deltaTime);
        if (transform.rotation.y != currentDirection.y)
        {
            float rotate = ((currentDirection.eulerAngles.y) - transform.rotation.eulerAngles.y);

            if (rotate > 180)
                rotate -= 360;
            else if (rotate < -180)
                rotate += 360;

            //Debug.Log(rotate);



            if (rotate > maxRotation)
                rotate = maxRotation;
            else if (rotate < maxRotation * (-1))
                rotate = maxRotation * (-1);

            if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0)
                transform.Rotate(new Vector3(0, rotate, 0));

        }


        //transform.Rotate(new Vector3(0, inputX * rotationSpeed * Time.deltaTime, 0));
        //transform.rotation = currentDirection;


        //animation ---------------------------------------

        animator.SetBool("Jump", jumped);

        if (controller.isGrounded && jumped == false && animator.IsInTransition(0) == false)
        {
            if (CrossPlatformInputManager.GetAxis("Horizontal") != 0 || CrossPlatformInputManager.GetAxis("Vertical") != 0)
                animator.SetFloat("Speed", new Vector2(moveDirection.x, moveDirection.z).magnitude * (dash ? 2 : 1));
            else
                animator.SetFloat("Speed", 0);
            //Debug.Log(new Vector2(moveDirection.x, moveDirection.z).magnitude);
        }


    }

    //=====================================================================

    void PlayJumpSound()
    {
        PlaySound(jumpSounds[Random.Range(0, jumpSounds.Length)]);
    }

    void PlaySound(QuerySoundController.QueryChanSoundType soundType)
    {
        querySound.PlaySoundByType(soundType);
    }


}

