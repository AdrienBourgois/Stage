using UnityEngine;

/// <summary>
/// Tutorial: Comment peindre des textures sur un terrain
/// </summary>
[CreateAssetMenu(fileName = "TutorialTerrainPainting", menuName = "Stage GTech/Tutorials/Tutorial Terrain")]
public class TutorialTerrainPainting : ScriptableObject
{
    [Header("Tutoriel: Peindre des textures sur un terrain")]
    [TextArea(3, 10)]
    public string description = @"
Ce tutoriel vous guide pour peindre des textures sur un terrain.

Etapes:
1. Selectionnez votre Terrain dans la Hierarchie
2. Dans l'Inspector, trouvez le composant Terrain
3. Cliquez sur l'icone du pinceau (Paint Texture)
4. Si aucune texture n'existe, cliquez sur 'Create Layer'
5. Assignez une texture a votre layer
6. Ajoutez d'autres layers si necessaire
7. Selectionnez un layer dans la liste
8. Ajustez la taille du pinceau (Brush Size)
9. Ajustez l'opacite (Opacity)
10. Cliquez et glissez dans la Scene view pour peindre
11. Maintenez Shift pour effacer
";
}
