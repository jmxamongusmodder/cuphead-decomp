using System;

// Token: 0x02000936 RID: 2358
public class MapDicePalaceSceneLoader : MapSceneLoader
{
	// Token: 0x06003722 RID: 14114 RVA: 0x001FC2EF File Offset: 0x001FA6EF
	protected override void LoadScene()
	{
		if (!PlayerData.Data.GetLevelData(Levels.DicePalaceMain).played)
		{
			SceneLoader.LoadScene(this.scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
		else
		{
			SceneLoader.LoadScene(Scenes.scene_level_dice_palace_main, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}
}
