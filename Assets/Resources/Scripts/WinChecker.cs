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
		if(robotsWin && squareWins &&
		   numRobots <= robotLimit &&
		   GameManager.player.gridCoords == winCoords) {
			GameManager.Win();
		}
		if(robotsWin && numRobots <= robotLimit) {
			GameManager.Win();
		}
		if(squareWins && GameManager.player.gridCoords == winCoords) {
			GameManager.Win();
		}
	}
}