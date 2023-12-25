using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000677 RID: 1655
public class FlyingGenieLevelPyramid : AbstractCollidableObject
{
	// Token: 0x060022DB RID: 8923 RVA: 0x00147310 File Offset: 0x00145710
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (GameObject gameObject in this.beams)
		{
			gameObject.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x00147365 File Offset: 0x00145765
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x00147383 File Offset: 0x00145783
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x0014739C File Offset: 0x0014579C
	public void Init(LevelProperties.FlyingGenie.Pyramids properties, Vector2 startPos, float startAngle, float speed, Transform pivot, int number, bool isClockWise)
	{
		base.transform.position = startPos;
		this.angle = startAngle;
		this.speed = speed;
		this.pivotPoint = pivot;
		this.number = number;
		this.properties = properties;
		this.isClockwise = isClockWise;
		base.StartCoroutine(this.path_cr());
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x001473F8 File Offset: 0x001457F8
	public IEnumerator beam_cr()
	{
		this.finishedATK = false;
		AudioManager.Play("genie_pyramid_attack");
		this.emitAudioFromObject.Add("genie_pyramid_attack");
		base.animator.SetTrigger("OnStartAttack");
		yield return base.animator.WaitForAnimationToEnd(this, "Open_Start", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.warningDuration);
		base.animator.SetTrigger("OnShoot");
		yield return base.animator.WaitForAnimationToEnd(this, "Shoot_Start", false, true);
		foreach (GameObject gameObject in this.beams)
		{
			gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.beamDuration);
		base.animator.SetTrigger("OnEnd");
		foreach (GameObject gameObject2 in this.beams)
		{
			gameObject2.GetComponent<Animator>().SetBool("IsAttacking", false);
		}
		this.finishedATK = true;
		yield break;
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x00147414 File Offset: 0x00145814
	private IEnumerator path_cr()
	{
		for (;;)
		{
			this.PathMovement();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x00147430 File Offset: 0x00145830
	private void PathMovement()
	{
		this.angle += this.speed * CupheadTime.Delta;
		Vector3 a;
		if (this.isClockwise)
		{
			a = new Vector3(Mathf.Sin(this.angle) * this.properties.pyramidLoopSize, 0f, 0f);
		}
		else
		{
			a = new Vector3(-Mathf.Sin(this.angle) * this.properties.pyramidLoopSize, 0f, 0f);
		}
		Vector3 b = new Vector3(0f, Mathf.Cos(this.angle) * this.properties.pyramidLoopSize, 0f);
		base.transform.position = this.pivotPoint.position;
		base.transform.position += a + b;
	}

	// Token: 0x04002B78 RID: 11128
	public int number;

	// Token: 0x04002B79 RID: 11129
	public bool finishedATK;

	// Token: 0x04002B7A RID: 11130
	[SerializeField]
	private GameObject[] beams;

	// Token: 0x04002B7B RID: 11131
	private LevelProperties.FlyingGenie.Pyramids properties;

	// Token: 0x04002B7C RID: 11132
	private DamageDealer damageDealer;

	// Token: 0x04002B7D RID: 11133
	private DamageReceiver damageReceiver;

	// Token: 0x04002B7E RID: 11134
	private Transform pivotPoint;

	// Token: 0x04002B7F RID: 11135
	private float speed;

	// Token: 0x04002B80 RID: 11136
	private float angle;

	// Token: 0x04002B81 RID: 11137
	private bool isClockwise;
}
