using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	public float width = 5.6f;

	// Use this for initialization
	void Start()
	{
		Camera.main.orthographicSize = width * Screen.height / Screen.width * 0.5f;
	}
}