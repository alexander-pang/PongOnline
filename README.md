# PongOnline

This is our multiplayer version of pong. To play, you will need to open the Player1[Server] folder and launch Unity.exe. Then you will need to open Player2[Client] folder and launch Unity.exe on the same computer. One player will have to click host on the top left hand corner. The other player will have to click local. This is a basic version of pong. You should see scores increment on both screens and the ball will reset in the center after each score.

We tried to add a chat function for messages to be communicated between players. However, the chat function never got to work as the Network part of the script started to mess with the other Network scripts. So, we decided not to include this function. The script and prefabs for the chat function is in the ChatFuntion branch in our repository.

What we learned:
This project was heavy on trial and error as we slowly discovered how messages were being sent from server to clients. We figured out how to use syncVars and hooks in order to keep variable changes constant among host and client of the game.
