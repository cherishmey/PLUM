using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryPlumComponent : MonoBehaviour
{

    public GameObject day1Title;
    public GameObject day2Title;

    public GameObject day1Grid;
    public GameObject day2Grid;

    public List<Sprite> plumSprites;

    public void CreatePlumGrid(string title, List<PlumObject> plumList, bool isDay1)
    {
        switch (isDay1)
        {
            case true:
                day1Title.GetComponent<Text>().text = title;
                break;
            case false:
                day2Title.GetComponent<Text>().text = title;
                break;
        }

        foreach(PlumObject plumObj in plumList)
        {
            addPlumImages(plumObj, isDay1);
        }
    }

    void addPlumImages(PlumObject plumObj, bool isDay1)
    {
        GameObject NewObj = new GameObject(); //Create the GameObject
        Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script

        switch (plumObj.plumStatus)
        {
            case 0:
                NewImage.sprite = plumSprites[0];
                break;
            case 1:
            default:
                NewImage.sprite = plumSprites[1];
                break;
        }

        if (isDay1)
        {
            NewImage.transform.SetParent(day1Grid.transform, false);
        }
        else
        {
            NewImage.transform.SetParent(day2Grid.transform, false);
        }
        
    }

}
