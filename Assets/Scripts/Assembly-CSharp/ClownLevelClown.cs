using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000558 RID: 1368
public class ClownLevelClown : LevelProperties.Clown.Entity
{
	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06001987 RID: 6535 RVA: 0x000E7BFD File Offset: 0x000E5FFD
	// (set) Token: 0x06001988 RID: 6536 RVA: 0x000E7C05 File Offset: 0x000E6005
	public ClownLevelClown.State state { get; private set; }

	// Token: 0x06001989 RID: 6537 RVA: 0x000E7C0E File Offset: 0x000E600E
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600198A RID: 6538 RVA: 0x000E7C44 File Offset: 0x000E6044
	private void Start()
	{
		this.state = ClownLevelClown.State.BumperCar;
		this.notDashing = true;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600198B RID: 6539 RVA: 0x000E7C61 File Offset: 0x000E6061
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x0600198C RID: 6540 RVA: 0x000E7C6A File Offset: 0x000E606A
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x0600198D RID: 6541 RVA: 0x000E7C7D File Offset: 0x000E607D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.state != ClownLevelClown.State.Helium && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600198E RID: 6542 RVA: 0x000E7CA7 File Offset: 0x000E60A7
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x000E7CBF File Offset: 0x000E60BF
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x000E7CE0 File Offset: 0x000E60E0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.regularDuck = null;
		this.pinkDuck = null;
		this.bombDuck = null;
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x000E7D00 File Offset: 0x000E6100
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("clown_intro_continue");
		this.emitAudioFromObject.Add("clown_intro_continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_End", false, true);
		this.StartBumperCar();
		yield break;
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x000E7D1B File Offset: 0x000E611B
	public void StartBumperCar()
	{
		this.state = ClownLevelClown.State.BumperCar;
		base.animator.SetBool("BumperDeath", false);
		base.StartCoroutine(this.bumper_car_cr());
		base.StartCoroutine(this.ducks_cr());
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x000E7D4F File Offset: 0x000E614F
	public void EndBumperCar()
	{
		base.animator.SetBool("BumperDeath", true);
	}

	// Token: 0x06001994 RID: 6548 RVA: 0x000E7D62 File Offset: 0x000E6162
	private void SwitchLayer()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
		base.GetComponent<SpriteRenderer>().sortingOrder = 101;
	}

	// Token: 0x06001995 RID: 6549 RVA: 0x000E7D84 File Offset: 0x000E6184
	private IEnumerator end_bumper_car_cr()
	{
		AudioManager.Play("clown_bumper_death");
		this.emitAudioFromObject.Add("clown_bumper_death");
		while (base.transform.position.y > -660f)
		{
			if (CupheadTime.Delta != 0f)
			{
				base.transform.position += new Vector2(-300f, this.fallAccumulatedGravity) * CupheadTime.Delta;
				this.fallAccumulatedGravity += -100f;
			}
			yield return null;
		}
		this.clownHelium.StartHeliumTank();
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x000E7DA0 File Offset: 0x000E61A0
	private IEnumerator dash_timer_cr(string[] delayPattern)
	{
		float waitTime;
		Parser.FloatTryParse(delayPattern[this.timerIndex], out waitTime);
		yield return CupheadTime.WaitForSeconds(this, waitTime);
		this.notDashing = false;
		this.timerIndex = (this.timerIndex + 1) % delayPattern.Length;
		yield return null;
		yield break;
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x000E7DC4 File Offset: 0x000E61C4
	private IEnumerator bumper_car_cr()
	{
		this.notDashing = true;
		bool isFlipped = false;
		Vector3 bumperPos = base.transform.position;
		float offsetDash = 150f;
		float offsetMove = 250f;
		LevelProperties.Clown.BumperCar p = base.properties.CurrentState.bumperCar;
		string[] movementPattern = p.movementStrings.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] dashDelayPattern = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		float t = 0f;
		float speed = p.movementSpeed;
		int movementIndex = UnityEngine.Random.Range(0, movementPattern.Length);
		this.timerIndex = UnityEngine.Random.Range(0, dashDelayPattern.Length);
		base.StartCoroutine(this.dash_timer_cr(dashDelayPattern));
		this.emitAudioFromObject.Add("clown_bumper_move");
		this.emitAudioFromObject.Add("clown_dash_start");
		this.emitAudioFromObject.Add("clown_dash_end");
		for (;;)
		{
			if (this.notDashing)
			{
				if (movementPattern[movementIndex][0] == 'F')
				{
					base.animator.SetTrigger("Move");
					while (t < p.movementDuration && this.notDashing && !this.stop)
					{
						speed = ((!isFlipped) ? (-p.movementSpeed) : p.movementSpeed);
						base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
						t += CupheadTime.Delta;
						yield return null;
					}
					AudioManager.Play("clown_bumper_move");
					if (this.notDashing)
					{
						yield return CupheadTime.WaitForSeconds(this, p.movementDelay);
					}
				}
				else if (movementPattern[movementIndex][0] == 'B')
				{
					base.animator.SetTrigger("Move");
					while (t < p.movementDuration && this.notDashing && !this.stop)
					{
						speed = ((!isFlipped) ? p.movementSpeed : (-p.movementSpeed));
						if (base.transform.position.x >= (float)Level.Current.Left + offsetDash && base.transform.position.x <= (float)Level.Current.Right - offsetDash)
						{
							base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
							t += CupheadTime.Delta;
							yield return null;
						}
						yield return null;
					}
					AudioManager.Play("clown_bumper_move");
					if (this.notDashing)
					{
						yield return CupheadTime.WaitForSeconds(this, p.movementDelay);
					}
				}
				this.stop = false;
				t = 0f;
				movementIndex = (movementIndex + 1) % movementPattern.Length;
				yield return null;
			}
			else
			{
				float dist = 640f - base.transform.position.x;
				if (dist < 50f)
				{
					base.animator.Play("Move_Forward");
					while (t < p.movementDuration)
					{
						speed = ((!isFlipped) ? (-p.movementSpeed) : p.movementSpeed);
						base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
						t += CupheadTime.Delta;
						yield return null;
					}
				}
				AudioManager.Play("clown_dash_start");
				base.animator.Play("Dash_Start");
				yield return CupheadTime.WaitForSeconds(this, p.bumperDashWarning);
				base.animator.SetTrigger("Continue");
				float endPos = isFlipped ? ((float)Level.Current.Right - offsetMove) : ((float)Level.Current.Left + offsetMove);
				while (base.transform.position.x != endPos)
				{
					bumperPos.x = Mathf.MoveTowards(base.transform.position.x, endPos, p.dashSpeed * CupheadTime.Delta * this.hitPauseCoefficient());
					base.transform.position = bumperPos;
					yield return null;
				}
				AudioManager.Play("clown_dash_end");
				base.animator.SetTrigger("End");
				isFlipped = !isFlipped;
				yield return base.animator.WaitForAnimationToEnd(this, "Dash_End", false, true);
				this.notDashing = true;
				t = 0f;
				base.StartCoroutine(this.dash_timer_cr(dashDelayPattern));
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x000E7DE0 File Offset: 0x000E61E0
	private void FlipSprite()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x000E7E28 File Offset: 0x000E6228
	private void MoveAStop()
	{
		Vector2 v = base.transform.position;
		this.stop = true;
		v.y = this.forwardYPos.position.y;
		base.transform.position = v;
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x000E7E78 File Offset: 0x000E6278
	private void MoveBStop()
	{
		this.stop = true;
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x000E7E84 File Offset: 0x000E6284
	private void AnimationOffsetUp()
	{
		Vector2 v = base.transform.position;
		v.y = this.forwardYPos.position.y;
		base.transform.position = v;
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x000E7ED0 File Offset: 0x000E62D0
	private void SpawnDuck(ClownLevelDucks prefab, float startPercent)
	{
		if (prefab != null)
		{
			float num = 100f;
			LevelProperties.Clown.Duck duck = base.properties.CurrentState.duck;
			float maxYPos = duck.duckYHeightRange.RandomFloat();
			float num2 = startPercent / 100f * duck.duckYHeightRange.max;
			Vector2 pos = Vector3.zero;
			pos.y = 360f - num2;
			pos.x = 640f + num;
			ClownLevelDucks clownLevelDucks = UnityEngine.Object.Instantiate<ClownLevelDucks>(prefab).Init(pos, base.properties.CurrentState.duck, maxYPos, duck.duckYMovementSpeed);
			clownLevelDucks.Init(pos, base.properties.CurrentState.duck, maxYPos, duck.duckYMovementSpeed);
		}
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x000E7F90 File Offset: 0x000E6390
	private IEnumerator ducks_cr()
	{
		LevelProperties.Clown.Duck p = base.properties.CurrentState.duck;
		string[] positionPattern = p.duckYStartPercentString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] typePattern = p.duckTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int typeIndex = UnityEngine.Random.Range(0, typePattern.Length);
		int posPercentIndex = UnityEngine.Random.Range(0, positionPattern.Length);
		for (;;)
		{
			float spawnY = 0f;
			Parser.FloatTryParse(positionPattern[posPercentIndex], out spawnY);
			ClownLevelDucks toSpawn = null;
			if (typePattern[typeIndex][0] == 'R')
			{
				toSpawn = this.regularDuck;
			}
			else if (typePattern[typeIndex][0] == 'P')
			{
				toSpawn = this.pinkDuck;
			}
			else if (typePattern[typeIndex][0] == 'B')
			{
				toSpawn = this.bombDuck;
			}
			if (this.state != ClownLevelClown.State.Death)
			{
				this.SpawnDuck(toSpawn, spawnY);
			}
			yield return CupheadTime.WaitForSeconds(this, p.duckDelay);
			typeIndex = (typeIndex + 1) % typePattern.Length;
			posPercentIndex = (posPercentIndex + 1) % positionPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x040022A6 RID: 8870
	private const float FALL_GRAVITY = -100f;

	// Token: 0x040022A8 RID: 8872
	[SerializeField]
	private Transform forwardYPos;

	// Token: 0x040022A9 RID: 8873
	[SerializeField]
	private ClownLevelDucks regularDuck;

	// Token: 0x040022AA RID: 8874
	[SerializeField]
	private ClownLevelDucks pinkDuck;

	// Token: 0x040022AB RID: 8875
	[SerializeField]
	private ClownLevelDucks bombDuck;

	// Token: 0x040022AC RID: 8876
	[SerializeField]
	private ClownLevelClownHelium clownHelium;

	// Token: 0x040022AD RID: 8877
	private bool notDashing;

	// Token: 0x040022AE RID: 8878
	private bool firstSelection;

	// Token: 0x040022AF RID: 8879
	private bool stop;

	// Token: 0x040022B0 RID: 8880
	private float speed;

	// Token: 0x040022B1 RID: 8881
	private float fallAccumulatedGravity;

	// Token: 0x040022B2 RID: 8882
	private int timerIndex;

	// Token: 0x040022B3 RID: 8883
	private DamageDealer damageDealer;

	// Token: 0x040022B4 RID: 8884
	private DamageReceiver damageReceiver;

	// Token: 0x040022B5 RID: 8885
	private DamageReceiver damageReceiverChild;

	// Token: 0x040022B6 RID: 8886
	private Vector2 fallVelocity;

	// Token: 0x02000559 RID: 1369
	public enum State
	{
		// Token: 0x040022B8 RID: 8888
		BumperCar,
		// Token: 0x040022B9 RID: 8889
		Helium,
		// Token: 0x040022BA RID: 8890
		Death
	}
}
