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
    
    [MenuItem("Stage GTech/Validation")]
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
        GameManager[] gameManagers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        GameManager gameManager = gameManagers.Length > 0 ? gameManagers[0] : null;
        DrawValidationItem("GameManager existe dans la scene", gameManager != null);
        DrawValidationItem("Un seul GameManager est present", gameManagers.Length == 1);
        
        if (gameManager != null)
        {
            SerializedObject so = new SerializedObject(gameManager);
            SerializedProperty spawnProp = so.FindProperty("defaultSpawnPoint");
            
            DrawValidationItem("GameManager: Point d'apparition par defaut assigne", spawnProp != null && spawnProp.objectReferenceValue != null);
        }
        
        // Check Player
        PlayerBase[] players = FindObjectsByType<PlayerBase>(FindObjectsSortMode.None);
        PlayerBase player = players.Length > 0 ? players[0] : null;
        DrawValidationItem("Player (3D ou TopDown) existe dans la scene", player != null);
        DrawValidationItem("Un seul joueur (PlayerBase) est present", players.Length == 1);

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

            if (player is TopDownPlayer)
            {
                Camera childCamera = player.GetComponentInChildren<Camera>();
                DrawValidationItem("TopDownPlayer: Une camera enfant est detectee", childCamera != null);
            }
        }
        
        // Check Camera
        Camera mainCamera = Camera.main;
        DrawValidationItem("Camera principale existe dans la scene", mainCamera != null);
        int mainCameraCount = GameObject.FindGameObjectsWithTag("MainCamera").Length;
        DrawValidationItem("Une seule camera a le tag MainCamera", mainCameraCount == 1);
        
        // Check Checkpoints
        Checkpoint[] checkpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
        DrawValidationItem($"Au moins un Checkpoint existe ({checkpoints.Length} trouve(s))", checkpoints.Length > 0);
        
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

        string prefix = isValid ? "[OK]" : "[!]";
        GUIStyle style = isValid ? EditorStyles.label : GetErrorStyle();

        EditorGUILayout.LabelField(prefix, GUILayout.Width(30));
        EditorGUILayout.LabelField(label, style);

        EditorGUILayout.EndHorizontal();
    }

    private GUIStyle GetErrorStyle()
    {
        GUIStyle style = new GUIStyle(EditorStyles.label);
        style.normal.textColor = Color.red;
        return style;
    }

    private void OnInspectorUpdate()
    {
        // Repaint automatically when something changes
        Repaint();
    }
}
