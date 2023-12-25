using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000500 RID: 1280
public class BatLevelBat : LevelProperties.Bat.Entity
{
	// Token: 0x17000325 RID: 805
	// (get) Token: 0x0600168B RID: 5771 RVA: 0x000CA9B1 File Offset: 0x000C8DB1
	// (set) Token: 0x0600168C RID: 5772 RVA: 0x000CA9B9 File Offset: 0x000C8DB9
	public BatLevelBat.State state { get; private set; }

	// Token: 0x0600168D RID: 5773 RVA: 0x000CA9C4 File Offset: 0x000C8DC4
	public override void LevelInit(LevelProperties.Bat properties)
	{
		base.LevelInit(properties);
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.speed = properties.CurrentState.movement.movementSpeed;
		this.originalSpeed = this.speed;
		this.inMovingPhase = true;
		this.moving = true;
		this.startPosition = base.transform.position;
		this.startPosition.y = properties.CurrentState.movement.startPosY;
		base.transform.position = this.startPosition;
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Left, base.transform);
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600168E RID: 5774 RVA: 0x000CAA94 File Offset: 0x000C8E94
	private IEnumerator intro_cr()
	{
		LevelProperties.Bat.State p = base.properties.CurrentState;
		this.anglePattern = p.batBouncer.bounceAngleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.angleIndex = UnityEngine.Random.Range(0, this.anglePattern.Length);
		yield return CupheadTime.WaitForSeconds(this, 5f);
		base.animator.SetTrigger("OnIntro");
		base.StartCoroutine(this.bat_movement_cr());
		this.state = BatLevelBat.State.Idle;
		yield break;
	}

