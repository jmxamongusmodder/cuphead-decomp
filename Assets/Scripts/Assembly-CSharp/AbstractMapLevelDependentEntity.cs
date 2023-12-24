using UnityEngine;

public class AbstractMapLevelDependentEntity : AbstractMonoBehaviour
{
	[SerializeField]
	protected bool anyLevelPassesCheck;
	[SerializeField]
	protected Levels[] _levels;
	[SerializeField]
	private Vector2 _cameraPosition;
	public bool panCamera;
}
