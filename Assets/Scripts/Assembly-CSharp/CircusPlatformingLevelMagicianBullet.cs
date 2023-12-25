using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020008A7 RID: 2215
public class CircusPlatformingLevelMagicianBullet : BasicProjectile
{
	// Token: 0x14000062 RID: 98
	// (add) Token: 0x0600339E RID: 13214 RVA: 0x001DFEF8 File Offset: 0x001DE2F8
	// (remove) Token: 0x0600339F RID: 13215 RVA: 0x001DFF30 File Offset: 0x001DE330
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnProjectileDeath;

	// Token: 0x17000446 RID: 1094
	// (get) Token: 0x060033A0 RID: 13216 RVA: 0x001DFF66 File Offset: 0x001DE366
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060033A1 RID: 13217 RVA: 0x001DFF70 File Offset: 0x001DE370
	protected override void Start()
	{
		base.Start();
		AudioManager.PlayLoop("circus_magician_magic_loop");
		this.emitAudioFromObject.Add("circus_magician_magic_loop");
		this.puffs.flipX = Rand.Bool();
		this.puffs.flipY = Rand.Bool();
		this.DestroyDistance = 0f;
	}

	// Token: 0x060033A2 RID: 13218 RVA: 0x001DFFC8 File Offset: 0x001DE3C8
	protected override void OnDestroy()
	{
		AudioManager.Stop("circus_magician_magic_loop");
		if (this.OnProjectileDeath != null)
		{
			this.OnProjectileDeath();
		}
		base.OnDestroy();
	}

	// Token: 0x04003BEF RID: 15343
	[SerializeField]
	private SpriteRenderer puffs;
}
