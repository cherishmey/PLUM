using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUIEvent : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject resetPanel;

    void Start()
    {
        resetPanel = GameObject.Find("Canvas").transform.Find("ResetUI").gameObject;
    }
    
    public void ActiveResetButton()
    {
        resetPanel.SetActive(!resetPanel.active);
    }

    public void ConfirmButton() // 확인 버튼을 눌렀을 때 처리
    {
        resetPanel.SetActive(false);
//        normalPanel.SetActive(true);
    }
}
