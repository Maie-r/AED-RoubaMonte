# AED-RoubaMonte
Simulação do jogo Rouba Monte em uma aplicação de console.

Ver History:
* 1.4
  * Optimized ShowAvailable to only one int value, since you can only have one choice.
  * Adjusted SetScore to use 0 index, shornening the code.
  * Cleaned up useless return values in GameProper
* 1.3
  * Fixed a major flaw in card stacking logic: Previously the drawn card was ignored when counting and stacking into player decks.
* 1.2
  * Optimized Discard area logic, using only a list element containing cards, instead of needing a new player and new deck instance for each new discard area card
  * Added smaller menu after a game, to show some other options such as showing the ranking of a specific player
* 1.1
  * Added user saving up to 5 last matches (user is the same of the name is identical)
* 1.0
  * Entire rest of the game made. Completed GameProper method, with adjustments to the other classes.

    ![RoubaMonte_qVm7fcdDqg](https://github.com/user-attachments/assets/ae48d374-7ee6-49a4-a26b-a33eda75d188)
    
* 0.1
  * Initial Build. Has the BuyDeck (Linear Structure), Deck (Flexible Structure), Jogador and Card Classes built. Further development will go on through the game logic implementation, which so far only allows you to go through the buy deck.

  ![RoubaMonte_BanAobh14m](https://github.com/user-attachments/assets/79c8765c-64cf-413c-82d8-cdb6db88174f)

