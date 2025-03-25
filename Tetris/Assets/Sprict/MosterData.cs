using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MosterData:MonoBehaviour 
{
    public MusicManage musicManage;
    public GameObject bouns;
    public static float speedBouns = 1;
    public static float hpBouns = 1;
    public TextMeshProUGUI effectText;
    public List<M_Color> MosterList = new List<M_Color>(6);
    public Moster[] MosterDir = new Moster[7];
    public Dictionary<int, M_Color> colorDic = new Dictionary<int, M_Color>();
    public TextMeshProUGUI LevelText;
    public Board board;

    public float times = 0;

    public bool begin = false;
    private void Awake()
    {
        for (int i = 0;i<7;i++ )
        {
            MosterList.Add((M_Color)i);
        }
        colorDic.Add(0, M_Color.Bule);
        colorDic.Add(1, M_Color.Cyan);
        colorDic.Add(2, M_Color.Green);
        colorDic.Add(3, M_Color.Orange);
        colorDic.Add(4, M_Color.Purple);
        colorDic.Add(5, M_Color.Red);
        colorDic.Add(6, M_Color.Yellow);
    }
    private void Start()
    {
        effectText.text = $"EFFECT: " + "##";
        LevelText.text = 1.0f+"";
    }
    private void Update()
    {
        listUpdate();
        if (begin)
        {
            times += Time.deltaTime;
        }
        if (times > 1.0f)
        {
            times = 0;
            bouns.SetActive(false);
            begin = false;
        }
        float temp = float.Parse((speedBouns * hpBouns).ToString("#0.00"));
        LevelText.text = ""+(temp>10?10:temp);
        if(temp<4)
        {
            ColorLerpUtility.UpdateTextColor(temp, 0f, 4, Color.blue, Color.red, LevelText);
        }
    }
    public void listUpdate()
    {
        while (MosterList.Count<7)
        {
            MosterList.Add((M_Color)Random.Range(0, 7));
        }
    }
    public void decrease_moster(ref bool clearLine)
    {
        if(clearLine)
        {
            bonus(MosterList[0]);
            clearLine = false;
        }
        MosterList.RemoveAt(0);
    }
    public void bonus(M_Color color)
    {
        effectText.text = $"EFFECT:"+ SelectEffect(color);
        bouns.SetActive(true);
        begin = true;
        switch (color)
        {
            case M_Color.Bule:
                blueBonus();
                break;
            case M_Color.Cyan:
                cyanBouns();
                break;
            case M_Color.Green:
                greenBouns();
                break;
            case M_Color.Red:
                if(Board.clearing_row > -11) redBonus(Board.clearing_row-1);
                break;
            case M_Color.Orange:
                orangeBouns();
                break;
            case M_Color.Yellow:
                yellowBouns();
                break;
            case M_Color.Purple:
                purpleBouns();
                break;
        }
    }
    string SelectEffect(M_Color color)
    {
        string s;
        switch (color)
        {
            case M_Color.Bule:
                s = "Ix3";
                break;
            case M_Color.Cyan:
                s = "SU";
                break;
            case M_Color.Green:
                s = "Hp¡ü";
                break;
            case M_Color.Red:
                s = "OML";
                break;
            case M_Color.Orange:
                s = "SD";
                break;
            case M_Color.Yellow:
                s = "Hp¡ý";
                break;
            case M_Color.Purple:
                s = "Lx5";
                break;
            default :
                s = "";
                break;
        };
        return s;
    }
    public bool SameColor(int colors)
    {
        if (colorDic[colors] == MosterList[0])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void blueBonus()
    {
        Board.setBonusPiece(0, 3);
    }

    public void redBonus(int row)
    {
        board.ClearRow(row);
        board.DropRow(row + 1);
    }

    public void purpleBouns()
    {
        Board.setBonusPiece(6, 5);
    }

    public void yellowBouns()
    {
        Hp.hurt();
        hpBouns *= 1.1f;
        musicManage.PlayHurt();
        board.GameOverJudge();
    }

    public void greenBouns()
    {
        Hp.bonus_addHp();
    }

    public void cyanBouns()
    {
        Piece.normalSpeed *= 0.8f;
        speedBouns *= 2f;
    }

    public void orangeBouns()
    {
        Piece.normalSpeed *= 1.25f;
        speedBouns *= 0.5f;
    }
}

