//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour 
{
	public GameObject[] enemyPrefab;
	private GameObject choosenEnemy;
	public float width, height;
	public float speed = 2f;
	public float spawnDelay = 0.5f;
	private bool movingRight = true;
	private float minX, maxX;
	
	void Start () 
	{
		CameraViewPort();
		ChooseEnemy();
		SpawnUntilFull();
	}
	void Update () 
	{
		EnemyMove();
		EnemyMoveSpaceRestriction();

		if (AllMembersDead())
		{
			SpawnUntilFull();
		}
	}
	void EnemyMove()
	{
		if (movingRight)
			transform.position += Vector3.right * speed * Time.deltaTime;
		else
			transform.position += Vector3.left * speed * Time.deltaTime;
	}
    private void EnemyMoveSpaceRestriction()
    {
        float rightEdgeOfFormation = transform.position.x + width/2;
	  	float leftEdgeOfFormation = transform.position.x - width/2;
		
		if (leftEdgeOfFormation < minX)
			movingRight = true;
		else if (rightEdgeOfFormation > maxX)
			movingRight = false;
    }
    public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position,
							new Vector3(width, height, 0));	
	}
	
	void CameraViewPort()
	{
		float distanceToCamera = transform.position.z -
								 Camera.main.transform.position.z;
		Vector3 leftEdge = Camera.main
						   .ViewportToWorldPoint(new Vector3(0f, 0f, 
							distanceToCamera));
		Vector3 rightEdge = Camera.main
							.ViewportToWorldPoint(new Vector3(1f, 0f, 
							distanceToCamera));
		minX = leftEdge.x;
		maxX = rightEdge.x;
	}
	bool AllMembersDead()
	{
		foreach (Transform childPositionGameObject in transform)
			if (childPositionGameObject.childCount > 0)
				return false;
		return true;	
	}
	void SpawnEnemies()
	{
		foreach (Transform child in transform)
		{
			GameObject enemy = Instantiate(choosenEnemy, 
										   child.transform.position, 
										   Quaternion.identity);
			enemy.transform.parent = child;
		}
	}

	void SpawnUntilFull()
	{
		Transform freePosition = NextFreePosition();
		if (freePosition)
		{
			ChooseEnemy();
			GameObject enemy = Instantiate(choosenEnemy, 
										freePosition.position, 
										Quaternion.identity);
			enemy.transform.parent = freePosition;
		}
		if (NextFreePosition())
			Invoke ("SpawnUntilFull", spawnDelay);
	}

	Transform NextFreePosition()
	{
		foreach (Transform childPositionGameObject in transform)
			if (childPositionGameObject.childCount == 0 )
				return childPositionGameObject;
		return null;
	}
	void ChooseEnemy()
	{
		int	number = Random.Range(0, 10001);
		number /= 100;
		number *= (int) Time.time;
		int color = 0;
		if (number < 5000)
			color = Random.Range(0, 4);
		else if (number >= 5000 && number < 8000)
			color = Random.Range(4, 8);
		else if (number >= 8000 && number < 9000)
			color = Random.Range(8, 12);
		else if (number >= 9000 && number < 9700)
			color = Random.Range(12, 16);
		else if (number >= 9700 && number < 10001)
			color = Random.Range(16, 20);
		
		choosenEnemy = enemyPrefab[color];
	}
}
