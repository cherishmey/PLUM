using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumPlumUIEvent : MonoBehaviour
{
    private bool numPlumOn = false;
    private GameObject normalPanel;
    private GameObject numPlumPanel;


    void Awake()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        numPlumPanel = GameObject.Find("Canvas").transform.Find("NumPlumUI").gameObject;
    }

    public void ActiveSaveButton() // 저장 버튼을 눌렀을 때 처리
    {
        if(!numPlumOn)
        {
            numPlumPanel.SetActive(true);
            normalPanel.SetActive(false);
        }
        else
        {
            numPlumPanel.SetActive(false);
            normalPanel.SetActive(true);
        }

        numPlumOn = !numPlumOn; // bool값 반전
    }

    public void ConfirmButton() // 확인 버튼을 눌렀을 때 처리
    {
        numPlumPanel.SetActive(false);
        normalPanel.SetActive(true);
    }
}
