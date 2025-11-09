# Project: Dune Survivors

## 1. Project Summary

- **Technology:** Unity Engine, C#
- **Concept:** A 2D, top-down arena survival shooter. The core goal is to survive as long as possible against endless waves of enemies.

- **Core Loop:**
  1. **Survive:** Fight a wave of enemies.
  2. **Upgrade:** After the wave, choose from 3 random weapons to switch in a shop.
  3. **Repeat:** Face a new, more difficult wave.

## 2. Core Gameplay Features

- **Player:**

  - Controls: WASD for movement, Mouse to aim and shoot.
  - Health: A simple heart-based health system.
  - Combat: Uses various guns with stats like `damage` and `fireRate`.
    - The exception here is the shotgun where it can shoot multiple bullets

- **Enemies & Waves:**

  - The game spawns endless, escalating waves of enemies.
  - Enemies have different behaviors (Melee and Ranged).

- **Shop & Progression:**
  - A shop menu appears between waves.
  - It offers 3 randomized upgrade choices.
  - Upgrades can include new guns, weapon improvements, or increased health.

## 3. Key Systems to Develop

- **Player Management:** Handling movement, shooting, and health/damage.
- **Enemy Spawning:** A wave management system to spawn enemies.
- **UI & Shop:** Managing the shop UI, upgrade selection, and applying upgrades.
- **Game State:** Controlling the flow between the Main Menu, Gameplay, and Game Over states.
