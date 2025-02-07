using System.Collections;
using UnityEngine;

public class DollController : MonoBehaviour
{
    [Header("Basae Setup")]
    public float minTimer, maxTimer;
    public bool isGreenLight = true;
    public bool lightSignal;
    public readonly string GLanimation = "GL";

    [Header("References")]
    public Animator animator;
    public GameObject bullet;
    public Transform shotPoint;
    public bool hasShot = false;
    public AudioSource audioSource;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip greenLightSound;
    public AudioClip redLightSound;




    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

   

    public IEnumerator changeLightCorroutine(){
        yield return new WaitForSeconds(Random.Range(minTimer, maxTimer));
        isGreenLight = !isGreenLight;
        animator.SetBool(GLanimation, isGreenLight);
        StartCoroutine(changeLightCorroutine());
    }

    public void ShootPlayer(Transform target){
        if(hasShot) return;
        audioSource.PlayOneShot(shootSound);
        GameObject bulletObj = Instantiate(bullet, shotPoint.position, Quaternion.identity);
        bulletObj.GetComponent<BulletMovement>().target = target;
        hasShot = true;
    }

    public void DisableDoll(){
        StopAllCoroutines();
    }

}
