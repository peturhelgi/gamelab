### Playtesting feedback

* Give me a second after I respawn
* Latch onto the rope while climbing, quite hard to stay on it
* When both miners jump, the camera gets too dizzy
* Press jump button to get off the rope


### Improvements

Make the menu less hardcoded.

### Tasks

MAIN TASK: Find people to playtest our game and record results. 

[] Nicolas: Add saving to the level editor 

[] Simon: Add sounds to the menu and the game

[] Andreea: Add platforms to the level editor

[] Andreea: Fix doors in level editor (when copy a door, make a new instance)

[] Petur: Add parameter tweaking to the level editor (via a nice menu)

[] Petur: Connect doors between levels (graph thingy)
 
[] Bjarni: Make sprites look better in the Picker Wheel

## 04.05.2018
### Progress report:
Nicolas: Found a solution to his burnt laptop problem

Bjarni: Fixed crate movement, added ladder and also rising platform with a lever;

Pétur: Implemented the GameOver when both miners fall down, added a fall damage, refactorized adding of the GameObjects to the GameState;
       Improved the Object Factory
       Made the darkness in the game be pitch black; 

Andreea: Added a tool above the miners; added pickaxe animation; added resources to the Level

BoundingBoxes of Rock and Miner should be changed, that the miner stands on the rock

Simon: 

### Discussions
Alpha release is by next Monday, at 5pm!!!

Ideas for more miners: model them as resources depending on the tools, and just swap tools when they die or when the 
user chooses another "miner".

Make a checkpoint at the beginning of the scene where the miner that died will respawn. 
Both miners will be respawned at the checkpoint when one dies. 
When all miners die, it's game over.

For the Menu selection, add a pickaxe symbol on the sides of the Menu Items to indicate what is active


### Improvements after alpha
Make camera change smoother/slower when the miner is running towards the edge


### Tasks
**Andreea**: 

[] Add more miners and the UI interface for switching between them

[] Center the tools above the miners

**Bjarni**:

[] Add rising platform mechanism sprite

[] Add a Rope as a tool

[] Make the platform also move horizontally

**Pétur**:

[] Add key to door

[] Add green/red above the door depending on whether it's unlocked or the key has been picked up by a miner

[] Make the directional lights movable

[] Create the game world as a graph (A World is comprised of more levels, as in what we currently have)

**Nicolas**:

[] Integrate the Menu images within our game (make MenuManager able to load new levels)

[] Add saving to Level Editor

**Simon**:

[] Sound effects


---

Next meetings: Friday, 04.05.2018, 12:00 - Tuesday, 8.05.2018 after class

## 02.05.2018
### Progress report:
Nicolas: Laptop is burnt. He will use the Lab PCs

