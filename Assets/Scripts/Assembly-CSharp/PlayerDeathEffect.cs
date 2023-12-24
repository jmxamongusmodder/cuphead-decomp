using UnityEngine;

public class PlayerDeathEffect : AbstractMonoBehaviour
{
	[SerializeField]
	protected SpriteRenderer cuphead;
	[SerializeField]
	protected SpriteRenderer mugman;
	[SerializeField]
	protected SpriteRenderer chalice;
	[SerializeField]
	protected PlayerDeathParrySwitch parrySwitch;
	[SerializeField]
	private LevelPlayerDeathEffect explosionPrefab;
	[SerializeField]
	private SpriteRenderer effect;
	[SerializeField]
	private SpriteRenderer chaliceEffect;
}
