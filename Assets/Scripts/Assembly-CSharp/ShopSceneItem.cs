using UnityEngine;

public class ShopSceneItem : AbstractMonoBehaviour
{
	public ItemType itemType;
	public Weapon weapon;
	public Super super;
	public Charm charm;
	public SpriteRenderer spriteInactive;
	public SpriteRenderer spriteSelected;
	public SpriteRenderer spritePurchased;
	public SpriteRenderer spriteShadowObject;
	public Sprite spriteShadow;
	public float cantPurchaseYMovementPosition;
	public float cantPurchaseYMovementValue;
	public Vector3 poofOffset;
	public Vector3 endPosition;
	public PlayerId player;
	public Vector3 startPosition;
	public Vector3 originalShadowScale;
	public SpriteRenderer buyAnimation;
	[SerializeField]
	private bool isDLCItem;
}
