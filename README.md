# UR3e Digital Twin Project Tutorial

This repository documents the implementation of a **digital twin of a UR3e robot** using:

- a **real UR3e robotic arm**
- **Python RTDE** for real-time joint acquisition
- **CSV-based communication**
- **Emulate3D** for simulation and visualization
- **C# controllers** attached to each robot joint in Emulate3D
The objective is to reproduce the motion of the **real robot** on a **virtual robot** in near real time.

## Table of Contents
1. [Introduction](#introduction)
2. [Project Architecture](#project-architecture)
3. [Code Examples](#code-examples)
4. [Python RTDE Explanation](#python-rtde-script-explanation)
5. [Emulate3D Configuration](#emulate3d-configuration)
6. [C# Controller Scripts](#c-controller-scripts)
7. [Synchronization Details](#synchronization-details)
8. [Troubleshooting Guide](#troubleshooting-guide)
9. [Future Improvements](#future-improvements)
10. [Conclusion](#conclusiob)


## Introduction

The UR3e Digital Twin project connects a **real Universal Robots UR3e arm** to a **virtual robot in Emulate3D**.

The real robot provides its **actual joint angles** through the **RTDE interface**.  

A Python script reads these angles, converts them into degrees, applies calibration offsets, and writes the results into a CSV file.

Emulate3D then reads this file and uses **C# scripts** attached to each robot link to update the corresponding **joint positions** through `PositionController` aspects.

This creates a simple but effective **digital twin architecture** where the virtual robot follows the real robot motion.

---

## Project Architecture
The system is based on a simple and readable architecture:

```text
UR3e Real Robot
      ↓
RTDE Communication
      ↓
Python Acquisition Layer
      ↓
angles_robot.csv
      ↓
Emulate3D Joint Controllers
      ↓
UR3e Virtual Robot
```

## Code Examples
### Python RTDE Script
```python
# Example RTDE script for controlling the UR3e robot
import rtde

def main():
    # Connect to the robot
    rtde_connection = rtde.RTDE("192.168.x.x", 30004)
    rtde_connection.connect()
    # Your code here
...
```
## Python RTDE Explanation

Python acts as the data acquisition and formatting layer.
Its role is not to control the virtual robot directly, but to serve as an intermediary between the physical robot and Emulate3D.
The Python side performs four main functions:
-connect to the UR3e through RTDE,
-read the actual joint positions,
-convert and calibrate the values,
-export the data into angles_robot.csv.

A calibration step is necessary because the reference positions of the real robot and the virtual robot are not strictly identical.
This means that some joint values must be corrected before being sent to the simulation so that both robots share the same visual and mechanical reference.

### C# Controller Scripts
```csharp
// Example C# script for controlling the robot
using System;
...
```

## Synchronization Details
The system is synchronized through a periodic update cycle.
The real robot data is exported at a fixed interval, and Emulate3D reads the file at the same rate.
The chosen refresh rate is 10 Hz.
This means the data is updated every 0.1 second.
This frequency provides a good compromise between:

-motion fluidity,

-file access stability,

-implementation simplicity.

It is sufficient for a visual digital twin demonstration and for most educational or prototyping scenarios.

## Troubleshooting Guide
### 1.Virtual robot does not move:

This usually indicates that one of the following elements is missing or incorrectly configured:

-motor,

-PositionController,

-link hierarchy,

-script assignment,

-CSV update.  

The first checks should always be:

-confirm that the CSV file is being updated,

-confirm that Emulate3D reads the correct file,

-confirm that each link is associated with the right controller,

-confirm that every moving joint has both a motor and a PositionController.  

### 2.Joint motion is incorrect:

If a link moves in the wrong direction or starts from the wrong orientation, this generally comes from a mismatch between the real robot reference frame and the virtual robot reference frame.
This is solved by calibrating the joint values before they are used by Emulate3D.  

### 3.Motion is unstable

If the robot appears jerky or inconsistent, the issue is usually caused by a mismatch in update rate or by excessive file access.
Using the same refresh interval on both sides improves stability and consistency.

## Future Improvements
This project can be extended in several ways.

A first improvement would be to replace CSV communication with a more direct real-time method such as TCP or OPC UA.

A second improvement would be to centralize the control inside Emulate3D so that a single controller manages all six joints instead of one controller per link.

A third improvement would be to compare the real TCP pose with the virtual TCP pose in order to validate the digital twin quantitatively.

Finally, the same architecture could be expanded to model a complete robotic cell including grippers, conveyors, sensors, and workstations.

## Conclusion
This project demonstrates a complete digital twin workflow for a UR3e robot:

-acquisition of real robot joint data,

-transfer of the robot state to the simulation,

-configuration of a kinematic virtual model,

-reproduction of the physical motion in Emulate3D.

The result is a functional and understandable digital twin architecture suitable for a master's project in industrial informatics and robotic simulation.
---
*Last updated: 2026-03-13*
