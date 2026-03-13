# Projet-Emulate3D-UR3e
UR3e Digital Twin with Emulate3D

Ce projet permet de créer un jumeau numérique du robot UR3e en connectant un robot réel à une simulation Emulate3D.

Les positions articulaires du robot réel sont récupérées via RTDE avec Python, puis envoyées dans Emulate3D, qui reproduit les mouvements du robot virtuel.

Architecture du système
UR3e Robot
   ↓ RTDE
Python Script
   ↓
angles_robot.csv
   ↓
Emulate3D (C# scripts + PositionController)
   ↓
UR3e Virtual Robot
1. Installation des dépendances

Installer Python puis la librairie RTDE :

pip install ur-rtde

Configuration réseau :

Robot IP : 192.168.1.100
PC IP : même réseau
2. Connexion au robot et récupération des angles

Le script Python se connecte au robot UR3e via RTDE et récupère les positions des articulations.

Les valeurs sont ensuite écrites dans un fichier CSV qui sera lu par Emulate3D.

Script Python
import time
import csv
import math
from rtde_receive import RTDEReceiveInterface

ROBOT_IP = "192.168.1.100"

OUTPUT_FILE = r"C:\Users\Etudiant\Desktop\M2_IN\Projet Jumeau numérique\angles_robot.csv"

print("Connexion au robot UR3...")

rtde_r = RTDEReceiveInterface(ROBOT_IP)

print("Lecture RTDE démarrée")

offset = [0, 90, 0, 90, 0, 0]

while True:

    try:
        joints_rad = rtde_r.getActualQ()

        q1 = int(math.degrees(joints_rad[0])) + offset[0]
        q2 = int(math.degrees(joints_rad[1])) + offset[1]

        # axe 3 inversé
        q3 = -int(math.degrees(joints_rad[2])) + offset[2]

        q4 = int(math.degrees(joints_rad[3])) + offset[3]
        q5 = int(math.degrees(joints_rad[4])) + offset[4]
        q6 = int(math.degrees(joints_rad[5])) + offset[5]

        with open(OUTPUT_FILE, "w", newline="") as f:
            writer = csv.writer(f)
            writer.writerow(["time","q1","q2","q3","q4","q5","q6"])
            writer.writerow([int(time.time()), q1, q2, q3, q4, q5, q6])
            f.flush()

    except PermissionError:
        print("Emulate utilise le fichier...")

    time.sleep(0.1)

Ce script :

se connecte au robot

récupère les angles articulaires

convertit radians → degrés

applique les offsets

écrit les données dans un fichier CSV.

3. Import du robot dans Emulate3D

Le robot UR3e est importé dans Emulate3D via le Catalog.

Étapes

Ouvrir Catalog

Importer le robot UR3e

Placer le robot dans la scène

Le robot est composé de plusieurs pièces :

Base
Link1
Link2
Link3
Link4
Link5
Link6

Chaque pièce correspond à une articulation du robot.

4. Configuration cinématique du robot

Chaque articulation doit être configurée avec :

un axe de rotation

un moteur

un PositionController

Les moteurs permettent de définir :

vitesse

accélération

comportement dynamique

Structure cinématique du robot :

Base
 └ Link1
     └ Link2
         └ Link3
             └ Link4
                 └ Link5
                     └ Link6

Pour chaque axe :

Ajouter :

Motor

Puis ajouter l’aspect :

PositionController

Le moteur permet d’effectuer la rotation physique de l’axe tandis que PositionController définit la position cible.

Variable utilisée :

TargetPosition

Cette variable correspond à l’angle de rotation du joint.

5. Utilisation de la librairie CITM

Les scripts dans Emulate3D utilisent les librairies :

Demo3D.Native
Demo3D.Visuals
Demo3D.Components

Ces librairies permettent :

d’accéder aux objets de la simulation

d’utiliser les aspects

de contrôler les mouvements.

Exemple :

sender.FindAspect<PositionController>()

Cette commande permet de récupérer le PositionController attaché à la pièce.

6. Scripts C# pour contrôler les axes

Chaque pièce du robot possède un script C# associé.

Chaque script :

lit le fichier CSV

récupère l’angle correspondant

applique la valeur au moteur via PositionController.

7. Exemple de script pour un axe

Exemple pour Link1.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Demo3D.Native;
using Demo3D.Visuals;
using Demo3D.Components;

[Auto] public class Link1Controller : NativeObject {

    public Link1Controller(Visual sender) : base(sender) {}

    string filePath = @"C:\Users\Etudiant\Desktop\M2_IN\Projet Jumeau numérique\angles_robot.csv";

    [Auto] void OnReset(Visual sender) {

        sender.FindAspect<PositionController>().TargetPosition = 0;

    }

    [Auto] IEnumerable OnInitialize (Visual sender)
    {

        var PosAspect = sender.FindAspect<PositionController>();

        while (true)
        {

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader sr = new StreamReader(fs))
                {

                    var lines = new List<string>();

                    while (!sr.EndOfStream)
                        lines.Add(sr.ReadLine());

                    if (lines.Count > 1)
                    {

                        var data = lines[1].Split(',');

                        int angle = Convert.ToInt32(data[1]);

                        PosAspect.TargetPosition = angle;

                    }
                }
            }

            catch
            {

            }

            yield return Wait.ForSeconds(0.1);

        }

    }
}

Chaque axe possède un script similaire.

Correspondance :

Link1 → q1
Link2 → q2
Link3 → q3
Link4 → q4
Link5 → q5
Link6 → q6
8. Synchronisation

Fréquence de mise à jour :

Python : 10 Hz
Emulate3D : 10 Hz

Python :

time.sleep(0.1)

Emulate :

Wait.ForSeconds(0.1)

Cette fréquence permet une simulation fluide et stable.

9. Résultat

Le système permet :

récupération des angles du robot réel

transmission vers la simulation

reproduction des mouvements dans Emulate3D.

Le robot virtuel suit le robot réel en quasi temps réel, créant ainsi un jumeau numérique fonctionnel du UR3e.
