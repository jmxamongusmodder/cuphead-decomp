using UnityEngine;

public class ChessBishopLevelCandle : AbstractCollidableObject
{
	[SerializeField]
	private Transform blowoutRoot;
	[SerializeField]
	private GameObject smoke;
	[SerializeField]
	private GameObject vanquishFX;
	[SerializeField]
	private GameObject vanquishSpark;
	[SerializeField]
	private GameObject shadow;
	[SerializeField]
	private float staggerLoopTime;
	[SerializeField]
	private float floatAmplitude;
	[SerializeField]
	private bool offsetFloat;
	[SerializeField]
	private GameObject glow;
	[SerializeField]
	private ChessBishopLevelIntroCandle introCandle;
	[SerializeField]
	private bool isLastIntro;
	[SerializeField]
	private float introOvershoot;
}
