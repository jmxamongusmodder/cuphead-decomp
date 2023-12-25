using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005EC RID: 1516
public class DragonLevelCloudPlatform : LevelPlatform
{
	// Token: 0x06001E12 RID: 7698 RVA: 0x00114C20 File Offset: 0x00113020
	protected override void Awake()
	{
		base.Awake();
		base.animator.SetInteger("Cloud", UnityEngine.Random.Range(0, 3));
		this.minX = -640f - base.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
		this.maxX = 640f + base.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x00114CA4 File Offset: 0x001130A4
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		base.animator.SetBool("HasPlayer", true);
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x00114CBE File Offset: 0x001130BE
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		if (base.players.Count <= 0)
		{
			base.animator.SetBool("HasPlayer", false);
		}
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x00114CE9 File Offset: 0x001130E9
	private void OnDisable()
	{
		this.top.sprite = null;
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x00114CF7 File Offset: 0x001130F7
	public void GetProperties(DragonLevelPlatformManager manager, LevelProperties.Dragon.Clouds properties)
	{
		this.properties = properties;
		this.manager = manager;
		this.speed = ((!properties.movingRight) ? properties.cloudSpeed : (-properties.cloudSpeed));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x00114D37 File Offset: 0x00113137
	public void GetProperties(LevelProperties.Dragon.Clouds properties, bool firstTime)
	{
		this.properties = properties;
		this.speed = ((!properties.movingRight) ? properties.cloudSpeed : (-properties.cloudSpeed));
		if (firstTime)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x00114D78 File Offset: 0x00113178
	private IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(-DragonLevel.SPEED * this.speed * CupheadTime.Delta, 0f, 0f);
			yield return null;
			if (this.properties.movingRight)
			{
				if (base.transform.position.x >= this.maxX)
				{
					if (this.manager != null)
					{
						this.manager.DestroyObjectPool(this);
					}
					else
					{
						UnityEngine.Object.Destroy(base.gameObject);
					}
				}
			}
			else if (base.transform.position.x <= this.minX)
			{
				if (this.manager != null)
				{
					this.manager.DestroyObjectPool(this);
				}
				else
				{
					UnityEngine.Object.Destroy(base.gameObject);
				}
			}
		}
		yield break;
	}

	// Token: 0x040026DC RID: 9948
	[SerializeField]
	public SpriteRenderer top;

	// Token: 0x040026DD RID: 9949
	private float minX;

	// Token: 0x040026DE RID: 9950
	private float maxX;

	// Token: 0x040026DF RID: 9951
	private LevelProperties.Dragon.Clouds properties;

	// Token: 0x040026E0 RID: 9952
	private DragonLevelPlatformManager manager;

	// Token: 0x040026E1 RID: 9953
	private float speed;
}
