using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConveyor : Tile {

	// TODO: Abstract the conveyor-specific logic away for implementation of roads, etc.
	// Then make conveyor extend that abstract class with params. for models

	// Begin model variation definitions

	protected int[] straight = { 
		NONE, 
		NORTH,
		SOUTH,
		EAST,
		WEST,
		NORTH + SOUTH,
		EAST + WEST 
	};

	protected int[] turn = { 
		NORTH + EAST,
		NORTH + WEST,
		SOUTH + EAST,
		SOUTH + WEST 
	};

	protected int[] t = { 
		NORTH + EAST + WEST,
		SOUTH + EAST + WEST,
		EAST + NORTH + SOUTH,
		WEST + NORTH + SOUTH 
	};

	protected int[] cross = { 
		NORTH + SOUTH + EAST + WEST 
	};

	// Begin orientation definitions

	int[] rotate0 = { NONE, EAST, SOUTH + EAST, SOUTH + EAST + WEST, NORTH + SOUTH + EAST + WEST };
	int[] rotate90 = { NORTH, SOUTH, NORTH + SOUTH, SOUTH + WEST, NORTH + SOUTH + WEST };
	int[] rotate180 = { NORTH + WEST, NORTH + EAST + WEST };
	int[] rotate270 = { NORTH + EAST, NORTH + SOUTH + EAST };

	public TileConveyor() : base(0) {}

	public override GameObject getModel() {
		int bv = getBitvector ();

		GameObject conveyorStraight = Resources.Load<GameObject> ("Prefabs/ConveyorStraight");
		GameObject conveyorTurn = Resources.Load<GameObject> ("Prefabs/ConveyorTurn");
		GameObject conveyorT = Resources.Load<GameObject> ("Prefabs/ConveyorT");
		GameObject conveyorCross = Resources.Load<GameObject> ("Prefabs/ConveyorCross");

		if (System.Array.IndexOf (straight, bv) != -1) {
			return GameObject.Instantiate<GameObject> (conveyorStraight);
		} else if (System.Array.IndexOf (turn, bv) != -1) {
			return GameObject.Instantiate<GameObject> (conveyorTurn);
		} else if (System.Array.IndexOf (t, bv) != -1) {
			return GameObject.Instantiate<GameObject> (conveyorT);
		} else if (System.Array.IndexOf (cross, bv) != -1) {
			return GameObject.Instantiate<GameObject> (conveyorCross);
		} else {
			Debug.LogError ("Got unhandled bitvector: " + bv);
			return GameObject.Instantiate<GameObject> (conveyorStraight);
		}
	}

	public override int getRotation() {
		int bv = getBitvector ();

		if (System.Array.IndexOf (rotate0, bv) != -1) {
			return 0;
		} else if (System.Array.IndexOf (rotate90, bv) != -1) {
			return 90;
		} else if (System.Array.IndexOf (rotate180, bv) != -1) {
			return 180;
		} else if (System.Array.IndexOf (rotate270, bv) != -1) {
			return 270;
		} else {
			return 0;
		}
	}
}
