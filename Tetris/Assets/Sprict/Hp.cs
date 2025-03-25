using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Hp 
{
    public static int maxHp=6;
    public static int currHp=3;
    public static int lastHp = 0;
    public static void hurt()
    {
        currHp--;
    }
    public static void bonus_addHp()
    {
        currHp++;
    }
    public static void resetHp()
    {
        maxHp = 6;
        currHp = 3;
        lastHp = 0;
    }
}
