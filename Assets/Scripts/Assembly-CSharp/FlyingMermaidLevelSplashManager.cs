using System;
using UnityEngine;

// Token: 0x0200069F RID: 1695
public class FlyingMermaidLevelSplashManager : AbstractPausableComponent
{
	// Token: 0x170003A6 RID: 934
	// (get) Token: 0x060023E9 RID: 9193 RVA: 0x0015166C File Offset: 0x0014FA6C
	public static FlyingMermaidLevelSplashManager Instance
	{
		get
		{
			if (FlyingMermaidLevelSplashManager.splashManager == null)
			{
				FlyingMermaidLevelSplashManager.splashManager = new GameObject
				{
					name = "SplashManager"
				}.AddComponent<FlyingMermaidLevelSplashManager>();
			}
			return FlyingMermaidLevelSplashManager.splashManager;
		}
	}

	// Token: 0x060023EA RID: 9194 RVA: 0x001516AA File Offset: 0x0014FAAA
	protected override void Awake()
	{
		base.Awake();
		FlyingMermaidLevelSplashManager.splashManager = this;
	}

	// Token: 0x060023EB RID: 9195 RVA: 0x001516B8 File Offset: 0x0014FAB8
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "EnemyProjectile" && collider.gameObject.GetComponent<FlyingMermaidLevelNoSplashMarker>() == null)
		{
			if (collider.GetComponent<Collider2D>().bounds.size.x > 200f)
			{
				this.SpawnMegaSplashMedium(collider.gameObject, 0f, false, 0f);
			}
			else if (collider.GetComponent<Collider2D>().bounds.size.x > 50f)
			{
				this.SpawnSplashMedium(collider.gameObject, 0f, false, 0f);
			}
			else
			{
				this.SpawnSplashSmall(collider.gameObject);
			}
		}
	}

	// Token: 0x060023EC RID: 9196 RVA: 0x00151783 File Offset: 0x0014FB83
	public void SpawnMegaSplashLarge(GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		this.CreateSplash(this.MegasplashLarge, gameObject, extraX, overrideY, y);
	}

	// Token: 0x060023ED RID: 9197 RVA: 0x00151796 File Offset: 0x0014FB96
	public void SpawnMegaSplashMedium(GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		this.CreateSplash(this.MegasplashMedium, gameObject, extraX, overrideY, y);
	}

	// Token: 0x060023EE RID: 9198 RVA: 0x001517A9 File Offset: 0x0014FBA9
	public void SpawnSplashMedium(GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		this.CreateSplash(this.SplashMedium, gameObject, extraX, overrideY, y);
	}

	// Token: 0x060023EF RID: 9199 RVA: 0x001517BC File Offset: 0x0014FBBC
	public void SpawnSplashSmall(GameObject gameObject)
	{
		this.CreateSplash(this.SplashSmall, gameObject, 0f, false, 0f);
	}

	// Token: 0x060023F0 RID: 9200 RVA: 0x001517D8 File Offset: 0x0014FBD8
	private void CreateSplash(Effect effect, GameObject gameObject, float extraX = 0f, bool overrideY = false, float y = 0f)
	{
		float num = 0f;
		if (gameObject.GetComponent<Renderer>() != null)
		{
			num = gameObject.GetComponent<Renderer>().bounds.size.y / 4f;
		}
		Vector3 position = new Vector3(gameObject.transform.position.x + extraX, gameObject.transform.position.y - num);
		if (overrideY)
		{
			position.y = y;
		}
		Effect effect2 = UnityEngine.Object.Instantiate<Effect>(effect);
		effect2.transform.position = position;
		if (gameObject.GetComponent<SpriteRenderer>() != null)
		{
			effect2.GetComponent<SpriteRenderer>().sortingLayerName = gameObject.GetComponent<SpriteRenderer>().sortingLayerName;
			effect2.GetComponent<SpriteRenderer>().sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder + 1;
		}
	}

	// Token: 0x060023F1 RID: 9201 RVA: 0x001518B7 File Offset: 0x0014FCB7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.MegasplashLarge = null;
		this.MegasplashMedium = null;
		this.SplashMedium = null;
		this.SplashSmall = null;
	}

	// Token: 0x04002CB4 RID: 11444
	public static FlyingMermaidLevelSplashManager splashManager;

	// Token: 0x04002CB5 RID: 11445
	public Transform spawnRootFront;

	// Token: 0x04002CB6 RID: 11446
	public Transform spawnRootBack;

	// Token: 0x04002CB7 RID: 11447
	[SerializeField]
	private Effect MegasplashLarge;

	// Token: 0x04002CB8 RID: 11448
	[SerializeField]
	private Effect MegasplashMedium;

	// Token: 0x04002CB9 RID: 11449
	[SerializeField]
	private Effect SplashMedium;

	// Token: 0x04002CBA RID: 11450
	[SerializeField]
	private Effect SplashSmall;
}
