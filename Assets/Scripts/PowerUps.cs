using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour 
{
	public  GameObject powerUp;
	public float damage;
	public float fireRate;
	public float projectileSpeed;
	public float shield;

	public void Taken()
	{
		Destroy(gameObject);
	}
	public float GetDamage()
	{
		return damage;
	}
	public float GetShield()
	{
		return shield;
	}
	public float GetFireRate()
	{
		return fireRate;
	}
	public float GetProjectileSpeed()
	{
		return projectileSpeed;
	}
}
