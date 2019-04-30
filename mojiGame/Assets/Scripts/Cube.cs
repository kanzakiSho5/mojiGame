using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private char blockChar;
    [SerializeField]
    public bool isMovable;
    [SerializeField]
    public Vector2Int Pos;
    
    private void OnEnable()
    {
        Pos = new Vector2Int(4, 0);
        isMovable = true;
        SetChar(true);
    }

    private void Update()
    {
        // TODO: いずれ操作系を専用のManagerに移行
        

        gameObject.transform.position = new Vector3(Pos.x - 1, (20 - Pos.y) - 1, 0);
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

    /// <summary>
    /// 1ブロック分下げる
    /// </summary>
    public void DropBlock()
    {
        Pos.y++;
    }
}
