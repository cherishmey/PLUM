using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CantMovePlumUIEvent : MonoBehaviour
{
    private bool cantMovePlumOn = false;
    //private GameObject normalPanel;
    private GameObject cantMovePlumPanel;


    void Awake()
    {
        //normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        cantMovePlumPanel = GameObject.Find("Canvas").transform.Find("CantMovePlumUI").gameObject;
    }

    public void ActivecantMovePlumButton() // 도움말 버튼을 눌렀을 때 처리
    {
        if(!cantMovePlumOn)
        {
            cantMovePlumPanel.SetActive(true);
            //normalPanel.SetActive(false);
        }
        else
        {
            cantMovePlumPanel.SetActive(false);
            //normalPanel.SetActive(true);
        }

        cantMovePlumOn = !cantMovePlumOn; // bool값 반전
    }

    public void ConfirmButton() // 확인 버튼을 눌렀을 때 처리
    {
        cantMovePlumPanel.SetActive(false);
        //normalPanel.SetActive(true);
    }
}

