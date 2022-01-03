using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class GameActionScale : AGameAction
{
    [Header("Scale")]
    public Transform target;
    public float secondsToScale = 3f;
    public bool endThisBeforeContinue = true;
    [Header("How to scale this")]
    public ScaleMode scaleMode = ScaleMode.WorldVector;
    [HideInInspector] public Transform objectToCopy;
    [HideInInspector] public Vector3 newScale = new Vector3(2, 2, 2);
    [HideInInspector] public Vector3 newScaleOffset = new Vector3(1.5f, 1.5f, 1.5f);

    [HideInInspector] public Vector3 scaleToGo = Vector3.zero;
    float timer = 0f;
    bool _isCompleted;

    public enum ScaleMode
    {
        CopyObjectScale,
        WorldVector,
        AddVectorToCurrentScale,
    }
    void OnDrawGizmos()
    {
        ActualiseScaleToGo();
    }

    void OnValidate()
    {
        ActualiseScaleToGo();
    }

    void ActualiseScaleToGo()
    {
        switch (scaleMode)
        {
            case ScaleMode.CopyObjectScale:
                scaleToGo = objectToCopy.transform.localScale;
                break;
            case ScaleMode.WorldVector:
                scaleToGo = newScale;
                break;
            case ScaleMode.AddVectorToCurrentScale:
                scaleToGo = newScaleOffset + target.transform.localScale;
                break;
        }
    }

    protected override void OnExecute()
    {
        _isCompleted = false;
        timer = secondsToScale;

        ActualiseScaleToGo();
        target.DOScale(scaleToGo, secondsToScale);
        if (!endThisBeforeContinue)
            _isCompleted = true;
    }
    public override bool IsFinished()
    {
        return _isCompleted;
    }
    protected override string BuildGameObjectName()
    {
        string newVector = "x: " + scaleToGo.x.ToString() + "  | y: " + scaleToGo.y.ToString() + "  | z: " + scaleToGo.z.ToString();
        return $"[GAScale] : {target.name} to ( {newVector} ) in {secondsToScale} seconds";
    }
    public override void ActionUpdate()
    {
        if (endThisBeforeContinue)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                _isCompleted = true;
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(GameActionScale))]
public class GameActionScaleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameActionScale gaScale = (GameActionScale)target;
        base.OnInspectorGUI();

        switch (gaScale.scaleMode)
        {
            case GameActionScale.ScaleMode.CopyObjectScale:
                gaScale.objectToCopy = EditorGUILayout.ObjectField("Object to Copy", gaScale.objectToCopy, typeof(Transform), true) as Transform;
                break;
            case GameActionScale.ScaleMode.WorldVector:
                gaScale.newScale = EditorGUILayout.Vector3Field("New Scale", gaScale.newScale);
                break;
            case GameActionScale.ScaleMode.AddVectorToCurrentScale:
                gaScale.newScaleOffset = EditorGUILayout.Vector3Field("New Scale Offset", gaScale.newScaleOffset);
                break;
            default:
                break;
        }
        EditorGUILayout.Space(10);

        if (GUILayout.Button("Actualise Gizmos"))
        {
            if (!EditorWindow.GetWindow<SceneView>().drawGizmos)
                EditorWindow.GetWindow<SceneView>().drawGizmos = true;
            if (gaScale.transform.parent.GetComponent<GameActionsSequencer>().debug == GameActionsSequencer.DebugWay.none)
                gaScale.transform.parent.GetComponent<GameActionsSequencer>().debug = GameActionsSequencer.DebugWay.onlyInEditor;
        }
    }
}
#endif
