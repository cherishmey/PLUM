using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseUIEvent : MonoBehaviour
{
    private bool useOn = false;
    private GameObject normalPanel;
    private GameObject usePanel;
    private GameObject selectUIPanel;
    public GameObject plumManager;

    public TMP_Text textObj;

    public delegate void BtnClickDelegate();
    public BtnClickDelegate ConfirmButtonClick;
    
    void OnEnable()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        usePanel = GameObject.Find("Canvas").transform.Find("UseUI").gameObject;
        selectUIPanel = GameObject.Find("Canvas").transform.Find("SelectTodayUI").gameObject;
    }

    public void ActiveUseButton() // 사용 버튼을 눌렀을 때 처리
    {
        String content = "자두 " + plumManager.GetComponent<PlumManager>().getSelectedPlumNum() + "개를 사용할까요?";
        textObj.text = content;
        usePanel.transform.Find("Image").transform.Find("Yes").gameObject.SetActive(true);
        usePanel.transform.Find("Image").transform.Find("No").gameObject.SetActive(true);
        if(plumManager.GetComponent<PlumManager>().getSelectedPlumNum() == 0 || plumManager.GetComponent<PlumManager>().getPlumNumWStatus(2, !(plumManager.GetComponent<PlumManager>().isDay1) ) !=0 ) // 오늘 거에서 선택 안 했을 때
        {
            
            textObj.text = "오늘의 자두만 사용해주세요.";
            usePanel.transform.Find("Image").transform.Find("Yes").gameObject.SetActive(false);
            usePanel.transform.Find("Image").transform.Find("No").gameObject.SetActive(false);
            if(plumManager.GetComponent<PlumManager>().getPlumNumWStatus(2, true) == 0 && plumManager.GetComponent<PlumManager>().getPlumNumWStatus(2, false) == 0) // 아예 선택 안했을때 
            {
                textObj.text = "자두를 선택해주세요.";
            }   
        }   
        usePanel.SetActive(!usePanel.active);
    }

    public void ConfirmButton() // 확인 버튼을 눌렀을 때 처리
    {
        ConfirmButtonClick();
        usePanel.SetActive(false);
//        normalPanel.SetActive(true);
    }
}
