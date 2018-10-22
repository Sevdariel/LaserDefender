using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
	public float damage;

	public void Hit()
	{
		Destroy(gameObject);
	}

	public float GetDamage()
	{
		return damage;
	}
	public void SetDamage(float dmg)
	{
		damage += dmg;
	}
}
