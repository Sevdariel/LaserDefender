using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour 
{
	public GameObject projectile;
	public GameObject[] powerUp;
	private GameObject choosenPowerUp;
	public ScoreKeeper score;
	public AudioClip fireSound;
	public AudioClip deathSound;
	public float projectileSpeed;
	public float health = 150;
	public float shotsPerSeconds = 0.5f;
	public int scoreValue = 150;

	void Start()
	{
		score = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}
	void Update()
	{
		float probability = Time.deltaTime * shotsPerSeconds;
		if (Random.value < probability)
			Fire();	
	}
	void Fire()
	{
		GameObject missile = Instantiate(projectile, transform.position, Quaternion.identity);
		missile.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile)
		{
			health -= missile.GetDamage();
			missile.Hit();
			if (health <= 0)
				Die();
			Debug.Log("Hit by a projectile");
			Debug.Log(missile.GetDamage());
		}
	}
	void Die()
	{
		SpawnPowerUp();
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		score.Score(scoreValue);
		Destroy(gameObject);
	}
	private void ChoosePowerUp()
	{
		int num = Random.Range(0,10001);
		int color = 0;

		if (num >= 5500 && num < 7750)
		{
			color = Random.Range(0, 3);
			if (num >= 7000  && num < 7750)
				color = 3;
		}
		else if (num >= 7750 && num <= 9000)
		{
			color = Random.Range(4, 7);
			if (num >= 8500 && num <= 9000)
				color = 7;
		}
		else if (num >= 9000 && num <= 9750)
		{
			color = Random.Range(8, 11);
			if (num >= 9500 && num <= 9750)
				color = 11;
		}
		else if (num >= 9750 && num <= 10000)
		{
			color = Random.Range(12, 15);
			if (num >= 9900 && num <= 10000)
				color = 15;
		}
		choosenPowerUp = powerUp[color];
	}
    private void SpawnPowerUp()
    {
		ChoosePowerUp();
        GameObject newPowerUp = Instantiate(choosenPowerUp, transform.position, Quaternion.identity);
		newPowerUp.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-2, 2), -2, 0);
    }
}
