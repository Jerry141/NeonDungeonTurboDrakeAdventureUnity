# Neon Dungeon Turbo Drake Adventure

## Table of Contents
* [General Info](#general-information)
* [Content](#content)
* [Screenshots](#screenshots)
* [Manual](#manual)
* [Project Status](#project-status)
* [Room for Improvement](#room-for-improvement)
* [Acknowledgements](#acknowledgements)
* [Contact](#contact)


## General Information
- Neon Dungeon Turbo Drake Adventure (NDTDA) is a Rogue Like inspired by no other game than Rogue itself, Synthwave and Vampires.
- You play as the Turbo Drake - manifestation of Vlad Drakula Tepesh in the Synthwave inspired world where Neons are the source of power.
- The lower you go through the dungeon the more dangerous it gets, be ready to get more powerful as well by feeding upon the enemies residing this wretched place.
- The game is in very early stages of development, but content is planned to be added as frequent as possible, as this is my side project and I have no time or resources to commit to this 100% unfortunately.
- The whole purpose of this project is to learn C#, Unity and overall game development. It is just more fun to do with some story.
- Everything is a subject to change, I will treat this one as my sandbox and will mess around from time to time.

The game is possible to test here: [Unity Play](https://play.unity.com/mg/other/ndtda_build1)


## Content
### Enemies:
- Commoner [C] - Low class human, for some reason there are many of them in the dungeon. They posses no real threat, easy prey for Drake.
- Templar [T] - Templars are assigned by the Neon Church to protect the Commoners inside the dungeon, the lower you go the more you will encounter them, looks like they do not bother about the entrance anymore. Fairly powerful beings.
- Vampire Hunter [H] - Hunters set by the Neon Church to eradicate Vampires searching the dungeon for Neon Power. Well trained and viscious against their target, even with low defenses, they are a big threat to any Vampire deciding to venture through the Dungeon.
- Neon Addict [a]- Neon addicts are mostly commoners who were exposed to the Neon for so long that they have lost their mind and the only thing they want is to get more Neon. They are very vulnerable but due to huge exposure to the Neon, they are very strong and should be erradicated before they have a chance to attack. 
- Miner [M] - Miners were ordered to work tirelessly in the dungeon to provide the Neon to the Church. Over time and due to huge exposure to the Neon they became the husks of their formerselves. Miners will attack anyone who will spoil their focus.
- Supervisor [S] - Supervisors were originally assinged to keep Miners in check and to make sure that the Church will get their precious Neon

### Items:
- Neon Blood Vial [!] - Blood Vial contaminated by the large amount of Neon - for mere human it is like a drug, easy to get addicted to it and loose sanity trying to find more and more. For Drake, it is great source of Health, fantastic to regain Vitals before next fights.
- Neon Bolt Chip [~] - This Chip contains technology to cast Neon Bolt, harming enemies by dealing them massive damage.
- Confusion Chip [~] -  This Chip contains technology to cast Confusion "spell", making enemies lose their mind for a brief moment.
- Neonball Chip [~] - This Chip contains technilogy on how to cast Neonball, powerfull AoE spell, dealing damage on fairly wide range.
- Life Steal Chip [~] - This chip contains the forbidden Vampire technology. With infusing the victim's blood with Neon, Vampires are able to transfer it to themselves. Restores some HP upon using, while damaging the enemy.

### Equipment:
- Neon Dagger [/] - Simple dagger infused with Neon, probably due to long exposure within the dungeon. Great for dealing with small threats
- Neon Saber [/] - Saber greatly infused with Neon, can be found in lower levels of the dungeon, probably was used by previous residents of this place, an elegant weapon for a more civilized age.
- Pickaxe [/] - Simple pickaxe covered with neon. Used to mine the Neon from the dungeon.
- Leather Jacket [ [ ] - Light leather jacket. It is more about style than defense, but still better than nothing.
- Enforced Jacket [ [ ] - Leather Jacket enforced with small amount of Neon. Provides moderate defense.
- Vampire Robe [ [ ] - Robe designed by the high Vampires, provides good defense to the Vampire. Humans tried to use those but it only slows them down.


## Screenshots

- <img width="1578" alt="start" src="https://github.com/Jerry141/NeonDungeonTurboDrakeAdventureUnity/assets/38975789/419450dd-91dd-4fca-80d6-97c9a5d55d6e">
Start of the game

- <img width="289" alt="image" src="https://github.com/Jerry141/NeonDungeonTurboDrakeAdventureUnity/assets/38975789/7fe2f58a-660d-45a1-89e3-86bb66fb03da">
[!] - Blood Vial, [%] - Enemy's Corpse

- <img width="1574" alt="image" src="https://github.com/Jerry141/NeonDungeonTurboDrakeAdventureUnity/assets/38975789/4f30bfd1-9306-4079-b22d-eb6031da6555">
Level Up menu

- <img width="335" alt="image" src="https://github.com/Jerry141/NeonDungeonTurboDrakeAdventureUnity/assets/38975789/f1b0724e-d47e-438f-9b95-166214bf889d">
Stairs down to lower level

- <img width="497" alt="image" src="https://github.com/Jerry141/NeonDungeonTurboDrakeAdventureUnity/assets/38975789/c0844646-4988-4572-bbff-1ffbcc44963a">
Inventory

- <img width="1574" alt="image" src="https://github.com/Jerry141/NeonDungeonTurboDrakeAdventureUnity/assets/38975789/563369d0-f012-4e58-9d00-d2281f426ce3">
Character information

- <img width="1533" alt="image" src="https://github.com/Jerry141/NeonDungeonTurboDrakeAdventureUnity/assets/38975789/9d1dc424-560e-4780-9a30-0285d4bb3412">
Message history

## Manual

Neon Dungeon is a very dangerous place, therefor it is nice to know how to move around this wretched place.

### Key Bindings:

Movement:
- UP - [Arrow Key Up]
- DOWN - [Arrow Key Down]
- LEFT - [Arrow Key Left]
- RIGHT - [Arrow Key Right]


Inventory:
- Open Inventory - [i]
- Use Items - [Enter] / [Mouse Click]
- Drop Items Menu - [d]
- Drop Items from Drop Item Menu - [Enter] / [Mouse Click]
- Pick Up Item - [g]

- Go to the next level - [Enter] - first you need to stand on [>]
- Open message log - [v]

- [C] - Character information
- [Esc] - Escape menu (not fully functional yet)

### Combat:
The Combat in NDTDA is turn based. Once player moves/pick up items/uses item it counts as a turn. After that every enemy on the map (if visible) takes their turn to get close to the player and attack them.

Attacking is done by moving into the enemy, same comes from the enemies. Attack can be done from 8 directions - Horizontaly, Vertically and Diagonaly.

Using Chips - to use the spell you need to go to inventory [i] and select the Chip:
- Neon Bolt Chip - attacks for 20 damage, max range is 5 dots. Targets the closest enemy.
- Confusion Chip - deals no damage, confuses the enemy, targetable - target must be visible and cannot be the player, confusion effect lasts for 10 turns.
- Neonball Chip - deals 12 damage within the radius of 3x3 dots. Targetable, can deal damage to the player if within the radius.
- Life Steal Chip - deals 10 damage at maximum range of 5 dots. Heals player for 10 HP.

### Equipment:
There are currently 2 types of equipment: Armor and Weapon.

To equip gear you need to go into the Inventory [i] and select equipment to equip, in the inventory you will notice (E) next to the equipment if equiped.

You can have only 1 of each type equiped.

In Game there are currently the following equipment items:

Weapons:
- Neon Dagger - adds 2 power - starting weapon
- Neon Saber - adds 4 power
- Pickaxe - adds 6 power

Armor:
- Leather Jacket - adds 1 defense - starting armor
- Enforced Jacket - adds 3 defense
- Vampire Robe - 5 defense


## Project Status
Project is: _in progress_ (indefinitely)

Current focus:
- UI - Inventory, Drop Menu, Main Menu, Death Menu, Character information etc.
- Adding new menus (Death menu, info menu, equipment menu - separate from inventory)

## Room for Improvement

Room for improvement:
- Banalce - at the moment the game is not balanced at all
- Improve the Inventory to be less messy (Sorting and stacking)
- Imporove UI 
- Add sounds
- Add end game menu - restart / quit (upon reaching final floor if there will be one)
- Inventory - Drop menu - merge


## Acknowledgements
- This project was inspired by Rogue: Exploring the Dungeons of Doom
- This project was initially based on [this tutorial](https://rogueliketutorials.com/tutorials/tcod/v2/).
- Porting from Python to Unity was done with help of [The Sleepy Koala](https://www.youtube.com/@thesleepykoala) and his amazing youtube series.
- Main Menu background - found in google, unfortunately I forgot to write down the author, so if you find it somehow please let me know so I can properly mentioned them. For now it is only a placeholder but I really like it so it would be nice to keep it as is.


## Contact
Created by [@JayJayJerry](https://www.facebook.com/jayjayjerrygaming) - feel free to contact me!
