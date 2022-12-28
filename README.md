# Bootie and Bossy
## A Game about Sisterhood and Cooperation
Developed in Unity by werignac.

### Razor Pitch
Bootie and Bossy is a 2-player coop game based on a podcast where the players control two kneedles connected by a strand of yarn to collect good items and avoid the bad. 

![](https://github.com/werignac/BootieAndBossy/blob/main/Demo%20Materials/15fps_bootie_and_bossy_demo.gif)

### Current Features
- Two knitting needles controlled by WASD and the arrow keys.
- A resizeable thread of yarn that connects the needles.
- Collectable items that dissapear when they come in contact with the yarn.

### Yarn Implementation
The biggest challenge, but most important feature of this game is the strand of yarn. Currently, the yarn is simulated via dividing it into discrete 2D capsules with rigidbodies that are connected by hinge joints. The yarn is then drawn over it with a Line Renderer following a Catmull-Rom interpolation of the hinge joints.

### Next Steps
- Find a good segment resolution for the yarn.
- Implement having the yarn grow as good collectables are collected.
- Implement collectables that can cut the yarn.
- Implement collectable oversight.
- Sprite work.
- Sound effects and music.
- Main Menu.
- In-game tutorial.
