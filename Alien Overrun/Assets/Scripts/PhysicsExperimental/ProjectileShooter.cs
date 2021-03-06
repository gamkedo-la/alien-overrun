﻿/**
 * Description: Shooter component of Towers - shoots arrows.
 * Authors: Kornel
 * Copyright: © 2019 Kornel. All rights reserved. For license see: 'LICENSE.txt'
 **/

using UnityEngine;
using UnityEngine.Assertions;

public class ProjectileShooter : MonoBehaviour
{
	[SerializeField] private Transform spawnPoint = null;
	[SerializeField] private Transform spawnPointPivot = null;
	[SerializeField] private GameObject projectile = null;
	[Space]
	[SerializeField] private float reloadTime = 2f;
	//[SerializeField] private int ammoCost = 0;
	[Space]
	[SerializeField] private float shootingForce = 3.7f;
	//[SerializeField] private float upAngleCorrection = -90f;
	//[SerializeField] private float arrowTimePerUnit = 0.09f;
	[SerializeField] private float verticalSpread = 5f;
	[SerializeField] private float horizontalSpread = 5f;

	private float timeToNextShot = 0f;
	private Transform target = null;

	void Start()
	{
		Assert.IsNotNull(projectile);
		Assert.IsNotNull(spawnPoint);
		Assert.IsNotNull(spawnPointPivot);
	}

	void Update()
	{
		timeToNextShot -= Time.deltaTime;

		TryToShoot();
	}

	public void OnNewOpponent(Transform oponent)
	{
		target = oponent;
	}

	public void OnOpponentLost()
	{
		target = null;
	}

	private void TryToShoot()
	{
		if (target && timeToNextShot <= 0)// && HasAmmo())
		{
			timeToNextShot = reloadTime;
			Shoot();
		}
	}

	/*
	private bool HasAmmo()
	{
		return ResourceManager.Instance.CheckResources(ResourceType.Minerals, ammoCost);
	}
	*/

	private void Shoot()
	{
		if (!target)
		{
			Debug.LogError("Tried shooting but enemy = null");
			return;
		}

		//ResourceManager.Instance.UseResources(ResourceType.Minerals, ammoCost);

		Vector3 standardTower = transform.parent.position;
		standardTower.y = 0;
		Vector3 standardEnemy = target.position;
		standardEnemy.y = 0;

		Vector3 dir = (target.position - spawnPoint.position).normalized;

		float angle = Utilities.AngleBetweenVectors(new Vector2(transform.parent.position.x, transform.parent.position.z), new Vector2(target.transform.position.x, target.transform.position.z));
		spawnPointPivot.rotation = Quaternion.Euler(0, -angle + 90, 0);

		// Some randomness
		dir = Quaternion.AngleAxis(Random.Range(-horizontalSpread, horizontalSpread), Vector3.Cross(Vector3.down, dir)) * dir;
		dir = Quaternion.AngleAxis(Random.Range(-verticalSpread, verticalSpread), Vector3.Cross(Vector3.right, dir)) * dir;

		Vector3 force = dir * (shootingForce);
		var go = Instantiate(projectile, spawnPoint.position, Quaternion.identity);
		go.GetComponent<Rigidbody>().AddForce( force * PauseGame.ForceScale );
	}

	private void OnTriggerStay(Collider other)
	{
		if (target == null && other.gameObject.tag == "Enemy")
			target = other.gameObject.transform;
	}

	private void OnTriggerExit(Collider other)
	{
		if (target != null && other.gameObject.tag == "Enemy")
			target = null;
	}
}
