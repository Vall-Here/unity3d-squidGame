
using System.Collections;
using UnityEngine;

public class FallHandle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerController>()) {
            PlayerController player = other.GetComponent<PlayerController>();
                player.animator.SetBool("isFalling", true);
                player.audioSource.PlayOneShot(player.audioManager.fallSFX);
            
            }
    }

}
