using System.Collections;
using System.Collections.Generic;
using EzySlice;
using UnityEngine;

public class Saw : MonoBehaviour
{

    private GameObject objectInToch;

    private void OnTriggerEnter(Collider other)
    {
        objectInToch=other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if(objectInToch==other.gameObject)
            objectInToch=null;
    }
    public void Slice()
    {
        objectInToch.SliceInstantiate(transform.position, transform.up, objectInToch.GetComponent<MeshRenderer>().materials[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Slice();
        }
    }
}
