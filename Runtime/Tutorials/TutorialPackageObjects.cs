using UnityEngine;

/// <summary>
/// Tutorial: Comment ajouter et configurer les objets du package Stage GTech
/// </summary>
[CreateAssetMenu(fileName = "TutorialPackageObjects", menuName = "Stage GTech/Tutorials/Tutorial Objets Package")]
public class TutorialPackageObjects : ScriptableObject
{
    [Header("Tutoriel: Objets du Package Stage GTech")]
    [TextArea(3, 20)]
    public string description = @"
Ce tutoriel vous guide pour utiliser les objets du package.

=== CHECKPOINT ===
1. Creez un GameObject vide (Clic droit > Create Empty)
2. Nommez-le 'Checkpoint'
3. Dans Add Component, recherchez et ajoutez 'Checkpoint'
4. Cochez 'Is Starting Checkpoint' pour le premier checkpoint
5. Optionnel: Assignez des effets visuels et audio

=== TRAP (Piege) ===
1. Creez un objet 3D (Cube, Sphere, etc.)
2. Dans Add Component, ajoutez 'Trap'
3. Ajustez le Respawn Delay si necessaire
4. Optionnel: Assignez des effets sonores et visuels
5. Pour un piege mobile, ajoutez 'Oscillating Mover'
   - Choisissez l'axe de mouvement (X, Y ou Z)
   - Definissez la distance du mouvement
   - Ajustez la periode (temps d'un cycle complet)

=== PORTAL (Portail vers autre niveau) ===
1. Creez un objet 3D pour visualiser le portail
2. Dans Add Component, ajoutez 'Level Portal'
3. Dans 'Scene To Load', tapez le nom exact de la scene
4. Ajustez le Load Delay si vous voulez un delai
5. Optionnel: Assignez des effets

=== RESPAWN TRIGGER (Zone de mort) ===
1. Creez un grand Cube pour couvrir la zone de mort
2. Dans Add Component, ajoutez 'Respawn Trigger'
3. Cochez 'Player Only' pour affecter uniquement le joueur
4. Le trigger respawn automatiquement le joueur au checkpoint

Conseil: Utilisez les Gizmos pour visualiser les zones !
";
}
