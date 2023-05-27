using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ButtonStatus : MonoBehaviour
{
    public GameObject buttonManager;

    public ButtonMetaData buttonData;

    public int idx;
    
    public string clickedDate;

    public int whichWeek;

    ButtonMetaData ButtonData;
    PlumMetaData PlumData;
    int i;
    int frequency;

    // Start is called before the first frame update
    void Start()
    {
        
    }

        // 함수를 하나 따로 만들어서 리스트날짜마다 터치한 날 11시 59분이 지나면 대응하는 아이콘 비활성화하고 그 다음거 활성화하도록?
        // 사용횟수 홀짝에 따라 다름
        // 나머지 다 꺼줘야 함
        // 자정 지나고 나면 꺼야 함
    // Update is called once per frame
    void Update()
    {
        // frequency = PlumData.frequency;
        // i = buttonData.usedDays;
        // if(frequency % 2 == 1) // 사용횟수가 홀수
        // {
            
        //     ButtonData.firstWeekDateStr[i]
        //     buttonData.
        // }
        // else // 짝수
        // {

        // }
        
    }
    
    public void UpdateDate()
    {
        clickedDate = DateTime.Now.ToString("yyyy_MM_dd");
    }

    void ButtonEnabler()
    {
        if(buttonData.startDate.Day < DateTime.Now.Day)
        {
         //   this.gameObject
        }
    }
}
