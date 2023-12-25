using System;
using UnityEngine;

// Token: 0x02000780 RID: 1920
public class RobotLevelScrollingSprite : ScrollingSpriteSpawner
{
	// Token: 0x06002A35 RID: 10805 RVA: 0x0018B2A4 File Offset: 0x001896A4
	protected override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		SpriteRenderer component = obj.GetComponent<SpriteRenderer>();
		component.sortingLayerName = this.layer.ToString();
		component.sprite = this.sprites[UnityEngine.Random.Range(0, this.sprites.Length)];
		Vector3 b = Vector3.up * UnityEngine.Random.Range(this.yOffset.min, this.yOffset.max);
		obj.transform.position += b;
		obj.transform.localScale = new Vector3(base.transform.localScale.x * (float)MathUtils.PlusOrMinus(), base.transform.localScale.y, base.transform.localScale.z);
	}

	// Token: 0x0400330E RID: 13070
	[SerializeField]
	private SpriteLayer layer = SpriteLayer.Default;

	// Token: 0x0400330F RID: 13071
	[SerializeField]
	private MinMax yOffset;

	// Token: 0x04003310 RID: 13072
	[SerializeField]
	private Sprite[] sprites;
}
