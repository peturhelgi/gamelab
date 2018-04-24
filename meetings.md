# Meetings

Next meetings: Tuesday, 24.04.2018, after class - Friday, 27.04.2018, 12:00 - Tuesday, 1.05.2018


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


