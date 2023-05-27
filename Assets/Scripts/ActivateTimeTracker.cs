using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTimeTracker : MonoBehaviour
{
    private GameObject timeTracker;
    // Start is called before the first frame update

    // Update is called once per frame
    public void ActivateTracker()
    {
        timeTracker = GameObject.Find("Canvas").transform.Find("TimeTracker").gameObject;
        timeTracker.SetActive(true);
    }
}
