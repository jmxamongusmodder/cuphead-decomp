using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class ClownLevelCoaster : AbstractCollidableObject
{
	// Token: 0x060019E4 RID: 6628 RVA: 0x000EC8A3 File Offset: 0x000EACA3
	public void Init(Vector3 backStartPos, Vector3 frontStartPos, LevelProperties.Clown.Coaster properties, float coasterLength, float coasterSize, ClownLevelLights warningLights)
	{
		base.transform.position = backStartPos;
		this.frontStartPos = frontStartPos;
		this.properties = properties;
		this.coasterLength = coasterLength;
		this.coasterSize = coasterSize;
		this.warningLights = warningLights;
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x060019E5 RID: 6629 RVA: 0x000EC8E3 File Offset: 0x000EACE3
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060019E6 RID: 6630 RVA: 0x000EC8F8 File Offset: 0x000EACF8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			hit.GetComponent<LevelPlayerController>().OnPitKnockUp(base.transform.position.y, 0.85f);
		}
	}

	// Token: 0x060019E7 RID: 6631 RVA: 0x000EC944 File Offset: 0x000EAD44
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060019E8 RID: 6632 RVA: 0x000EC95C File Offset: 0x000EAD5C
	private void ChompSound()
	{
		if (this.inView)
		{
			AudioManager.Play("clown_coaster_main");
			this.emitAudioFromObject.Add("clown_coaster_main");
		}
	}

	// Token: 0x060019E9 RID: 6633 RVA: 0x000EC984 File Offset: 0x000EAD84
	private IEnumerator move_coaster_front_cr()
	{
		bool lightsOff = true;
		AudioManager.PlayLoop("sfx_clown_coaster_ratchet_loop");
		this.emitAudioFromObject.Add("sfx_clown_coaster_ratchet_loop");
		yield return CupheadTime.WaitForSeconds(this, this.properties.coasterBackToFrontDelay);
		while (base.transform.position.x > -640f - this.coasterSize * this.coasterLength)
		{
			base.transform.position += -base.transform.right * this.properties.coasterSpeed * CupheadTime.Delta;
			if (base.transform.position.x < 640f + 0.2f * this.coasterSize && lightsOff)
			{
				this.warningLights.StartWarningLights();
				lightsOff = false;
			}
			if (base.transform.position.x < -640f - this.coasterSize * this.coasterLength && !lightsOff)
			{
				this.warningLights.StopWarningLights();
				lightsOff = true;
			}
			yield return null;
		}
		this.inView = false;
		AudioManager.Stop("sfx_clown_coaster_ratchet_loop");
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060019EA RID: 6634 RVA: 0x000EC9A0 File Offset: 0x000EADA0
	private IEnumerator move_coaster_back_cr()
	{
		this.inView = true;
		AudioManager.PlayLoop("sfx_clown_coaster_distant_by");
		this.emitAudioFromObject.Add("sfx_clown_coaster_distant_by");
		while (base.transform.position.x < 640f + this.coasterSize * 0.44f * this.coasterLength)
		{
			base.transform.position += base.transform.right * this.properties.coasterSpeed * CupheadTime.Delta;
			yield return null;
		}
		AudioManager.Stop("sfx_clown_coaster_distant_by");
		this.FrontCoasterSetup();
		yield return null;
		yield break;
	}

	// Token: 0x060019EB RID: 6635 RVA: 0x000EC9BC File Offset: 0x000EADBC
	public void BackCoasterSetup()
	{
		int num = 97;
		this.knobCollider.enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		this.childrenSprites = base.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in this.childrenSprites)
		{
			if (spriteRenderer.gameObject.GetComponent<LevelPlatform>() != null)
			{
				spriteRenderer.gameObject.GetComponent<Collider2D>().enabled = false;
			}
		}
		base.transform.SetScale(new float?(-0.44f), new float?(0.44f), null);
		base.transform.SetEulerAngles(null, null, new float?(17.57f));
		this.sprite.sortingLayerName = "Background";
		this.sprite.sortingOrder = 45;
		Color color;
		ColorUtility.TryParseHtmlString("#b6b6b6", out color);
		this.sprite.color = color;
		for (int j = 0; j < this.childrenSprites.Length; j++)
		{
			this.childrenSprites[j].color = color;
			this.childrenSprites[j].sortingLayerName = "Background";
			this.childrenSprites[j].sortingOrder = num - j;
			if (this.childrenSprites[j].GetComponent<ClownLevelRiders>() != null)
			{
				this.childrenSprites[j].GetComponent<Collider2D>().enabled = false;
			}
		}
		this.knobSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
		this.knobSprite.GetComponent<SpriteRenderer>().sortingOrder = num + 1;
		base.StartCoroutine(this.move_coaster_back_cr());
	}

	// Token: 0x060019EC RID: 6636 RVA: 0x000ECB74 File Offset: 0x000EAF74
	public void FrontCoasterSetup()
	{
		int num = 79;
		this.knobCollider.enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		foreach (SpriteRenderer spriteRenderer in this.childrenSprites)
		{
			if (spriteRenderer.gameObject.GetComponent<LevelPlatform>() != null)
			{
				spriteRenderer.gameObject.GetComponent<Collider2D>().enabled = true;
			}
		}
		base.transform.position = this.frontStartPos;
		base.transform.SetScale(new float?(1f), new float?(1f), null);
		base.transform.SetEulerAngles(null, null, new float?(0f));
		this.sprite.sortingLayerName = "Player";
		this.sprite.sortingOrder = 80;
		Color color;
		ColorUtility.TryParseHtmlString("#FFFFFFFF", out color);
		this.sprite.color = color;
		for (int j = 0; j < this.childrenSprites.Length; j++)
		{
			this.childrenSprites[j].color = color;
			if (this.childrenSprites[j].transform.parent == base.transform || this.childrenSprites[j].transform == base.transform)
			{
				this.childrenSprites[j].sortingLayerName = "Player";
				this.childrenSprites[j].sortingOrder = num - j;
			}
			else if (this.childrenSprites[j].GetComponent<ClownLevelRiders>() != null)
			{
				this.childrenSprites[j].GetComponent<Collider2D>().enabled = true;
				this.childrenSprites[j].sortingLayerName = "Player";
				this.childrenSprites[j].sortingOrder = num - j;
				this.childrenSprites[j].GetComponent<ClownLevelRiders>().FrontLayers(num - j);
			}
			else if (!this.childrenSprites[j].transform.parent.GetComponent<ClownLevelRiders>())
			{
				this.childrenSprites[j].sortingLayerName = "Default";
				this.childrenSprites[j].sortingOrder = 4;
			}
		}
		this.knobSprite.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
		this.knobSprite.GetComponent<SpriteRenderer>().sortingOrder = num + 1;
		base.StartCoroutine(this.move_coaster_front_cr());
	}

	// Token: 0x060019ED RID: 6637 RVA: 0x000ECDFB File Offset: 0x000EB1FB
	protected void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002301 RID: 8961
	[SerializeField]
	private SpriteRenderer knobSprite;

	// Token: 0x04002302 RID: 8962
	[SerializeField]
	private Collider2D knobCollider;

	// Token: 0x04002303 RID: 8963
	public Transform pieceRoot;

	// Token: 0x04002304 RID: 8964
	private LevelProperties.Clown.Coaster properties;

	// Token: 0x04002305 RID: 8965
	private ClownLevelLights warningLights;

	// Token: 0x04002306 RID: 8966
	private SpriteRenderer sprite;

	// Token: 0x04002307 RID: 8967
	private SpriteRenderer[] childrenSprites;

	// Token: 0x04002308 RID: 8968
	private Vector3 frontStartPos;

	// Token: 0x04002309 RID: 8969
	private DamageDealer damageDealer;

	// Token: 0x0400230A RID: 8970
	private float coasterSize;

	// Token: 0x0400230B RID: 8971
	private float coasterLength;

	// Token: 0x0400230C RID: 8972
	private bool inView;
}
