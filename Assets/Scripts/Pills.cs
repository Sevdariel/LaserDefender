using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pills : MonoBehaviour {
	public float health;

	void Start () {

	}		
	void Update () {
		
	}
	public void Taken()
	{
		Destroy(gameObject);
	}
	public float GetHealth()
	{
		return health;
	}
}
