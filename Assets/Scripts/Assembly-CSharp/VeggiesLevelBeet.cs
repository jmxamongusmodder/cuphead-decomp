using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200083A RID: 2106
public class VeggiesLevelBeet : LevelProperties.Veggies.Entity
{
	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x060030C9 RID: 12489 RVA: 0x001CB398 File Offset: 0x001C9798
	// (set) Token: 0x060030CA RID: 12490 RVA: 0x001CB3A0 File Offset: 0x001C97A0
	public VeggiesLevelBeet.State state { get; private set; }

	// Token: 0x1400005A RID: 90
	// (add) Token: 0x060030CB RID: 12491 RVA: 0x001CB3AC File Offset: 0x001C97AC
	// (remove) Token: 0x060030CC RID: 12492 RVA: 0x001CB3E4 File Offset: 0x001C97E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event VeggiesLevelBeet.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x060030CD RID: 12493 RVA: 0x001CB41A File Offset: 0x001C981A
	protected override void Awake()
	{
		base.Awake();
		this.CreatePoints();
	}

	// Token: 0x060030CE RID: 12494 RVA: 0x001CB428 File Offset: 0x001C9828
	private void Start()
	{
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		this.boxCollider.enabled = false;
	}

	// Token: 0x060030CF RID: 12495 RVA: 0x001CB444 File Offset: 0x001C9844
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		foreach (Vector2 v in this.GetPoints())
		{
			Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
			Gizmos.DrawLine(this.babyRoot.position, v);
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(v, 10f);
		}
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.babyRoot.position, new Vector3(-150f, 360f, 0f));
		Gizmos.DrawLine(this.babyRoot.position, new Vector3(640f, 360f, 0f));
	}

	// Token: 0x060030D0 RID: 12496 RVA: 0x001CB520 File Offset: 0x001C9920
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.properties = (propertyGroup as LevelProperties.Veggies.Beet);
		this.hp = (float)this.properties.hp;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 0.2f, true, false, false);
		this.damageDealer.SetDirection(DamageDealer.Direction.Neutral, base.transform);
	}

	// Token: 0x060030D1 RID: 12497 RVA: 0x001CB594 File Offset: 0x001C9994
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state == VeggiesLevelBeet.State.Start)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060030D2 RID: 12498 RVA: 0x001CB5F2 File Offset: 0x001C99F2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060030D3 RID: 12499 RVA: 0x001CB610 File Offset: 0x001C9A10
	private void OnInAnimComplete()
	{
		this.boxCollider.enabled = true;
		this.state = VeggiesLevelBeet.State.Go;
		base.StartCoroutine(this.beet_cr());
	}

	// Token: 0x060030D4 RID: 12500 RVA: 0x001CB632 File Offset: 0x001C9A32
	private void OnDeathAnimComplete()
	{
		this.state = VeggiesLevelBeet.State.Complete;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060030D5 RID: 12501 RVA: 0x001CB646 File Offset: 0x001C9A46
	private void Die()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x060030D6 RID: 12502 RVA: 0x001CB65C File Offset: 0x001C9A5C
	private Vector2[] GetPoints()
	{
		Vector2[] array = new Vector2[8];
		for (int i = 0; i < 8; i++)
		{
			float t = (float)i / 7f;
			array[i] = Vector2.Lerp(new Vector2(-150f, 360f), new Vector2(640f, 360f), t);
		}
		return array;
	}

	// Token: 0x060030D7 RID: 12503 RVA: 0x001CB6BC File Offset: 0x001C9ABC
	private void CreatePoints()
	{
		Vector2[] array = this.GetPoints();
		this.points = new Transform[array.Length];
		for (int i = 0; i < this.points.Length; i++)
		{
			this.points[i] = new GameObject("Point " + i).transform;
			this.points[i].position = array[i];
			this.points[i].SetParent(base.transform);
		}
	}

	// Token: 0x060030D8 RID: 12504 RVA: 0x001CB74C File Offset: 0x001C9B4C
	private float GetPointAngle(int i)
	{
		this.babyRoot.LookAt2D(this.points[i]);
		return this.babyRoot.eulerAngles.z;
	}

	// Token: 0x060030D9 RID: 12505 RVA: 0x001CB780 File Offset: 0x001C9B80
	private IEnumerator beet_cr()
	{
		string[] array = this.properties.babyPatterns[UnityEngine.Random.Range(0, this.properties.babyPatterns.Length)].Split(new char[]
		{
			','
		});
		int[] numbers = new int[array.Length];
		for (int j = 0; j < numbers.Length; j++)
		{
			if (!Parser.IntTryParse(array[j], out numbers[j]))
			{
				global::Debug.LogError("Veggies.Beet.babyPatterns is not formatted correctly!\nExpecting 4,5,6,5,4", null);
				this.StopAllCoroutines();
			}
		}
		int typeIndex = 0;
		int specialIndex = this.properties.alternateRate.RandomInt();
		VeggiesLevelBeetBaby.Type type = VeggiesLevelBeetBaby.Type.Regular;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime);
			base.animator.SetTrigger("Shoot_Start");
			yield return CupheadTime.WaitForSeconds(this, 1f);
			int loop = 0;
			int point = 0;
			while (loop < numbers.Length)
			{
				for (int i = 0; i < numbers[loop]; i++)
				{
					int newPoint;
					for (newPoint = point; newPoint == point; newPoint = UnityEngine.Random.Range(0, 8))
					{
					}
					point = newPoint;
					if (typeIndex >= specialIndex)
					{
						type = ((!Rand.Bool()) ? VeggiesLevelBeetBaby.Type.Fat : VeggiesLevelBeetBaby.Type.Pink);
						typeIndex = 0;
						specialIndex = this.properties.alternateRate.RandomInt();
					}
					else
					{
						type = VeggiesLevelBeetBaby.Type.Regular;
					}
					typeIndex++;
					base.animator.SetTrigger("Shoot_" + type.ToString());
					this.babyPrefab.Create(type, this.properties.babySpeedUp, (float)this.properties.babySpeedSpread, this.properties.babySpreadAngle, this.babyRoot.position, this.GetPointAngle(point));
					yield return CupheadTime.WaitForSeconds(this, this.properties.babyDelay);
				}
				loop++;
				if (loop < numbers.Length)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.babyGroupDelay);
				}
			}
			base.animator.SetTrigger("Shoot_End");
			yield return CupheadTime.WaitForSeconds(this, this.properties.babyGroupDelay);
		}
		yield break;
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x001CB79C File Offset: 0x001C9B9C
	private IEnumerator die_cr()
	{
		this.boxCollider.enabled = false;
		base.animator.SetTrigger("Idle");
		yield return base.StartCoroutine(base.dieFlash_cr());
		base.animator.SetTrigger("Dead");
		yield break;
	}

	// Token: 0x04003970 RID: 14704
	public const float MAX_Y = 360f;

	// Token: 0x04003971 RID: 14705
	private const float POINTS_X_MIN = -150f;

	// Token: 0x04003972 RID: 14706
	private const float POINTS_X_MAX = 640f;

	// Token: 0x04003973 RID: 14707
	private const int POINTS_COUNT = 8;

	// Token: 0x04003975 RID: 14709
	[SerializeField]
	private Transform babyRoot;

	// Token: 0x04003976 RID: 14710
	[SerializeField]
	private VeggiesLevelBeetBaby babyPrefab;

	// Token: 0x04003977 RID: 14711
	private new LevelProperties.Veggies.Beet properties;

	// Token: 0x04003978 RID: 14712
	private BoxCollider2D boxCollider;

	// Token: 0x04003979 RID: 14713
	private float hp;

	// Token: 0x0400397A RID: 14714
	private Transform[] points;

	// Token: 0x0400397B RID: 14715
	private DamageDealer damageDealer;

	// Token: 0x0200083B RID: 2107
	public enum State
	{
		// Token: 0x0400397E RID: 14718
		Start,
		// Token: 0x0400397F RID: 14719
		Go,
		// Token: 0x04003980 RID: 14720
		Complete
	}

	// Token: 0x0200083C RID: 2108
	// (Invoke) Token: 0x060030DC RID: 12508
	public delegate void OnDamageTakenHandler(float damage);
}
