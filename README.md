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
4. [Python RTDE Script Explanation](#python-rtde-script-explanation)
5. [Emulate3D Configuration](#emulate3d-configuration)
6. [C# Controller Scripts](#c-controller-scripts)
7. [Synchronization Details](#synchronization-details)
8. [Troubleshooting Guide](#troubleshooting-guide)
9. [Future Improvements](#future-improvements)


## Introduction

The UR3e Digital Twin project connects a **real Universal Robots UR3e arm** to a **virtual robot in Emulate3D**.
The real robot provides its **actual joint angles** through the **RTDE interface**.  
A Python script reads these angles, converts them into degrees, applies calibration offsets, and writes the results into a CSV file.
Emulate3D then reads this file and uses **C# scripts** attached to each robot link to update the corresponding **joint positions** through `PositionController` aspects.
This creates a simple but effective **digital twin architecture** where the virtual robot follows the real robot motion.
---

## Project Architecture
The overall system is based on the following data flow:

text'''
UR3e Real Robot
      ↓
RTDE Communication
      ↓
Python Acquisition Script
      ↓
angles_robot.csv
      ↓
Emulate3D C# Joint Controllers
      ↓
UR3e Virtual Robot


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
##Python RTDE Script Explanation


### C# Controller Scripts
```csharp
// Example C# script for controlling the robot
using System;
...
```

## Synchronization Details
- Detailed description of synchronization between the robot and simulation...

## Troubleshooting Guide
- Common issues and how to resolve them...

## Future Improvements
- Suggestions for future enhancements...

## Conclusion
This tutorial aims to equip users with the knowledge needed to successfully implement and work with the UR3e Digital Twin project. For detailed explanations, refer to each section as needed.

---
*Last updated: 2026-03-13*
