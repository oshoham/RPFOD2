using UnityEngine;
using System.Collections;
using System.Collections.Generic;

interface Level {
	void MakeWalls();
	void MakeBots();
	void MakeBelts();
	void MakeTraps();
	void MakePaint();
}
