using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class MinkSum : MonoBehaviour
{
    public GameObject PrefGameObject;
    public GameObject cube1;
    public GameObject cube2;
    public List<GameObject> points;
    // Start is called before the first frame update
    void Start()
    {
        List<Vector3> points1 = new List<Vector3>();
        List<Vector3> points2 = new List<Vector3>();
        for (int i = -1; i < 2; i += 2)
        {

            for (int j = -1; j < 2; j += 2)
            {
                for (int k = -1; k < 2; k += 2)
                {
                    points1.Add(cube1.transform.position + new Vector3(i * (cube1.transform.localScale.x / 2f),
                                                                             j * (cube1.transform.localScale.y / 2f),
                                                                             k * (cube1.transform.localScale.z / 2f)));

                    points2.Add(cube2.transform.position + new Vector3(i * (cube2.transform.localScale.x / 2f),
                                                                            j * (cube2.transform.localScale.y / 2f),
                                                                            k * (cube2.transform.localScale.z / 2f)));

                }
            }
        }
        var minkPoints = new List<Vector3>();
        foreach (var p1 in points1)
        {
            foreach (var p2 in points2)
            {
                minkPoints.Add(p1 - p2);
                var sphere = GameObject.Instantiate(PrefGameObject, p1 - p2, Quaternion.identity);
                points.Add(sphere);
            }
        }
    }
}