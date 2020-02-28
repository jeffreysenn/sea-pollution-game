using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects]
public class FlowEditorWindow : EditorWindow
{
    GameObject flowPrefab = null;
    bool isEditing = true;
    bool autoUpdateFlow = true;
    Flow flow = null;

    [MenuItem("Window/" + "FlowEditor")]
    public static void Init()
    {
        FlowEditorWindow window = GetWindow<FlowEditorWindow>();
        window.titleContent = new GUIContent { text = "Flow editor" };
    }

    void OnEnable()
    {
        flowPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(
                                       "Assets/SeaPollutionGame/Prefabs/NewNodes/Flow.prefab");
        SceneView.duringSceneGui += OnScene;
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

    private void EditFlow()
    {
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
                else if (flow.GetInNode() != hitNode)
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

    void OnScene(SceneView sceneview)
    {
        if (isEditing) { EditFlow(); }
        if (autoUpdateFlow) { UpdateFlow(); }
    }

    private void UpdateFlow()
    {
        if (!flow)
        {
            var objs = Selection.gameObjects;
            foreach (var obj in objs)
            {
                {
                    var flow = obj.GetComponent<Flow>();
                    if (flow)
                    {
                        UpdateArrow(flow);
                        UpdateCollider(flow);
                    }
                }
                {
                    var node = obj.GetComponent<Node>();
                    if (node)
                    {
                        foreach (var flow in node.GetAllFlows())
                        {
                            if (flow)
                            {
                                UpdateArrow(flow);
                                UpdateCollider(flow);
                            }
                        }
                    }
                }
            }
        }
    }

    private void UpdateArrow(Flow flow)
    {
        var arrow = flow.GetComponent<LineRendererArrow>();
        arrow.UpdateArrow();
    }

    private void UpdateCollider(Flow flow)
    {
        var colliderUpdater = flow.GetComponent<FlowColliderUpdater>();
        colliderUpdater.UpdateFlowCollider();
    }

    void CompleteFlow(Node hitNode)
    {
        EditorUtility.SetDirty(hitNode);
        hitNode.AddInFlow(flow);

        EditorUtility.SetDirty(flow);
        flow.SetOutNode(hitNode);

        var l = flow.GetComponent<LineRenderer>();
        l.SetPosition(1, hitNode.transform.position);

        var colliderUpdater = flow.GetComponent<FlowColliderUpdater>();
        colliderUpdater.UpdateFlowCollider();

        flow = null;
    }

    void CreateFlow(Node hitNode)
    {
        var flowObj = (GameObject)PrefabUtility.InstantiatePrefab(flowPrefab, hitNode.transform.parent);
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
        autoUpdateFlow = EditorGUILayout.Toggle("Automatically Update Flow", autoUpdateFlow);
    }
}
