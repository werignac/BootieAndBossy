# Bootie and Bossy
## A Game about Sisterhood and Cooperation
Developed in Unity by werignac.
See game's credits for art, font, and audio contributions.

### Razor Pitch
Bootie and Bossy is a 2-player coop game based on a podcast where the players control two kneedles connected by a strand of yarn to collect good items and avoid the bad. 

### Link to Project
https://werignac.itch.io/bootie-and-bossy

### Link to Podcast (game is in the footer of the home page)
https://www.bootieandbossy.com/

### Current Features
- Two knitting needles controlled by WASD and the arrow keys.
- A resizeable thread of yarn that connects the needles (when a collectable is collected, the yarn grows in length).
- Collectable items that dissapear when they come in contact with the yarn.
- Bad items that cut the yarn when they come into contact with it.
- All major 2D art assets implemented.
- All major sound effects and music implemented.
- Interactive controls screen.
- Level scoring and stored high score.
- Audio settings.

### Yarn Implementation
The biggest challenge, but most important feature of this game is the strand of yarn. Currently, the yarn is simulated via dividing it into discrete 2D capsules with rigidbodies that are connected by hinge joints. The yarn is then drawn over it with a Line Renderer following a Catmull-Rom interpolation of the hinge joints.

### Postmortem
https://werignac.itch.io/bootie-and-bossy/devlog/468340/bootie-and-bossy-eat-knit-play-postmortem-werignac

### Next Steps
Officially the game is finished. That being said, I am considering adding updates.
Some things in the back of my mind include:
- Improving rope physics and drawing
- Pause menu
- Levels with obstacles
- Improved / more interesting graphics (shawdows)
- Networked multiplayer

I am also considering improving the documentation of the project as a profolio piece.
