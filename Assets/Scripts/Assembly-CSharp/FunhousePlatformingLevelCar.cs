using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B1 RID: 2225
public class FunhousePlatformingLevelCar : AbstractCollidableObject
{
	// Token: 0x060033DC RID: 13276 RVA: 0x001E16DB File Offset: 0x001DFADB
	protected override void Awake()
	{
		base.Awake();
		FunhousePlatformingLevelCar.CARS_ALIVE++;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060033DD RID: 13277 RVA: 0x001E16FA File Offset: 0x001DFAFA
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060033DE RID: 13278 RVA: 0x001E1712 File Offset: 0x001DFB12
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060033DF RID: 13279 RVA: 0x001E1730 File Offset: 0x001DFB30
	public void Init(Vector2 pos, float rotation, float carSpeed, int index, bool leader, bool last)
	{
		base.transform.position = pos;
		base.transform.SetEulerAngles(null, null, new float?(rotation));
		this.speed = carSpeed;
		this.leader = leader;
		this.last = last;
		foreach (GameObject gameObject in this.carSprites)
		{
			gameObject.SetActive(false);
		}
		this.carSprites[index].SetActive(true);
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060033E0 RID: 13280 RVA: 0x001E17CC File Offset: 0x001DFBCC
	private IEnumerator move_cr()
	{
		if (this.leader)
		{
			AudioManager.PlayLoop("funhouse_car_idle");
			this.emitAudioFromObject.Add("funhouse_car_idle");
		}
		YieldInstruction wait = new WaitForFixedUpdate();
		float size = base.GetComponent<Collider2D>().bounds.size.x;
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - (size + 50f))
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		if (this.last && FunhousePlatformingLevelCar.CARS_ALIVE <= 1)
		{
			AudioManager.Stop("funhouse_car_idle");
		}
		FunhousePlatformingLevelCar.CARS_ALIVE--;
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003C23 RID: 15395
	private static int CARS_ALIVE;

	// Token: 0x04003C24 RID: 15396
	[SerializeField]
	private GameObject[] carSprites;

	// Token: 0x04003C25 RID: 15397
	private bool leader;

	// Token: 0x04003C26 RID: 15398
	private bool last;

	// Token: 0x04003C27 RID: 15399
	private float speed;

	// Token: 0x04003C28 RID: 15400
	private DamageDealer damageDealer;
}
