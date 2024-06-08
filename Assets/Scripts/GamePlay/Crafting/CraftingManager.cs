using System.Collections.Generic;
using System.Linq;
using Crafting;
using Newtonsoft.Json;
using RuntimeGizmos;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{

    [SerializeField] private TransformGizmo cameraGizmo;
    private CraftedObject _activeCraftedObject;
    private List<CraftingPart> _selectedParts;
    private void Awake()
    {
        cameraGizmo.TransformHandleReleased += CheckForNearFaces;
    }

    private void CheckForNearFaces(List<Transform> transforms)
    {

        foreach (var t in transforms)
        {
            BakeTransform.ApplyTransform(t,false,false,true);
            var craftingPart = t.GetComponent<CraftingPart>();
            if (craftingPart is null)
                continue;
            foreach (var face in craftingPart.faceDetection.Faces)
            {
                var center = (t.position + t.rotation * face.Value.Aggregate(new Vector3(0, 0, 0), (s, v) => s + v) / face.Value.Count);
                var ray = new Ray(center, t.rotation * face.Key);

                if (!Physics.Raycast(ray, out var hit) || hit.collider.transform.root == t.root) continue;
                var misAlignment = (hit.normal.normalized + (t.rotation * face.Key).normalized).magnitude;
                if (misAlignment <= 0.05f && hit.distance < 0.2f)// TODO Magic Numbers Fix Needed
                {
                    t.position-=hit.distance* hit.normal;
                    t.parent = hit.transform;
                    Debug.Log("Connect them baby");

                    /****** Or We Can Use PARENT CONSTRAIN Class ********/ 
                }
            }
        }
    }

    public void Save()
    {

        var craftingData = PlayerPrefs.GetString("craftedObjects");
        var co = JsonConvert.DeserializeObject<CraftingData>(craftingData);
        //   co?.CraftedObjects.TryAdd(id, _activeCraftedObject);
        var t = JsonConvert.SerializeObject(_activeCraftedObject);
        PlayerPrefs.SetString("craftedObjects", t);
    }
}