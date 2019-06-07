using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    private bool isMovable;

    public bool isFixed { get; protected set; }

    [System.NonSerialized]
    public Vector2Int Pos;
    
    [System.NonSerialized]
    public char blockChar;

    private InputManager inputMan;
    private GameManager gameMan;
    private const float DeleteTime = 5.0f;
    private float FixedTime = 0;

    private void OnEnable()
    {
        isFixed = false;
        // TODO: Positionを[1,0]が原点になっているのを[0,0]に変更
        Pos = new Vector2Int(4, 0);
        inputMan = InputManager.Instance;
        gameMan = GameManager.Instance;
        gameObject.transform.position = new Vector3(Pos.x, (20 - Pos.y), 0);
        isMovable = true;
        SetChar(true);
    }

    private void Update()
    {
        Move();
        if (!isMovable && !isFixed)
            DeleteBlock();
    }

    private void DeleteBlock()
    {
        if (gameMan.currentTime - FixedTime >= DeleteTime)
            gameMan.DestroyBlockByPos(Pos);

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
            int rand = Random.Range(0, 8);
            char ret;
            switch(rand)
            {
                case 0:
                    ret = 'め';
                    break;
                case 1:
                    ret = 'め';
                    break;
                case 2:
                    ret = 'た';
                    break;
                case 3:
                    ret = 'ぁ';
                    break;
                case 4:
                    ret = 'く';
                    break;
                case 5:
                    ret = 'ぱ';
                    break;
                case 6:
                    ret = 'ぱ';
                    break;
                case 7:
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
            if (inputMan.BtnLeftDown && gameMan.isBlockByPos(-1, 0))
            {
                Pos.x--;
                UpdatePos();
            }
            else if (inputMan.BtnRightDown && gameMan.isBlockByPos(1, 0))
            {
                Pos.x++;
                UpdatePos();
            }
            else if (inputMan.BtnDownDown && gameMan.isBlockByPos())
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
    /// 動かないようにする。
    /// </summary>
    public void FixedBlock()
    {
        isMovable = false;
        FixedTime = gameMan.currentTime;
    }

    /// <summary>
    /// ブロックが文字ができた時
    /// </summary>
    public void CreatedWord()
    {
        isFixed = true;
        text.color = Color.red;
    }
}
