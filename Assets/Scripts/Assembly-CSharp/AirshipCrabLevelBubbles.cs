using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004CE RID: 1230
public class AirshipCrabLevelBubbles : AbstractProjectile
{
	// Token: 0x060014ED RID: 5357 RVA: 0x000BBA94 File Offset: 0x000B9E94
	public void Init(Vector2 pos, LevelProperties.AirshipCrab.Bubbles properties, float speed)
	{
		this.properties = properties;
		base.transform.position = pos;
		this.speed = speed;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x000BBAC2 File Offset: 0x000B9EC2
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x000BBAE0 File Offset: 0x000B9EE0
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000BBAF8 File Offset: 0x000B9EF8
	private IEnumerator move_cr()
	{
		this.speedY = this.properties.sinWaveStrength;
		float t = UnityEngine.Random.Range(0f, 6.2831855f);
		while (base.transform.position.x > -640f)
		{
			Vector3 pos = base.transform.position;
			pos.x = Mathf.MoveTowards(base.transform.position.x, -640f, this.speed * CupheadTime.Delta);
			base.transform.position = new Vector3(pos.x, base.transform.position.y + Mathf.Sin(t) * this.speedY * CupheadTime.Delta * 60f, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x000BBB13 File Offset: 0x000B9F13
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001E4C RID: 7756
	private LevelProperties.AirshipCrab.Bubbles properties;

	// Token: 0x04001E4D RID: 7757
	private float speed;

	// Token: 0x04001E4E RID: 7758
	private float speedY;
}
