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
			
		camera.transform.position = newPosition;

		// Ghost object placement
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		float distance;
		if (RAYCAST_PLANE.Raycast(ray, out distance)) {
			Vector3 point = ray.GetPoint(distance);
			Vector3 flooredPoint = new Vector3 (Mathf.Round(point.x), 0, Mathf.Round(point.z));

			if (ghost != null) {
				Object.Destroy (ghost);
			}
			ghost = (GameObject)Instantiate(Resources.Load("Prefabs/GhostCube"));
			ghost.transform.position = flooredPoint;

			Debug.Log ("Raw point: " + point);
			Debug.Log ("Floored point: " + flooredPoint);

			// Actually create the object
			if (Input.GetMouseButtonDown(0)) {
				GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/ConveyorStraight"));
				go.transform.position = flooredPoint;
			}
		}

	}
}
