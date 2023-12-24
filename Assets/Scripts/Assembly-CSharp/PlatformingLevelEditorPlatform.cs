using UnityEngine;

public class PlatformingLevelEditorPlatform : AbstractMonoBehaviour
{
	public enum Type
	{
		Platform = 0,
		Solid = 1,
	}

	[SerializeField]
	private Type _type;
	[SerializeField]
	private bool _canFallThrough;
	[SerializeField]
	private Vector2 _size;
	[SerializeField]
	private Vector2 _offset;
}
