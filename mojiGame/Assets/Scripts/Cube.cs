using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    public bool isMovable;
    public bool isCreatedWord { get; protected set; }

    [System.NonSerialized]
    public Vector2Int Pos;
    
    [System.NonSerialized]
    public char blockChar;

    private InputManager inputMan;

    private void OnEnable()
    {
        Pos = new Vector2Int(4, 0);
        inputMan = InputManager.Instance;
        gameObject.transform.position = new Vector3(Pos.x, (20 - Pos.y), 0);
        isMovable = true;
        isCreatedWord = false;
        SetChar(true);
    }

    private void Update()
    {
        Move();
    }

    private void UpdatePos()
    {
        gameObject.transform.position = new Vector3(Pos.x, (20 - Pos.y), 0);
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
                    ret = 'ぁ';
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
            if (inputMan.BtnLeftDown && GameManager.Instance.isBlockByPos(-1, 0))
            {
                Pos.x--;
                UpdatePos();
            }
            else if (inputMan.BtnRightDown && GameManager.Instance.isBlockByPos(1, 0))
            {
                Pos.x++;
                UpdatePos();
            }
            else if (inputMan.BtnDownDown && GameManager.Instance.isBlockByPos())
            {
                Pos.y++;
                UpdatePos();
            }
        }
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
    /// Textの色を決める
    /// </summary>
    public void OnCreatedWord()
    {
        isCreatedWord = true;
        text.color = Color.red;
    }
}
