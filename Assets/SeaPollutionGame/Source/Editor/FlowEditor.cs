using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;


public class FlowEditor : EditorWindow
{
    GameObject flowPrefab = null;
    bool isEditing = true;
    Node inNode = null;
    Flow flow = null;


    [MenuItem("Window/" + "FlowEditor")]
    public static void Init()
    {
        FlowEditor window = GetWindow<FlowEditor>();
        window.titleContent = new GUIContent { text = "Flow editor" };
        SceneView.duringSceneGui += window.OnScene;
    }

    void OnEnable()
    {
        flowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                                       "Assets/SeaPollutionGame/Prefabs/NewNodes/Flow.prefab");
    }

    Node GetHitNode(Vector2 mousePos)
    {
        var ray = HandleUtility.GUIPointToWorldRay(mousePos);
        RaycastHit[] hits = Physics.RaycastAll(ray, 9999);
        foreach (var hit in hits)
        {
            var node = hit.collider.GetComponent<Node>();
            if (node) { return node; }
        }
        return null;
    }

    void OnScene(SceneView sceneview)
    {
        if (!isEditing) { return; }

        var currentEvent = Event.current;
        if (currentEvent != null)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
            {
                if (!inNode)
                {
                    inNode = GetHitNode(currentEvent.mousePosition);
                    if (inNode)
                    {
                        var flowObj = GameObject.Instantiate(flowPrefab);
                        flow = flowObj.GetComponent<Flow>();
                        inNode.AddOutFlow(flow);
                        flow.SetInNode(inNode);
                        var l = flow.GetComponent<LineRenderer>();
                        l.SetPosition(0, inNode.transform.position);
                    }
                }
                else
                {
                    var outNode = GetHitNode(currentEvent.mousePosition);
                    if (outNode)
                    {
                        outNode.AddInFlow(flow);
                        flow.SetOutNode(outNode);
                        var l = flow.GetComponent<LineRenderer>();
                        l.SetPosition(1, outNode.transform.position);
                        inNode = null;
                        flow = null;
                    }
                }
            }

            if (inNode && flow)
            {
                var l = flow.GetComponent<LineRenderer>();
                var ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                var pos = ray.origin + ray.direction * 100;
                //pos = Vector3.ProjectOnPlane(pos, new Vector3(0,1,0));
                l.SetPosition(1, pos);
            }
        }

    }


    public void OnDestroy()
    {
        SceneView.duringSceneGui -= OnScene;
    }

    void OnGUI()
    {
        isEditing = EditorGUILayout.Toggle("Editing Mode On", isEditing);
    }
}
