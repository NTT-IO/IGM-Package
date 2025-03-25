using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpManage : MonoBehaviour
{
    public Board board;
    public List<GameObject> HpImage;
    public GameObject P_Hp;
    public List<GameObject> HpStart=new List<GameObject>();
    public bool[] canMove=new bool[6];
    //¼ä¸ôÊ±¼ä
    public float time;
    private void Update()
    {
        HpUpdate();
    }
    void HpUpdate()
    {
        if(Hp.currHp>=Hp.maxHp)
        {
            Hp.currHp = Hp.maxHp;
        }
        if(Hp.currHp<0)
        {
            Hp.currHp = 0;
        }

        StartCoroutine(HpControl());
        StartCoroutine(HpMoving());
        while (HpStart.Count > Hp.currHp&&!board.gameOver)
        {
            HpStart[HpStart.Count - 1].GetComponent<Animator>().SetBool("Hurt", true);
            HpStart.Remove(HpStart[HpStart.Count - 1]);
        }
    }
    public IEnumerator HpControl()
    {
        while (HpStart.Count < Hp.currHp)
        {
            GameObject Hp = Instantiate(P_Hp, HpImage[HpStart.Count].transform);
            HpStart.Add(Hp);
            canMove[HpStart.Count - 1] = true;
            yield return new WaitForSeconds(time);
        }
        yield return null;
    }
    public IEnumerator HpMoving()
    {
        for (int i = 0; i < canMove.Length; i++)
        {
            if (canMove[i]&& i < HpStart.Count)
            {
                float lerpValueX = Mathf.Lerp(HpStart[i].transform.position.x, HpImage[i].transform.position.x, 4f * Time.deltaTime);
                float lerpValueY = Mathf.Lerp(HpStart[i].transform.position.y, HpImage[i].transform.position.y, 4f * Time.deltaTime);
                Vector3 less = new Vector3(0.01f, 0.01f, 0);
                HpStart[i].transform.position = new Vector3(lerpValueX, lerpValueY, 0);
                if ((HpStart[i].transform.position - HpImage[i].transform.position).sqrMagnitude < 0.05f)
                {
                    HpStart[i].transform.position = HpImage[i].transform.position;
                    HpStart[i].GetComponent<Animator>().SetBool("Flash", true);
                    Hp.lastHp++;
                    canMove[i] = false;
                }
                yield return new WaitForSeconds(time);
            }
            yield return null;
        }
    }
    public void resetHpImage()
    {
        for (int i = 0;i<6;i++ )
        {
            while (HpImage[i].transform.childCount>0)
            {
                Transform child = HpImage[i].transform.GetChild(0);
                child.SetParent(null);
                Destroy(child.gameObject);
            }
        }
        HpStart.Clear();
    }
}
