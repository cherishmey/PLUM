using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalUIEvent : MonoBehaviour
{
    // Start is called before the first frame update
private bool numPlumOn = false;
    private GameObject normalPanel;
    private GameObject PlzNumPanel;


    void Awake()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        PlzNumPanel = GameObject.Find("Canvas").transform.Find("PlzNumUI").gameObject;
    }

    public void ActiveSaveButton() // 저장 버튼을 눌렀을 때 처리
    {
        if(!numPlumOn)
        {
            PlzNumPanel.SetActive(true);
            normalPanel.SetActive(false);
        }
        else
        {
            PlzNumPanel.SetActive(false);
            normalPanel.SetActive(true);
        }

        numPlumOn = !numPlumOn; // bool값 반전
    }

    public void ConfirmButton() // 확인 버튼을 눌렀을 때 처리
    {
        PlzNumPanel.SetActive(false);
        normalPanel.SetActive(true);
    }
}