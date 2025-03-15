# A Mod fore the server FR horlogerie
But you can use on other !

## 📌 Description
This mod is created for the French server **"Horlogerie"**. At the moment, it doesn't include much, but in the future, many features will be added (so stay tuned!).
Although it was developed for the **"Horlogerie"** server, you can use it on any server. The configuration allows you to disable features that don’t suit you and/or modify certain aspects or behaviors of the mod.

The mod is currently translated into two languages: **FR** and **EN**.


## ⚙️ Features
- A **Translocation Device**: configurable teleportation tool.
- A **Configuration Workshop**: allows setting teleportation rules.
- Customizable teleportation ranges and behaviors.

## 🛠️ Configuration
Modify the `horlogeriemodpack.json` file to change settings. Below are the parameters you can adjust:

```json
{
  "UseCharge": true, // Enable energy consumption
  "MaxCharge": 3, // Maximum charge before depletion
  "DestroyAfterUse": true, // Whether the device is destroyed after use
  "CanConfigureForToNorth": true, // Allow teleportation to the north
  "MaxNorthTeleportationRangeZ": 500,
  "MinNorthTeleportationRangeZ": 100,
  "NorthTeleportationRangeX": 200,
  "CanConfigureForToSouth": true, // Allow teleportation to the south
  "MaxSouthTeleportationRangeZ": 500,
  "MinSouthTeleportationRangeZ": 100,
  "SouthTeleportationRangeX": 200,
  "CanConfigureForToEast": true, // Allow teleportation to the east
  "EastTeleportationRangeZ": 200,
  "MaxEastTeleportationRangeX": 500,
  "MinEastTeleportationRangeX": 100,
  "CanConfigureForToWest": true, // Allow teleportation to the west
  "WestTeleportationRangeZ": 200,
  "MaxWestTeleportationRangeX": 500,
  "MinWestTeleportationRangeX": 100,
  "CanConfigureForRandom": true, // Allow teleportation to a random position around you
  "MaxRandomTeleportationRangeZ": 500,
  "MinRandomTeleportationRangeZ": 100,
  "MaxRandomTeleportationRangeX": 500,
  "MinRandomTeleportationRangeX": 100,
  "CanConfigureForPlayer": true, // Allow teleporting to a specific player
  "CanConfigureForSpawn": true, // Allow teleporting to world spawn
  "CanConfigureForSpawnPlayer": true // Allow teleporting to player's spawn
}
```

## 🔜 Future Updates
- Additional teleportation mechanics (convocation, mass teleportation).
- Addition of a travel journal.
- Addition of crafting recipes.
- **New tools**: Trowel, Branch Cutter.

## 🙌 Contributors & Feedback
Feel free to contribute or report issues in the [GitHub Issues](https://github.com/Runeranock/HorlogerieModPack/issues) section.

---
© 2025 Runernaock | License: MIT
