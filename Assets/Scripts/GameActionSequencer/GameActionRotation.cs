using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class GameActionRotation : AGameAction
{
    [Header("Rotation")]
    public Transform target;
    public float secondsToRotate = 3f;
    public bool endThisBeforeContinue = false;

    [Header("How to rotate this")]
    public RotateMode rotateMode;
    public HowRotate howRotate;
    public enum HowRotate
    {
        CopyObjectRotation,
        WorldRotation,
        AddVectorToCurrentScale,
    }

    [HideInInspector] public Vector3 newRotationWorld = new Vector3(90, 90, 90);
    [HideInInspector] public Vector3 newRotationOffset = new Vector3(0, 90, 0);
    [HideInInspector] public Transform objectToCopy;
    [HideInInspector] public Vector3 rotationToGo;

    private float timer = 0f;
    private Rigidbody _targetRb;


    private bool _isCompleted;


    void OnDrawGizmos()
    {
        ActualiseRotationToGo();
    }

    void OnValidate()
    {
        ActualiseRotationToGo();
    }

    void ActualiseRotationToGo()
    {
        switch (howRotate)
        {
            case HowRotate.CopyObjectRotation:
                rotationToGo = objectToCopy.rotation.eulerAngles;
                break;
            case HowRotate.WorldRotation:
                rotationToGo = newRotationWorld;
                break;
            case HowRotate.AddVectorToCurrentScale:
                rotationToGo = newRotationOffset;
                break;
        }
    }

    protected override void OnExecute()
    {
        ActualiseRotationToGo();
        timer = secondsToRotate;
        target.DORotate(rotationToGo, secondsToRotate, rotateMode);

        if (target.GetComponent<Rigidbody>() != null)
        {
            _targetRb = target.GetComponent<Rigidbody>();
        }
        if (!endThisBeforeContinue)
            _isCompleted = true;
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

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        string newVector = "x: " + rotationToGo.x.ToString() + "  | y: " + rotationToGo.y.ToString() + "  | z: " + rotationToGo.z.ToString();
        return $"[GARotate] : {target.name} to ( {newVector} ) in {secondsToRotate} seconds";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameActionRotation))]
public class GameActionRotationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameActionRotation gaRotation = (GameActionRotation)target;
        base.OnInspectorGUI();

        switch (gaRotation.howRotate)
        {
            case GameActionRotation.HowRotate.CopyObjectRotation:
                gaRotation.objectToCopy = EditorGUILayout.ObjectField("New Position", gaRotation.objectToCopy, typeof(Transform), true) as Transform;
                break;
            case GameActionRotation.HowRotate.WorldRotation:
                gaRotation.newRotationWorld = EditorGUILayout.Vector3Field("New Position", gaRotation.newRotationWorld);
                break;
            case GameActionRotation.HowRotate.AddVectorToCurrentScale:
                gaRotation.newRotationOffset = EditorGUILayout.Vector3Field("New Position", gaRotation.newRotationOffset);
                break;
        }

        EditorGUILayout.Space(10);

        string rotationAfter = "x: " + (gaRotation.rotationToGo.x % 360).ToString() + "  | y: " + (gaRotation.rotationToGo.y % 360).ToString() + "  | z: " + (gaRotation.rotationToGo.z % 360).ToString();
        EditorGUILayout.HelpBox("After rotating, the object rotation will be " + rotationAfter, MessageType.Info);
        if (GUILayout.Button("Actualise Gizmos"))
        {
            if (!EditorWindow.GetWindow<SceneView>().drawGizmos)
                EditorWindow.GetWindow<SceneView>().drawGizmos = true;
            if (gaRotation.transform.parent.GetComponent<GameActionsSequencer>().debug == GameActionsSequencer.DebugWay.none)
                gaRotation.transform.parent.GetComponent<GameActionsSequencer>().debug = GameActionsSequencer.DebugWay.onlyInEditor;
        }

    }
}
#endif