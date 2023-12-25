using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000888 RID: 2184
public class TreePlatformingLevelButterfly : AbstractPausableComponent
{
	// Token: 0x1700043F RID: 1087
	// (get) Token: 0x060032C1 RID: 12993 RVA: 0x001D78A4 File Offset: 0x001D5CA4
	// (set) Token: 0x060032C2 RID: 12994 RVA: 0x001D78AC File Offset: 0x001D5CAC
	public bool isActive { get; private set; }

	// Token: 0x060032C3 RID: 12995 RVA: 0x001D78B8 File Offset: 0x001D5CB8
	private void Start()
	{
		this.maxCounter = UnityEngine.Random.Range(4, 7);
		if (this.sprite3.GetComponent<ParrySwitch>() != null)
		{
			this.sprite3.GetComponent<ParrySwitch>().OnActivate += this.Deactivate;
		}
	}

	// Token: 0x060032C4 RID: 12996 RVA: 0x001D7904 File Offset: 0x001D5D04
	public void Init(Vector2 velocity, float scale, int color, MinMax velMinMax)
	{
		this.isActive = true;
		base.transform.SetScale(new float?(scale), null, null);
		base.transform.SetEulerAngles(null, null, new float?((scale >= 0f) ? base.transform.eulerAngles.z : (-base.transform.eulerAngles.z)));
		this.velocity = velocity;
		this.velMinMax = velMinMax;
		this.SelectColor(color);
		this.Setup();
	}

	// Token: 0x060032C5 RID: 12997 RVA: 0x001D79B0 File Offset: 0x001D5DB0
	private void Setup()
	{
		string stateName = "P" + UnityEngine.Random.Range(1, 5).ToStringInvariant();
		base.animator.Play(stateName);
		base.StartCoroutine(this.check_dist_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.switch_y_cr());
		base.StartCoroutine(this.adjust_x_speed(this.velMinMax));
	}

	// Token: 0x060032C6 RID: 12998 RVA: 0x001D7A1A File Offset: 0x001D5E1A
	public void Deactivate()
	{
		this.isActive = false;
		this.StopAllCoroutines();
		this.sprite1.SetActive(false);
		this.sprite2.SetActive(false);
		this.sprite3.SetActive(false);
	}

	// Token: 0x060032C7 RID: 12999 RVA: 0x001D7A50 File Offset: 0x001D5E50
	private void SelectColor(int color)
	{
		this.sprite1.SetActive(false);
		this.sprite2.SetActive(false);
		this.sprite3.SetActive(false);
		if (color != 1)
		{
			if (color != 2)
			{
				if (color == 3)
				{
					this.sprite3.SetActive(true);
				}
			}
			else
			{
				this.sprite2.SetActive(true);
			}
		}
		else
		{
			this.sprite1.SetActive(true);
		}
	}

	// Token: 0x060032C8 RID: 13000 RVA: 0x001D7AD0 File Offset: 0x001D5ED0
	private IEnumerator move_cr()
	{
		for (;;)
		{
			this.frameTime += CupheadTime.Delta;
			if (this.frameTime > 0.083333336f)
			{
				this.frameTime -= 0.083333336f;
				Vector2 vector = base.transform.position;
				vector += this.velocity * CupheadTime.Delta;
				base.transform.position = vector;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032C9 RID: 13001 RVA: 0x001D7AEC File Offset: 0x001D5EEC
	private IEnumerator switch_y_cr()
	{
		float time = UnityEngine.Random.Range(1f, 2f);
		float t = 0f;
		float startVel = this.velocity.y;
		float endVel = -this.velocity.y;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(3f, 6f));
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.velocity.y = Mathf.Lerp(startVel, endVel, t / time);
				yield return null;
			}
			this.velocity.y = endVel;
			t = 0f;
			startVel = this.velocity.y;
			endVel = -this.velocity.y;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032CA RID: 13002 RVA: 0x001D7B08 File Offset: 0x001D5F08
	private IEnumerator adjust_x_speed(MinMax adjustment)
	{
		float t = 0f;
		float time = UnityEngine.Random.Range(1f, 2f);
		float startVel = this.velocity.x;
		float endVel = (Mathf.Sign(this.velocity.x) != 1f) ? (-adjustment.RandomFloat()) : adjustment.RandomFloat();
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(4f, 6f));
			while (t < time)
			{
				this.velocity.x = Mathf.Lerp(startVel, endVel, time);
				yield return null;
			}
			this.velocity.x = endVel;
			endVel = ((Mathf.Sign(this.velocity.x) != 1f) ? (-adjustment.RandomFloat()) : adjustment.RandomFloat());
			startVel = this.velocity.x;
			t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032CB RID: 13003 RVA: 0x001D7B2C File Offset: 0x001D5F2C
	private IEnumerator check_dist_cr()
	{
		for (;;)
		{
			float dist = Vector3.Distance(CupheadLevelCamera.Current.transform.position, base.transform.position);
			if (dist > 2000f)
			{
				break;
			}
			yield return null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x060032CC RID: 13004 RVA: 0x001D7B48 File Offset: 0x001D5F48
	private void Counter()
	{
		if (this.loopCounter < this.maxCounter)
		{
			this.loopCounter++;
		}
		else
		{
			string stateName = "P" + UnityEngine.Random.Range(1, 5).ToStringInvariant();
			base.animator.Play(stateName);
			this.maxCounter = UnityEngine.Random.Range(4, 6);
			this.loopCounter = 0;
		}
	}

	// Token: 0x04003AEE RID: 15086
	[SerializeField]
	private GameObject sprite1;

	// Token: 0x04003AEF RID: 15087
	[SerializeField]
	private GameObject sprite2;

	// Token: 0x04003AF0 RID: 15088
	[SerializeField]
	private GameObject sprite3;

	// Token: 0x04003AF1 RID: 15089
	private const float FRAME_TIME = 0.083333336f;

	// Token: 0x04003AF2 RID: 15090
	private Vector2 velocity;

	// Token: 0x04003AF3 RID: 15091
	private float rotation;

	// Token: 0x04003AF4 RID: 15092
	private float frameTime;

	// Token: 0x04003AF5 RID: 15093
	private int loopCounter;

	// Token: 0x04003AF6 RID: 15094
	private int maxCounter;

	// Token: 0x04003AF7 RID: 15095
	private MinMax velMinMax;
}
