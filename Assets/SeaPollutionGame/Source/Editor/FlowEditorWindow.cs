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
    Flow editingFlow = null;

    float nodeRadius = 0.5f;
    float colliderWidth = 0.6f;
    float colliderHeight = 1.0f;
    float percentHead = 0.4f;

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
                if (!editingFlow)
                {
                    CreateFlow(hitNode);
                }
                else if (editingFlow.GetInNode() != hitNode)
                {
                    CompleteFlow(hitNode);
                }
            }
        }

        if (editingFlow)
        {
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
            {
                EditorUtility.SetDirty(editingFlow.GetInNode());
                editingFlow.OnDisable();
                DestroyImmediate(editingFlow.gameObject);
                editingFlow = null;
            }
            else
            {
                var l = editingFlow.GetComponent<LineRenderer>();
                var ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
                var pos = ray.origin + ray.direction * 100;
                l.SetPosition(1, pos);
            }
        }
    }


    void OnScene(SceneView sceneview)
    {
        if (isEditing) { EditFlow(); }
        if (autoUpdateFlow) { UpdateSelectedFlows(); }
    }

    private void UpdateSelectedFlows()
    {
        if (!editingFlow)
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
                                UpdateFlow(flow);
                            }
                        }
                    }
                }
            }
        }
    }

    private void UpdateFlow(Flow flow)
    {
        UpdateArrow(flow);
        UpdateCollider(flow);
    }

    private void UpdateArrow(Flow flow)
    {
        var origin = flow.GetInNode();
        var target = flow.GetOutNode();
        if (!(origin && target)) { return; }
        var cachedLineRenderer = flow.GetComponent<LineRenderer>();
        cachedLineRenderer.widthCurve = new AnimationCurve(
            new Keyframe(0, 0.4f),
            new Keyframe(0.999f - percentHead, 0.4f), // neck of arrow
            new Keyframe(1 - percentHead, 1f),  // max width of arrow head
            new Keyframe(1, 0f));  // tip of arrow
        cachedLineRenderer.positionCount = 4;
        var originPos = origin.transform.position;
        var targetPos = target.transform.position;
        cachedLineRenderer.SetPositions(new Vector3[] {
              originPos,
              Vector3.Lerp(originPos, targetPos, 0.999f - percentHead),
              Vector3.Lerp(originPos, targetPos, 1 - percentHead),
              targetPos });
    }

    private void UpdateCollider(Flow flow)
    {
        var originPos = flow.GetInNode().transform.position;
        var targetPos = flow.GetOutNode().transform.position;
        var delta = targetPos - originPos;
        float distance = delta.magnitude;
        flow.transform.position = originPos + delta / 2;
        flow.transform.right = delta.normalized;

        var boxCollider = flow.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(distance - 2 * nodeRadius, colliderHeight, colliderWidth);
        boxCollider.center = Vector3.zero;
    }

    void CompleteFlow(Node hitNode)
    {
        EditorUtility.SetDirty(hitNode);
        hitNode.AddInFlow(editingFlow);

        EditorUtility.SetDirty(editingFlow);
        editingFlow.SetOutNode(hitNode);

        UpdateFlow(editingFlow);

        editingFlow = null;
    }

    void CreateFlow(Node hitNode)
    {
        var flowObj = (GameObject)PrefabUtility.InstantiatePrefab(flowPrefab, hitNode.transform.parent);
        editingFlow = flowObj.GetComponent<Flow>();
        EditorUtility.SetDirty(hitNode);
        hitNode.AddOutFlow(editingFlow);
        editingFlow.SetInNode(hitNode);
        var l = editingFlow.GetComponent<LineRenderer>();
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
        nodeRadius = EditorGUILayout.FloatField("Node Radius", nodeRadius);
        colliderWidth = EditorGUILayout.FloatField("Collider Width", colliderWidth);
        colliderHeight = EditorGUILayout.FloatField("Collider Height", colliderHeight);
        percentHead = EditorGUILayout.FloatField("Percent Arrow Head", percentHead);

        if (GUILayout.Button("Delete Flow"))
        {
            var objs = Selection.gameObjects;
            foreach (var obj in objs)
            {
                var flow = obj.GetComponent<Flow>();
                if (flow)
                {
                    EditorUtility.SetDirty(flow.GetInNode());
                    EditorUtility.SetDirty(flow.GetOutNode());
                    flow.OnDisable();
                    DestroyImmediate(flow.gameObject);
                }
            }
        }

        if (GUILayout.Button("Find bugged Node"))
        {
            var objs = Selection.gameObjects;
            var buggedNodes = new List<GameObject> { };
            foreach (var obj in objs)
            {
                foreach (var node in obj.GetComponentsInChildren<Node>())
                {
                    foreach (var editingFlow in node.GetAllFlows())
                    {
                        if (editingFlow)
                        {
                            var outNode = editingFlow.GetOutNode();
                            if (outNode.GetInFlows().Find(f => f == editingFlow))
                            {
                                continue;
                            }
                        }
                        buggedNodes.Add(node.gameObject);
                    }
                }
            }
            Selection.objects = buggedNodes.ToArray();
        }
    }
}
