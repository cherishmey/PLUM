using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
   public void ChangeToDaily()
    {
        SceneManager.LoadScene("DailyUsageScene");
    }

    public void ChangeToRule()
    {
        SceneManager.LoadScene("DailyUsageScene");
    }

    public void ChangeToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
}