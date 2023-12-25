using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C4F RID: 3151
	[AddComponentMenu("")]
	[RequireComponent(typeof(CanvasGroup))]
	public class Window : MonoBehaviour
	{
		// Token: 0x170007C8 RID: 1992
		// (get) Token: 0x06004D6B RID: 19819 RVA: 0x00265D73 File Offset: 0x00264173
		public bool hasFocus
		{
			get
			{
				return this._isFocusedCallback != null && this._isFocusedCallback(this._id);
			}
		}

		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06004D6C RID: 19820 RVA: 0x00265D97 File Offset: 0x00264197
		public int id
		{
			get
			{
				return this._id;
			}
		}

		// Token: 0x170007CA RID: 1994
		// (get) Token: 0x06004D6D RID: 19821 RVA: 0x00265D9F File Offset: 0x0026419F
		public RectTransform rectTransform
		{
			get
			{
				if (this._rectTransform == null)
				{
					this._rectTransform = base.gameObject.GetComponent<RectTransform>();
				}
				return this._rectTransform;
			}
		}

		// Token: 0x170007CB RID: 1995
		// (get) Token: 0x06004D6E RID: 19822 RVA: 0x00265DC9 File Offset: 0x002641C9
		public Text titleText
		{
			get
			{
				return this._titleText;
			}
		}

		// Token: 0x170007CC RID: 1996
		// (get) Token: 0x06004D6F RID: 19823 RVA: 0x00265DD1 File Offset: 0x002641D1
		public List<Text> contentText
		{
			get
			{
				return this._contentText;
			}
		}

		// Token: 0x170007CD RID: 1997
		// (get) Token: 0x06004D70 RID: 19824 RVA: 0x00265DD9 File Offset: 0x002641D9
		// (set) Token: 0x06004D71 RID: 19825 RVA: 0x00265DE1 File Offset: 0x002641E1
		public GameObject defaultUIElement
		{
			get
			{
				return this._defaultUIElement;
			}
			set
			{
				this._defaultUIElement = value;
			}
		}

		// Token: 0x170007CE RID: 1998
		// (get) Token: 0x06004D72 RID: 19826 RVA: 0x00265DEA File Offset: 0x002641EA
		// (set) Token: 0x06004D73 RID: 19827 RVA: 0x00265DF2 File Offset: 0x002641F2
		public Action<int> updateCallback
		{
			get
			{
				return this._updateCallback;
			}
			set
			{
				this._updateCallback = value;
			}
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06004D74 RID: 19828 RVA: 0x00265DFB File Offset: 0x002641FB
		public Window.Timer timer
		{
			get
			{
				return this._timer;
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06004D75 RID: 19829 RVA: 0x00265E04 File Offset: 0x00264204
		// (set) Token: 0x06004D76 RID: 19830 RVA: 0x00265E28 File Offset: 0x00264228
		public int width
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.x;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.x = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06004D77 RID: 19831 RVA: 0x00265E58 File Offset: 0x00264258
		// (set) Token: 0x06004D78 RID: 19832 RVA: 0x00265E7C File Offset: 0x0026427C
		public int height
		{
			get
			{
				return (int)this.rectTransform.sizeDelta.y;
			}
			set
			{
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				sizeDelta.y = (float)value;
				this.rectTransform.sizeDelta = sizeDelta;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x06004D79 RID: 19833 RVA: 0x00265EAA File Offset: 0x002642AA
		protected bool initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x06004D7A RID: 19834 RVA: 0x00265EB2 File Offset: 0x002642B2
		private void OnEnable()
		{
			base.StartCoroutine("OnEnableAsync");
		}

		// Token: 0x06004D7B RID: 19835 RVA: 0x00265EC0 File Offset: 0x002642C0
		protected virtual void Update()
		{
			if (!this._initialized)
			{
				return;
			}
			if (!this.hasFocus)
			{
				return;
			}
			this.CheckUISelection();
			if (this._updateCallback != null)
			{
				this._updateCallback(this._id);
			}
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x00265EFC File Offset: 0x002642FC
		public virtual void Initialize(int id, Func<int, bool> isFocusedCallback)
		{
			if (this._initialized)
			{
				UnityEngine.Debug.LogError("Window is already initialized!");
				return;
			}
			this._id = id;
			this._isFocusedCallback = isFocusedCallback;
			this._timer = new Window.Timer();
			this._contentText = new List<Text>();
			this._canvasGroup = base.GetComponent<CanvasGroup>();
			this._initialized = true;
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x00265F56 File Offset: 0x00264356
		public void SetSize(int width, int height)
		{
			this.rectTransform.sizeDelta = new Vector2((float)width, (float)height);
		}

		// Token: 0x06004D7E RID: 19838 RVA: 0x00265F6C File Offset: 0x0026436C
		public void CreateTitleText(GameObject prefab, Vector2 offset)
		{
			this.CreateText(prefab, ref this._titleText, "Title Text", UIPivot.TopCenter, UIAnchor.TopHStretch, offset);
		}

		// Token: 0x06004D7F RID: 19839 RVA: 0x00265F8B File Offset: 0x0026438B
		public void CreateTitleText(GameObject prefab, Vector2 offset, string text)
		{
			this.CreateTitleText(prefab, offset);
			this.SetTitleText(text);
		}

		// Token: 0x06004D80 RID: 19840 RVA: 0x00265F9C File Offset: 0x0026439C
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			Text item = null;
			this.CreateText(prefab, ref item, "Content Text", pivot, anchor, offset);
			this._contentText.Add(item);
		}

		// Token: 0x06004D81 RID: 19841 RVA: 0x00265FC9 File Offset: 0x002643C9
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentText(prefab, pivot, anchor, offset, text, 0);
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x00265FD9 File Offset: 0x002643D9
		public void AddContentText(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text, int fontSize)
		{
			this.AddContentText(prefab, pivot, anchor, offset);
			this.SetContentText(text, fontSize, this._contentText.Count - 1);
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x00265FFD File Offset: 0x002643FD
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			this.CreateImage(prefab, "Image", pivot, anchor, offset);
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0026600F File Offset: 0x0026440F
		public void AddContentImage(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string text)
		{
			this.AddContentImage(prefab, pivot, anchor, offset);
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0026601C File Offset: 0x0026441C
		public void CreateButton(GameObject prefab, UIPivot pivot, UIAnchor anchor, Vector2 offset, string buttonText, UnityAction confirmCallback, UnityAction cancelCallback, bool setDefault)
		{
			if (prefab == null)
			{
				return;
			}
			ButtonInfo buttonInfo;
			GameObject gameObject = this.CreateButton(prefab, "Button", anchor, pivot, offset, out buttonInfo);
			if (gameObject == null)
			{
				return;
			}
			Button component = gameObject.GetComponent<Button>();
			if (confirmCallback != null)
			{
				component.onClick.AddListener(confirmCallback);
			}
			CustomButton customButton = component as CustomButton;
			if (cancelCallback != null && customButton != null)
			{
				customButton.CancelEvent += cancelCallback;
			}
			if (buttonInfo.text != null)
			{
				buttonInfo.text.text = buttonText;
			}
			if (setDefault)
			{
				this._defaultUIElement = gameObject;
			}
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x002660BF File Offset: 0x002644BF
		public string GetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return string.Empty;
			}
			return this._titleText.text;
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x002660E3 File Offset: 0x002644E3
		public void SetTitleText(string text)
		{
			if (this._titleText == null)
			{
				return;
			}
			this._titleText.text = text;
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x00266104 File Offset: 0x00264504
		public string GetContentText(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return string.Empty;
			}
			return this._contentText[index].text;
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0026615C File Offset: 0x0026455C
		public float GetContentTextHeight(int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return 0f;
			}
			return this._contentText[index].rectTransform.sizeDelta.y;
		}

		// Token: 0x06004D8A RID: 19850 RVA: 0x002661C0 File Offset: 0x002645C0
		public void SetContentText(string text, int fontsize, int index)
		{
			this.SetContentText(text, index);
			if (fontsize > 0)
			{
				this._contentText[index].fontSize = fontsize;
			}
		}

		// Token: 0x06004D8B RID: 19851 RVA: 0x002661E4 File Offset: 0x002645E4
		public void SetContentText(string text, int index)
		{
			if (this._contentText == null || this._contentText.Count <= index || this._contentText[index] == null)
			{
				return;
			}
			this._contentText[index].text = text;
		}

		// Token: 0x06004D8C RID: 19852 RVA: 0x00266237 File Offset: 0x00264637
		public void SetUpdateCallback(Action<int> callback)
		{
			this.updateCallback = callback;
		}

		// Token: 0x06004D8D RID: 19853 RVA: 0x00266240 File Offset: 0x00264640
		public virtual void TakeInputFocus()
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(this._defaultUIElement);
			this.Enable();
		}

		// Token: 0x06004D8E RID: 19854 RVA: 0x00266269 File Offset: 0x00264669
		public virtual void Enable()
		{
			this._canvasGroup.interactable = true;
		}

		// Token: 0x06004D8F RID: 19855 RVA: 0x00266277 File Offset: 0x00264677
		public virtual void Disable()
		{
			this._canvasGroup.interactable = false;
		}

		// Token: 0x06004D90 RID: 19856 RVA: 0x00266285 File Offset: 0x00264685
		public virtual void Cancel()
		{
			if (!this.initialized)
			{
				return;
			}
			if (this.cancelCallback != null)
			{
				this.cancelCallback();
			}
		}

		// Token: 0x06004D91 RID: 19857 RVA: 0x002662AC File Offset: 0x002646AC
		private void CreateText(GameObject prefab, ref Text textComponent, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			if (textComponent != null)
			{
				UnityEngine.Debug.LogError("Window already has " + name + "!");
				return;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<Text>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return;
			}
			textComponent = gameObject.GetComponent<Text>();
		}

		// Token: 0x06004D92 RID: 19858 RVA: 0x0026633C File Offset: 0x0026473C
		private void CreateImage(GameObject prefab, string name, UIPivot pivot, UIAnchor anchor, Vector2 offset)
		{
			if (prefab == null || this.content == null)
			{
				return;
			}
			UITools.InstantiateGUIObject<Image>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
		}

		// Token: 0x06004D93 RID: 19859 RVA: 0x00266390 File Offset: 0x00264790
		private GameObject CreateButton(GameObject prefab, string name, UIAnchor anchor, UIPivot pivot, Vector2 offset, out ButtonInfo buttonInfo)
		{
			buttonInfo = null;
			if (prefab == null)
			{
				return null;
			}
			GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(prefab, this.content.transform, name, pivot, anchor.min, anchor.max, offset);
			if (gameObject == null)
			{
				return null;
			}
			buttonInfo = gameObject.GetComponent<ButtonInfo>();
			Button component = gameObject.GetComponent<Button>();
			if (component == null)
			{
				UnityEngine.Debug.Log("Button prefab is missing Button component!");
				return null;
			}
			if (buttonInfo == null)
			{
				UnityEngine.Debug.Log("Button prefab is missing ButtonInfo component!");
				return null;
			}
			return gameObject;
		}

		// Token: 0x06004D94 RID: 19860 RVA: 0x0026642C File Offset: 0x0026482C
		private IEnumerator OnEnableAsync()
		{
			yield return 1;
			if (EventSystem.current == null)
			{
				yield break;
			}
			if (this.defaultUIElement != null)
			{
				EventSystem.current.SetSelectedGameObject(this.defaultUIElement);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(null);
			}
			yield break;
		}

		// Token: 0x06004D95 RID: 19861 RVA: 0x00266448 File Offset: 0x00264848
		private void CheckUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (EventSystem.current == null)
			{
				return;
			}
			if (EventSystem.current.currentSelectedGameObject == null)
			{
				this.RestoreDefaultOrLastUISelection();
			}
			this.lastUISelection = EventSystem.current.currentSelectedGameObject;
		}

		// Token: 0x06004D96 RID: 19862 RVA: 0x002664A0 File Offset: 0x002648A0
		private void RestoreDefaultOrLastUISelection()
		{
			if (!this.hasFocus)
			{
				return;
			}
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetUISelection(this._defaultUIElement);
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x06004D97 RID: 19863 RVA: 0x002664F3 File Offset: 0x002648F3
		private void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x04005199 RID: 20889
		public Image backgroundImage;

		// Token: 0x0400519A RID: 20890
		public GameObject content;

		// Token: 0x0400519B RID: 20891
		private bool _initialized;

		// Token: 0x0400519C RID: 20892
		private int _id = -1;

		// Token: 0x0400519D RID: 20893
		private RectTransform _rectTransform;

		// Token: 0x0400519E RID: 20894
		private Text _titleText;

		// Token: 0x0400519F RID: 20895
		private List<Text> _contentText;

		// Token: 0x040051A0 RID: 20896
		private GameObject _defaultUIElement;

		// Token: 0x040051A1 RID: 20897
		private Action<int> _updateCallback;

		// Token: 0x040051A2 RID: 20898
		private Func<int, bool> _isFocusedCallback;

		// Token: 0x040051A3 RID: 20899
		private Window.Timer _timer;

		// Token: 0x040051A4 RID: 20900
		private CanvasGroup _canvasGroup;

		// Token: 0x040051A5 RID: 20901
		public UnityAction cancelCallback;

		// Token: 0x040051A6 RID: 20902
		private GameObject lastUISelection;

		// Token: 0x02000C50 RID: 3152
		public class Timer
		{
			// Token: 0x170007D3 RID: 2003
			// (get) Token: 0x06004D99 RID: 19865 RVA: 0x00266519 File Offset: 0x00264919
			public bool started
			{
				get
				{
					return this._started;
				}
			}

			// Token: 0x170007D4 RID: 2004
			// (get) Token: 0x06004D9A RID: 19866 RVA: 0x00266521 File Offset: 0x00264921
			public bool finished
			{
				get
				{
					if (!this.started)
					{
						return false;
					}
					if (Time.realtimeSinceStartup < this.end)
					{
						return false;
					}
					this._started = false;
					return true;
				}
			}

			// Token: 0x170007D5 RID: 2005
			// (get) Token: 0x06004D9B RID: 19867 RVA: 0x0026654A File Offset: 0x0026494A
			public float remaining
			{
				get
				{
					if (!this._started)
					{
						return 0f;
					}
					return this.end - Time.realtimeSinceStartup;
				}
			}

			// Token: 0x06004D9C RID: 19868 RVA: 0x00266569 File Offset: 0x00264969
			public void Start(float length)
			{
				this.end = Time.realtimeSinceStartup + length;
				this._started = true;
			}

			// Token: 0x06004D9D RID: 19869 RVA: 0x0026657F File Offset: 0x0026497F
			public void Stop()
			{
				this._started = false;
			}

			// Token: 0x040051A7 RID: 20903
			private bool _started;

			// Token: 0x040051A8 RID: 20904
			private float end;
		}
	}
}
