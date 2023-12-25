using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000876 RID: 2166
public class ForestPlatformingLevelAcorn : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600324D RID: 12877 RVA: 0x001D4D9C File Offset: 0x001D319C
	public ForestPlatformingLevelAcorn Spawn(ForestPlatformingLevelAcornMaker parent, Vector2 position, ForestPlatformingLevelAcorn.Direction direction, bool moveUpFirst)
	{
		ForestPlatformingLevelAcorn forestPlatformingLevelAcorn = this.InstantiatePrefab<ForestPlatformingLevelAcorn>();
		forestPlatformingLevelAcorn.transform.position = position;
		forestPlatformingLevelAcorn._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		forestPlatformingLevelAcorn._direction = direction;
		forestPlatformingLevelAcorn._player = PlayerManager.GetNext();
		forestPlatformingLevelAcorn.parent = parent;
		if (moveUpFirst)
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.move_up_cr());
		}
		else
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.main_cr());
		}
		return forestPlatformingLevelAcorn;
	}

	// Token: 0x0600324E RID: 12878 RVA: 0x001D4E08 File Offset: 0x001D3208
	public ForestPlatformingLevelAcorn Spawn(Vector2 position, ForestPlatformingLevelAcorn.Direction direction, bool moveUpFirst)
	{
		ForestPlatformingLevelAcorn forestPlatformingLevelAcorn = this.InstantiatePrefab<ForestPlatformingLevelAcorn>();
		forestPlatformingLevelAcorn.transform.position = position;
		forestPlatformingLevelAcorn._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant;
		forestPlatformingLevelAcorn._direction = direction;
		forestPlatformingLevelAcorn._player = PlayerManager.GetNext();
		if (moveUpFirst)
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.move_up_cr());
		}
		else
		{
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.main_cr());
		}
		return forestPlatformingLevelAcorn;
	}

	// Token: 0x0600324F RID: 12879 RVA: 0x001D4E6C File Offset: 0x001D326C
	protected override void Awake()
	{
		base.Awake();
		AudioManager.PlayLoop("level_acorn_fly");
		this.emitAudioFromObject.Add("level_acorn_fly");
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x001D4E90 File Offset: 0x001D3290
	protected override void Start()
	{
		base.Start();
		if (this.parent != null)
		{
			ForestPlatformingLevelAcornMaker forestPlatformingLevelAcornMaker = this.parent;
			forestPlatformingLevelAcornMaker.killAcorns = (Action)Delegate.Combine(forestPlatformingLevelAcornMaker.killAcorns, new Action(this.Kill));
			base.StartCoroutine(this.acorn_death_timer_cr());
		}
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x001D4EE8 File Offset: 0x001D32E8
	protected override void OnStart()
	{
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x001D4EEC File Offset: 0x001D32EC
	protected override void Update()
	{
		base.Update();
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position) && !this._enteredScreen)
		{
			this._enteredScreen = true;
		}
		if (this._enteredScreen && !CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 100f)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		if (base.transform.position.x < (float)PlatformingLevel.Current.Left - 100f || base.transform.position.x > (float)PlatformingLevel.Current.Right + 100f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		base.transform.SetScale(new float?((float)((this._direction != ForestPlatformingLevelAcorn.Direction.Left) ? -1 : 1)), null, null);
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x001D5008 File Offset: 0x001D3408
	private IEnumerator move_up_cr()
	{
		float yOffset = 100f;
		while (base.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax - yOffset)
		{
			base.transform.AddPosition(0f, base.Properties.AcornFlySpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		base.StartCoroutine(this.main_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x001D5024 File Offset: 0x001D3424
	private IEnumerator main_cr()
	{
		while ((this._direction == ForestPlatformingLevelAcorn.Direction.Left && base.transform.position.x > this._player.center.x) || (this._direction == ForestPlatformingLevelAcorn.Direction.Right && base.transform.position.x < this._player.center.x))
		{
			base.transform.AddPosition((this._direction != ForestPlatformingLevelAcorn.Direction.Right) ? (-base.Properties.AcornFlySpeed * CupheadTime.FixedDelta) : (base.Properties.AcornFlySpeed * CupheadTime.FixedDelta), 0f, 0f);
			yield return new WaitForFixedUpdate();
			if (this._player == null || this._player.IsDead)
			{
				this._player = PlayerManager.GetNext();
			}
		}
		base.animator.SetTrigger("Drop");
		AudioManager.Stop("level_acorn_fly");
		AudioManager.Play("level_acorn_drop");
		this.emitAudioFromObject.Add("level_acorn_drop");
		float t = 0f;
		this._hasDropped = true;
		this.LaunchPropeller();
		while (t < 0.5f)
		{
			base.transform.AddPosition(0f, -base.Properties.AcornDropSpeed * CupheadTime.FixedDelta * t / 0.5f, 0f);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		for (;;)
		{
			base.transform.AddPosition(0f, -base.Properties.AcornDropSpeed * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x001D5040 File Offset: 0x001D3440
	private IEnumerator acorn_death_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		ForestPlatformingLevelAcornMaker forestPlatformingLevelAcornMaker = this.parent;
		forestPlatformingLevelAcornMaker.killAcorns = (Action)Delegate.Remove(forestPlatformingLevelAcornMaker.killAcorns, new Action(this.Kill));
		yield return null;
		yield break;
	}

	// Token: 0x06003256 RID: 12886 RVA: 0x001D505B File Offset: 0x001D345B
	private void LaunchPropeller()
	{
		this.propellerPrefab.Create(base.transform.position, base.Properties.AcornPropellerSpeed);
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x001D5084 File Offset: 0x001D3484
	protected override void Die()
	{
		if (!this._hasDropped)
		{
			this.LaunchPropeller();
			AudioManager.Stop("level_acorn_fly");
		}
		else
		{
			AudioManager.Stop("level_acorn_drop");
		}
		AudioManager.Play("level_flowergrunt_death");
		this.emitAudioFromObject.Add("level_flowergrunt_death");
		base.Die();
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x001D50DB File Offset: 0x001D34DB
	private void Kill()
	{
		base.Die();
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x001D50E3 File Offset: 0x001D34E3
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.propellerPrefab = null;
	}

	// Token: 0x04003AAF RID: 15023
	[SerializeField]
	private ForestPlatformingLevelAcornPropeller propellerPrefab;

	// Token: 0x04003AB0 RID: 15024
	private const float SCREEN_PADDING = 100f;

	// Token: 0x04003AB1 RID: 15025
	private const float DROP_EASE_TIME = 0.5f;

	// Token: 0x04003AB2 RID: 15026
	private ForestPlatformingLevelAcorn.Direction _direction;

	// Token: 0x04003AB3 RID: 15027
	private AbstractPlayerController _player;

	// Token: 0x04003AB4 RID: 15028
	private bool _hasDropped;

	// Token: 0x04003AB5 RID: 15029
	private bool _enteredScreen;

	// Token: 0x04003AB6 RID: 15030
	private ForestPlatformingLevelAcornMaker parent;

	// Token: 0x02000877 RID: 2167
	public enum Direction
	{
		// Token: 0x04003AB8 RID: 15032
		Left,
		// Token: 0x04003AB9 RID: 15033
		Right
	}
}
