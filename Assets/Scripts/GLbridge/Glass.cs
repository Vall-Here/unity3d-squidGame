
using System.Collections;
using UnityEngine;

public class Glass : MonoBehaviour
{
    public bool isBroken;
    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    public GameObject brokenGlass;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void BreakGlass(){
        meshRenderer.enabled = false;
        boxCollider.enabled = false;
        brokenGlass.SetActive(true); 
    }

    public IEnumerator BreakGlassWithDelay(float delay){
        yield return new WaitForSeconds(delay);
        BreakGlass();
    }

   
    
}
