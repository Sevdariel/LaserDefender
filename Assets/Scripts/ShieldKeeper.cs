using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldKeeper : MonoBehaviour 
{
	public static float health;
	private Text myText;

	void Start() 
	{
		myText = GetComponent<Text>();
		myText.text = "Shield: " + health.ToString();
	}
	public void ShowHealth(float hp)
	{
		health = hp;
		myText = GetComponent<Text>();
		myText.text = "Shield: " + health.ToString();
	}
	public static void Reset()
	{
		health = 0;
	}
}
