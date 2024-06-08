using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BakeTransform 
{
	// Apply an individual transform.
	public static void ApplyTransform(
		Transform transform,
		bool applyTranslation,
		bool applyRotation,
		bool applyScale)
	{
		var meshFilter = transform.GetComponent<MeshFilter>();
		if (meshFilter != null)
		{
			Debug.Log("MeshApplyTransform:: Baking mesh for object (" + transform.name + ").");
			var originalMeshName = meshFilter.sharedMesh.name;

			var newMesh = ApplyTransform(
				transform,
				GameObject.Instantiate(meshFilter.sharedMesh),
				applyTranslation, applyRotation, applyScale);

		//	Undo.RegisterCompleteObjectUndo(transform, "Apply Transform");

			meshFilter.sharedMesh = newMesh;

		/*	if (!AssetDatabase.IsValidFolder("Assets/Baked Meshes"))
				AssetDatabase.CreateFolder("Assets", "Baked Meshes");
		*/
			var prefabPath = "";
			if (originalMeshName.StartsWith("BakedMesh"))
			{
				Debug.Log("MeshApplyTransform:: Replacing existing baked mesh (" + originalMeshName + ").");
				prefabPath = "Assets/Baked Meshes/" + originalMeshName + ".asset";
			}
			else
			{
				prefabPath = string.Format("Assets/Baked Meshes/BakedMesh_{0}_{1}_{2}.asset",
					transform.name, originalMeshName, (int)Mathf.Abs(newMesh.GetHashCode()));
			}

		//	AssetDatabase.CreateAsset(newMesh, prefabPath);
		//	AssetDatabase.SaveAssets();
		}

		// Even with no mesh filter, might still reset the transform
		// of a parent game object, i.e., reset either way.
		ResetTransform(transform,
				applyTranslation, applyRotation, applyScale);
	}

	// Apply a transform to a mesh. The transform needs to be
	// reset also after this application to keep the same shape.
	private static Mesh ApplyTransform(
		Transform transform,
		Mesh mesh,
		bool applyTranslation,
		bool applyRotation,
		bool applyScale)
	{
		var verts = mesh.vertices;
		var norms = mesh.normals;

		// Handle vertices.
		for (int i = 0; i < verts.Length; ++i)
		{
			var nvert = verts[i];

			if (applyScale)
			{
				var scale = transform.localScale;
				nvert.x *= scale.x;
				nvert.y *= scale.y;
				nvert.z *= scale.z;
			}

			if (applyRotation)
			{
				nvert = transform.rotation * nvert;
			}

			if (applyTranslation)
			{
				nvert += transform.position;
			}

			verts[i] = nvert;
		}

		// Handle normals.
		for (int i = 0; i < verts.Length; ++i)
		{
			var nnorm = norms[i];

			if (applyRotation)
			{
				nnorm = transform.rotation * nnorm;
			}

			norms[i] = nnorm;
		}

		mesh.vertices = verts;
		mesh.normals = norms;

		mesh.RecalculateBounds();
		mesh.RecalculateTangents();

		return mesh;
	}

	// Reset the transform values, this should be executed after
	// applying the transform to the mesh data.
	private static Transform ResetTransform(
		Transform transform,
		bool applyTranslation,
		bool applyRotation,
		bool applyScale)
	{
		var scale = transform.localScale;
		var rotation = transform.localRotation;
		var translation = transform.position;

		// Update the children to keep their shape.
		foreach (Transform child in transform)
		{
			if (applyTranslation)
				child.Translate(transform.localPosition);

			if (applyRotation)
			{
				var worldPos = rotation * child.localPosition;
				child.localRotation = rotation * child.localRotation;
				child.localPosition = worldPos;
			}

			if (applyScale)
			{
				var childScale = child.localScale;
				childScale.x *= scale.x;
				childScale.y *= scale.y;
				childScale.z *= scale.z;
				child.localScale = childScale;

				var childPosition = child.localPosition;
				childPosition.x *= scale.x;
				childPosition.y *= scale.y;
				childPosition.z *= scale.z;
				child.localPosition = childPosition;
			}

			// This makes the inspector update the position values.
			child.Translate(Vector3.zero);
			// Though for some reason the .position value is still screwed
			// for this frame though.
		}

		// Reset the transform.
		if (applyTranslation)
			transform.position = Vector3.zero;
		if (applyRotation)
			transform.rotation = Quaternion.identity;
		if (applyScale)
			transform.localScale = Vector3.one;

		return transform;
	}
}

