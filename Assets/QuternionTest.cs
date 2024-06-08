using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;



public class QuternionTest : MonoBehaviour
{
    private BoxCollider collider;
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        //  VectorTest();

    }


    void Update()
    {
         Debug.Log("bound center:" + collider.bounds.center);
         Debug.Log("bound extends:" + collider.bounds.extents);
         Debug.Log("bound max:" + collider.bounds.max);
    }
    void EulerAngleTest()
    {
        var q = new Quaternion(0.19811f, 0.13872f, 0.13872f, 0.96035f);
        var angles = q.eulerAngles;
        Debug.Log(angles);
        Debug.Log(transform.rotation);
        Assert.AreEqual(new Vector3(20f, 20f, 20f), angles);
    }

    public void LookRotation()
    {
        var forward = new Vector3(1, 0, 1);

        var q = Quaternion.LookRotation(forward, Vector3.up);
        var angles = q.eulerAngles;
        Debug.Log(angles);
        Assert.AreEqual(new Vector3(20f, 20f, 20f), angles);
    }

    public void mul_operator()
    {
        var q1 = new Quaternion(0.2f, 0.2f, 0.2f, 0.9f);
        var q2 = new Quaternion(0.1f, 0.1f, 0.1f, 0.9f);

        var q3 = q1 * q2;
        var q4 = new Quaternion(0.2700f, 0.2700f, 0.2700f, 0.7500f);
        Debug.Log(q3);
        Assert.AreEqual(q3, q4);
    }

    public void mul_qv_operator()
    {
        var q1 = new Quaternion(0.19f, 0.14f, 0.14f, 0.96f);
        var v1 = new Vector3(0, 0, 1);

        var v2 = q1 * v1;
        var v3 = new Vector3(0, 0, 1);
        Debug.Log(v2);
        Assert.AreEqual(v3, v2);
    }

    public void Euler()
    {
        var q1 = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        var q2 = Quaternion.Euler(new Vector3(45f, 0f, 0f));
        var q3 = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        var q4 = Quaternion.Euler(new Vector3(90f, 90f, 0f));

        Debug.Log(q1);
        Debug.Log(q2);
        Debug.Log(q3);
        Debug.Log(q4);
    }

    public void lerp_test()
    {
        var q1 = new Quaternion(0.5f, 0f, 0f, 0.9f);
        var q2 = new Quaternion(0.0f, 0.5f, 0f, 0.9f);
        var q3 = Quaternion.LerpUnclamped(q1, q2, 1.4f);

        Debug.Log(q1);
        Debug.Log(q2);
        Debug.Log(q3);
    }

    public void FromToRotation()
    {
        var v1 = new Vector3(1f, 0f, 0f);
        var v2 = new Vector3(0f, 0f, 1f);
        var q = Quaternion.FromToRotation(v1, v2);
        

        var q2 = Quaternion.RotateTowards(Quaternion.Euler(v1), Quaternion.Euler(v2), Single.MaxValue);

        Debug.Log(q);
        Debug.Log(q2);

        Debug.Log(q * v1);
        Debug.Log(Quaternion.Inverse(q) * v2);
        // Assert.True(q.NearEqual(new FixedQuaternion(0.1f, 0f, 0f, 0f)));
    }

    public void VectorTest()
    {
        var v1 = new Vector3(1f, 1f, 1f);
        var v2 = new Vector3(1f, 0f, 2f);
        var planeNormal = new Vector3(1, 0, 0);// XZ plane

        var cross = Vector3.Cross(v1, v2);

        var reflect = Vector3.Reflect(v1, planeNormal);
        var project = Vector3.Project(v1, v2);
        Vector3.RotateTowards(v1, v2, 0.5f, 1f);

        var p1 = new Vector3(1, 2, 3);
        var p2 = new Vector3(0, 3, 4);
        var move = Vector3.MoveTowards(p1, p2, 1f);



        Debug.Log("cross: " + cross);
        Debug.Log("project: " + project);
        Debug.Log("move: " + move);
        Debug.Log("reflect: " + reflect);
        

    }
}

