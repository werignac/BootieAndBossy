using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TensionResolver), true)]
public class TensionResolverEditor : Editor
{
	private void OnSceneGUI()
	{
		Debug.Log("Called");
		
		TensionResolver resolver = (TensionResolver)target;
		HingeJoint2D hinge = (HingeJoint2D)serializedObject.FindProperty("_hinge").objectReferenceValue;
		Handles.Label(resolver.transform.TransformPoint(hinge.anchor) + new Vector3(0,0,-1), resolver.GetAngle().ToString());
	}
}
