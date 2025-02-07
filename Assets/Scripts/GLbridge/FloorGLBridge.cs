
using UnityEngine;

public class FloorGLBridge : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerController>()) {
            PlayerController player = other.GetComponent<PlayerController>();
                player.Dead();
            
            }
    }
}
