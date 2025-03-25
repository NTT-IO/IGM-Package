using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManage : MonoBehaviour
{
    public AudioSource Change;
    public AudioSource Clear;
    public AudioSource Hurt;
    public AudioSource Fall;
    public void PlayChange()
    {
        Change.Play();
    }
    public void PlayClear()
    {
        Clear.Play();
    }
    public void PlayHurt()
    {
        Hurt.Play();
    }
    public void PlayFall()
    {
        Fall.Play();
    }
}
