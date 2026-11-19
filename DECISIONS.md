# JungleGame

# DECISIONS.md

## Week 10: Enemy AI

2026-11-11  Patroller update: asked the agent for a patrol FSM (Patrol -> Chase -> WindUp -> Recover). The enemy now walks between two waypoints, checks the player with distance, angle and a Raycast on the Walls layer (walls block the view), runs after the player, and turns red during a 0.5 s wind-up before the melee hit. I moved all numbers (view range, speeds, wind-up, recover) into serialized fields.

2026-11-11  Shooter update: added a laser line so the player can see the shot coming. The Shooter goes Idle -> Keep distance -> Aim -> Cooldown. In Aim it locks the direction, shows a red laser for 0.6 s and only then fires a slow bullet. Before, the shot had no warning. Aim time, cooldown and keep distance are serialized fields.

2026-11-11  Charger added: states Idle -> WindUp -> Dash -> Stunned. It turns red during a 0.7 s wind-up, dashes in a straight line, then turns gray and stands still for 1 s so the player can hit back. I changed its sprite so it looks different from the Patroller and the Shooter.

2026-11-11  Charger update: after the dash it hit the wall and stood there like a stone. I added a fifth state, Return: after the stun it walks back to its home point (3 m/s), turns to its original side and waits again. The agent's version had no way back, so the Charger stayed at the wall forever.

2026-11-11  Review found that player bullets never damaged the Charger (Bullet.cs only checked Patrol and EnemyShooter). Added the Charger there too, so it dies after 3 hits. Also made enemies start an attack only when they are inside the camera view (no attacks from off-screen).

2026-11-12  Playtest:  Sayat 2 tries, Erman 3 tries. Changed Wind UP  0.7  -> 0.9 and cooldown 1.5 -> 2.0. Now 2 tries.

## Week 11: Content and Data

2026-11-18 Current HP is now saved in Health.cs. The asset stores only max HP.

2026-11-18 Moved player, enemy, boss, bullet and bomb settings into separate data files.

2026-11-18 Enemies stay in the scenes. LevelData files store the time, coin goal and bomb settings for each level. One LevelLoader loads the data.

2026-11-18 The timer only counts down. There is no time penalty. Score is calculated by multiplying coins by scorePerCoin.

2026-11-18 JsonUtility does not support Dictionary, so unlocked levels are saved as a List<int>. BinaryFormatter is not used.

2026-11-18 SaveSystem saves to save.tmp first and then replaces save.json. Loading uses try/catch and returns default values if the file is broken. Tested with 5 broken files.

2026-11-19 The game saves after a level, when paused or closed, and when the volume slider changes. There are no saves during a level because each level takes only 1-2 minutes.

2026-11-19 The agent saved the volume only when Back was pressed. I added a short delay so the volume is saved after every slider change.

2026-11-19 Found 3 numbers in the code that needed to be moved into data. They were related to falling, walls and ground checks.

2026-11-19 SettingsScreen was missing from the function list, so I connected the settings buttons directly in the code using AddListener.

2026-11-19 Reset now clears the save file, PlayerPrefs, GameState and saved data in memory. Before, old values stayed until the game was restarted.