using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000451 RID: 1105
public class AchievementIcon : MonoBehaviour
{
	// Token: 0x0600109E RID: 4254 RVA: 0x0009F9C9 File Offset: 0x0009DDC9
	private void Awake()
	{
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x0600109F RID: 4255 RVA: 0x0009F9D7 File Offset: 0x0009DDD7
	public void SetIcon(Sprite sprite)
	{
		this.image.sprite = sprite;
	}

	// Token: 0x040019D3 RID: 6611
	private Image image;
}
