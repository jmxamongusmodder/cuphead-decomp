using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200065E RID: 1630
public class FlyingCowboyLevelSnake : AbstractProjectile
{
	// Token: 0x060021F1 RID: 8689 RVA: 0x0013C37A File Offset: 0x0013A77A
	public void Move(Vector3 position, float speedX, float speedY, float stopPosX, float gravity, LevelProperties.FlyingCowboy.SnakeAttack properties)
	{
		base.transform.position = position;
		this.properties = properties;
		this.speed = new Vector3(speedX, speedY);
		this.stopPosX = stopPosX;
		this.gravity = gravity;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x0013C3BA File Offset: 0x0013A7BA
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060021F3 RID: 8691 RVA: 0x0013C3D8 File Offset: 0x0013A7D8
	private IEnumerator move_cr()
	{
		while (base.transform.position.x < this.stopPosX)
		{
			this.speed += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
			base.transform.Translate(this.speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		BasicProjectile snake = this.snakeLine.Create(base.transform.position, 0f, -this.properties.snakeSpeed);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002AA7 RID: 10919
	[SerializeField]
	private BasicProjectile snakeLine;

	// Token: 0x04002AA8 RID: 10920
	private LevelProperties.FlyingCowboy.SnakeAttack properties;

	// Token: 0x04002AA9 RID: 10921
	private Vector3 speed;

	// Token: 0x04002AAA RID: 10922
	private float gravity;

	// Token: 0x04002AAB RID: 10923
	private float stopPosX;
}
