using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FinishPlum;
using System;

public class TimerUIManager : MonoBehaviour
{
    public GameObject plumManager;
    public GameObject UsingUI;
    public GameObject CompleteUI;

    public GameObject SelectTodayUI;

    public GameObject TimeTracker;

    public RecordUsageTime recordUsgeTime;

    bool isDay1;
       

    // Start is called before the first frame update
    void Start()
    {
        TimeTracker.GetComponent<RecordUsageTime>().FinishTimerEvnetHandler += OnFinishTimer;
        
        // UsingUI.SetActive(false);
        
    }

    void Awake(){
        CompleteUI.SetActive(false);
    }

    private void OnFinishTimer(object sender, EventArgs e)
    {
        UsingUI.SetActive(false);
        CompleteUI.SetActive(true);
        StartCoroutine(UICoroutine());
    }

    private IEnumerator UICoroutine()
    {
        WaitForSeconds waitSec = new WaitForSeconds(1);
        yield return waitSec;
        this.gameObject.SetActive(false);
        CompleteUI.SetActive(false);
        UsingUI.SetActive(true);
    }

    public void ActivateOnBtn()
    {
        PlumManager pm = plumManager.GetComponent<PlumManager>();
        isDay1 = pm.isDay1;

        int day1SelectedNum = pm.getPlumNumWStatus(2, true) + pm.getPlumNumWStatus(3, true);
        int day2SelectedNum = pm.getPlumNumWStatus(2, false) + pm.getPlumNumWStatus(3, false);
        //print(String.Format("{0} / {1}", day1SelectedNum, day2SelectedNum));
        if(isDay1) // 첫째날일때
        {
            if(day1SelectedNum != 0 && day2SelectedNum == 0) // 왼쪽거만 선택했으면
            {
                
                this.gameObject.SetActive(true);
            }
            else
            {
                SelectTodayUI.SetActive(true);
            }
        }
        else // 둘째날일때
        {
            if(day2SelectedNum != 0 && day1SelectedNum == 0) // 오른쪽거만 선택했으면
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                SelectTodayUI.SetActive(true);
            }

        }
    }

    public void OnClickStopButton()
    {
        this.gameObject.SetActive(false);
        recordUsgeTime.Stop();
    }

    
}
