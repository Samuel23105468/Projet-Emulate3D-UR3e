import time
import csv
import math
from rtde_receive import RTDEReceiveInterface

ROBOT_IP = "192.168.1.100"

OUTPUT_FILE = r"C:\Users\Etudiant\Desktop\M2_IN\Projet Jumeau numérique\angles_robot.csv"

print("Connexion au robot UR3...")

rtde_r = RTDEReceiveInterface(ROBOT_IP)

print("Lecture RTDE démarrée")

# offsets calibrés pour ton Emulate
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

        print("Angles Emulate:", q1, q2, q3, q4, q5, q6)

        with open(OUTPUT_FILE, "w", newline="") as f:
            writer = csv.writer(f)
            writer.writerow(["time","q1","q2","q3","q4","q5","q6"])
            writer.writerow([int(time.time()), q1, q2, q3, q4, q5, q6])
            f.flush()

    except PermissionError:
        print("Emulate utilise le fichier...")

    time.sleep(0.1)
