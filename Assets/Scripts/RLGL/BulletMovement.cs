using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Transform target;
    public float bulletSpeed = 70f;


    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerController>()) {
            other.GetComponent<PlayerController>().Dead();
            Destroy(gameObject);
        }
    }

}
