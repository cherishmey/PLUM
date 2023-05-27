using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MovePlumUIToR;
    public GameObject MovePlumUIToL;

    public GameObject CantMovePlumUI;

    public GameObject plumManager;

    public GameObject plumMaker;

    public void onClickToRightButton(){
        int normalPlumNum = plumManager.GetComponent<PlumManager>().getPlumNumWStatus(2, true);
        if(normalPlumNum == 0)
        {
            CantMovePlumUI.SetActive(true);
        }
        else {//if (normalPlumNum != plumManager.GetComponent<PlumManager>().day1.Count){
            MovePlumUIToR.SetActive(true);
        }

    }

    public void onClickToLeftButton(){
        int normalPlumNum = plumManager.GetComponent<PlumManager>().getPlumNumWStatus(2, false);
        // Debug.Log("count: "+plumManager.GetComponent<PlumManager>().day2.Count);
        if(normalPlumNum == 0)
        {
            CantMovePlumUI.SetActive(true);
        }
        else{ //if (normalPlumNum != plumManager.GetComponent<PlumManager>().day2.Count){
            MovePlumUIToL.SetActive(true);
        }
    }
}
