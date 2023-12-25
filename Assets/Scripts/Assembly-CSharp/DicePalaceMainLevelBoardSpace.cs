using System;
using UnityEngine;

// Token: 0x020005CF RID: 1487
public class DicePalaceMainLevelBoardSpace : MonoBehaviour
{
	// Token: 0x17000366 RID: 870
	// (get) Token: 0x06001D35 RID: 7477 RVA: 0x0010C476 File Offset: 0x0010A876
	// (set) Token: 0x06001D36 RID: 7478 RVA: 0x0010C4A7 File Offset: 0x0010A8A7
	public bool HasHeart
	{
		get
		{
			return !(this.heartSpace == null) && !(this.odds == null) && this.heartSpace.activeSelf;
		}
		set
		{
			if (this.heartSpace != null && this.odds != null)
			{
				this.odds.SetActive(!value);
				this.heartSpace.SetActive(value);
			}
		}
	}

	// Token: 0x17000367 RID: 871
	// (get) Token: 0x06001D37 RID: 7479 RVA: 0x0010C4E6 File Offset: 0x0010A8E6
	public Transform Pivot
	{
		get
		{
			return this.pivot;
		}
	}

	// Token: 0x17000368 RID: 872
	// (set) Token: 0x06001D38 RID: 7480 RVA: 0x0010C4EE File Offset: 0x0010A8EE
	public bool Clear
	{
		set
		{
			this.space.SetActive(!value);
			this.clearSpace.SetActive(value);
		}
	}

	// Token: 0x17000369 RID: 873
	// (get) Token: 0x06001D39 RID: 7481 RVA: 0x0010C50B File Offset: 0x0010A90B
	public Vector3 HeartSpacePosition
	{
		get
		{
			if (this.heartSpace != null)
			{
				return this.heartSpace.transform.position;
			}
			return Vector3.zero;
		}
	}

	// Token: 0x0400261F RID: 9759
	[SerializeField]
	private GameObject space;

	// Token: 0x04002620 RID: 9760
	[SerializeField]
	private GameObject clearSpace;

	// Token: 0x04002621 RID: 9761
	[SerializeField]
	private GameObject odds;

	// Token: 0x04002622 RID: 9762
	[SerializeField]
	private GameObject heartSpace;

	// Token: 0x04002623 RID: 9763
	[SerializeField]
	private Transform pivot;
}
