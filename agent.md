# Stage GTech - Unity Package Context

## Vue d'ensemble

Ce package Unity est conçu pour un bootcamp de formation destiné à des adolescents qui souhaitent apprendre à créer des jeux sur Unity. Les participants ne savent pas coder, donc l'objectif est de leur fournir tous les outils nécessaires pour créer un petit jeu de plateforme 3D en se concentrant principalement sur le Level Design.

## Objectifs du Package

- Fournir un système de contrôle de personnage prêt à l'emploi pour un jeu de plateforme 3D
- Offrir une caméra qui tourne autour du joueur avec contrôle à la souris
- Implémenter un système de checkpoints pour la progression
- Créer un système de pièges et de déclencheurs pour les zones de mort
- Fournir des scripts simples pour les animations de pièges (mouvement oscillant)
- Ajouter des outils d'édition pour aider les étudiants à configurer leur scène
- Inclure des tutoriels en français pour guider les participants

## Architecture du Package

### Runtime/Scripts/
Contient tous les scripts de jeu principaux :

- **Player.cs** : Contrôleur de personnage avec mouvement, saut, gravité et caméra intégrée
- **GameManager.cs** : Singleton qui gère le joueur, les checkpoints et le respawn
- **Checkpoint.cs** : Points de sauvegarde que le joueur peut atteindre
- **Trap.cs** : Pièges qui déclenchent le respawn du joueur
- **RespawnTrigger.cs** : Zones de mort (puits, lave, etc.)
- **LevelPortal.cs** : Portails pour charger une nouvelle scène
- **OscillatingMover.cs** : Fait bouger un objet d'avant en arrière sur un axe (pour pièges mobiles)

### Editor/
Contient les outils d'édition Unity :

- **StageValidationWindow.cs** : Fenêtre d'éditeur avec checklist de validation pour aider les étudiants à vérifier leur configuration
- **PlayerEditor.cs** : Éditeur personnalisé pour le Player avec boutons pour appliquer des presets

### Presets/
Configurations prédéfinies pour le joueur :

- **PlayerPresetData.cs** : Structure de données pour stocker les paramètres
- **PlayerPresetDefault.cs** : Configuration équilibrée par défaut
- **PlayerPresetMoon.cs** : Gravité lunaire (sauts hauts, chute lente)
- **PlayerPresetMario.cs** : Mouvement rapide et précis type Mario

### Tutorials/
Tutoriels scriptables en français :

- **TutorialTextMeshPro.cs** : Comment créer un texte 3D avec TextMeshPro
- **TutorialTerrainPainting.cs** : Comment peindre des textures sur un terrain
- **TutorialPackageObjects.cs** : Comment ajouter et configurer les objets du package
- **TutorialPlayerConfig.cs** : Comment configurer le joueur

## Principes de Conception

### Simplicité d'utilisation
- Tous les scripts ont des tooltips en français (sans accents ni caractères spéciaux)
- Le code reste en anglais pour la compatibilité
- Les Gizmos de debug sont toujours affichés quand un objet est sélectionné
- Pas de couleurs de debug configurables - couleurs constantes pour simplifier

### Debug et Visualisation
- Tous les scripts affichent des Gizmos pour visualiser leur portée et leur fonctionnement
- Les Gizmos s'affichent automatiquement quand l'objet est sélectionné (pas de checkbox)
- Couleurs fixes pour les Gizmos (pas de personnalisation)

### Système de Respawn
- Respawn instantané (pas de coroutine)
- Le système désactive/réactive simplement le CharacterController pour téléporter le joueur

## Dépendances

- Unity Tutorial Framework (com.unity.learn.iet-framework v3.1.3) : Pour créer des tutoriels interactifs guidés

## Utilisation pour les Étudiants

1. **Configuration Initiale** :
   - Ajouter un GameManager à la scène
   - Ajouter un Player avec le tag "Player"
   - Configurer au moins un Checkpoint de départ
   - Vérifier la configuration avec Window > Stage GTech > Validation

2. **Level Design** :
   - Créer le terrain et les plateformes
   - Placer des Checkpoints
   - Ajouter des Traps et RespawnTriggers
   - Configurer des LevelPortals pour la progression

3. **Personnalisation** :
   - Utiliser les presets pour modifier le comportement du joueur
   - Ajuster les paramètres visuels
   - Ajouter des effets sonores et visuels aux déclencheurs

## Notes Techniques

### Corrections Récentes
- Suppression du coroutine inutile dans Player.RespawnAt() pour un respawn instantané
- Remplacement de FindObjectOfType (obsolète) par FindFirstObjectByType
- Amélioration de la détection du sol avec Physics.CheckSphere en complément de isGrounded

### Évolutions Possibles
- Ajouter plus de presets (vitesse élevée, plateforme précise, etc.)
- Créer des tutoriels interactifs avec le Tutorial Framework
- Ajouter des effets visuels par défaut pour les checkpoints et portails
- Système de collectibles
- Power-ups temporaires

## Maintenance

Ce package est maintenu pour le bootcamp GTech. Les mises à jour doivent toujours privilégier la simplicité d'utilisation par des débutants non-programmeurs.
