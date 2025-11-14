# Project: Dune Survivors

## 1. Project Summary

- **Technology:** Unity Engine, C#
- **Concept:** A 2D, top-down arena survival shooter. The core goal is to survive as long as possible against endless waves of enemies.

- **Core Loop:**
  1. **Survive:** Fight a wave of enemies.
  2. **Upgrade:** After the wave, options to upgrade the gun, heal, and increase max health displays.
  3. **Repeat:** Face a new, more difficult wave.

## 2. Core Gameplay Features

- **Player:**

  - Controls: WASD for movement, Mouse to aim and shoot.
  - Health: A simple heart-based health system.
  - Combat: Uses various guns with stats like `projectile count` and `fireRate`.

- **Enemies & Waves:**

  - The game spawns a set number of waves, each increasing in difficulty.
  - Enemies have different behaviors (Melee and Ranged).

- **Upgrades & Progression:**
  - A upgrade menu appears between waves.
  - It offers options to upgrade the gun, increase max health, and heal.

## 3. Key Systems to Develop

- **Player Management:** Handling movement, shooting, and health/damage.
- **Enemy Spawning:** A wave management system to spawn enemies.
- **UI & Upgrade Menu:** Managing the upgrade menu UI, upgrade selection, and applying upgrades.
- **Game State:** Controlling the flow between the Main Menu, Gameplay, and Game Over states.

## 4. Agent Instructions

- **Build Commands:** Do not attempt to execute build commands. The user will handle compilation and verification within the Unity editor.
- **Git Commands:** Do not execute any git commands (e.g., `git status`, `git add`, `git commit`). The user will handle all git operations.
