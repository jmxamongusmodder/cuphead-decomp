using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000895 RID: 2197
public class TreePlatformingLevelMosquito : AbstractCollidableObject
{
	// Token: 0x17000443 RID: 1091
	// (get) Token: 0x06003315 RID: 13077 RVA: 0x001DB307 File Offset: 0x001D9707
	// (set) Token: 0x06003316 RID: 13078 RVA: 0x001DB30F File Offset: 0x001D970F
	public bool isActive { get; private set; }

	// Token: 0x06003317 RID: 13079 RVA: 0x001DB318 File Offset: 0x001D9718
	private void Start()
	{
		this.startPos = base.transform.position;
		AudioManager.PlayLoop("level_platform_mosquito_loop");
		this.emitAudioFromObject.Add("level_platform_mosquito_loop");
		this.YPositionDown = this.YPositionUp - 30f;
		this.YFall = this.YPositionUp - 35f;
		this.endPos = base.transform.position;
		this.endPos.y = this.YPositionDown;
		base.StartCoroutine(this.delay_start_cr(UnityEngine.Random.Range(0f, 3f)));
	}

	// Token: 0x06003318 RID: 13080 RVA: 0x001DB3B4 File Offset: 0x001D97B4
	private IEnumerator delay_start_cr(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.StartCoroutine(this.activate_cr());
		yield break;
	}

	// Token: 0x06003319 RID: 13081 RVA: 0x001DB3D6 File Offset: 0x001D97D6
	private void SetLetters(int one, int two)
	{
		base.animator.SetInteger("FirstLetter", one);
		base.animator.SetInteger("SecondLetter", two);
	}

