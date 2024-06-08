using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;


namespace Crafting
{
    public class FaceDetection : MonoBehaviour
    {
        public GameObject indicatorPrefab;
        public Dictionary<Vector3, List<Vector3>> Faces { get; private set; }

        private void Start()
        {
            UpdateFaces();
        }

        public void UpdateFaces()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            var normals = new List<Vector3>();
            var verts = new List<Vector3>();
            mesh.GetNormals(normals);
            mesh.GetVertices(verts);

            var refVects = GetStartingVectors(24);
            var clusters = Cluster(refVects, normals);

            Faces = GetFaces(clusters, verts);


            // DrawFaces();

        }

        private void DrawFaces()
        {
            foreach (var face in Faces)
            {
                Vector3 meanPos = Vector3.zero;
                for (int i = 0; i < face.Value.Count; i++)
                {
                    meanPos += face.Value[i];
                }

                meanPos /= face.Value.Count;
                var go = Instantiate(indicatorPrefab, transform);
                go.transform.localPosition = meanPos;
                go.transform.localRotation = Quaternion.LookRotation(face.Key);

            }

        }

        private Dictionary<Vector3, List<Vector3>> GetFaces(Dictionary<Vector3, List<int>> faces,
            List<Vector3> vertices)
        {
            var result = new Dictionary<Vector3, List<Vector3>>();

            foreach (var face in faces)
            {
                var vertList = face.Value.Select(t => vertices[t]).ToList();
                result.Add(face.Key, vertList);
            }

            return result;
        }

        // should be developed in async mode
        private List<Vector3> GetStartingVectors(int fullCuts = 6) // number of cuts around circle
        {
            var vectors = new List<Vector3>();
            for (var i = 0; i < fullCuts; i++)
            {
                for (var j = 0; j < 1 + fullCuts / 2; j++)
                {
                    var theta = i * Mathf.PI * 2 / fullCuts;
                    var phi = j * Mathf.PI * 2 / fullCuts;

                    var x = Mathf.Cos(theta) * Mathf.Sin(phi);
                    var y = Mathf.Cos(phi);
                    var z = Mathf.Sin(theta) * Mathf.Sin(phi);
                    var vect = new Vector3(x, y, z).normalized;
                    vectors.Add(vect);
                }
            }

            return vectors;
        }

        private Dictionary<Vector3, List<int>> Cluster(List<Vector3> clusterCenters, List<Vector3> normals)
        {
            var result = new Dictionary<Vector3, List<int>>();
            var clusterList = new List<int>[clusterCenters.Count];
            for (var k = 0; k < 3; k++)
            {
                clusterList = new List<int>[clusterCenters.Count];

                for (var i = 0; i < normals.Count; i++)
                {

                    float minDist = 10;
                    var clusterIndex = 0;

                    for (var j = 0; j < clusterCenters.Count; j++)
                    {
                        var newDist = Vector3.Magnitude(clusterCenters[j] - normals[i]);
                        if (newDist < minDist)
                        {
                            minDist = newDist;
                            clusterIndex = j;
                        }
                    }

                    clusterList[clusterIndex] ??= new List<int>();
                    clusterList[clusterIndex].Add(i);
                }

                for (var i = 0; i < clusterCenters.Count; i++)
                {
                    if (clusterList[i] is null) continue;
                    var sum = Vector3.zero;
                    for (var j = 0; j < clusterList[i].Count; j++)
                    {
                        sum += normals[clusterList[i][j]];
                    }

                    clusterCenters[i] = sum / clusterList[i].Count;
                }
            }

            for (int i = 0; i < clusterCenters.Count; i++)
            {
                if (clusterList[i] is null)
                    continue;
                result.Add(clusterCenters[i], clusterList[i]);
                // Debug.DrawLine(transform.position, transform.position+clusterCenters[i], Color.green, 30);
            }

            return result;
        }
    }
}