using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004C6 RID: 1222
public class AirplaneLevelTerrier : AbstractCollidableObject
{
	// Token: 0x17000313 RID: 787
	// (get) Token: 0x0600148C RID: 5260 RVA: 0x000B8426 File Offset: 0x000B6826
	// (set) Token: 0x0600148D RID: 5261 RVA: 0x000B842E File Offset: 0x000B682E
	public bool IsDead { get; private set; }

	// Token: 0x17000314 RID: 788
	// (get) Token: 0x0600148E RID: 5262 RVA: 0x000B8437 File Offset: 0x000B6837
	// (set) Token: 0x0600148F RID: 5263 RVA: 0x000B843F File Offset: 0x000B683F
	public bool ReadyToMove { get; private set; }

	// Token: 0x06001490 RID: 5264 RVA: 0x000B8448 File Offset: 0x000B6848
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x000B8478 File Offset: 0x000B6878
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x000B8488 File Offset: 0x000B6888
	public void Init(Transform pivotPoint, float angle, LevelProperties.Airplane.Terriers properties, float hp, float pivotOffsetX, float pivotOffsetY, bool isClockwise, int index)
	{
		this.angle = angle;
		this.pivotPoint = pivotPoint;
		this.pivotOffset = new Vector2(pivotOffsetX, pivotOffsetY);
		this.properties = properties;
		this.hp = hp;
		this.smokingThreshold = hp * properties.secretHPPercentage;
		this.isClockwise = isClockwise;
		this.index = index;
		this.wobbleTimer = (float)index;
		base.StartCoroutine(this.setup_dogs_cr());
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x000B84F6 File Offset: 0x000B68F6
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x000B852C File Offset: 0x000B692C
	public bool IsSmoking()
	{
		if (!this.isSmoking && this.hp < this.smokingThreshold)
		{
			this.isSmoking = true;
			base.animator.SetTrigger(AirplaneLevelTerrier.OnShockParameterID);
			AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_dmgsmoke");
			this.emitAudioFromObject.Add("sfx_dlc_dogfight_p2_terrierjetpack_dmgsmoke");
		}
		return this.isSmoking;
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x000B858C File Offset: 0x000B698C
	public float Health()
	{
		return this.hp;
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x000B8594 File Offset: 0x000B6994
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x000B85B2 File Offset: 0x000B69B2
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		this.wobbleTimer += this.wobbleSpeed * CupheadTime.Delta;
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x000B85E8 File Offset: 0x000B69E8
	private Vector3 WobblePos()
	{
		return new Vector3(Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX, Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY, 0f) * this.wobbleModifier;
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x000B863C File Offset: 0x000B6A3C
	private IEnumerator setup_dogs_cr()
	{
		this.rotationOffset = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		int indexToPlay = this.index;
		base.transform.SetScale(new float?((!this.isClockwise) ? Mathf.Abs(base.transform.localScale.x) : (-Mathf.Abs(base.transform.localScale.x))), null, null);
		int num = this.index;
		if (num != 1)
		{
			if (num == 3)
			{
				indexToPlay = ((!this.isClockwise) ? this.index : (indexToPlay = 1));
			}
		}
		else
		{
			indexToPlay = ((!this.isClockwise) ? this.index : (indexToPlay = 3));
		}
		base.animator.Play("Intro_" + indexToPlay);
		int flamePos = indexToPlay * 4;
		if (indexToPlay == 3)
		{
			flamePos = 4;
		}
		this.flame.transform.localPosition = this.flameOffset[flamePos];
		if (indexToPlay == 1)
		{
			this.flame.transform.localPosition = new Vector3(-this.flame.transform.localPosition.x, this.flame.transform.localPosition.y);
		}
		this.angle *= 0.017453292f;
		this.loopSizeX = 675f;
		this.loopSizeY = 328.5f;
		this.rotationOffset.x = Mathf.Sin(this.angle) * this.loopSizeX;
		this.rotationOffset.y = Mathf.Cos(this.angle) * this.loopSizeY;
		Vector3 startPos = this.pivotPoint.position + this.pivotOffset + this.rotationOffset * 2f;
		Vector3 endPos = this.pivotPoint.position + this.pivotOffset + this.rotationOffset;
		float t = 0f;
		float time = 0.5f;
		base.transform.position = startPos;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(startPos, endPos, t / time) + this.WobblePos();
			yield return wait;
		}
		base.animator.SetTrigger("ContinueIntro");
		t = 0f;
		while (t < 0.9f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = endPos + this.WobblePos();
			this.wobbleModifier = Mathf.Lerp(1f, 0f, t / 0.9f);
			yield return wait;
		}
		this.wobbleModifier = 0f;
		base.animator.SetTrigger("EndIntro");
		this.rotationSpeed = 0f;
		this.ReadyToMove = true;
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		this.introFinished = true;
		((AirplaneLevel)Level.Current).terriersIntroFinished = true;
		yield break;
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x000B8658 File Offset: 0x000B6A58
	private IEnumerator ease_to_full_speed_and_radius_cr()
	{
		float t = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < 1f)
		{
			this.loopSizeX = Mathf.Lerp(675f, 750f, EaseUtils.EaseOutSine(0f, 1f, t / 1f));
			this.loopSizeY = Mathf.Lerp(328.5f, 365f, EaseUtils.EaseOutSine(0f, 1f, t / 1f));
			this.rotationSpeed = Mathf.Lerp(0f, this.properties.rotationTime, EaseUtils.EaseInSine(0f, 1f, t / 1f));
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		this.loopSizeX = 750f;
		this.loopSizeY = 365f;
		this.rotationSpeed = this.properties.rotationTime;
		yield break;
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x000B8673 File Offset: 0x000B6A73
	public void StartMoving()
	{
		base.StartCoroutine(this.move_in_circle_cr());
		base.StartCoroutine(this.ease_to_full_speed_and_radius_cr());
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x000B8690 File Offset: 0x000B6A90
	private IEnumerator move_in_circle_cr()
	{
		this.rotationOffset = Vector3.zero;
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			this.angle += this.rotationSpeed * CupheadTime.FixedDelta * (float)((!this.isClockwise) ? -1 : 1);
			if (!this.gettingEaten)
			{
				this.rotationOffset.x = Mathf.Sin(this.angle) * this.loopSizeX;
			}
			else
			{
				bool flag = (!this.isClockwise) ? (this.angle < 3.1415927f) : (this.angle > 3.1415927f);
				this.rotationOffset.x = Mathf.Sin(this.angle) * ((!flag) ? this.loopSizeX : 1000f);
				if (flag)
				{
					this.loopSizeY -= CupheadTime.FixedDelta * 50f;
				}
			}
			this.rotationOffset.y = Mathf.Cos(this.angle) * this.loopSizeY;
			Vector3 lastPos = base.transform.position;
			base.transform.position = this.pivotPoint.position + this.pivotOffset;
			base.transform.position += this.rotationOffset;
			this.flame.flipX = (Mathf.Sign(lastPos.x - base.transform.position.x) == -1f);
			if (this.angle > 6.2831855f)
			{
				this.angle -= 6.2831855f;
			}
			if (this.angle < 0f)
			{
				this.angle += 6.2831855f;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x000B86AC File Offset: 0x000B6AAC
	public Vector3 GetPredictedAttackPos()
	{
		float num = 0.125f;
		float f = this.angle + this.properties.rotationTime * num * (float)((!this.isClockwise) ? -1 : 1);
		return this.pivotPoint.position + this.pivotOffset + new Vector2(Mathf.Sin(f) * 750f, Mathf.Cos(f) * 365f);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x000B872C File Offset: 0x000B6B2C
	public void StartAttack(bool isPink, bool isWow)
	{
		this.isPink = isPink;
		this.isWow = isWow;
		if (this.isClockwise)
		{
			base.transform.SetScale(new float?(Mathf.Abs(base.transform.localScale.x)), null, null);
		}
		base.animator.Play("Attack");
		this.SFX_DOGFIGHT_P2_TerrierJetpack_BarkShoot();
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000B87A4 File Offset: 0x000B6BA4
	private void AniEvent_BarkFX()
	{
		this.barkFXRenderer.sortingLayerID = this.rends[this.currentAngle].sortingLayerID;
		this.barkFXRenderer.sortingOrder = this.rends[this.currentAngle].sortingOrder + ((this.currentAngle > 4) ? -1 : 1);
		this.barkFXRenderer.flipX = this.rends[this.currentAngle].flipX;
		this.barkFXRenderer.transform.localPosition = -this.flame.transform.localPosition * 0.5f;
		if (this.currentAngle == 1)
		{
			this.barkFXRenderer.transform.localPosition += new Vector3((float)((!this.barkFXRenderer.flipX) ? 10 : -10), 12f);
		}
		if (this.currentAngle == 2)
		{
			this.barkFXRenderer.transform.localPosition += new Vector3((float)((!this.barkFXRenderer.flipX) ? -10 : 10), 5f);
		}
		this.barkFXRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.barkFXAnimator.Play((!Rand.Bool()) ? "B" : "A");
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x000B8930 File Offset: 0x000B6D30
	private void AniEvent_ShootProjectile()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = next.center - this.barkFXRenderer.transform.position;
		float acceleration = Vector3.Magnitude(this.rotationOffset) / 750f;
		AirplaneLevelTerrierBullet airplaneLevelTerrierBullet;
		if (this.isPink)
		{
			airplaneLevelTerrierBullet = this.pinkProjectile.Create(this.barkFXRenderer.transform.position, MathUtils.DirectionToAngle(v), this.properties.shotSpeed, acceleration);
		}
		else
		{
			airplaneLevelTerrierBullet = this.regularProjectile.Create(this.barkFXRenderer.transform.position, MathUtils.DirectionToAngle(v), this.properties.shotSpeed, acceleration);
		}
		if (this.isWow)
		{
			airplaneLevelTerrierBullet.PlayWow();
		}
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x000B8A04 File Offset: 0x000B6E04
	private void AniEvent_SetScale()
	{
		if (this.isClockwise)
		{
			base.transform.SetScale(new float?(Mathf.Abs(base.transform.localScale.x)), null, null);
		}
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x000B8A56 File Offset: 0x000B6E56
	public void StartSecret()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.move_into_mouth_cr());
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x000B8A74 File Offset: 0x000B6E74
	public void PrepareForChomp()
	{
		this.gettingEaten = true;
		foreach (SpriteRenderer spriteRenderer in this.rends)
		{
			spriteRenderer.sortingOrder = 2;
			spriteRenderer.sortingLayerName = "Foreground";
		}
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x000B8ABC File Offset: 0x000B6EBC
	private IEnumerator move_into_mouth_cr()
	{
		float t = 0f;
		float startSpeed = this.rotationSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < 1f)
		{
			t += CupheadTime.FixedDelta;
			this.rotationSpeed = Mathf.Lerp(startSpeed, 2.5f, EaseUtils.EaseInSine(0f, 1f, t));
			this.loopSizeX = Mathf.Lerp(750f, 600f, EaseUtils.EaseInSine(0f, 1f, t));
			this.loopSizeY = Mathf.Lerp(365f, 292f, EaseUtils.EaseInSine(0f, 1f, t));
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x000B8AD8 File Offset: 0x000B6ED8
	private void Die()
	{
		this.IsDead = true;
		this.flame.enabled = false;
		this.coll.enabled = false;
		base.animator.Play((!this.lastOne) ? "Death" : "DeathShort");
		this.SFX_DOGFIGHT_P2_TerrierJetpack_Explosion();
		base.StartCoroutine(this.SFX_DOGFIGHT_P2_TerrierJetpack_DeathBark_cr(this.index));
	}

	// Token: 0x060014A6 RID: 5286 RVA: 0x000B8B42 File Offset: 0x000B6F42
	private void AniEvent_DeathLayering()
	{
		this.deathRenderer.sortingLayerName = "Background";
		this.deathRenderer.sortingOrder = 0;
	}

	// Token: 0x060014A7 RID: 5287 RVA: 0x000B8B60 File Offset: 0x000B6F60
	private void AniEvent_OnDeath()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x000B8B74 File Offset: 0x000B6F74
	private void CheckAllAnimations()
	{
		if (this.gettingEaten)
		{
			return;
		}
		float num = this.angle * 57.29578f;
		if ((num > 344f && num < 360f) || (num < 8.2f && num > 0f))
		{
			this.ChangeAngle(false, 0);
		}
		else if (num > 8.2f && num < 31.8f)
		{
			this.ChangeAngle(true, 1);
		}
		else if (num > 31.8f && num < 55.4f)
		{
			this.ChangeAngle(true, 2);
		}
		else if (num > 55.4f && num < 79f)
		{
			this.ChangeAngle(true, 3);
		}
		else if (num > 79f && num < 102.6f)
		{
			this.ChangeAngle(true, 4);
		}
		else if (num > 102.6f && num < 126.2f)
		{
			this.ChangeAngle(true, 5);
		}
		else if (num > 126.2f && num < 149.8f)
		{
			this.ChangeAngle(true, 6);
		}
		else if (num > 149.8f && num < 164.75f)
		{
			this.ChangeAngle(true, 7);
		}
		else if (num > 164.75f && num < 195.25f)
		{
			this.ChangeAngle(false, 8);
		}
		else if (num > 195.25f && num < 218.85f)
		{
			this.ChangeAngle(false, 7);
		}
		else if (num > 218.85f && num < 242.45f)
		{
			this.ChangeAngle(false, 6);
		}
		else if (num > 242.45f && num < 259f)
		{
			this.ChangeAngle(false, 5);
		}
		else if (num > 259f && num < 282.6f)
		{
			this.ChangeAngle(false, 4);
		}
		else if (num > 282.6f && num < 306.2f)
		{
			this.ChangeAngle(false, 3);
		}
		else if (num > 306.2f && num < 329.8f)
		{
			this.ChangeAngle(false, 2);
		}
		else if (num > 329.8f && num < 344f)
		{
			this.ChangeAngle(false, 1);
		}
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x000B8DDC File Offset: 0x000B71DC
	private void ChangeAngle(bool flipSprite, int layerIndex)
	{
		this.currentAngle = layerIndex;
		if (!this.isCurved != (layerIndex == 3 || layerIndex == 4 || layerIndex == 5))
		{
			this.flameAnimator.Play((layerIndex != 3 && layerIndex != 4 && layerIndex != 5) ? "Curve" : "Straight", 0, this.flameAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
			this.isCurved = !this.isCurved;
		}
		foreach (GameObject gameObject in this.terrierLayers)
		{
			gameObject.SetActive(gameObject == this.terrierLayers[layerIndex]);
		}
		this.rends[layerIndex].flipX = flipSprite;
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x000B8EA8 File Offset: 0x000B72A8
	private void FixedUpdate()
	{
		if (!this.IsDead)
		{
			this.smokeTimer += CupheadTime.FixedDelta * ((!this.introFinished) ? 0.2f : ((!this.gettingEaten) ? 1f : 0.1f));
			if (this.smokeTimer > this.smokeDelay)
			{
				this.smokeTimer -= this.smokeDelay;
				((AirplaneLevel)Level.Current).CreateSmokeFX(this.flame.transform.position, (!this.introFinished) ? (MathUtils.AngleToDirection(this.flame.transform.eulerAngles.z - 90f) * 300f) : Vector2.zero, this.hp < this.smokingThreshold, this.rends[this.currentAngle].sortingLayerID, (this.currentAngle > 4) ? 30 : -1);
			}
		}
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x000B8FC0 File Offset: 0x000B73C0
	private void LateUpdate()
	{
		if (this.rends[9].sprite == null)
		{
			this.CheckAllAnimations();
		}
		if (this.introFinished)
		{
			this.flame.sortingLayerID = this.rends[this.currentAngle].sortingLayerID;
			this.flame.sortingOrder = this.rends[this.currentAngle].sortingOrder - 1;
			this.flame.transform.localPosition = new Vector3(this.flameOffset[this.currentAngle].x * (float)((!this.rends[this.currentAngle].flipX) ? 1 : -1), this.flameOffset[this.currentAngle].y);
		}
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x000B9094 File Offset: 0x000B7494
	public float RelativeAngle()
	{
		if (!this.isClockwise)
		{
			return 6.2831855f - this.angle;
		}
		return this.angle;
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x000B90B4 File Offset: 0x000B74B4
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		float[] array = new float[]
		{
			344f,
			8.2f,
			31.8f,
			55.4f,
			329.8f,
			306.2f,
			282.6f,
			164.75f,
			242.45f,
			218.85f,
			195.25f,
			102.6f,
			126.2f,
			149.8f,
			79f,
			259f
		};
		Vector3 a = Vector3.zero;
		float d = 400f;
		for (int i = 0; i < array.Length; i++)
		{
			a = MathUtils.AngleToDirection(array[i] + 90f);
			if (array[i] == 344f || array[i] == 164.75f || array[i] == 259f || array[i] == 79f)
			{
				Gizmos.color = Color.blue;
			}
			else if (array[i] == 195.25f || array[i] == 282.6f || array[i] == 8.2f || array[i] == 102.6f)
			{
				Gizmos.color = Color.green;
			}
			else
			{
				Gizmos.color = Color.red;
			}
			Gizmos.DrawLine(Vector3.zero, a * d);
		}
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x000B91AF File Offset: 0x000B75AF
	private void SFX_DOGFIGHT_P2_TerrierJetpack_BarkShoot()
	{
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_barkshoot");
	}

	// Token: 0x060014AF RID: 5295 RVA: 0x000B91BB File Offset: 0x000B75BB
	private void SFX_DOGFIGHT_P2_TerrierJetpack_Explosion()
	{
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_explosion");
	}

	// Token: 0x060014B0 RID: 5296 RVA: 0x000B91C8 File Offset: 0x000B75C8
	private IEnumerator SFX_DOGFIGHT_P2_TerrierJetpack_DeathBark_cr(int id)
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_dlc_dogfight_p2_terrierjetpack_dmgdeath_0" + id);
		yield break;
	}

	// Token: 0x060014B1 RID: 5297 RVA: 0x000B91EC File Offset: 0x000B75EC
	private void WORKAROUND_NullifyFields()
	{
		this.coll = null;
		this.regularProjectile = null;
		this.pinkProjectile = null;
		this.terrierLayers = null;
		this.deathRenderer = null;
		this.rends = null;
		this.flameOffset = null;
		this.flame = null;
		this.flameAnimator = null;
		this.barkFXRenderer = null;
		this.barkFXAnimator = null;
		this.pivotPoint = null;
		this.damageDealer = null;
	}

	// Token: 0x04001DE0 RID: 7648
	private static readonly int OnShockParameterID = Animator.StringToHash("OnShock");

	// Token: 0x04001DE3 RID: 7651
	private const float NINETY_DEGREES = 90f;

	// Token: 0x04001DE4 RID: 7652
	private const float THREE_SIXTY = 360f;

	// Token: 0x04001DE5 RID: 7653
	private const float LOOP_SIZE_Y = 365f;

	// Token: 0x04001DE6 RID: 7654
	private const float LOOP_SIZE_X = 750f;

	// Token: 0x04001DE7 RID: 7655
	private const float LOOP_SIZE_X_SECRET_INTRO = 1000f;

	// Token: 0x04001DE8 RID: 7656
	private const float LOOP_SIZE_INTRO_MOD = 0.9f;

	// Token: 0x04001DE9 RID: 7657
	private const float TIME_TO_FULL_LOOP_SIZE = 1f;

	// Token: 0x04001DEA RID: 7658
	private const float UP = 344f;

	// Token: 0x04001DEB RID: 7659
	private const float UP_RIGHT_1 = 8.2f;

	// Token: 0x04001DEC RID: 7660
	private const float UP_RIGHT_2 = 31.8f;

	// Token: 0x04001DED RID: 7661
	private const float UP_RIGHT_3 = 55.4f;

	// Token: 0x04001DEE RID: 7662
	private const float RIGHT = 79f;

	// Token: 0x04001DEF RID: 7663
	private const float DOWN_RIGHT_1 = 102.6f;

	// Token: 0x04001DF0 RID: 7664
	private const float DOWN_RIGHT_2 = 126.2f;

	// Token: 0x04001DF1 RID: 7665
	private const float DOWN_RIGHT_3 = 149.8f;

	// Token: 0x04001DF2 RID: 7666
	private const float DOWN = 164.75f;

	// Token: 0x04001DF3 RID: 7667
	private const float DOWN_LEFT_1 = 242.45f;

	// Token: 0x04001DF4 RID: 7668
	private const float DOWN_LEFT_2 = 218.85f;

	// Token: 0x04001DF5 RID: 7669
	private const float DOWN_LEFT_3 = 195.25f;

	// Token: 0x04001DF6 RID: 7670
	private const float LEFT = 259f;

	// Token: 0x04001DF7 RID: 7671
	private const float UP_LEFT_1 = 329.8f;

	// Token: 0x04001DF8 RID: 7672
	private const float UP_LEFT_2 = 306.2f;

	// Token: 0x04001DF9 RID: 7673
	private const float UP_LEFT_3 = 282.6f;

	// Token: 0x04001DFA RID: 7674
	[SerializeField]
	private BoxCollider2D coll;

	// Token: 0x04001DFB RID: 7675
	[SerializeField]
	private AirplaneLevelTerrierBullet regularProjectile;

	// Token: 0x04001DFC RID: 7676
	[SerializeField]
	private AirplaneLevelTerrierBullet pinkProjectile;

	// Token: 0x04001DFD RID: 7677
	[SerializeField]
	private GameObject[] terrierLayers;

	// Token: 0x04001DFE RID: 7678
	[SerializeField]
	private SpriteRenderer deathRenderer;

	// Token: 0x04001DFF RID: 7679
	[SerializeField]
	private SpriteRenderer[] rends;

	// Token: 0x04001E00 RID: 7680
	[SerializeField]
	private Vector3[] flameOffset;

	// Token: 0x04001E01 RID: 7681
	[SerializeField]
	private SpriteRenderer flame;

	// Token: 0x04001E02 RID: 7682
	[SerializeField]
	private Animator flameAnimator;

	// Token: 0x04001E03 RID: 7683
	[SerializeField]
	private SpriteRenderer barkFXRenderer;

	// Token: 0x04001E04 RID: 7684
	[SerializeField]
	private Animator barkFXAnimator;

	// Token: 0x04001E05 RID: 7685
	private LevelProperties.Airplane.Terriers properties;

	// Token: 0x04001E06 RID: 7686
	public float angle;

	// Token: 0x04001E07 RID: 7687
	private float hp;

	// Token: 0x04001E08 RID: 7688
	private float smokingThreshold;

	// Token: 0x04001E09 RID: 7689
	private int index;

	// Token: 0x04001E0A RID: 7690
	private bool isClockwise;

	// Token: 0x04001E0B RID: 7691
	private bool isPink;

	// Token: 0x04001E0C RID: 7692
	private bool isWow;

	// Token: 0x04001E0D RID: 7693
	private bool isSmoking;

	// Token: 0x04001E0E RID: 7694
	private bool gettingEaten;

	// Token: 0x04001E0F RID: 7695
	private Transform pivotPoint;

	// Token: 0x04001E10 RID: 7696
	private DamageDealer damageDealer;

	// Token: 0x04001E11 RID: 7697
	private DamageReceiver damageReceiver;

	// Token: 0x04001E12 RID: 7698
	private Vector2 pivotOffset;

	// Token: 0x04001E13 RID: 7699
	[SerializeField]
	private float wobbleX = 10f;

	// Token: 0x04001E14 RID: 7700
	[SerializeField]
	private float wobbleY = 10f;

	// Token: 0x04001E15 RID: 7701
	[SerializeField]
	private float wobbleSpeed = 1f;

	// Token: 0x04001E16 RID: 7702
	private float wobbleTimer;

	// Token: 0x04001E17 RID: 7703
	private float wobbleModifier = 1f;

	// Token: 0x04001E18 RID: 7704
	private float rotationSpeed;

	// Token: 0x04001E19 RID: 7705
	private float loopSizeX;

	// Token: 0x04001E1A RID: 7706
	private float loopSizeY;

	// Token: 0x04001E1B RID: 7707
	private bool isCurved;

	// Token: 0x04001E1C RID: 7708
	private int currentAngle;

	// Token: 0x04001E1D RID: 7709
	private float smokeDelay = 0.02f;

	// Token: 0x04001E1E RID: 7710
	private float smokeTimer;

	// Token: 0x04001E1F RID: 7711
	private bool introFinished;

	// Token: 0x04001E20 RID: 7712
	public bool lastOne;

	// Token: 0x04001E21 RID: 7713
	private Vector3 rotationOffset;
}
