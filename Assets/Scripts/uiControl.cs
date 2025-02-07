using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiControl : MonoBehaviour
{
    public GameObject menuPanel;
    public TMPro.TextMeshProUGUI infoText;
    public TMPro.TextMeshProUGUI goText;
    public Button restartButton;
    public Button exitButton;


    public IEnumerator showGoText(){
        goText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        goText.gameObject.SetActive(false);
    }
}
