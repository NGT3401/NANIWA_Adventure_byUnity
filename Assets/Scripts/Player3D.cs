using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Player3D : MonoBehaviour
{

    [SerializeField]float moveSpeed;
    [SerializeField]float flySpeed;
    float rotationSpeed = 400f;

    [SerializeField]float jumpPower;
    float jumpInterval = 1.3f;
    [SerializeField]float gravity;

    float maxRotation = 20f;

    private Quaternion currentDirection;
    private float jumpingSpeedY = 0; //ジャンプ中のY軸速度(重力の設定)

    private GameObject body;
    private GameObject jumpEffect;
    private GameObject flyingEffect;
    private GameObject locusEffect;

    private bool dashAnimation;
    public bool isFly = false;
    private bool shiftingFly = false;

    private Vector3 cameraForward;

    private bool hitEnemyFlag = false;
    private bool isPlayable = true;

    bool jumped = false; //ジャンプした瞬間trueになる

    [SerializeField]bool debugClear = false;

    //private NavMeshAgent agent;
    
    //入力データ
    //前後左右の移動
    private float inputX;
    private float inputY;
    //ダッシュ(たぶん使わない)
    private bool dash;
    //ジャンプ
    private bool inputJump;


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

        body = transform.FindChild("BodyParts").gameObject;
        jumpEffect = transform.FindChild("JumpEffect").gameObject;
        flyingEffect = transform.FindChild("FlyingEffect").gameObject;
        locusEffect = transform.FindChild("LocusEffect").gameObject;
        flyingEffect.GetComponent<ParticleSystem>().Stop();
        locusEffect.GetComponent<ParticleSystem>().Stop();

        //agent = GetComponent<NavMeshAgent>();

     
    }


    //=========================================================

    void Update()
    {
        if (isPlayable == true)
        {
            playerInput();

            //カメラの向いている方向をy軸要素を抜いて取得
            cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

            body.transform.localPosition = new Vector3(0, 0.65f, 0);

            if (isFly == false)
                updateMoveGround();
            else
                updateMoveSky();

            GetComponent<ClampPosition>().Clamp();

            playerAnimation();
        }

        if (debugClear == true)
        {
            GameObject.Find("GameClear").GetComponent<GameClear>().ActGameClear();
            locusEffect.GetComponent<ParticleSystem>().Stop();
            animator.SetTrigger("ShiftingGround");
            isPlayable = false;
            debugClear = false;
        }
    }

    void playerInput()
    {


        inputX = CrossPlatformInputManager.GetAxis("Horizontal");
        inputY = CrossPlatformInputManager.GetAxis("Vertical");
        dash = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        inputJump = CrossPlatformInputManager.GetButton("Jump");

    }

    void playerAnimation()
    {
        animator.SetFloat("Speed", new Vector2(moveDirection.x, moveDirection.z).magnitude);
        animator.SetBool("Ground", controller.isGrounded);
        animator.SetBool("Jump", jumped);
        animator.SetFloat("YSpeed", moveDirection.y);
    }


    void updateMoveGround()
    {

        jumped = false;

        //カメラの向きを基準とした移動方向
        moveDirection = cameraForward * inputY + Camera.main.transform.right * inputX;
        if(moveDirection.magnitude > 1)
            moveDirection = moveDirection.normalized;

        if (inputX != 0 || inputY != 0)
            if (moveDirection != Vector3.zero)
                currentDirection = Quaternion.LookRotation(moveDirection);


        moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {

            if (inputJump && animator.IsInTransition(0) == false && nextAllowedJumpTime <= Time.time)
            {
                jumpingSpeedY = jumpPower;
                jumped = true;
                nextAllowedJumpTime = Time.time + jumpInterval;

                //PlayJumpSound();
                GetComponents<AudioSource>()[2].Play();
            }
            else
            {
                jumpingSpeedY = 0;
                isFly = false;
            }

        }
        else
        {
            jumpingSpeedY -= gravity * Time.deltaTime;
        }

        if (jumpingSpeedY < -10)
            jumpingSpeedY = -10;

        moveDirection.y = jumpingSpeedY;

        controller.Move(moveDirection * Time.deltaTime);
        //agent.Move(moveDirection * Time.deltaTime);

        //プレイヤーの回転処理

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

        //エフェクト
        if (jumped == true)
            jumpEffect.GetComponent<ParticleSystem>().Play();
        else
            jumpEffect.GetComponent<ParticleSystem>().Stop();


    }

    void updateMoveSky()
    {
        if (shiftingFly == true)
        {
            transform.rotation = Quaternion.LookRotation(cameraForward);
            //こっちはなんかうまくいかない
            //transform.rotation.SetLookRotation(cameraForward);

            jumpingSpeedY -= gravity * Time.deltaTime;

            if (jumpingSpeedY < 0)
            {
                jumpingSpeedY = 0;
                shiftingFly = false;
                locusEffect.GetComponent<ParticleSystem>().Play();
                GetComponents<AudioSource>()[0].Play();
            }

            moveDirection = new Vector3(0, jumpingSpeedY, 0);
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            transform.rotation = Camera.main.transform.rotation;
            moveDirection = transform.forward * flySpeed;
            controller.Move(moveDirection * Time.deltaTime);
            flySpeed -= transform.forward.y * 0.8f * Time.deltaTime;
            if (flySpeed < 0)
                flySpeed = 0;

            //locusEffect.GetComponent<ParticleSystem>().startSpeed = flySpeed* 0.2f;
            //locusEffect.GetComponent<ParticleSystem>().transform.rotation = Quaternion.LookRotation(Camera.main.transform.rotation * new Vector3(0, 0, -1));

            if (controller.isGrounded)
            {
                isFly = false;
                transform.rotation = Quaternion.LookRotation(cameraForward);
                flyingEffect.GetComponent<ParticleSystem>().Stop();
                locusEffect.GetComponent<ParticleSystem>().Stop();
                GetComponents<AudioSource>()[0].Stop();
                jumpEffect.GetComponent<ParticleSystem>().Play();
                animator.SetTrigger("ShiftingGround");

            }
        }

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Ghost"))
        {
            if (hitEnemyFlag == false && isPlayable == true)
            {
                hitEnemyFlag = true;
                animator.SetTrigger("Hit");
                isPlayable = false;
                //ゲームオーバー
                GameObject.Find("GameOver").GetComponent<GameOver>().ActGameOver();
                locusEffect.GetComponent<ParticleSystem>().Stop();
                
            }

        }
        if (c.CompareTag("Source"))
        {
            SourceManager com = c.GetComponent<Source>().sourceManager.GetComponent<SourceManager>();
            com.sourceCount--;
            GetComponents<AudioSource>()[3].Play();
            GameObject go = c.gameObject.transform.FindChild("GetEffect").gameObject;
            go = (GameObject)Instantiate(go, go.transform.position, go.transform.rotation);
            go.GetComponent<ParticleSystem>().Play();
            Destroy(c.gameObject);
            GameObject.Find("GhostManager").GetComponent<GhostManager>().addGhost(com.sourceCount, com.sourceSetting);
            if (com.sourceCount == 0)
            {
                //ゲームクリア
                GameObject.Find("GameClear").GetComponent<GameClear>().ActGameClear();
                locusEffect.GetComponent<ParticleSystem>().Stop();
                animator.SetTrigger("ShiftingGround");
                isPlayable = false;
            }
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (c.CompareTag("FlyableArea"))
        {
            if (inputJump == true && isFly == false)
            {
                isFly = true;
                shiftingFly = true;
                jumped = true;
                animator.SetTrigger("ShiftingFly");
                flyingEffect.GetComponent<ParticleSystem>().Play();
                GetComponents<AudioSource>()[1].Play();
                jumpingSpeedY = 15;
                flySpeed = 3;
                moveDirection.y = jumpingSpeedY;
            }

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

