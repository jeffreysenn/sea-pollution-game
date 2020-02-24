using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;


[CustomEditor(typeof(Node))]
public class FlowEditor : Editor
{
    GameObject flowPrefab = null;


    void OnSceneGUI()
    {
        if (!flowPrefab) { flowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
            "Assets/SeaPollutionGame/Prefabs/NewNodes/Flow.prefab"); }
        var currentEvent = Event.current;
        if (currentEvent != null)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                var activeObj = (GameObject)Selection.activeObject;
                Debug.Log(activeObj.name);
                GameObject.Instantiate(flowPrefab);
                var lineRenderer = flowPrefab.GetComponent<LineRenderer>();
                lineRenderer.SetPosition(0, activeObj.transform.position);
            }
        }
    }
}
