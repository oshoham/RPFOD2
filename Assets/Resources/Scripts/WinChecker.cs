using UnityEngine;

public class WinChecker : MonoBehaviour {

	// Is the number of robots a win condition?
	public static bool robotsWin = false;
	// The number of robots in the game.
	public static int numRobots = 0;
	// The number at which we win.
	public static int robotLimit = -1;

	// Is there a win square?
	public static bool squareWins = false;
	// The square at which we'll win;
	public static Vector2 winCoords;

	void Update() {
		bool win = true;
		if(robotsWin && numRobots > robotLimit) {
			win = false;
		}
		if(squareWins && GameManager.player.gridCoords != winCoords) {
			win = false;
		}
		if(win) {
			GameManager.Win();
		}
	}
}