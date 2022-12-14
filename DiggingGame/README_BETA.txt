~ ADDITIONS
- Building Info
	~ This tab gives you information on your Buildings, their Cost, and purpose. Treat it as a quick guide to remember
	what your Buildings do. 
- Weather
	~ Cards now change the in-game Weather in different ways. Additionally, every round, it'll shift from Day to Night
	and visa-versa. 
- Gallery
	~ You can now view cards in the gallery! Prepare yourself before playing!

~ CHANGES
Mechanics:
	- You can now Build on Gold Pieces. Buildings on Gold are indestructable but cost 1 additional card. Mines will 
	collect Gold Pieces from the Supply.

Balance:
- Thief (1 Grass -> 2 Grass); Dirty Thief (1 Dirt -> 2 Dirt); Master Thief (1 Grass 1 Dirt -> 1 Grass 1 Dirt 1 Stone, Steal
  4 -> Steal 3)
	~ Thief cards, at the moment, are capable of badly stunting the opponent player very early on. Raising the cost for
	Thief cards to be equivalent with their steal amount gives them a more strategic purpose later on into the game flow
	that allows players to commit more to slowing down their opponent. Master Thief's cost was updated according to this
	rules but its current cost should make its activation amount less prevalent.
- Shovel (1 Dirt 1 Stone -> 1 Dirt)
	~ Shovel's stone cost was a tad steep. Its new price should put it more in line with Morning Jog.
- Geologist (1 Dirt 2 Stone -> 2 Dirt 1 Stone)
	~ Geologist is a very powerful card if a player is able to play it, but it still only tends to shine once every few
	games. This updated cost should make the card more prevalent in games and would make losing it to Flood less punishing.
- Master Builder (Rework: Your Building Costs are reduced by 1.)
	~ Master builder, before this rework, sat in an awkward position where actually being able to pay its cost and get its
	max reward was rare and debatably not worth the setup necessary. Its cost was kept the same for this rework to reflect
	its long term reward. Playing Master Builder early in a game could provide huge payoff to a player.
- Earthquake (1 Dirt 2 Stone -> 1 Grass 1 Dirt 1 Stone)
	~ Earthquake is a very powerful card, yet tends to get underutilized due to its 2 Stone cost, leaving most buildings on
	Stone untouched for the entire game. This new cost should reflect its thematic purpose and make it more common in games.
- Metal Detector (1 Stone -> 1 Dirt); Deck Presence (1 -> 2)
	~ Metal Detector had a very expensive cost for a possibly detrimental effect. With updated changes to Gold Pieces on the
	board, Metal Detector should become a commonly played card that leads to interesting strategies between players. 
- Planned Gamble (1 Stone -> 1 Dirt)
	~ Planned Gamble is exactly that, a gamble. And spending stone on a card that could rarely give you benefit isn't 
	something that's too readily done. This change should make Planned Gamble still not overly easy to play, but would give
	it a dynamic purpose in the game and control deck cycling more.
- Dam (1 Dirt 1 Stone -> 1 Dirt 1 Grass)
	~ Protecting a building on Dirt is already fairly niche, making Dam's stone cost fairly unjustified. This should allow
	the card to be easier to play.
- Weed Whacker & Dam (Rework: Discard Immediately, Dice = 1)
	~ Weed Whacker & Dam now both happen immediately once a Building matching their type is selected to be damaged. This is
	to allow the cards cycle back into the deck quicker instead of sitting in a player's hand all game. Additionally, the 
	card now simply forces a roll of 1 to increase simplicity.
- Garden (1 Dirt -> 3 Dirt)
	~ Garden is a wildly powerful card, being able to score 1 point very easily, along with a possible 2 points. This change
	makes the card fairly expensive, which we believe is a justified cost for how powerful the card proves to be. 
- Fertilizer (1 Grass 1 Dirt -> 2 Dirt)
	~ Fertilizer, in comparison to Flowers and Compaction, was somewhat difficult to use at most points of the game due to
	the Dirt Supply being fairly empty. Making it similar to other placement cards by making its cost equivalent to its
	placement suit should allow players to use it more often.
- Erosion (1 Grass 1 Dirt -> 1 Stone)
	~ Updated Erosion's cost to keep it more in line with Lawnmower and Excavator. 
- Compaction (Deck Presence 2 -> 1)
	~ Compaction now only has a single copy of itself in the Universal Deck. Being a card with somewhat niche use until
	later in rounds, its common appearance wasn't justified. 
- Transmutation (1 Point -> 1 Point per 2 Supply Gold)
	~ Transmutation's old effect was pretty uninspired. This change should have its purpose fluxuate through play, making
	the Card act primarily for any suit early on, and in rare cases, score massive points.
- Tornado (3 Grass 1 Stone -> 3 Grass 2 Dirt 1 Stone)
	~ Tornado is easily the most powerful card in the game, capable of wiping a player off the board in a single activation.
	This activation should be a players' end goal, not just something haphazardly activated. Its current cost acts of as a
	combination of other disaster cards, marking it as the most powerful disaster.
- Gold Cards
   ~ Golden Shovel (1 Grass 1 Dirt -> 1 Dirt 1 Stone)
   ~ Regeneration (3 Grass -> 2 Grass 1 Dirt 1 Stone)
   ~ Tornado (3 Grass 1 Dirt -> 3 Grass 1 Stone)
   ~ Transmutation (1 Stone -> 1 Dirt 1 Stone)
   ~ Holy Idol (2 Grass 1 Stone -> 1 Grass 1 Dirt 1 Stone)
   ~ Discerning Eye (1 Stone -> 2 Stone)
	~ Certain games of Subterranean tend to have an influx of Gold Cards with a drought of Stone Pieces in the Supply. These
	changes were put in place to encourage more Stone being sent to the Supply, give Stone Mines a further use for late game
	rounds, and allow placing Stone to be a more viable choice. Gold Card Effects should be a mid to late game tool, and their
	higher cost should encourage using them for other uses more often.

Bugs: 
- Fixed an issue where Garden and Flowers wouldn't allow players to place Pieces on the board and softlock the game.
- Fixed an issue where Garden and Flowers would think that there's no open spots on the board where Pieces could be placed.
- Stopped allowing players to move Pawns onto Pieces with newly added Pawns.
- Corrected an issue where the game would say Buildings have taken damage when they've actually taken none.
- Fixed Gold Pieces staying in an undiggable state.
- Fixed an issue where Planned Gamble would just discard one card.
- Fixed jittery behavior with some Card Discard animations.
- Mines no longer collect Pieces after being destroyed.
- Walkway no longer prompts the player to dig a Grass Piece at times. 
- Shovel no longer operates while in a player's hand. 
- Golden Shovel now correctly removes up to 4 Pieces.
- Fixed some issues with incorrect UI layering. 
- Fixed an issue where Dirty Thief wouldn't let you steal 1 Stone.
- Tornado now only lets you select Building Types if any of them are on the board.
- Fixed an issue where Secret Tunnels couldn't be discarded.
- Thunderstorm no longer softlocks the game if no Buildings are made. 
- Regeneration now scores the correct amount of points listed on the card.
- Fixed an issue where players could sometimes damage multiple buildings with one card.