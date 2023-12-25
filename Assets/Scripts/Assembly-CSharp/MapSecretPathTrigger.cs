using System;
using UnityEngine;

// Token: 0x0200095F RID: 2399
public class MapSecretPathTrigger : AbstractMonoBehaviour
{
	// Token: 0x06003800 RID: 14336 RVA: 0x00200FBA File Offset: 0x001FF3BA
	private void Start()
	{
		this.size = base.GetComponent<BoxCollider2D>().size;
	}

	// Token: 0x06003801 RID: 14337 RVA: 0x00200FD0 File Offset: 0x001FF3D0
	private bool PointInBounds(Vector3 pos)
	{
		return pos.x > base.transform.position.x - this.size.x / 2f && pos.x < base.transform.position.x + this.size.x / 2f && pos.y > base.transform.position.y - this.size.y / 2f && pos.y < base.transform.position.y + this.size.y / 2f;
	}

	// Token: 0x06003802 RID: 14338 RVA: 0x002010A4 File Offset: 0x001FF4A4
	private void OnTriggerStay2D(Collider2D collider)
	{
		MapPlayerController component = collider.GetComponent<MapPlayerController>();
		if (component && this.PointInBounds(component.transform.position))
		{
			component.SecretPathEnter(this.enablePath);
		}
	}

	// Token: 0x04003FE7 RID: 16359
	[SerializeField]
	private bool enablePath;

	// Token: 0x04003FE8 RID: 16360
	private Vector2 size;
}
