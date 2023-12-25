using System;
using UnityEngine;

// Token: 0x0200094A RID: 2378
public class MapNPCBoatman : AbstractMonoBehaviour
{
	// Token: 0x0600378C RID: 14220 RVA: 0x001FEBF0 File Offset: 0x001FCFF0
	private void Start()
	{
		this.AddDialoguerEvents();
		Dialoguer.SetGlobalFloat(22, (float)((!PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted) ? 0 : 1));
		if (PlayerData.Data.CurrentMap == Scenes.scene_map_world_1)
		{
			base.GetComponent<SpriteRenderer>().sortingOrder = 1000;
		}
		PlayerData.Data.hasUnlockedBoatman = true;
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x0600378D RID: 14221 RVA: 0x001FEC58 File Offset: 0x001FD058
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x0600378E RID: 14222 RVA: 0x001FEC60 File Offset: 0x001FD060
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onStarted += this.OnDialoguerStart;
		Dialoguer.events.onEnded += this.OnDialoguerEnd;
	}

	// Token: 0x0600378F RID: 14223 RVA: 0x001FECB0 File Offset: 0x001FD0B0
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onStarted -= this.OnDialoguerStart;
		Dialoguer.events.onEnded -= this.OnDialoguerEnd;
	}

	// Token: 0x06003790 RID: 14224 RVA: 0x001FED00 File Offset: 0x001FD100
	private void SetOptions()
	{
		SpeechBubble instance = SpeechBubble.Instance;
		Scenes currentMap = PlayerData.Data.CurrentMap;
		switch (currentMap)
		{
		case Scenes.scene_map_world_1:
			instance.HideOptionByIndex(0);
			break;
		case Scenes.scene_map_world_2:
			instance.HideOptionByIndex(1);
			break;
		case Scenes.scene_map_world_3:
			instance.HideOptionByIndex(2);
			break;
		default:
			if (currentMap == Scenes.scene_map_world_DLC)
			{
				instance.HideOptionByIndex(3);
			}
			break;
		}
		if (!PlayerData.Data.GetMapData(Scenes.scene_map_world_2).sessionStarted)
		{
			instance.HideOptionByIndex(1);
		}
		if (!PlayerData.Data.GetMapData(Scenes.scene_map_world_3).sessionStarted)
		{
			instance.HideOptionByIndex(2);
		}
	}

	// Token: 0x06003791 RID: 14225 RVA: 0x001FEDA8 File Offset: 0x001FD1A8
	private void SelectWorld(string metadata)
	{
		if (this.selectionMade)
		{
			return;
		}
		int num;
		Parser.IntTryParse(metadata, out num);
		if (num > -1)
		{
			base.GetComponent<MapDialogueInteraction>().enabled = false;
			this.selectionMade = true;
			AudioManager.Play("sfx_worldmap_boattravel_accept");
			if (num == 3)
			{
				if (PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).sessionStarted)
				{
					SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
					PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
				}
				else
				{
					PlayerData.Data.Gift(PlayerId.PlayerOne, Charm.charm_chalice);
					PlayerData.Data.Gift(PlayerId.PlayerTwo, Charm.charm_chalice);
					PlayerData.Data.shouldShowChaliceTooltip = true;
					Cutscene.Load(Scenes.scene_level_kitchen, Scenes.scene_cutscene_dlc_intro, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass);
					PlayerData.Data.GetMapData(Scenes.scene_map_world_DLC).enteringFrom = PlayerData.MapData.EntryMethod.None;
					PlayerData.Data.CurrentMap = Scenes.scene_map_world_DLC;
				}
			}
			else if (num != 0)
			{
				if (num != 1)
				{
					if (num == 2)
					{
						PlayerData.Data.GetMapData(Scenes.scene_map_world_3).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
						SceneLoader.LoadScene(Scenes.scene_map_world_3, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
					}
				}
				else
				{
					PlayerData.Data.GetMapData(Scenes.scene_map_world_2).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
					SceneLoader.LoadScene(Scenes.scene_map_world_2, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
				}
			}
			else
			{
				PlayerData.Data.GetMapData(Scenes.scene_map_world_1).enteringFrom = PlayerData.MapData.EntryMethod.Boatman;
				SceneLoader.LoadScene(Scenes.scene_map_world_1, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			}
		}
	}

	// Token: 0x06003792 RID: 14226 RVA: 0x001FEF00 File Offset: 0x001FD300
	private void Update()
	{
		this.blinkTimer -= CupheadTime.Delta;
		if (this.blinkTimer < 0f)
		{
			this.blinkTimer = this.blinkRange.RandomFloat();
			base.animator.SetTrigger("Blink");
		}
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x001FEF55 File Offset: 0x001FD355
	private void OnDialoguerStart()
	{
		base.animator.SetBool("Talk", true);
	}

	// Token: 0x06003794 RID: 14228 RVA: 0x001FEF68 File Offset: 0x001FD368
	private void OnDialoguerEnd()
	{
		base.animator.SetBool("Talk", false);
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x001FEF7B File Offset: 0x001FD37B
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "BoatmanSetOptions")
		{
			this.SetOptions();
		}
		if (message == "BoatmanSelection")
		{
			this.SelectWorld(metadata);
		}
	}

	// Token: 0x04003F9C RID: 16284
	private const int DIALOGUER_BOATMAN_STATE = 22;

	// Token: 0x04003F9D RID: 16285
	private const int W1 = 0;

	// Token: 0x04003F9E RID: 16286
	private const int W2 = 1;

	// Token: 0x04003F9F RID: 16287
	private const int W3 = 2;

	// Token: 0x04003FA0 RID: 16288
	private const int WDLC = 3;

	// Token: 0x04003FA1 RID: 16289
	[SerializeField]
	private MinMax blinkRange = new MinMax(2.5f, 4.5f);

	// Token: 0x04003FA2 RID: 16290
	private float blinkTimer;

	// Token: 0x04003FA3 RID: 16291
	private bool selectionMade;
}