	// Token: 0x0600331A RID: 13082 RVA: 0x001DB3FC File Offset: 0x001D97FC
	private IEnumerator check_platform_cr()
	{
		for (;;)
		{
			while (this.platform.transform.childCount <= 0)
			{
				yield return null;
			}
			this.StopMoveCoroutines();
			base.StartCoroutine(this.fall_cr());
			base.animator.SetBool("Struggling", true);
			AudioManager.Play("level_platform_mosquito_step_on");
			this.emitAudioFromObject.Add("level_platform_mosquito_step_on");
			AudioManager.Stop("level_platform_mosquito_loop");
			AudioManager.PlayLoop("level_platform_mosquito_struggle_loop");
			this.emitAudioFromObject.Add("level_platform_mosquito_struggle_loop");
			if (!this.projectileShooting)
			{
				base.StartCoroutine(this.shoot_up_cr());
			}
			while (this.platform.transform.childCount > 0)
			{
				yield return null;
			}
			this.StopMoveCoroutines();
			this.StartUp();
			base.animator.SetBool("Struggling", false);
			AudioManager.Stop("level_platform_mosquito_struggle_loop");
			AudioManager.PlayLoop("level_platform_mosquito_loop");
			this.emitAudioFromObject.Add("level_platform_mosquito_loop");
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600331B RID: 13083 RVA: 0x001DB417 File Offset: 0x001D9817
	protected override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemyProjectile(hit, phase);
		if (hit.GetComponent<TreePlatformingLevelDragonflyProjectile>())
		{
			this.KillPlatform();
		}
	}

	// Token: 0x0600331C RID: 13084 RVA: 0x001DB438 File Offset: 0x001D9838
	private IEnumerator activate_cr()
	{
		base.animator.Play("Pick_Type");
		base.transform.position = new Vector3(this.startPos.x, 1200f);
		float t = 0f;
		base.GetComponent<Collider2D>().enabled = true;
		this.platform.gameObject.SetActive(true);
		while (t < this.returnTime)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / this.returnTime);
			base.transform.position = Vector2.Lerp(base.transform.position, this.startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.isActive = true;
		base.transform.position = this.startPos;
		this.StartDown();
		base.StartCoroutine(this.check_platform_cr());
		switch (this.type)
		{
		case TreePlatformingLevelMosquito.Type.AA:
			this.SetLetters(1, 1);
			break;
		case TreePlatformingLevelMosquito.Type.AB:
			this.SetLetters(1, 2);
			break;
		case TreePlatformingLevelMosquito.Type.AC:
			this.SetLetters(1, 3);
			break;
		case TreePlatformingLevelMosquito.Type.BA:
			this.SetLetters(2, 1);
			break;
		case TreePlatformingLevelMosquito.Type.BB:
			this.SetLetters(2, 2);
			break;
		case TreePlatformingLevelMosquito.Type.BC:
			this.SetLetters(2, 3);
			break;
		case TreePlatformingLevelMosquito.Type.CA:
			this.SetLetters(3, 1);
			break;
		case TreePlatformingLevelMosquito.Type.CB:
			this.SetLetters(3, 2);
			break;
		case TreePlatformingLevelMosquito.Type.CC:
			this.SetLetters(3, 3);
			break;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600331D RID: 13085 RVA: 0x001DB454 File Offset: 0x001D9854
	public void KillPlatform()
	{
		if (this.explosion != null)
		{
			this.explosion.Create(base.transform.position, new Vector3(0.85f, 0.85f, 0.85f));
		}
		this.platform.transform.DetachChildren();
		this.platform.gameObject.SetActive(false);
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetBool("Struggling", false);
		AudioManager.Stop("level_platform_mosquito_loop");
		AudioManager.Stop("level_platform_mosquito_struggle_loop");
		AudioManager.Play("level_platform_mosquito_death");
		this.emitAudioFromObject.Add("level_platform_mosquito_death");
		this.isActive = false;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x0600331E RID: 13086 RVA: 0x001DB524 File Offset: 0x001D9924
	private IEnumerator die_cr()
	{
		base.animator.SetTrigger("Death");
		float velocity = 0f;
		float gravity = 2250f;
		while (base.transform.position.y > -CupheadLevelCamera.Current.Height - 200f)
		{
			base.transform.AddPosition(0f, velocity * CupheadTime.Delta, 0f);
			velocity -= gravity * CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.reappearDelay);
		this.projectileShooting = false;
		base.StartCoroutine(this.activate_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600331F RID: 13087 RVA: 0x001DB540 File Offset: 0x001D9940
	public IEnumerator sine_cr()
	{
		float time = UnityEngine.Random.Range(1f, 1.5f);
		float t = UnityEngine.Random.Range(0f, 0.5f);
		float val = 0.5f;
		for (;;)
		{
			if (CupheadTime.Delta != 0f)
			{
				t += CupheadTime.Delta;
				float num = Mathf.Sin(t / time);
				base.transform.AddPosition(0f, num * val, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003320 RID: 13088 RVA: 0x001DB55C File Offset: 0x001D995C
	private IEnumerator shoot_up_cr()
	{
		this.projectileShooting = true;
		yield return CupheadTime.WaitForSeconds(this, this.projectileShootUpTime);
		if (this.projectile != null)
		{
			this.projectile.Create(new Vector2(base.transform.position.x, base.transform.position.y - 500f), 90f, this.projectileSpeed);
		}
		this.projectileShooting = false;
		yield return null;
		yield break;
	}

	// Token: 0x06003321 RID: 13089 RVA: 0x001DB578 File Offset: 0x001D9978
	private void StopMoveCoroutines()
	{
		if (this.upCoroutine != null)
		{
			base.StopCoroutine(this.upCoroutine);
			this.upCoroutine = null;
		}
		if (this.downCoroutine != null)
		{
			base.StopCoroutine(this.downCoroutine);
			this.downCoroutine = null;
		}
		if (this.fallCoroutine != null)
		{
			base.StopCoroutine(this.fallCoroutine);
			this.fallCoroutine = null;
		}
		if (this.gotoCoroutine != null)
		{
			base.StopCoroutine(this.gotoCoroutine);
			this.gotoCoroutine = null;
		}
	}

	// Token: 0x06003322 RID: 13090 RVA: 0x001DB5FD File Offset: 0x001D99FD
	public void StartDown()
	{
		this.StopMoveCoroutines();
		this.downCoroutine = base.StartCoroutine(this.down_cr());
	}

	// Token: 0x06003323 RID: 13091 RVA: 0x001DB617 File Offset: 0x001D9A17
	public void StartUp()
	{
		this.StopMoveCoroutines();
		this.upCoroutine = base.StartCoroutine(this.up_cr());
	}

	// Token: 0x06003324 RID: 13092 RVA: 0x001DB634 File Offset: 0x001D9A34
	private IEnumerator down_cr()
	{
		yield return new WaitForSeconds(0f);
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(this.YPositionUp, this.YPositionDown, 1.5f, EaseUtils.EaseType.easeInOutSine));
		yield return this.gotoCoroutine;
		this.StartUp();
		yield break;
	}

	// Token: 0x06003325 RID: 13093 RVA: 0x001DB650 File Offset: 0x001D9A50
	private IEnumerator up_cr()
	{
		yield return new WaitForSeconds(0f);
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(this.YPositionDown, this.YPositionUp, 1.5f, EaseUtils.EaseType.easeInOutSine));
		yield return this.gotoCoroutine;
		this.StartDown();
		yield break;
	}

	// Token: 0x06003326 RID: 13094 RVA: 0x001DB66C File Offset: 0x001D9A6C
	private IEnumerator fall_cr()
	{
		float time = (1f - (base.transform.position.y - this.startPos.y) / this.YFall) * 0.13f;
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(base.transform.position.y - this.startPos.y, this.YFall, time, EaseUtils.EaseType.easeOutSine));
		yield return this.gotoCoroutine;
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(this.YFall, this.YPositionDown, 0.12f, EaseUtils.EaseType.easeInOutSine));
		yield return this.gotoCoroutine;
		yield break;
	}

	// Token: 0x06003327 RID: 13095 RVA: 0x001DB688 File Offset: 0x001D9A88
	private IEnumerator goTo_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		base.transform.SetPosition(null, new float?(this.startPos.y + start), null);
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(null, new float?(this.startPos.y + EaseUtils.Ease(ease, start, end, val)), null);
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.SetPosition(null, new float?(this.startPos.y + end), null);
		yield break;
	}

	// Token: 0x04003B42 RID: 15170
	[Header("Projectile Variables")]
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04003B43 RID: 15171
	[SerializeField]
	private float projectileSpeed;

	// Token: 0x04003B44 RID: 15172
	[SerializeField]
	private bool projectileShootsUP;

	// Token: 0x04003B45 RID: 15173
	[SerializeField]
	private float projectileShootUpTime;

	// Token: 0x04003B46 RID: 15174
	[Space(10f)]
	[SerializeField]
	private LevelPlatform platform;

	// Token: 0x04003B47 RID: 15175
	[SerializeField]
	private float reappearDelay = 1f;

	// Token: 0x04003B48 RID: 15176
	[SerializeField]
	private PlatformingLevelGenericExplosion explosion;

	// Token: 0x04003B49 RID: 15177
	public float returnTime = 1.5f;

	// Token: 0x04003B4B RID: 15179
	private bool projectileShooting;

	// Token: 0x04003B4C RID: 15180
	public TreePlatformingLevelMosquito.Type type;

	// Token: 0x04003B4D RID: 15181
	public float YPositionUp;

	// Token: 0x04003B4E RID: 15182
	public const float TIME = 1.5f;

	// Token: 0x04003B4F RID: 15183
	public const float FALL_TIME = 0.13f;

	// Token: 0x04003B50 RID: 15184
	public const float FALL_BOUNCE_TIME = 0.12f;

	// Token: 0x04003B51 RID: 15185
	public const float DELAY = 0f;

	// Token: 0x04003B52 RID: 15186
	public const EaseUtils.EaseType FLOAT_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x04003B53 RID: 15187
	public const EaseUtils.EaseType FALL_EASE = EaseUtils.EaseType.easeOutSine;

	// Token: 0x04003B54 RID: 15188
	public const EaseUtils.EaseType FALL_BOUNCE_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x04003B55 RID: 15189
	[SerializeField]
	private TreePlatformingLevelMosquito.State state;

	// Token: 0x04003B56 RID: 15190
	private Vector3 startPos;

	// Token: 0x04003B57 RID: 15191
	private Vector3 endPos;

	// Token: 0x04003B58 RID: 15192
	private float YPositionDown;

	// Token: 0x04003B59 RID: 15193
	private float YFall;

	// Token: 0x04003B5A RID: 15194
	private Coroutine upCoroutine;

	// Token: 0x04003B5B RID: 15195
	private Coroutine downCoroutine;

	// Token: 0x04003B5C RID: 15196
	private Coroutine fallCoroutine;

	// Token: 0x04003B5D RID: 15197
	private Coroutine gotoCoroutine;

	// Token: 0x02000896 RID: 2198
	public enum Type
	{
		// Token: 0x04003B5F RID: 15199
		AA,
		// Token: 0x04003B60 RID: 15200
		AB,
		// Token: 0x04003B61 RID: 15201
		AC,
		// Token: 0x04003B62 RID: 15202
		BA,
		// Token: 0x04003B63 RID: 15203
		BB,
		// Token: 0x04003B64 RID: 15204
		BC,
		// Token: 0x04003B65 RID: 15205
		CA,
		// Token: 0x04003B66 RID: 15206
		CB,
		// Token: 0x04003B67 RID: 15207
		CC
	}

	// Token: 0x02000897 RID: 2199
	public enum State
	{
		// Token: 0x04003B69 RID: 15209
		Up,
		// Token: 0x04003B6A RID: 15210
		Down,
		// Token: 0x04003B6B RID: 15211
		PlayerOn
	}
}
