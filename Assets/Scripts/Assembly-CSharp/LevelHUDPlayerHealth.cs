using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200048D RID: 1165
public class LevelHUDPlayerHealth : AbstractLevelHUDComponent
{
	// Token: 0x0600124D RID: 4685 RVA: 0x000A982E File Offset: 0x000A7C2E
	protected override void Awake()
	{
		base.Awake();
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x0600124E RID: 4686 RVA: 0x000A9842 File Offset: 0x000A7C42
	public override void Init(LevelHUDPlayer hud)
	{
		base.Init(hud);
		this.lastHealth = base._player.stats.Health;
		this.OnHealthChanged(base._player.stats.Health);
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x000A9878 File Offset: 0x000A7C78
	public void OnHealthChanged(int health)
	{
		base.animator.SetInteger("Health", Mathf.Clamp(health, 0, base._player.stats.HealthMax));
		base.animator.Play("Entry");
		if (this.lastHealth != health)
		{
			this.OnChangedHealth();
		}
		this.lastHealth = health;
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x000A98D5 File Offset: 0x000A7CD5
	private void OnChangedHealth()
	{
		base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutSine, new AbstractMonoBehaviour.TweenUpdateHandler(this.ChangedHealthTween));
	}

	// Token: 0x06001251 RID: 4689 RVA: 0x000A98FC File Offset: 0x000A7CFC
	private void ChangedHealthTween(float value)
	{
		Color white = Color.white;
		Color gray = Color.gray;
		base.transform.localScale = Vector3.one * Mathf.Lerp(2f, 1f, value);
		this.image.color = Color.Lerp(white, gray, value);
	}

	// Token: 0x04001BBB RID: 7099
	private const string HealthParameter = "Health";

	// Token: 0x04001BBC RID: 7100
	private Image image;

	// Token: 0x04001BBD RID: 7101
	private int lastHealth;
}
