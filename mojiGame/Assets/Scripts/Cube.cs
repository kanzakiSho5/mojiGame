using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject Effect;
    
    [SerializeField]
    public bool isMovable;
    public bool isCreatedWord { get; protected set; }


    [SerializeField]
    public Vector2Int Pos;
    
    [System.NonSerialized]
    public char blockChar;

    private InputManager inputMan;
    private CubeManager cubeMan;
    private const float DeleteTime = 20.0f;
    private float FixedTime = 0;

    public delegate void FixedEnter();
    public FixedEnter OnFixedEnter;

    private void OnEnable()
    {
        Pos = new Vector2Int(3, 4);
        inputMan = InputManager.Instance;
        cubeMan = CubeManager.Instance;
        gameObject.transform.localPosition = new Vector3(Pos.x, (20 - Pos.y), Random.Range(0f, .7f));
        isMovable = true;
        isCreatedWord = false;
        SetChar(true);
        OnFixedEnter += FixedBlock;
        OnFixedEnter += FixedEffects;
    }

    private void Update()
    {
        Move();
        if (!isMovable && !isCreatedWord)
            DeleteBlock();
    }

    private void DeleteBlock()
    {
        if (cubeMan.currentTime - FixedTime >= DeleteTime)
            cubeMan.DestroyBlockByPos(Pos);
    }

    private void UpdatePos()
    {
        gameObject.transform.localPosition = new Vector3(Pos.x, (20 - Pos.y), this.transform.localPosition.z);
    }

    /// <summary>
    /// ブロックの文字を設定
    /// </summary>
    /// <param name="DropItem">trueで"メメタァクパパン"のみ選ばれる</param>
    public void SetChar(bool DropItem = false)
    {
        if(DropItem)
        {
            int rand = Random.Range(0, 6);
            char ret;
            switch(rand)
            {
                case 0:
                    ret = 'め';
                    break;
                case 1:
                    ret = 'た';
                    break;
                case 2:
                    ret = 'あ';
                    break;
                case 3:
                    ret = 'く';
                    break;
                case 4:
                    ret = 'ぱ';
                    break;
                case 5:
                    ret = 'ん';
                    break;
                default:
                    ret = 'め';
                    break;
            }

            blockChar = ret;
        }
        else
        {
            int rand = Random.Range(12353, 12436);
            blockChar = (char)rand;
        }

        if (text != null)
            text.SetText(blockChar.ToString());
    }
    
    private void Move()
    {
        if(isMovable)
        {
            if (inputMan.BtnLeftDown && cubeMan.isBlockByPos(-1, 0))
            {
                Pos.x--;
                UpdatePos();
            }
            else if (inputMan.BtnRightDown && cubeMan.isBlockByPos(1, 0))
            {
                Pos.x++;
                UpdatePos();
            }
            else if (inputMan.BtnDownDown && cubeMan.isBlockByPos())
            {
                Pos.y++;
                UpdatePos();
            }
            else if (inputMan.BtnUpDown && cubeMan.isBlockByPos())
            {
                print("Harddrop");
                StartCoroutine(HardDrop());
            }
        }
    }

    /// <summary>
    /// 動かないようにする。
    /// </summary>
    private void FixedBlock()
    {
        print("FixedBlock");
        isMovable = false;
        FixedTime = cubeMan.currentTime;
    }

    private void FixedEffects()
    {
        StartCoroutine(FixedEffect());
    }

    IEnumerator HardDrop()
    {
        while(cubeMan.isBlockByPos())
        {
            Pos.y++;
            UpdatePos();
            yield return new WaitForEndOfFrame();
        }
        print("EndHardDrop");
        cubeMan.FixedCurrentCube();
    }

    IEnumerator FixedEffect()
    {
        GameObject obj = Instantiate<GameObject>(Effect, transform.position, Quaternion.identity, this.transform);
        yield return new WaitForSeconds(1f);
        Destroy(obj);
    }


    /// <summary>
    /// 1ブロック分下げる
    /// </summary>
    public void DropBlock()
    {
        Pos.y++;
        UpdatePos();
    }

    /// <summary>
    /// ブロックが文字ができた時
    /// </summary>
    /// <returns><c>true</c>, if word was createded, <c>false</c> otherwise.</returns>
    public bool CreatedWord()
    {
        if (isCreatedWord)
            return false;

        isCreatedWord = true;
        text.color = Color.red;

        return true;
    }
}
