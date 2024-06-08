using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Shape : MonoBehaviour
{
    public GameObject supportPoint;
    public List<Vector3> verts;
    public virtual Vector3 Support(Vector3 direction)
    {
        if (verts == null || verts.Count == 0)
        {
            GetVertices();
        }

        var p = verts[0];
        for (int i = 0; i < verts.Count; i++)
        {
            if (Vector3.Dot(verts[i], direction) > Vector3.Dot(p, direction))
            {
                p = verts[i];
            }
        }
        Instantiate(supportPoint, p, Quaternion.identity, transform);
        return p;
    }
    protected virtual void GetVertices()
    {
        /* for (var i = -1; i < 2; i += 2)
         {
             var dx = i * transform.localScale.x/2;
             for (var j = -1; j < 2; j += 2)
             {
                 var dy = j * transform.localScale.y/2;
                 for (var k = -1; k < 2; k += 2)
                 {
                     var dz = k * transform.localScale.z/2;
                     verts.Add(transform.position + new Vector3(dx, dy, dz));
                 }
             }
         }
        */


        var meshFilter = GetComponent<MeshFilter>();
        verts ??= new List<Vector3>();

        foreach (var vertex in meshFilter.mesh.vertices)
        {
            verts.Add(transform.position + vertex);
        }

    }
}