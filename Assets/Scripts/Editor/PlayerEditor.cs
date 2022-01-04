using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    bool seePowerup;
    public override void OnInspectorGUI()
    {
        Player player = (Player)target;
        if (!seePowerup)
        {
            if (GUILayout.Button("SEE powerups"))
            {
                seePowerup = true;
            }

        }
        else
        {
            if (GUILayout.Button("HIDE powerups"))
            {
                seePowerup = false;
            }

            player.playerCanUsePowerShot = EditorGUILayout.Toggle("Can Use Power Shot", player.playerCanUsePowerShot);
            player.playerCanAim = EditorGUILayout.Toggle("Can Aim", player.playerCanAim);
            player.playerCanDestroyLine = EditorGUILayout.Toggle("Can Destroy Line", player.playerCanDestroyLine);
            player.playerShield = EditorGUILayout.Toggle("Can Use Shield", player.playerShield);
        }



        EditorGUILayout.Space(20);

        DrawDefaultInspector();

    }
}
