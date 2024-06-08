using UnityEngine;
using System.Collections.Generic;
using UnityEngine.ProBuilder;

public class MeshModifier : MonoBehaviour
{
    [SerializeField] Transform rootBone;
    [SerializeField] List<Transform> bones;
    [SerializeField] Vector3[] vertices;
    [SerializeField] GameObject mainMesh;

    private void Start()
    {
        var smr = mainMesh.GetComponent<SkinnedMeshRenderer>();
        var mesh = smr.sharedMesh;
        vertices = mesh.vertices;
      
        var a = mesh.GetVertices();
        GetAllBones(rootBone);
        smr.bones = bones.ToArray();
        smr.rootBone = rootBone;

        BoneWeight[] weights = SetBoaneWeights(vertices, bones.ToArray());
        mesh.boneWeights = weights;

        mesh.RecalculateBounds();

        Matrix4x4[] bindPoses = new Matrix4x4[bones.Count];
        for (int i = 0; i < bones.Count; i++)
        {
            bindPoses[i] = bones[i].worldToLocalMatrix * rootBone.localToWorldMatrix;
        }
            mesh.bindposes = bindPoses;

    }

    private void GetAllBones(Transform root)
    {
        bones.Add(root);
        for (int i = 0; i < root.childCount; i++)
        {
            GetAllBones(root.GetChild(i));
        }
        return;
    }
    private BoneWeight[] SetBoaneWeights(Vector3[] vertices, Transform[] bones)
    {
        BoneWeight[] weights = new BoneWeight[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            var dist = float.MaxValue;
            for (int j = 0; j < bones.Length; j++)
            {
                var newdist = Vector3.Distance(vertices[i], bones[j].position);
                if (dist > newdist)
                {
                    dist = newdist;
                    weights[i].boneIndex0 = j;
                    weights[i].weight0 = 1;
                }
            }
        }
        return weights;
    }
}


