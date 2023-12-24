using UnityEngine;

public class BeeLevelQueen : LevelProperties.Bee.Entity
{
	[SerializeField]
	private BeeLevelAirplane airplane;
	[SerializeField]
	private Transform bottomHoney;
	[SerializeField]
	private Effect puff;
	[SerializeField]
	private Transform[] puffRoots;
	[SerializeField]
	private GameObject head;
	[SerializeField]
	private GameObject body;
	[SerializeField]
	private GameObject chain;
	[SerializeField]
	private BeeLevelQueenSpitProjectile spitPrefab;
	[SerializeField]
	private Transform spitRoot;
	[SerializeField]
	private BeeLevelQueenBlackHole blackHolePrefab;
	[SerializeField]
	private Transform[] blackHoleRoots;
	[SerializeField]
	private BeeLevelQueenTriangle trianglePrefab;
	[SerializeField]
	private BeeLevelQueenTriangle triangleInvinciblePrefab;
	[SerializeField]
	private float followerRadius;
	[SerializeField]
	private Transform followerRoot;
	[SerializeField]
	private BeeLevelQueenFollower followerPrefab;
	[SerializeField]
	private Effect dustEffect;
	[SerializeField]
	private Effect sparkEffect;
}
