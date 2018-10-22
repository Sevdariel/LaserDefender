using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour 
{
	public float health;
	public ShieldKeeper shieldKeeper;
	void Start()
	{
		shieldKeeper = FindObjectOfType<ShieldKeeper>();
		shieldKeeper.ShowHealth(health);		
	}
	void Update()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			float delta = Time.deltaTime;
			transform.position += Vector3.left * 4.25f * delta;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			float delta = Time.deltaTime;
			transform.position += Vector3.right * 4.25f * delta;
		}		
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile)
		{
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0)
			{
				health = 0;	
				Hit();
			}
			shieldKeeper.ShowHealth(health);
		}	
	}
	public void Hit()
	{
		Destroy(gameObject);
	}
	public float GetHealth()
	{
		return health;
	}
	public void ChangePos(GameObject player)
	{
		this.transform.position = player.transform.position;
	}
}
