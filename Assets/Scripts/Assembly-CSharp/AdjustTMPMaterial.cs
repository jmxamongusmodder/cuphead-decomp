using UnityEngine;
using System;
using TMPro;

public class AdjustTMPMaterial : MonoBehaviour
{
	[Serializable]
	public struct MaterialData
	{
		public Localization.Languages language;
		public string materialName;
	}

	[SerializeField]
	private TextMeshProUGUI text;
	[SerializeField]
	private Material defaultMaterial;
	[SerializeField]
	private MaterialData[] materials;
}
