# Tutoriel Emulate3D – Intégration UR3e (Catalogue → Axes → Scripts C# → CSV)

## Objectif

Mettre en place un jumeau numérique d’un robot UR3e dans Emulate3D avec :
- Import du robot depuis le catalogue
- Ajout des moteurs et des PositionControllers
- Pilotage des axes via scripts C#
- Lecture des angles depuis un fichier CSV

---

## 1. Import du robot depuis le catalogue

### Étapes

1. Ouvrir Emulate3D  
2. Aller dans :  
   `Tools → Catalog`  
3. Rechercher :  
   `UR3e`  
4. Glisser-déposer le robot dans la scène  

### Résultat attendu

Le robot apparaît dans le Model Browser avec ses différents links.

---

## 2. Vérification de la hiérarchie

Vérifier une structure de ce type :
![Hiérarchie du robot](./image/hierarchie.png)


Chaque link correspond à un axe du robot.

---

## 3. Ajout des moteurs (Motor)

### Principe

Chaque axe est piloté par un moteur en rotation.

### Étapes (à répéter pour chaque Link)

1. Sélectionner un Link  
2. Ajouter :  
   `CAD Is The Model → Motors → Motor`  
3. Configurer :
   - Type : Rotational  
   - Axe de rotation : selon le joint  
   - Parent / Child correctement définis  

### Résultat attendu

![moteurs des axes](./image/Motor.png)

Chaque axe devient mobile.

---

## 4. Ajout des PositionControllers

### Principe

Le PositionController permet de piloter l’angle d’un axe.

### Étapes

Pour chaque Link :

`CAD Is The Model → Motor Controller → Position Controller`

### Résultat attendu

Chaque axe peut être piloté via la propriété :

![Position controller des axes](./image/Position_Controller.png)

`TargetPosition`

---

## 5. Architecture d’un axe

Pour chaque axe :
![Vue globale](./image/Vue_glob_Emulate.png)

---

## 6. Script C# par axe (Exemple pour l'axe1 ->Link1Controller pour l'axe2->Link2Controller)
Dans un premier temps ajouter un script (Restait sur Script et non Aspect)en faisant clique droit sur l'axe que vous voulez faire dans la hierarchie et cliquer Edit Script.

- [Link1Controller](./code/Link1Controller.cs)

### Principe

- Un script par axe

## 7. Lien avec le script Python et gestion de l’offset

À partir de cette étape, le fonctionnement du robot dans Emulate3D dépend directement du script Python.
- [ur3e_rtde_reader_Csv](./code/ur3e_rtde_reader_Csv.py)
Le script Python :
- récupère les angles du robot réel via RTDE  
- applique un offset si nécessaire  
- écrit les valeurs dans un fichier CSV  

Les scripts C# dans Emulate3D lisent ensuite ce fichier pour piloter chaque axe.

### Accès au script Python

- [Script Python (RTDE → CSV)](./code/python_rtde.py)

---

## 8. Principe de l’offset

### Définition

Un offset est une correction appliquée aux angles du robot afin de :
- aligner le robot virtuel avec le robot réel  
- corriger les différences de zéro mécanique  
- adapter les conventions d’angles entre systèmes  

### Exemple

```python
angle_corrige = angle_reel + offset
- Lecture du fichier CSV  
- Mise à jour de TargetPosition  

---
