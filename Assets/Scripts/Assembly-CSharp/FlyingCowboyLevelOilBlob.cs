using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000656 RID: 1622
public class FlyingCowboyLevelOilBlob : AbstractProjectile
{
	// Token: 0x060021CE RID: 8654 RVA: 0x0013B1FC File Offset: 0x001395FC
	public FlyingCowboyLevelOilBlob Create(Vector3 position, float finalYPosition, float snakeSpawnX, LevelProperties.FlyingCowboy.SnakeAttack properties, bool playSplatSFX)
	{
		FlyingCowboyLevelOilBlob flyingCowboyLevelOilBlob = base.Create(position) as FlyingCowboyLevelOilBlob;
		flyingCowboyLevelOilBlob.initialYPosition = position.y;
		flyingCowboyLevelOilBlob.finalYPosition = finalYPosition;
		float f = flyingCowboyLevelOilBlob.finalYPosition - flyingCowboyLevelOilBlob.initialYPosition;
		float num = Mathf.Abs(f);
		if (num >= FlyingCowboyLevelOilBlob.BlobCHeight)
		{
			flyingCowboyLevelOilBlob.animator.Play((!Rand.Bool()) ? "F" : "C");
			flyingCowboyLevelOilBlob.finalYPosition -= Mathf.Sign(f) * FlyingCowboyLevelOilBlob.BlobCHeight;
		}
		else if (num >= FlyingCowboyLevelOilBlob.BlobBHeight)
		{
			flyingCowboyLevelOilBlob.animator.Play("B");
			flyingCowboyLevelOilBlob.finalYPosition -= Mathf.Sign(f) * FlyingCowboyLevelOilBlob.BlobBHeight;
		}
		else
		{
			flyingCowboyLevelOilBlob.animator.Play("A");
		}
		if (flyingCowboyLevelOilBlob.finalYPosition < flyingCowboyLevelOilBlob.initialYPosition)
		{
			Vector3 localScale = flyingCowboyLevelOilBlob.transform.localScale;
			localScale.y *= -1f;
			flyingCowboyLevelOilBlob.transform.localScale = localScale;
		}
		flyingCowboyLevelOilBlob.StartCoroutine(flyingCowboyLevelOilBlob.snakeSpawn_cr(snakeSpawnX, finalYPosition, properties, playSplatSFX));
		return flyingCowboyLevelOilBlob;
	}

	// Token: 0x060021CF RID: 8655 RVA: 0x0013B32A File Offset: 0x0013972A
	protected override void Awake()
	{
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060021D0 RID: 8656 RVA: 0x0013B338 File Offset: 0x00139738
	private void LateUpdate()
	{
		if (this.spriteRenderer.sprite != this.previousSprite)
		{
			this.previousSprite = this.spriteRenderer.sprite;
			this.frameCounter++;
		}
		Vector3 position = base.transform.position;
		position.y = Mathf.Lerp(this.initialYPosition, this.finalYPosition, (float)this.frameCounter / 28f);
		base.transform.position = position;
	}

	// Token: 0x060021D1 RID: 8657 RVA: 0x0013B3BC File Offset: 0x001397BC
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060021D2 RID: 8658 RVA: 0x0013B3DC File Offset: 0x001397DC
	private IEnumerator snakeSpawn_cr(float snakeSpawnX, float snakeSpawnY, LevelProperties.FlyingCowboy.SnakeAttack properties, bool playSplatSFX)
	{
		yield return null;
		yield return null;
		yield return base.animator.WaitForNormalizedTime(this, 1f, null, 0, false, false, true);
		BasicProjectile snake = this.snakePrefab.Create(new Vector2(snakeSpawnX + FlyingCowboyLevelOilBlob.SnakeSpawnOffsetX, snakeSpawnY), 0f, -properties.snakeSpeed);
		snake.animator.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
		this.splatEffect.Create(new Vector2(640f, snakeSpawnY));
		if (playSplatSFX)
		{
			AudioManager.Play("sfx_DLC_Cowgirl_P1_LiquidSplat");
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002A81 RID: 10881
	private static readonly float BlobBHeight = 79f;

	// Token: 0x04002A82 RID: 10882
	private static readonly float BlobCHeight = 293f;

	// Token: 0x04002A83 RID: 10883
	private static readonly float SnakeSpawnOffsetX = 130f;

	// Token: 0x04002A84 RID: 10884
	[SerializeField]
	private BasicProjectile snakePrefab;

	// Token: 0x04002A85 RID: 10885
	[SerializeField]
	private Effect splatEffect;

	// Token: 0x04002A86 RID: 10886
	private SpriteRenderer spriteRenderer;

	// Token: 0x04002A87 RID: 10887
	private Sprite previousSprite;

	// Token: 0x04002A88 RID: 10888
	private float initialYPosition;

	// Token: 0x04002A89 RID: 10889
	private float finalYPosition;

	// Token: 0x04002A8A RID: 10890
	private int frameCounter;
}
