using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopGame : MonoBehaviour
{
    public void stop()
    {
        Time.timeScale = 0;
    }
}
