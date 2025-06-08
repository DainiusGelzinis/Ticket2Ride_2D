using UnityEngine;

public class IntroController : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public void OnIntroFinished()
    {
        mainMenuPanel.SetActive(true);
        //gameObject.SetActive(false); 
    }
}
