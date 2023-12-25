using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000898 RID: 2200
public class TreePlatformingLevelWoodpecker : PlatformingLevelShootingEnemy
{
	// Token: 0x06003329 RID: 13097 RVA: 0x001DC58C File Offset: 0x001DA98C
	protected override void Start()
	{
		base.Start();
		this.isDown = false;
		this.startPos = base.transform.position;
		this.endPos = this.setEndPos.transform.position;
		this.midPos = new Vector3(this.endPos.x, this.endPos.y + 200f);
		base.GetComponent<DamageReceiver>().enabled = false;
	}

	// Token: 0x0600332A RID: 13098 RVA: 0x001DC60F File Offset: 0x001DAA0F
	protected override void Shoot()
	{
		if (!this.isDown)
		{
			base.StartCoroutine(this.move_down_cr());
		}
	}

	// Token: 0x0600332B RID: 13099 RVA: 0x001DC62C File Offset: 0x001DAA2C
	private IEnumerator move_down_cr()
	{
		this.isDown = true;
		base.animator.SetBool("movingDown", true);
		float t = 0f;
		Vector2 start = base.transform.position;
		while (t < base.Properties.WoodpeckermoveDownTime)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / base.Properties.WoodpeckermoveDownTime);
			base.transform.position = Vector2.Lerp(start, this.midPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.midPos;
		start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, base.Properties.WoodpeckerWarningDuration);
		base.animator.SetTrigger("Continue");
		t = 0f;
		while (t < 0.2f)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / 0.5f);
			base.transform.position = Vector2.Lerp(start, this.endPos, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		base.transform.position = this.endPos;
		start = base.transform.position;
		base.animator.SetBool("isAttacking", true);
		CupheadLevelCamera.Current.Shake(10f, base.Properties.WoodpeckerAttackDuration, false);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.WoodpeckerAttackDuration);
		base.animator.SetBool("isAttacking", false);
		while (t < base.Properties.WoodpeckermoveUpTime)
		{
			float val3 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / base.Properties.WoodpeckermoveUpTime);
			base.transform.position = Vector2.Lerp(start, this.startPos, val3);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetBool("movingDown", false);
		this.isDown = false;
		yield return null;
		yield break;
	}

	// Token: 0x0600332C RID: 13100 RVA: 0x001DC647 File Offset: 0x001DAA47
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Gizmos.DrawWireSphere(this.endPos, 100f);
	}

	// Token: 0x0600332D RID: 13101 RVA: 0x001DC682 File Offset: 0x001DAA82
	private void SoundWoodpeckerStart()
	{
		AudioManager.Play("level_platform_woodpecker_attack_start");
		this.emitAudioFromObject.Add("level_platform_woodpecker_attack_start");
	}

	// Token: 0x04003B6C RID: 15212
	[SerializeField]
	private Transform setEndPos;

	// Token: 0x04003B6D RID: 15213
	private Vector2 endPos;

	// Token: 0x04003B6E RID: 15214
	private Vector2 midPos;

	// Token: 0x04003B6F RID: 15215
	private Vector2 startPos;

	// Token: 0x04003B70 RID: 15216
	private bool isDown;
}
