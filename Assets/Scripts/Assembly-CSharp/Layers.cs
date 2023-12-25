using System;

// Token: 0x020002F7 RID: 759
public enum Layers
{
	// Token: 0x0400116E RID: 4462
	Default,
	// Token: 0x0400116F RID: 4463
	TransparentFX,
	// Token: 0x04001170 RID: 4464
	Ignore_Raycast,
	// Token: 0x04001171 RID: 4465
	Water = 4,
	// Token: 0x04001172 RID: 4466
	UI,
	// Token: 0x04001173 RID: 4467
	Player = 8,
	// Token: 0x04001174 RID: 4468
	PlayerHurt,
	// Token: 0x04001175 RID: 4469
	Loader,
	// Token: 0x04001176 RID: 4470
	PlayerTrigger,
	// Token: 0x04001177 RID: 4471
	Projectile,
	// Token: 0x04001178 RID: 4472
	ProjectileNoClip,
	// Token: 0x04001179 RID: 4473
	HUD = 15,
	// Token: 0x0400117A RID: 4474
	AchievementToast,
	// Token: 0x0400117B RID: 4475
	Bounds_Walls = 18,
	// Token: 0x0400117C RID: 4476
	Bounds_Ceiling,
	// Token: 0x0400117D RID: 4477
	Bounds_Ground,
	// Token: 0x0400117E RID: 4478
	Map_Secret = 25,
	// Token: 0x0400117F RID: 4479
	Editor_Locked = 30,
	// Token: 0x04001180 RID: 4480
	Renderer
}
