using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MainChar : MonoBehaviour
{
    [SerializeField]
    Animator anim;

    public static Action pr;
    int count = 0;

    private void Awake()
    {
        pr = () => { Pr(); };
    }
    public void Pr() //main scene 시작버튼 캐릭터 회전
    {
        if (count == 0)
        {
            ++count;
            transform.Rotate(0, -90, Time.deltaTime * 1);
            anim.Play("Sword Slash");
        }
    }
}
