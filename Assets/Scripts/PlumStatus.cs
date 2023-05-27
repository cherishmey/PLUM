using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlumStatus : MonoBehaviour
{

    public GameObject plumManager;
    public Toggle plums;
    public int status = 1;
    private int beforStatus = 1;

    public Sprite spriteSeed;

    public Sprite spritePlum;

    public Sprite spriteSelected;

    public Sprite spriteUsing;

    public event EventHandler ClickedPlum;

    public int idx;

    public int whichDay;


        // Update is called once per frame
    void Update()
    {
        // bool isDay1 = plumManager.GetComponent<PlumManager>().isDay1;
        // List<PlumObject> changePlumList = isDay1 ? plumManager.GetComponent<PlumManager>().day1 : plumManager.GetComponent<PlumManager>().day2;

        if(plums.isOn)
        {
            // 1 -> 2
            if(plums.GetComponent<PlumStatus>().status == 1)
            {
                plums.GetComponent<PlumStatus>().status = 2;
                ClickedPlum(this.gameObject, null);
                // ClickedPlum(plums.gameObject, null);

            }         
        }
        else
        {
            // 2 -> 1
            if(plums.GetComponent<PlumStatus>().status == 2)
            {
                plums.GetComponent<PlumStatus>().status = 1;    
                ClickedPlum(this.gameObject, null);
            }
        }

        if (beforStatus != status){
            
            
        }
        beforStatus = status;
        
    }

    public void setStatus(int statusInt){
        status = statusInt;
        if (status == 0){
                // 씨
                plums.image.sprite = spriteSeed;
                plums.isOn = false;
                plums.GetComponent<Toggle>().interactable = false;
                
            }
            else if(status == 1){
                // 사용 가능
                plums.image.sprite = spritePlum;
                plums.isOn = false;
                plums.GetComponent<Toggle>().interactable = true;
            }
            else if(status == 2)
            {
                // 사용 대기
                plums.image.sprite = spriteSelected;
                plums.isOn = true;
                plums.GetComponent<Toggle>().interactable = true;
            }
            else if(status == 3) { // 3
                // 사용중
                plums.image.sprite = spriteUsing;
                plums.isOn = false;
                plums.GetComponent<Toggle>().interactable = false;
            }
            else {
                plums.GetComponent<Toggle>().interactable = false;
            }
    }
}
