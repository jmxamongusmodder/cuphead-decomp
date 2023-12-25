using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200053D RID: 1341
public class ChessKingLevelParryPoint : ParrySwitch
{
	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06001871 RID: 6257 RVA: 0x000DD7CC File Offset: 0x000DBBCC
	// (set) Token: 0x06001872 RID: 6258 RVA: 0x000DD7D4 File Offset: 0x000DBBD4
	public bool GOT_PARRIED { get; private set; }

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06001873 RID: 6259 RVA: 0x000DD7DD File Offset: 0x000DBBDD
	// (set) Token: 0x06001874 RID: 6260 RVA: 0x000DD7E5 File Offset: 0x000DBBE5
	public bool IS_BLUE { get; private set; }

	// Token: 0x06001875 RID: 6261 RVA: 0x000DD7EE File Offset: 0x000DBBEE
	protected override void Awake()
	{
		this.GOT_PARRIED = false;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().color = Color.grey;
		base.Awake();
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x000DD819 File Offset: 0x000DBC19
	public void Init(Vector3 pos)
	{
		base.transform.position = pos;
		this.IS_BLUE = false;
		this.GOT_PARRIED = false;
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x000DD838 File Offset: 0x000DBC38
	public void Init(Vector3 pos, Vector3 dir, float speed, float amount)
	{
		base.GetComponent<SpriteRenderer>().color = Color.blue;
		base.transform.position = pos;
		this.dir = dir;
		this.amount = amount;
		this.speed = speed;
		this.IS_BLUE = true;
		this.GOT_PARRIED = false;
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x000DD885 File Offset: 0x000DBC85
	public void Activate()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().color = Color.magenta;
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x000DD8A3 File Offset: 0x000DBCA3
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		this.GOT_PARRIED = true;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().color = Color.grey;
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x000DD8CF File Offset: 0x000DBCCF
	public void MovePoint()
	{
		if (this.IS_BLUE)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x000DD8EC File Offset: 0x000DBCEC
	private IEnumerator move_cr()
	{
		Vector3 startPos = base.transform.position;
		Vector3 endPos = this.GetEndPos();
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		float time = Vector3.Distance(startPos, endPos) / this.speed;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(startPos, endPos, t / time);
			yield return wait;
		}
		base.transform.position = endPos;
		yield return null;
		yield break;
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x000DD908 File Offset: 0x000DBD08
	private Vector3 GetEndPos()
	{
		if (this.dir == Vector3.right)
		{
			return new Vector3(base.transform.position.x + this.amount, base.transform.position.y);
		}
		if (this.dir == Vector3.left)
		{
			return new Vector3(base.transform.position.x - this.amount, base.transform.position.y);
		}
		if (this.dir == Vector3.up)
		{
			return new Vector3(base.transform.position.x, base.transform.position.y + this.amount);
		}
		return new Vector3(base.transform.position.x, base.transform.position.y - this.amount);
	}

	// Token: 0x040021A0 RID: 8608
	private Vector3 dir;

	// Token: 0x040021A1 RID: 8609
	private float amount;

	// Token: 0x040021A2 RID: 8610
	private float speed;
}
