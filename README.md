# Stage GTech - Unity Platformer Package

Un package Unity conçu pour les bootcamps de formation destinés aux adolescents qui souhaitent créer des jeux de plateforme 3D.

## 🎯 Objectif

Ce package fournit tous les outils nécessaires pour créer un petit jeu de plateforme 3D, permettant aux étudiants de se concentrer sur le Level Design plutôt que sur la programmation.

## ✨ Fonctionnalités

### Contrôle du Joueur
- Mouvement fluide avec acceleration progressive
- Système de saut avec coyote time et jump buffering
- Caméra en orbite contrôlée par la souris
- Détection du sol robuste
- Presets configurables (Lune, Mario, Défaut)

### Système de Progression
- **Checkpoints** : Points de sauvegarde pour respawn
- **Portails** : Transition entre les niveaux
- **GameManager** : Singleton pour gérer le jeu

### Pièges et Dangers
- **Trap** : Déclenche le respawn du joueur
- **RespawnTrigger** : Zones de mort (puits, lave, etc.)
- **OscillatingMover** : Animation de mouvement pour pièges mobiles

### Outils d'Édition
- **Fenêtre de Validation** : Checklist pour vérifier la configuration
- **Éditeur de Player** : Boutons pour appliquer des presets rapidement
- **Gizmos** : Visualisation automatique des zones et des mouvements

### Tutoriels (en Français)
- Créer un texte 3D avec TextMeshPro
- Peindre des textures sur un terrain
- Ajouter et configurer les objets du package
- Configurer le joueur

## 📦 Installation

### Via Git URL (Unity Package Manager)
1. Ouvrir Unity
2. Aller dans **Window > Package Manager**
3. Cliquer sur **+ > Add package from git URL**
4. Entrer : `https://github.com/AdrienBourgois/Stage.git`

### Téléchargement Manuel
1. Télécharger le repository
2. Dans Unity, **Window > Package Manager > + > Add package from disk**
3. Sélectionner le fichier `package.json`

## 🚀 Démarrage Rapide

### 1. Configuration de Base

1. **Créer un GameManager**
   - Créer un GameObject vide dans la scène
   - Ajouter le composant `GameManager`
   - Assigner un point d'apparition par défaut (ou utiliser un Checkpoint)

2. **Ajouter un Joueur**
   - Créer un GameObject avec un Capsule
   - Assigner le tag `Player`
   - Ajouter le composant `Character Controller`
   - Ajouter le composant `Player`
   - La caméra principale sera assignée automatiquement

3. **Créer un Checkpoint**
   - Créer un GameObject vide
   - Ajouter le composant `Checkpoint`
   - Cocher `Is Starting Checkpoint` pour le premier checkpoint

4. **Vérifier la Configuration**
   - Ouvrir **Stage GTech > Validation**
   - Vérifier que tous les éléments sont correctement configurés

### 2. Ajouter des Éléments de Gameplay

#### Piège Simple
```
1. Créer un Cube ou autre forme 3D
2. Ajouter le composant "Trap"
3. Optionnel : Assigner des effets audio/visuels
```

#### Piège Mobile
```
1. Créer un objet 3D
2. Ajouter "Trap" ET "Oscillating Mover"
3. Configurer l'axe et la distance de mouvement
4. Les Gizmos montrent le trajet
```

#### Zone de Mort (Puits)
```
1. Créer un grand Cube qui couvre la zone
2. Ajouter "Respawn Trigger"
3. Laisser "Player Only" coché
```

#### Portail de Niveau
```
1. Créer un objet 3D visuel
2. Ajouter "Level Portal"
3. Entrer le nom exact de la scène à charger
```

## 🎮 Presets de Joueur

Trois presets sont disponibles dans l'Inspector du Player :

### Preset Par Défaut
Configuration équilibrée pour un gameplay standard
- Vitesse : 7 m/s
- Gravité : -20
- Hauteur de saut : 1.6m

### Preset Lune
Gravité faible, sauts hauts et flottants
- Vitesse : 6 m/s
- Gravité : -8
- Hauteur de saut : 3m

### Preset Mario
Mouvement rapide, précis et réactif
- Vitesse : 9 m/s
- Gravité : -25
- Hauteur de saut : 2m

## 🛠️ Composants Détaillés

### Player
Le contrôleur principal du joueur avec mouvement, saut et caméra intégrée.

**Paramètres principaux :**
- Move Speed : Vitesse maximale
- Jump Height : Hauteur du saut
- Gravity : Force de gravité
- Camera Distance/Height : Position de la caméra

### GameManager
Singleton qui gère l'état du jeu et le respawn.

**Paramètres :**
- Default Spawn Point : Point d'apparition initial
- Player : Référence au joueur (auto-détectée)
- Persist Across Scenes : Garder le manager entre les scènes

### Checkpoint
Point de sauvegarde pour le respawn du joueur.

**Paramètres :**
- Is Starting Checkpoint : Premier checkpoint de la scène
- Activate Effect/Audio : Effets lors de l'activation

### OscillatingMover
Fait bouger un objet d'avant en arrière sur un axe.

**Paramètres :**
- Axis : Axe de mouvement (X, Y ou Z)
- Distance : Distance totale du mouvement
- Period : Temps pour un cycle complet
- Time Offset : Décalage temporel pour désynchroniser

## 📝 Notes pour les Formateurs

### Simplicité d'abord
- Tous les tooltips sont en français (sans accents)
- Les Gizmos s'affichent automatiquement
- Pas d'options de debug complexes
- Preset pour configuration rapide

### Validation Automatique
La fenêtre de validation vérifie :
- Présence du GameManager et du Player
- Configuration des composants
- Tag "Player" correct
- Références assignées
- Checkpoints présents

### Debug Visuel
Tous les composants affichent des Gizmos :
- **Checkpoint** : Cube jaune/vert
- **Player** : Ligne cyan pour la caméra
- **OscillatingMover** : Ligne magenta montrant le trajet
- **GameManager** : Sphère cyan au checkpoint actuel

## 🐛 Dépannage

### Le joueur ne saute pas
- Vérifier que le Character Controller est configuré correctement
- Ajuster la hauteur et le rayon du Character Controller
- Vérifier que le sol a un collider

### Le respawn ne fonctionne pas
- Vérifier que le GameManager existe
- Vérifier qu'un checkpoint est configuré
- Vérifier que le joueur a le tag "Player"

### La caméra ne bouge pas
- Vérifier que Camera Transform est assigné
- Vérifier que le curseur est verrouillé (automatique en jeu)

### Les portails ne marchent pas
- Vérifier que le nom de la scène est exact
- Vérifier que la scène est ajoutée dans Build Settings

## 📄 Licence

Ce package est créé pour le bootcamp GTech.

## 🤝 Contribution

Pour améliorer ce package, créer une issue ou une pull request sur GitHub.
