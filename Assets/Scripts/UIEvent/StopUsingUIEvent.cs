using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopUsingUIEvent : MonoBehaviour
{
    private GameObject  usingPanel;
    // Start is called before the first frame update
    void Start()
    {
        usingPanel = GameObject.Find("Canvas").transform.Find("TimerUI").transform.Find("UsingImage").gameObject;
    }

    // Update is called once per frame
    public void OnClickStopBtn()
    {
        usingPanel.SetActive(false);
    }
}
