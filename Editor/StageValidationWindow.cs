using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Fenetre d'editeur qui affiche une checklist de validation pour aider les
/// etudiants a verifier que leur scene est correctement configuree.
/// </summary>
public class StageValidationWindow : EditorWindow
{
    private Vector2 scrollPosition;
    
    [MenuItem("Window/Stage GTech/Validation")]
    public static void ShowWindow()
    {
        var window = GetWindow<StageValidationWindow>("Validation Scene");
        window.minSize = new Vector2(400, 300);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Checklist de Validation", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox("Cette fenetre vous aide a verifier que votre scene est correctement configuree.", MessageType.Info);
        EditorGUILayout.Space();
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // Check GameManager
        GameManager gameManager = FindFirstObjectByType<GameManager>();
        DrawValidationItem("GameManager existe dans la scene", gameManager != null);
        
        if (gameManager != null)
        {
            SerializedObject so = new SerializedObject(gameManager);
            SerializedProperty playerProp = so.FindProperty("player");
            SerializedProperty spawnProp = so.FindProperty("defaultSpawnPoint");
            
            DrawValidationItem("GameManager: Le joueur est assigne", playerProp.objectReferenceValue != null);
            DrawValidationItem("GameManager: Point d'apparition par defaut assigne", spawnProp.objectReferenceValue != null);
        }
        
        // Check Player
        Player player = FindFirstObjectByType<Player>();
        DrawValidationItem("Player existe dans la scene", player != null);
        
        if (player != null)
        {
            // Check player tag
            DrawValidationItem("Player: Tag 'Player' est assigne", player.CompareTag("Player"));
            
            // Check CharacterController
            CharacterController controller = player.GetComponent<CharacterController>();
            DrawValidationItem("Player: CharacterController est present", controller != null);
            
            if (controller != null && controller.enabled)
            {
                DrawValidationItem("Player: CharacterController est active", true);
            }
            else if (controller != null)
            {
                DrawValidationItem("Player: CharacterController est active", false);
            }
            
            // Check camera reference
            SerializedObject playerSO = new SerializedObject(player);
            SerializedProperty cameraProp = playerSO.FindProperty("cameraTransform");
            DrawValidationItem("Player: Camera est assignee", cameraProp.objectReferenceValue != null);
        }
        
        // Check Camera
        Camera mainCamera = Camera.main;
        DrawValidationItem("Camera principale existe dans la scene", mainCamera != null);
        
        // Check Checkpoints
        Checkpoint[] checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        DrawValidationItem($"Au moins un Checkpoint existe ({checkpoints.Length} trouve(s))", checkpoints.Length > 0);
        
        if (checkpoints.Length > 0)
        {
            bool hasStartingCheckpoint = false;
            foreach (var cp in checkpoints)
            {
                if (cp.isStartingCheckpoint)
                {
                    hasStartingCheckpoint = true;
                    break;
                }
            }
            DrawValidationItem("Au moins un Checkpoint est marque comme point de depart", hasStartingCheckpoint);
        }
        
        // Check Portals
        LevelPortal[] portals = FindObjectsByType<LevelPortal>(FindObjectsSortMode.None);
        if (portals.Length > 0)
        {
            DrawValidationItem($"Portals trouves: {portals.Length}", true);
            
            foreach (var portal in portals)
            {
                if (portal != null)
                {
                    SerializedObject portalSO = new SerializedObject(portal);
                    SerializedProperty sceneProp = portalSO.FindProperty("sceneToLoad");
                    bool hasScene = !string.IsNullOrEmpty(sceneProp.stringValue);
                    DrawValidationItem($"Portal '{portal.name}': Scene assignee", hasScene);
                }
            }
        }
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("Actualiser"))
        {
            Repaint();
        }
    }
    
    private void DrawValidationItem(string label, bool isValid)
    {
        EditorGUILayout.BeginHorizontal();
        
        if (isValid)
        {
            EditorGUILayout.LabelField("✓", GUILayout.Width(20));
            EditorGUILayout.LabelField(label);
        }
        else
        {
            EditorGUILayout.LabelField("✗", GUILayout.Width(20));
            GUIStyle errorStyle = new GUIStyle(EditorStyles.label);
            errorStyle.normal.textColor = Color.red;
            EditorGUILayout.LabelField(label, errorStyle);
        }
        
        EditorGUILayout.EndHorizontal();
    }
    
    private void OnInspectorUpdate()
    {
        // Repaint automatically when something changes
        Repaint();
    }
}
