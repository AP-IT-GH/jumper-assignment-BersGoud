# Tutorial Jumper

---

### Overview

This Unity project implements a cube agent trained using Deep reinforced learning to navigate and interact with its environment. The agent must avoid obstacles, collect rewards, and learn to move efficiently.

### Set-up

1. **Environment**:

   - The environment consists of a platform where the agent operates.
   - Place the platform prefab at for example coordinates (0, 0, 0).
   - Position the agent prefab at coordinates (0, 0.30, 0) on the platform.

2. **Agent**:

   - The `cubeAgent` script controls the behavior of the cube agent.
   - Attach the `cubeAgent` script to the agent prefab.
   - Set the parameters `moveSpeed` and `jumpForce` according to your requirements.

3. **Obstacles and Rewards**:
   - Obstacles and rewards spawn dynamically in the environment.
   - Obstacles include walls and beams, while rewards are spherical objects.
   - Assign the corresponding prefabs (`wallPrefab`, `linePrefab`, `rewardPrefab`) in the Unity Editor.

### Goal

The goal of the agent is to maximize its cumulative reward by avoiding walls, jumping over beams, collecting rewards spheres, and efficiently navigating the environment.

### Agent Reward Function

- **Positive Reward**:

  - +0.7 for collecting a reward sphere.
  - +3.0 for jumping over a beam obstacle.
  - +0.8 for successfully navigating past a wall obstacle.

- **Negative Reward**:
  - -1.0 for falling off the platform.
  - -0.01 for jumping too much.
  - -1.0 for hitting a obstacle.
  - -0.05 for navigating past a reward sphere.

### Observations and Actions

- **Observations**:

  - The agent's observation space includes the type of the random item (reward, wall, or line).
  - Observations are collected using the `CollectObservations` method in the `cubeAgent` script.

- **Actions**:
  - The agent can perform the following actions:
    - Continuous: Left/right movement.
    - Discrete: Jumping.

### Behavior Parameters

- **Vector Observation Space**: Size of 2 with 2 discrete branches, with 5 raycast beams per direction at 90 degrees, each detecting the type of the random item spawned.
- **Actions**:
  - Continuous: Movement in the horizontal direction.
  - Discrete: Jumping action.
- **Reward Function**: As described above.
- **Hyperparameters**: Customize hyperparameters in the config folder in CubeAgent.yaml file.

---

Made by Grim Van Daele & Bers Goudantov
students at AP Hogeschool
