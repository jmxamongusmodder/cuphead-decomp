using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F5 RID: 1525
public class DragonLevelMeteor : AbstractProjectile
{
	// Token: 0x06001E60 RID: 7776 RVA: 0x0011849C File Offset: 0x0011689C
	public DragonLevelMeteor Create(Vector2 pos, DragonLevelMeteor.Properties properties)
	{
		DragonLevelMeteor dragonLevelMeteor = base.Create() as DragonLevelMeteor;
		dragonLevelMeteor.properties = properties;
		dragonLevelMeteor.transform.position = pos;
		return dragonLevelMeteor;
	}

	// Token: 0x06001E61 RID: 7777 RVA: 0x001184D0 File Offset: 0x001168D0
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.smoke_cr());
		base.StartCoroutine(this.moveX_cr());
		if (this.properties.state != DragonLevelMeteor.State.Forward)
		{
			base.StartCoroutine(this.moveY_cr());
		}
		base.StartCoroutine(this.rotate_cr());
		AudioManager.PlayLoop("level_dragon_left_dragon_meteor_a_loop");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_a_loop");
	}

	// Token: 0x06001E62 RID: 7778 RVA: 0x00118543 File Offset: 0x00116943
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E63 RID: 7779 RVA: 0x00118564 File Offset: 0x00116964
	private IEnumerator rotate_cr()
	{
		Vector2 lastPos = base.transform.position;
		for (;;)
		{
			base.transform.LookAt2D(lastPos);
			lastPos = base.transform.position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E64 RID: 7780 RVA: 0x00118580 File Offset: 0x00116980
	private IEnumerator smoke_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			this.smokePrefab.Create(base.transform.position).transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(base.transform.eulerAngles.z + UnityEngine.Random.Range(-45f, 45f)));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E65 RID: 7781 RVA: 0x0011859C File Offset: 0x0011699C
	private IEnumerator moveX_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > -840f)
		{
			base.transform.AddPosition(-this.properties.speedX * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x001185B8 File Offset: 0x001169B8
	private IEnumerator moveY_cr()
	{
		int state = (int)this.properties.state;
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(start.x - this.properties.speedX / 2f, 300f * (float)state);
		yield return base.TweenPositionY(start.y, end.y, this.properties.timeY / 2f, EaseUtils.EaseType.easeOutSine);
		for (;;)
		{
			state *= -1;
			start = base.transform.position;
			end = new Vector2(start.x - this.properties.speedX, 300f * (float)state);
			yield return base.TweenPositionY(start.y, end.y, this.properties.timeY, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x001185D3 File Offset: 0x001169D3
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		AudioManager.Stop("level_dragon_left_dragon_meteor_a_loop");
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x001185EB File Offset: 0x001169EB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.smokePrefab = null;
	}

	// Token: 0x0400273E RID: 10046
	private DragonLevelMeteor.Properties properties;

	// Token: 0x0400273F RID: 10047
	[SerializeField]
	private Effect smokePrefab;

	// Token: 0x020005F6 RID: 1526
	public enum State
	{
		// Token: 0x04002741 RID: 10049
		Up = 1,
		// Token: 0x04002742 RID: 10050
		Down = -1,
		// Token: 0x04002743 RID: 10051
		Both,
		// Token: 0x04002744 RID: 10052
		Forward = 10
	}

	// Token: 0x020005F7 RID: 1527
	public class Properties
	{
		// Token: 0x06001E69 RID: 7785 RVA: 0x001185FA File Offset: 0x001169FA
		public Properties(float timeY, float speedX, DragonLevelMeteor.State state)
		{
			this.timeY = timeY;
			this.speedX = speedX;
			this.state = state;
		}

		// Token: 0x04002745 RID: 10053
		public float timeY;

		// Token: 0x04002746 RID: 10054
		public float speedX;

		// Token: 0x04002747 RID: 10055
		public DragonLevelMeteor.State state;
	}
}
