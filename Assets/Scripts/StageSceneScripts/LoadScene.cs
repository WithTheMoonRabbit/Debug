using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    public void OnClickStartBtn()
    {
        MainChar.pr();
        Invoke("LoadS", 1f);
    }
    public void LoadS()
    {
        //LoadingCtrl.Instance.LoadScene("StageScene");
    }
}
