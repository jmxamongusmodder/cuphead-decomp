using UnityEngine;

public class MapNPCMusic : MonoBehaviour
{
	public enum MusicType
	{
		Regular = 0,
		Minimalist = 1,
	}

	[SerializeField]
	private MusicType musicType;
}
