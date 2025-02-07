using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour
{
    public static BridgeManager Instance { get; private set; }
    public Transform leftsideBridge, rightsideBridge;
    public int totalRow;
    public float glasOffset;

    [Header("Glass")]
    public GameObject glassPrefab;
    public Glass[,] glasses;
    



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {Destroy(gameObject);}
        else{Instance = this;}
    }

    private void Start() {
        CreateBridge();
    }


    public void CreateBridge()
    {
        glasses = new Glass[totalRow, 2]; 


        for (int i = 0; i < totalRow; i++) 
        {
            GameObject leftGlass = Instantiate(glassPrefab, leftsideBridge);
            GameObject rightGlass = Instantiate(glassPrefab, rightsideBridge);

            
            leftGlass.transform.localPosition = new Vector3(0, 0.4f, i * 9 + glasOffset);
            rightGlass.transform.localPosition = new Vector3(0, 0.4f, i * 9 + glasOffset );
          
            Glass glassLeft = leftGlass.GetComponent<Glass>();
            Glass glassRight = rightGlass.GetComponent<Glass>();
            glasses[i, 0] = glassLeft;
            glasses[i, 1] = glassRight;

            bool rightIsBroken = RightisBroken();   
            if(rightIsBroken){
                glassRight.isBroken = true;
            }else{
                glassLeft.isBroken = true;
            }
        }
    }

    public bool RightisBroken(){
        bool isBroken = false;
        int randomNumber = Random.Range(0, 2);

        if(randomNumber == 0){
            isBroken = true;
        }else{
            isBroken = false;
        }
        return isBroken;
    }
}
