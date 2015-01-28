PlayerPrefs contains:

int max_level_unlocked -- the highest level currently unlocked by the user. Saved across AppOpen/Closes (henceforth called App Sessions)
int level_to_load -- Used in player movment to determine number of fish to be eaten
string game_mode -- used in player movement to determine if survival, growing, or other modes
int high score -- used in scoring mode, saved across app sessions.
int input mode -- used in player movment to determine how to get inputs. Saved across app sessions. 
int level_complete -- used in playermovement, and splash script. Determines if given game over splash or completion splash screens. If level completed, increases the max_unlocked_level


Note that when the user selects Quit from the main menu, it will save all changed values to playerprefs. This is an explicit declaration of the default behavior. Only save all variables when quitting, otherwise it will slow down the game. All items changed in playerprefs will be saved, however, unless stated above as saved across sessions, it does not matter what the value is.

