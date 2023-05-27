using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPlumUIEvent : MonoBehaviour
{
    private bool nextPlumOn = false;
    private GameObject normalPanel;
    private GameObject nextPlumPanel;


    void Awake()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        nextPlumPanel = GameObject.Find("Canvas").transform.Find("nextPlumUI").gameObject;
    }

    public void ActiveNextPlumButton() // 다음 자두를 눌렀을 때 처리
    {
        if(!nextPlumOn)
        {
            nextPlumPanel.SetActive(true);
            normalPanel.SetActive(false);
        }
        else
        {
            nextPlumPanel.SetActive(false);
            normalPanel.SetActive(true);
        }

        nextPlumOn = !nextPlumOn; // bool값 반전
    }

    public void ConfirmButton() // 예/아니오 버튼을 눌렀을 때 처리
    {
        nextPlumPanel.SetActive(false);
        normalPanel.SetActive(true);
    }
}
