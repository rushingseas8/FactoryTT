using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlan {

	private static List<FloorPlan> floorPlans = new List<FloorPlan> ();

	private int x;
	private int z;

	private int width;
	private int height;

	public FloorPlan(float x, float z, float width, float height) {
		if (width < 0) {
			x = x + width;
			width = -width;
		}

		if (height < 0) {
			z = z + height;
			height = -height;
		}

		this.x = (int)x;
		this.z = (int)z;
		this.width = (int)width;
		this.height = (int)height;
	}

	/**
	 * Attempt to add a given floorplan. Will fail if there are any existing
	 * floorplans that overlap this one.
	 */
	public static bool add(FloorPlan plan) {
		Debug.Log ("Trying to add plan: x=" + plan.x + " z=" + plan.z + " w=" + plan.width + " h=" + plan.height);
		if (floorPlans.TrueForAll (p => !p.overlaps(plan))) {

			floorPlans.Add (plan);
			return true;
		} else {
			Debug.Log ("Failed to add floorplan.");
			return false;
		}
	}

	private bool overlaps(FloorPlan other) {
		int x2 = x + width;
		int z2 = z + height;

		int ox = other.x;
		int oz = other.z;
		int ox2 = other.x + other.width;
		int oz2 = other.z + other.height;

		return !(((x > ox2 || ox > x2) || (z > oz2 || oz > z2)));
	}

	public static bool add(float x, float z, float width, float height) {
		return add (new FloorPlan (x, z, width, height));
	}
}
