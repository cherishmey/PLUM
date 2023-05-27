using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour
{
    public GameObject go;
    // Start is called before the first frame update
    public void ActivateOnBtn()
    {
        go.SetActive(false);
    }
}
