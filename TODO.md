# Scoring

- [x] Plus de points pour plus de poissons pêchés en un coup [x(log_3(x))+1]
- [x] [**Solution autre**] Plus de points pour les poissons plus loin sur l'écran
- [x] [**Solution autre**] Multiplicateur pour les combos
- [ ] Multiplicateur en fonction de la difficulté

# Menus et Interfaces

- [x] Changer l'icône de mort
- [x] Enregistrer le meilleur score
- [x] Ajouter une page de statistiques
- [x] Ajouter un compendium
- [x] Ajouter une emphase visuelle sur les vies lors de la perte
- [x] Faire clignoter le score pour la perte de points
- [x] Couper le son des poissons dans les menus
- [x] Faire des boutons en poisson
- [ ] Ajourter un tutoriel
- [ ] Utiliser des hameçons pour la vie 
- [ ] Séparer le mode compétitif du mode normal dans les statistiques
- [ ] Faire les panneaux d'affichage
- [ ] Ajouter une page pour visualiser les hauts faits

# Poissons

- [x] Poisson rouge
- [x] Banc de poisson (sardines)
- [x] Espadon embrocheur
- [x] Requin mange tout sur son passage
- [ ] Poisson volant
- [ ] Poisson doré
- [ ] Poisson venimeux
- [ ] Poisson bombe
- [ ] Poisson erratique
- [ ] Méduse de vie
- [ ] Nautile bonus
- [ ] Calamar encreur
- [ ] Étoile de mer d'invincibilité
- [ ] Raie manta répulsive
- [ ] Phoque qui attire l'orque
- [ ] Orque
- [ ] Seiche (change de motif aléatoirement)
- [ ] Poisson chonomêtre
- [ ] Araignée de mer
- [ ] Grenouille
- [ ] Saumon
- [ ] Thon

# Objets pêchables

- [ ] Crabe d'appât
- [ ] Oursin
- [ ] Anémone avec des bonus
- [ ] Crevettes d'appât

# Objets équipables

- [ ] Poids (augmente la vitesse de descente, réduit la vitesse de remonté)
- [ ] Flotteur (réduit la vitesse de descente, augmente la vitesse de remonté)
- [ ] Hameçon plus grand (augmente la taille de la zone de pêche)
- [ ] Leurre (attire les poissons)
- [ ] Aimant (attire les déchêts)

# Biomes

- [x] Plage (départ)
- [x] Epave (pont)
- [x] Epave (intérieur)
- [x] Abysses
- [x] Récif de corail
- [x] Grand bleu
- [x] Courant marin
- [ ] Rivière
- [ ] Lac
- [ ] Marre
- [ ] Mangrove

# Gameplay

- [x] Régler le problème de frustration
- [x] Ajouter des biomes et des niveaux de difficulté
- [x] Rendre la taille des poissons aléatoire
- [ ] Ajouter des objets équipables
- [ ] Ajouter des objets activables en jeu
	- [ ] Appât
	- [ ] Filet
	- [ ] Truc pour virer les déchêts

# Modes de jeu

- [x] Classique
- [x] Contre la montre
- [x] Go green
- [X] Poissons ciblés
- [ ] Zen
- [ ] Training mode

# Autre

- [x] Support de différentes tailles d'écran
- [x] Ajouter une vitesse propre à chaque poisson
- [x] Rendre le délai d'apparition aléatoire
- [x] Refondre la structure de dossier pour `art`
- [x] Utiliser une boucle avec une clause d'essai pour éviter les erreurs lors de la récupération de la sauvegarde
- [x] Retravailler la logique de la ligne de pêche
- [x] [**Actuellement impossible**]Trouver comment générer de la corde 
- [x] [**Actuellement impossible**]Faire des recherches sur l'intégration en page d'accueil
- [x] [**Solution autre**] Ajouter des poissons à chemins
- [x] Mettre à jour les statistiques avec les nouveaux modes de jeu
- [x] Mettre à jour les records avec les nouveaux modes de jeu
- [x] Donner la responsabilité au poisson de se placer sur l'écran
- [x] Ajouter la sauvegarde des paramètres
- [x] Ajouter une probabilité d'apparition
- [x] Mettre à jour le compendium avec les déchêts
- [x] Mettre à jour le compendium avec les biomes
- [x] Ajouter des hauts faits
- [ ] Support de différentes langues
	- [x] Anglais
	- [ ] Français
	- [ ] Allemand
	- [ ] Thai
- [ ] Ajouter des morts différentes
- [ ] Ajouter une vitesse de descente et de remontée différente pour la ligne
- [ ] SLB dans le fond
- [ ] Ajouter une animation pour signifier la casse de l'hameçon à la remontée
- [ ] Afficher un biome aléatoire dans ceux visités pour le fond du menu
- [ ] Mettre les chronomètres d'apparition des poissons et des déchêts sur les biomes
- [ ] Ajouter un moyen de trier/filtrer le compendium
- [ ] Ajouter un outil pour voire les graphes de selection des biomes ainsi que les probabilités d'apparition des poissons
- [ ] Ajouter pleins de hauts faits différents
	- [x] Jouer X parties
		- [ ] Jouer X parties dans chaque mode
	- [x] Pêcher X poissons
		- [ ] Pêcher X poissons dans chaque mode
	- [ ] Rencontrer <tel> biome
	- [ ] Ramasser X déchêts
	- [ ] Perdre X vies
		- [ ] Perdre X vies de chaque manière
	- [ ] Pêcher <tel> poisson
	- [ ] Voir <tel> poisson
	- [ ] Voir <tel> déchêt

# Corrections

- [x] Empêcher les poissons rouges de s'entretuer
- [x] Corriger le retournement des poissons (`CollisionShape2D`)
- [X] Corriger la vélocité non constante des poissons
- [x] Corriger la gravité en pêche de poisson mort
- [x] Corriger la disparition des poissions hors écran
- [x] Attention le poisson ne sait s'il est mort et pêché en même temps
- [x] Corriger le comportement de l'espadon quand il est pêché
- [x] Changer la diparition par chronomêtre pour une disparition par zone
- [x] Limiter la vitesse minimum des espadons à la charge
- [x] Corriger le reparentage lors de la touche d'un déchêt
- [x] Verifier le passage en négatif de la statistique de score
- [x] Couper le son d'un poisson mort
- [x] Corriger la présence de la cible au changement de biome
- [x] [**Solution autre**] Corriger le problème lorsqu'un espadon vole un poisson pêché
- [x] Corriger le problème des espadons fous
- [x] Corriger le problème du reparentage d'espadon
- [x] Corriger l'espadon qui chasse même mort
- [x] Empêcher les requins de se faire tuer par les poissons rouges
- [x] [**Actuellement Impossible**] Corriger les bulles des requins qui disparaissent
- [x] [**Actuellement impossible**] Corriger les requins en attente de lancement
- [ ] Corriger les requins qui mangent des poissons déjà pêchés
- [ ] Passer à des listes de noms plutôt que des énumérations
- [ ] Grouper les vérifications de hauts faits en un seul appel de fonction