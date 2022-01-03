using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class GameActionMove : AGameAction
{
    [Header("Move")]
    [Tooltip("The object which will move")] public Transform target;
    [Tooltip("Time in seconds to move")] public float secondsToMove = 3f;

    [Header("Specifics")]
    [Tooltip("If true, the object wont change his y position")] public bool freezeY = false;
    [Tooltip("If false, this object must end his movement before allowing the next one to move")] public bool endThisBeforeContinue = true;

    [Header("How to move this")]
    [Tooltip("How will move the object")] public MovementMode vectorOrTarget;
    [HideInInspector] public Transform pointB;
    [HideInInspector] public Vector3 newVector;
    [HideInInspector] public Vector3 newVectorOffset;


    private bool _isCompleted;

    private float timer = 0f;

    [HideInInspector] public Vector3 posToGo;
    public enum MovementMode
    {
        WorldPositionVector,
        CopyObjectPosition,
        AddVectorToCurrentPosition,
    }


    void OnDrawGizmos()
    {
        ActualisePosToGo();
    }

    void OnValidate()
    {
        ActualisePosToGo();
    }

    void ActualisePosToGo()
    {
        switch (vectorOrTarget)
        {
            case MovementMode.CopyObjectPosition:
                if (freezeY)
                    posToGo = new Vector3(pointB.position.x, target.position.y, pointB.position.z);
                else
                    posToGo = new Vector3(pointB.position.x, pointB.position.y, pointB.position.z);
                break;
            case MovementMode.WorldPositionVector:
                if (freezeY)
                    posToGo = new Vector3(newVector.x, target.position.y, newVector.z);
                else
                    posToGo = new Vector3(newVector.x, newVector.y, newVector.z);
                break;
            case MovementMode.AddVectorToCurrentPosition:
                if (freezeY)
                    posToGo = new Vector3(newVectorOffset.x + target.position.x, target.position.y, newVectorOffset.z + target.position.z);
                else
                    posToGo = new Vector3(newVectorOffset.x + target.position.x, newVectorOffset.y + target.position.y, newVectorOffset.z + target.position.z);
                break;
        }
    }

    protected override void OnExecute()
    {
        _isCompleted = false;
        timer = secondsToMove;

        ActualisePosToGo();

        target.DOMove(posToGo, secondsToMove);

        if (!endThisBeforeContinue)
        {
            _isCompleted = true;
        }
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        string newVector = "x: " + posToGo.x.ToString() + "  | y: " + posToGo.y.ToString() + "  | z: " + posToGo.z.ToString();
        return $"[GAMove] {target.name} to ( {newVector} ) in {secondsToMove} seconds";
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

[CustomEditor(typeof(GameActionMove))]
public class GameActionMoveEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GameActionMove gaMove = (GameActionMove)target;
        base.OnInspectorGUI();


        switch (gaMove.vectorOrTarget)
        {
            case GameActionMove.MovementMode.CopyObjectPosition:
                gaMove.pointB = EditorGUILayout.ObjectField("New Position", gaMove.pointB, typeof(Transform), true) as Transform;
                break;
            case GameActionMove.MovementMode.WorldPositionVector:
                gaMove.newVector = EditorGUILayout.Vector3Field("New Position", gaMove.newVector);
                break;
            case GameActionMove.MovementMode.AddVectorToCurrentPosition:
                gaMove.newVectorOffset = EditorGUILayout.Vector3Field("New Position", gaMove.newVectorOffset);
                break;
        }

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Actualise Gizmos"))
        {
            if (!EditorWindow.GetWindow<SceneView>().drawGizmos)
                EditorWindow.GetWindow<SceneView>().drawGizmos = true;
            if (gaMove.transform.parent.GetComponent<GameActionsSequencer>().debug == GameActionsSequencer.DebugWay.none)
                gaMove.transform.parent.GetComponent<GameActionsSequencer>().debug = GameActionsSequencer.DebugWay.onlyInEditor;
        }
    }
}
#endif