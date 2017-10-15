using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	/**
	 * What kind of object is at this location? Conveyor belt; machine; etc.
	 */
	private int ID;

	/**
	 * How should this be displayed? E.g., if there is a similar object to the
	 * North and East, maybe we can tile a certain way.
	 */
	private int renderType;

	/**
	 * World-space coordinates.
	 */
	private int x;
	private int z;

	public Tile(int x, int z, int ID) {
		this.x = x;
		this.z = z;
		this.ID = ID;
	}
}
