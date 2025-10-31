# Stage GTech - Unity Package Context

## Vue d'ensemble

Ce package Unity est concu pour un bootcamp de formation destine a des adolescents qui souhaitent apprendre a creer des jeux sur Unity. Les participants ne savent pas coder, donc l'objectif est de leur fournir tous les outils necessaires pour assembler un petit jeu de plateforme 3D en se concentrant principalement sur le level design.

## Objectifs du Package

- Fournir un systeme de controle de personnage pret a l'emploi pour un jeu de plateforme 3D
- Offrir une camera qui tourne autour du joueur avec controle a la souris
- Implementer un systeme de checkpoints pour la progression
- Creer un systeme de pieges et de declencheurs pour les zones de mort
- Proposer des scripts simples pour les animations de pieges (mouvement oscillant et rotation)
- Ajouter des outils d'edition pour aider les etudiants a configurer leur scene
- Introduire un systeme simple de score et de vies pour suivre la progression des joueurs
- Inclure des tutoriels en francais pour guider les participants

## Architecture du Package

### Runtime/Scripts/
Contient tous les scripts de jeu principaux :

- **Player.cs** : Controleur de personnage avec mouvement, saut, gravite, camera integree, deceleration au relachement des entrees, gestion du score et des vies. Singleton accessible via `Player.Instance` qui gere automatiquement la camera en recuperant l'objet tagge `MainCamera`.
- **GameManager.cs** : Singleton (`GameManager.Instance`) qui gere le joueur, les checkpoints, la teleportation lors du respawn et la perte de vies sans assignation manuelle.
- **Checkpoint.cs** : Points de sauvegarde que le joueur peut atteindre pour definir un nouveau point de respawn.
- **Trap.cs** : Pieges qui declenchent le respawn du joueur (avec delai optionnel) tout en affichant le volume d'effet via des gizmos.
- **LevelPortal.cs** : Portails pour charger une nouvelle scene.
- **OscillatingMover.cs** : Fait bouger un objet d'avant en arriere sur un axe (pour pieges mobiles) avec visualisation complete de la trajectoire.
- **EventMover.cs** : Deplace un objet d'un point a un autre lorsque la methode `BeginMove` est appelee via un UnityEvent.
- **Collectible.cs** : Collectible qui accorde un nombre de points configurable et expose un UnityEvent declenche au ramassage.
- **Rotator.cs** : Utilitaire simple pour faire tourner un objet autour d'un axe choisi, ideal pour les collectibles type pieces.

### Editor/
Contient les outils d'edition Unity :

- **StageValidationWindow.cs** : Fenetre d'editeur avec checklist de validation pour aider les etudiants a verifier leur configuration (accessible via Stage GTech > Validation).
- **PlayerEditor.cs** : Editeur personnalise pour le Player avec boutons pour appliquer des presets.

### Runtime/Presets/
Configurations predefinies pour le joueur :

- **PlayerPresetData.cs** : Structure de donnees pour stocker les parametres.
- **PlayerPresetDefault.cs** : Configuration equilibree par defaut.
- **PlayerPresetMoon.cs** : Gravite lunaire (sauts hauts, chute lente).
- **PlayerPresetMario.cs** : Mouvement rapide et precis type Mario.

## Systemes de Score, Vies et Collectibles

- Le joueur dispose d'un nombre de vies definies via `startingLives`. Chaque mort decremente ce compteur avant de teleporter le joueur au dernier checkpoint actif.
- Le score commence a zero et est augmente via `Player.AddScore(int)` ; les collectibles s'en servent pour attribuer un nombre de points variable.
- Les collectibles possedent un UnityEvent `onCollected` pour chainer des effets supplementaires (son, VFX, activation de plateformes, etc.) puis se detruisent automatiquement.
- Combinez `Collectible` et `Rotator` pour creer des pieces animees et evidentes visuellement pour les etudiants.
- Le joueur et le GameManager se retrouvent automatiquement via leurs singletons respectifs, supprimant les assignations manuelles.

## EventMover et Animation d'Objets

- `EventMover` permet de definir un mouvement aller simple entre deux points sans coder : il suffit de relier la methode `BeginMove` a un UnityEvent (bouton, trigger, timeline, etc.).
- `OscillatingMover` reste disponible pour les mouvements repetitifs et maintenant affiche ses points de depart et d'arrivee dans la scene afin de faciliter le placement.
- Les gizmos des scripts `Trap`, `OscillatingMover`, `Collectible`, `Rotator` et `EventMover` fournissent des reperes visuels coherents pour aider les etudiants a comprendre l'espace d'interaction de chaque element.

## Principes de Conception

### Simplicite d'utilisation
- Tous les scripts ont des tooltips en francais (sans accents ni caracteres speciaux).
- Le code reste en anglais pour la compatibilite.
- Les gizmos de debug sont toujours affiches quand un objet est selectionne.
- Pas de couleurs de debug configurables : couleurs constantes pour simplifier.
- Le Player recupere automatiquement la camera taggee `MainCamera` et le GameManager se lie au joueur sans assignation dans l'inspecteur.

### Debug et visualisation
- Tous les scripts affichent des gizmos pour visualiser leur portee et leur fonctionnement (checkpoints, pieges, collectibles, trajectoires).
- Les gizmos s'affichent automatiquement quand l'objet est selectionne (pas de checkbox).
- Couleurs fixes pour les gizmos (pas de personnalisation).

### Systeme de respawn
- Respawn instantane (pas de coroutine).
- Le systeme desactive/reactive simplement le `CharacterController` pour teleporter le joueur.
- La mort du joueur retire une vie avant le respawn ; adaptez la logique si vous souhaitez finir la partie quand le compteur atteint zero.

## Utilisation pour les Etudiants

1. **Configuration initiale** :
   - Ajouter un `GameManager` a la scene.
   - Ajouter un `Player` avec le tag `Player`.
   - Configurer au moins un `Checkpoint` de depart.
   - Verifier la configuration avec Stage GTech > Validation.

2. **Level design** :
   - Creer le terrain et les plateformes.
   - Placer des `Checkpoint`.
   - Ajouter des `Trap` et `RespawnTrigger`.
   - Configurer des `LevelPortal` pour la progression.
   - Positionner des `Collectible` et animer leur presentation avec `Rotator` ou `OscillatingMover`.

3. **Personnalisation** :
   - Utiliser les presets pour modifier le comportement du joueur.
   - Ajuster les parametres visuels.
   - Ajouter des effets sonores et visuels aux declencheurs.
   - Lier `EventMover.BeginMove` a des UnityEvents pour mettre en scene des plateformes ou obstacles temporises.

## Notes Techniques

### Evolutions possibles
- Ajouter une interface utilisateur par defaut pour afficher le score et les vies.
- Ajouter plus de presets (vitesse elevee, plateforme precise, etc.).
- Ajouter des effets visuels par defaut pour les checkpoints et portails.
- Prevoir un systeme de collectibles specifiques (clefs, fragments) ou de power-ups temporaires.
