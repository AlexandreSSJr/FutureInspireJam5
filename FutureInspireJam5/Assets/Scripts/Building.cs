using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private GameObject areaEffect;
    public Material idle;
    public Material active;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            areaEffect.GetComponent<Renderer>().material = active;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            areaEffect.GetComponent<Renderer>().material = idle;
        }
    }

    private void Awake() {
        areaEffect = transform.Find("AreaEffect")?.gameObject;
    }
}
