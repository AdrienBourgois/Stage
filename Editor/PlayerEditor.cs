using UnityEditor;
using UnityEngine;

/// <summary>
/// Editeur personnalise pour le composant Player permettant d'appliquer
/// facilement des presets de configuration
/// </summary>
[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    private PlayerPresetMoon presetMoon;
    private PlayerPresetMario presetMario;
    private PlayerPresetDefault presetDefault;
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Presets de Configuration", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Utilisez ces presets pour appliquer rapidement des configurations predefinies.", MessageType.Info);
        
        Player player = (Player)target;
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Preset Par Defaut"))
        {
            if (presetDefault == null)
            {
                presetDefault = new PlayerPresetDefault();
            }
            
            Undo.RecordObject(player, "Apply Default Preset");
            presetDefault.ApplyToPlayer(player);
            EditorUtility.SetDirty(player);
        }
        
        if (GUILayout.Button("Preset Lune"))
        {
            if (presetMoon == null)
            {
                presetMoon = new PlayerPresetMoon();
            }
            
            Undo.RecordObject(player, "Apply Moon Preset");
            presetMoon.ApplyToPlayer(player);
            EditorUtility.SetDirty(player);
        }
        
        if (GUILayout.Button("Preset Mario"))
        {
            if (presetMario == null)
            {
                presetMario = new PlayerPresetMario();
            }
            
            Undo.RecordObject(player, "Apply Mario Preset");
            presetMario.ApplyToPlayer(player);
            EditorUtility.SetDirty(player);
        }
        
        EditorGUILayout.EndHorizontal();
    }
}
