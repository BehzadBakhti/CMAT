using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxShape : Shape
{

    void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] c = new ContactPoint[collision.contactCount];
        collision.GetContacts(c);
        
        for (int i = 0; i < c.Length; i++)
        {
            Debug.Log(gameObject.name);
            Debug.Log(c[i].point);
            Debug.Log(c[i].normal);

            Debug.Log(c[i].separation);
        }
    }

    protected override void GetVertices()
    {
        for (var i = -1; i < 2; i += 2)
        {
            var dx = i * transform.localScale.x / 2;
            for (var j = -1; j < 2; j += 2)
            {
                var dy = j * transform.localScale.y / 2;
                for (var k = -1; k < 2; k += 2)
                {
                    var dz = k * transform.localScale.z / 2;
                    verts.Add(transform.position + new Vector3(dx, dy, dz));
                }
            }
        }
    }
}
