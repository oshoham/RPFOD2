using UnityEngine;

public class WinChecker : MonoBehaviour {

	// Is the number of robots a win condition?
	public static bool robotsWin;
	// The number of robots in the game.
	public static int numRobots;
	// The number at which we win.
	public static int robotLimit;

	// Is there a win square?
	public static bool squareWins;
	// The square at which we'll win;
	public static Vector2 winCoords;
	
	void Update() {
		if(robotsWin && squareWins &&
		   numRobots <= robotLimit &&
		   GameManager.player.gridCoords == winCoords) {
			print(GameManager.player.gridCoords);
			GameManager.Win();
		}
		else if(robotsWin && numRobots <= robotLimit &&
			!squareWins) {
			GameManager.Win();
		}
		else if(squareWins && GameManager.player.gridCoords == winCoords &&
			!robotsWin) {
			GameManager.Win();
		}
	}
}