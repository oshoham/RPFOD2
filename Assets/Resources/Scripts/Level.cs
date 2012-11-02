using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface Level {

	// public Level(){}

	// public void L1()
	// {
	// 	L1 lev = new L1();
	// }

	void MakeWalls();
	void MakeBots();
	void MakeBelts();
	void MakeTraps();
	void MakePaint();
}
