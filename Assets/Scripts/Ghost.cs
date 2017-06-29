using UnityEngine;
using System.Collections;

public class Ghost : MonoBehaviour
{

    public GameObject player;
    public GameObject exclamation;

    //マテリアル
    public Material materialGround;
    public Material materialSky;
    public Material materialThrough;

    public Texture[] face;

    //コンポーネント取得
    private NavMeshAgent agent;
    private Rigidbody rigid;

    [SerializeField]
    private int actionType = 0;
    [SerializeField]
    private int section = 0;
    [SerializeField]
    private int mode = 0;
    private Vector3 destination;

    //skyで使用する変数
    private float minHeight;
    private bool hasPath;
    private Vector3 rayHitPosition;
    private float rayY = 40;
    private Vector3 moveDirection;

    private int searchRange;
    private int chaseRange;
    private int searchAngle;

    public enum Mode
    {
        Search,
        Chase,
    }

    public enum ActionType
    {
        Ground,
        Sky,
        Through,
    }

    public enum Section
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
    }

    // Use this for initialization
    void Start()
    {

        player = GameObject.Find("Player_Query-Chan");

        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();

        mode = (int)Mode.Search;
        searchRange = 10;
        chaseRange = 30;
        searchAngle = 140 / 2;
        hasPath = false;

        exclamation.SetActive(false);

        switch (actionType)
        {
            case (int)ActionType.Ground:
                GetComponent<Renderer>().material = materialGround;
                rigid.isKinematic = true;
                agent.enabled = true;
                break;
            case (int)ActionType.Sky:
                GetComponent<Renderer>().material = materialSky;
                rigid.isKinematic = false;
                agent.enabled = false;
                searchRange = 20;
                chaseRange = 50;
                searchAngle = 170 / 2;
                break;
            case (int)ActionType.Through:
                GetComponent<Renderer>().material = materialThrough;
                rigid.isKinematic = false;
                agent.enabled = false; mode = (int)Mode.Chase;
                break;
        }

        GetComponent<Renderer>().material.SetTexture("_Face_texture", face[Random.Range(0, face.Length)]);
        //GetComponent<Renderer>().material.SetColor("_in_Color", new Color(1, 0, 0));

        
    }

    // Update is called once per frame
    void Update()
    {

        switch (mode)
        {
            case (int)Mode.Search: modeSearch(); break;
            case (int)Mode.Chase: modeChase(); break;
        }

    }

    //ゴーストの目的地を設定
    private Vector3 setDestination()
    {
        Vector3 tmp = new Vector3(0, 0, 0);

        //Groundの場合
        if (actionType == (int)ActionType.Ground)
        {
            tmp.y = 0.5f;

            switch ((int)section)
            {

                case (int)Section.A: tmp.x = Random.Range(90f, 135f); tmp.z = Random.Range(0f, 37f); break;
                case (int)Section.B: tmp.x = Random.Range(90f, 135f); tmp.z = Random.Range(37f, 74f); break;
                case (int)Section.C: tmp.x = Random.Range(90f, 135f); tmp.z = Random.Range(74f, 110f); break;
                case (int)Section.D: tmp.x = Random.Range(45f, 90f); tmp.z = Random.Range(0f, 37f); break;
                case (int)Section.E: tmp.x = Random.Range(45f, 90f); tmp.z = Random.Range(37f, 74f); break;
                case (int)Section.F: tmp.x = Random.Range(45f, 90f); tmp.z = Random.Range(74f, 110f); break;
                case (int)Section.G: tmp.x = Random.Range(0f, 45f); tmp.z = Random.Range(0f, 37f); break;
                case (int)Section.H: tmp.x = Random.Range(0f, 45f); tmp.z = Random.Range(45f, 74f); break;
                case (int)Section.I: tmp.x = Random.Range(0f, 45f); tmp.z = Random.Range(74f, 110f); break;
            }

        }

        //Skyの場合
        else if (actionType == (int)ActionType.Sky)
        {
            bool breakFlag = true;
            do
            {
                switch ((int)section)
                {
                    case (int)Section.A: tmp.x = Random.Range(90f, 135f); tmp.z = Random.Range(0f, 37f); break;
                    case (int)Section.B: tmp.x = Random.Range(90f, 135f); tmp.z = Random.Range(37f, 74f); break;
                    case (int)Section.C: tmp.x = Random.Range(90f, 135f); tmp.z = Random.Range(74f, 110f); break;
                    case (int)Section.D: tmp.x = Random.Range(45f, 90f); tmp.z = Random.Range(0f, 37f); break;
                    case (int)Section.E: tmp.x = Random.Range(45f, 90f); tmp.z = Random.Range(37f, 74f); break;
                    case (int)Section.F: tmp.x = Random.Range(45f, 90f); tmp.z = Random.Range(74f, 110f); break;
                    case (int)Section.G: tmp.x = Random.Range(0f, 45f); tmp.z = Random.Range(0f, 37f); break;
                    case (int)Section.H: tmp.x = Random.Range(0f, 45f); tmp.z = Random.Range(45f, 74f); break;
                    case (int)Section.I: tmp.x = Random.Range(0f, 45f); tmp.z = Random.Range(74f, 110f); break;
                }

                //設定した地点が高い場所かを判別
                RaycastHit hitInfo;
                Physics.Linecast(new Vector3(tmp.x, 20, tmp.z), new Vector3(tmp.x, 0, tmp.z), out hitInfo);
                if (hitInfo.distance < 17)
                {
                    breakFlag = false;
                    tmp.y = 20 - hitInfo.distance;
                    transform.LookAt(tmp);
                    transform.rotation = Quaternion.LookRotation(Vector3.Scale(transform.forward, new Vector3(1, 0, 1))); 

                    
                }

            } while (breakFlag);

        }


        return tmp;
    }

    //索敵モード時の動作
    private void modeSearch()
    {
        //索敵時の移動処理
        if (actionType == (int)ActionType.Ground)
        {
            if (agent.hasPath == false)
            {
                //目的地へagentで移動
                destination = setDestination();
                NavMeshHit hit;
                if(agent.Raycast(destination, out hit) == false)
                    agent.SetDestination(destination);
            }
        }

        else if (actionType == (int)ActionType.Sky)
        {

            //パスの判定
            if (hasPath == false)
            {
                destination = setDestination();
                hasPath = true;
            }
            if (Vector3.Scale((transform.position - destination), new Vector3(1, 0, 1)).sqrMagnitude < 0.2)
                hasPath = false;

            //Raycastを用いた移動Y座標計算
            RaycastHit ghostRay;
            RaycastHit frontRay;

            rayHitPosition = new Vector3(transform.position.x, rayY, transform.position.z) + 2  * Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
            Physics.Raycast(transform.position, new Vector3(0, -1, 0), out ghostRay);
            Physics.Raycast(rayHitPosition, new Vector3(0, -1, 0), out frontRay);

            //一定以上の高度に障害物があれば
            if (frontRay.distance < rayY - 3)
            {
                rayHitPosition -= new Vector3(0, frontRay.distance - 1, 0); 
                //ゴーストの真下に障害物がなければ
                if (ghostRay.distance > 1)
                {
                    iTween.LookUpdate(gameObject, rayHitPosition, 0.5f);
                }
                else
                {
                    transform.rotation = Quaternion.LookRotation(Vector3.Scale(destination - transform.position, new Vector3(1, 0, 1)));
                }
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(Vector3.Scale(destination - transform.position, new Vector3(1, 0, 1)));
            }

            
            rigid.velocity = transform.forward * agent.speed * 2.7f;
            
            

        }
        

        //プレイヤーが視界に入るかチェックする
        if ((transform.position - player.transform.position).sqrMagnitude < searchRange && Vector3.Angle(player.transform.position - transform.position, transform.forward) <= searchAngle)
        {

            if (Physics.Linecast(transform.position, player.transform.position))
            {
                if (actionType != (int)ActionType.Sky || (actionType == (int)ActionType.Sky && (player.transform.position.y > 3 || player.GetComponent<Player3D>().isFly == true)))
                {
                    //追跡モードに移行
                    mode = (int)Mode.Chase;
                    if (actionType == (int)ActionType.Ground)
                        agent.ResetPath();
                    StartCoroutine("animateExclamation");
                    agent.speed *= 2f;
                    agent.autoBraking = false;
                    hasPath = false;
                    GetComponent<AudioSource>().Play();
                    //Debug.Log("chase");
                }

            }
        }

        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log((transform.position - player.transform.position).sqrMagnitude);
            Debug.Log(player.transform.position);
        }*/

    }

    //追跡モード時の動作
    private void modeChase()
    {
        //プレイヤーを追跡
        if (actionType == (int)ActionType.Ground)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (actionType == (int)ActionType.Sky)
        {
            transform.LookAt(player.transform.position);
            if (player.GetComponent<Player3D>().isFly == true)
                rigid.velocity = transform.forward * agent.speed * 1.8f;
            else
                rigid.velocity = transform.forward * agent.speed * 1f;

        }
        else if (actionType == (int)ActionType.Through)
        {
            transform.LookAt(player.transform.position);
            if (player.GetComponent<Player3D>().isFly == true)
                rigid.velocity = transform.forward * agent.speed * 2.3f;
            else
                rigid.velocity = transform.forward * agent.speed * 1f;
        }

        //プレイヤーが範囲外まで離れるかチェックする(Skyはプレイヤーが着地した場合も)
        if ((((transform.position - player.transform.position).sqrMagnitude > chaseRange) && actionType != (int)ActionType.Through) ||(player.GetComponent<Player3D>().isFly == false && player.transform.position.y < 3 && actionType == (int)ActionType.Sky))
        {
            //索敵モードに移行
            mode = (int)Mode.Search;
            if (actionType == (int)ActionType.Ground)
                agent.ResetPath();
            agent.speed *= 0.5f;
            agent.autoBraking = true;
        }

    }

    public void setActionType(int num)
    {
        actionType = num;
    }

    public void setSection(int num)
    {
        section = num;
    }

    //プレイヤー発見時の！マークのアニメーション
    private IEnumerator animateExclamation()
    {
        exclamation.SetActive(true);
        iTween.ScaleFrom(exclamation, iTween.Hash("y", 0));
        iTween.ScaleTo(exclamation, iTween.Hash("y", 0.15, "time", 0.5f));
        StartCoroutine("rotateExclamation");
        yield return new WaitForSeconds(3.0f);
        StopCoroutine("rotateExclamation");
        exclamation.SetActive(false);

        yield return null;
    }

    //！マークを2D表現に
    private IEnumerator rotateExclamation()
    {
        while (true)
        {
            exclamation.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            yield return null;

        }

    }
}