Bjarni: Rocks are solid, can pick up crate (jump not yet affected, but collides, can't push it), ladder controls are not perfect yet, but already work pretty well

Pétur: on L: no lights, Advanced debugger on P
Object Factory: on GameObject and Tools
Exit door, i.e. you won

Andreea: Animation works, with some miner inconsistencies. BoundingBoxes of Rock and Miner should be changed, that the miner stands on the rock

Simon: Multiple things on the GameEditor

### Discussions
None

### Tasks
**Andreea**: 

[] Adds more sprites to the **Miner** and starts with showing the animations

[] Adding Tools onto the miner (Drawing)

[] GameMenu, GameOver, LevelCompleted, PauseMenu, LevelSelector
[] Add credits


**Bjarni**:

[] Ladder / Create: minor control fixes, change box to face direction of walking

[] Create some interactibles: Ropes, Plattforms

**Pétur**:

[] change debug function (right now when pressing `P`) for the collisions (i.e. drawing a polygon, where the BBoxes are, ...)

[] GameOver functionality (falling out of the level, including removal of Objects which drop out of the level, add fall damage )

[] Add key to door

**Nicolas**:

[] Add saving to Level Editor

[] GameMenu

**Simon**:

[] Level Editor 


---


## 26.04.2018
### Rules
Are OK, are now final and added to the readme

### Progress report:
Nicolas: As some issues with his laptop, partally helped Simon in his device
Bjarni: Had a presentation, wasn't able to do anything yet
Pétur: Menu overlays one ( GameOver, Won, Pause Menu), ready to merge
Simon: Level Editor, ready to merge first version
Andreea: Animation: Thought about how to integrate them into the renderer process, would need to load all sprites

### Discussions:
We'll use a Factory pattern to init the GameObjects

### Tasks
**Andreea**: 

[] Adds more sprites to the **Miner** and starts with showing the animations

[] Look at Sprite Mesh Rendering possibilities

**Bjarni**:

[] Adding seperate BBox for the Miner to indicate the area, in which he can interact with things.

[] Changing the seperation of collectibles, solids and miners ( see GameState lines 22ff)

[] Create some interactibles: Crates & Rocks, Ropes and Ladders

**Pétur**:

[] change debug function (right now when pressing `P`) for the collisions (i.e. drawing a polygon, where the BBoxes are, switching of lighting , ...)

[] GameOver / "You Won" / Pause-Menu Merge

[] Object Factory


**Nicolas**:

[] Fix his Laptop (or set it on fire)

**Simon**:

[] Level Editor (& Merge)

[] Look at Sprite Mesh Rendering possibilities

---

## 24.04.2018
### Progress report:
Andreea: created a bigger level: we need to tweak jumping height and speed (sprint) to make the levels more interesting.
Bjarni: Controlls on XBox are working
Simon: Added flickering lighting, rules and mettings to repo
Pétur (absent): Report/Porject creation

### Discussions:
We shortly discussed about possible approaches for the Level Editor and then created a list with all task for the next two weeks (at the bottom of this document), then picking the tasks for this week: 

### Merge Decisions
None for this Meeting


### Task to complete until the next meeting
**Andreea**: 

[] Adds more sprites to the **Miner** and starts with showing the animations

**Bjarni**:

[] Adding seperate BBox for the Miner to indicate the area, in which he can interact with things.

[] Changing the seperation of collectibles, solids and miners ( see GameState lines 22ff)

[] Create some interactibles: Crates & Rocks

**Pétur**:

[] change debug function (right now when pressing `P`) for the collisions (i.e. drawing a polygon, where the BBoxes are, switching of lighting , ...)

[] Pause-Menu (a Menu overlaying the Game Screen, i.e. render both on top of each other)

[] GameOver / "You Won" Screens (not connected to interactibles yet, but show then on a certain button-press, i.e. `F1` and `F2`

**Nicolas and Simon**:

[] Level Editor

---

## 22.04.2018

### Decisions:
- The structure used in the collsion_detection branch will from now on be used for the Game.
- The report will be written in LaTeX
- The rules will be discussed and decided on in the next meeting

### Tasks:

Bjarni: Adding the controlls for the XBox

Pétur: Write a draft for the report, compile it and hand it in, test the game on the XBox and hand it in

Andreea: Create a bigger level to show durring the presentation

Simon: Merge collision_detection into master, add the proposed rules to the README, Make Lighting 'fancier'/more flexible

Everybody: collaborate on the report

---


## List of Task to implement for the alpha release
- Level Editor
- interactibles
-- doors to walk through and final escape-door
-- dying (i.e. when the miner is not within the bounding box describing the level anymore)
-- seesaw
-- movable crates
-- minable rocks
-- moving plattforms
-- buttons/levers
- sound (music and gamesound)
- miners
-- switching miners
-- add more types of miners
-- add more visual looks of the miners
-- animate miner movement
- improve lighting/shaddow casting
- improve the camera
- Game Overlay 
-- hints (i.e. hint that the player can interact with a certain object)
-- UI (which miners are available, ...)


