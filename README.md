# Drone Delivery Game (Unity)

A 3D drone game built in Unity 6. You fly a delivery drone, collect packages, and fend off rogue drones that hunt you down.

## Gameplay
- Collect **10 packages** to win. Touching a **rogue drone** ends the run.
- **Rogue drones chase you** with separation steering, so they spread out around you instead of clumping together.
- **EMP blast:** stuns nearby rogue drones for a few seconds (visible colour flash and spin).
- **Stunned drones that collide destroy each other and drop a package.**
- Scenes: Main Menu, Level 1, Game Over, Victory.

## Controls
| Action | Key |
|---|---|
| Move forward / back | W / S (or Up / Down) |
| Turn | A / D (or Left / Right) |
| Ascend / descend | E / Q |
| EMP blast | Fire1 (Left Ctrl or left mouse button by default) |

## Code layout (`Assets/Scripts`)
| Script | Role |
|---|---|
| `DroneMovement.cs` | Player flight controls (Rigidbody-based) |
| `RogueDroneMovement.cs` | Enemy chase AI, separation, stun, destruction and package drop |
| `PlayerEmp.cs`, `EmpWave.cs` | EMP spawn and expanding trigger wave |
| `PlayerCollision.cs` | Package pickup, score, game-over / victory transitions |
| `CameraFollow.cs`, `VictoryManager.cs`, `GameOver.cs`, `MainMenuButton.cs`, `MenuDroneSpin.cs` | Camera, UI and scene flow |

## Setup
1. Open the project with **Unity 6000.5.4f1** (Unity 6) via Unity Hub.
2. Import the free **AllSkyFree** skybox pack from the Unity Asset Store (not included here because of size and licensing). Without it the skies will show as missing materials but the game still runs.
3. Open `Assets/Scenes/MainMenu.unity` and press Play.

## Things I learned
- Kinematic Rigidbodies ignore colliders, which is why my drone kept passing through walls.
- Unity's collision callbacks can hit a child object instead of the object holding your script, so use `GetComponentInParent`.

Author: Soha Chand
