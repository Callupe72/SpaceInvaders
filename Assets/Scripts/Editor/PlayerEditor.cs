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

            player.playerCanUsePowerShot = EditorGUILayout.Toggle("Use Power Shot", player.playerCanUsePowerShot);
            player.playerCanAim = EditorGUILayout.Toggle("Aim", player.playerCanAim);
            player.playerCanDestroyLine = EditorGUILayout.Toggle("Destroy Line", player.playerCanDestroyLine);
            player.playerCanShield = EditorGUILayout.Toggle("Use Shield", player.playerCanShield);
            player.playerCanDash = EditorGUILayout.Toggle("Dash", player.playerCanDash);
            player.playerCanMakeDamagesOnDash = EditorGUILayout.Toggle("Make Damages on Dash", player.playerCanMakeDamagesOnDash);
            player.debrisMakesDamages = EditorGUILayout.Toggle("Debris Make Damages", player.debrisMakesDamages);
        }

        EditorGUILayout.Space(20);

        DrawDefaultInspector();

    }
}
