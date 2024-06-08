using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public class Simplex3
{
    private readonly float Threshold = 0.00001f;

    public List<Vector3> Points { get; } = new(4);

    public int Size => Points.Count;

    public void Add(Vector3 newPoint)
    {
        Points.Add(newPoint);
    }

    public (bool intersected, Vector3 newDirection) ContainsOrigin()
    {
        Vector3 newDirection;
        var result = false;

        if (Size <= 1)
            throw new ConstraintException("The minimum number of point in the simplex is 2 points.");

        // get the last point added to the simplex
        var a = Points[Size - 1];
        var ao = Vector3.zero - a;

        if (Size == 4) // the tetrahedron case
        {
            // get b , c and d
            var b = Points[Size - 2];
            var c = Points[Size - 3];
            var d = Points[Size - 4];

            // compute the edges vectors
            var ab = b - a;
            var ac = c - a;
            var ad = d - a;

            var abcPerp = Vector3.Cross(ab, ac);
            var acdPerp = Vector3.Cross(ac, ad);
            var adbPerp = Vector3.Cross(ad, ab);

            // Test to check where is origin with respect to the side planes
            var planeTests = (Vector3.Dot(ao, abcPerp) > 0 ? 0x1 : 0) |
                             (Vector3.Dot(ao, acdPerp) > 0 ? 0x2 : 0) |
                             (Vector3.Dot(ao, adbPerp) > 0 ? 0x4 : 0);

            switch (planeTests)
            {
                case 0x0:
                    // the origin is below all planes , 
                    newDirection = default;
                    result = true;
                    break;

                case 0x1:
                    // it is just above abc plane so the point d must be removed
                    // and a new point should be found in the abcCross direction
                    Points.Remove(d);
                    newDirection = abcPerp;
                    break;

                case 0x2:
                    // it is just above acd plane so the point b must be removed
                    // and a new point should be found in the acdCross direction
                    Points.Remove(b);
                    newDirection = acdPerp;
                    break;

                case 0x4:
                    // it is just above adb plane so the point c must be removed
                    // and a new point should be found in the adbCross direction
                    Points.Remove(c);
                    newDirection = adbPerp;

                    // swap the [Size - 2] and [Size - 3] points
                    Points[Size - 2] = a; // it is true because the c was removed before this step
                    Points[Size - 3] = b;
                    break;

                case 0x1 | 0x2:
                    // above both abc and acd
                    if (Vector3.Dot(ao, Vector3.Cross(abcPerp, ac)) > 0) // origin is on the acd side
                    {
                        //like above acd region
                        Points.Remove(b);
                        newDirection = acdPerp;
                    }
                    else
                    {
                        // like above abc region
                        Points.Remove(d);
                        newDirection = abcPerp;
                    }
                    break;

                case 0x1 | 0x4:
                    // above both abc and adb
                    if (Vector3.Dot(ao, Vector3.Cross(abcPerp, ab)) > 0)
                    {
                        //like above adb region
                        Points.Remove(c);
                        newDirection = adbPerp;
                        // swap the [Size - 2] and [Size - 3] points
                        Points[Size - 2] = a; // it is true because the c was removed before this step
                        Points[Size - 3] = b;
                    }
                    else
                    {
                        // like above abc region
                        Points.Remove(d);
                        newDirection = abcPerp;
                    }
                    break;

                case 0x2 | 0x4:
                    // above both acd and adb
                    if (Vector3.Dot(ao, Vector3.Cross(acdPerp, ad)) > 0)
                    {
                        //like above adb region
                        Points.Remove(c);
                        newDirection = adbPerp;

                        // swap the [Size - 2] and [Size - 3] points
                        Points[Size - 2] = a; // it is true because the c was removed before this step
                        Points[Size - 3] = b;
                    }
                    else
                    {
                        // like above abc region
                        Points.Remove(b);
                        newDirection = acdPerp;
                    }
                    break;

                case 0x1 | 0x2 | 0x4:
                    // above all planes, there is no collision
                    var bc = c - b;
                    var bd = d - b;
                    newDirection = Vector3.Cross(bc, bd);
                    break;

                default:
                    newDirection = default;
                    break;
            }
        }
        else if (Size == 3) //The triangle case
        {
            // get b and c
            var b = Points[Size - 2];
            var c = Points[Size - 3];

            // compute the edges
            var ab = b - a;
            var ac = c - a;

            // compute the normals to ab and ac toward out side of triangle,  (AC x AB) x AB, and  (AB x AC) x AC
            var abPerp = TripleProduct(ac, ab, ab);
            var acPerp = TripleProduct(ab, ac, ac);

            // the origin is out side of the triangle near ab edge
            if (Vector3.Dot(abPerp, ao) > 0)
            {
                // Remove point c
                Points.Remove(c);
                // set the new direction to abPerp
                newDirection = abPerp;
            }
            else if (Vector3.Dot(acPerp, ao) > 0)// the origin is out side of the triangle near ac edge
            {
                // Remove point b
                Points.Remove(b);
                // set the new direction to acPerp
                newDirection = acPerp;
            }
            else
            {
                // the origin is in the triangle so we set the new direction normal to triangle towards origin 
                var abc = Vector3.Cross(ab, ac);// a Vector3 perpendicular to abc plane

                if (Vector3.Dot(abc, ao) < 0)
                {
                    // swap the [Size - 2] and [Size - 3] points
                    Points[Size - 2] = c;
                    Points[Size - 3] = b;

                }
                newDirection = (Vector3.Dot(ao, abc) > 0 ? 1 : -1) * abc; // make sure it is in the direction of origin

                if (newDirection.sqrMagnitude < Threshold)
                {
                    // the origin is exactly located on the triangle
                    result = true;
                    newDirection = abc; // just for adding an extra point to the simplex for EPA 
                }
            }
        }
        else //Size==2, the line segment case
        {
            var b = Points[Size - 2];
            var ab = b - a;

            // get the perp to AB in the direction of the origin, (AB x AO) x AB
            var abPerp = TripleProduct(ab, ao, ab);

            newDirection = abPerp;
            if (newDirection.sqrMagnitude < Threshold) // the origin is aligned to the line segment
            {

                if (a.sqrMagnitude + b.sqrMagnitude == ab.sqrMagnitude)
                {
                    // the origin is located on the line segment
                    result = true;
                }

                Points.Remove(b);
                // find a Vector3 p perpendicular to ab for next direction which is Vector3(1,1,m)
                var m = -(ab.x + ab.y) / (1 + ab.z);
                newDirection = new Vector3(1, 1, m);
            }
        }

        return (result, newDirection);
    }



    public (Vector3 normal, float peneteration) EPA(Shape shapeA, Shape shapeB, List<Vector3> simplex)
    {
        //TODO: if the number of vertices in the simplex is less than 4 we should add on more to be able to calculate.

        if (simplex.Count < 4)
        {

        }

        var triangles = new List<int>
        {
            0, 1, 2,
           1, 2, 3,
           2, 3, 0,
           3, 0, 1
        };
        var counter = 0;
        while (true)
        {
            counter++;
            if (counter >=1000)
            {

                Debug.LogError("not going to stop!");
                return (Vector3.zero, 0);
            }

            // obtain the feature (Face for 3D) closest to the origin on the Minkowski Difference convex hull
            var closestFace = FindClosestFace(simplex, triangles);

            // obtain a new support point in the direction of the face normal
            var p = shapeA.Support(closestFace.Normal) - shapeB.Support(-closestFace.Normal);

            // check the distance from the origin to the face against the
            // distance p is along e.normal
            var d = Vector3.Dot(p, closestFace.Normal);
            if (d - closestFace.Distance > Threshold)
            {
                // we haven't reached the face of the Minkowski Difference
                // so continue expanding by adding the new point to the simplex
                // new point will go to last index for simplicity 
                simplex.Add(p);
                var cvx = new ConvexHullCalculator();
                var normals = new List<Vector3>();
                cvx.GenerateHull(simplex.ToList(), false, ref simplex, ref triangles, ref normals);
                // remove the closest face from the triangle and replace it with new 3 triangles 
                //var cf = triangles[closestFace.Index];
                //triangles.RemoveAt(closestFace.Index);

                //var newTriangles = new List<(int, int, int)>
                //{
                //    (cf.Item1, simplex.Count - 1, cf.Item2),
                //    (cf.Item1, simplex.Count - 1, cf.Item3),
                //    (cf.Item2, simplex.Count - 1, cf.Item3),
                //};

                //triangles.InsertRange(closestFace.Index, newTriangles);
            }
            else
            {
                // if the difference is less than the tolerance then we can
                // assume that we cannot expand the simplex any further and
                // we have our solution

                return (closestFace.Normal, closestFace.Distance);
            }
        }
    }

    public Face FindClosestFace(List<Vector3> vertices, List<int> triangles)
    {
        var closest = new Face
        {
            Distance = float.MaxValue
        };

        for (var i = 0; i < triangles.Count; i+=3)
        {
            // get the points of the the triangle i
            var a = vertices[triangles[i]];
            var b = vertices[triangles[i+1]];
            var c = vertices[triangles[i+2]];

            // create the face vectors
            var ab = b - a;
            var ac = c - a;

            // get the Vector3 from the face towards the origin
            var n = Vector3.Cross(ab, ac);

            //fix the sign to be inline with the a (oa) vector
            n *= Vector3.Dot(n, a) > 0 ? 1 : -1;

            // normalize the Vector3
            n = n.normalized; //TODO: check if we can optimize the normal calculations using caching
            // calculate the distance from the origin to the face
            var d = Vector3.Dot(n, a);

            // check the distance against the other distances
            if (d < closest.Distance)
            {
                closest.Distance = d;
                closest.Normal = n;
                closest.Index = i;// the index of the triangle in the triangles collection
            }
        }

        return closest;
    }
    public Vector3 TripleProduct(Vector3 a, Vector3 b, Vector3 c)
    {
        //  return b * a.Dot(c) - a * b.Dot(c); // simplified form of (a x b) x c
        return Vector3.Cross(Vector3.Cross(a, b), c);
    }
}


public record Face
{
    public float Distance;
    public Vector3 Normal;
    public int Index;
}
