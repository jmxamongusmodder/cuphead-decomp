using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020009B0 RID: 2480
public class UserProfileDisplay : AbstractMonoBehaviour
{
	// Token: 0x06003A2C RID: 14892 RVA: 0x002113A4 File Offset: 0x0020F7A4
	protected override void Awake()
	{
		this.root.SetActive(false);
		this.gamerPic.gameObject.SetActive(false);
	}

	// Token: 0x06003A2D RID: 14893 RVA: 0x002113C4 File Offset: 0x0020F7C4
	private void Start()
	{
		this.isSlotSelect = (SceneManager.GetActiveScene().name == Scenes.scene_slot_select.ToString());
	}

	// Token: 0x06003A2E RID: 14894 RVA: 0x002113F8 File Offset: 0x0020F7F8
	private void Update()
	{
		if (OnlineManager.Instance.Interface.SupportsMultipleUsers || (this.showForMultipleUsersUnsupported && OnlineManager.Instance.Interface.SupportsUserSignIn))
		{
			OnlineUser user = OnlineManager.Instance.Interface.GetUser(this.player);
			if (user != null)
			{
				this.root.SetActive(true);
				string name = user.Name;
				if (this.gamerTag.text != name)
				{
					this.gamerTag.text = name;
				}
				Texture2D profilePic = OnlineManager.Instance.Interface.GetProfilePic(this.player);
				if (profilePic != null && this.currentPicUser != user)
				{
					this.currentPicUser = user;
					Sprite sprite = Sprite.Create(profilePic, new Rect(0f, 0f, (float)profilePic.width, (float)profilePic.height), new Vector2(0.5f, 0.5f));
					this.gamerPic.sprite = sprite;
					this.gamerPic.gameObject.SetActive(true);
				}
				else if (profilePic == null)
				{
					this.gamerPic.gameObject.SetActive(false);
				}
			}
			else
			{
				this.root.SetActive(false);
			}
		}
		else
		{
			this.root.SetActive(false);
		}
	}

	// Token: 0x0400425E RID: 16990
	[SerializeField]
	private Image gamerPic;

	// Token: 0x0400425F RID: 16991
	[SerializeField]
	private Text gamerTag;

	// Token: 0x04004260 RID: 16992
	[SerializeField]
	private PlayerId player;

	// Token: 0x04004261 RID: 16993
	[SerializeField]
	private GameObject root;

	// Token: 0x04004262 RID: 16994
	[SerializeField]
	private GameObject switchPromptRoot;

	// Token: 0x04004263 RID: 16995
	[SerializeField]
	private bool showForMultipleUsersUnsupported;

	// Token: 0x04004264 RID: 16996
	[SerializeField]
	private Sprite defaultAvatarCuphead;

	// Token: 0x04004265 RID: 16997
	[SerializeField]
	private Sprite defaultAvatarMugman;

	// Token: 0x04004266 RID: 16998
	private OnlineUser currentPicUser;

	// Token: 0x04004267 RID: 16999
	private bool isSlotSelect;
}
