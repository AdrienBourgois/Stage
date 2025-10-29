using UnityEngine;

/// <summary>
/// Tutorial: Comment configurer le joueur
/// </summary>
[CreateAssetMenu(fileName = "TutorialPlayerConfig", menuName = "Stage GTech/Tutorials/Tutorial Configuration Joueur")]
public class TutorialPlayerConfig : ScriptableObject
{
    [Header("Tutoriel: Configuration du Joueur")]
    [TextArea(3, 20)]
    public string description = @"
Ce tutoriel vous guide pour configurer le joueur.

=== CONFIGURATION DE BASE ===
1. Selectionnez votre Player dans la Hierarchie
2. Verifiez qu'il a le tag 'Player' (en haut de l'Inspector)
3. Verifiez la presence du composant 'Character Controller'
4. Verifiez la presence du composant 'Player'

=== PARAMETRES DE MOUVEMENT ===
- Move Speed: Vitesse maximale (defaut: 7)
- Ground Acceleration: Vitesse d'acceleration au sol (defaut: 20)
- Air Acceleration: Vitesse d'acceleration en l'air (defaut: 10)
- Turn Speed: Vitesse de rotation (defaut: 12)

=== PARAMETRES DE SAUT ===
- Jump Height: Hauteur du saut en metres (defaut: 1.6)
- Coyote Time: Temps supplementaire pour sauter apres avoir quitte le sol
- Jump Buffer Time: Permet de sauter juste avant d'atterrir

=== PARAMETRES DE GRAVITE ===
- Gravity: Force de gravite (defaut: -20)
- Terminal Velocity: Vitesse de chute maximale (defaut: 50)

=== VERIFICATION DU SOL ===
- Ground Check Offset: Position du point de verification
- Ground Check Radius: Taille de la sphere de verification
- Ground Mask: Calques consideres comme sol

=== CAMERA ===
- Camera Transform: Reference a la camera (auto si Camera.main existe)
- Camera Distance: Distance camera-joueur (defaut: 5)
- Camera Height: Hauteur de la camera (defaut: 1.6)
- Mouse Sensitivity X/Y: Sensibilite de la souris
- Min/Max Vertical Angle: Limites de rotation verticale

Conseil: Utilisez les presets pour des configurations rapides !
";
}
