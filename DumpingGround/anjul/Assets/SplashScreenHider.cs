using UnityEngine;
using System.Collections;

public class SplashScreenHider : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Hide()
	{
		gameObject.SetActive (false);
	}

	public void Show()
	{
		gameObject.SetActive (true);
	}
}
