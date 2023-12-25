using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000671 RID: 1649
public class FlyingGenieLevelMummy : BasicProjectile
{
	// Token: 0x17000397 RID: 919
	// (get) Token: 0x060022B0 RID: 8880 RVA: 0x00145C68 File Offset: 0x00144068
	// (set) Token: 0x060022B1 RID: 8881 RVA: 0x00145C70 File Offset: 0x00144070
	public FlyingGenieLevelMummy.MummyType type { get; private set; }

	// Token: 0x060022B2 RID: 8882 RVA: 0x00145C7C File Offset: 0x0014407C
	public FlyingGenieLevelMummy Create(Vector3 position, float speed, float rotation, LevelProperties.FlyingGenie.Coffin properties, FlyingGenieLevelMummy.MummyType type, float hp, int sortingOrder)
	{
		FlyingGenieLevelMummy flyingGenieLevelMummy = base.Create(position, rotation, speed) as FlyingGenieLevelMummy;
		flyingGenieLevelMummy.transform.position = position;
		flyingGenieLevelMummy.properties = properties;
		flyingGenieLevelMummy.type = type;
		flyingGenieLevelMummy.hp = hp;
		flyingGenieLevelMummy.rotation = rotation;
		flyingGenieLevelMummy.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
		flyingGenieLevelMummy.purpleSprite.sortingOrder = sortingOrder + 1;
		flyingGenieLevelMummy.purpleColor = this.purpleSprite.color;
		return flyingGenieLevelMummy;
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x00145CF6 File Offset: 0x001440F6
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x00145D24 File Offset: 0x00144124
	protected override void Start()
	{
		base.Start();
		AudioManager.Play("genie_mummy_voice_attack");
		this.emitAudioFromObject.Add("genie_mummy_voice_attack");
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.fade_purple_cr());
		FlyingGenieLevelMummy.MummyType type = this.type;
		if (type != FlyingGenieLevelMummy.MummyType.Classic)
		{
			if (type != FlyingGenieLevelMummy.MummyType.Chomper)
			{
				if (type == FlyingGenieLevelMummy.MummyType.Grabby)
				{
					base.animator.Play("Grabby");
				}
			}
			else
			{
				base.animator.Play("Chomper");
			}
		}
		else
		{
			this.CalculateSin();
			if (this.properties.mummyASinWave)
			{
				base.StartCoroutine(this.classic_bounce_cr());
			}
			base.animator.Play("Classic");
		}
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x00145E04 File Offset: 0x00144204
	private IEnumerator fade_purple_cr()
	{
		float t = 0f;
		float time = 1.5f;
		Color start = this.purpleSprite.color;
		Color end = new Color(1f, 1f, 1f, 0f);
		while (t < time)
		{
			this.purpleSprite.color = Color.Lerp(start, end, t / time);
			this.purpleColor = this.purpleSprite.color;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x00145E20 File Offset: 0x00144220
	private void CalculateSin()
	{
		Vector3 vector = MathUtils.AngleToDirection(this.rotation);
		Vector2 zero = Vector2.zero;
		zero.x = (vector.x + base.transform.position.x) / 2f;
		zero.y = (vector.y + base.transform.position.y) / 2f;
		float num = -((vector.x - base.transform.position.x) / (vector.y - base.transform.position.y));
		float num2 = zero.y - num * zero.x;
		Vector2 zero2 = Vector2.zero;
		zero2.x = zero.x + 1f;
		zero2.y = num * zero2.x + num2;
		this.normalized = Vector3.zero;
		this.normalized = zero2 - zero;
		this.normalized.Normalize();
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x00145F3C File Offset: 0x0014433C
	private IEnumerator classic_bounce_cr()
	{
		Vector3 pos = base.transform.position;
		float angle = 0f;
		for (;;)
		{
			angle += 10f * CupheadTime.Delta;
			if (CupheadTime.Delta != 0f)
			{
				pos = this.normalized * Mathf.Sin(angle) * 2f;
				base.transform.position += pos;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x00145F58 File Offset: 0x00144358
	private IEnumerator grabby_speed_cr()
	{
		float t = 0f;
		float time = 0.5f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.Speed = Mathf.Lerp(-this.properties.mummyCSpeed, 0f, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x00145F73 File Offset: 0x00144373
	private void ChangeSpeed()
	{
		if (this.properties.mummyCSlowdown)
		{
			this.Speed = -this.properties.mummyCSpeed;
			base.StartCoroutine(this.grabby_speed_cr());
		}
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x00145FA4 File Offset: 0x001443A4
	protected override void Die()
	{
		this.StopAllCoroutines();
		this.Explosion();
		base.Die();
		AudioManager.Stop("genie_mummy_voice_attack");
		AudioManager.Play("genie_mummy_voice_die");
		this.emitAudioFromObject.Add("genie_mummy_voice_die");
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x00145FFF File Offset: 0x001443FF
	private void Explosion()
	{
		this.sparkFX.Create(base.transform.position, this.purpleColor);
	}

	// Token: 0x04002B4E RID: 11086
	[SerializeField]
	private FlyingGenieLevelMummyDeathEffect sparkFX;

	// Token: 0x04002B4F RID: 11087
	[SerializeField]
	private SpriteRenderer purpleSprite;

	// Token: 0x04002B51 RID: 11089
	private LevelProperties.FlyingGenie.Coffin properties;

	// Token: 0x04002B52 RID: 11090
	private Vector3 normalized;

	// Token: 0x04002B53 RID: 11091
	private DamageReceiver damageReceiver;

	// Token: 0x04002B54 RID: 11092
	private float hp;

	// Token: 0x04002B55 RID: 11093
	private float rotation;

	// Token: 0x04002B56 RID: 11094
	private Color purpleColor;

	// Token: 0x02000672 RID: 1650
	public enum MummyType
	{
		// Token: 0x04002B58 RID: 11096
		Classic,
		// Token: 0x04002B59 RID: 11097
		Chomper,
		// Token: 0x04002B5A RID: 11098
		Grabby
	}
}
