using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200055F RID: 1375
public class ClownLevelClownSwing : LevelProperties.Clown.Entity
{
	// Token: 0x17000348 RID: 840
	// (get) Token: 0x060019D4 RID: 6612 RVA: 0x000EBCE0 File Offset: 0x000EA0E0
	// (set) Token: 0x060019D5 RID: 6613 RVA: 0x000EBCE8 File Offset: 0x000EA0E8
	public ClownLevelClownSwing.State state { get; private set; }

	// Token: 0x060019D6 RID: 6614 RVA: 0x000EBCF1 File Offset: 0x000EA0F1
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.state = ClownLevelClownSwing.State.Intro;
	}

	// Token: 0x060019D7 RID: 6615 RVA: 0x000EBD24 File Offset: 0x000EA124
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != ClownLevelClownSwing.State.Death)
		{
			this.state = ClownLevelClownSwing.State.Death;
			this.StartDeath();
		}
	}

	// Token: 0x060019D8 RID: 6616 RVA: 0x000EBD70 File Offset: 0x000EA170
	public override void LevelInit(LevelProperties.Clown properties)
	{
		base.LevelInit(properties);
		this.eyeMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.swing.positionString.Length);
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x000EBD97 File Offset: 0x000EA197
	public void StartSwing()
	{
		AudioManager.Play("clown_swing_face_intro");
		this.emitAudioFromObject.Add("clown_swing_face_intro");
		base.StartCoroutine(this.swing_intro_cr());
	}

	// Token: 0x060019DA RID: 6618 RVA: 0x000EBDC0 File Offset: 0x000EA1C0
	private IEnumerator swing_intro_cr()
	{
		float t = 0f;
		float time = 5f;
		Vector2 start = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.swingStopPosition.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.swingStopPosition.position;
		t = 0f;
		time = 0.5f;
		start = this.umbrella.transform.position;
		Vector2 end = new Vector3(this.umbrella.transform.position.x, this.umbrella.transform.position.y - 30f);
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this.umbrella.transform.position = Vector2.Lerp(start, end, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.umbrella.transform.position = start;
		this.umbrella.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		this.umbrella.GetComponent<SpriteRenderer>().sortingOrder = 200;
		this.state = ClownLevelClownSwing.State.Idle;
		base.StartCoroutine(this.swing_cr());
		this.coasterHandler.OnCoasterLeave += this.StartEnemies;
		yield return null;
		yield break;
	}

	// Token: 0x060019DB RID: 6619 RVA: 0x000EBDDC File Offset: 0x000EA1DC
	private IEnumerator swing_cr()
	{
		LevelProperties.Clown.Swing p = base.properties.CurrentState.swing;
		float spacingFront = this.swingFrontPrefab.GetComponent<Renderer>().bounds.size.x + p.swingSpacing;
		float spacingBack = this.swingBackPrefab.GetComponent<Renderer>().bounds.size.x + p.swingSpacing;
		int numOfSwings = 6;
		AudioManager.Play("clown_swing_open");
		this.emitAudioFromObject.Add("clown_swing_open");
		base.animator.SetTrigger("Continue");
		for (int i = 0; i < numOfSwings; i++)
		{
			Vector3 pos = new Vector3(-640f - spacingFront + spacingFront * (float)i, 360f, 0f);
			ClownLevelSwings clownLevelSwings = UnityEngine.Object.Instantiate<ClownLevelSwings>(this.swingFrontPrefab);
			clownLevelSwings.Init(pos, base.properties.CurrentState.swing, spacingFront, (float)i);
		}
		for (int j = 0; j < numOfSwings; j++)
		{
			Vector3 pos2 = new Vector3(640f + spacingBack - spacingBack * (float)j, 360f, 0f);
			ClownLevelSwings clownLevelSwings2 = UnityEngine.Object.Instantiate<ClownLevelSwings>(this.swingBackPrefab);
			clownLevelSwings2.Init(pos2, base.properties.CurrentState.swing, spacingBack, (float)j);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060019DC RID: 6620 RVA: 0x000EBDF7 File Offset: 0x000EA1F7
	private void StartBottom()
	{
		base.animator.Play("Swing_Bottom_Idle");
	}

	// Token: 0x060019DD RID: 6621 RVA: 0x000EBE09 File Offset: 0x000EA209
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.enemy = null;
		this.swingFrontPrefab = null;
		this.swingBackPrefab = null;
	}

	// Token: 0x060019DE RID: 6622 RVA: 0x000EBE28 File Offset: 0x000EA228
	private void StartDeath()
	{
		if (this.OnDeath != null)
		{
			this.OnDeath();
		}
		this.StopAllCoroutines();
		AudioManager.Play("clown_swing_death");
		this.emitAudioFromObject.Add("clown_swing_death");
		base.animator.Play("Swing_Death");
		base.animator.Play("Swing_Bottom_Death");
		base.animator.Play("Face_Death");
		ClownLevelSwings.moveSpeed = base.properties.CurrentState.swing.swingSpeed * 2f;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x060019DF RID: 6623 RVA: 0x000EBEC7 File Offset: 0x000EA2C7
	private void SetMoveDirection(int set)
	{
		if (set == 1)
		{
			this.moveUp = true;
		}
		else
		{
			this.moveUp = false;
		}
	}

	// Token: 0x060019E0 RID: 6624 RVA: 0x000EBEE4 File Offset: 0x000EA2E4
	private IEnumerator move_topper_cr()
	{
		float speed = 60f;
		for (;;)
		{
			if (this.moveUp)
			{
				this.topper.transform.position += Vector3.up * speed * CupheadTime.Delta;
			}
			else
			{
				this.topper.transform.position -= Vector3.up * speed * CupheadTime.Delta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060019E1 RID: 6625 RVA: 0x000EBEFF File Offset: 0x000EA2FF
	private void StartEnemies()
	{
		base.animator.SetBool("IsAttacking", true);
		base.StartCoroutine(this.enemies_cr());
	}

	// Token: 0x060019E2 RID: 6626 RVA: 0x000EBF20 File Offset: 0x000EA320
	private IEnumerator enemies_cr()
	{
		LevelProperties.Clown.Swing p = base.properties.CurrentState.swing;
		string[] enemyPosString = p.positionString[this.eyeMainIndex].Split(new char[]
		{
			','
		});
		this.state = ClownLevelClownSwing.State.Enemies;
		AudioManager.Play("clown_swing_face_attack_intro");
		this.emitAudioFromObject.Add("clown_swing_face_attack_intro");
		yield return base.animator.WaitForAnimationToEnd(this, "Face_Attack_Intro", 1, false, true);
		for (int i = 0; i < enemyPosString.Length; i++)
		{
			string[] enemyPos = enemyPosString[i].Split(new char[]
			{
				'-'
			});
			foreach (string pos in enemyPos)
			{
				float targetX = 0f;
				Parser.FloatTryParse(pos, out targetX);
				this.enemy.Create(base.transform.position, targetX, p.HP, p, this);
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
		}
		this.eyeMainIndex = (this.eyeMainIndex + 1) % p.positionString.Length;
		base.animator.SetBool("IsAttacking", false);
		AudioManager.Play("clown_swing_face_attack_outro");
		this.emitAudioFromObject.Add("clown_swing_face_attack_outro");
		yield return null;
		yield break;
	}

	// Token: 0x040022EE RID: 8942
	public const int NumOfSwings = 6;

	// Token: 0x040022EF RID: 8943
	[SerializeField]
	private ClownLevelCoasterHandler coasterHandler;

	// Token: 0x040022F0 RID: 8944
	[SerializeField]
	private GameObject umbrella;

	// Token: 0x040022F1 RID: 8945
	[SerializeField]
	private GameObject topper;

	// Token: 0x040022F2 RID: 8946
	[SerializeField]
	private ClownLevelEnemy enemy;

	// Token: 0x040022F3 RID: 8947
	[SerializeField]
	private ClownLevelSwings swingFrontPrefab;

	// Token: 0x040022F4 RID: 8948
	[SerializeField]
	private ClownLevelSwings swingBackPrefab;

	// Token: 0x040022F5 RID: 8949
	[SerializeField]
	private BasicProjectile clownBullet;

	// Token: 0x040022F6 RID: 8950
	[SerializeField]
	private Transform swingStopPosition;

	// Token: 0x040022F8 RID: 8952
	private DamageReceiver damageReceiver;

	// Token: 0x040022F9 RID: 8953
	private bool moveUp;

	// Token: 0x040022FA RID: 8954
	private int eyeMainIndex;

	// Token: 0x040022FB RID: 8955
	public Action OnDeath;

	// Token: 0x02000560 RID: 1376
	public enum State
	{
		// Token: 0x040022FD RID: 8957
		Intro,
		// Token: 0x040022FE RID: 8958
		Idle,
		// Token: 0x040022FF RID: 8959
		Enemies,
		// Token: 0x04002300 RID: 8960
		Death
	}
}
