using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FinishPlum;

public class CheckSelectMode : MonoBehaviour
{

    public bool isSelect = true;
    // public Button SelectBtn;
    // public Button CancelBtn;
    // public Button ConfirmBtn;

    public GameObject YesBtn;
    public GameObject TimerTracker;

    public GameObject plumManager;

    // Start is called before the first frame update
    // void Start()
    // {
    //     SelectBtn.gameObject.SetActive(true);
    //     CancelBtn.gameObject.SetActive(false);
    //     ConfirmBtn.gameObject.SetActive(false);
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if(isSelect){

    //     }else{

    //     }
        
    // }

    // public void onClickSelectBtn(){
    //     SelectBtn.gameObject.SetActive(false);
    //     CancelBtn.gameObject.SetActive(true);
    //     ConfirmBtn.gameObject.SetActive(true);


        
    // }

    // public void onClickCancelBtn(){
    //     SelectBtn.gameObject.SetActive(true);
    //     CancelBtn.gameObject.SetActive(false);
    //     ConfirmBtn.gameObject.SetActive(false);
    // }



    public void onClickFinishBtn(){
        // 사용하시겠습니까 창을 띄우면 됨
        UseUIEvent uiEvent = YesBtn.GetComponent<UseUIEvent>();
        uiEvent.ConfirmButtonClick = startTimer;
    }

    public void startTimer(){
        // 선택한 자두 가져와서 시간 설정하기
        TimerTracker.GetComponent<RecordUsageTime>().createTimerWSelectedPlumQue();
    }
}
