using System;
using UnityEngine;

public class SallyStagePlayLevelBackgroundHandler : AbstractPausableComponent
{
	[Serializable]
	public class Cupid
	{
		public Transform cupidTransform;
		public Vector3 startPosition;
		public bool acceptableLevel;
		public bool playSound;
	}

	[SerializeField]
	private SallyStagePlayLevelSally sally;
	[SerializeField]
	private Transform curtain;
	[SerializeField]
	private Transform curtainSprite;
	[SerializeField]
	private Transform curtainShadow;
	[SerializeField]
	private Transform curtainUpRoot;
	[SerializeField]
	private SpriteRenderer[] flickeringLights;
	[SerializeField]
	private SallyStagePlayApplauseHandler applauseHandler;
	[SerializeField]
	private Transform[] churchSwingies;
	[SerializeField]
	private Cupid[] cupids;
	[SerializeField]
	private Animator priest;
	[SerializeField]
	private Animator husband;
	[SerializeField]
	private Animator sallyBackground;
	[SerializeField]
	private Transform car;
	[SerializeField]
	private Transform carRoot;
	[SerializeField]
	private Transform chandelier;
	[SerializeField]
	private Transform sallyRoot;
	[SerializeField]
	private SallyStagePlayLevelHouse residence;
	[SerializeField]
	private Animator husbandPhase2;
	[SerializeField]
	private Transform[] husbandRoots;
	[SerializeField]
	private Animator priestPhase2;
	[SerializeField]
	private Transform[] priestRoots;
	[SerializeField]
	private Transform[] purgSwingies;
	[SerializeField]
	private Transform[] finaleSwingies;
	[SerializeField]
	private GameObject husbandAliveObject;
	[SerializeField]
	private GameObject husbandDeadObject;
	[SerializeField]
	private GameObject[] backgrounds;
}
