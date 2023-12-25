using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009B1 RID: 2481
public class SplashScreenMDHR : AbstractMonoBehaviour
{
	// Token: 0x06003A30 RID: 14896 RVA: 0x0021155A File Offset: 0x0020F95A
	protected override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		this.input = new CupheadInput.AnyPlayerInput(false);
		this.fader.color = new Color(0f, 0f, 0f, 1f);
	}

	// Token: 0x06003A31 RID: 14897 RVA: 0x00211598 File Offset: 0x0020F998
	private void Start()
	{
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06003A32 RID: 14898 RVA: 0x002115A7 File Offset: 0x0020F9A7
	private void Update()
	{
		if (this.fading)
		{
			return;
		}
		if (this.input.GetButtonDown(CupheadButton.Accept))
		{
			this.BeginFadeOut();
		}
	}

	// Token: 0x06003A33 RID: 14899 RVA: 0x002115D0 File Offset: 0x0020F9D0
	private IEnumerator go_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		base.animator.Play("Logo");
		yield break;
	}

	// Token: 0x06003A34 RID: 14900 RVA: 0x002115EB File Offset: 0x0020F9EB
	private void BeginFadeOut()
	{
		if (this.fading)
		{
			return;
		}
		this.fading = true;
		SceneLoader.properties.transitionEnd = SceneLoader.Transition.Iris;
		SceneLoader.properties.transitionStart = SceneLoader.Transition.Iris;
		SceneLoader.LoadScene(Scenes.scene_title, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x04004268 RID: 17000
	private const float WAIT = 3f;

	// Token: 0x04004269 RID: 17001
	[SerializeField]
	private SpriteRenderer fader;

	// Token: 0x0400426A RID: 17002
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x0400426B RID: 17003
	private bool fading;
}
