using UnityEngine;

/// <summary>
/// Tutorial: Comment creer un texte 3D avec TextMeshPro
/// </summary>
[CreateAssetMenu(fileName = "TutorialTextMeshPro", menuName = "Stage GTech/Tutorials/Tutorial TextMeshPro")]
public class TutorialTextMeshPro : ScriptableObject
{
    [Header("Tutoriel: Creer un texte 3D avec TextMeshPro")]
    [TextArea(3, 10)]
    public string description = @"
Ce tutoriel vous guide pour creer un texte 3D avec TextMeshPro.

Etapes:
1. Faites un clic droit dans la Hierarchie
2. Selectionnez 3D Object > Text - TextMeshPro
3. Si demande, importez les ressources TMP Essentials
4. Selectionnez le texte dans la scene
5. Dans l'Inspector, modifiez le texte dans le champ 'Text'
6. Ajustez la taille avec Font Size
7. Changez la couleur avec Vertex Color
8. Utilisez la poignee pour positionner le texte dans la scene
";
}
