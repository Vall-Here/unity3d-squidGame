using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogleDoll : MonoBehaviour
{
    public DollController dollController;
    

    public void TogleRed(){
        dollController.audioSource.PlayOneShot(dollController.redLightSound);
        StartCoroutine(TogleDelay());
        dollController.lightSignal = false;
    }

    public void TogleGreen(){
        dollController.audioSource.PlayOneShot(dollController.greenLightSound);
        StartCoroutine(TogleDelay());
        dollController.lightSignal = true;

    }

    IEnumerator TogleDelay(){
        yield return new WaitForSeconds(1.2f);
    }
}
