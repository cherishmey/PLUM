using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovePlumUIRToL : MonoBehaviour
{
    // Start is called before the first frame update
    private bool movePlumOn = false;
    public GameObject plumManager;
    private GameObject normalPanel;
    private GameObject movePlumPanel;
    public GameObject plumMaker;

    public TMP_Text textObj;


    void OnEnable()
    {
        normalPanel = GameObject.Find("Canvas").transform.Find("NormalUI").gameObject;
        movePlumPanel = GameObject.Find("Canvas").transform.Find("MovePlumUIRToL").gameObject;
        String content = "자두 " + plumManager.GetComponent<PlumManager>().getPlumNumWStatus(2, false) + "개를 옮길까요?";
        textObj.text = content;
    }

    public void ActiveMovePlumButton() // 도움말 버튼을 눌렀을 때 처리
    {
        
        if(!movePlumOn) // movePlumOn이 거짓이면
        {
            movePlumPanel.SetActive(true);
            normalPanel.SetActive(false);
        }
        movePlumOn = !movePlumOn; // bool값 반전
    }

    public void ConfirmButton() // 예/아니오 버튼을 눌렀을 때 처리
    {
        // movePlumOn = !movePlumOn;
        // movePlumPanel.SetActive(false);
        // normalPanel.SetActive(true);

        plumMaker.GetComponent<AddPlum>().movePlumToLeft();
        this.gameObject.SetActive(false);
    }

    public void CancleButton()
    {
        this.gameObject.SetActive(false);
    }

}
