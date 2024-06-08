using EzySlice;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.Shapes;
using Random = UnityEngine.Random;

public class GjkEpa : MonoBehaviour
{
    public GameObject PrefGameObject;
    public Shape shape1;
    public Shape shape2;
    public List<Vector3> verts;

    public Simplex3 simplex = new Simplex3();
    public bool isIntersecting;

    public Vector3 invasionVector;
    public float invasion;

    public List<int> triangles = new List<int>()
    {
        0, 1, 2,
       1, 2, 3,
       2, 3, 0,
       3, 0, 1
    };


    void Start()
    {
        RunGjk();
    }

    // Start is called before the first frame update
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddPoint();
        }

    }

    void RunGjk()
    {
        simplex = new Simplex3();
        isIntersecting = Intersect(shape1, shape2);

        foreach (var point in simplex.Points)
        {
            GameObject.Instantiate(PrefGameObject, point, Quaternion.identity);
        }
        verts = simplex.Points;



        for (var i = 0; i < triangles.Count - 1; i += 3)
        {
            Debug.Log($"Triangle at index {i} is {triangles[i]}");
            Debug.DrawLine(verts[triangles[i]], verts[triangles[i + 1]], Color.green, 100);
            Debug.DrawLine(verts[triangles[i + 1]], verts[triangles[i + 2]], Color.green, 100);
            Debug.DrawLine(verts[triangles[i + 2]], verts[triangles[i]], Color.green, 100);

        }

        if (isIntersecting)
        {
            (invasionVector, invasion) = simplex.EPA(shape1, shape2, simplex.Points);
        }
    }

    void AddPoint()
    {
        if (!isIntersecting)
        {
            Debug.Log(" No Intersection!");
            return;
        }

        // obtain the feature (Face for 3D) closest to the origin on the Minkowski Difference convex hull
        var closestFace = simplex.FindClosestFace(verts, triangles);
        // obtain a new support point in the direction of the face normal
        var p = shape1.Support(closestFace.Normal) - shape2.Support(-closestFace.Normal);


        verts.Add(p);
        var cvx = new ConvexHullCalculator();
        var normals = new List<Vector3>();
        cvx.GenerateHull(verts.ToList(), false, ref verts, ref triangles, ref normals);




        for (var i = 0; i < triangles.Count - 1; i += 3)
        {
            Debug.DrawLine(verts[triangles[i]], verts[triangles[i + 1]], Color.red, 10);
            Debug.DrawLine(verts[triangles[i + 1]], verts[triangles[i + 2]], Color.red, 10);
            Debug.DrawLine(verts[triangles[i + 2]], verts[triangles[i]], Color.red, 10);

        }



        GameObject.Instantiate(PrefGameObject, p, Quaternion.identity);
    }

    public bool Intersect(Shape shape, Shape other)
    {
        //Initial random search direction
        var rng = new System.Random();
        var direction0 = 5 * Vector3.one - new Vector3(rng.Next(10), rng.Next(10), rng.Next(10));

        var firstPoint = shape.Support(direction0) - other.Support(-direction0);
        simplex.Add(firstPoint);

        var direction = -direction0;

        const int loopLimit = 1000;
        var loopCounter = 0;
        while (true)
        {
            loopCounter++;
            if (loopCounter >= loopLimit)
            {
                throw new Exception("Endless loop");
            }

            var point = shape.Support(direction) - other.Support(-direction);
            simplex.Add(point);

            // make sure that the last point we added actually passed the origin
            if (Vector3.Dot(point, direction) < 0)
            {
                //no collision
                return false;
            }

            // is the origin inside the current simplex (line, triangle, or tetrahedron)
            var result = simplex.ContainsOrigin();
            if (result.intersected)
            {
                if (simplex.Points.Count < 4)
                {
                    var p = shape.Support(result.newDirection) - other.Support(-result.newDirection);
                    simplex.Add(p);
                }

                return true;
            }

            direction = result.newDirection;
        }
    }

}
