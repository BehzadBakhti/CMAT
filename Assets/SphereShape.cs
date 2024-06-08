using UnityEngine;

public class SphereShape : Shape
{
    public override Vector3 Support(Vector3 direction)
    {
       var p= transform.position + direction.normalized * transform.localScale.x / 2;
       Instantiate(supportPoint, p, Quaternion.identity,transform);
       return p;
    }
    protected override void GetVertices()
    {
        base.GetVertices();
        for (int i = 0; i < verts.Count; i++)
        {
            verts[i] *= transform.localScale.x;
        }
    }
}


// Vianna ghahreman