using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmUIEvent : MonoBehaviour
{
    private bool confirmOn = false;
    private GameObject normalPanel;
    private GameObject confirmPanel;


    void Awake()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        // confirmPanel = GameObject.Find("Canvas").transform.Find("ConfirmUI").gameObject;
    }

    public void ActiveConfirmButton() // 
    {
        if(!confirmOn)
        {
            // confirmPanel.SetActive(true);
            normalPanel.SetActive(false);
        }
        else
        {
            // confirmPanel.SetActive(false);
            normalPanel.SetActive(true);
        }

        confirmOn = !confirmOn; // bool값 반전
    }

    public void ConfirmButton() // 확인 버튼을 눌렀을 때 처리
    {
        // confirmPanel.SetActive(false);
        normalPanel.SetActive(true);
    }
}
