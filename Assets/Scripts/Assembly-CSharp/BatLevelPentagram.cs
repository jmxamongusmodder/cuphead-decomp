using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200050A RID: 1290
public class BatLevelPentagram : AbstractCollidableObject
{
	// Token: 0x060016E1 RID: 5857 RVA: 0x000CDAE0 File Offset: 0x000CBEE0
	public void Init(Vector2 pos, LevelProperties.Bat.Pentagrams properties, AbstractPlayerController player, bool onRight)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.player = player;
		this.onRight = onRight;
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.move_cr());
		base.transform.SetScale(new float?(properties.pentagramSize), new float?(properties.pentagramSize), new float?(1f));
	}

	// Token: 0x060016E2 RID: 5858 RVA: 0x000CDB58 File Offset: 0x000CBF58
	private IEnumerator move_cr()
	{
		Vector3 pos = base.transform.position;
		float endPos = 560f;
		if (this.onRight)
		{
			while (base.transform.position.x > this.player.transform.position.x)
			{
				pos.x = Mathf.MoveTowards(base.transform.position.x, this.player.transform.position.x, this.properties.xSpeed * CupheadTime.Delta);
				base.transform.position = pos;
				yield return null;
			}
		}
		else
		{
			while (base.transform.position.x < this.player.transform.position.x)
			{
				pos.x = Mathf.MoveTowards(base.transform.position.x, this.player.transform.position.x, this.properties.xSpeed * CupheadTime.Delta);
				base.transform.position = pos;
				yield return null;
			}
		}
		base.GetComponent<Collider2D>().enabled = true;
		while (base.transform.position.y < endPos)
		{
			pos.y = Mathf.MoveTowards(base.transform.position.y, endPos, this.properties.ySpeed * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x000CDB73 File Offset: 0x000CBF73
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400202A RID: 8234
	private LevelProperties.Bat.Pentagrams properties;

	// Token: 0x0400202B RID: 8235
	private AbstractPlayerController player;

	// Token: 0x0400202C RID: 8236
	private bool onRight;
}
