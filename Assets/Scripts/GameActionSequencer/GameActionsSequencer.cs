using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameActionsSequencer : MonoBehaviour
{

    public bool playOnStart = true;
    public enum DebugWay { none, onlyInEditor, always }
    public DebugWay debug = DebugWay.none;

    private AGameAction[] _actions;
    private int _currentActionIndex = 0;

    private bool _isRunning = false;
    public bool IsRunning { get { return _isRunning; } }

    private bool _isStart = true;

    Mesh _mesh;

    public enum TransformAction { move, scale, rotation }

    [HideInInspector] public List<GameActionSequencerValues> listValues;

    #region Debug

    void OnDrawGizmos()
    {
        if (debug != DebugWay.none)
        {
            if (debug == DebugWay.onlyInEditor && Application.isPlaying)
            {
                return;
            }

            listValues.Clear();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<AGameAction>())
                {
                    AGameAction _gameAction = transform.GetChild(i).GetComponent<AGameAction>();
                    _gameAction.Rename();
                    if (_gameAction.GetComponent<GameActionMove>())
                    {
                        GameActionMove _gameActionMove = _gameAction.GetComponent<GameActionMove>();
                        ObjectToCompareGizmos(_gameActionMove.target, TransformAction.move, _gameActionMove.posToGo, _gameActionMove.target, _gameActionMove, _gameActionMove.posToGo);

                    }
                    else if (_gameAction.GetComponent<GameActionScale>())
                    {
                        GameActionScale _gameActionScale = _gameAction.GetComponent<GameActionScale>();
                        ObjectToCompareGizmos(_gameActionScale.target, TransformAction.scale, _gameActionScale.scaleToGo, _gameActionScale.target, _gameActionScale, Vector3.zero);
                    }
                    else if (_gameAction.GetComponent<GameActionRotation>())
                    {
                        GameActionRotation _gameActionRotation = _gameAction.GetComponent<GameActionRotation>();
                        ObjectToCompareGizmos(_gameActionRotation.target, TransformAction.rotation, _gameActionRotation.rotationToGo, _gameActionRotation.target, _gameActionRotation, Vector3.zero);
                    }
                }
            }

            for (int i = 0; i < listValues.Count; i++)
            {
                for (int j = 0; j < listValues[i].position.Count; j++)
                {
                    if (listValues[i].obj.GetComponent<MeshFilter>())
                        _mesh = listValues[i].obj.GetComponent<MeshFilter>().sharedMesh;
                    foreach (GameObject obj in Selection.gameObjects)
                    {
                        if (obj.transform.parent == transform)
                        {
                            if (obj == listValues[i].gameAction[j].gameObject)
                            {
                                ShowObject(i, j, 0.7f);
                            }
                            else
                            {
                                ShowObject(i, j, 0.2f);
                            }
                        }
                        else if (obj.transform == transform)
                        {
                            ShowObject(i, j, 0.2f);
                        }
                        int gaMoveNumbers = -1;
                        foreach (AGameAction child in GetComponentsInChildren<AGameAction>())
                        {
                            if (child.GetComponent<GameActionMove>())
                            {
                                gaMoveNumbers++;
                                GameActionMove gaMove = child.GetComponent<GameActionMove>();
                                if (obj.gameObject == gaMove.target.gameObject)
                                {
                                    if (listValues[i].obj.gameObject == gaMove.target.gameObject)
                                    {
                                        ShowObject(i, j, 0.5f);
                                    }
                                }
                                else if (gaMove.vectorOrTarget == GameActionMove.MovementMode.CopyObjectPosition && obj.gameObject == gaMove.pointB.gameObject)
                                {
                                    if (listValues[i].newPosition[j] != Vector3.zero)
                                    {
                                        if (listValues[i].position[j] == gaMove.posToGo)
                                        {
                                            ShowObject(i, j, 0.5f);
                                        }
                                    }
                                }

                                Vector3 startPos = listValues[i].position[j];
                                Vector3 endPos = gaMove.posToGo;
                                Gizmos.DrawLine(startPos, endPos);

                                if (gaMoveNumbers == 0)
                                {
                                    Gizmos.DrawLine(gaMove.target.position, gaMove.posToGo);
                                }
                            }
                            else if (child.GetComponent<GameActionScale>())
                            {
                                GameActionScale gaScale = child.GetComponent<GameActionScale>();
                                if (obj.gameObject == gaScale.target.gameObject)
                                {
                                    if (listValues[i].obj.gameObject == gaScale.target.gameObject)
                                    {
                                        ShowObject(i, j, 0.5f);
                                    }
                                }
                            }
                            else if (child.GetComponent<GameActionRotation>())
                            {
                                GameActionRotation gaRotation = child.GetComponent<GameActionRotation>();
                                if (obj.gameObject == gaRotation.target.gameObject)
                                {
                                    if (listValues[i].obj.gameObject == gaRotation.target.gameObject)
                                    {
                                        ShowObject(i, j, 0.5f);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (transform.childCount == 0)
        {
            GameObject child = new GameObject();
            child.transform.parent = transform;
            child.name = "Add game action on this object";
        }
    }

    void ObjectToCompareGizmos(Transform transform, TransformAction transformAction, Vector3 vector3, Transform objTransform, AGameAction gameAction, Vector3 transformPointB)
    {
        if (listValues.Count > 0)
        {
            for (int i = 0; i < listValues.Count; i++)
            {
                if (listValues[i].obj.gameObject.GetInstanceID() == objTransform.gameObject.GetInstanceID())
                {
                    AddListGizmos(false, i, transformAction, vector3, objTransform, gameAction, transformPointB);
                    return;
                }
            }
            AddListGizmos(true, 0, transformAction, vector3, objTransform, gameAction, transformPointB);
        }
        else
        {
            AddListGizmos(true, 0, transformAction, vector3, objTransform, gameAction, transformPointB);
        }
    }

    void AddListGizmos(bool addNewObj, int objIndex, TransformAction transformAction, Vector3 vector3, Transform objTransform, AGameAction gameAction, Vector3 transformPointB)
    {
        GameActionSequencerValues value = new GameActionSequencerValues();
        if (addNewObj)
        {
            value.name = objTransform.name;
            value.obj = objTransform;
            value.gameAction.Add(gameAction);
            value.position.Add(objTransform.position);
            value.scale.Add(objTransform.localScale);
            value.rotation.Add(objTransform.rotation.eulerAngles);
            value.newPosition.Add(Vector3.zero);
            switch (transformAction)
            {
                case TransformAction.move:
                    value.position[0] = vector3;
                    value.newPosition[0] = transformPointB;
                    break;
                case TransformAction.scale:
                    value.scale[0] = vector3;
                    break;
                case TransformAction.rotation:
                    value.rotation[0] = vector3;
                    break;
                default:
                    break;
            }

            listValues.Add(value);
        }
        else
        {
            value = listValues[objIndex];
            value.gameAction.Add(gameAction);
            value.position.Add(value.position[value.position.Count - 1]);
            value.scale.Add(value.scale[value.scale.Count - 1]);
            value.rotation.Add(value.rotation[value.rotation.Count - 1]);
            value.newPosition.Add(Vector3.zero);

            switch (transformAction)
            {
                case TransformAction.move:
                    value.position[value.position.Count - 1] = vector3;
                    value.newPosition[value.position.Count - 1] = transformPointB;
                    break;
                case TransformAction.scale:
                    value.scale[value.scale.Count - 1] = vector3;
                    break;
                case TransformAction.rotation:
                    value.rotation[value.rotation.Count - 1] = vector3;
                    break;
                default:
                    break;
            }
        }
    }

    static void DrawString(string text, Vector3 worldPos, Color? colour = null)
    {
        UnityEditor.Handles.BeginGUI();
        if (colour.HasValue) GUI.color = colour.Value;
        var view = UnityEditor.SceneView.currentDrawingSceneView;
        Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);
        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
        GUI.skin.label.fontSize = 30;
        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
        UnityEditor.Handles.EndGUI();
    }

    void ShowObject(int i, int j, float alpha)
    {
        ChangeColor(i, j);
        Color newColor = new Vector4(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, alpha);
        Gizmos.color = newColor;
        Gizmos.DrawMesh(_mesh, listValues[i].position[j], Quaternion.Euler(listValues[i].rotation[j]), listValues[i].scale[j]);
        Vector3 textPosition = new Vector3(listValues[i].position[j].x, listValues[i].position[j].y + (j * 0.5f), listValues[i].position[j].x);
        Color gizmoColor = new Vector4(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 1);
        string action = "";
        if (listValues[i].gameAction[j].gameObject.GetComponent<GameActionMove>())
        {
            action = "Move";
        }
        else if (listValues[i].gameAction[j].gameObject.GetComponent<GameActionScale>())
        {
            action = "Scale";
        }
        else if (listValues[i].gameAction[j].gameObject.GetComponent<GameActionRotation>())
        {
            action = "Rotate";
        }

        DrawString((j + 1).ToString() + " : " + action, textPosition, gizmoColor);
    }
    void ChangeColor(int i, int j)
    {
        if (listValues[i].gameAction[j].gameObject.GetComponent<GameActionMove>())
        {
            Gizmos.color = Color.blue;
        }
        else if (listValues[i].gameAction[j].gameObject.GetComponent<GameActionScale>())
        {
            Gizmos.color = Color.red;
        }
        else if (listValues[i].gameAction[j].gameObject.GetComponent<GameActionRotation>())
        {
            Gizmos.color = Color.green;
        }
    }

    #endregion


    void Start()
    {
        if (playOnStart)
        {
            Reload();
            Play();
            _isStart = false;
        }
    }
    void OnEnable()
    {
        if (playOnStart && !_isStart)
        {
            Reload();
            Play();
            if (_actions.Length < GetComponentsInChildren<AGameAction>().Length)
            {
                _actions = GetComponentsInChildren<AGameAction>();
            }
        }
    }

    void Awake()
    {
        _actions = GetComponentsInChildren<AGameAction>();
    }

    void Update()
    {
        Execution();
    }

    void Execution()
    {
        if (!_isRunning) return;

        if (_currentActionIndex >= _actions.Length)
        {
            Stop();
            return;
        }

        AGameAction action = null;
        bool actionFinished = false;
        do
        {

            action = _actions[_currentActionIndex];
            //Start action (if not running)
            if (!action.IsRunning())
            {
                action.Execute();
            }
            else
            {
                //Update action
                action.ActionUpdate();
            }
            //Check if action is finished
            actionFinished = action.IsFinished();
            if (actionFinished)
            {
                _currentActionIndex++;
            }
        } while ((action != null) && actionFinished && (_currentActionIndex < _actions.Length));


    }

    public void Play()
    {
        _isRunning = true;
    }

    public void Reload()
    {
        _currentActionIndex = 0;

        foreach (AGameAction action in _actions)
        {
            action.Reload();
        }
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }
}

[System.Serializable]
public class GameActionSequencerValues
{
    [HideInInspector] public string name;
    public Transform obj;
    public List<Vector3> position = new List<Vector3>();
    public List<Vector3> newPosition = new List<Vector3>();
    public List<Vector3> scale = new List<Vector3>();
    public List<Vector3> rotation = new List<Vector3>();
    public List<AGameAction> gameAction = new List<AGameAction>();
}

#if UNITY_EDITOR

[CustomEditor(typeof(GameActionsSequencer))]
public class GameActionSequencerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameActionsSequencer gaSequencer = (GameActionsSequencer)target;
        base.OnInspectorGUI();

        EditorGUILayout.Space(20);

        if (gaSequencer.debug != GameActionsSequencer.DebugWay.none && !EditorWindow.GetWindow<SceneView>().drawGizmos)
        {
            EditorGUILayout.HelpBox("If you want to see Gizmos, be sure to enable it over scene view", MessageType.Info);

        }
        if (GUILayout.Button("Enable gizmos"))
        {
            if (EditorWindow.GetWindow<SceneView>().drawGizmos)
                Debug.Log("Gizmos already enabled");

            EditorWindow.GetWindow<SceneView>().drawGizmos = true;
            if (gaSequencer.debug == GameActionsSequencer.DebugWay.none)
                gaSequencer.debug = GameActionsSequencer.DebugWay.onlyInEditor;
        }

    }
}
#endif