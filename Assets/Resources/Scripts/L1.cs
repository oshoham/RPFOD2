using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class L1 : Level {


	public static Grid floor = GameManager.floor;
	
	public L1() { MakeLev(); } 

	public void MakeLev()
	{
		MakeWalls();
		MakeBots();
		MakeBelts();
		MakeTraps();
		MakePaint();
	}

	public void MakeWalls() {
		Color col = Color.black;
		/*
		 * West Wall
		 */
	
		Wall.MakeWall(0, 7, 1, false, col); 	
		Wall.MakeWall(0, 9, 1, false, col); 	
		Wall.MakeWall(1, 7, 1, false, col); 	
		Wall.MakeWall(1, 9, 1, false, col);
 		
		WLoops(2, 0, 8, 1);	
		WLoops(2, 9, 16, 1);

		// Inserts
		Wall.MakeWall(3, 5, 1, false, col);
		Wall.MakeWall(4, 5, 1, false, col);
		Wall.MakeWall(3, 11, 1, false, col);
		Wall.MakeWall(4, 11, 1, false, col);	
	
	
		/*
		 * North Wall
		 */	
		WLoops(3, 15, 19, 0);
		WLoops(19, 17, 28, 0);
		WLoops(19, 13, 18, 1);
		
		// Inserts
		Wall.MakeWall(8, 14, 1, false, col);	
		Wall.MakeWall(8, 13, 1, false, col);	
		
	
		/*
		 * South Wall
		 */ 	
		WLoops(3, 0, 20, 0);
		WLoops(19, 1, 28, 0);
	
		// Inserts	
		SpikeWall.MakeSpikeWall(7, 1, 1, false, new List<Vector2>() { new Vector2(1, 0) }, col);	
		Wall.MakeWall(19, 1, 1, false, col);	
		
	
		/* 
		 * East Wall
		 */
		WLoops(27, 2, 6, 1);
		WLoops(26, 5, 29, 0);
		WLoops(28, 6, 11, 1);
		WLoops(26, 11, 29, 0);
		WLoops(27, 12, 17, 1); 
	}


	public void MakeBots() {
	
		/*
		 * Robots that only go forward
		 */	
		Robot.MakeRobot(x: 9, y: 1, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 4, sideRange: 4, movementDirection: new Vector2(0, 1),
				colorVisible: Color.red, turnsLeft: false);
		
		Robot.MakeRobot(x: 13 , y: 1, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 4, sideRange: 4, movementDirection: new Vector2(0, 1),
				colorVisible: Color.red, turnsLeft: false);
		Robot.MakeRobot(x: 17, y: 1, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 4, sideRange: 4, movementDirection: new Vector2(0, 1),
				colorVisible: Color.red, turnsLeft: false);
		Robot.MakeRobot(x: 11, y: 14, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 4, sideRange: 4, movementDirection: new Vector2(0, 1),
				colorVisible: Color.red, turnsLeft: false);
		Robot.MakeRobot(x: 15, y: 14, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 4, sideRange: 4, movementDirection: new Vector2(0, 1),
				colorVisible: Color.red, turnsLeft: false);

		/*
		 * Left bots
		 */		
		Robot.MakeRobot(x: 7 , y: 13, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 3, sideRange: 3, movementDirection: new Vector2(0, 1),
				colorVisible: Color.blue, turnsLeft: true);
		Robot.MakeRobot(x: 4, y: 4, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 3, sideRange: 3, movementDirection: new Vector2(-1, 0),
				colorVisible: Color.blue, turnsLeft: true);
		Robot.MakeRobot(x: 20, y: 2, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 3, sideRange: 3, movementDirection: new Vector2(1, 0),
				colorVisible: Color.blue, turnsLeft: true);
		Robot.MakeRobot(x: 26, y: 12, speed: 0.5f, damage: 2, health: 10,
				forwardRange: 3, sideRange: 3, movementDirection: new Vector2(0, 1),
				colorVisible: Color.blue, turnsLeft: true);


	
	}
	public void MakeBelts() {
		Conveyor.MakeConveyor(new Vector2(0, 8), new Vector2(1, 0), 5, 0.005f);
		Conveyor.MakeConveyor(new Vector2(7, 8), new Vector2(1, 0), 2, 0.005f);
		for(int i = 0; i < 8; i+=2)
			Conveyor.MakeConveyor(new Vector2(10+i, 8), new Vector2(1, 0), 1, 0.005f);
		Conveyor.MakeConveyor(new Vector2(18, 8), new Vector2(1, 0), 2, 0.005f);
		Conveyor.MakeConveyor(new Vector2(21, 8), new Vector2(1, 0), 5, 0.005f);
		Conveyor.MakeConveyor(new Vector2(26, 8), new Vector2(1, 0), 1, 0.005f, true, 3);

	
		Conveyor.MakeConveyor(new Vector2(4, 10), new Vector2(-1, 0), 1, 0.005f);
		Conveyor.MakeConveyor(new Vector2(8, 10), new Vector2(-1, 0), 2, 0.005f);
		for(int i = 0; i < 8; i+=2)
			Conveyor.MakeConveyor(new Vector2(10+i, 10), new Vector2(-1, 0), 1, 0.005f);
		Conveyor.MakeConveyor(new Vector2(19, 10), new Vector2(-1, 0), 2, 0.005f);
		Conveyor.MakeConveyor(new Vector2(26, 10), new Vector2(-1, 0), 6, 0.005f);
		
		
		Conveyor.MakeConveyor(new Vector2(4, 6), new Vector2(-1, 0), 1, 0.005f);
		Conveyor.MakeConveyor(new Vector2(8, 6), new Vector2(-1, 0), 2, 0.005f);
		for(int i = 0; i < 8; i+=2)
			Conveyor.MakeConveyor(new Vector2(10+i, 6), new Vector2(-1, 0), 1, 0.005f);
		Conveyor.MakeConveyor(new Vector2(19, 6), new Vector2(-1, 0), 2, 0.005f);
		Conveyor.MakeConveyor(new Vector2(26, 6), new Vector2(-1, 0), 6, 0.005f);

		
		Conveyor.MakeConveyor(new Vector2(3, 10), new Vector2(0, -1), 2, 0.005f);
		Conveyor.MakeConveyor(new Vector2(3, 6), new Vector2(0, 1), 2, 0.005f);
	}


	public void MakeTraps() {}
	public void MakePaint() {	
	//public static GameObject MakePaint(int x, int y, Color color, float respawnTime) {
		Paint.MakePaint( 4, 9, Color.blue, 5);
		Paint.MakePaint( 4, 7, Color.blue, 5);
		Paint.MakePaint( 3, 14, Color.blue, 5);
		Paint.MakePaint( 3, 1, Color.blue, 5);
		Paint.MakePaint( 26, 2, Color.blue, 5);
		Paint.MakePaint( 26, 16, Color.blue, 5);

		Paint.MakePaint( 4, 14, Color.red, 5);
		Paint.MakePaint( 3, 13, Color.red, 5);
		Paint.MakePaint( 4, 1, Color.red, 5);
		Paint.MakePaint( 3, 2, Color.red, 5);
		Paint.MakePaint( 25, 2, Color.red, 5);
		Paint.MakePaint( 26, 3, Color.red, 5);
		Paint.MakePaint( 25, 16, Color.red, 5);
		Paint.MakePaint( 26, 15, Color.red, 5);
	}

	public void WLoops(int x, int y, int end, int xy)
	{
		Color col = Color.black;
		if(xy == 0)		
			for(; x < end; x++)
				Wall.MakeWall(x, y, 1, false, col); 	
		else if(xy == 1)
			for(; y < end; y++)
				Wall.MakeWall(x, y, 1, false, col); 
		else	
			for(; x < end || y < end; x++, y++)
				Wall.MakeWall(x, y, 1, false, col); 	
	}
} 
