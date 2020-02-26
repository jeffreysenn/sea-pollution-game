using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class FlowEditor : EditorWindow
{
    GameObject flowPrefab = null;
    bool isEditing = false;
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
        if (currentEvent == null) { return; }

        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0)
        {
            var hitNode = GetHitNode(currentEvent.mousePosition);
            if (hitNode)
            {
                if (!flow)
                {
                    CreateFlow(hitNode);
                }
                else
                {
                    CompleteFlow(hitNode);
                }
            }
        }

        if (flow)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
            {
                EditorUtility.SetDirty(flow.GetInNode());
                flow.OnDisable();
                DestroyImmediate(flow.gameObject);
                flow = null;
            }
            else
            {
                var l = flow.GetComponent<LineRenderer>();
                var ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                var pos = ray.origin + ray.direction * 100;
                l.SetPosition(1, pos);
            }
        }
    }

    void CompleteFlow(Node hitNode)
    {
        EditorUtility.SetDirty(hitNode);
        hitNode.AddInFlow(flow);

        EditorUtility.SetDirty(flow);
        flow.SetOutNode(hitNode);

        var l = flow.GetComponent<LineRenderer>();
        l.SetPosition(1, hitNode.transform.position);
        var arrow = flow.GetComponent<LineRendererArrow>();
        arrow.origin = flow.GetInNode().transform.position;
        arrow.target = flow.GetOutNode().transform.position;
        arrow.UpdateArrow();

        var boxCollider = flow.gameObject.AddComponent<BoxCollider>();
        var size = boxCollider.size;
        size.z = 2 * size.z;
        boxCollider.size = size;

        flow = null;
    }

    void CreateFlow(Node hitNode)
    {
        var flowObj = (GameObject)PrefabUtility.InstantiatePrefab(flowPrefab);
        flow = flowObj.GetComponent<Flow>();
        EditorUtility.SetDirty(hitNode);
        hitNode.AddOutFlow(flow);
        flow.SetInNode(hitNode);
        var l = flow.GetComponent<LineRenderer>();
        l.SetPosition(0, hitNode.transform.position);
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
