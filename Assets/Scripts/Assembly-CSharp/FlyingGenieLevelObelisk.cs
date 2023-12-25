using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000674 RID: 1652
public class FlyingGenieLevelObelisk : AbstractCollidableObject
{
	// Token: 0x17000398 RID: 920
	// (get) Token: 0x060022C0 RID: 8896 RVA: 0x00146464 File Offset: 0x00144864
	// (set) Token: 0x060022C1 RID: 8897 RVA: 0x0014646C File Offset: 0x0014486C
	public bool isOn { get; private set; }

	// Token: 0x17000399 RID: 921
	// (get) Token: 0x060022C2 RID: 8898 RVA: 0x00146475 File Offset: 0x00144875
	// (set) Token: 0x060022C3 RID: 8899 RVA: 0x0014647D File Offset: 0x0014487D
	public bool isFirst { get; private set; }

	// Token: 0x060022C4 RID: 8900 RVA: 0x00146488 File Offset: 0x00144888
	public void Init(Vector2 pos, LevelProperties.FlyingGenie.Obelisk properties, FlyingGenieLevelGenie parent, bool isFirst)
	{
		this.isFirst = isFirst;
		base.transform.position = pos;
		this.startPosition = pos;
		this.properties = properties;
		this.parent = parent;
		if (isFirst)
		{
			AudioManager.PlayLoop("genie_pillar_main_loop");
			AudioManager.PlayLoop("genie_pillar_destructable_loop");
			this.emitAudioFromObject.Add("genie_pillar_main_loop");
			this.emitAudioFromObject.Add("genie_pillar_destructable_loop");
		}
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x00146503 File Offset: 0x00144903
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		if (Rand.Bool())
		{
			this.baseA.SetActive(true);
		}
		else
		{
			this.baseB.SetActive(true);
		}
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x00146538 File Offset: 0x00144938
	public void ActivateObelisk(string[] genieHeadNums)
	{
		this.isOn = true;
		this.bouncerWall.enabled = true;
		base.transform.position = this.startPosition;
		float num = this.obeliskBlock.GetComponent<Renderer>().bounds.size.y / 1.95f;
		int num2 = 100;
		int num3 = 0;
		int num4 = 5;
		bool flag = false;
		this.obeliskBlocks = new List<FlyingGenieLevelObeliskBlock>();
		this.genieHeads = new List<FlyingGenieLevelGenieHead>();
		bool[] array = new bool[num4];
		int[] array2 = new int[num4];
		for (int i = 0; i < num4; i++)
		{
			bool flag2 = false;
			foreach (string s in genieHeadNums)
			{
				Parser.IntTryParse(s, out num3);
				if (num3 == i + 1)
				{
					flag2 = true;
				}
				if (genieHeadNums.Length < 2 && num3 == 2)
				{
					flag = true;
				}
			}
			array[i] = flag2;
		}
		int num5 = (num3 <= 1) ? (num4 - 1) : (num3 - 1 - 1);
		if (genieHeadNums.Length > 1)
		{
			if (genieHeadNums[0][0] == '2' && genieHeadNums[1][0] == '5')
			{
				array2[0] = 1;
				array2[2] = 5;
				array2[3] = 1;
			}
			else if (genieHeadNums[0][0] == '1' && genieHeadNums[1][0] == '4')
			{
				array2[1] = 5;
				array2[2] = 1;
				array2[4] = 5;
			}
			else if (genieHeadNums[0][0] == '1' && genieHeadNums[1][0] == '5')
			{
				array2[1] = 4;
				array2[2] = 5;
				array2[3] = 1;
			}
		}
		else
		{
			for (int k = 0; k < num4; k++)
			{
				array2[num5] = k + 1;
				num5 = (num5 + 1) % num4;
			}
		}
		bool flag3 = Rand.Bool();
		for (int l = 0; l < num4; l++)
		{
			Vector3 b = new Vector3(0f, (float)(-(float)l) * num - num / 1.5f, 0f);
			if (array[l])
			{
				FlyingGenieLevelGenieHead flyingGenieLevelGenieHead = UnityEngine.Object.Instantiate<FlyingGenieLevelGenieHead>(this.genieHead);
				flyingGenieLevelGenieHead.Init(base.transform.position + b, this.properties.obeliskGenieHP, this.parent);
				flyingGenieLevelGenieHead.transform.parent = base.transform;
				flyingGenieLevelGenieHead.GetComponent<SpriteRenderer>().sortingOrder = num2;
				flyingGenieLevelGenieHead.animator.SetBool("GoClockwise", flag3);
				this.genieHeads.Add(flyingGenieLevelGenieHead);
			}
			else
			{
				FlyingGenieLevelObeliskBlock flyingGenieLevelObeliskBlock = UnityEngine.Object.Instantiate<FlyingGenieLevelObeliskBlock>(this.obeliskBlock);
				flyingGenieLevelObeliskBlock.Init(base.transform.position + b, this.properties);
				flyingGenieLevelObeliskBlock.transform.parent = base.transform;
				flyingGenieLevelObeliskBlock.GetComponent<SpriteRenderer>().sortingOrder = num2;
				flyingGenieLevelObeliskBlock.animator.SetBool("GoClockwise", flag3);
				flyingGenieLevelObeliskBlock.animator.SetInteger("PickBlock", array2[l]);
				this.obeliskBlocks.Add(flyingGenieLevelObeliskBlock);
			}
			if (!flag)
			{
				flag3 = !flag3;
			}
			num2 -= 2;
		}
		base.StartCoroutine(this.move_cr());
		if (this.properties.normalShotOn)
		{
			base.StartCoroutine(this.shoot_cr());
		}
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x001468A0 File Offset: 0x00144CA0
	public void SetColliders(float width, float offset)
	{
		this.ceiling.transform.position = new Vector3(offset, this.ceiling.transform.position.y);
		this.ceiling.size = new Vector3(width, this.ceiling.size.y);
		this.ground.transform.position = new Vector3(offset, -360f);
		this.ground.size = new Vector3(width, this.ground.size.y);
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x00146948 File Offset: 0x00144D48
	private IEnumerator shoot_cr()
	{
		string[] anglePattern = this.properties.obeliskShotDirection.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] pinkPattern = this.properties.obeliskPinkString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int angleIndex = UnityEngine.Random.Range(0, anglePattern.Length);
		int pinkIndex = UnityEngine.Random.Range(0, pinkPattern.Length);
		float angle = 0f;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.obeliskShootDelay);
			yield return null;
			foreach (FlyingGenieLevelObeliskBlock block in this.obeliskBlocks)
			{
				Parser.FloatTryParse(anglePattern[angleIndex], out angle);
				if (pinkPattern[pinkIndex][0] == 'P')
				{
					block.ShootPink(angle);
				}
				else
				{
					block.ShootRegular(angle);
				}
				yield return null;
			}
			angleIndex %= anglePattern.Length;
			pinkIndex %= pinkPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x00146963 File Offset: 0x00144D63
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x00146984 File Offset: 0x00144D84
	private IEnumerator move_cr()
	{
		this.isFirst = true;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > -840f)
		{
			base.transform.AddPosition(-this.properties.obeliskMovementSpeed * CupheadTime.Delta, 0f, 0f);
			yield return wait;
		}
		this.isFirst = false;
		this.End();
		yield return null;
		yield break;
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x001469A0 File Offset: 0x00144DA0
	private void End()
	{
		this.isOn = false;
		foreach (FlyingGenieLevelObeliskBlock flyingGenieLevelObeliskBlock in this.obeliskBlocks)
		{
			UnityEngine.Object.Destroy(flyingGenieLevelObeliskBlock.gameObject);
		}
		foreach (FlyingGenieLevelGenieHead flyingGenieLevelGenieHead in this.genieHeads)
		{
			if (flyingGenieLevelGenieHead != null)
			{
				UnityEngine.Object.Destroy(flyingGenieLevelGenieHead.gameObject);
			}
		}
		this.obeliskBlocks.Clear();
		this.genieHeads.Clear();
		this.StopAllCoroutines();
		this.bouncerWall.enabled = false;
	}

	// Token: 0x04002B5F RID: 11103
	[SerializeField]
	private GameObject baseA;

	// Token: 0x04002B60 RID: 11104
	[SerializeField]
	private GameObject baseB;

	// Token: 0x04002B61 RID: 11105
	[SerializeField]
	private BoxCollider2D bouncerWall;

	// Token: 0x04002B62 RID: 11106
	[SerializeField]
	private BoxCollider2D ceiling;

	// Token: 0x04002B63 RID: 11107
	[SerializeField]
	private BoxCollider2D ground;

	// Token: 0x04002B64 RID: 11108
	[SerializeField]
	private FlyingGenieLevelGenieHead genieHead;

	// Token: 0x04002B65 RID: 11109
	[SerializeField]
	private FlyingGenieLevelObeliskBlock obeliskBlock;

	// Token: 0x04002B66 RID: 11110
	private List<FlyingGenieLevelObeliskBlock> obeliskBlocks;

	// Token: 0x04002B67 RID: 11111
	private List<FlyingGenieLevelGenieHead> genieHeads;

	// Token: 0x04002B68 RID: 11112
	private LevelProperties.FlyingGenie.Obelisk properties;

	// Token: 0x04002B69 RID: 11113
	private FlyingGenieLevelGenie parent;

	// Token: 0x04002B6A RID: 11114
	private DamageDealer damageDealer;

	// Token: 0x04002B6B RID: 11115
	private Vector3 startPosition;

	// Token: 0x04002B6C RID: 11116
	private Vector3 newEmitPosition;

	// Token: 0x04002B6D RID: 11117
	private float health;

	// Token: 0x04002B6E RID: 11118
	private float shootAngle;
}
