using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeEditor : EditorWindow
{
    public List<Rect> bloc = new List<Rect>();
    public int gameActionNumber;
    public string Text;
    public AGameAction gameAction;
    public bool boolTest;
    public List<bool> selectedObj = new List<bool>();
    bool isMovingBlock = false;
    public List<Rect> gameActionSequencerInScene = new List<Rect>();
    bool startSelected;

    private Vector2 offset;
    private Vector2 drag;
    private bool isPressingAKey;

    Rect gameActionSequencerRect;

    public enum GameAction { Move, Scale, Rotate }
    public List<GameAction> gameActionsEnum = new List<GameAction>();

    Vector2 size = new Vector2(200, 300);

    public GameActionMove gaMove;
    public Transform gaSequencer;

    [MenuItem("Window/Node editor")]
    static void ShowEditor()
    {
        NodeEditor editor = GetWindow<NodeEditor>();
        editor.Show();
    }

    public void Init(int index)
    {
        gameActionsEnum.Add(new GameAction());
        selectedObj.Add(false);
        bloc[index] = new Rect(size.x * 2 * (index) + 200, 0, size.x, size.y);
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }


    void OnGUI()
    {
        gaSequencer = EditorGUILayout.ObjectField("GameActionSequencer", gaSequencer, typeof(Transform), true) as Transform;
        if (GUILayout.Button("Save"))
        {
            if (gaSequencer != null && !gaSequencer.gameObject.GetComponent<GameActionsSequencer>())
            {
                gaSequencer.gameObject.AddComponent<GameActionsSequencer>();
            }
            if (bloc.Count > gaSequencer.childCount)
            {
                GameObject newChild = Instantiate(new GameObject(), gaSequencer.position, gaSequencer.rotation);
                newChild.transform.parent = gaSequencer.transform;
            }
        }

        GUIStyle selectedStyle = new GUIStyle();
        selectedStyle.border = new RectOffset(50, 50, 50, 50);
        selectedStyle.normal.background = MakeTex(10, 102, new Color(0.99f, 0.99f, 0.99f, 0.1f));

        Color gridColor = Color.gray;
        if (!isMovingBlock)
        {
            DrawGrid(20, 0.2f, gridColor);
            DrawGrid(100, 0.4f, gridColor);
        }
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add Game Action"))
        {
            AddGameAction();
        }
        else if (GUILayout.Button("Delete Game Action"))
        {
            DeleteGameAction();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        GameActionInScene();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();


        for (int i = 1; i < bloc.Count - 1; i++)
        {
            DrawNodeCurve(bloc[i], bloc[i + 1]);
        }
        DrawNodeCurve(gameActionSequencerRect, bloc[0]);
        BeginWindows();

        gameActionSequencerRect = GUI.Window(0, gameActionSequencerRect, DrawNodeWindow, "Game Action Start");


        for (int i = 0; i < bloc.Count; i++)
        {
            bloc[i] = GUI.Window(i + 1, bloc[i], DrawNodeWindow, "Game Action " + (i + 1).ToString() + " : " + gameActionsEnum[i].ToString());
        }
        EndWindows();
        Event ev = Event.current;
        DetectAction(ev);


        for (int i = 0; i < selectedObj.Count; i++)
        {
            if (selectedObj[i])
            {
                GUI.Box(bloc[i], "", selectedStyle);
                bloc[i] = new Rect(bloc[i].position, bloc[i].size);
            }
        }
    }

    void GameActionInScene()
    {
        GUI.backgroundColor = Color.white;
        for (int i = 0; i < FindObjectsOfType<GameActionsSequencer>().Length; i++)
        {
            GUILayout.Button(FindObjectsOfType<GameActionsSequencer>()[i].gameObject.name, GUILayout.Height(50));
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }
        Handles.color = Color.white;
        Handles.EndGUI();
    }

    void ChangeValue()
    {
        for (int i = 0; i < gameActionsEnum.Count; i++)
        {
            switch (gameActionsEnum[i])
            {
                case GameAction.Move:
                    break;
                case GameAction.Scale:
                    break;
                case GameAction.Rotate:
                    break;
                default:
                    break;
            }
        }
    }



    void AddGameAction()
    {
        bloc.Add(new Rect());
        Init(bloc.Count - 1);
    }
    void DeleteGameAction()
    {
        for (int i = bloc.Count - 1; i >= 0; i--)
        {
            if (selectedObj[i])
            {
                bloc.RemoveAt(i);
                selectedObj.RemoveAt(i);
            }
        }
    }

    void DrawNodeWindow(int id)
    {
        for (int i = 0; i < selectedObj.Count; i++)
        {
            if (id - 1 == i)
            {
                gameActionsEnum[i] = (GameAction)EditorGUILayout.EnumPopup(gameActionsEnum[i]);
            }
        }
        Repaint();
    }

    void DetectAction(Event e)
    {
        drag = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (!isPressingAKey)
                    {
                        for (int i = 0; i < selectedObj.Count; i++)
                        {
                            selectedObj[i] = false;
                        }
                        startSelected = false;
                    }
                    for (int i = 0; i < bloc.Count; i++)
                    {
                        if (bloc[i].Contains(e.mousePosition))
                        {
                            selectedObj[i] = true;
                        }
                        if (gameActionSequencerRect.Contains(e.mousePosition))
                        {
                            startSelected = true;
                        }
                    }
                }
                else if (e.button == 1)
                {
                    OpenContextMenu(e.mousePosition);
                }
                else if (e.button == 0)
                {
                    EditorGUILayout.BeginScrollView(e.mousePosition);
                }
                break;
            case EventType.MouseUp:
                if (e.button == 0)
                {
                    isMovingBlock = false;
                    isPressingAKey = false;
                    EditorGUILayout.EndScrollView();
                }
                break;
            case EventType.MouseDrag:
                if (e.button == 0 || e.button == 2)
                {
                    for (int i = 0; i < bloc.Count; i++)
                    {
                        if (bloc[i].Contains(e.mousePosition))
                        {
                            isMovingBlock = true;
                        }
                    }
                    if (gameActionSequencerRect.Contains(e.mousePosition))
                    {
                        OnDrag(e.delta);
                    }

                    OnDrag(e.delta);
                }
                break;
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.LeftControl || e.keyCode == KeyCode.LeftShift || e.keyCode == KeyCode.RightShift || e.keyCode == KeyCode.RightControl)
                {
                    isPressingAKey = true;
                }
                else if (e.keyCode == KeyCode.Delete)
                {
                    DeleteGameAction();
                }
                else if (e.keyCode == KeyCode.Space)
                {
                    AddGameAction();
                }
                break;
        }
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (!isMovingBlock)
        {
            if (bloc != null)
            {
                for (int i = 0; i < bloc.Count; i++)
                {
                    Vector2 blocPos = bloc[i].position;
                    Vector2 blocSize = bloc[i].size;
                    Rect rect = new Rect(blocPos, blocSize);
                    rect.position += delta;
                    bloc[i] = rect;
                }
                Vector2 gaSequencerPos = gameActionSequencerRect.position;
                Vector2 gaSequencerSize = gameActionSequencerRect.size;
                Rect rectSeq = new Rect(gaSequencerPos, gaSequencerSize);
                rectSeq.position += delta;
                gameActionSequencerRect = rectSeq;

            }
        }
        else
        {
            for (int i = 0; i < selectedObj.Count; i++)
            {
                if (selectedObj[i])
                {
                    Vector2 blocPos = bloc[i].position;
                    Vector2 blocSize = bloc[i].size;
                    Rect rect = new Rect(blocPos, blocSize);
                    rect.position += delta;
                    bloc[i] = rect;
                }
                if (startSelected)
                {
                    Vector2 gaSequencerPos = gameActionSequencerRect.position;
                    Vector2 gaSequencerSize = gameActionSequencerRect.size;
                    Rect rectSeq = new Rect(gaSequencerPos, gaSequencerSize);
                    rectSeq.position += delta;
                    gameActionSequencerRect = rectSeq;
                }
            }
        }

        GUI.changed = true;
    }

    void OpenContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Game Action"), false, () => AddGameAction());
        genericMenu.AddItem(new GUIContent("Delete Game Action"), false, () => DeleteGameAction());

        genericMenu.ShowAsContext();
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++) // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}
