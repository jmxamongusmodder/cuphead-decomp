using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000885 RID: 2181
public class TreePlatformingLevelAudioArea : AbstractPausableComponent
{
	// Token: 0x060032A1 RID: 12961 RVA: 0x001D6B42 File Offset: 0x001D4F42
	private void Start()
	{
		base.StartCoroutine(this.check_sound_cr());
	}

	// Token: 0x060032A2 RID: 12962 RVA: 0x001D6B51 File Offset: 0x001D4F51
	private void PlaySound()
	{
		AudioManager.PlayLoop("amb_treecave");
		base.StartCoroutine(this.fade_volume_cr(true));
	}

	// Token: 0x060032A3 RID: 12963 RVA: 0x001D6B6B File Offset: 0x001D4F6B
	private void StopSound()
	{
		base.StartCoroutine(this.fade_volume_cr(false));
	}

	// Token: 0x060032A4 RID: 12964 RVA: 0x001D6B7C File Offset: 0x001D4F7C
	private IEnumerator fade_volume_cr(bool fadeIn)
	{
		this.isFading = true;
		float time = 1f;
		float t = 0f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			AudioManager.Attenuation("amb_treecave", true, (!fadeIn) ? (1f - t / time) : (t / time));
			yield return null;
		}
		if (!fadeIn)
		{
			AudioManager.Stop("amb_treecave");
		}
		this.isFading = false;
		yield return null;
		yield break;
	}

	// Token: 0x060032A5 RID: 12965 RVA: 0x001D6BA0 File Offset: 0x001D4FA0
	private IEnumerator check_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		for (;;)
		{
			if (player.transform.position.x > this.startPoint.transform.position.x && player.transform.position.x < this.endPoint.transform.position.x)
			{
				if (!AudioManager.CheckIfPlaying("amb_treecave"))
				{
					this.PlaySound();
				}
			}
			else if (AudioManager.CheckIfPlaying("amb_treecave") && !this.isFading)
			{
				this.StopSound();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032A6 RID: 12966 RVA: 0x001D6BBC File Offset: 0x001D4FBC
	private IEnumerator play_one_shots_cr()
	{
		MinMax delay = new MinMax(4f, 8f);
		for (;;)
		{
			if (AudioManager.CheckIfPlaying("amb_treecave"))
			{
				yield return CupheadTime.WaitForSeconds(this, delay);
				AudioManager.Play("NAME");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032A7 RID: 12967 RVA: 0x001D6BD8 File Offset: 0x001D4FD8
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector2(this.startPoint.position.x, this.startPoint.position.y + 1000f), new Vector2(this.startPoint.position.x, this.startPoint.position.y - 1000f));
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(new Vector2(this.endPoint.position.x, this.endPoint.position.y + 1000f), new Vector2(this.endPoint.position.x, this.endPoint.position.y - 1000f));
	}

	// Token: 0x04003AE5 RID: 15077
	[SerializeField]
	private Transform startPoint;

	// Token: 0x04003AE6 RID: 15078
	[SerializeField]
	private Transform endPoint;

	// Token: 0x04003AE7 RID: 15079
	private bool isFading;
}
