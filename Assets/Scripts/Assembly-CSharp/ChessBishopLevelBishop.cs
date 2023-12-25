using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000536 RID: 1334
public class ChessBishopLevelBishop : LevelProperties.ChessBishop.Entity
{
	// Token: 0x06001820 RID: 6176 RVA: 0x000DA13C File Offset: 0x000D853C
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		for (int i = 0; i < this.candles.Length; i++)
		{
			this.candles[i].Init(base.properties.CurrentState.candle.candleDistToBlowout);
		}
	}

	// Token: 0x06001821 RID: 6177 RVA: 0x000DA18F File Offset: 0x000D858F
	public override void LevelInit(LevelProperties.ChessBishop properties)
	{
		base.LevelInit(properties);
		Level.Current.OnIntroEvent += this.onIntroEventHandler;
		this.setupPatternStrings();
	}

	// Token: 0x06001822 RID: 6178 RVA: 0x000DA1B4 File Offset: 0x000D85B4
	private void UpdateBodyFade()
	{
		float num = Mathf.Clamp(this.bodyOpacity, 0f, 1f);
		this.bodyRenderer.color = new Color(1f, 1f, 1f, num);
		this.bodyRenderer.material.SetFloat("_BlurAmount", (1f - num) * 5f);
		this.bodyRenderer.material.SetFloat("_BlurLerp", (1f - num) * 5f);
	}

	// Token: 0x06001823 RID: 6179 RVA: 0x000DA23C File Offset: 0x000D863C
	private void FixedUpdate()
	{
		if (PlayerManager.GetPlayer(PlayerId.PlayerOne) && !PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
		{
			this.playerMask[0].transform.position = PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position + Vector3.up * 50f;
		}
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) && !PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead)
		{
			this.playerMask[1].transform.position = PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.up * 50f;
		}
		if (this.introPlaying || this.dead)
		{
			return;
		}
		this.bodyOpacity -= CupheadTime.FixedDelta * this.fadeRate;
		this.UpdateBodyFade();
		if (this.damageDealer != null)
		{
			this.damageDealer.FixedUpdate();
		}
	}

	// Token: 0x06001824 RID: 6180 RVA: 0x000DA341 File Offset: 0x000D8741
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x000DA358 File Offset: 0x000D8758
	public void StartNewPhase()
	{
		this.stateDidChange = true;
		this.cancelShoot();
		this.StopAllCoroutines();
		this.candleOrderMainIndex %= base.properties.CurrentState.candle.candleOrder.Length;
		this.setupPatternStrings();
		base.StartCoroutine(this.disappear_cr());
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x000DA3B0 File Offset: 0x000D87B0
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.cancelShoot();
		base.properties.DealDamage((!PlayerManager.BothPlayersActive()) ? 10f : ChessKingLevelKing.multiplayerDamageNerf);
		if (base.properties.CurrentHealth <= 0f)
		{
			this.die();
		}
		else
		{
			this.bodyOpacity = 1.75f;
			this.bodyAnimator.SetTrigger("Hit");
			this.bodyExplosion.Create(this.bodyExplosionSpawnPoint.position);
			this.turnDormant(this.stateDidChange);
			this.stateDidChange = false;
		}
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x000DA453 File Offset: 0x000D8853
	private void onIntroEventHandler()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001828 RID: 6184 RVA: 0x000DA464 File Offset: 0x000D8864
	private IEnumerator intro_cr()
	{
		this.bodyAnimator.SetTrigger("StartIntro");
		yield return this.bodyAnimator.WaitForAnimationToEnd(this, "Intro.End", false, true);
		this.isPathTwo = MathUtils.RandomBool();
		this.startPath();
		yield return CupheadTime.WaitForSeconds(this, 0.55f);
		base.animator.SetBool("CanParry", true);
		base.animator.SetTrigger("Appear");
		yield return base.animator.WaitForAnimationToEnd(this, "AppearActive", false, true);
		this.candleOrderMainIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.candle.candleOrder.Length);
		this.candlesHolder.SetActive(true);
		this.canMove = true;
		this.introPlaying = false;
		yield break;
	}

	// Token: 0x06001829 RID: 6185 RVA: 0x000DA480 File Offset: 0x000D8880
	private void turnDormant(bool willDisappear)
	{
		this._canParry = false;
		if (base.properties.CurrentHealth > 0f)
		{
			base.StartCoroutine(this.candles_cr());
		}
		if (!willDisappear)
		{
			base.animator.SetTrigger("ToDormant");
			base.StartCoroutine(this.postHitToggleCollider_cr());
		}
	}

	// Token: 0x0600182A RID: 6186 RVA: 0x000DA4DC File Offset: 0x000D88DC
	private IEnumerator postHitToggleCollider_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bishop.colliderOffTime);
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x0600182B RID: 6187 RVA: 0x000DA4F8 File Offset: 0x000D88F8
	private IEnumerator candles_cr()
	{
		while (this.disappearingState == ChessBishopLevelBishop.DisappearingState.Disappearing)
		{
			yield return null;
		}
		LevelProperties.ChessBishop.Candle p = base.properties.CurrentState.candle;
		string[] candleOrder = p.candleOrder[this.candleOrderMainIndex].Split(new char[]
		{
			','
		});
		int length = candleOrder.Length + ((!PlayerManager.BothPlayersActive()) ? 0 : 3);
		ChessBishopLevelCandle[] activeCandles = new ChessBishopLevelCandle[length];
		int index = 0;
		for (int i = 0; i < candleOrder.Length; i++)
		{
			Parser.IntTryParse(candleOrder[i], out index);
			this.candles[index].LightUp();
			activeCandles[i] = this.candles[index];
		}
		if (PlayerManager.BothPlayersActive())
		{
			List<ChessBishopLevelCandle> list = (from c in this.candles
			where !c.isLit
			select c).ToList<ChessBishopLevelCandle>();
			for (int j = 0; j < 3; j++)
			{
				if (list.Count > 0)
				{
					index = UnityEngine.Random.Range(0, list.Count);
					list[index].LightUp();
					activeCandles[candleOrder.Length + j] = list[index];
					list.RemoveAt(index);
				}
			}
		}
		this.candleOrderMainIndex = (this.candleOrderMainIndex + 1) % p.candleOrder.Length;
		yield return null;
		bool candlesStillLit = true;
		while (candlesStillLit)
		{
			candlesStillLit = false;
			for (int k = 0; k < activeCandles.Length; k++)
			{
				if (activeCandles[k] != null && activeCandles[k].isLit)
				{
					candlesStillLit = true;
					break;
				}
			}
			yield return null;
		}
		this.cancelShoot();
		if (this.disappearingState == ChessBishopLevelBishop.DisappearingState.Disappearing)
		{
			this._canParry = true;
			yield break;
		}
		while (this.disappearingState == ChessBishopLevelBishop.DisappearingState.Reappearing)
		{
			yield return null;
		}
		this._canParry = true;
		base.animator.SetTrigger("ToActive");
		yield break;
	}

	// Token: 0x0600182C RID: 6188 RVA: 0x000DA514 File Offset: 0x000D8914
	private IEnumerator disappear_cr()
	{
		this.disappearingState = ChessBishopLevelBishop.DisappearingState.Disappearing;
		this.isFirstPhase = false;
		this.canMove = false;
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = false;
		base.animator.SetTrigger("HitDisappear");
		yield return base.animator.WaitForAnimationToEnd(this, "HitDisappear", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.invisibleTime.PopFloat());
		this.disappearingState = ChessBishopLevelBishop.DisappearingState.Reappearing;
		this.isPathTwo = !this.isPathTwo;
		this.startPath();
		base.animator.SetBool("CanParry", this._canParry);
		base.animator.SetTrigger("Appear");
		string animationName = (!base.canParry) ? "AppearDormant" : "AppearActive";
		yield return base.animator.WaitForAnimationToEnd(this, animationName, false, true);
		this.canMove = true;
		collider.enabled = true;
		this.disappearingState = ChessBishopLevelBishop.DisappearingState.None;
		yield break;
	}

	// Token: 0x0600182D RID: 6189 RVA: 0x000DA52F File Offset: 0x000D892F
	private void startPath()
	{
		if (this.isPathTwo)
		{
			base.StartCoroutine(this.moveHorizontal_cr());
		}
		else
		{
			base.StartCoroutine(this.moveVertical_cr());
		}
	}

	// Token: 0x0600182E RID: 6190 RVA: 0x000DA55C File Offset: 0x000D895C
	private IEnumerator moveVertical_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 pivotOffset = Vector3.up * 2f * 150f;
		this.invert = true;
		float value = -1f;
		float speed = base.properties.CurrentState.bishop.movementSpeed;
		float angle = 3.9269907f;
		if (!this.isFirstPhase)
		{
			float minimumDistance = base.GetComponent<CircleCollider2D>().radius * 1.5f;
			angle = ChessBishopLevelBishop.findMoveVerticalInitialAngle(minimumDistance, value, this.invert, speed, this.pivotPoint, pivotOffset);
		}
		base.StartCoroutine(this.spawnProjectiles_cr());
		for (;;)
		{
			base.transform.position = ChessBishopLevelBishop.calculateMoveVerticalPosition(ref angle, ref value, ref this.invert, speed, this.pivotPoint, pivotOffset);
			while (!this.canMove)
			{
				yield return null;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600182F RID: 6191 RVA: 0x000DA578 File Offset: 0x000D8978
	private static float findMoveVerticalInitialAngle(float minimumDistance, float value, bool invert, float speed, Transform pivotPoint, Vector3 pivotOffset)
	{
		float num = minimumDistance * minimumDistance;
		List<Vector3> list = new List<Vector3>();
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerOne).center);
		}
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center);
		}
		int i = 0;
		float num2 = 0f;
		while (i < 20)
		{
			i++;
			num2 = UnityEngine.Random.Range(0f, 6.2831855f);
			float num3 = num2;
			float num4 = value;
			bool flag = invert;
			Vector3 a = ChessBishopLevelBishop.calculateMoveVerticalPosition(ref num3, ref num4, ref flag, speed, pivotPoint, pivotOffset);
			bool flag2 = false;
			foreach (Vector3 b in list)
			{
				if ((a - b).sqrMagnitude < num)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				break;
			}
		}
		return num2;
	}

	// Token: 0x06001830 RID: 6192 RVA: 0x000DA67C File Offset: 0x000D8A7C
	private static Vector3 calculateMoveVerticalPosition(ref float angle, ref float value, ref bool invert, float speed, Transform pivotPoint, Vector3 pivotOffset)
	{
		angle += speed * CupheadTime.FixedDelta;
		if (angle > 6.2831855f)
		{
			invert = !invert;
			angle -= 6.2831855f;
		}
		if (angle < 0f)
		{
			angle += 6.2831855f;
		}
		Vector3 a;
		if (invert)
		{
			a = pivotPoint.position + pivotOffset;
			value = -1f;
		}
		else
		{
			a = pivotPoint.position;
			value = 1f;
		}
		Vector3 b = new Vector3(-Mathf.Sin(angle) * 500f, Mathf.Cos(angle) * value * 150f, 0f);
		return a + b;
	}

	// Token: 0x06001831 RID: 6193 RVA: 0x000DA72C File Offset: 0x000D8B2C
	private IEnumerator moveHorizontal_cr()
	{
		LevelProperties.ChessBishop.Bishop p = base.properties.CurrentState.bishop;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.invert = true;
		float xSpeed = p.xSpeed;
		float amplitude = p.amplitude;
		float frequency = p.freqMultiplier * 2f * 3.1415927f / (p.maxDistance * 2f);
		if (this.isFirstPhase)
		{
			base.transform.position = new Vector3(500f, base.transform.position.y);
		}
		else
		{
			float minimumDistance = base.GetComponent<CircleCollider2D>().radius * 1.5f;
			base.transform.position = ChessBishopLevelBishop.findMoveHorizontalInitialPosition(minimumDistance, base.transform.position.y, p.maxDistance, xSpeed, amplitude, frequency);
		}
		base.StartCoroutine(this.spawnProjectiles_cr());
		Vector3 goalPos = base.transform.position;
		float distanceTraveled = 1.5707964f;
		for (;;)
		{
			base.transform.position = ChessBishopLevelBishop.calculateMoveHorizontalPosition(ref goalPos, ref xSpeed, ref distanceTraveled, amplitude, frequency, p.maxDistance);
			while (!this.canMove)
			{
				yield return null;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001832 RID: 6194 RVA: 0x000DA748 File Offset: 0x000D8B48
	private static Vector3 findMoveHorizontalInitialPosition(float minimumDistance, float yPosition, float maxDistance, float xSpeed, float amplitude, float frequency)
	{
		float num = minimumDistance * minimumDistance;
		List<Vector3> list = new List<Vector3>();
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerOne))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerOne).center);
		}
		if (PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo))
		{
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center);
		}
		int i = 0;
		Vector3 zero = Vector3.zero;
		while (i < 20)
		{
			i++;
			zero = new Vector3(UnityEngine.Random.Range(-maxDistance, maxDistance), yPosition);
			Vector3 vector = zero;
			float num2 = 1.5707964f;
			float num3 = xSpeed;
			ChessBishopLevelBishop.calculateMoveHorizontalPosition(ref vector, ref num3, ref num2, amplitude, frequency, maxDistance);
			bool flag = false;
			foreach (Vector3 b in list)
			{
				if ((zero - b).sqrMagnitude < num)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		return zero;
	}

	// Token: 0x06001833 RID: 6195 RVA: 0x000DA850 File Offset: 0x000D8C50
	private static Vector3 calculateMoveHorizontalPosition(ref Vector3 goalPosition, ref float xSpeed, ref float distanceTravelled, float amplitude, float frequency, float maxDistance)
	{
		Vector3 vector = goalPosition;
		vector.x += xSpeed * CupheadTime.FixedDelta;
		distanceTravelled += Mathf.Abs(xSpeed) * CupheadTime.FixedDelta;
		if (vector.x > maxDistance || vector.x < -maxDistance)
		{
			xSpeed *= -1f;
		}
		vector.y = amplitude * Mathf.Sin(frequency * distanceTravelled);
		goalPosition = vector;
		if (vector.x < -maxDistance + 100f || vector.x > maxDistance - 100f)
		{
			float num = Mathf.InverseLerp(maxDistance - 100f, maxDistance, Mathf.Abs(vector.x));
			num *= 1.5707964f;
			num = Mathf.Sin(num) * 100f / 2f;
			vector.x = (maxDistance - 100f + num) * Mathf.Sign(vector.x);
		}
		return vector;
	}

	// Token: 0x06001834 RID: 6196 RVA: 0x000DA94C File Offset: 0x000D8D4C
	private IEnumerator spawnProjectiles_cr()
	{
		while (!this.canMove)
		{
			yield return null;
		}
		LevelProperties.ChessBishop.Bishop p = base.properties.CurrentState.bishop;
		PatternString delayPattern = new PatternString(p.attackDelayString, true, true);
		for (;;)
		{
			float delay = delayPattern.PopFloat();
			yield return CupheadTime.WaitForSeconds(this, delay);
			this.bulletSpawnCoroutine = base.StartCoroutine(this.shoot_cr());
			while (this.bulletSpawnCoroutine != null)
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001835 RID: 6197 RVA: 0x000DA968 File Offset: 0x000D8D68
	private IEnumerator shoot_cr()
	{
		float previousTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		float currentTime = previousTime;
		while (previousTime >= 0.625f || currentTime <= 0.625f)
		{
			yield return null;
			previousTime = currentTime;
			currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("IdleActive") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("IsDormant"))
		{
			this.mainRenderer.enabled = false;
		}
		this.summonOverlayRenderer.enabled = true;
		previousTime = (currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
		while (previousTime <= currentTime)
		{
			yield return null;
			previousTime = currentTime;
			currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
		ChessBishopLevelBell bell = this.bellProjectile.Spawn<ChessBishopLevelBell>();
		this.SFX_KOG_Bishop_Shoot();
		bell.Init(this.projectileSpawnPoint.position, PlayerManager.GetNext(), base.properties.CurrentState.bishop);
		previousTime = (currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
		while (previousTime >= 0.525f || currentTime <= 0.525f)
		{
			yield return null;
			previousTime = currentTime;
			currentTime = MathUtilities.DecimalPart(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
		this.mainRenderer.enabled = true;
		this.summonOverlayRenderer.enabled = false;
		this.bulletSpawnCoroutine = null;
		yield break;
	}

	// Token: 0x06001836 RID: 6198 RVA: 0x000DA983 File Offset: 0x000D8D83
	private void cancelShoot()
	{
		if (this.bulletSpawnCoroutine != null)
		{
			base.StopCoroutine(this.bulletSpawnCoroutine);
			this.bulletSpawnCoroutine = null;
		}
		this.mainRenderer.enabled = true;
		this.summonOverlayRenderer.enabled = false;
	}

	// Token: 0x06001837 RID: 6199 RVA: 0x000DA9BC File Offset: 0x000D8DBC
	private void die()
	{
		if (this.dead)
		{
			return;
		}
		this.dead = true;
		this.bodyOpacity = 1f;
		this.UpdateBodyFade();
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		this.SFX_KOG_Bishop_Death();
		this.bodyAnimator.Play("Death");
		base.animator.Play("Death");
		base.animator.Update(0f);
	}

	// Token: 0x06001838 RID: 6200 RVA: 0x000DAA35 File Offset: 0x000D8E35
	private void setupPatternStrings()
	{
		this.invisibleTime = new PatternString(base.properties.CurrentState.bishop.invisibleTimeString, true);
	}

	// Token: 0x06001839 RID: 6201 RVA: 0x000DAA58 File Offset: 0x000D8E58
	private void AnimationEvent_SFX_KOG_Bishop_Wakeup()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_wakeup");
	}

	// Token: 0x0600183A RID: 6202 RVA: 0x000DAA64 File Offset: 0x000D8E64
	private void AnimationEvent_SFX_KOG_Bishop_HeadDisappearsFromBody()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_headdisappearsfrombody");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_headdisappearsfrombody");
	}

	// Token: 0x0600183B RID: 6203 RVA: 0x000DAA80 File Offset: 0x000D8E80
	private void AnimationEvent_SFX_KOG_Bishop_HeadReappears()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_headreappears");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_headreappears");
	}

	// Token: 0x0600183C RID: 6204 RVA: 0x000DAA9C File Offset: 0x000D8E9C
	private void SFX_KOG_Bishop_Shoot()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_shoot");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_shoot");
	}

	// Token: 0x0600183D RID: 6205 RVA: 0x000DAAB8 File Offset: 0x000D8EB8
	private void SFX_KOG_Bishop_Death()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_death");
		AudioManager.Play("sfx_level_knockout_boom");
	}

	// Token: 0x0600183E RID: 6206 RVA: 0x000DAACE File Offset: 0x000D8ECE
	private void AnimationEvent_SFX_KOG_Bishop_Vocal()
	{
		base.StartCoroutine(this.SFX_KOG_Bihop_Vocal_cr());
	}

	// Token: 0x0600183F RID: 6207 RVA: 0x000DAAE0 File Offset: 0x000D8EE0
	private IEnumerator SFX_KOG_Bihop_Vocal_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AudioManager.Play("sfx_dlc_kog_bishop_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_vocal");
		yield break;
	}

	// Token: 0x0400214F RID: 8527
	private const int MULTIPLAYER_CANDLE_DIFFERENCE = 3;

	// Token: 0x04002150 RID: 8528
	private const float VER_LOOPSIZEY = 150f;

	// Token: 0x04002151 RID: 8529
	private const float VER_LOOPSIZEX = 500f;

	// Token: 0x04002152 RID: 8530
	private const float H_EASE_DISTANCE = 100f;

	// Token: 0x04002153 RID: 8531
	[SerializeField]
	private ChessBishopLevelBell bellProjectile;

	// Token: 0x04002154 RID: 8532
	[SerializeField]
	private SpriteRenderer mainRenderer;

	// Token: 0x04002155 RID: 8533
	[SerializeField]
	private SpriteRenderer summonOverlayRenderer;

	// Token: 0x04002156 RID: 8534
	[SerializeField]
	private Transform projectileSpawnPoint;

	// Token: 0x04002157 RID: 8535
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x04002158 RID: 8536
	[SerializeField]
	private GameObject candlesHolder;

	// Token: 0x04002159 RID: 8537
	[SerializeField]
	private ChessBishopLevelCandle[] candles;

	// Token: 0x0400215A RID: 8538
	[SerializeField]
	private Animator bodyAnimator;

	// Token: 0x0400215B RID: 8539
	[SerializeField]
	private Effect bodyExplosion;

	// Token: 0x0400215C RID: 8540
	[SerializeField]
	private Transform bodyExplosionSpawnPoint;

	// Token: 0x0400215D RID: 8541
	[SerializeField]
	private SpriteRenderer bodyRenderer;

	// Token: 0x0400215E RID: 8542
	private float bodyOpacity = 1f;

	// Token: 0x0400215F RID: 8543
	[SerializeField]
	private float fadeRate = 0.75f;

	// Token: 0x04002160 RID: 8544
	[SerializeField]
	private GameObject[] playerMask;

	// Token: 0x04002161 RID: 8545
	private int candleOrderMainIndex;

	// Token: 0x04002162 RID: 8546
	private bool invert;

	// Token: 0x04002163 RID: 8547
	private bool isPathTwo;

	// Token: 0x04002164 RID: 8548
	private DamageDealer damageDealer;

	// Token: 0x04002165 RID: 8549
	private PatternString invisibleTime;

	// Token: 0x04002166 RID: 8550
	private bool canMove;

	// Token: 0x04002167 RID: 8551
	private bool isFirstPhase = true;

	// Token: 0x04002168 RID: 8552
	private bool stateDidChange;

	// Token: 0x04002169 RID: 8553
	private ChessBishopLevelBishop.DisappearingState disappearingState;

	// Token: 0x0400216A RID: 8554
	private bool dead;

	// Token: 0x0400216B RID: 8555
	private bool introPlaying = true;

	// Token: 0x0400216C RID: 8556
	private Coroutine bulletSpawnCoroutine;

	// Token: 0x02000537 RID: 1335
	private enum DisappearingState
	{
		// Token: 0x0400216E RID: 8558
		None,
		// Token: 0x0400216F RID: 8559
		Disappearing,
		// Token: 0x04002170 RID: 8560
		Reappearing
	}
}
