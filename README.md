# GMTK-game-jam-2023

*Prompt:* Roles Reversed

*Inspirations:* Papers Please, Stardew Valley

*Idea:* A stressful Stardew Valley-like

*Setting:* A fantasy setting where the monstrous forces are the human kingdom encroaching on the goblin's land. There are resistance goblin fighters.
Player Role: Not a resistance fighter, but a goblin subsistance farmer trying to survive and grow crops
Calendar system: On every passing day you spend action points to plant and water crops.
	You only have as many AP as you are full, so you'll have to eat some of your crops too
Each day you can:
	* Eat food (+AP)
	* Plant/water crops (-AP)
	* Random event (choices and consequences)
Does time advance during the day?
	Yes, and that can affect when the random event occurs and so you might have sold or eaten food already


*Win condition:* Just to survive as long as possible, or until the war ends (with an option to keep playing or start a new run). The war ends on day 20(?) at which point we'll flip a coin that is weighted by how much you supported either faction to determine the winner.

*Start with random events (maybe add fixed events later)*
	Griffin infestation (they eat your crops until you pull them off, takes AP and receives a wound)
	Robber demands either money or crops (you pick), or you fight them off (takes AP and maybe receives a wound)
	Starving child and mother show up, begging for food (give or don't)
	Tax collection from either faction (money only or else a severe warning)
	Nearby battle, your plot is damaged
	Drought, watering costs twice as much AP for the day
	Recruitment to the cause to fight for the goblins (if yes, + goblin faction points, but you get wounded affecting your AP)
	Injured soldier appears asking for help (give food, or turn over to other faction for money)
	Hide an injured human in your house
	Inspector shows up (if they find a human then they get killed and you pay a fine)
	Mercenaries show up demaning protection money (they will protect you for the next robbery/griffin attack)
	Rain (no AP to water crops)
	War treasure washes up on the river (money and/or health potion)
	Merchants show up for better prices (they also sell health potions rarely)
Fail conditions (result in the end of a run)
	Starving
	Death for not paying taxes
	Killed as a goblin traitor
	3 wounds = death



*Technical details:*
One main screen	which is the player farm, top down 2D camera view ala Star Dew Valley
Small plot of land and a tiny house
Start with two crops: cheap and 1 day to grow, expensive and 3 days to grow
2D Tile Map
The farm is the only space the player moves in
Money as a separate system from the Food+Seed inventory
Clicking on a plot asks you which of your inventory seeds you'd like to plant (takes AP)
Clicking on a plant waters it (takes AP)
A grown plant will be added to inventory when interacted with (takes AP)
Starvation affects the AP you start with the next day
For random events, new character sprite show up on the farm and trigger a character portrait dialog system with choices
Magic computer for ordering more seeds, either from the humans or other goblins, that arrive 24 hours later

*Important questions:*
What is the alternative to planting? What makes it a tricky decision?


Sprints:
* Sprint 1: Experience the thinnest version of the Game Feel (2 days)
** [Jenn] All the art for Sprint 1
** [Mat, Sam S] Player Systems
*** Map for farm
*** Player sprite + movement
** [Felix] Inventory System (attachable to both Player and Merchants)
*** Money
*** Seeds, Crops (items)
** [Sam S] Crop management
*** Planting seeds, watering seeds
***  Harvest quick seeds/plants on day 2
** [Franklin] Time Management:
*** Day-clock time mechanic
*** Calendar mechanic
** [Franklin?] Event System:
*** First day event for purchasing seeds from merchant
*** Tax event on day 2 (can you pay or not?)
** [Cho] Basic UI:
***Title screen
***Setting Screen
***
* Sprint 2:
** end days early
** starvation
** Action Points (AP)
* Sprint 3:
** 
