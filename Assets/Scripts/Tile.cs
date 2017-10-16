using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile {
	// A static list of all the Tile objects in the game.
	protected static List<Tile> gameTiles = new List<Tile>();

	// Used as constants for bitvector generation
	protected const int NONE = 0;
	protected const int NORTH = 1;
	protected const int SOUTH = 2;
	protected const int EAST = 4;
	protected const int WEST = 8;
	protected const int NORTHWEST = 16;
	protected const int NORTHEAST = 32;
	protected const int SOUTHWEST = 64;
	protected const int SOUTHEAST = 128;

	protected GameObject gameObject;

	/**
	 * What kind of object is at this location? Conveyor belt; machine; etc.
	 * Set this value only on creation of a base-level instance of Tile; 
	 * abstract classes used for structure shouldn't set an ID.
	 */
	protected int id;

	/**
	 * World-space coordinates.
	 */
	protected int x { get; set; }
	protected int z { get; set; }

	public Tile(int id) {
		this.id = id;
		this.gameObject = null;
	}

	/**
	 * Adds the requested tile at the provided (world) coordinates.
	 * 
	 * Sets the variables for the tile accordingly; simply pass in a new
	 * empty tile for this.
	 */
	public static void add(Tile tile, float x, float z) {
		if (get(x, z) == null) {
			tile.x = (int)x;
			tile.z = (int)z;
			gameTiles.Add (tile);

			tile.updateModel ();
			getAdjacent(x, z).ForEach(t => t.updateModel());
		}
	}

	/**
	 * Given a set of world coordinates, returns the tile there (if any)
	 */
	public static Tile get(float x, float z) {
		return gameTiles.Find (t => t.x == (int)x && t.z == (int)z);
	}

	/**
	 * Removes the requested tile at the provided (world) coordinates.
	 * 
	 * Handles removing the model and updating neighbors, as needed.
	 */
	public static void remove(float x, float z) {
		Tile toRemove = get (x, z);
		if (toRemove != null) {
			gameTiles.Remove (toRemove);
			Object.Destroy (toRemove.gameObject);
			getAdjacent (x, z).ForEach (t => t.updateModel ());
		}
	}

	/**
	 * Gets a list of adjacent tiles (in a 3x3 grid centered around the 
	 * provided world coordinates). Used for updating neighbors.
	 */
	public static List<Tile> getAdjacent(float x, float z) {
		return gameTiles.FindAll (t => Mathf.Abs (t.x - x) <= 1 && Mathf.Abs (t.z - z) <= 1);
	}

	/**
	 * Returns an integer representing which neighbors exist.
	 * 
	 * TODO: make a version of this method which checks if neighbors are
	 * equal type or a subtype of this tile's type.
	 */
	public int getBitvector() {
		int toReturn = 0;
		if (get (x, z + 1) != null) { // North
			toReturn += NORTH;
		}
		if (get (x, z - 1) != null) { // South
			toReturn += SOUTH;
		}
		if (get (x + 1, z) != null) { // East
			toReturn += EAST;
		}
		if (get (x - 1, z) != null) { // West
			toReturn += WEST;
		}
		// TODO: Add the other four directions
		return toReturn;
	}

	/**
	 * Returns the prefab we should use as the model for this object. This is
	 * based on factors such as surrounding tiles (if any), and the types.
	 */
	public abstract GameObject getModel ();

	/**
	 * Update this tile's model based on its surroundings.
	 */
	public void updateModel() {
		if (this.gameObject != null) {
			Object.Destroy (gameObject);
			gameObject = null;
		}

		this.gameObject = this.getModel();
		this.gameObject.transform.position = new Vector3 (this.x, 0, this.z);
		this.gameObject.transform.rotation = Quaternion.Euler (0, this.getRotation (), 0);
	}

	/**
	 * Returns the degrees by which this model should be rotated. This is based 
	 * on similar logic to getModel().
	 */
	public abstract int getRotation ();
}
