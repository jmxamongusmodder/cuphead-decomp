using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B9 RID: 1977
public class SallyStagePlayLevelUmbrella : GroundHomingMovement
{
	// Token: 0x06002CAC RID: 11436 RVA: 0x001A5795 File Offset: 0x001A3B95
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.drop_cr());
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x001A57AA File Offset: 0x001A3BAA
	public void GetProperties(LevelProperties.SallyStagePlay properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x001A57B4 File Offset: 0x001A3BB4
	private void FixedUpdate()
	{
		this.shadow.transform.SetLocalEulerAngles(null, null, new float?(-base.transform.localEulerAngles.z));
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x001A57FC File Offset: 0x001A3BFC
	private IEnumerator drop_cr()
	{
		AudioManager.PlayLoop("sally_umbrella_fall");
		this.emitAudioFromObject.Add("sally_umbrella_fall");
		YieldInstruction wait = new WaitForFixedUpdate();
		float speed = 200f;
		float t = 0f;
		float rotateAmount = 12f;
		float rotateT = 0f;
		float time = 0.2f;
		while (base.transform.position.y > (float)Level.Current.Ground + 100f)
		{
			t += CupheadTime.FixedDelta;
			base.transform.AddPosition(0f, -speed * CupheadTime.FixedDelta, 0f);
			rotateT += CupheadTime.FixedDelta;
			float phase = Mathf.Sin(rotateT / time);
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * rotateAmount));
			yield return wait;
		}
		this.maxSpeed = this.properties.CurrentState.umbrella.homingMaxSpeed;
		this.acceleration = this.properties.CurrentState.umbrella.homingAcceleration;
		this.bounceRatio = this.properties.CurrentState.umbrella.homingBounceRatio;
		UnityEngine.Object.Destroy(base.GetComponent<LevelCharacterShadow>());
		AudioManager.Stop("sally_umbrella_fall");
		AudioManager.PlayLoop("sally_umbrella_spin_loop");
		this.emitAudioFromObject.Add("sally_umbrella_spin_loop");
		base.animator.SetTrigger("Land");
		base.EnableHoming = true;
		this.enableRadishRot = true;
		base.StartCoroutine(this.check_dir_change_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x001A5818 File Offset: 0x001A3C18
	private IEnumerator check_dir_change_cr()
	{
		for (;;)
		{
			if (base.MoveDirection != this.moveDir)
			{
				AudioManager.Play("sally_umbrella_change_direction");
				this.emitAudioFromObject.Add("sally_umbrella_change_direction");
				this.moveDir = base.MoveDirection;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003531 RID: 13617
	[SerializeField]
	private Transform shadow;

	// Token: 0x04003532 RID: 13618
	private float startPosX;

	// Token: 0x04003533 RID: 13619
	private LevelProperties.SallyStagePlay properties;

	// Token: 0x04003534 RID: 13620
	private GroundHomingMovement.Direction moveDir;
}