	// Token: 0x0600168F RID: 5775 RVA: 0x000CAAAF File Offset: 0x000C8EAF
	public void Die()
	{
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001690 RID: 5776 RVA: 0x000CAAC1 File Offset: 0x000C8EC1
	private void Update()
	{
		this.damageDealer.Update();
		if (this.state != BatLevelBat.State.Phase2)
		{
			this.VaryingSpeed();
		}
	}

	// Token: 0x06001691 RID: 5777 RVA: 0x000CAAE0 File Offset: 0x000C8EE0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001692 RID: 5778 RVA: 0x000CAAF7 File Offset: 0x000C8EF7
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001693 RID: 5779 RVA: 0x000CAB0C File Offset: 0x000C8F0C
	private void OnTurnAnimComplete()
	{
		base.transform.SetScale(new float?(base.transform.localScale.x * -1f), null, null);
	}

	// Token: 0x06001694 RID: 5780 RVA: 0x000CAB54 File Offset: 0x000C8F54
	private IEnumerator bat_movement_cr()
	{
		float offset = 200f;
		float stopDist = 100f;
		Vector3 pos = base.transform.position;
		for (;;)
		{
			if (this.direction == BatLevelBat.Direction.Left)
			{
				while (base.transform.position.x > -640f + offset)
				{
					if (this.moving)
					{
						float num = -640f + offset - base.transform.position.x;
						num = Mathf.Abs(num);
						pos.x = Mathf.MoveTowards(base.transform.position.x, -640f + offset, this.speed * CupheadTime.Delta);
						if (num < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				if (!this.inMovingPhase)
				{
					break;
				}
				this.direction = BatLevelBat.Direction.Right;
				yield return null;
			}
			else if (this.direction == BatLevelBat.Direction.Right)
			{
				while (base.transform.position.x < 640f - offset)
				{
					if (this.moving)
					{
						float num2 = 640f - offset - base.transform.position.x;
						num2 = Mathf.Abs(num2);
						pos.x = Mathf.MoveTowards(base.transform.position.x, 640f - offset, this.speed * CupheadTime.Delta);
						if (num2 < stopDist)
						{
							this.slowDown = true;
						}
						base.transform.position = pos;
					}
					yield return null;
				}
				base.animator.SetTrigger("OnTurn");
				if (!this.inMovingPhase)
				{
					goto IL_336;
				}
				this.direction = BatLevelBat.Direction.Left;
				yield return null;
			}
			yield return null;
		}
		this.onRight = false;
		base.StartCoroutine(this.phase_2_handler_cr());
		goto IL_399;
		IL_336:
		this.onRight = true;
		base.StartCoroutine(this.phase_2_handler_cr());
		IL_399:
		yield break;
	}

	// Token: 0x06001695 RID: 5781 RVA: 0x000CAB70 File Offset: 0x000C8F70
	private void VaryingSpeed()
	{
		float num = 10f;
		if (this.slowDown)
		{
			if (this.speed <= 50f)
			{
				this.slowDown = false;
			}
			else
			{
				this.speed -= num;
			}
		}
		else if (this.speed < this.originalSpeed)
		{
			this.speed += num;
		}
	}

	// Token: 0x06001696 RID: 5782 RVA: 0x000CABDC File Offset: 0x000C8FDC
	public void StartBouncer()
	{
		if (this.pattern != null)
		{
			base.StopCoroutine(this.pattern);
		}
		this.pattern = base.StartCoroutine(this.bouncer_cr());
	}

	// Token: 0x06001697 RID: 5783 RVA: 0x000CAC08 File Offset: 0x000C9008
	private void SpawnBouncer()
	{
		float num = 0f;
		Parser.FloatTryParse(this.anglePattern[this.angleIndex], out num);
		num = ((this.direction != BatLevelBat.Direction.Right) ? num : (num + 90f));
		BatLevelBouncer batLevelBouncer = UnityEngine.Object.Instantiate<BatLevelBouncer>(this.bouncerPrefab);
		batLevelBouncer.Init(base.properties.CurrentState.batBouncer, this.bouncerRoot.position, num);
		this.angleIndex = (this.angleIndex + 1) % this.anglePattern.Length;
	}

	// Token: 0x06001698 RID: 5784 RVA: 0x000CAC94 File Offset: 0x000C9094
	private IEnumerator bouncer_cr()
	{
		LevelProperties.Bat.BatBouncer p = base.properties.CurrentState.batBouncer;
		this.state = BatLevelBat.State.Bouncer;
		this.moving = false;
		yield return CupheadTime.WaitForSeconds(this, p.stopDelay);
		this.SpawnBouncer();
		this.moving = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = BatLevelBat.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001699 RID: 5785 RVA: 0x000CACAF File Offset: 0x000C90AF
	public void StartGoblin()
	{
		base.StartCoroutine(this.goblin_cr());
	}

	// Token: 0x0600169A RID: 5786 RVA: 0x000CACC0 File Offset: 0x000C90C0
	private IEnumerator goblin_cr()
	{
		LevelProperties.Bat.Goblins p = base.properties.CurrentState.goblins;
		string[] delayPattern = p.appearDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] entrancePattern = p.entranceString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayPattern.Length);
		int entranceIndex = UnityEngine.Random.Range(0, entrancePattern.Length);
		int counter = 0;
		float delay = 0f;
		float startX = 0f;
		float pickShooter = (float)p.shooterOccuranceRange.RandomInt();
		float startY = (float)Level.Current.Ground + 100f;
		bool isShooter = false;
		for (;;)
		{
			Parser.FloatTryParse(delayPattern[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			if (entrancePattern[entranceIndex][0] == 'R')
			{
				startX = 640f;
				Vector2 startPos = new Vector2(startX, startY);
				this.SpawnGoblin(false, startPos, isShooter);
			}
			else if (entrancePattern[entranceIndex][0] == 'L')
			{
				startX = -640f;
				Vector2 startPos = new Vector2(startX, startY);
				this.SpawnGoblin(true, startPos, isShooter);
			}
			isShooter = false;
			counter++;
			if ((float)counter == pickShooter)
			{
				isShooter = true;
				counter = 0;
			}
			entranceIndex = (entranceIndex + 1) % entrancePattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600169B RID: 5787 RVA: 0x000CACDC File Offset: 0x000C90DC
	private void SpawnGoblin(bool leftSide, Vector2 startPos, bool isShooter)
	{
		LevelProperties.Bat.Goblins goblins = base.properties.CurrentState.goblins;
		BatLevelGoblin batLevelGoblin = UnityEngine.Object.Instantiate<BatLevelGoblin>(this.goblinPrefab);
		batLevelGoblin.Init(goblins, startPos, leftSide, isShooter, goblins.HP);
	}

	// Token: 0x0600169C RID: 5788 RVA: 0x000CAD16 File Offset: 0x000C9116
	public void StartLightning()
	{
		if (this.pattern != null)
		{
			base.StopCoroutine(this.pattern);
		}
		this.pattern = base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x0600169D RID: 5789 RVA: 0x000CAD41 File Offset: 0x000C9141
	private void SpawnCloud(Vector2 startPos)
	{
		this.lightning = UnityEngine.Object.Instantiate<BatLevelLightning>(this.lightningPrefab);
		this.lightning.Init(base.properties.CurrentState.batLightning, startPos);
	}

	// Token: 0x0600169E RID: 5790 RVA: 0x000CAD70 File Offset: 0x000C9170
	private IEnumerator lightning_cr()
	{
		this.state = BatLevelBat.State.Lightning;
		this.moving = false;
		LevelProperties.Bat.BatLightning p = base.properties.CurrentState.batLightning;
		string[] offsetString = p.centerOffset.GetRandom<string>().Split(new char[]
		{
			','
		});
		int offsetIndex = 0;
		float offset = 0f;
		Vector2 pos = Vector2.zero;
		pos.y = p.cloudHeight;
		int num = 0;
		while ((float)num < p.cloudCount)
		{
			Parser.FloatTryParse(offsetString[offsetIndex], out offset);
			pos.x = p.cloudDistance * (float)num + offset - (float)(Level.Current.Right / 2);
			this.SpawnCloud(pos);
			offsetIndex %= offsetString.Length;
			num++;
		}
		while (this.lightning != null)
		{
			yield return null;
		}
		this.moving = true;
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = BatLevelBat.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600169F RID: 5791 RVA: 0x000CAD8B File Offset: 0x000C918B
	public void StartPhase2()
	{
		this.inMovingPhase = false;
	}

	// Token: 0x060016A0 RID: 5792 RVA: 0x000CAD94 File Offset: 0x000C9194
	private IEnumerator phase_2_handler_cr()
	{
		float yPos = base.transform.position.y - 100f;
		this.state = BatLevelBat.State.Phase2;
		this.speed = this.originalSpeed;
		while (base.transform.position.y != yPos)
		{
			Vector3 pos = base.transform.position;
			pos.y = Mathf.MoveTowards(base.transform.position.y, yPos, this.speed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		this.StartMiniBats();
		this.StartPentagram();
		this.StartCross();
		yield return null;
		yield break;
	}

	// Token: 0x060016A1 RID: 5793 RVA: 0x000CADAF File Offset: 0x000C91AF
	public void StartMiniBats()
	{
		base.StartCoroutine(this.mini_bats_cr());
	}

	// Token: 0x060016A2 RID: 5794 RVA: 0x000CADC0 File Offset: 0x000C91C0
	private void SpawnMiniBat(float angle)
	{
		LevelProperties.Bat.MiniBats miniBats = base.properties.CurrentState.miniBats;
		float rotation = (!this.onRight) ? angle : (-angle);
		float velocity = (!this.onRight) ? miniBats.speedX : (-miniBats.speedX);
		this.minibatPrefab.Create(this.coffinRoot.position, rotation, velocity, miniBats.speedY, miniBats.yMinMax, miniBats.HP);
	}

	// Token: 0x060016A3 RID: 5795 RVA: 0x000CAE4C File Offset: 0x000C924C
	private IEnumerator mini_bats_cr()
	{
		LevelProperties.Bat.MiniBats p = base.properties.CurrentState.miniBats;
		string[] angleString = p.batAngleString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = UnityEngine.Random.Range(0, angleString.Length);
		float angle = 0f;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			Parser.FloatTryParse(angleString[angleIndex], out angle);
			this.SpawnMiniBat(angle);
			yield return CupheadTime.WaitForSeconds(this, p.delay);
			angleIndex = (angleIndex + 1) % angleString.Length;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016A4 RID: 5796 RVA: 0x000CAE67 File Offset: 0x000C9267
	public void StartPentagram()
	{
		base.StartCoroutine(this.pentagram_cr());
	}

	// Token: 0x060016A5 RID: 5797 RVA: 0x000CAE78 File Offset: 0x000C9278
	private void SpawnPentagram()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 position = this.pentagramRoot.position;
		position.y = (float)Level.Current.Ground - 10f;
		BatLevelPentagram batLevelPentagram = UnityEngine.Object.Instantiate<BatLevelPentagram>(this.pentagramPrefab);
		batLevelPentagram.Init(position, base.properties.CurrentState.pentagrams, next, this.onRight);
	}

	// Token: 0x060016A6 RID: 5798 RVA: 0x000CAEE0 File Offset: 0x000C92E0
	private IEnumerator pentagram_cr()
	{
		LevelProperties.Bat.Pentagrams p = base.properties.CurrentState.pentagrams;
		string[] delayString = p.pentagramDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
		float delay = 0f;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			Parser.FloatTryParse(delayString[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			this.SpawnPentagram();
			delayIndex %= delayString.Length;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016A7 RID: 5799 RVA: 0x000CAEFB File Offset: 0x000C92FB
	public void StartCross()
	{
		base.StartCoroutine(this.cross_cr());
	}

	// Token: 0x060016A8 RID: 5800 RVA: 0x000CAF0C File Offset: 0x000C930C
	private void SpawnCross(int count)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		this.cross = UnityEngine.Object.Instantiate<BatLevelCross>(this.crossPrefab);
		this.cross.Init(this.bouncerRoot.position, base.properties.CurrentState.crossToss, count, next);
	}

	// Token: 0x060016A9 RID: 5801 RVA: 0x000CAF60 File Offset: 0x000C9360
	private IEnumerator cross_cr()
	{
		LevelProperties.Bat.CrossToss p = base.properties.CurrentState.crossToss;
		string[] delayString = p.crossDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] countString = p.attackCount.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
		int countIndex = UnityEngine.Random.Range(0, countString.Length);
		float delay = 0f;
		int count = 0;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Coffin)
		{
			if (this.cross == null)
			{
				Parser.FloatTryParse(delayString[delayIndex], out delay);
				Parser.IntTryParse(countString[countIndex], out count);
				yield return CupheadTime.WaitForSeconds(this, delay);
				this.SpawnCross(count);
			}
			delayIndex %= delayString.Length;
			count %= countString.Length;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060016AA RID: 5802 RVA: 0x000CAF7B File Offset: 0x000C937B
	public void StartPhase3()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.soul_cr());
	}

	// Token: 0x060016AB RID: 5803 RVA: 0x000CAFA0 File Offset: 0x000C93A0
	private IEnumerator shoot_cr()
	{
		LevelProperties.Bat.WolfFire p = base.properties.CurrentState.wolfFire;
		AbstractPlayerController player = PlayerManager.GetNext();
		float counter = 0f;
		while (base.properties.CurrentState.stateName == LevelProperties.Bat.States.Wolf)
		{
			yield return CupheadTime.WaitForSeconds(this, p.bulletDelay);
			this.ShootBullet(p.bulletSpeed, player);
			counter += 1f;
			if (counter >= p.bulletAimCount)
			{
				player = PlayerManager.GetNext();
				counter = 0f;
			}
		}
		yield break;
	}

	// Token: 0x060016AC RID: 5804 RVA: 0x000CAFBC File Offset: 0x000C93BC
	private void ShootBullet(float speed, AbstractPlayerController player)
	{
		float x = player.transform.position.x - this.bouncerRoot.position.x;
		float y = player.transform.position.y - this.bouncerRoot.position.y;
		float rotation = Mathf.Atan2(y, x) * 57.29578f;
		this.wolfProjectile.Create(this.bouncerRoot.position, rotation, speed);
	}

	// Token: 0x060016AD RID: 5805 RVA: 0x000CB04C File Offset: 0x000C944C
	private void SpawnSoul()
	{
		LevelProperties.Bat.WolfSoul wolfSoul = base.properties.CurrentState.wolfSoul;
		AbstractPlayerController next = PlayerManager.GetNext();
		BatLevelHomingSoul batLevelHomingSoul = UnityEngine.Object.Instantiate<BatLevelHomingSoul>(this.soulPrefab);
		batLevelHomingSoul.Init(this.bouncerRoot.position, next, wolfSoul);
	}

	// Token: 0x060016AE RID: 5806 RVA: 0x000CB094 File Offset: 0x000C9494
	private IEnumerator soul_cr()
	{
		this.SpawnSoul();
		yield return null;
		yield break;
	}

	// Token: 0x04001FDC RID: 8156
	[SerializeField]
	private Transform bouncerRoot;

	// Token: 0x04001FDD RID: 8157
	[SerializeField]
	private Transform coffinRoot;

	// Token: 0x04001FDE RID: 8158
	[SerializeField]
	private Transform pentagramRoot;

	// Token: 0x04001FDF RID: 8159
	[SerializeField]
	private BatLevelBouncer bouncerPrefab;

	// Token: 0x04001FE0 RID: 8160
	[SerializeField]
	private BatLevelGoblin goblinPrefab;

	// Token: 0x04001FE1 RID: 8161
	[SerializeField]
	private BatLevelMiniBat minibatPrefab;

	// Token: 0x04001FE2 RID: 8162
	[SerializeField]
	private BatLevelLightning lightningPrefab;

	// Token: 0x04001FE3 RID: 8163
	private BatLevelLightning lightning;

	// Token: 0x04001FE4 RID: 8164
	[SerializeField]
	private BatLevelPentagram pentagramPrefab;

	// Token: 0x04001FE5 RID: 8165
	[SerializeField]
	private BasicProjectile wolfProjectile;

	// Token: 0x04001FE6 RID: 8166
	[SerializeField]
	private BatLevelCross crossPrefab;

	// Token: 0x04001FE7 RID: 8167
	private BatLevelCross cross;

	// Token: 0x04001FE8 RID: 8168
	[SerializeField]
	private BatLevelHomingSoul soulPrefab;

	// Token: 0x04001FE9 RID: 8169
	private BatLevelBat.Direction direction = BatLevelBat.Direction.Left;

	// Token: 0x04001FEB RID: 8171
	private DamageDealer damageDealer;

	// Token: 0x04001FEC RID: 8172
	private bool slowDown;

	// Token: 0x04001FED RID: 8173
	private bool onRight;

	// Token: 0x04001FEE RID: 8174
	private bool inMovingPhase;

	// Token: 0x04001FEF RID: 8175
	private bool moving;

	// Token: 0x04001FF0 RID: 8176
	private string[] anglePattern;

	// Token: 0x04001FF1 RID: 8177
	private int angleIndex;

	// Token: 0x04001FF2 RID: 8178
	private float speed;

	// Token: 0x04001FF3 RID: 8179
	private float originalSpeed;

	// Token: 0x04001FF4 RID: 8180
	private Vector3 startPosition;

	// Token: 0x04001FF5 RID: 8181
	private Coroutine pattern;

	// Token: 0x02000501 RID: 1281
	public enum Direction
	{
		// Token: 0x04001FF7 RID: 8183
		Right,
		// Token: 0x04001FF8 RID: 8184
		Left
	}

	// Token: 0x02000502 RID: 1282
	public enum State
	{
		// Token: 0x04001FFA RID: 8186
		Idle,
		// Token: 0x04001FFB RID: 8187
		Bouncer,
		// Token: 0x04001FFC RID: 8188
		Lightning,
		// Token: 0x04001FFD RID: 8189
		Phase2
	}
}
