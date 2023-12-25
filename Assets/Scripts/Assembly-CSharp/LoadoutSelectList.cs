using System;
using System.Collections;
using RektTransform;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009A8 RID: 2472
public class LoadoutSelectList : AbstractMonoBehaviour
{
	// Token: 0x060039FE RID: 14846 RVA: 0x0020F4A2 File Offset: 0x0020D8A2
	protected override void Awake()
	{
		base.Awake();
		this.SetupList();
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x0020F4B0 File Offset: 0x0020D8B0
	private void SetupList()
	{
		Array array = null;
		switch (this.mode)
		{
		case LoadoutSelectList.Mode.Primary:
		case LoadoutSelectList.Mode.Secondary:
			array = Enum.GetValues(typeof(Weapon));
			break;
		case LoadoutSelectList.Mode.Super:
			array = Enum.GetValues(typeof(Super));
			break;
		case LoadoutSelectList.Mode.Charm:
			array = Enum.GetValues(typeof(Charm));
			break;
		case LoadoutSelectList.Mode.Difficulty:
			array = Enum.GetValues(typeof(Level.Mode));
			break;
		}
		int num = 0;
		IEnumerator enumerator = array.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LoadoutSelectList.<SetupList>c__AnonStorey0 <SetupList>c__AnonStorey = new LoadoutSelectList.<SetupList>c__AnonStorey0();
				<SetupList>c__AnonStorey.value = enumerator.Current;
				<SetupList>c__AnonStorey.$this = this;
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.button.gameObject);
				Button b = gameObject.GetComponent<Button>();
				string text = <SetupList>c__AnonStorey.value.ToString().Replace("level_", string.Empty).Replace("weapon_", string.Empty).Replace("super_", string.Empty).Replace("charm_", string.Empty);
				b.name = <SetupList>c__AnonStorey.value.ToString();
				gameObject.GetComponentInChildren<Text>().text = text;
				b.onClick.AddListener(delegate()
				{
					PlayerId[] array2 = new PlayerId[]
					{
						PlayerId.PlayerOne,
						PlayerId.PlayerTwo
					};
					foreach (PlayerId player in array2)
					{
						PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(player);
						switch (<SetupList>c__AnonStorey.$this.mode)
						{
						case LoadoutSelectList.Mode.Primary:
							playerLoadout.primaryWeapon = (Weapon)<SetupList>c__AnonStorey.value;
							break;
						case LoadoutSelectList.Mode.Secondary:
							playerLoadout.secondaryWeapon = (Weapon)<SetupList>c__AnonStorey.value;
							break;
						case LoadoutSelectList.Mode.Super:
							playerLoadout.super = (Super)<SetupList>c__AnonStorey.value;
							break;
						case LoadoutSelectList.Mode.Charm:
						{
							Charm charm = (Charm)<SetupList>c__AnonStorey.value;
							playerLoadout.charm = charm;
							break;
						}
						case LoadoutSelectList.Mode.Difficulty:
							Level.SetCurrentMode((Level.Mode)<SetupList>c__AnonStorey.value);
							break;
						}
					}
					if (<SetupList>c__AnonStorey.$this.selectedButton != null)
					{
						ColorBlock colors = <SetupList>c__AnonStorey.$this.selectedButton.colors;
						colors.colorMultiplier = 2f;
						<SetupList>c__AnonStorey.$this.selectedButton.colors = colors;
					}
					<SetupList>c__AnonStorey.$this.selectedButton = b;
					ColorBlock colors2 = <SetupList>c__AnonStorey.$this.selectedButton.colors;
					colors2.colorMultiplier = 4f;
					<SetupList>c__AnonStorey.$this.selectedButton.colors = colors2;
				});
				b.transform.SetParent(this.button.transform.parent);
				b.transform.ResetLocalTransforms();
				num++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this.button.gameObject.SetActive(false);
		this.contentPanel.SetHeight(30f * (float)num);
	}

	// Token: 0x040041E1 RID: 16865
	[SerializeField]
	public LoadoutSelectList.Mode mode;

	// Token: 0x040041E2 RID: 16866
	public Button button;

	// Token: 0x040041E3 RID: 16867
	public Button selectedButton;

	// Token: 0x040041E4 RID: 16868
	public RectTransform contentPanel;

	// Token: 0x020009A9 RID: 2473
	public enum Mode
	{
		// Token: 0x040041E6 RID: 16870
		Primary,
		// Token: 0x040041E7 RID: 16871
		Secondary,
		// Token: 0x040041E8 RID: 16872
		Super,
		// Token: 0x040041E9 RID: 16873
		Charm,
		// Token: 0x040041EA RID: 16874
		Difficulty
	}
}
