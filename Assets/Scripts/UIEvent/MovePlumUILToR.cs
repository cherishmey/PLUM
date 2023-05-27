using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class MovePlumUILToR : MonoBehaviour
{
    // Start is called before the first frame update
    private bool movePlumOn = false;
    private GameObject normalPanel;
    private GameObject movePlumPanel;
    public GameObject plumMaker;
    public GameObject plumManager;
    public TMP_Text textObj;

    void OnEnable()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        movePlumPanel = GameObject.Find("Canvas").transform.Find("MovePlumUIRToL").gameObject;
        String content = "자두 " + plumManager.GetComponent<PlumManager>().getPlumNumWStatus(2, true) + "개를 옮길까요?";
        textObj.text = content;
    }

    public void ActiveMovePlumButton() // 도움말 버튼을 눌렀을 때 처리
    {
        if(!movePlumOn)
        {
            movePlumPanel.SetActive(true);
            normalPanel.SetActive(false);
        }

        movePlumOn = !movePlumOn; // bool값 반전
    }

    public void ConfirmButton() // 예/아니오 버튼을 눌렀을 때 처리
    {
        
        // 오른쪽으로 옮기기
        plumMaker.GetComponent<AddPlum>().movePlumToRight();
        this.gameObject.SetActive(false);
    }

    public void CancelButton()
    {
        this.gameObject.SetActive(false);
    }

}
