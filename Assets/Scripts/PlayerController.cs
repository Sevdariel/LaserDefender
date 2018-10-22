using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public GameObject[] projectileColor;
	private GameObject choosenProjectile;
	public GameObject shield;
	public LevelManager levelManager;
	public ScoreKeeper score;
	public HealthKeeper healthKeeper;
	public ShieldKeeper shieldKeeper;
	public AudioClip fireSound;
	private Shield moveShield;
	private float shieldPoints;
	private bool shieldFlag = false;
	private float additionalProjectileSpeed;
	private float additionalFiringRate;
	private float additionalDamage;
	public float speed = 3.5f;
	public float padding = 0.5f;
	public float projectileSpeed;
	public float firingRate = 0.2f;
	public float health;
	private float startTime, endTime;

	float minX, maxX;

	void Start()
	{
		score = GameObject.Find("Score").GetComponent<ScoreKeeper>();
		healthKeeper = FindObjectOfType<HealthKeeper>();
		shieldKeeper = FindObjectOfType<ShieldKeeper>();

		choosenProjectile = projectileColor[0];
		healthKeeper.ShowHealth(health);
		shieldKeeper.ShowHealth(shieldPoints);
		float distanceZ = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distanceZ));
		Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distanceZ));
		minX = leftMost.x + padding;
		maxX = rightMost.x - padding;
	}
	void Update () 
	{
		MoveWithKeyboard();
		ChooseProjectileColor();
		PlayerGameSpaceRestriction();
		Fire();
		if (shieldPoints > 0 && !shieldFlag)
			Shield();
		//shieldPoints = shield.GetComponent<Shield>().health;
		CheckTime();
	}
	void MoveWithKeyboard()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			float delta = Time.deltaTime;
			transform.position += Vector3.left * speed * delta;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			float delta = Time.deltaTime;
			transform.position += Vector3.right * speed * delta;
		}
	}
	void PlayerGameSpaceRestriction()
	{
		float newX = Mathf.Clamp(transform.position.x, minX, maxX);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	void CreateLaser()
	{
		GameObject laser = Instantiate(choosenProjectile, transform.position, Quaternion.identity);
		laser.GetComponent<Projectile>().SetDamage(additionalDamage);
		laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position);
	}
	void Fire()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			InvokeRepeating("CreateLaser", 0.00001f, firingRate);

		if (Input.GetKeyUp(KeyCode.Space))
			CancelInvoke("CreateLaser");
	}
	void OnTriggerEnter2D(Collider2D collider)
	{
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		if (missile)
		{
			health -= missile.GetDamage();
			missile.Hit();
			healthKeeper.ShowHealth(health);
			if (health <= 0)
				Die();
		}
		Pills pills = collider.gameObject.GetComponent<Pills>();
		if (pills)
		{
			health += pills.GetHealth();
			pills.Taken();
			healthKeeper.ShowHealth(health);
		}
		PowerUps powerUps = collider.gameObject.GetComponent<PowerUps>();
		if (powerUps)
		{ 
			startTime = Time.time;
			additionalDamage = 0;
			additionalFiringRate = 0;
			additionalProjectileSpeed = 0;
			additionalFiringRate += powerUps.GetFireRate();
			additionalDamage += powerUps.GetDamage();
			additionalProjectileSpeed += powerUps.GetProjectileSpeed();
			shieldPoints += powerUps.GetShield();
			powerUps.Taken();
			Debug.Log(startTime);
		}
	}
	void Die()
	{
		Destroy(gameObject);
		levelManager.LoadLevel("Lose Screen");
	}
	void ChooseProjectileColor()
	{
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			choosenProjectile = projectileColor[0];
			projectileSpeed = 10 + additionalProjectileSpeed;
			firingRate = 0.3f + additionalFiringRate;
		}
		else if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			choosenProjectile = projectileColor[1];
			projectileSpeed = 7.5f + additionalProjectileSpeed;
			firingRate = 0.4f + additionalFiringRate;
		}
		else if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			choosenProjectile = projectileColor[2];
			projectileSpeed = 10 + additionalProjectileSpeed;
			firingRate = 0.25f + additionalProjectileSpeed;
		}
	}
	void CheckTime()
	{
		endTime = Time.time;
		if (endTime - startTime >= 20)
		{
			additionalDamage = 0;
			additionalFiringRate = 0;
			additionalProjectileSpeed = 0;
			shieldPoints = 0;
			shield.GetComponent<Shield>().health = 0;
			shieldFlag = false;
		}
		if (shieldPoints <= 0)
			shieldFlag = false;
	}
	private void Shield()
	{
		Debug.Log("Im in shield func");
		GameObject newShield = Instantiate(shield, transform.position, Quaternion.identity);
		shield.GetComponent<Shield>().health = shieldPoints;
		shield.GetComponent<Transform>().position = transform.position;
		shieldFlag = true;
	}
}
