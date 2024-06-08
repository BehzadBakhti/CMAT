using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollidersTest : MonoBehaviour
{
    Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
    }
}
