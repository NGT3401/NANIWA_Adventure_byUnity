using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GhostManager : MonoBehaviour
{

    public GameObject ghost;

    private int addCount;

    private GameObject groundGhostPosition;
    private List<Transform> groundGhostPositionList = new List<Transform>();
    private GameObject skyGhostPosition;
    private List<Transform> skyGhostPositionList = new List<Transform>();
    private GameObject throughGhostPosition;
    private List<Transform> throughGhostPositionList = new List<Transform>();

    // Use this for initialization
    void Start()
    {

        groundGhostPosition = transform.FindChild("GroundGhostPosition").gameObject;
        skyGhostPosition = transform.FindChild("SkyGhostPosition").gameObject;
        throughGhostPosition = transform.FindChild("ThroughGhostPosition").gameObject;

        setGhost((int)Ghost.ActionType.Ground, 2);
        setGhost((int)Ghost.ActionType.Sky, 2);

        addCount = 0;

    }

    //取得したSourceに応じてゴースト追加
    public void addGhost(int currentNum, int maxNum)
    {
        float per = (float)currentNum / (float)maxNum;

        if (per <= 0.75 && addCount == 0)
        {
            addCount++;
            setGhost((int)Ghost.ActionType.Ground, 1);
            setGhost((int)Ghost.ActionType.Sky, 1);

        }

        else if (per <= 0.5 && addCount == 1)
        {
            addCount++;
            setGhost((int)Ghost.ActionType.Ground, 1);
            setGhost((int)Ghost.ActionType.Sky, 1);
            setGhost((int)Ghost.ActionType.Through, 1);

        }
        else if (per <= 0.25 && addCount == 2)
        {
            addCount++;
            setGhost((int)Ghost.ActionType.Ground, 1);
            setGhost((int)Ghost.ActionType.Sky, 1);
            setGhost((int)Ghost.ActionType.Through, 1);

        }

    }


    //ゴーストの配置
    public void setGhost(int actionType, int number)
    {
        GameObject obj = new GameObject();
        List<Transform> list = new List<Transform>();

        switch (actionType)
        {
            case (int)Ghost.ActionType.Ground: obj = groundGhostPosition; list = groundGhostPositionList; break;
            case (int)Ghost.ActionType.Sky: obj = skyGhostPosition; list = skyGhostPositionList; break;
            case (int)Ghost.ActionType.Through: obj = throughGhostPosition; list = throughGhostPositionList; break;
        }

        //子オブジェクト全取得
        list.AddRange(obj.GetComponentsInChildren<Transform>());
        list.RemoveAt(0);

        //Ghost配置
        while (list.Count > 0)
        {
            int num = Random.Range(0, list.Count);
            for (int i = 0; i < number; i++)
                createGhost(ghost, list[num].position, new Quaternion(0, 0, 0, 0), actionType, referSection(list[num].position));
            list.RemoveAt(num);
        }
 
        //リストクリア
        list.Clear();
     

        /*//子オブジェクトのHaloを非アクティブに
        foreach (Transform n in groundGhostPosition.transform)
        {
            GameObject.Destroy(n.gameObject);
        }
        foreach (Transform n in skyGhostPosition.transform)
        {
            GameObject.Destroy(n.gameObject);
        }*/


    }

    //ゴーストの生成・設定
    private void createGhost(GameObject ghost, Vector3 position, Quaternion rotation, int actionType, int section)
    {
        GameObject tmp = (GameObject)Instantiate(ghost, position, rotation);
        tmp.GetComponent<Ghost>().setActionType(actionType);
        tmp.GetComponent<Ghost>().setSection(section);
        
    }

    //GhostPositionの座標を元にゴーストのセクション値を返す
    private int referSection(Vector3 position)
    {
        float x = position.x;
        float z = position.z;
        int returnSection = -1;

        if ((0 <= x && x < 45) && (0 <= z && z < 37))
            returnSection = (int)Ghost.Section.G;
        else if ((0 <= x && x < 45) && (37 <= z && z < 74))
            returnSection = (int)Ghost.Section.H;
        else if ((0 <= x && x < 45) && (74 <= z && z < 110))
            returnSection = (int)Ghost.Section.I;
        else if ((45 <= x && x < 90) && (0 <= z && z < 37))
            returnSection = (int)Ghost.Section.D;
        else if ((45 <= x && x < 90) && (37 <= z && z < 74))
            returnSection = (int)Ghost.Section.E;
        else if ((45 <= x && x < 90) && (74 <= z && z < 110))
            returnSection = (int)Ghost.Section.F;
        else if ((90 <= x && x < 135) && (0 <= z && z < 37))
            returnSection = (int)Ghost.Section.A;
        else if ((90 <= x && x < 135) && (37 <= z && z < 74))
            returnSection = (int)Ghost.Section.B;
        else if ((90 <= x && x < 135) && (74 <= z && z < 110))
            returnSection = (int)Ghost.Section.C;

        return returnSection;
    }
}
