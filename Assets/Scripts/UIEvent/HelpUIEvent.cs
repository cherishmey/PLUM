using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HelpUIEvent : MonoBehaviour
{
    private bool helpOn = false;
    //private GameObject normalPanel;
    private GameObject helpPanel;
    public GameObject firstWeekButtons;


    void Awake()
    {
        helpPanel = GameObject.Find("Canvas").transform.Find("HelpUI").gameObject;
    }

    public void ActiveHelpButton() // 도움말 버튼을 눌렀을 때 처리
    {
        if(!helpOn)
        {
            helpPanel.SetActive(true);
            //normalPanel.SetActive(false);
        }
        else
        {
            helpPanel.SetActive(false);
            //normalPanel.SetActive(true);
        }

        helpOn = !helpOn; // bool값 반전
    }

    public void ConfirmButton() // 확인 버튼을 눌렀을 때 처리
    {
        helpPanel.SetActive(false);
        //normalPanel.SetActive(true);
    }
}

