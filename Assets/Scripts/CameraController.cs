using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private const float BASE_MOVE_MULTIPLIER = 0.1f;
	private const float SCROLL_MULTIPLIER = 0.4f;

	private float zoomLevel;
	private const float MIN_ZOOM_LEVEL = 1.0f;
	private const float MAX_ZOOM_LEVEL = 32.0f;

	private Plane RAYCAST_PLANE = new Plane (Vector3.up, new Vector3(0, 0.5f, 0));

	private GameObject ghost = null;

	private Vector2 startDrag = new Vector2(-9999, -9999);

	void Start () {
		zoomLevel = 5.0f;
		this.gameObject.transform.position = new Vector3 (0, zoomLevel, 0);
		this.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (90, 0, 0));
	}

	void Update () {
		GameObject camera = this.gameObject;
		Vector3 oldPosition = camera.transform.position;
		Vector3 newPosition = oldPosition;

		float moveMultiplier = BASE_MOVE_MULTIPLIER * (1.0f + (Mathf.Log (zoomLevel) / Mathf.Log (3.0f)));

		// WASD movement
		if (Input.GetKey (KeyCode.W)) {
			newPosition += new Vector3 (0, 0, moveMultiplier);
		} 

		if (Input.GetKey (KeyCode.S)) {
			newPosition += new Vector3 (0, 0, -moveMultiplier);
		}  

		if (Input.GetKey (KeyCode.A)) {
			newPosition += new Vector3 (-moveMultiplier, 0, 0);
		}

		if (Input.GetKey (KeyCode.D)) {
			newPosition += new Vector3 (moveMultiplier, 0, 0);
		}

		// Zooming in and out
		float mouseWheel = -Input.GetAxis ("Mouse ScrollWheel") * SCROLL_MULTIPLIER;
		zoomLevel += mouseWheel;
		if (zoomLevel < MIN_ZOOM_LEVEL) {
			zoomLevel = MIN_ZOOM_LEVEL;
		} else if (zoomLevel > MAX_ZOOM_LEVEL) {
			zoomLevel = MAX_ZOOM_LEVEL;
		}
		newPosition.y = zoomLevel;
			
		// Move the camera to the new position.
		camera.transform.position = newPosition;

		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float distance;
		if (RAYCAST_PLANE.Raycast(ray, out distance)) {
			Vector3 point = ray.GetPoint(distance);
			int floorX = (int)Mathf.Round(point.x);
			int floorZ = (int)Mathf.Round(point.z);

			// Delete old ghost, if possible
			if (ghost != null) {
				Object.Destroy (ghost);
			}

			// Create new ghost object
			ghost = (GameObject)Instantiate(Resources.Load("Prefabs/GhostCube"));
			ghost.transform.position = new Vector3 (floorX, 0, floorZ);

			// Create new floorplan controls
			if (Input.GetMouseButtonDown (0)) {
				startDrag = new Vector2 (floorX, floorZ);
			}

			if (Input.GetMouseButton (0)) {
				if (startDrag.x != -9999 && startDrag.y != -9999) {
					Vector2 currDrag = new Vector2 (floorX, floorZ);

					int xScale = (int)(currDrag.x - startDrag.x);
					int zScale = (int)(currDrag.y - startDrag.y);
					ghost.transform.localScale = new Vector3 (
						xScale + (xScale > 0 ? 1 : -1),
						1,
						zScale + (zScale > 0 ? 1 : -1));
					ghost.transform.position = new Vector3 (floorX - (xScale * 0.5f), 0, floorZ - (zScale * 0.5f));
				}
			}

			if (Input.GetMouseButtonUp (0)) {
				if (startDrag.x != -9999 && startDrag.y != -9999) {
					Vector2 currDrag = new Vector2 (floorX, floorZ);

					if (FloorPlan.add (startDrag.x, startDrag.y, currDrag.x - startDrag.x, currDrag.y - startDrag.y)) {
						Object.Destroy (ghost);

						GameObject newFloor = (GameObject)Instantiate (Resources.Load ("Prefabs/FloorCube"));

						int xScale = (int)(currDrag.x - startDrag.x);
						int zScale = (int)(currDrag.y - startDrag.y);
						newFloor.transform.position = new Vector3 (floorX - (xScale * 0.5f), 0, floorZ - (zScale * 0.5f));
						newFloor.transform.localScale = new Vector3 (
							xScale + (xScale > 0 ? 1 : -1),
							1,
							zScale + (zScale > 0 ? 1 : -1));
						
						Material mat = new Material (Resources.Load<Material> ("Materials/Floor"));
						mat.color = new Color (Random.Range (0, 1.0f), Random.Range (0, 1.0f), Random.Range (0, 1.0f), 0.5f);
						newFloor.GetComponent<Renderer>().material = mat;
					}
				}
			}

			// Create/delete controls
			/*
			// Actually create the requested object
			if (Input.GetMouseButton(0)) {
				if (Tile.get(floorX, floorZ) == null) {
					Tile.add (new TileConveyor(), floorX, floorZ);
				}
			}

			// Delete the object (if present) on right click
			if (Input.GetMouseButton (1)) {
				if (Tile.get (floorX, floorZ) != null) {
					Tile.remove (floorX, floorZ);
				}
			}
			*/
		}

	}
}
