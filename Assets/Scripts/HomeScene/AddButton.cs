using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

public class AddButton : MonoBehaviour
{

    public GameObject dayButton;

    public GameObject firstWeekGameObject;
    public GameObject secondWeekGameObject;
    public List<GameObject> firstArray = new List<GameObject>(); 
    public List<GameObject> secondArray = new List<GameObject>();
    PlumMetaData metaPlum;
    string folderPath;
    void Start()
    {

        folderPath = (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer ?
            Application.persistentDataPath : Application.dataPath) + "/Save/";

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        int i = 1;
        metaPlum = PlumMetaData.readOrCreate(folderPath);
        int numButton = metaPlum.frequency;
        for(i=1; i<=numButton; i++){
            GameObject go = Instantiate (dayButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.name = "Button_Week1_" + i.ToString();
            go.SetActive(true);
            go.GetComponentInChildren<Text>().text = i.ToString();
            go.GetComponent<ButtonStatus>().idx = 10+i;
            go.GetComponent<ButtonStatus>().whichWeek = 1;
            //if(i!=1)
            go.GetComponent<Button>().interactable = false;
            go.transform.SetParent(firstWeekGameObject.transform);
            firstArray.Add(go);
        }

        for(i=1; i<=numButton; i++){
            GameObject go = Instantiate (dayButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            go.name = "Button_Week2_" + i.ToString();
            go.SetActive(true);
            go.GetComponentInChildren<Text>().text = i.ToString();
            go.GetComponent<ButtonStatus>().idx = 20+i;
            go.GetComponent<ButtonStatus>().whichWeek = 2;
            go.GetComponent<Button>().interactable = false;
            go.transform.SetParent(secondWeekGameObject.transform);
            secondArray.Add(go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
