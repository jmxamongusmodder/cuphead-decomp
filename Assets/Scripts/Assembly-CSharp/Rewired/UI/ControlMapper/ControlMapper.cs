using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Rewired.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.UI.ControlMapper
{
	// Token: 0x02000C0D RID: 3085
	[AddComponentMenu("")]
	public class ControlMapper : MonoBehaviour
	{
		// Token: 0x140000E4 RID: 228
		// (add) Token: 0x0600499C RID: 18844 RVA: 0x00267793 File Offset: 0x00265B93
		// (remove) Token: 0x0600499D RID: 18845 RVA: 0x002677AC File Offset: 0x00265BAC
		public event Action ScreenClosedEvent
		{
			add
			{
				this._ScreenClosedEvent = (Action)Delegate.Combine(this._ScreenClosedEvent, value);
			}
			remove
			{
				this._ScreenClosedEvent = (Action)Delegate.Remove(this._ScreenClosedEvent, value);
			}
		}

		// Token: 0x140000E5 RID: 229
		// (add) Token: 0x0600499E RID: 18846 RVA: 0x002677C5 File Offset: 0x00265BC5
		// (remove) Token: 0x0600499F RID: 18847 RVA: 0x002677DE File Offset: 0x00265BDE
		public event Action ScreenOpenedEvent
		{
			add
			{
				this._ScreenOpenedEvent = (Action)Delegate.Combine(this._ScreenOpenedEvent, value);
			}
			remove
			{
				this._ScreenOpenedEvent = (Action)Delegate.Remove(this._ScreenOpenedEvent, value);
			}
		}

		// Token: 0x140000E6 RID: 230
		// (add) Token: 0x060049A0 RID: 18848 RVA: 0x002677F7 File Offset: 0x00265BF7
		// (remove) Token: 0x060049A1 RID: 18849 RVA: 0x00267810 File Offset: 0x00265C10
		public event Action PopupWindowClosedEvent
		{
			add
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Combine(this._PopupWindowClosedEvent, value);
			}
			remove
			{
				this._PopupWindowClosedEvent = (Action)Delegate.Remove(this._PopupWindowClosedEvent, value);
			}
		}

		// Token: 0x140000E7 RID: 231
		// (add) Token: 0x060049A2 RID: 18850 RVA: 0x00267829 File Offset: 0x00265C29
		// (remove) Token: 0x060049A3 RID: 18851 RVA: 0x00267842 File Offset: 0x00265C42
		public event Action PopupWindowOpenedEvent
		{
			add
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Combine(this._PopupWindowOpenedEvent, value);
			}
			remove
			{
				this._PopupWindowOpenedEvent = (Action)Delegate.Remove(this._PopupWindowOpenedEvent, value);
			}
		}

		// Token: 0x140000E8 RID: 232
		// (add) Token: 0x060049A4 RID: 18852 RVA: 0x0026785B File Offset: 0x00265C5B
		// (remove) Token: 0x060049A5 RID: 18853 RVA: 0x00267874 File Offset: 0x00265C74
		public event Action InputPollingStartedEvent
		{
			add
			{
				this._InputPollingStartedEvent = (Action)Delegate.Combine(this._InputPollingStartedEvent, value);
			}
			remove
			{
				this._InputPollingStartedEvent = (Action)Delegate.Remove(this._InputPollingStartedEvent, value);
			}
		}

		// Token: 0x140000E9 RID: 233
		// (add) Token: 0x060049A6 RID: 18854 RVA: 0x0026788D File Offset: 0x00265C8D
		// (remove) Token: 0x060049A7 RID: 18855 RVA: 0x002678A6 File Offset: 0x00265CA6
		public event Action InputPollingEndedEvent
		{
			add
			{
				this._InputPollingEndedEvent = (Action)Delegate.Combine(this._InputPollingEndedEvent, value);
			}
			remove
			{
				this._InputPollingEndedEvent = (Action)Delegate.Remove(this._InputPollingEndedEvent, value);
			}
		}

		// Token: 0x140000EA RID: 234
		// (add) Token: 0x060049A8 RID: 18856 RVA: 0x002678BF File Offset: 0x00265CBF
		// (remove) Token: 0x060049A9 RID: 18857 RVA: 0x002678CD File Offset: 0x00265CCD
		public event UnityAction onScreenClosed
		{
			add
			{
				this._onScreenClosed.AddListener(value);
			}
			remove
			{
				this._onScreenClosed.RemoveListener(value);
			}
		}

		// Token: 0x140000EB RID: 235
		// (add) Token: 0x060049AA RID: 18858 RVA: 0x002678DB File Offset: 0x00265CDB
		// (remove) Token: 0x060049AB RID: 18859 RVA: 0x002678E9 File Offset: 0x00265CE9
		public event UnityAction onScreenOpened
		{
			add
			{
				this._onScreenOpened.AddListener(value);
			}
			remove
			{
				this._onScreenOpened.RemoveListener(value);
			}
		}

		// Token: 0x140000EC RID: 236
		// (add) Token: 0x060049AC RID: 18860 RVA: 0x002678F7 File Offset: 0x00265CF7
		// (remove) Token: 0x060049AD RID: 18861 RVA: 0x00267905 File Offset: 0x00265D05
		public event UnityAction onPopupWindowClosed
		{
			add
			{
				this._onPopupWindowClosed.AddListener(value);
			}
			remove
			{
				this._onPopupWindowClosed.RemoveListener(value);
			}
		}

		// Token: 0x140000ED RID: 237
		// (add) Token: 0x060049AE RID: 18862 RVA: 0x00267913 File Offset: 0x00265D13
		// (remove) Token: 0x060049AF RID: 18863 RVA: 0x00267921 File Offset: 0x00265D21
		public event UnityAction onPopupWindowOpened
		{
			add
			{
				this._onPopupWindowOpened.AddListener(value);
			}
			remove
			{
				this._onPopupWindowOpened.RemoveListener(value);
			}
		}

		// Token: 0x140000EE RID: 238
		// (add) Token: 0x060049B0 RID: 18864 RVA: 0x0026792F File Offset: 0x00265D2F
		// (remove) Token: 0x060049B1 RID: 18865 RVA: 0x0026793D File Offset: 0x00265D3D
		public event UnityAction onInputPollingStarted
		{
			add
			{
				this._onInputPollingStarted.AddListener(value);
			}
			remove
			{
				this._onInputPollingStarted.RemoveListener(value);
			}
		}

		// Token: 0x140000EF RID: 239
		// (add) Token: 0x060049B2 RID: 18866 RVA: 0x0026794B File Offset: 0x00265D4B
		// (remove) Token: 0x060049B3 RID: 18867 RVA: 0x00267959 File Offset: 0x00265D59
		public event UnityAction onInputPollingEnded
		{
			add
			{
				this._onInputPollingEnded.AddListener(value);
			}
			remove
			{
				this._onInputPollingEnded.RemoveListener(value);
			}
		}

		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x060049B4 RID: 18868 RVA: 0x00267967 File Offset: 0x00265D67
		// (set) Token: 0x060049B5 RID: 18869 RVA: 0x0026796F File Offset: 0x00265D6F
		public InputManager rewiredInputManager
		{
			get
			{
				return this._rewiredInputManager;
			}
			set
			{
				this._rewiredInputManager = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x060049B6 RID: 18870 RVA: 0x0026797F File Offset: 0x00265D7F
		// (set) Token: 0x060049B7 RID: 18871 RVA: 0x00267987 File Offset: 0x00265D87
		public bool dontDestroyOnLoad
		{
			get
			{
				return this._dontDestroyOnLoad;
			}
			set
			{
				if (value != this._dontDestroyOnLoad && value)
				{
					UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
				}
				this._dontDestroyOnLoad = value;
			}
		}

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x060049B8 RID: 18872 RVA: 0x002679B2 File Offset: 0x00265DB2
		// (set) Token: 0x060049B9 RID: 18873 RVA: 0x002679BA File Offset: 0x00265DBA
		public int keyboardMapDefaultLayout
		{
			get
			{
				return this._keyboardMapDefaultLayout;
			}
			set
			{
				this._keyboardMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x060049BA RID: 18874 RVA: 0x002679CA File Offset: 0x00265DCA
		// (set) Token: 0x060049BB RID: 18875 RVA: 0x002679D2 File Offset: 0x00265DD2
		public int mouseMapDefaultLayout
		{
			get
			{
				return this._mouseMapDefaultLayout;
			}
			set
			{
				this._mouseMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060049BC RID: 18876 RVA: 0x002679E2 File Offset: 0x00265DE2
		// (set) Token: 0x060049BD RID: 18877 RVA: 0x002679EA File Offset: 0x00265DEA
		public int joystickMapDefaultLayout
		{
			get
			{
				return this._joystickMapDefaultLayout;
			}
			set
			{
				this._joystickMapDefaultLayout = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060049BE RID: 18878 RVA: 0x002679FA File Offset: 0x00265DFA
		// (set) Token: 0x060049BF RID: 18879 RVA: 0x00267A17 File Offset: 0x00265E17
		public bool showPlayers
		{
			get
			{
				return this._showPlayers && ReInput.players.playerCount > 1;
			}
			set
			{
				this._showPlayers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060049C0 RID: 18880 RVA: 0x00267A27 File Offset: 0x00265E27
		// (set) Token: 0x060049C1 RID: 18881 RVA: 0x00267A2F File Offset: 0x00265E2F
		public bool showControllers
		{
			get
			{
				return this._showControllers;
			}
			set
			{
				this._showControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060049C2 RID: 18882 RVA: 0x00267A3F File Offset: 0x00265E3F
		// (set) Token: 0x060049C3 RID: 18883 RVA: 0x00267A47 File Offset: 0x00265E47
		public bool showKeyboard
		{
			get
			{
				return this._showKeyboard;
			}
			set
			{
				this._showKeyboard = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060049C4 RID: 18884 RVA: 0x00267A57 File Offset: 0x00265E57
		// (set) Token: 0x060049C5 RID: 18885 RVA: 0x00267A5F File Offset: 0x00265E5F
		public bool showMouse
		{
			get
			{
				return this._showMouse;
			}
			set
			{
				this._showMouse = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060049C6 RID: 18886 RVA: 0x00267A6F File Offset: 0x00265E6F
		// (set) Token: 0x060049C7 RID: 18887 RVA: 0x00267A77 File Offset: 0x00265E77
		public int maxControllersPerPlayer
		{
			get
			{
				return this._maxControllersPerPlayer;
			}
			set
			{
				this._maxControllersPerPlayer = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060049C8 RID: 18888 RVA: 0x00267A87 File Offset: 0x00265E87
		// (set) Token: 0x060049C9 RID: 18889 RVA: 0x00267A8F File Offset: 0x00265E8F
		public bool showActionCategoryLabels
		{
			get
			{
				return this._showActionCategoryLabels;
			}
			set
			{
				this._showActionCategoryLabels = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060049CA RID: 18890 RVA: 0x00267A9F File Offset: 0x00265E9F
		// (set) Token: 0x060049CB RID: 18891 RVA: 0x00267AA7 File Offset: 0x00265EA7
		public int keyboardInputFieldCount
		{
			get
			{
				return this._keyboardInputFieldCount;
			}
			set
			{
				this._keyboardInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A5 RID: 1701
		// (get) Token: 0x060049CC RID: 18892 RVA: 0x00267AB7 File Offset: 0x00265EB7
		// (set) Token: 0x060049CD RID: 18893 RVA: 0x00267ABF File Offset: 0x00265EBF
		public int mouseInputFieldCount
		{
			get
			{
				return this._mouseInputFieldCount;
			}
			set
			{
				this._mouseInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060049CE RID: 18894 RVA: 0x00267ACF File Offset: 0x00265ECF
		// (set) Token: 0x060049CF RID: 18895 RVA: 0x00267AD7 File Offset: 0x00265ED7
		public int controllerInputFieldCount
		{
			get
			{
				return this._controllerInputFieldCount;
			}
			set
			{
				this._controllerInputFieldCount = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060049D0 RID: 18896 RVA: 0x00267AE7 File Offset: 0x00265EE7
		// (set) Token: 0x060049D1 RID: 18897 RVA: 0x00267AEF File Offset: 0x00265EEF
		public bool showFullAxisInputFields
		{
			get
			{
				return this._showFullAxisInputFields;
			}
			set
			{
				this._showFullAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060049D2 RID: 18898 RVA: 0x00267AFF File Offset: 0x00265EFF
		// (set) Token: 0x060049D3 RID: 18899 RVA: 0x00267B07 File Offset: 0x00265F07
		public bool showSplitAxisInputFields
		{
			get
			{
				return this._showSplitAxisInputFields;
			}
			set
			{
				this._showSplitAxisInputFields = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060049D4 RID: 18900 RVA: 0x00267B17 File Offset: 0x00265F17
		// (set) Token: 0x060049D5 RID: 18901 RVA: 0x00267B1F File Offset: 0x00265F1F
		public bool allowElementAssignmentConflicts
		{
			get
			{
				return this._allowElementAssignmentConflicts;
			}
			set
			{
				this._allowElementAssignmentConflicts = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060049D6 RID: 18902 RVA: 0x00267B2F File Offset: 0x00265F2F
		// (set) Token: 0x060049D7 RID: 18903 RVA: 0x00267B37 File Offset: 0x00265F37
		public int actionLabelWidth
		{
			get
			{
				return this._actionLabelWidth;
			}
			set
			{
				this._actionLabelWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060049D8 RID: 18904 RVA: 0x00267B47 File Offset: 0x00265F47
		// (set) Token: 0x060049D9 RID: 18905 RVA: 0x00267B4F File Offset: 0x00265F4F
		public int keyboardColMaxWidth
		{
			get
			{
				return this._keyboardColMaxWidth;
			}
			set
			{
				this._keyboardColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006AC RID: 1708
		// (get) Token: 0x060049DA RID: 18906 RVA: 0x00267B5F File Offset: 0x00265F5F
		// (set) Token: 0x060049DB RID: 18907 RVA: 0x00267B67 File Offset: 0x00265F67
		public int mouseColMaxWidth
		{
			get
			{
				return this._mouseColMaxWidth;
			}
			set
			{
				this._mouseColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006AD RID: 1709
		// (get) Token: 0x060049DC RID: 18908 RVA: 0x00267B77 File Offset: 0x00265F77
		// (set) Token: 0x060049DD RID: 18909 RVA: 0x00267B7F File Offset: 0x00265F7F
		public int controllerColMaxWidth
		{
			get
			{
				return this._controllerColMaxWidth;
			}
			set
			{
				this._controllerColMaxWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006AE RID: 1710
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x00267B8F File Offset: 0x00265F8F
		// (set) Token: 0x060049DF RID: 18911 RVA: 0x00267B97 File Offset: 0x00265F97
		public float inputRowHeight
		{
			get
			{
				return this._inputRowHeight;
			}
			set
			{
				this._inputRowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006AF RID: 1711
		// (get) Token: 0x060049E0 RID: 18912 RVA: 0x00267BA7 File Offset: 0x00265FA7
		// (set) Token: 0x060049E1 RID: 18913 RVA: 0x00267BAF File Offset: 0x00265FAF
		public float inputColumnSpacing
		{
			get
			{
				return this._inputColumnSpacing;
			}
			set
			{
				this._inputColumnSpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006B0 RID: 1712
		// (get) Token: 0x060049E2 RID: 18914 RVA: 0x00267BBF File Offset: 0x00265FBF
		// (set) Token: 0x060049E3 RID: 18915 RVA: 0x00267BC7 File Offset: 0x00265FC7
		public int inputRowCategorySpacing
		{
			get
			{
				return this._inputRowCategorySpacing;
			}
			set
			{
				this._inputRowCategorySpacing = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x060049E4 RID: 18916 RVA: 0x00267BD7 File Offset: 0x00265FD7
		// (set) Token: 0x060049E5 RID: 18917 RVA: 0x00267BDF File Offset: 0x00265FDF
		public int invertToggleWidth
		{
			get
			{
				return this._invertToggleWidth;
			}
			set
			{
				this._invertToggleWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x060049E6 RID: 18918 RVA: 0x00267BEF File Offset: 0x00265FEF
		// (set) Token: 0x060049E7 RID: 18919 RVA: 0x00267BF7 File Offset: 0x00265FF7
		public int defaultWindowWidth
		{
			get
			{
				return this._defaultWindowWidth;
			}
			set
			{
				this._defaultWindowWidth = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006B3 RID: 1715
		// (get) Token: 0x060049E8 RID: 18920 RVA: 0x00267C07 File Offset: 0x00266007
		// (set) Token: 0x060049E9 RID: 18921 RVA: 0x00267C0F File Offset: 0x0026600F
		public int defaultWindowHeight
		{
			get
			{
				return this._defaultWindowHeight;
			}
			set
			{
				this._defaultWindowHeight = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006B4 RID: 1716
		// (get) Token: 0x060049EA RID: 18922 RVA: 0x00267C1F File Offset: 0x0026601F
		// (set) Token: 0x060049EB RID: 18923 RVA: 0x00267C27 File Offset: 0x00266027
		public float controllerAssignmentTimeout
		{
			get
			{
				return this._controllerAssignmentTimeout;
			}
			set
			{
				this._controllerAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006B5 RID: 1717
		// (get) Token: 0x060049EC RID: 18924 RVA: 0x00267C37 File Offset: 0x00266037
		// (set) Token: 0x060049ED RID: 18925 RVA: 0x00267C3F File Offset: 0x0026603F
		public float preInputAssignmentTimeout
		{
			get
			{
				return this._preInputAssignmentTimeout;
			}
			set
			{
				this._preInputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x060049EE RID: 18926 RVA: 0x00267C4F File Offset: 0x0026604F
		// (set) Token: 0x060049EF RID: 18927 RVA: 0x00267C57 File Offset: 0x00266057
		public float inputAssignmentTimeout
		{
			get
			{
				return this._inputAssignmentTimeout;
			}
			set
			{
				this._inputAssignmentTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x060049F0 RID: 18928 RVA: 0x00267C67 File Offset: 0x00266067
		// (set) Token: 0x060049F1 RID: 18929 RVA: 0x00267C6F File Offset: 0x0026606F
		public float axisCalibrationTimeout
		{
			get
			{
				return this._axisCalibrationTimeout;
			}
			set
			{
				this._axisCalibrationTimeout = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x060049F2 RID: 18930 RVA: 0x00267C7F File Offset: 0x0026607F
		// (set) Token: 0x060049F3 RID: 18931 RVA: 0x00267C87 File Offset: 0x00266087
		public bool ignoreMouseXAxisAssignment
		{
			get
			{
				return this._ignoreMouseXAxisAssignment;
			}
			set
			{
				this._ignoreMouseXAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x060049F4 RID: 18932 RVA: 0x00267C97 File Offset: 0x00266097
		// (set) Token: 0x060049F5 RID: 18933 RVA: 0x00267C9F File Offset: 0x0026609F
		public bool ignoreMouseYAxisAssignment
		{
			get
			{
				return this._ignoreMouseYAxisAssignment;
			}
			set
			{
				this._ignoreMouseYAxisAssignment = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x060049F6 RID: 18934 RVA: 0x00267CAF File Offset: 0x002660AF
		// (set) Token: 0x060049F7 RID: 18935 RVA: 0x00267CB7 File Offset: 0x002660B7
		public bool universalCancelClosesScreen
		{
			get
			{
				return this._universalCancelClosesScreen;
			}
			set
			{
				this._universalCancelClosesScreen = value;
				this.InspectorPropertyChanged(false);
			}
		}

		// Token: 0x170006BB RID: 1723
		// (get) Token: 0x060049F8 RID: 18936 RVA: 0x00267CC7 File Offset: 0x002660C7
		// (set) Token: 0x060049F9 RID: 18937 RVA: 0x00267CCF File Offset: 0x002660CF
		public bool showInputBehaviorSettings
		{
			get
			{
				return this._showInputBehaviorSettings;
			}
			set
			{
				this._showInputBehaviorSettings = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006BC RID: 1724
		// (get) Token: 0x060049FA RID: 18938 RVA: 0x00267CDF File Offset: 0x002660DF
		// (set) Token: 0x060049FB RID: 18939 RVA: 0x00267CE7 File Offset: 0x002660E7
		public bool useThemeSettings
		{
			get
			{
				return this._useThemeSettings;
			}
			set
			{
				this._useThemeSettings = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006BD RID: 1725
		// (get) Token: 0x060049FC RID: 18940 RVA: 0x00267CF7 File Offset: 0x002660F7
		// (set) Token: 0x060049FD RID: 18941 RVA: 0x00267CFF File Offset: 0x002660FF
		public LanguageData language
		{
			get
			{
				return this._language;
			}
			set
			{
				this._language = value;
				if (this._language != null)
				{
					this._language.Initialize();
				}
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x060049FE RID: 18942 RVA: 0x00267D2B File Offset: 0x0026612B
		// (set) Token: 0x060049FF RID: 18943 RVA: 0x00267D33 File Offset: 0x00266133
		public bool showPlayersGroupLabel
		{
			get
			{
				return this._showPlayersGroupLabel;
			}
			set
			{
				this._showPlayersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06004A00 RID: 18944 RVA: 0x00267D43 File Offset: 0x00266143
		// (set) Token: 0x06004A01 RID: 18945 RVA: 0x00267D4B File Offset: 0x0026614B
		public bool showControllerGroupLabel
		{
			get
			{
				return this._showControllerGroupLabel;
			}
			set
			{
				this._showControllerGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06004A02 RID: 18946 RVA: 0x00267D5B File Offset: 0x0026615B
		// (set) Token: 0x06004A03 RID: 18947 RVA: 0x00267D63 File Offset: 0x00266163
		public bool showAssignedControllersGroupLabel
		{
			get
			{
				return this._showAssignedControllersGroupLabel;
			}
			set
			{
				this._showAssignedControllersGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06004A04 RID: 18948 RVA: 0x00267D73 File Offset: 0x00266173
		// (set) Token: 0x06004A05 RID: 18949 RVA: 0x00267D7B File Offset: 0x0026617B
		public bool showSettingsGroupLabel
		{
			get
			{
				return this._showSettingsGroupLabel;
			}
			set
			{
				this._showSettingsGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x06004A06 RID: 18950 RVA: 0x00267D8B File Offset: 0x0026618B
		// (set) Token: 0x06004A07 RID: 18951 RVA: 0x00267D93 File Offset: 0x00266193
		public bool showMapCategoriesGroupLabel
		{
			get
			{
				return this._showMapCategoriesGroupLabel;
			}
			set
			{
				this._showMapCategoriesGroupLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06004A08 RID: 18952 RVA: 0x00267DA3 File Offset: 0x002661A3
		// (set) Token: 0x06004A09 RID: 18953 RVA: 0x00267DAB File Offset: 0x002661AB
		public bool showControllerNameLabel
		{
			get
			{
				return this._showControllerNameLabel;
			}
			set
			{
				this._showControllerNameLabel = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06004A0A RID: 18954 RVA: 0x00267DBB File Offset: 0x002661BB
		// (set) Token: 0x06004A0B RID: 18955 RVA: 0x00267DC3 File Offset: 0x002661C3
		public bool showAssignedControllers
		{
			get
			{
				return this._showAssignedControllers;
			}
			set
			{
				this._showAssignedControllers = value;
				this.InspectorPropertyChanged(true);
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06004A0C RID: 18956 RVA: 0x00267DD3 File Offset: 0x002661D3
		// (set) Token: 0x06004A0D RID: 18957 RVA: 0x00267DDB File Offset: 0x002661DB
		public bool showControllerGroupButtons { get; set; }

		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06004A0E RID: 18958 RVA: 0x00267DE4 File Offset: 0x002661E4
		// (set) Token: 0x06004A0F RID: 18959 RVA: 0x00267DEC File Offset: 0x002661EC
		public Action restoreDefaultsDelegate
		{
			get
			{
				return this._restoreDefaultsDelegate;
			}
			set
			{
				this._restoreDefaultsDelegate = value;
			}
		}

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06004A10 RID: 18960 RVA: 0x00267DF8 File Offset: 0x002661F8
		public bool isOpen
		{
			get
			{
				if (!this.initialized)
				{
					return this.references.canvas != null && this.references.canvas.gameObject.activeInHierarchy;
				}
				return this.canvas.activeInHierarchy;
			}
		}

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06004A11 RID: 18961 RVA: 0x00267E4D File Offset: 0x0026624D
		private bool isFocused
		{
			get
			{
				return this.initialized && !this.windowManager.isWindowOpen;
			}
		}

		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x06004A12 RID: 18962 RVA: 0x00267E6A File Offset: 0x0026626A
		private bool inputAllowed
		{
			get
			{
				return this.blockInputOnFocusEndTime <= Time.unscaledTime && !InterruptingPrompt.IsInterrupting();
			}
		}

		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06004A13 RID: 18963 RVA: 0x00267E8C File Offset: 0x0026628C
		private int inputGridColumnCount
		{
			get
			{
				int num = 1;
				if (this._showKeyboard)
				{
					num++;
				}
				if (this._showMouse)
				{
					num++;
				}
				if (this._showControllers)
				{
					num++;
				}
				return num;
			}
		}

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06004A14 RID: 18964 RVA: 0x00267ECC File Offset: 0x002662CC
		private int inputGridWidth
		{
			get
			{
				return this._actionLabelWidth + ((!this._showKeyboard) ? 0 : this._keyboardColMaxWidth) + ((!this._showMouse) ? 0 : this._mouseColMaxWidth) + ((!this._showControllers) ? 0 : this._controllerColMaxWidth) + (int)((float)(this.inputGridColumnCount - 1) * this._inputColumnSpacing);
			}
		}

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06004A15 RID: 18965 RVA: 0x00267F39 File Offset: 0x00266339
		private Player currentPlayer
		{
			get
			{
				return ReInput.players.GetPlayer(this.currentPlayerId);
			}
		}

		// Token: 0x170006CD RID: 1741
		// (get) Token: 0x06004A16 RID: 18966 RVA: 0x00267F4B File Offset: 0x0026634B
		private InputCategory currentMapCategory
		{
			get
			{
				return ReInput.mapping.GetMapCategory(this.currentMapCategoryId);
			}
		}

		// Token: 0x170006CE RID: 1742
		// (get) Token: 0x06004A17 RID: 18967 RVA: 0x00267F60 File Offset: 0x00266360
		private ControlMapper.MappingSet currentMappingSet
		{
			get
			{
				if (this.currentMapCategoryId < 0)
				{
					return null;
				}
				for (int i = 0; i < this._mappingSets.Length; i++)
				{
					if (this._mappingSets[i].mapCategoryId == this.currentMapCategoryId)
					{
						return this._mappingSets[i];
					}
				}
				return null;
			}
		}

		// Token: 0x170006CF RID: 1743
		// (get) Token: 0x06004A18 RID: 18968 RVA: 0x00267FB6 File Offset: 0x002663B6
		private Joystick currentJoystick
		{
			get
			{
				return ReInput.controllers.GetJoystick(this.currentJoystickId);
			}
		}

		// Token: 0x170006D0 RID: 1744
		// (get) Token: 0x06004A19 RID: 18969 RVA: 0x00267FC8 File Offset: 0x002663C8
		private bool isJoystickSelected
		{
			get
			{
				return this.currentJoystickId >= 0;
			}
		}

		// Token: 0x170006D1 RID: 1745
		// (get) Token: 0x06004A1A RID: 18970 RVA: 0x00267FD6 File Offset: 0x002663D6
		private GameObject currentUISelection
		{
			get
			{
				return (!(EventSystem.current != null)) ? null : EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x170006D2 RID: 1746
		// (get) Token: 0x06004A1B RID: 18971 RVA: 0x00267FF8 File Offset: 0x002663F8
		private bool showSettings
		{
			get
			{
				return this._showInputBehaviorSettings && this._inputBehaviorSettings.Length > 0;
			}
		}

		// Token: 0x170006D3 RID: 1747
		// (get) Token: 0x06004A1C RID: 18972 RVA: 0x00268013 File Offset: 0x00266413
		private bool showMapCategories
		{
			get
			{
				return this._mappingSets != null && this._mappingSets.Length > 1;
			}
		}

		// Token: 0x06004A1D RID: 18973 RVA: 0x00268033 File Offset: 0x00266433
		private void Awake()
		{
			if (this._dontDestroyOnLoad)
			{
				UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			}
			this.PreInitialize();
			if (this.isOpen)
			{
				this.Initialize();
				this.Open(true);
			}
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x0026806E File Offset: 0x0026646E
		private void Start()
		{
			if (this._openOnStart)
			{
				this.Open(false);
			}
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x00268082 File Offset: 0x00266482
		private void Update()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			this.CheckUISelection();
		}

		// Token: 0x06004A20 RID: 18976 RVA: 0x002680A2 File Offset: 0x002664A2
		private void OnDestroy()
		{
			ReInput.ControllerConnectedEvent -= this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent -= this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent -= this.OnJoystickPreDisconnect;
			this.UnsubscribeMenuControlInputEvents();
		}

		// Token: 0x06004A21 RID: 18977 RVA: 0x002680DD File Offset: 0x002664DD
		private void PreInitialize()
		{
			if (!ReInput.isReady)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: Rewired has not been initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			this.SubscribeMenuControlInputEvents();
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x002680FC File Offset: 0x002664FC
		private void Initialize()
		{
			if (this.initialized)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			this.currentPlayerId = Mathf.Clamp(this.currentPlayerId, 0, 1);
			if (this._rewiredInputManager == null)
			{
				this._rewiredInputManager = UnityEngine.Object.FindObjectOfType<InputManager>();
				if (this._rewiredInputManager == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: A Rewired Input Manager was not assigned in the inspector or found in the current scene! Control Mapper will not function.");
					return;
				}
			}
			if (ControlMapper.Instance != null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: Only one ControlMapper can exist at one time!");
				return;
			}
			ControlMapper.Instance = this;
			if (this.prefabs == null || !this.prefabs.Check())
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: All prefabs must be assigned in the inspector!");
				return;
			}
			if (this.references == null || !this.references.Check())
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: All references must be assigned in the inspector!");
				return;
			}
			this.references.inputGridLayoutElement = this.references.inputGridContainer.GetComponent<LayoutElement>();
			if (this.references.inputGridLayoutElement == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: InputGridContainer is missing LayoutElement component!");
				return;
			}
			if (this._showKeyboard && this._keyboardInputFieldCount < 1)
			{
				UnityEngine.Debug.LogWarning("Rewired Control Mapper: Keyboard Input Fields must be at least 1!");
				this._keyboardInputFieldCount = 1;
			}
			if (this._showMouse && this._mouseInputFieldCount < 1)
			{
				UnityEngine.Debug.LogWarning("Rewired Control Mapper: Mouse Input Fields must be at least 1!");
				this._mouseInputFieldCount = 1;
			}
			if (this._showControllers && this._controllerInputFieldCount < 1)
			{
				UnityEngine.Debug.LogWarning("Rewired Control Mapper: Controller Input Fields must be at least 1!");
				this._controllerInputFieldCount = 1;
			}
			if (this._maxControllersPerPlayer < 0)
			{
				UnityEngine.Debug.LogWarning("Rewired Control Mapper: Max Controllers Per Player must be at least 0 (no limit)!");
				this._maxControllersPerPlayer = 0;
			}
			if (this._useThemeSettings && this._themeSettings == null)
			{
				UnityEngine.Debug.LogWarning("Rewired Control Mapper: To use theming, Theme Settings must be set in the inspector! Theming has been disabled.");
				this._useThemeSettings = false;
			}
			if (this._language == null)
			{
				UnityEngine.Debug.LogError("Rawired UI: Language must be set in the inspector!");
				return;
			}
			this._language.Initialize();
			this.inputFieldActivatedDelegate = new Action<InputFieldInfo>(this.OnInputFieldActivated);
			this.inputFieldInvertToggleStateChangedDelegate = new Action<ToggleInfo, bool>(this.OnInputFieldInvertToggleStateChanged);
			ReInput.ControllerConnectedEvent += this.OnJoystickConnected;
			ReInput.ControllerDisconnectedEvent += this.OnJoystickDisconnected;
			ReInput.ControllerPreDisconnectEvent += this.OnJoystickPreDisconnect;
			PlayerManager.OnControlsChanged += this.OnControlsChanged;
			this.playerCount = ReInput.players.playerCount;
			this.canvas = this.references.canvas.gameObject;
			this.windowManager = new ControlMapper.WindowManager(this.prefabs.window, this.prefabs.fader, this.references.canvas.transform);
			this.playerButtons = new List<ControlMapper.GUIButton>();
			this.mapCategoryButtons = new List<ControlMapper.GUIButton>();
			this.assignedControllerButtons = new List<ControlMapper.GUIButton>();
			this.miscInstantiatedObjects = new List<GameObject>();
			this.currentMapCategoryId = this._mappingSets[0].mapCategoryId;
			this.Draw();
			this.CreateInputGrid();
			this.CreateLayout();
			this.SubscribeFixedUISelectionEvents();
			this.initialized = true;
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x00268414 File Offset: 0x00266814
		private void OnJoystickConnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x0026843A File Offset: 0x0026683A
		private void OnJoystickDisconnected(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
			this.ClearVarsOnJoystickChange();
			this.ForceRefresh();
		}

		// Token: 0x06004A25 RID: 18981 RVA: 0x00268460 File Offset: 0x00266860
		private void OnJoystickPreDisconnect(ControllerStatusChangedEventArgs args)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this._showControllers)
			{
				return;
			}
		}

		// Token: 0x06004A26 RID: 18982 RVA: 0x0026847C File Offset: 0x0026687C
		public void OnButtonActivated(ButtonInfo buttonInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			AudioManager.Play("level_menu_select");
			string identifier = buttonInfo.identifier;
			switch (identifier)
			{
			case "PlayerSelection":
				this.OnPlayerSelected(buttonInfo.intData, true);
				break;
			case "AssignedControllerSelection":
				this.OnControllerSelected(buttonInfo.intData);
				break;
			case "RemoveController":
				this.OnRemoveCurrentController();
				break;
			case "AssignController":
				this.ShowAssignControllerWindow();
				break;
			case "CalibrateController":
				this.ShowCalibrateControllerWindow();
				break;
			case "EditInputBehaviors":
				this.ShowEditInputBehaviorsWindow();
				break;
			case "MapCategorySelection":
				this.OnMapCategorySelected(buttonInfo.intData, true);
				break;
			case "Done":
				this.Close(true);
				break;
			case "RestoreDefaults":
				this.OnRestoreDefaults();
				break;
			case "ToggleRumble":
				this.ToggleRumble();
				break;
			}
		}

		// Token: 0x06004A27 RID: 18983 RVA: 0x00268611 File Offset: 0x00266A11
		private void ToggleRumble()
		{
			SettingsData.Data.canVibrate = !SettingsData.Data.canVibrate;
			SettingsData.Save();
			this.UpdateRumbleText();
		}

		// Token: 0x06004A28 RID: 18984 RVA: 0x00268638 File Offset: 0x00266A38
		private void UpdateRumbleText()
		{
			if (this._rumbleButtonText != null)
			{
				this._rumbleButtonText.text = Localization.Translate((!SettingsData.Data.canVibrate) ? "ToggleRumbleOff" : "ToggleRumbleOn").text;
			}
		}

		// Token: 0x06004A29 RID: 18985 RVA: 0x0026868C File Offset: 0x00266A8C
		private void OnEnable()
		{
			this.UpdateRumbleText();
		}

		// Token: 0x06004A2A RID: 18986 RVA: 0x00268694 File Offset: 0x00266A94
		public void OnInputFieldActivated(InputFieldInfo fieldInfo)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			AudioManager.Play("level_menu_select");
			if (this.currentPlayer == null)
			{
				return;
			}
			InputAction action = ReInput.mapping.GetAction(fieldInfo.actionId);
			if (action == null)
			{
				return;
			}
			string text;
			if (action.type == InputActionType.Button)
			{
				text = action.descriptiveName;
			}
			else
			{
				if (action.type != InputActionType.Axis)
				{
					throw new NotImplementedException();
				}
				if (fieldInfo.axisRange == AxisRange.Full)
				{
					text = action.descriptiveName;
				}
				else if (fieldInfo.axisRange == AxisRange.Positive)
				{
					if (string.IsNullOrEmpty(action.positiveDescriptiveName))
					{
						text = action.descriptiveName + " +";
					}
					else
					{
						text = action.positiveDescriptiveName;
					}
				}
				else
				{
					if (fieldInfo.axisRange != AxisRange.Negative)
					{
						throw new NotImplementedException();
					}
					if (string.IsNullOrEmpty(action.negativeDescriptiveName))
					{
						text = action.descriptiveName + " -";
					}
					else
					{
						text = action.negativeDescriptiveName;
					}
				}
			}
			text = Localization.Translate(text).text;
			ControllerMap controllerMap = this.GetControllerMap(fieldInfo.controllerType);
			if (controllerMap == null)
			{
				return;
			}
			ActionElementMap actionElementMap = (fieldInfo.actionElementMapId < 0) ? null : controllerMap.GetElementMap(fieldInfo.actionElementMapId);
			if (actionElementMap != null)
			{
				this.ShowBeginElementAssignmentReplacementWindow(fieldInfo, action, controllerMap, actionElementMap, text);
			}
			else
			{
				this.ShowCreateNewElementAssignmentWindow(fieldInfo, action, controllerMap, text);
			}
		}

		// Token: 0x06004A2B RID: 18987 RVA: 0x00268813 File Offset: 0x00266C13
		public void OnInputFieldInvertToggleStateChanged(ToggleInfo toggleInfo, bool newState)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.inputAllowed)
			{
				return;
			}
			AudioManager.Play("level_menu_select");
			this.SetActionAxisInverted(newState, toggleInfo.controllerType, toggleInfo.actionElementMapId);
		}

		// Token: 0x140000F0 RID: 240
		// (add) Token: 0x06004A2C RID: 18988 RVA: 0x0026884C File Offset: 0x00266C4C
		// (remove) Token: 0x06004A2D RID: 18989 RVA: 0x00268880 File Offset: 0x00266C80
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event ControlMapper.PlayerChangeAction OnPlayerChange;

		// Token: 0x06004A2E RID: 18990 RVA: 0x002688B4 File Offset: 0x00266CB4
		private void OnPlayerSelected(int playerId, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentPlayerId = playerId;
			this.ClearVarsOnPlayerChange();
			this.Redraw(true, true);
			for (int i = 0; i < this.axisToggleObjects.Count; i++)
			{
				this.axisToggleObjects[i].SetActive(this.currentPlayer.controllers.joystickCount > 0);
			}
			for (int j = 0; j < this.inactiveAxisToggleObjects.Count; j++)
			{
				this.inactiveAxisToggleObjects[j].SetActive(this.currentPlayer.controllers.joystickCount == 0);
			}
			if (ControlMapper.OnPlayerChange != null)
			{
				ControlMapper.OnPlayerChange();
			}
		}

		// Token: 0x06004A2F RID: 18991 RVA: 0x00268976 File Offset: 0x00266D76
		private void OnControllerSelected(int joystickId)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentJoystickId = joystickId;
			this.Redraw(true, true);
		}

		// Token: 0x06004A30 RID: 18992 RVA: 0x00268993 File Offset: 0x00266D93
		private void OnRemoveCurrentController()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentJoystickId < 0)
			{
				return;
			}
			this.RemoveController(this.currentPlayer, this.currentJoystickId);
			this.ClearVarsOnJoystickChange();
			this.Redraw(false, false);
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x002689CE File Offset: 0x00266DCE
		private void OnMapCategorySelected(int id, bool redraw)
		{
			if (!this.initialized)
			{
				return;
			}
			this.currentMapCategoryId = id;
			if (redraw)
			{
				this.Redraw(true, true);
			}
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x002689F1 File Offset: 0x00266DF1
		private void OnRestoreDefaults()
		{
			if (!this.initialized)
			{
				return;
			}
			this.ShowRestoreDefaultsWindow();
		}

		// Token: 0x06004A33 RID: 18995 RVA: 0x00268A05 File Offset: 0x00266E05
		private void OnScreenToggleActionPressed(InputActionEventData data)
		{
			if (!this.isOpen)
			{
				this.Open();
				return;
			}
			if (!this.initialized)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x00268A38 File Offset: 0x00266E38
		private void OnScreenOpenActionPressed(InputActionEventData data)
		{
			this.Open();
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x00268A40 File Offset: 0x00266E40
		private void OnScreenCloseActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (!this.isFocused)
			{
				return;
			}
			this.Close(true);
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x00268A70 File Offset: 0x00266E70
		private void OnUniversalCancelActionPressed(InputActionEventData data)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (this._universalCancelClosesScreen)
			{
				if (this.isFocused)
				{
					this.Close(true);
					return;
				}
			}
			else if (this.isFocused)
			{
				return;
			}
			if (this.isPollingForInput)
			{
				return;
			}
			this.CloseAllWindows();
		}

		// Token: 0x06004A37 RID: 18999 RVA: 0x00268AD6 File Offset: 0x00266ED6
		private void OnWindowCancel(int windowId)
		{
			if (!this.initialized)
			{
				return;
			}
			if (windowId < 0)
			{
				return;
			}
			this.CloseWindow(windowId);
		}

		// Token: 0x06004A38 RID: 19000 RVA: 0x00268AF3 File Offset: 0x00266EF3
		private void OnRemoveElementAssignment(int windowId, ControllerMap map, ActionElementMap aem)
		{
			if (map == null || aem == null)
			{
				return;
			}
			map.DeleteElementMap(aem.id);
			this.CloseWindow(windowId);
		}

		// Token: 0x06004A39 RID: 19001 RVA: 0x00268B18 File Offset: 0x00266F18
		private void OnBeginElementAssignment(InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, string actionName)
		{
			if (fieldInfo == null || map == null)
			{
				return;
			}
			this.pendingInputMapping = new ControlMapper.InputMapping(actionName, fieldInfo, map, aem, fieldInfo.controllerType, fieldInfo.controllerId);
			switch (fieldInfo.controllerType)
			{
			case ControllerType.Keyboard:
				this.ShowElementAssignmentPollingWindow();
				break;
			case ControllerType.Mouse:
				this.ShowElementAssignmentPollingWindow();
				break;
			case ControllerType.Joystick:
				this.ShowElementAssignmentPrePollingWindow();
				break;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004A3A RID: 19002 RVA: 0x00268B99 File Offset: 0x00266F99
		private void OnControllerAssignmentConfirmed(int windowId, Player player, int controllerId)
		{
			if (windowId < 0 || player == null || controllerId < 0)
			{
				return;
			}
			this.AssignController(player, controllerId);
			this.CloseWindow(windowId);
		}

		// Token: 0x06004A3B RID: 19003 RVA: 0x00268BC0 File Offset: 0x00266FC0
		private void OnMouseAssignmentConfirmed(int windowId, Player player)
		{
			if (windowId < 0 || player == null)
			{
				return;
			}
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != player)
				{
					players[i].controllers.hasMouse = false;
				}
			}
			player.controllers.hasMouse = true;
			this.CloseWindow(windowId);
		}

		// Token: 0x06004A3C RID: 19004 RVA: 0x00268C34 File Offset: 0x00267034
		private void OnElementAssignmentConflictReplaceConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: Error creating conflict check!");
				this.CloseWindow(windowId);
				return;
			}
			if (skipOtherPlayers)
			{
				ReInput.players.SystemPlayer.controllers.conflictChecking.RemoveElementAssignmentConflicts(conflictCheck);
				this.currentPlayer.controllers.conflictChecking.RemoveElementAssignmentConflicts(conflictCheck);
			}
			else
			{
				ReInput.controllers.conflictChecking.RemoveElementAssignmentConflicts(conflictCheck);
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			this.CloseWindow(windowId);
		}

		// Token: 0x06004A3D RID: 19005 RVA: 0x00268CD7 File Offset: 0x002670D7
		private void OnElementAssignmentAddConfirmed(int windowId, ControlMapper.InputMapping mapping, ElementAssignment assignment)
		{
			if (this.currentPlayer == null || mapping == null)
			{
				return;
			}
			mapping.map.ReplaceOrCreateElementMap(assignment);
			this.CloseWindow(windowId);
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x00268D00 File Offset: 0x00267100
		private void OnRestoreDefaultsConfirmed(int windowId)
		{
			if (this._restoreDefaultsDelegate == null)
			{
				IList<Player> players = ReInput.players.Players;
				for (int i = 0; i < players.Count; i++)
				{
					Player player = players[i];
					if (this._showControllers)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Joystick);
					}
					if (this._showKeyboard)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Keyboard);
					}
					if (this._showMouse)
					{
						player.controllers.maps.LoadDefaultMaps(ControllerType.Mouse);
					}
				}
			}
			this.CloseWindow(windowId);
			if (this._restoreDefaultsDelegate != null)
			{
				this._restoreDefaultsDelegate();
			}
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x00268DB4 File Offset: 0x002671B4
		private void OnAssignControllerWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo controllerPollingInfo = ReInput.controllers.polling.PollAllControllersOfTypeForFirstElementDown(ControllerType.Joystick);
			if (!controllerPollingInfo.success)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				return;
			}
			this.InputPollingStopped();
			if (ReInput.controllers.IsControllerAssigned(ControllerType.Joystick, controllerPollingInfo.controllerId) && !this.currentPlayer.controllers.ContainsController(ControllerType.Joystick, controllerPollingInfo.controllerId))
			{
				return;
			}
			this.OnControllerAssignmentConfirmed(windowId, this.currentPlayer, controllerPollingInfo.controllerId);
		}

		// Token: 0x06004A40 RID: 19008 RVA: 0x00268E98 File Offset: 0x00267298
		private void OnElementAssignmentPrePollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				ControllerPollingInfo controllerPollingInfo;
				switch (this.pendingInputMapping.controllerType)
				{
				case ControllerType.Keyboard:
				case ControllerType.Mouse:
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, 0);
					break;
				case ControllerType.Joystick:
					if (this.currentPlayer.controllers.joystickCount == 0)
					{
						return;
					}
					controllerPollingInfo = ReInput.controllers.polling.PollControllerForFirstButtonDown(this.pendingInputMapping.controllerType, this.currentJoystick.id);
					break;
				default:
					throw new NotImplementedException();
				}
				if (!controllerPollingInfo.success)
				{
					return;
				}
			}
			this.ShowElementAssignmentPollingWindow();
		}

		// Token: 0x06004A41 RID: 19009 RVA: 0x00268FB0 File Offset: 0x002673B0
		private void OnJoystickElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			ControllerPollingInfo pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Joystick, this.currentJoystick.id);
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
			if (pollingInfo.elementIdentifierName.Contains("Trigger") && elementAssignment.axisRange == AxisRange.Negative)
			{
				return;
			}
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
			}
			else
			{
				this.InputPollingStopped();
				this.ShowElementAssignmentConflictWindow(elementAssignment, false);
			}
		}

		// Token: 0x06004A42 RID: 19010 RVA: 0x00269104 File Offset: 0x00267504
		private void OnKeyboardElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			ControllerPollingInfo pollingInfo;
			bool flag;
			ModifierKeyFlags modifierKeyFlags;
			string text;
			this.PollKeyboardForAssignment(out pollingInfo, out flag, out modifierKeyFlags, out text);
			if (flag)
			{
				window.timer.Start(this._inputAssignmentTimeout);
			}
			window.SetContentText((!flag) ? Mathf.CeilToInt(window.timer.remaining).ToString() : string.Empty, 2);
			window.SetContentText(text, 1);
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo, modifierKeyFlags);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, false))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
			}
			else
			{
				this.InputPollingStopped();
				this.ShowElementAssignmentConflictWindow(elementAssignment, false);
			}
		}

		// Token: 0x06004A43 RID: 19011 RVA: 0x00269244 File Offset: 0x00267644
		private void OnMouseElementAssignmentPollingWindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingInputMapping == null)
			{
				return;
			}
			this.InputPollingStarted();
			if (window.timer.finished)
			{
				this.InputPollingStopped();
				this.CloseWindow(windowId);
				return;
			}
			window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
			ControllerPollingInfo pollingInfo;
			if (this._ignoreMouseXAxisAssignment || this._ignoreMouseYAxisAssignment)
			{
				pollingInfo = default(ControllerPollingInfo);
				foreach (ControllerPollingInfo controllerPollingInfo in ReInput.controllers.polling.PollControllerForAllElementsDown(ControllerType.Mouse, 0))
				{
					if (controllerPollingInfo.elementType == ControllerElementType.Axis)
					{
						if (this._ignoreMouseXAxisAssignment && controllerPollingInfo.elementIndex == 0)
						{
							continue;
						}
						if (this._ignoreMouseYAxisAssignment && controllerPollingInfo.elementIndex == 1)
						{
							continue;
						}
					}
					pollingInfo = controllerPollingInfo;
					break;
				}
			}
			else
			{
				pollingInfo = ReInput.controllers.polling.PollControllerForFirstElementDown(ControllerType.Mouse, 0);
			}
			if (!pollingInfo.success)
			{
				return;
			}
			if (!this.IsAllowedAssignment(this.pendingInputMapping, pollingInfo))
			{
				return;
			}
			ElementAssignment elementAssignment = this.pendingInputMapping.ToElementAssignment(pollingInfo);
			if (!this.HasElementAssignmentConflicts(this.currentPlayer, this.pendingInputMapping, elementAssignment, true))
			{
				this.pendingInputMapping.map.ReplaceOrCreateElementMap(elementAssignment);
				this.InputPollingStopped();
				this.CloseWindow(windowId);
			}
			else
			{
				this.InputPollingStopped();
				this.ShowElementAssignmentConflictWindow(elementAssignment, true);
			}
		}

		// Token: 0x06004A44 RID: 19012 RVA: 0x0026941C File Offset: 0x0026781C
		private void OnCalibrateAxisStep1WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			this.InputPollingStarted();
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.pendingAxisCalibration.RecordZero();
			this.CloseWindow(windowId);
			this.ShowCalibrateAxisStep2Window();
		}

		// Token: 0x06004A45 RID: 19013 RVA: 0x002694F4 File Offset: 0x002678F4
		private void OnCalibrateAxisStep2WindowUpdate(int windowId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.windowManager.GetWindow(windowId);
			if (windowId < 0)
			{
				return;
			}
			if (this.pendingAxisCalibration == null || !this.pendingAxisCalibration.isValid)
			{
				return;
			}
			if (!window.timer.finished)
			{
				window.SetContentText(Mathf.CeilToInt(window.timer.remaining).ToString(), 1);
				this.pendingAxisCalibration.RecordMinMax();
				if (this.currentPlayer.controllers.joystickCount == 0)
				{
					return;
				}
				if (!this.pendingAxisCalibration.joystick.PollForFirstButtonDown().success)
				{
					return;
				}
			}
			this.EndAxisCalibration();
			this.InputPollingStopped();
			this.CloseWindow(windowId);
		}

		// Token: 0x06004A46 RID: 19014 RVA: 0x002695CC File Offset: 0x002679CC
		private void ShowAssignControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.SetUpdateCallback(new Action<int>(this.OnAssignControllerWindowUpdate));
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.assignControllerWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.assignControllerWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.timer.Start(this._controllerAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A47 RID: 19015 RVA: 0x002696B4 File Offset: 0x00267AB4
		private void ShowControllerAssignmentConflictWindow(int controllerId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (ReInput.controllers.joystickCount == 0)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string otherPlayerName = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer)
				{
					if (players[i].controllers.ContainsController(ControllerType.Joystick, controllerId))
					{
						otherPlayerName = players[i].descriptiveName;
						break;
					}
				}
			}
			Joystick joystick = ReInput.controllers.GetJoystick(controllerId);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.controllerAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetControllerAssignmentConflictWindowMessage(joystick.name, otherPlayerName, this.currentPlayer.descriptiveName));
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate()
			{
				this.OnControllerAssignmentConfirmed(window.id, this.currentPlayer, controllerId);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A48 RID: 19016 RVA: 0x002698A8 File Offset: 0x00267CA8
		private void ShowBeginElementAssignmentReplacementWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, ActionElementMap aem, string actionName)
		{
			ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData);
			if (guiinputField == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), guiinputField.GetLabel());
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate()
			{
				this.OnBeginElementAssignment(fieldInfo, map, aem, actionName);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.remove, delegate()
			{
				this.OnRemoveElementAssignment(window.id, map, aem);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A49 RID: 19017 RVA: 0x00269A70 File Offset: 0x00267E70
		private void ShowCreateNewElementAssignmentWindow(InputFieldInfo fieldInfo, InputAction action, ControllerMap map, string actionName)
		{
			if (this.inputGrid.GetGUIInputField(this.currentMapCategoryId, action.id, fieldInfo.axisRange, fieldInfo.controllerType, fieldInfo.intData) == null)
			{
				return;
			}
			this.OnBeginElementAssignment(fieldInfo, map, null, actionName);
		}

		// Token: 0x06004A4A RID: 19018 RVA: 0x00269ABC File Offset: 0x00267EBC
		private void ShowElementAssignmentPrePollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.elementAssignmentPrePollingWindowMessage);
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 10f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnElementAssignmentPrePollingWindowUpdate));
			window.timer.Start(this._preInputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A4B RID: 19019 RVA: 0x00269BD4 File Offset: 0x00267FD4
		private void ShowElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			switch (this.pendingInputMapping.controllerType)
			{
			case ControllerType.Keyboard:
				this.ShowKeyboardElementAssignmentPollingWindow();
				break;
			case ControllerType.Mouse:
				if (this.currentPlayer.controllers.hasMouse)
				{
					this.ShowMouseElementAssignmentPollingWindow();
				}
				else
				{
					this.ShowMouseAssignmentConflictWindow();
				}
				break;
			case ControllerType.Joystick:
				this.ShowJoystickElementAssignmentPollingWindow();
				break;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004A4C RID: 19020 RVA: 0x00269C58 File Offset: 0x00268058
		private void ShowJoystickElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = (this.pendingInputMapping.axisRange != AxisRange.Full || !this._showFullAxisInputFields || this._showSplitAxisInputFields) ? this._language.GetJoystickElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetJoystickElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnJoystickElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A4D RID: 19021 RVA: 0x00269D80 File Offset: 0x00268180
		private void ShowKeyboardElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetKeyboardElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName));
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -(window.GetContentTextHeight(0) + 50f)), string.Empty);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnKeyboardElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A4E RID: 19022 RVA: 0x00269E9C File Offset: 0x0026829C
		private void ShowMouseElementAssignmentPollingWindow()
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string text = (this.pendingInputMapping.axisRange != AxisRange.Full || !this._showFullAxisInputFields || this._showSplitAxisInputFields) ? this._language.GetMouseElementAssignmentPollingWindowMessage(this.pendingInputMapping.actionName) : this._language.GetMouseElementAssignmentPollingWindowMessage_FullAxisFieldOnly(this.pendingInputMapping.actionName);
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this.pendingInputMapping.actionName);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnMouseElementAssignmentPollingWindowUpdate));
			window.timer.Start(this._inputAssignmentTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A4F RID: 19023 RVA: 0x00269FC4 File Offset: 0x002683C4
		private void ShowElementAssignmentConflictWindow(ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (this.pendingInputMapping == null)
			{
				return;
			}
			bool flag = this.IsBlockingAssignmentConflict(this.pendingInputMapping, assignment, skipOtherPlayers);
			string text = (!flag) ? this._language.GetElementAlreadyInUseCanReplace(this.pendingInputMapping.elementName, this._allowElementAssignmentConflicts) : this._language.GetElementAlreadyInUseBlocked(this.pendingInputMapping.elementName);
			int elementAlreadyInUseCanReplaceFontSize = this._language.GetElementAlreadyInUseCanReplaceFontSize(this._allowElementAssignmentConflicts);
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.elementAssignmentConflictWindowMessage);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), text, elementAlreadyInUseCanReplaceFontSize);
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			if (flag)
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.okay, unityAction, unityAction, true);
			}
			else
			{
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.replace, delegate()
				{
					this.OnElementAssignmentConflictReplaceConfirmed(window.id, this.pendingInputMapping, assignment, skipOtherPlayers);
				}, unityAction, true);
				if (this._allowElementAssignmentConflicts)
				{
					window.CreateButton(this.prefabs.fitButton, UIPivot.BottomCenter, UIAnchor.BottomCenter, Vector2.zero, this._language.add, delegate()
					{
						this.OnElementAssignmentAddConfirmed(window.id, this.pendingInputMapping, assignment);
					}, unityAction, false);
				}
				window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.cancel, unityAction, unityAction, false);
			}
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A50 RID: 19024 RVA: 0x0026A204 File Offset: 0x00268604
		private void ShowMouseAssignmentConflictWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(true);
			if (window == null)
			{
				return;
			}
			string otherPlayerName = string.Empty;
			IList<Player> players = ReInput.players.Players;
			for (int i = 0; i < players.Count; i++)
			{
				if (players[i] != this.currentPlayer)
				{
					if (players[i].controllers.hasMouse)
					{
						otherPlayerName = players[i].descriptiveName;
						break;
					}
				}
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.mouseAssignmentConflictWindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetMouseAssignmentConflictWindowMessage(otherPlayerName, this.currentPlayer.descriptiveName));
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, this._language.yes, delegate()
			{
				this.OnMouseAssignmentConfirmed(window.id, this.currentPlayer);
			}, unityAction, true);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, this._language.no, unityAction, unityAction, false);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A51 RID: 19025 RVA: 0x0026A3C4 File Offset: 0x002687C4
		private void ShowCalibrateControllerWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			CalibrationWindow calibrationWindow = this.OpenWindow(this.prefabs.calibrationWindow, "CalibrationWindow", true) as CalibrationWindow;
			if (calibrationWindow == null)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			calibrationWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateControllerWindowTitle);
			calibrationWindow.SetJoystick(this.currentPlayer.id, currentJoystick);
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Calibrate, new Action<int>(this.StartAxisCalibration));
			calibrationWindow.SetButtonCallback(CalibrationWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(calibrationWindow);
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x0026A49C File Offset: 0x0026889C
		private void ShowCalibrateAxisStep1Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep1WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep1WindowMessage(joystick.AxisElementIdentifiers[axisIndex].name));
			if (this.prefabs.centerStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.centerStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 10f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep1WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x0026A60C File Offset: 0x00268A0C
		private void ShowCalibrateAxisStep2Window()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			Window window = this.OpenWindow(false);
			if (window == null)
			{
				return;
			}
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			Joystick joystick = this.pendingAxisCalibration.joystick;
			if (joystick.axisCount == 0)
			{
				return;
			}
			int axisIndex = this.pendingAxisCalibration.axisIndex;
			if (axisIndex < 0 || axisIndex >= joystick.axisCount)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.calibrateAxisStep2WindowTitle);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), this._language.GetCalibrateAxisStep2WindowMessage(joystick.AxisElementIdentifiers[axisIndex].name));
			if (this.prefabs.moveStickGraphic != null)
			{
				window.AddContentImage(this.prefabs.moveStickGraphic, UIPivot.BottomCenter, UIAnchor.BottomCenter, new Vector2(0f, 40f));
			}
			window.AddContentText(this.prefabs.windowContentText, UIPivot.BottomCenter, UIAnchor.BottomHStretch, Vector2.zero, string.Empty);
			window.SetUpdateCallback(new Action<int>(this.OnCalibrateAxisStep2WindowUpdate));
			window.timer.Start(this._axisCalibrationTimeout);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x0026A77C File Offset: 0x00268B7C
		private void ShowEditInputBehaviorsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this._inputBehaviorSettings == null)
			{
				return;
			}
			InputBehaviorWindow inputBehaviorWindow = this.OpenWindow(this.prefabs.inputBehaviorsWindow, "EditInputBehaviorsWindow", true) as InputBehaviorWindow;
			if (inputBehaviorWindow == null)
			{
				return;
			}
			inputBehaviorWindow.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, this._language.inputBehaviorSettingsWindowTitle);
			inputBehaviorWindow.SetData(this.currentPlayer.id, this._inputBehaviorSettings);
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Done, new Action<int>(this.CloseWindow));
			inputBehaviorWindow.SetButtonCallback(InputBehaviorWindow.ButtonIdentifier.Cancel, new Action<int>(this.CloseWindow));
			this.windowManager.Focus(inputBehaviorWindow);
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x0026A838 File Offset: 0x00268C38
		private void ShowRestoreDefaultsWindow()
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			this.OpenModal(this._language.restoreDefaultsWindowTitle, this._language.restoreDefaultsWindowMessage, this._language.yes, new Action<int>(this.OnRestoreDefaultsConfirmed), this._language.no, new Action<int>(this.OnWindowCancel), true);
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x0026A89C File Offset: 0x00268C9C
		private void CreateInputGrid()
		{
			this.InitializeInputGrid();
			this.CreateHeaderLabels();
			this.CreateActionLabelColumn();
			this.CreateKeyboardInputFieldColumn();
			this.CreateMouseInputFieldColumn();
			this.CreateControllerInputFieldColumn();
			this.CreateInputActionLabels();
			this.CreateInputFields();
			this.inputGrid.HideAll();
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0026A8DC File Offset: 0x00268CDC
		private void InitializeInputGrid()
		{
			if (this.inputGrid == null)
			{
				this.inputGrid = new ControlMapper.InputGrid();
			}
			else
			{
				this.inputGrid.ClearAll();
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null)
					{
						if (mapCategory.userAssignable)
						{
							this.inputGrid.AddMapCategory(mappingSet.mapCategoryId);
							if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
							{
								IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
								for (int j = 0; j < actionCategoryIds.Count; j++)
								{
									int num = actionCategoryIds[j];
									InputCategory actionCategory = ReInput.mapping.GetActionCategory(num);
									if (actionCategory != null)
									{
										if (actionCategory.userAssignable)
										{
											this.inputGrid.AddActionCategory(mappingSet.mapCategoryId, num);
											foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(num))
											{
												if (inputAction.type == InputActionType.Axis)
												{
													if (this._showFullAxisInputFields)
													{
														this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Full);
													}
													if (this._showSplitAxisInputFields)
													{
														this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive);
														this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Negative);
													}
												}
												else if (inputAction.type == InputActionType.Button)
												{
													this.inputGrid.AddAction(mappingSet.mapCategoryId, inputAction, AxisRange.Positive);
												}
											}
										}
									}
								}
							}
							else
							{
								IList<int> actionIds = mappingSet.actionIds;
								for (int k = 0; k < actionIds.Count; k++)
								{
									InputAction action = ReInput.mapping.GetAction(actionIds[k]);
									if (action != null)
									{
										if (action.type == InputActionType.Axis)
										{
											if (this._showFullAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Full);
											}
											if (this._showSplitAxisInputFields)
											{
												this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
												this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Negative);
											}
										}
										else if (action.type == InputActionType.Button)
										{
											this.inputGrid.AddAction(mappingSet.mapCategoryId, action, AxisRange.Positive);
										}
									}
								}
							}
						}
					}
				}
			}
			this.references.inputGridLayoutElement.flexibleWidth = 0f;
			this.references.inputGridLayoutElement.preferredWidth = (float)this.inputGridWidth;
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x0026ABC8 File Offset: 0x00268FC8
		private void RefreshInputGridStructure()
		{
			if (this.currentMappingSet == null)
			{
				return;
			}
			this.inputGrid.HideAll();
			this.inputGrid.Show(this.currentMappingSet.mapCategoryId);
			this.references.inputGridInnerGroup.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.inputGrid.GetColumnHeight(this.currentMappingSet.mapCategoryId));
		}

		// Token: 0x06004A59 RID: 19033 RVA: 0x0026AC30 File Offset: 0x00269030
		private void CreateHeaderLabels()
		{
			this.references.inputGridHeader1 = this.CreateNewColumnGroup("ActionsHeader", this.references.actionsColumnHeadersGroup, this._actionLabelWidth).transform;
			ControlMapper.GUILabel guilabel = this.CreateLabel(this.prefabs.actionsHeaderLabel, this._language.actionColumnLabel.ToUpper(), this.references.inputGridHeader1, new Vector2(14f, -10f));
			guilabel.rectTransform.offsetMax = Vector2.zero;
			if (this._showKeyboard)
			{
				this.references.inputGridHeader2 = this.CreateNewColumnGroup("KeyboardHeader", this.references.inputGridHeadersGroup, 226).transform;
				ControlMapper.GUILabel guilabel2 = this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.keyboardColumnLabel, this.references.inputGridHeader2, new Vector2(14f, -10f));
				guilabel2.rectTransform.offsetMax = Vector2.zero;
			}
			if (this._showMouse)
			{
				this.references.inputGridHeader3 = this.CreateNewColumnGroup("MouseHeader", this.references.inputGridHeadersGroup, this._mouseColMaxWidth).transform;
				ControlMapper.GUILabel guilabel2 = this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.mouseColumnLabel, this.references.inputGridHeader3, Vector2.zero);
				guilabel2.SetTextAlignment(TextAnchor.MiddleCenter);
			}
			if (this._showControllers)
			{
				this.references.inputGridHeader4 = this.CreateNewColumnGroup("ControllerHeader", this.references.inputGridHeadersGroup, 230).transform;
				ControlMapper.GUILabel guilabel2 = this.CreateLabel(this.prefabs.inputGridHeaderLabel, this._language.controllerColumnLabel, this.references.inputGridHeader4, new Vector2(14f, -10f));
				guilabel2.rectTransform.offsetMax = Vector2.zero;
			}
		}

		// Token: 0x06004A5A RID: 19034 RVA: 0x0026AE1C File Offset: 0x0026921C
		private void CreateActionLabelColumn()
		{
			Transform transform = this.CreateNewColumnGroup("ActionLabelColumn", this.references.actionsColumn, this._actionLabelWidth).transform;
			this.references.inputGridActionColumn = transform;
			RectTransform component = transform.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 1f);
			component.pivot = new Vector2(0.5f, 0.5f);
			component.offsetMin = new Vector2(14f, 0f);
			component.offsetMax = new Vector2(0f, -70f);
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x0026AEC7 File Offset: 0x002692C7
		private void CreateKeyboardInputFieldColumn()
		{
			if (!this._showKeyboard)
			{
				return;
			}
			this.CreateInputFieldColumn("KeyboardColumn", ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
		}

		// Token: 0x06004A5C RID: 19036 RVA: 0x0026AEEE File Offset: 0x002692EE
		private void CreateMouseInputFieldColumn()
		{
			if (!this._showMouse)
			{
				return;
			}
			this.CreateInputFieldColumn("MouseColumn", ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
		}

		// Token: 0x06004A5D RID: 19037 RVA: 0x0026AF15 File Offset: 0x00269315
		private void CreateControllerInputFieldColumn()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.CreateInputFieldColumn("ControllerColumn", ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x0026AF3C File Offset: 0x0026933C
		private void CreateInputFieldColumn(string name, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			Transform transform = this.CreateNewColumnGroup(name, this.references.inputGridInnerGroup, maxWidth).transform;
			switch (controllerType)
			{
			case ControllerType.Keyboard:
				this.references.inputGridKeyboardColumn = transform;
				break;
			case ControllerType.Mouse:
				this.references.inputGridMouseColumn = transform;
				break;
			case ControllerType.Joystick:
				this.references.inputGridControllerColumn = transform;
				break;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x0026AFB4 File Offset: 0x002693B4
		private void CreateInputActionLabels()
		{
			Transform inputGridActionColumn = this.references.inputGridActionColumn;
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					float num = 6f;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						int num2 = 0;
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null)
							{
								if (actionCategory.userAssignable)
								{
									if (this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
									{
										if (this._showActionCategoryLabels)
										{
											if (num2 > 0)
											{
												num -= (float)this._inputRowCategorySpacing;
											}
											ControlMapper.GUILabel guilabel = this.CreateLabel(Localization.Translate(actionCategory.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
											guilabel.SetFontStyle(FontStyle.Bold);
											guilabel.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
											this.inputGrid.AddActionCategoryLabel(mappingSet.mapCategoryId, actionCategory.id, guilabel);
											num -= this._inputRowHeight;
										}
										foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
										{
											if (inputAction.type == InputActionType.Axis)
											{
												if (this._showFullAxisInputFields)
												{
													ControlMapper.GUILabel guilabel2 = this.CreateLabel(Localization.Translate(inputAction.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Full, guilabel2);
													num -= this._inputRowHeight;
												}
												if (this._showSplitAxisInputFields)
												{
													string labelText = string.IsNullOrEmpty(inputAction.positiveDescriptiveName) ? (Localization.Translate(inputAction.descriptiveName).text + " +") : Localization.Translate(inputAction.positiveDescriptiveName).text;
													ControlMapper.GUILabel guilabel2 = this.CreateLabel(labelText, inputGridActionColumn, new Vector2(0f, num));
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2);
													num -= this._inputRowHeight;
													string labelText2 = string.IsNullOrEmpty(inputAction.negativeDescriptiveName) ? (Localization.Translate(inputAction.descriptiveName).text + " -") : Localization.Translate(inputAction.negativeDescriptiveName).text;
													guilabel2 = this.CreateLabel(labelText2, inputGridActionColumn, new Vector2(0f, num));
													guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Negative, guilabel2);
													num -= this._inputRowHeight;
												}
											}
											else if (inputAction.type == InputActionType.Button)
											{
												ControlMapper.GUILabel guilabel2 = this.CreateLabel(Localization.Translate(inputAction.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
												guilabel2.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
												this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, inputAction.id, AxisRange.Positive, guilabel2);
												num -= this._inputRowHeight;
											}
										}
										num2++;
									}
								}
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null)
							{
								if (action.userAssignable)
								{
									InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
									if (actionCategory2 != null)
									{
										if (actionCategory2.userAssignable)
										{
											if (action.type == InputActionType.Axis)
											{
												if (this._showFullAxisInputFields)
												{
													ControlMapper.GUILabel guilabel3 = this.CreateLabel(Localization.Translate(action.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Full, guilabel3);
													num -= this._inputRowHeight;
												}
												if (this._showSplitAxisInputFields)
												{
													ControlMapper.GUILabel guilabel3 = this.CreateLabel(Localization.Translate(action.positiveDescriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3);
													num -= this._inputRowHeight;
													guilabel3 = this.CreateLabel(Localization.Translate(action.negativeDescriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
													guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
													this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Negative, guilabel3);
													num -= this._inputRowHeight;
												}
											}
											else if (action.type == InputActionType.Button)
											{
												ControlMapper.GUILabel guilabel3 = this.CreateLabel(Localization.Translate(action.descriptiveName).text, inputGridActionColumn, new Vector2(0f, num));
												guilabel3.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
												this.inputGrid.AddActionLabel(mappingSet.mapCategoryId, action.id, AxisRange.Positive, guilabel3);
												num -= this._inputRowHeight;
											}
										}
									}
								}
							}
						}
						for (int l = 0; l < 2; l++)
						{
							ControlMapper.GUILabel guilabel4 = this.CreateDeactivatedLabel(Localization.Translate((l != 0) ? "RemapFlipYAxis" : "RemapFlipXAxis").text, inputGridActionColumn, new Vector2(0f, num));
							guilabel4.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
							this.inactiveAxisToggleObjects.Add(guilabel4.gameObject);
							guilabel4 = this.CreateLabel(Localization.Translate((l != 0) ? "RemapFlipYAxis" : "RemapFlipXAxis").text, inputGridActionColumn, new Vector2(0f, num));
							guilabel4.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
							this.axisToggleObjects.Add(guilabel4.gameObject);
							num -= this._inputRowHeight;
						}
					}
					this.inputGrid.SetColumnHeight(mappingSet.mapCategoryId, -num);
				}
			}
		}

		// Token: 0x06004A60 RID: 19040 RVA: 0x0026B6D4 File Offset: 0x00269AD4
		private void CreateInputFields()
		{
			if (this._showControllers)
			{
				this.CreateInputFields(this.references.inputGridControllerColumn, ControllerType.Joystick, this._controllerColMaxWidth, this._controllerInputFieldCount, false);
			}
			if (this._showKeyboard)
			{
				this.CreateInputFields(this.references.inputGridKeyboardColumn, ControllerType.Keyboard, this._keyboardColMaxWidth, this._keyboardInputFieldCount, true);
			}
			if (this._showMouse)
			{
				this.CreateInputFields(this.references.inputGridMouseColumn, ControllerType.Mouse, this._mouseColMaxWidth, this._mouseInputFieldCount, false);
			}
		}

		// Token: 0x06004A61 RID: 19041 RVA: 0x0026B760 File Offset: 0x00269B60
		private void CreateInputFields(Transform columnXform, ControllerType controllerType, int maxWidth, int cols, bool disableFullAxis)
		{
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null && mappingSet.isValid)
				{
					int fieldWidth = maxWidth / cols;
					float num = 6f;
					int num2 = 0;
					if (mappingSet.actionListMode == ControlMapper.MappingSet.ActionListMode.ActionCategory)
					{
						IList<int> actionCategoryIds = mappingSet.actionCategoryIds;
						for (int j = 0; j < actionCategoryIds.Count; j++)
						{
							InputCategory actionCategory = ReInput.mapping.GetActionCategory(actionCategoryIds[j]);
							if (actionCategory != null)
							{
								if (actionCategory.userAssignable)
								{
									if (this.CountIEnumerable<InputAction>(ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id)) != 0)
									{
										if (this._showActionCategoryLabels)
										{
											num -= ((num2 <= 0) ? this._inputRowHeight : (this._inputRowHeight + (float)this._inputRowCategorySpacing));
										}
										foreach (InputAction inputAction in ReInput.mapping.UserAssignableActionsInCategory(actionCategory.id, true))
										{
											if (inputAction.type == InputActionType.Axis)
											{
												if (this._showFullAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Full, controllerType, cols, fieldWidth, ref num, disableFullAxis);
												}
												if (this._showSplitAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Negative, controllerType, cols, fieldWidth, ref num, false);
												}
											}
											else if (inputAction.type == InputActionType.Button)
											{
												this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, inputAction, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
											}
											num2++;
										}
									}
								}
							}
						}
					}
					else
					{
						IList<int> actionIds = mappingSet.actionIds;
						for (int k = 0; k < actionIds.Count; k++)
						{
							InputAction action = ReInput.mapping.GetAction(actionIds[k]);
							if (action != null)
							{
								if (action.userAssignable)
								{
									InputCategory actionCategory2 = ReInput.mapping.GetActionCategory(action.categoryId);
									if (actionCategory2 != null)
									{
										if (actionCategory2.userAssignable)
										{
											if (action.type == InputActionType.Axis)
											{
												if (this._showFullAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Full, controllerType, cols, fieldWidth, ref num, disableFullAxis);
												}
												if (this._showSplitAxisInputFields)
												{
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
													this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Negative, controllerType, cols, fieldWidth, ref num, false);
												}
											}
											else if (action.type == InputActionType.Button)
											{
												this.CreateInputFieldSet(columnXform, mappingSet.mapCategoryId, action, AxisRange.Positive, controllerType, cols, fieldWidth, ref num, false);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004A62 RID: 19042 RVA: 0x0026BA74 File Offset: 0x00269E74
		private void CreateInputFieldSet(Transform parent, int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int cols, int fieldWidth, ref float yPos, bool disableFullAxis)
		{
			GameObject gameObject = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0f, yPos));
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(0f, 1f);
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = Vector2.zero;
			component.offsetMin = new Vector2(5f, component.offsetMin.y - 1f);
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 227f);
			this.inputGrid.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, gameObject);
			for (int i = 0; i < cols; i++)
			{
				int num = (axisRange != AxisRange.Full) ? 0 : this._invertToggleWidth;
				ControlMapper.GUIInputField guiinputField = this.CreateInputField(horizontalLayoutGroup.transform, Vector2.zero, string.Empty, action.id, axisRange, controllerType, i);
				guiinputField.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.PreferredSize, fieldWidth - num);
				this.inputGrid.AddInputField(mapCategoryId, action, axisRange, controllerType, i, guiinputField);
				if (axisRange == AxisRange.Full && controllerType == ControllerType.Joystick)
				{
					GameObject gameObject2 = this.CreateNewGUIObject("FieldLayoutGroup", parent, new Vector2(0f, yPos - this._inputRowHeight * (float)((!(action.name == "MoveHorizontal")) ? 9 : 13)));
					HorizontalLayoutGroup horizontalLayoutGroup2 = gameObject2.AddComponent<HorizontalLayoutGroup>();
					RectTransform component2 = gameObject2.GetComponent<RectTransform>();
					component2.anchorMin = new Vector2(0f, 1f);
					component2.anchorMax = new Vector2(0f, 1f);
					component2.pivot = new Vector2(0f, 1f);
					component2.sizeDelta = Vector2.zero;
					component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._inputRowHeight);
					component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 227f);
					component2.offsetMin = new Vector2(5f, component2.offsetMin.y);
					ControlMapper.GUIToggle guitoggle = this.CreateToggle(this.prefabs.inputGridFieldInvertToggle, horizontalLayoutGroup2.transform, Vector2.zero, string.Empty, action.id, axisRange, controllerType, i);
					guitoggle.SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType.MinSize, num);
					guiinputField.AddToggle(guitoggle);
					this.axisToggleObjects.Add(guitoggle.gameObject);
				}
			}
			yPos -= this._inputRowHeight;
		}

		// Token: 0x06004A63 RID: 19043 RVA: 0x0026BD04 File Offset: 0x0026A104
		private void PopulateInputFields()
		{
			this.inputGrid.InitializeFields(this.currentMapCategoryId);
			if (this.currentPlayer == null)
			{
				return;
			}
			this.inputGrid.SetFieldsActive(this.currentMapCategoryId, true);
			foreach (ControlMapper.InputActionSet actionSet in this.inputGrid.GetActionSets(this.currentMapCategoryId))
			{
				if (this._showKeyboard)
				{
					if (this.currentPlayerId == 0)
					{
						ControllerType controllerType = ControllerType.Keyboard;
						int controllerId = 0;
						int layoutId = this._keyboardMapDefaultLayout;
						int maxFields = this._keyboardInputFieldCount;
						ControllerMap controllerMapOrCreateNew = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
						this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew, controllerType, controllerId, maxFields);
					}
					else
					{
						this.DisableInputFieldGroup(actionSet, ControllerType.Keyboard, this._keyboardInputFieldCount);
					}
				}
				if (this._showMouse)
				{
					ControllerType controllerType = ControllerType.Mouse;
					int controllerId = 0;
					int layoutId = this._mouseMapDefaultLayout;
					int maxFields = this._mouseInputFieldCount;
					ControllerMap controllerMapOrCreateNew2 = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
					if (this.currentPlayer.controllers.hasMouse)
					{
						this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew2, controllerType, controllerId, maxFields);
					}
				}
				if (this.isJoystickSelected && this.currentPlayer.controllers.joystickCount > 0)
				{
					ControllerType controllerType = ControllerType.Joystick;
					int controllerId = this.currentJoystick.id;
					int layoutId = this._joystickMapDefaultLayout;
					int maxFields = this._controllerInputFieldCount;
					ControllerMap controllerMapOrCreateNew3 = this.GetControllerMapOrCreateNew(controllerType, controllerId, layoutId);
					this.PopulateInputFieldGroup(actionSet, controllerMapOrCreateNew3, controllerType, controllerId, maxFields);
				}
				else
				{
					this.DisableInputFieldGroup(actionSet, ControllerType.Joystick, this._controllerInputFieldCount);
				}
			}
		}

		// Token: 0x06004A64 RID: 19044 RVA: 0x0026BEAC File Offset: 0x0026A2AC
		private void PopulateInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerMap controllerMap, ControllerType controllerType, int controllerId, int maxFields)
		{
			if (controllerMap == null)
			{
				return;
			}
			int num = 0;
			this.inputGrid.SetFixedFieldData(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId);
			foreach (ActionElementMap actionElementMap in controllerMap.ElementMapsWithAction(actionSet.actionId))
			{
				if (actionElementMap.elementType == ControllerElementType.Button)
				{
					if (actionSet.axisRange == AxisRange.Full)
					{
						continue;
					}
					if (actionSet.axisRange == AxisRange.Positive)
					{
						if (actionElementMap.axisContribution == Pole.Negative)
						{
							continue;
						}
					}
					else if (actionSet.axisRange == AxisRange.Negative && actionElementMap.axisContribution == Pole.Positive)
					{
						continue;
					}
					this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, false);
				}
				else if (actionElementMap.elementType == ControllerElementType.Axis)
				{
					if (actionSet.axisRange == AxisRange.Full)
					{
						if (actionElementMap.axisRange != AxisRange.Full)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, actionElementMap.invert);
					}
					else if (actionSet.axisRange == AxisRange.Positive)
					{
						if (actionElementMap.axisRange == AxisRange.Full)
						{
							continue;
						}
						if (actionElementMap.axisContribution == Pole.Negative)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, false);
					}
					else if (actionSet.axisRange == AxisRange.Negative)
					{
						if (actionElementMap.axisRange == AxisRange.Full)
						{
							continue;
						}
						if (actionElementMap.axisContribution == Pole.Positive)
						{
							continue;
						}
						this.inputGrid.PopulateField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, controllerId, num, actionElementMap.id, actionElementMap.elementIdentifierName, false);
					}
				}
				num++;
				if (num > maxFields)
				{
					break;
				}
			}
		}

		// Token: 0x06004A65 RID: 19045 RVA: 0x0026C0F0 File Offset: 0x0026A4F0
		private void DisableInputFieldGroup(ControlMapper.InputActionSet actionSet, ControllerType controllerType, int fieldCount)
		{
			for (int i = 0; i < fieldCount; i++)
			{
				ControlMapper.GUIInputField guiinputField = this.inputGrid.GetGUIInputField(this.currentMapCategoryId, actionSet.actionId, actionSet.axisRange, controllerType, i);
				if (guiinputField != null)
				{
					guiinputField.SetInteractible(false, false);
				}
			}
		}

		// Token: 0x06004A66 RID: 19046 RVA: 0x0026C144 File Offset: 0x0026A544
		private void CreateLayout()
		{
			this.references.playersGroup.gameObject.SetActive(this.showPlayers);
			this.references.controllerGroup.gameObject.SetActive(this._showControllers);
			this.references.removeControllerButton.gameObject.SetActive(this.showControllerGroupButtons);
			this.references.assignControllerButton.gameObject.SetActive(this.showControllerGroupButtons);
			this.references.assignedControllersGroup.gameObject.SetActive(this._showControllers && this.ShowAssignedControllers());
			this.references.settingsAndMapCategoriesGroup.gameObject.SetActive(this.showSettings || this.showMapCategories);
			this.references.settingsGroup.gameObject.SetActive(this.showSettings);
			this.references.mapCategoriesGroup.gameObject.SetActive(this.showMapCategories);
		}

		// Token: 0x06004A67 RID: 19047 RVA: 0x0026C245 File Offset: 0x0026A645
		private void Draw()
		{
			this.DrawPlayersGroup();
			this.DrawControllersGroup();
			this.DrawSettingsGroup();
			this.DrawMapCategoriesGroup();
			this.DrawWindowButtonsGroup();
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x0026C268 File Offset: 0x0026A668
		private void DrawPlayersGroup()
		{
			this.references.playersGroup.labelText = this._language.playersGroupLabel;
			this.references.playersGroup.SetLabelActive(this._showPlayersGroupLabel);
			for (int i = 0; i < this.playerCount; i++)
			{
				Player player = ReInput.players.GetPlayer(i);
				if (player != null)
				{
					GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.playerButton, this.references.playersGroup.content, "Player" + i + "Button");
					ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(gameObject);
					guibutton.SetLabel(Localization.Translate(player.descriptiveName).text);
					guibutton.SetButtonInfoData("PlayerSelection", player.id);
					guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					guibutton.gameObject.GetComponent<CustomButtonPlayerSelect>().mapper = this;
					this.playerButtons.Add(guibutton);
				}
			}
			this.playerButtons[0].SetInteractible(false, true);
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x0026C395 File Offset: 0x0026A795
		public Selectable GetUnselectedPlayerButton()
		{
			return this.playerButtons[1 - this.currentPlayerId].gameObject.GetComponent<Selectable>();
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x0026C3B4 File Offset: 0x0026A7B4
		private void DrawControllersGroup()
		{
			if (!this._showControllers)
			{
				return;
			}
			this.references.controllerSettingsGroup.labelText = this._language.controllerSettingsGroupLabel;
			this.references.controllerSettingsGroup.SetLabelActive(this._showControllerGroupLabel);
			this.references.controllerNameLabel.gameObject.SetActive(this._showControllerNameLabel);
			this.references.controllerGroupLabelGroup.gameObject.SetActive(this._showControllerGroupLabel || this._showControllerNameLabel);
			if (this.ShowAssignedControllers())
			{
				this.references.assignedControllersGroup.labelText = this._language.assignedControllersGroupLabel;
				this.references.assignedControllersGroup.SetLabelActive(this._showAssignedControllersGroupLabel);
			}
			ButtonInfo component = this.references.removeControllerButton.GetComponent<ButtonInfo>();
			component.text.text = this._language.removeControllerButtonLabel;
			component = this.references.calibrateControllerButton.GetComponent<ButtonInfo>();
			component.text.text = this._language.calibrateControllerButtonLabel;
			component = this.references.assignControllerButton.GetComponent<ButtonInfo>();
			component.text.text = this._language.assignControllerButtonLabel;
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.none, this.references.assignedControllersGroup.content, Vector2.zero);
			guibutton.SetInteractible(false, false, true);
			this.assignedControllerButtonsPlaceholder = guibutton;
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x0026C52C File Offset: 0x0026A92C
		private void DrawSettingsGroup()
		{
			if (!this.showSettings)
			{
				return;
			}
			this.references.settingsGroup.labelText = this._language.settingsGroupLabel;
			this.references.settingsGroup.SetLabelActive(this._showSettingsGroupLabel);
			ControlMapper.GUIButton guibutton = this.CreateButton(this._language.inputBehaviorSettingsButtonLabel, this.references.settingsGroup.content, Vector2.zero);
			this.miscInstantiatedObjects.Add(guibutton.gameObject);
			guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
			guibutton.SetButtonInfoData("EditInputBehaviors", 0);
			guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
		}

		// Token: 0x06004A6C RID: 19052 RVA: 0x0026C5E4 File Offset: 0x0026A9E4
		private void DrawMapCategoriesGroup()
		{
			if (!this.showMapCategories)
			{
				return;
			}
			if (this._mappingSets == null)
			{
				return;
			}
			this.references.mapCategoriesGroup.labelText = this._language.mapCategoriesGroupLabel;
			this.references.mapCategoriesGroup.SetLabelActive(this._showMapCategoriesGroupLabel);
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				ControlMapper.MappingSet mappingSet = this._mappingSets[i];
				if (mappingSet != null)
				{
					InputMapCategory mapCategory = ReInput.mapping.GetMapCategory(mappingSet.mapCategoryId);
					if (mapCategory != null)
					{
						GameObject gameObject = UITools.InstantiateGUIObject<ButtonInfo>(this.prefabs.button, this.references.mapCategoriesGroup.content, mapCategory.name + "Button");
						ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(gameObject);
						guibutton.SetLabel(Localization.Translate(mapCategory.descriptiveName).text);
						guibutton.SetButtonInfoData("MapCategorySelection", mapCategory.id);
						guibutton.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
						guibutton.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
						this.mapCategoryButtons.Add(guibutton);
					}
				}
			}
		}

		// Token: 0x06004A6D RID: 19053 RVA: 0x0026C724 File Offset: 0x0026AB24
		private void DrawWindowButtonsGroup()
		{
			this.references.doneButton.GetComponent<ButtonInfo>().text.text = this._language.doneButtonLabel;
			this.references.restoreDefaultsButton.GetComponent<ButtonInfo>().text.text = this._language.restoreDefaultsButtonLabel;
			this.UpdateRumbleText();
		}

		// Token: 0x06004A6E RID: 19054 RVA: 0x0026C784 File Offset: 0x0026AB84
		private void Redraw(bool listsChanged, bool playTransitions)
		{
			this.RedrawPlayerGroup(playTransitions);
			this.RedrawControllerGroup();
			this.RedrawMapCategoriesGroup(playTransitions);
			this.RedrawInputGrid(listsChanged);
			if (this.currentUISelection == null || !this.currentUISelection.activeInHierarchy)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x06004A6F RID: 19055 RVA: 0x0026C7D4 File Offset: 0x0026ABD4
		private void RedrawPlayerGroup(bool playTransitions)
		{
			if (!this.showPlayers)
			{
				return;
			}
			for (int i = 0; i < this.playerButtons.Count; i++)
			{
				bool flag = this.currentPlayerId != this.playerButtons[i].buttonInfo.intData;
				this.playerButtons[i].SetInteractible(flag, playTransitions);
				this.playerButtons[i].gameObject.GetComponent<CustomButton>().SetNavOnToggle(flag);
			}
			this.playerButtons[1].SetInteractible(PlayerManager.Multiplayer, playTransitions);
		}

		// Token: 0x06004A70 RID: 19056 RVA: 0x0026C874 File Offset: 0x0026AC74
		private void RedrawControllerGroup()
		{
			int num = -1;
			this.references.controllerNameLabel.text = this._language.none;
			UITools.SetInteractable(this.references.removeControllerButton, false, false);
			UITools.SetInteractable(this.references.assignControllerButton, false, false);
			UITools.SetInteractable(this.references.calibrateControllerButton, false, false);
			if (this.ShowAssignedControllers())
			{
				foreach (ControlMapper.GUIButton guibutton in this.assignedControllerButtons)
				{
					if (!(guibutton.gameObject == null))
					{
						if (this.currentUISelection == guibutton.gameObject)
						{
							num = guibutton.buttonInfo.intData;
						}
						UnityEngine.Object.Destroy(guibutton.gameObject);
					}
				}
				this.assignedControllerButtons.Clear();
				this.assignedControllerButtonsPlaceholder.SetActive(true);
			}
			Player player = ReInput.players.GetPlayer(this.currentPlayerId);
			if (player == null)
			{
				return;
			}
			if (this.ShowAssignedControllers())
			{
				if (player.controllers.joystickCount > 0)
				{
					this.assignedControllerButtonsPlaceholder.SetActive(false);
				}
				foreach (Joystick joystick in player.controllers.Joysticks)
				{
					ControlMapper.GUIButton guibutton2 = this.CreateButton(joystick.name, this.references.assignedControllersGroup.content, Vector2.zero);
					guibutton2.SetButtonInfoData("AssignedControllerSelection", joystick.id);
					guibutton2.SetOnClickCallback(new Action<ButtonInfo>(this.OnButtonActivated));
					guibutton2.buttonInfo.OnSelectedEvent += this.OnUIElementSelected;
					this.assignedControllerButtons.Add(guibutton2);
					if (joystick.id == this.currentJoystickId)
					{
						guibutton2.SetInteractible(false, true);
					}
				}
				if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
				{
					this.currentJoystickId = player.controllers.Joysticks[0].id;
					this.assignedControllerButtons[0].SetInteractible(false, false);
				}
				if (num >= 0)
				{
					foreach (ControlMapper.GUIButton guibutton3 in this.assignedControllerButtons)
					{
						if (guibutton3.buttonInfo.intData == num)
						{
							this.SetUISelection(guibutton3.gameObject);
							break;
						}
					}
				}
			}
			else if (player.controllers.joystickCount > 0 && !this.isJoystickSelected)
			{
				this.currentJoystickId = player.controllers.Joysticks[0].id;
			}
			if (this.isJoystickSelected && player.controllers.joystickCount > 0 && this.currentPlayerId == 0)
			{
				this.references.removeControllerButton.interactable = true;
				this.references.controllerNameLabel.text = this.currentJoystick.name;
				if (this.currentJoystick.axisCount > 0)
				{
					this.references.calibrateControllerButton.interactable = true;
				}
			}
			int joystickCount = player.controllers.joystickCount;
			int joystickCount2 = ReInput.controllers.joystickCount;
			int maxControllersPerPlayer = this.GetMaxControllersPerPlayer();
			bool flag = maxControllersPerPlayer == 0;
			if (joystickCount2 > 0 && joystickCount < joystickCount2 && (maxControllersPerPlayer == 1 || flag || joystickCount < maxControllersPerPlayer))
			{
				UITools.SetInteractable(this.references.assignControllerButton, true, false);
			}
		}

		// Token: 0x06004A71 RID: 19057 RVA: 0x0026CC68 File Offset: 0x0026B068
		private void RedrawMapCategoriesGroup(bool playTransitions)
		{
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this.mapCategoryButtons.Count; i++)
			{
				bool state = this.currentMapCategoryId != this.mapCategoryButtons[i].buttonInfo.intData;
				this.mapCategoryButtons[i].SetInteractible(state, playTransitions);
			}
		}

		// Token: 0x06004A72 RID: 19058 RVA: 0x0026CCD2 File Offset: 0x0026B0D2
		private void RedrawInputGrid(bool listsChanged)
		{
			if (listsChanged)
			{
				this.RefreshInputGridStructure();
			}
			this.PopulateInputFields();
		}

		// Token: 0x06004A73 RID: 19059 RVA: 0x0026CCE6 File Offset: 0x0026B0E6
		private void ForceRefresh()
		{
			if (this.windowManager.isWindowOpen)
			{
				this.CloseAllWindows();
			}
			else
			{
				this.Redraw(false, false);
			}
		}

		// Token: 0x06004A74 RID: 19060 RVA: 0x0026CD0C File Offset: 0x0026B10C
		private void CreateInputCategoryRow(ref int rowCount, InputCategory category)
		{
			this.CreateLabel(Localization.Translate(category.descriptiveName).text, this.references.inputGridActionColumn, new Vector2(0f, (float)rowCount * this._inputRowHeight * -1f));
			rowCount++;
		}

		// Token: 0x06004A75 RID: 19061 RVA: 0x0026CD5E File Offset: 0x0026B15E
		private ControlMapper.GUILabel CreateLabel(string labelText, Transform parent, Vector2 offset)
		{
			return this.CreateLabel(this.prefabs.inputGridLabel, labelText, parent, offset);
		}

		// Token: 0x06004A76 RID: 19062 RVA: 0x0026CD74 File Offset: 0x0026B174
		private ControlMapper.GUILabel CreateDeactivatedLabel(string labelText, Transform parent, Vector2 offset)
		{
			return this.CreateLabel(this.prefabs.inputGridDeactivatedLabel, labelText, parent, offset);
		}

		// Token: 0x06004A77 RID: 19063 RVA: 0x0026CD8C File Offset: 0x0026B18C
		private ControlMapper.GUILabel CreateLabel(GameObject prefab, string labelText, Transform parent, Vector2 offset)
		{
			GameObject gameObject = this.InstantiateGUIObject(prefab, parent, offset);
			Text componentInSelfOrChildren = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
			if (componentInSelfOrChildren == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: Label prefab is missing Text component!");
				return null;
			}
			componentInSelfOrChildren.text = labelText;
			return new ControlMapper.GUILabel(gameObject);
		}

		// Token: 0x06004A78 RID: 19064 RVA: 0x0026CDD0 File Offset: 0x0026B1D0
		private ControlMapper.GUIButton CreateButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.button, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06004A79 RID: 19065 RVA: 0x0026CE00 File Offset: 0x0026B200
		private ControlMapper.GUIButton CreateFitButton(string labelText, Transform parent, Vector2 offset)
		{
			ControlMapper.GUIButton guibutton = new ControlMapper.GUIButton(this.InstantiateGUIObject(this.prefabs.fitButton, parent, offset));
			guibutton.SetLabel(labelText);
			return guibutton;
		}

		// Token: 0x06004A7A RID: 19066 RVA: 0x0026CE30 File Offset: 0x0026B230
		private ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIInputField guiinputField = this.CreateInputField(parent, offset);
			guiinputField.SetLabel(string.Empty);
			guiinputField.SetFieldInfoData(actionId, axisRange, controllerType, fieldIndex);
			guiinputField.SetOnClickCallback(this.inputFieldActivatedDelegate);
			guiinputField.fieldInfo.OnSelectedEvent += this.OnUIElementSelected;
			return guiinputField;
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x0026CE83 File Offset: 0x0026B283
		private ControlMapper.GUIInputField CreateInputField(Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIInputField(this.InstantiateGUIObject(this.prefabs.inputGridFieldButton, parent, offset));
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x0026CEA0 File Offset: 0x0026B2A0
		private ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset, string label, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
		{
			ControlMapper.GUIToggle guitoggle = this.CreateToggle(prefab, parent, offset);
			guitoggle.SetToggleInfoData(actionId, axisRange, controllerType, fieldIndex);
			guitoggle.SetOnSubmitCallback(this.inputFieldInvertToggleStateChangedDelegate);
			guitoggle.toggleInfo.OnSelectedEvent += this.OnUIElementSelected;
			return guitoggle;
		}

		// Token: 0x06004A7D RID: 19069 RVA: 0x0026CEE9 File Offset: 0x0026B2E9
		private ControlMapper.GUIToggle CreateToggle(GameObject prefab, Transform parent, Vector2 offset)
		{
			return new ControlMapper.GUIToggle(this.InstantiateGUIObject(prefab, parent, offset));
		}

		// Token: 0x06004A7E RID: 19070 RVA: 0x0026CEFC File Offset: 0x0026B2FC
		private GameObject InstantiateGUIObject(GameObject prefab, Transform parent, Vector2 offset)
		{
			if (prefab == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: Prefab is null!");
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x0026CF34 File Offset: 0x0026B334
		private GameObject CreateNewGUIObject(string name, Transform parent, Vector2 offset)
		{
			GameObject gameObject = new GameObject();
			gameObject.name = name;
			gameObject.AddComponent<RectTransform>();
			return this.InitializeNewGUIGameObject(gameObject, parent, offset);
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x0026CF60 File Offset: 0x0026B360
		private GameObject InitializeNewGUIGameObject(GameObject gameObject, Transform parent, Vector2 offset)
		{
			if (gameObject == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: GameObject is null!");
				return null;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			if (component == null)
			{
				UnityEngine.Debug.LogError("Rewired Control Mapper: GameObject does not have a RectTransform component!");
				return gameObject;
			}
			if (parent != null)
			{
				component.SetParent(parent, false);
			}
			component.anchoredPosition = offset;
			return gameObject;
		}

		// Token: 0x06004A81 RID: 19073 RVA: 0x0026CFC0 File Offset: 0x0026B3C0
		private GameObject CreateNewColumnGroup(string name, Transform parent, int maxWidth)
		{
			GameObject gameObject = this.CreateNewGUIObject(name, parent, Vector2.zero);
			this.inputGrid.AddGroup(gameObject);
			LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
			if (maxWidth >= 0)
			{
				layoutElement.preferredWidth = (float)maxWidth;
			}
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 0f);
			component.anchorMax = new Vector2(1f, 0f);
			return gameObject;
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x0026D02F File Offset: 0x0026B42F
		private Window OpenWindow(bool closeOthers)
		{
			return this.OpenWindow(string.Empty, closeOthers);
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x0026D040 File Offset: 0x0026B440
		private Window OpenWindow(string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(name, this._defaultWindowWidth, this._defaultWindowHeight);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x0026D08C File Offset: 0x0026B48C
		private Window OpenWindow(GameObject windowPrefab, bool closeOthers)
		{
			return this.OpenWindow(windowPrefab, string.Empty, closeOthers);
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x0026D09C File Offset: 0x0026B49C
		private Window OpenWindow(GameObject windowPrefab, string name, bool closeOthers)
		{
			if (closeOthers)
			{
				this.windowManager.CancelAll();
			}
			Window window = this.windowManager.OpenWindow(windowPrefab, name);
			if (window == null)
			{
				return null;
			}
			this.ChildWindowOpened();
			return window;
		}

		// Token: 0x06004A86 RID: 19078 RVA: 0x0026D0E0 File Offset: 0x0026B4E0
		private void OpenModal(string title, string message, string confirmText, Action<int> confirmAction, string cancelText, Action<int> cancelAction, bool closeOthers)
		{
			Window window = this.OpenWindow(closeOthers);
			if (window == null)
			{
				return;
			}
			window.CreateTitleText(this.prefabs.windowTitleText, Vector2.zero, title);
			window.AddContentText(this.prefabs.windowContentText, UIPivot.TopCenter, UIAnchor.TopHStretch, new Vector2(0f, -100f), message);
			UnityAction unityAction = delegate()
			{
				this.OnWindowCancel(window.id);
			};
			window.cancelCallback = unityAction;
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomLeft, UIAnchor.BottomLeft, Vector2.zero, confirmText, delegate()
			{
				this.OnRestoreDefaultsConfirmed(window.id);
			}, unityAction, false);
			window.CreateButton(this.prefabs.fitButton, UIPivot.BottomRight, UIAnchor.BottomRight, Vector2.zero, cancelText, unityAction, unityAction, true);
			this.windowManager.Focus(window);
		}

		// Token: 0x06004A87 RID: 19079 RVA: 0x0026D1EE File Offset: 0x0026B5EE
		private void CloseWindow(int windowId)
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseWindow(windowId);
			this.ChildWindowClosed();
		}

		// Token: 0x06004A88 RID: 19080 RVA: 0x0026D213 File Offset: 0x0026B613
		private void CloseTopWindow()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CloseTop();
			this.ChildWindowClosed();
		}

		// Token: 0x06004A89 RID: 19081 RVA: 0x0026D237 File Offset: 0x0026B637
		private void CloseAllWindows()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.windowManager.CancelAll();
			this.ChildWindowClosed();
			this.InputPollingStopped();
		}

		// Token: 0x06004A8A RID: 19082 RVA: 0x0026D264 File Offset: 0x0026B664
		private void ChildWindowOpened()
		{
			if (!this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(false);
			if (this._PopupWindowOpenedEvent != null)
			{
				this._PopupWindowOpenedEvent();
			}
			if (this._onPopupWindowOpened != null)
			{
				this._onPopupWindowOpened.Invoke();
			}
		}

		// Token: 0x06004A8B RID: 19083 RVA: 0x0026D2B8 File Offset: 0x0026B6B8
		private void ChildWindowClosed()
		{
			if (this.windowManager.isWindowOpen)
			{
				return;
			}
			this.SetIsFocused(true);
			if (this._PopupWindowClosedEvent != null)
			{
				this._PopupWindowClosedEvent();
			}
			if (this._onPopupWindowClosed != null)
			{
				this._onPopupWindowClosed.Invoke();
			}
		}

		// Token: 0x06004A8C RID: 19084 RVA: 0x0026D30C File Offset: 0x0026B70C
		private bool HasElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				return false;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				return ReInput.players.SystemPlayer.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck) || player.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck);
			}
			return ReInput.controllers.conflictChecking.DoesElementAssignmentConflict(conflictCheck);
		}

		// Token: 0x06004A8D RID: 19085 RVA: 0x0026D38C File Offset: 0x0026B78C
		private bool IsBlockingAssignmentConflict(ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				return false;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo.isUserAssignable)
					{
						return true;
					}
				}
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo2 in this.currentPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo2.isUserAssignable)
					{
						return true;
					}
				}
			}
			else
			{
				foreach (ElementAssignmentConflictInfo elementAssignmentConflictInfo3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!elementAssignmentConflictInfo3.isUserAssignable)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004A8E RID: 19086 RVA: 0x0026D4F4 File Offset: 0x0026B8F4
		private IEnumerable<ElementAssignmentConflictInfo> ElementAssignmentConflicts(Player player, ControlMapper.InputMapping mapping, ElementAssignment assignment, bool skipOtherPlayers)
		{
			if (player == null || mapping == null)
			{
				yield break;
			}
			ElementAssignmentConflictCheck conflictCheck;
			if (!this.CreateConflictCheck(mapping, assignment, out conflictCheck))
			{
				yield break;
			}
			if (skipOtherPlayers)
			{
				foreach (ElementAssignmentConflictInfo conflict in ReInput.players.SystemPlayer.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!conflict.isUserAssignable)
					{
						yield return conflict;
					}
				}
				foreach (ElementAssignmentConflictInfo conflict2 in player.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!conflict2.isUserAssignable)
					{
						yield return conflict2;
					}
				}
			}
			else
			{
				foreach (ElementAssignmentConflictInfo conflict3 in ReInput.controllers.conflictChecking.ElementAssignmentConflicts(conflictCheck))
				{
					if (!conflict3.isUserAssignable)
					{
						yield return conflict3;
					}
				}
			}
			yield break;
		}

		// Token: 0x06004A8F RID: 19087 RVA: 0x0026D534 File Offset: 0x0026B934
		private bool CreateConflictCheck(ControlMapper.InputMapping mapping, ElementAssignment assignment, out ElementAssignmentConflictCheck conflictCheck)
		{
			if (mapping == null || this.currentPlayer == null)
			{
				conflictCheck = default(ElementAssignmentConflictCheck);
				return false;
			}
			conflictCheck = assignment.ToElementAssignmentConflictCheck();
			conflictCheck.playerId = this.currentPlayer.id;
			conflictCheck.controllerType = mapping.controllerType;
			conflictCheck.controllerId = mapping.controllerId;
			conflictCheck.controllerMapId = mapping.map.id;
			conflictCheck.controllerMapCategoryId = mapping.map.categoryId;
			if (mapping.aem != null)
			{
				conflictCheck.elementMapId = mapping.aem.id;
			}
			return true;
		}

		// Token: 0x06004A90 RID: 19088 RVA: 0x0026D5D0 File Offset: 0x0026B9D0
		private void PollKeyboardForAssignment(out ControllerPollingInfo pollingInfo, out bool modifierKeyPressed, out ModifierKeyFlags modifierFlags, out string label)
		{
			pollingInfo = default(ControllerPollingInfo);
			label = string.Empty;
			modifierKeyPressed = false;
			modifierFlags = ModifierKeyFlags.None;
			int num = 0;
			ControllerPollingInfo controllerPollingInfo = default(ControllerPollingInfo);
			ControllerPollingInfo controllerPollingInfo2 = default(ControllerPollingInfo);
			ModifierKeyFlags modifierKeyFlags = ModifierKeyFlags.None;
			foreach (ControllerPollingInfo controllerPollingInfo3 in ReInput.controllers.Keyboard.PollForAllKeys())
			{
				KeyCode keyboardKey = controllerPollingInfo3.keyboardKey;
				if (keyboardKey != KeyCode.AltGr)
				{
					if (Keyboard.IsModifierKey(controllerPollingInfo3.keyboardKey))
					{
						if (num == 0)
						{
							controllerPollingInfo2 = controllerPollingInfo3;
							modifierKeyFlags |= Keyboard.KeyCodeToModifierKeyFlags(keyboardKey);
							num++;
						}
					}
					else if (controllerPollingInfo.keyboardKey == KeyCode.None)
					{
						controllerPollingInfo = controllerPollingInfo3;
					}
				}
			}
			if (controllerPollingInfo.keyboardKey == KeyCode.None)
			{
				if (num > 0)
				{
					modifierKeyPressed = true;
					if (num == 1)
					{
						if (ReInput.controllers.Keyboard.GetKeyTimePressed(controllerPollingInfo2.keyboardKey) > 1f)
						{
							pollingInfo = controllerPollingInfo2;
							return;
						}
						label = Localization.Translate(Keyboard.GetKeyName(controllerPollingInfo2.keyboardKey)).text;
					}
					else
					{
						label = Keyboard.ModifierKeyFlagsToString(modifierKeyFlags);
					}
				}
				return;
			}
			if (!ReInput.controllers.Keyboard.GetKeyDown(controllerPollingInfo.keyboardKey))
			{
				return;
			}
			if (num == 0)
			{
				pollingInfo = controllerPollingInfo;
				return;
			}
			pollingInfo = controllerPollingInfo;
			modifierFlags = modifierKeyFlags;
		}

		// Token: 0x06004A91 RID: 19089 RVA: 0x0026D75C File Offset: 0x0026BB5C
		private void StartAxisCalibration(int axisIndex)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			if (this.currentPlayer.controllers.joystickCount == 0)
			{
				return;
			}
			Joystick currentJoystick = this.currentJoystick;
			if (axisIndex < 0 || axisIndex >= currentJoystick.axisCount)
			{
				return;
			}
			this.pendingAxisCalibration = new ControlMapper.AxisCalibrator(currentJoystick, axisIndex);
			this.ShowCalibrateAxisStep1Window();
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x0026D7B9 File Offset: 0x0026BBB9
		private void EndAxisCalibration()
		{
			if (this.pendingAxisCalibration == null)
			{
				return;
			}
			this.pendingAxisCalibration.Commit();
			this.pendingAxisCalibration = null;
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x0026D7D9 File Offset: 0x0026BBD9
		private void SetUISelection(GameObject selection)
		{
			if (EventSystem.current == null)
			{
				return;
			}
			EventSystem.current.SetSelectedGameObject(selection);
		}

		// Token: 0x06004A94 RID: 19092 RVA: 0x0026D7F7 File Offset: 0x0026BBF7
		private void RestoreLastUISelection()
		{
			if (this.lastUISelection == null || !this.lastUISelection.activeInHierarchy)
			{
				this.SetDefaultUISelection();
				return;
			}
			this.SetUISelection(this.lastUISelection);
		}

		// Token: 0x06004A95 RID: 19093 RVA: 0x0026D830 File Offset: 0x0026BC30
		private void SetDefaultUISelection()
		{
			if (!this.isOpen)
			{
				return;
			}
			if (this.references.defaultSelection == null)
			{
				this.SetUISelection(null);
			}
			else
			{
				this.SetUISelection(this.references.defaultSelection.gameObject);
			}
		}

		// Token: 0x06004A96 RID: 19094 RVA: 0x0026D884 File Offset: 0x0026BC84
		private void SelectDefaultMapCategory(bool redraw)
		{
			this.currentMapCategoryId = this.GetDefaultMapCategoryId();
			this.OnMapCategorySelected(this.currentMapCategoryId, redraw);
			if (!this.showMapCategories)
			{
				return;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					this.currentMapCategoryId = this._mappingSets[i].mapCategoryId;
					break;
				}
			}
			if (this.currentMapCategoryId < 0)
			{
				return;
			}
			for (int j = 0; j < this._mappingSets.Length; j++)
			{
				bool state = this._mappingSets[j].mapCategoryId != this.currentMapCategoryId;
				this.mapCategoryButtons[j].SetInteractible(state, false);
			}
		}

		// Token: 0x06004A97 RID: 19095 RVA: 0x0026D963 File Offset: 0x0026BD63
		private void CheckUISelection()
		{
			if (!this.isFocused)
			{
				return;
			}
			if (this.currentUISelection == null)
			{
				this.RestoreLastUISelection();
			}
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x0026D988 File Offset: 0x0026BD88
		private void OnUIElementSelected(GameObject selectedObject)
		{
			this.lastUISelection = selectedObject;
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x0026D991 File Offset: 0x0026BD91
		private void SetIsFocused(bool state)
		{
			this.references.mainCanvasGroup.interactable = state;
			if (state)
			{
				this.Redraw(false, false);
				this.RestoreLastUISelection();
				this.blockInputOnFocusEndTime = Time.unscaledTime + 0.1f;
			}
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x0026D9C9 File Offset: 0x0026BDC9
		public void Toggle()
		{
			if (this.isOpen)
			{
				this.Close(true);
			}
			else
			{
				this.Open();
			}
		}

		// Token: 0x06004A9B RID: 19099 RVA: 0x0026D9E8 File Offset: 0x0026BDE8
		public void Open()
		{
			this.Open(false);
		}

		// Token: 0x06004A9C RID: 19100 RVA: 0x0026D9F4 File Offset: 0x0026BDF4
		private void Open(bool force)
		{
			if (!this.initialized)
			{
				this.Initialize();
			}
			if (!this.initialized)
			{
				return;
			}
			if (!force && this.isOpen)
			{
				return;
			}
			this.Clear();
			this.canvas.SetActive(true);
			this.OnPlayerSelected(0, false);
			this.SelectDefaultMapCategory(false);
			this.SetDefaultUISelection();
			this.Redraw(true, false);
			if (this._ScreenOpenedEvent != null)
			{
				this._ScreenOpenedEvent();
			}
			if (this._onScreenOpened != null)
			{
				this._onScreenOpened.Invoke();
			}
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x0026DA8C File Offset: 0x0026BE8C
		public void Close(bool save)
		{
			if (!this.initialized)
			{
				return;
			}
			if (!this.isOpen)
			{
				return;
			}
			if (save && ReInput.userDataStore != null)
			{
				ReInput.userDataStore.Save();
			}
			this.Clear();
			this.canvas.SetActive(false);
			this.SetUISelection(null);
			if (this._ScreenClosedEvent != null)
			{
				this._ScreenClosedEvent();
			}
			if (this._onScreenClosed != null)
			{
				this._onScreenClosed.Invoke();
			}
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x0026DB10 File Offset: 0x0026BF10
		private void Clear()
		{
			this.windowManager.CancelAll();
			this.lastUISelection = null;
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.InputPollingStopped();
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x0026DB38 File Offset: 0x0026BF38
		private void ClearCompletely()
		{
			this.ClearSpawnedObjects();
			this.ClearAllVars();
		}

		// Token: 0x06004AA0 RID: 19104 RVA: 0x0026DB48 File Offset: 0x0026BF48
		private void ClearSpawnedObjects()
		{
			this.windowManager.ClearCompletely();
			this.inputGrid.ClearAll();
			foreach (ControlMapper.GUIButton guibutton in this.playerButtons)
			{
				UnityEngine.Object.Destroy(guibutton.gameObject);
			}
			this.playerButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton2 in this.mapCategoryButtons)
			{
				UnityEngine.Object.Destroy(guibutton2.gameObject);
			}
			this.mapCategoryButtons.Clear();
			foreach (ControlMapper.GUIButton guibutton3 in this.assignedControllerButtons)
			{
				UnityEngine.Object.Destroy(guibutton3.gameObject);
			}
			this.assignedControllerButtons.Clear();
			if (this.assignedControllerButtonsPlaceholder != null)
			{
				UnityEngine.Object.Destroy(this.assignedControllerButtonsPlaceholder.gameObject);
				this.assignedControllerButtonsPlaceholder = null;
			}
			foreach (GameObject obj in this.miscInstantiatedObjects)
			{
				UnityEngine.Object.Destroy(obj);
			}
			this.miscInstantiatedObjects.Clear();
		}

		// Token: 0x06004AA1 RID: 19105 RVA: 0x0026DCFC File Offset: 0x0026C0FC
		private void ClearVarsOnPlayerChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x0026DD05 File Offset: 0x0026C105
		private void ClearVarsOnJoystickChange()
		{
			this.currentJoystickId = -1;
		}

		// Token: 0x06004AA3 RID: 19107 RVA: 0x0026DD10 File Offset: 0x0026C110
		private void ClearAllVars()
		{
			this.initialized = false;
			ControlMapper.Instance = null;
			this.playerCount = 0;
			this.inputGrid = null;
			this.windowManager = null;
			this.currentPlayerId = -1;
			this.currentMapCategoryId = -1;
			this.playerButtons = null;
			this.mapCategoryButtons = null;
			this.miscInstantiatedObjects = null;
			this.canvas = null;
			this.lastUISelection = null;
			this.currentJoystickId = -1;
			this.axisToggleObjects.Clear();
			this.inactiveAxisToggleObjects.Clear();
			this.pendingInputMapping = null;
			this.pendingAxisCalibration = null;
			this.inputFieldActivatedDelegate = null;
			this.inputFieldInvertToggleStateChangedDelegate = null;
			this.isPollingForInput = false;
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x0026DDB0 File Offset: 0x0026C1B0
		public void Reset()
		{
			if (!this.initialized)
			{
				return;
			}
			this.ClearCompletely();
			this.Initialize();
			if (this.isOpen)
			{
				this.Open(true);
			}
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x0026DDDC File Offset: 0x0026C1DC
		private void SetActionAxisInverted(bool state, ControllerType controllerType, int actionElementMapId)
		{
			if (this.currentPlayer == null)
			{
				return;
			}
			ControllerMapWithAxes controllerMapWithAxes = this.GetControllerMap(controllerType) as ControllerMapWithAxes;
			if (controllerMapWithAxes == null)
			{
				return;
			}
			ActionElementMap elementMap = controllerMapWithAxes.GetElementMap(actionElementMapId);
			if (elementMap == null)
			{
				return;
			}
			elementMap.invert = state;
		}

		// Token: 0x06004AA6 RID: 19110 RVA: 0x0026DE20 File Offset: 0x0026C220
		private ControllerMap GetControllerMap(ControllerType type)
		{
			if (this.currentPlayer == null)
			{
				return null;
			}
			int controllerId = 0;
			switch (type)
			{
			case ControllerType.Keyboard:
				break;
			case ControllerType.Mouse:
				break;
			case ControllerType.Joystick:
				if (this.currentPlayer.controllers.joystickCount <= 0)
				{
					return null;
				}
				controllerId = this.currentJoystick.id;
				break;
			default:
				throw new NotImplementedException();
			}
			return this.currentPlayer.controllers.maps.GetFirstMapInCategory(type, controllerId, this.currentMapCategoryId);
		}

		// Token: 0x06004AA7 RID: 19111 RVA: 0x0026DEB0 File Offset: 0x0026C2B0
		private ControllerMap GetControllerMapOrCreateNew(ControllerType controllerType, int controllerId, int layoutId)
		{
			ControllerMap controllerMap = this.GetControllerMap(controllerType);
			if (controllerMap == null)
			{
				this.currentPlayer.controllers.maps.AddEmptyMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
				controllerMap = this.currentPlayer.controllers.maps.GetMap(controllerType, controllerId, this.currentMapCategoryId, layoutId);
			}
			return controllerMap;
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x0026DF0C File Offset: 0x0026C30C
		private int CountIEnumerable<T>(IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				return 0;
			}
			IEnumerator<T> enumerator = enumerable.GetEnumerator();
			if (enumerator == null)
			{
				return 0;
			}
			int num = 0;
			while (enumerator.MoveNext())
			{
				num++;
			}
			return num;
		}

		// Token: 0x06004AA9 RID: 19113 RVA: 0x0026DF48 File Offset: 0x0026C348
		private int GetDefaultMapCategoryId()
		{
			if (this._mappingSets.Length == 0)
			{
				return 0;
			}
			for (int i = 0; i < this._mappingSets.Length; i++)
			{
				if (ReInput.mapping.GetMapCategory(this._mappingSets[i].mapCategoryId) != null)
				{
					return this._mappingSets[i].mapCategoryId;
				}
			}
			return 0;
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x0026DFB0 File Offset: 0x0026C3B0
		private void SubscribeFixedUISelectionEvents()
		{
			if (this.references.fixedSelectableUIElements == null)
			{
				return;
			}
			foreach (GameObject gameObject in this.references.fixedSelectableUIElements)
			{
				UIElementInfo component = UnityTools.GetComponent<UIElementInfo>(gameObject);
				if (!(component == null))
				{
					component.OnSelectedEvent += this.OnUIElementSelected;
				}
			}
		}

		// Token: 0x06004AAB RID: 19115 RVA: 0x0026E01C File Offset: 0x0026C41C
		private void SubscribeMenuControlInputEvents()
		{
			this.SubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.SubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06004AAC RID: 19116 RVA: 0x0026E08C File Offset: 0x0026C48C
		private void UnsubscribeMenuControlInputEvents()
		{
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenToggleAction, new Action<InputActionEventData>(this.OnScreenToggleActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenOpenAction, new Action<InputActionEventData>(this.OnScreenOpenActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._screenCloseAction, new Action<InputActionEventData>(this.OnScreenCloseActionPressed));
			this.UnsubscribeRewiredInputEventAllPlayers(this._universalCancelAction, new Action<InputActionEventData>(this.OnUniversalCancelActionPressed));
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x0026E0FC File Offset: 0x0026C4FC
		private void SubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				UnityEngine.Debug.LogWarning("Rewired Control Mapper: " + actionId + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.AddInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
			}
		}

		// Token: 0x06004AAE RID: 19118 RVA: 0x0026E198 File Offset: 0x0026C598
		private void UnsubscribeRewiredInputEventAllPlayers(int actionId, Action<InputActionEventData> callback)
		{
			if (actionId < 0 || callback == null)
			{
				return;
			}
			if (!ReInput.isReady)
			{
				return;
			}
			if (ReInput.mapping.GetAction(actionId) == null)
			{
				UnityEngine.Debug.LogWarning("Rewired Control Mapper: " + actionId + " is not a valid Action id!");
				return;
			}
			foreach (Player player in ReInput.players.AllPlayers)
			{
				player.RemoveInputEventDelegate(callback, UpdateLoopType.Update, InputActionEventType.ButtonJustPressed, actionId);
			}
		}

		// Token: 0x06004AAF RID: 19119 RVA: 0x0026E23C File Offset: 0x0026C63C
		private int GetMaxControllersPerPlayer()
		{
			if (this._rewiredInputManager.userData.ConfigVars.autoAssignJoysticks)
			{
				return this._rewiredInputManager.userData.ConfigVars.maxJoysticksPerPlayer;
			}
			return this._maxControllersPerPlayer;
		}

		// Token: 0x06004AB0 RID: 19120 RVA: 0x0026E274 File Offset: 0x0026C674
		private bool ShowAssignedControllers()
		{
			return this._showControllers && (this._showAssignedControllers || this.GetMaxControllersPerPlayer() != 1);
		}

		// Token: 0x06004AB1 RID: 19121 RVA: 0x0026E29F File Offset: 0x0026C69F
		private void InspectorPropertyChanged(bool reset = false)
		{
			if (reset)
			{
				this.Reset();
			}
		}

		// Token: 0x06004AB2 RID: 19122 RVA: 0x0026E2B0 File Offset: 0x0026C6B0
		private void AssignController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (player.controllers.ContainsController(ControllerType.Joystick, controllerId))
			{
				return;
			}
			if (this.GetMaxControllersPerPlayer() == 1)
			{
				this.RemoveAllControllers(player);
				this.ClearVarsOnJoystickChange();
			}
			foreach (Player player2 in ReInput.players.Players)
			{
				if (player2 != player)
				{
					this.RemoveController(player2, controllerId);
				}
			}
			player.controllers.AddController(ControllerType.Joystick, controllerId, false);
			PlayerManager.ControllerRemapped((this.currentPlayerId != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne, true, controllerId);
			this.OnPlayerSelected(this.currentPlayerId, true);
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.LoadControllerData(player.id, ControllerType.Joystick, controllerId);
			}
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x0026E3A0 File Offset: 0x0026C7A0
		private void RemoveAllControllers(Player player)
		{
			if (player == null)
			{
				return;
			}
			IList<Joystick> joysticks = player.controllers.Joysticks;
			for (int i = joysticks.Count - 1; i >= 0; i--)
			{
				this.RemoveController(player, joysticks[i].id);
			}
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x0026E3EC File Offset: 0x0026C7EC
		private void RemoveController(Player player, int controllerId)
		{
			if (player == null)
			{
				return;
			}
			if (!player.controllers.ContainsController(ControllerType.Joystick, controllerId))
			{
				return;
			}
			if (ReInput.userDataStore != null)
			{
				ReInput.userDataStore.SaveControllerData(player.id, ControllerType.Joystick, controllerId);
			}
			player.controllers.RemoveController(ControllerType.Joystick, controllerId);
			PlayerManager.ControllerRemapped((this.currentPlayerId != 0) ? PlayerId.PlayerTwo : PlayerId.PlayerOne, false, 0);
			this.OnPlayerSelected(this.currentPlayerId, true);
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x0026E462 File Offset: 0x0026C862
		private bool IsAllowedAssignment(ControlMapper.InputMapping pendingInputMapping, ControllerPollingInfo pollingInfo)
		{
			return pendingInputMapping != null && (pendingInputMapping.axisRange != AxisRange.Full || this._showSplitAxisInputFields || pollingInfo.elementType != ControllerElementType.Button);
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x0026E494 File Offset: 0x0026C894
		private void InputPollingStarted()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = true;
			if (!flag)
			{
				if (this._InputPollingStartedEvent != null)
				{
					this._InputPollingStartedEvent();
				}
				if (this._onInputPollingStarted != null)
				{
					this._onInputPollingStarted.Invoke();
				}
			}
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x0026E4E4 File Offset: 0x0026C8E4
		private void InputPollingStopped()
		{
			bool flag = this.isPollingForInput;
			this.isPollingForInput = false;
			if (flag)
			{
				if (this._InputPollingEndedEvent != null)
				{
					this._InputPollingEndedEvent();
				}
				if (this._onInputPollingEnded != null)
				{
					this._onInputPollingEnded.Invoke();
				}
			}
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x0026E531 File Offset: 0x0026C931
		private void OnControlsChanged()
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.Redraw(false, false);
			}
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x0026E54C File Offset: 0x0026C94C
		public static void ApplyTheme(ThemedElement.ElementInfo[] elementInfo)
		{
			if (ControlMapper.Instance == null)
			{
				return;
			}
			if (ControlMapper.Instance._themeSettings == null)
			{
				return;
			}
			if (!ControlMapper.Instance._useThemeSettings)
			{
				return;
			}
			ControlMapper.Instance._themeSettings[Mathf.Clamp(ControlMapper.Instance.currentPlayerId, 0, 1)].Apply(elementInfo);
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x0026E5AC File Offset: 0x0026C9AC
		public static LanguageData GetLanguage()
		{
			if (ControlMapper.Instance == null)
			{
				return null;
			}
			return ControlMapper.Instance._language;
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x0026E5CA File Offset: 0x0026C9CA
		public static int CurrentPlayer()
		{
			return ControlMapper.Instance.currentPlayerId;
		}

		// Token: 0x04004FB6 RID: 20406
		private const float blockInputOnFocusTimeout = 0.1f;

		// Token: 0x04004FB7 RID: 20407
		private const string buttonIdentifier_playerSelection = "PlayerSelection";

		// Token: 0x04004FB8 RID: 20408
		private const string buttonIdentifier_removeController = "RemoveController";

		// Token: 0x04004FB9 RID: 20409
		private const string buttonIdentifier_assignController = "AssignController";

		// Token: 0x04004FBA RID: 20410
		private const string buttonIdentifier_calibrateController = "CalibrateController";

		// Token: 0x04004FBB RID: 20411
		private const string buttonIdentifier_editInputBehaviors = "EditInputBehaviors";

		// Token: 0x04004FBC RID: 20412
		private const string buttonIdentifier_mapCategorySelection = "MapCategorySelection";

		// Token: 0x04004FBD RID: 20413
		private const string buttonIdentifier_assignedControllerSelection = "AssignedControllerSelection";

		// Token: 0x04004FBE RID: 20414
		private const string buttonIdentifier_done = "Done";

		// Token: 0x04004FBF RID: 20415
		private const string buttonIdentifier_restoreDefaults = "RestoreDefaults";

		// Token: 0x04004FC0 RID: 20416
		private const string buttonIdentifier_toggleRumble = "ToggleRumble";

		// Token: 0x04004FC1 RID: 20417
		[SerializeField]
		[Tooltip("Must be assigned a Rewired Input Manager scene object or prefab.")]
		private InputManager _rewiredInputManager;

		// Token: 0x04004FC2 RID: 20418
		[SerializeField]
		[Tooltip("Set to True to prevent the Game Object from being destroyed when a new scene is loaded.\n\nNOTE: Changing this value from True to False at runtime will have no effect because Object.DontDestroyOnLoad cannot be undone once set.")]
		private bool _dontDestroyOnLoad;

		// Token: 0x04004FC3 RID: 20419
		[SerializeField]
		[Tooltip("Open the control mapping screen immediately on start. Mainly used for testing.")]
		private bool _openOnStart;

		// Token: 0x04004FC4 RID: 20420
		[SerializeField]
		[Tooltip("The Layout of the Keyboard Maps to be displayed.")]
		private int _keyboardMapDefaultLayout;

		// Token: 0x04004FC5 RID: 20421
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		private int _mouseMapDefaultLayout;

		// Token: 0x04004FC6 RID: 20422
		[SerializeField]
		[Tooltip("The Layout of the Mouse Maps to be displayed.")]
		private int _joystickMapDefaultLayout;

		// Token: 0x04004FC7 RID: 20423
		[SerializeField]
		private ControlMapper.MappingSet[] _mappingSets = new ControlMapper.MappingSet[]
		{
			ControlMapper.MappingSet.Default
		};

		// Token: 0x04004FC8 RID: 20424
		[SerializeField]
		[Tooltip("Display a selectable list of Players. If your game only supports 1 player, you can disable this.")]
		private bool _showPlayers = true;

		// Token: 0x04004FC9 RID: 20425
		[SerializeField]
		[Tooltip("Display the Controller column for input mapping.")]
		private bool _showControllers = true;

		// Token: 0x04004FCA RID: 20426
		[SerializeField]
		[Tooltip("Display the Keyboard column for input mapping.")]
		private bool _showKeyboard = true;

		// Token: 0x04004FCB RID: 20427
		[SerializeField]
		[Tooltip("Display the Mouse column for input mapping.")]
		private bool _showMouse = true;

		// Token: 0x04004FCC RID: 20428
		[SerializeField]
		[Tooltip("The maximum number of controllers allowed to be assigned to a Player. If set to any value other than 1, a selectable list of currently-assigned controller will be displayed to the user. [0 = infinite]")]
		private int _maxControllersPerPlayer = 1;

		// Token: 0x04004FCD RID: 20429
		[SerializeField]
		[Tooltip("Display section labels for each Action Category in the input field grid. Only applies if Action Categories are used to display the Action list.")]
		private bool _showActionCategoryLabels;

		// Token: 0x04004FCE RID: 20430
		[SerializeField]
		[Tooltip("The number of input fields to display for the keyboard. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _keyboardInputFieldCount = 2;

		// Token: 0x04004FCF RID: 20431
		[SerializeField]
		[Tooltip("The number of input fields to display for the mouse. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _mouseInputFieldCount = 1;

		// Token: 0x04004FD0 RID: 20432
		[SerializeField]
		[Tooltip("The number of input fields to display for joysticks. If you want to support alternate mappings on the same device, set this to 2 or more.")]
		private int _controllerInputFieldCount = 1;

		// Token: 0x04004FD1 RID: 20433
		[SerializeField]
		[Tooltip("Display a full-axis input assignment field for every axis-type Action in the input field grid. Also displays an invert toggle for the user  to invert the full-axis assignment direction.\n\n*IMPORTANT*: This field is required if you have made any full-axis assignments in the Rewired Input Manager or in saved XML user data. Disabling this field when you have full-axis assignments will result in the inability for the user to view, remove, or modify these full-axis assignments. In addition, these assignments may cause conflicts when trying to remap the same axes to Actions.")]
		private bool _showFullAxisInputFields = true;

		// Token: 0x04004FD2 RID: 20434
		[SerializeField]
		[Tooltip("Display a positive and negative input assignment field for every axis-type Action in the input field grid.\n\n*IMPORTANT*: These fields are required to assign buttons, keyboard keys, and hat or D-Pad directions to axis-type Actions. If you have made any split-axis assignments or button/key/D-pad assignments to axis-type Actions in the Rewired Input Manager or in saved XML user data, disabling these fields will result in the inability for the user to view, remove, or modify these assignments. In addition, these assignments may cause conflicts when trying to remap the same elements to Actions.")]
		private bool _showSplitAxisInputFields = true;

		// Token: 0x04004FD3 RID: 20435
		[SerializeField]
		[Tooltip("If enabled, when an element assignment conflict is found, an option will be displayed that allows the user to make the conflicting assignment anyway.")]
		private bool _allowElementAssignmentConflicts;

		// Token: 0x04004FD4 RID: 20436
		[SerializeField]
		[Tooltip("The width in relative pixels of the Action label column.")]
		private int _actionLabelWidth = 360;

		// Token: 0x04004FD5 RID: 20437
		[SerializeField]
		[Tooltip("The width in relative pixels of the Keyboard column.")]
		private int _keyboardColMaxWidth = 360;

		// Token: 0x04004FD6 RID: 20438
		[SerializeField]
		[Tooltip("The width in relative pixels of the Mouse column.")]
		private int _mouseColMaxWidth = 200;

		// Token: 0x04004FD7 RID: 20439
		[SerializeField]
		[Tooltip("The width in relative pixels of the Controller column.")]
		private int _controllerColMaxWidth = 200;

		// Token: 0x04004FD8 RID: 20440
		[SerializeField]
		[Tooltip("The height in relative pixels of the input grid button rows.")]
		private float _inputRowHeight = 40f;

		// Token: 0x04004FD9 RID: 20441
		[SerializeField]
		[Tooltip("The width in relative pixels of spacing between columns.")]
		private float _inputColumnSpacing = 40f;

		// Token: 0x04004FDA RID: 20442
		[SerializeField]
		[Tooltip("The height in relative pixels of the space between Action Category sections. Only applicable if Show Action Category Labels is checked.")]
		private int _inputRowCategorySpacing = 20;

		// Token: 0x04004FDB RID: 20443
		[SerializeField]
		[Tooltip("The width in relative pixels of the invert toggle buttons.")]
		private int _invertToggleWidth = 40;

		// Token: 0x04004FDC RID: 20444
		[SerializeField]
		[Tooltip("The width in relative pixels of generated popup windows.")]
		private int _defaultWindowWidth = 500;

		// Token: 0x04004FDD RID: 20445
		[SerializeField]
		[Tooltip("The height in relative pixels of generated popup windows.")]
		private int _defaultWindowHeight = 400;

		// Token: 0x04004FDE RID: 20446
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning a controller to a Player. If this time elapses with no user input a controller, the assignment will be canceled.")]
		private float _controllerAssignmentTimeout = 5f;

		// Token: 0x04004FDF RID: 20447
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller while waiting for axes to be centered before assigning input.")]
		private float _preInputAssignmentTimeout = 5f;

		// Token: 0x04004FE0 RID: 20448
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller when assigning input. If this time elapses with no user input on the target controller, the assignment will be canceled.")]
		private float _inputAssignmentTimeout = 5f;

		// Token: 0x04004FE1 RID: 20449
		[SerializeField]
		[Tooltip("The time in seconds the user has to press an element on a controller during calibration.")]
		private float _axisCalibrationTimeout = 5f;

		// Token: 0x04004FE2 RID: 20450
		[SerializeField]
		[Tooltip("If checked, mouse X-axis movement will always be ignored during input assignment. Check this if you don't want the horizontal mouse axis to be user-assignable to any Actions.")]
		private bool _ignoreMouseXAxisAssignment = true;

		// Token: 0x04004FE3 RID: 20451
		[SerializeField]
		[Tooltip("If checked, mouse Y-axis movement will always be ignored during input assignment. Check this if you don't want the vertical mouse axis to be user-assignable to any Actions.")]
		private bool _ignoreMouseYAxisAssignment = true;

		// Token: 0x04004FE4 RID: 20452
		[SerializeField]
		[Tooltip("An Action that when activated will alternately close or open the main screen as long as no popup windows are open.")]
		private int _screenToggleAction = -1;

		// Token: 0x04004FE5 RID: 20453
		[SerializeField]
		[Tooltip("An Action that when activated will open the main screen if it is closed.")]
		private int _screenOpenAction = -1;

		// Token: 0x04004FE6 RID: 20454
		[SerializeField]
		[Tooltip("An Action that when activated will close the main screen as long as no popup windows are open.")]
		private int _screenCloseAction = -1;

		// Token: 0x04004FE7 RID: 20455
		[SerializeField]
		[Tooltip("An Action that when activated will cancel and close any open popup window. Use with care because the element assigned to this Action can never be mapped by the user (because it would just cancel his assignment).")]
		private int _universalCancelAction = -1;

		// Token: 0x04004FE8 RID: 20456
		[SerializeField]
		[Tooltip("If enabled, Universal Cancel will also close the main screen if pressed when no windows are open.")]
		private bool _universalCancelClosesScreen = true;

		// Token: 0x04004FE9 RID: 20457
		[SerializeField]
		[Tooltip("If checked, controls will be displayed which will allow the user to customize certain Input Behavior settings.")]
		private bool _showInputBehaviorSettings;

		// Token: 0x04004FEA RID: 20458
		[SerializeField]
		[Tooltip("Customizable settings for user-modifiable Input Behaviors. This can be used for settings like Mouse Look Sensitivity.")]
		private ControlMapper.InputBehaviorSettings[] _inputBehaviorSettings;

		// Token: 0x04004FEB RID: 20459
		[SerializeField]
		[Tooltip("If enabled, UI elements will be themed based on the settings in Theme Settings.")]
		private bool _useThemeSettings = true;

		// Token: 0x04004FEC RID: 20460
		[SerializeField]
		[Tooltip("Must be assigned a ThemeSettings object. Used to theme UI elements.")]
		private ThemeSettings[] _themeSettings;

		// Token: 0x04004FED RID: 20461
		[SerializeField]
		[Tooltip("Must be assigned a LanguageData object. Used to retrieve language entries for UI elements.")]
		private LanguageData _language;

		// Token: 0x04004FEE RID: 20462
		[SerializeField]
		[Tooltip("A list of prefabs. You should not have to modify this.")]
		private ControlMapper.Prefabs prefabs;

		// Token: 0x04004FEF RID: 20463
		[SerializeField]
		[Tooltip("A list of references to elements in the hierarchy. You should not have to modify this.")]
		private ControlMapper.References references;

		// Token: 0x04004FF0 RID: 20464
		[SerializeField]
		[Tooltip("Show the label for the Players button group?")]
		private bool _showPlayersGroupLabel = true;

		// Token: 0x04004FF1 RID: 20465
		[SerializeField]
		[Tooltip("Show the label for the Controller button group?")]
		private bool _showControllerGroupLabel = true;

		// Token: 0x04004FF2 RID: 20466
		[SerializeField]
		[Tooltip("Show the label for the Assigned Controllers button group?")]
		private bool _showAssignedControllersGroupLabel = true;

		// Token: 0x04004FF3 RID: 20467
		[SerializeField]
		[Tooltip("Show the label for the Settings button group?")]
		private bool _showSettingsGroupLabel = true;

		// Token: 0x04004FF4 RID: 20468
		[SerializeField]
		[Tooltip("Show the label for the Map Categories button group?")]
		private bool _showMapCategoriesGroupLabel = true;

		// Token: 0x04004FF5 RID: 20469
		[SerializeField]
		[Tooltip("Show the label for the current controller name?")]
		private bool _showControllerNameLabel = true;

		// Token: 0x04004FF6 RID: 20470
		[SerializeField]
		[Tooltip("Show the Assigned Controllers group? If joystick auto-assignment is enabled in the Rewired Input Manager and the max joysticks per player is set to any value other than 1, the Assigned Controllers group will always be displayed.")]
		private bool _showAssignedControllers = true;

		// Token: 0x04004FF7 RID: 20471
		[SerializeField]
		private Text _rumbleButtonText;

		// Token: 0x04004FF8 RID: 20472
		private List<GameObject> axisToggleObjects = new List<GameObject>();

		// Token: 0x04004FF9 RID: 20473
		private List<GameObject> inactiveAxisToggleObjects = new List<GameObject>();

		// Token: 0x04004FFA RID: 20474
		private Action _ScreenClosedEvent;

		// Token: 0x04004FFB RID: 20475
		private Action _ScreenOpenedEvent;

		// Token: 0x04004FFC RID: 20476
		private Action _PopupWindowOpenedEvent;

		// Token: 0x04004FFD RID: 20477
		private Action _PopupWindowClosedEvent;

		// Token: 0x04004FFE RID: 20478
		private Action _InputPollingStartedEvent;

		// Token: 0x04004FFF RID: 20479
		private Action _InputPollingEndedEvent;

		// Token: 0x04005000 RID: 20480
		[SerializeField]
		[Tooltip("Event sent when the UI is closed.")]
		private UnityEvent _onScreenClosed;

		// Token: 0x04005001 RID: 20481
		[SerializeField]
		[Tooltip("Event sent when the UI is opened.")]
		private UnityEvent _onScreenOpened;

		// Token: 0x04005002 RID: 20482
		[SerializeField]
		[Tooltip("Event sent when a popup window is closed.")]
		private UnityEvent _onPopupWindowClosed;

		// Token: 0x04005003 RID: 20483
		[SerializeField]
		[Tooltip("Event sent when a popup window is opened.")]
		private UnityEvent _onPopupWindowOpened;

		// Token: 0x04005004 RID: 20484
		[SerializeField]
		[Tooltip("Event sent when polling for input has started.")]
		private UnityEvent _onInputPollingStarted;

		// Token: 0x04005005 RID: 20485
		[SerializeField]
		[Tooltip("Event sent when polling for input has ended.")]
		private UnityEvent _onInputPollingEnded;

		// Token: 0x04005006 RID: 20486
		private static ControlMapper Instance;

		// Token: 0x04005007 RID: 20487
		private bool initialized;

		// Token: 0x04005008 RID: 20488
		private int playerCount;

		// Token: 0x04005009 RID: 20489
		private ControlMapper.InputGrid inputGrid;

		// Token: 0x0400500A RID: 20490
		private ControlMapper.WindowManager windowManager;

		// Token: 0x0400500B RID: 20491
		public int currentPlayerId;

		// Token: 0x0400500C RID: 20492
		private int currentMapCategoryId;

		// Token: 0x0400500D RID: 20493
		private List<ControlMapper.GUIButton> playerButtons;

		// Token: 0x0400500E RID: 20494
		private List<ControlMapper.GUIButton> mapCategoryButtons;

		// Token: 0x0400500F RID: 20495
		private List<ControlMapper.GUIButton> assignedControllerButtons;

		// Token: 0x04005010 RID: 20496
		private ControlMapper.GUIButton assignedControllerButtonsPlaceholder;

		// Token: 0x04005011 RID: 20497
		private List<GameObject> miscInstantiatedObjects;

		// Token: 0x04005012 RID: 20498
		private GameObject canvas;

		// Token: 0x04005013 RID: 20499
		private GameObject lastUISelection;

		// Token: 0x04005014 RID: 20500
		private int currentJoystickId = -1;

		// Token: 0x04005015 RID: 20501
		private float blockInputOnFocusEndTime;

		// Token: 0x04005016 RID: 20502
		private bool isPollingForInput;

		// Token: 0x04005017 RID: 20503
		private ControlMapper.InputMapping pendingInputMapping;

		// Token: 0x04005018 RID: 20504
		private ControlMapper.AxisCalibrator pendingAxisCalibration;

		// Token: 0x04005019 RID: 20505
		private Action<InputFieldInfo> inputFieldActivatedDelegate;

		// Token: 0x0400501A RID: 20506
		private Action<ToggleInfo, bool> inputFieldInvertToggleStateChangedDelegate;

		// Token: 0x0400501B RID: 20507
		private Action _restoreDefaultsDelegate;

		// Token: 0x02000C0E RID: 3086
		// (Invoke) Token: 0x06004ABD RID: 19133
		public delegate void PlayerChangeAction();

		// Token: 0x02000C0F RID: 3087
		private abstract class GUIElement
		{
			// Token: 0x06004AC0 RID: 19136 RVA: 0x0026E5D8 File Offset: 0x0026C9D8
			public GUIElement(GameObject gameObject)
			{
				if (gameObject == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.selectable = gameObject.GetComponent<Selectable>();
				if (this.selectable == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.gameObject = gameObject;
				this.rectTransform = gameObject.GetComponent<RectTransform>();
				this.text = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				this.uiElementInfo = gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x06004AC1 RID: 19137 RVA: 0x0026E660 File Offset: 0x0026CA60
			public GUIElement(Selectable selectable, Text label)
			{
				if (selectable == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: Selectable is null!");
					return;
				}
				this.selectable = selectable;
				this.gameObject = selectable.gameObject;
				this.rectTransform = this.gameObject.GetComponent<RectTransform>();
				this.text = label;
				this.uiElementInfo = this.gameObject.GetComponent<UIElementInfo>();
				this.children = new List<ControlMapper.GUIElement>();
			}

			// Token: 0x170006D4 RID: 1748
			// (get) Token: 0x06004AC2 RID: 19138 RVA: 0x0026E6D1 File Offset: 0x0026CAD1
			// (set) Token: 0x06004AC3 RID: 19139 RVA: 0x0026E6D9 File Offset: 0x0026CAD9
			public RectTransform rectTransform { get; private set; }

			// Token: 0x06004AC4 RID: 19140 RVA: 0x0026E6E2 File Offset: 0x0026CAE2
			public virtual void SetInteractible(bool state, bool playTransition)
			{
				this.SetInteractible(state, playTransition, false);
			}

			// Token: 0x06004AC5 RID: 19141 RVA: 0x0026E6F0 File Offset: 0x0026CAF0
			public virtual void SetInteractible(bool state, bool playTransition, bool permanent)
			{
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null)
					{
						this.children[i].SetInteractible(state, playTransition, permanent);
					}
				}
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.selectable == null)
				{
					return;
				}
				if (permanent)
				{
					this.permanentStateSet = true;
				}
				if (this.selectable.interactable == state)
				{
					return;
				}
				UITools.SetInteractable(this.selectable, state, playTransition);
			}

			// Token: 0x06004AC6 RID: 19142 RVA: 0x0026E790 File Offset: 0x0026CB90
			public virtual void SetTextWidth(int value)
			{
				if (this.text == null)
				{
					return;
				}
				LayoutElement layoutElement = this.text.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = this.text.gameObject.AddComponent<LayoutElement>();
				}
				layoutElement.preferredWidth = (float)value;
			}

			// Token: 0x06004AC7 RID: 19143 RVA: 0x0026E7E0 File Offset: 0x0026CBE0
			public virtual void SetFirstChildObjectWidth(ControlMapper.LayoutElementSizeType type, int value)
			{
				if (this.rectTransform.childCount == 0)
				{
					return;
				}
				Transform child = this.rectTransform.GetChild(0);
				LayoutElement layoutElement = child.GetComponent<LayoutElement>();
				if (layoutElement == null)
				{
					layoutElement = child.gameObject.AddComponent<LayoutElement>();
				}
				if (type == ControlMapper.LayoutElementSizeType.MinSize)
				{
					layoutElement.minWidth = (float)value;
				}
				else
				{
					if (type != ControlMapper.LayoutElementSizeType.PreferredSize)
					{
						throw new NotImplementedException();
					}
					layoutElement.preferredWidth = (float)value;
				}
			}

			// Token: 0x06004AC8 RID: 19144 RVA: 0x0026E857 File Offset: 0x0026CC57
			public virtual void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x06004AC9 RID: 19145 RVA: 0x0026E877 File Offset: 0x0026CC77
			public virtual string GetLabel()
			{
				if (this.text == null)
				{
					return string.Empty;
				}
				return this.text.text;
			}

			// Token: 0x06004ACA RID: 19146 RVA: 0x0026E89B File Offset: 0x0026CC9B
			public virtual void AddChild(ControlMapper.GUIElement child)
			{
				this.children.Add(child);
			}

			// Token: 0x06004ACB RID: 19147 RVA: 0x0026E8A9 File Offset: 0x0026CCA9
			public void SetElementInfoData(string identifier, int intData)
			{
				if (this.uiElementInfo == null)
				{
					return;
				}
				this.uiElementInfo.identifier = identifier;
				this.uiElementInfo.intData = intData;
			}

			// Token: 0x06004ACC RID: 19148 RVA: 0x0026E8D5 File Offset: 0x0026CCD5
			public virtual void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x06004ACD RID: 19149 RVA: 0x0026E8F8 File Offset: 0x0026CCF8
			protected virtual bool Init()
			{
				bool result = true;
				for (int i = 0; i < this.children.Count; i++)
				{
					if (this.children[i] != null)
					{
						if (!this.children[i].Init())
						{
							result = false;
						}
					}
				}
				if (this.selectable == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: UI Element is missing Selectable component!");
					result = false;
				}
				if (this.rectTransform == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: UI Element is missing RectTransform component!");
					result = false;
				}
				if (this.uiElementInfo == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: UI Element is missing UIElementInfo component!");
					result = false;
				}
				return result;
			}

			// Token: 0x0400501F RID: 20511
			public readonly GameObject gameObject;

			// Token: 0x04005020 RID: 20512
			protected readonly Text text;

			// Token: 0x04005021 RID: 20513
			public readonly Selectable selectable;

			// Token: 0x04005022 RID: 20514
			protected readonly UIElementInfo uiElementInfo;

			// Token: 0x04005023 RID: 20515
			protected bool permanentStateSet;

			// Token: 0x04005024 RID: 20516
			protected readonly List<ControlMapper.GUIElement> children;
		}

		// Token: 0x02000C10 RID: 3088
		private class GUIButton : ControlMapper.GUIElement
		{
			// Token: 0x06004ACE RID: 19150 RVA: 0x0026E9A9 File Offset: 0x0026CDA9
			public GUIButton(GameObject gameObject) : base(gameObject)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x06004ACF RID: 19151 RVA: 0x0026E9BE File Offset: 0x0026CDBE
			public GUIButton(Button button, Text label) : base(button, label)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x170006D5 RID: 1749
			// (get) Token: 0x06004AD0 RID: 19152 RVA: 0x0026E9D4 File Offset: 0x0026CDD4
			protected Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x170006D6 RID: 1750
			// (get) Token: 0x06004AD1 RID: 19153 RVA: 0x0026E9E1 File Offset: 0x0026CDE1
			public ButtonInfo buttonInfo
			{
				get
				{
					return this.uiElementInfo as ButtonInfo;
				}
			}

			// Token: 0x06004AD2 RID: 19154 RVA: 0x0026E9EE File Offset: 0x0026CDEE
			public void SetButtonInfoData(string identifier, int intData)
			{
				base.SetElementInfoData(identifier, intData);
			}

			// Token: 0x06004AD3 RID: 19155 RVA: 0x0026E9F8 File Offset: 0x0026CDF8
			public void SetOnClickCallback(Action<ButtonInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate()
				{
					callback(this.buttonInfo);
				});
			}
		}

		// Token: 0x02000C11 RID: 3089
		private class GUIInputField : ControlMapper.GUIElement
		{
			// Token: 0x06004AD4 RID: 19156 RVA: 0x0026EA67 File Offset: 0x0026CE67
			public GUIInputField(GameObject gameObject) : base(gameObject)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x06004AD5 RID: 19157 RVA: 0x0026EA7C File Offset: 0x0026CE7C
			public GUIInputField(Button button, Text label) : base(button, label)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x170006D7 RID: 1751
			// (get) Token: 0x06004AD6 RID: 19158 RVA: 0x0026EA92 File Offset: 0x0026CE92
			protected Button button
			{
				get
				{
					return this.selectable as Button;
				}
			}

			// Token: 0x170006D8 RID: 1752
			// (get) Token: 0x06004AD7 RID: 19159 RVA: 0x0026EA9F File Offset: 0x0026CE9F
			public InputFieldInfo fieldInfo
			{
				get
				{
					return this.uiElementInfo as InputFieldInfo;
				}
			}

			// Token: 0x170006D9 RID: 1753
			// (get) Token: 0x06004AD8 RID: 19160 RVA: 0x0026EAAC File Offset: 0x0026CEAC
			public bool hasToggle
			{
				get
				{
					return this.toggle != null;
				}
			}

			// Token: 0x170006DA RID: 1754
			// (get) Token: 0x06004AD9 RID: 19161 RVA: 0x0026EABA File Offset: 0x0026CEBA
			// (set) Token: 0x06004ADA RID: 19162 RVA: 0x0026EAC2 File Offset: 0x0026CEC2
			public ControlMapper.GUIToggle toggle { get; private set; }

			// Token: 0x170006DB RID: 1755
			// (get) Token: 0x06004ADB RID: 19163 RVA: 0x0026EACB File Offset: 0x0026CECB
			// (set) Token: 0x06004ADC RID: 19164 RVA: 0x0026EAEB File Offset: 0x0026CEEB
			public int actionElementMapId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.actionElementMapId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.actionElementMapId = value;
				}
			}

			// Token: 0x170006DC RID: 1756
			// (get) Token: 0x06004ADD RID: 19165 RVA: 0x0026EB0B File Offset: 0x0026CF0B
			// (set) Token: 0x06004ADE RID: 19166 RVA: 0x0026EB2B File Offset: 0x0026CF2B
			public int controllerId
			{
				get
				{
					if (this.fieldInfo == null)
					{
						return -1;
					}
					return this.fieldInfo.controllerId;
				}
				set
				{
					if (this.fieldInfo == null)
					{
						return;
					}
					this.fieldInfo.controllerId = value;
				}
			}

			// Token: 0x06004ADF RID: 19167 RVA: 0x0026EB4C File Offset: 0x0026CF4C
			public void SetFieldInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.fieldInfo == null)
				{
					return;
				}
				this.fieldInfo.actionId = actionId;
				this.fieldInfo.axisRange = axisRange;
				this.fieldInfo.controllerType = controllerType;
			}

			// Token: 0x06004AE0 RID: 19168 RVA: 0x0026EB9C File Offset: 0x0026CF9C
			public void SetOnClickCallback(Action<InputFieldInfo> callback)
			{
				if (this.button == null)
				{
					return;
				}
				this.button.onClick.AddListener(delegate()
				{
					callback(this.fieldInfo);
				});
			}

			// Token: 0x06004AE1 RID: 19169 RVA: 0x0026EBEB File Offset: 0x0026CFEB
			public virtual void SetInteractable(bool state, bool playTransition, bool permanent)
			{
				if (this.permanentStateSet)
				{
					return;
				}
				if (this.hasToggle && !state)
				{
					this.toggle.SetInteractible(state, playTransition, permanent);
				}
				base.SetInteractible(state, playTransition, permanent);
			}

			// Token: 0x06004AE2 RID: 19170 RVA: 0x0026EC21 File Offset: 0x0026D021
			public void AddToggle(ControlMapper.GUIToggle toggle)
			{
				if (toggle == null)
				{
					return;
				}
				this.toggle = toggle;
			}
		}

		// Token: 0x02000C12 RID: 3090
		private class GUIToggle : ControlMapper.GUIElement
		{
			// Token: 0x06004AE3 RID: 19171 RVA: 0x0026EC51 File Offset: 0x0026D051
			public GUIToggle(GameObject gameObject) : base(gameObject)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x06004AE4 RID: 19172 RVA: 0x0026EC66 File Offset: 0x0026D066
			public GUIToggle(Toggle toggle, Text label) : base(toggle, label)
			{
				if (!this.Init())
				{
					return;
				}
			}

			// Token: 0x170006DD RID: 1757
			// (get) Token: 0x06004AE5 RID: 19173 RVA: 0x0026EC7C File Offset: 0x0026D07C
			protected Toggle toggle
			{
				get
				{
					return this.selectable as Toggle;
				}
			}

			// Token: 0x170006DE RID: 1758
			// (get) Token: 0x06004AE6 RID: 19174 RVA: 0x0026EC89 File Offset: 0x0026D089
			public ToggleInfo toggleInfo
			{
				get
				{
					return this.uiElementInfo as ToggleInfo;
				}
			}

			// Token: 0x170006DF RID: 1759
			// (get) Token: 0x06004AE7 RID: 19175 RVA: 0x0026EC96 File Offset: 0x0026D096
			// (set) Token: 0x06004AE8 RID: 19176 RVA: 0x0026ECB6 File Offset: 0x0026D0B6
			public int actionElementMapId
			{
				get
				{
					if (this.toggleInfo == null)
					{
						return -1;
					}
					return this.toggleInfo.actionElementMapId;
				}
				set
				{
					if (this.toggleInfo == null)
					{
						return;
					}
					this.toggleInfo.actionElementMapId = value;
				}
			}

			// Token: 0x06004AE9 RID: 19177 RVA: 0x0026ECD8 File Offset: 0x0026D0D8
			public void SetToggleInfoData(int actionId, AxisRange axisRange, ControllerType controllerType, int intData)
			{
				base.SetElementInfoData(string.Empty, intData);
				if (this.toggleInfo == null)
				{
					return;
				}
				this.toggleInfo.actionId = actionId;
				this.toggleInfo.axisRange = axisRange;
				this.toggleInfo.controllerType = controllerType;
			}

			// Token: 0x06004AEA RID: 19178 RVA: 0x0026ED28 File Offset: 0x0026D128
			public void SetOnSubmitCallback(Action<ToggleInfo, bool> callback)
			{
				if (this.toggle == null)
				{
					return;
				}
				EventTrigger eventTrigger = this.toggle.GetComponent<EventTrigger>();
				if (eventTrigger == null)
				{
					eventTrigger = this.toggle.gameObject.AddComponent<EventTrigger>();
				}
				EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
				triggerEvent.AddListener(delegate(BaseEventData data)
				{
					PointerEventData pointerEventData = data as PointerEventData;
					if (pointerEventData != null && pointerEventData.button != PointerEventData.InputButton.Left)
					{
						return;
					}
					callback(this.toggleInfo, this.toggle.isOn);
				});
				EventTrigger.Entry item = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = EventTriggerType.Submit
				};
				EventTrigger.Entry item2 = new EventTrigger.Entry
				{
					callback = triggerEvent,
					eventID = EventTriggerType.PointerClick
				};
				if (eventTrigger.triggers != null)
				{
					eventTrigger.triggers.Clear();
				}
				else
				{
					eventTrigger.triggers = new List<EventTrigger.Entry>();
				}
				eventTrigger.triggers.Add(item);
				eventTrigger.triggers.Add(item2);
			}

			// Token: 0x06004AEB RID: 19179 RVA: 0x0026EE11 File Offset: 0x0026D211
			public void SetToggleState(bool state)
			{
				if (this.toggle == null)
				{
					return;
				}
				this.toggle.isOn = state;
			}
		}

		// Token: 0x02000C13 RID: 3091
		private class GUILabel
		{
			// Token: 0x06004AEC RID: 19180 RVA: 0x0026EE88 File Offset: 0x0026D288
			public GUILabel(GameObject gameObject)
			{
				if (gameObject == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: gameObject is null!");
					return;
				}
				this.text = UnityTools.GetComponentInSelfOrChildren<Text>(gameObject);
				this.Check();
			}

			// Token: 0x06004AED RID: 19181 RVA: 0x0026EEBA File Offset: 0x0026D2BA
			public GUILabel(Text label)
			{
				this.text = label;
				if (!this.Check())
				{
					return;
				}
			}

			// Token: 0x170006E0 RID: 1760
			// (get) Token: 0x06004AEE RID: 19182 RVA: 0x0026EED5 File Offset: 0x0026D2D5
			// (set) Token: 0x06004AEF RID: 19183 RVA: 0x0026EEDD File Offset: 0x0026D2DD
			public GameObject gameObject { get; private set; }

			// Token: 0x170006E1 RID: 1761
			// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x0026EEE6 File Offset: 0x0026D2E6
			// (set) Token: 0x06004AF1 RID: 19185 RVA: 0x0026EEEE File Offset: 0x0026D2EE
			private Text text { get; set; }

			// Token: 0x170006E2 RID: 1762
			// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x0026EEF7 File Offset: 0x0026D2F7
			// (set) Token: 0x06004AF3 RID: 19187 RVA: 0x0026EEFF File Offset: 0x0026D2FF
			public RectTransform rectTransform { get; private set; }

			// Token: 0x06004AF4 RID: 19188 RVA: 0x0026EF08 File Offset: 0x0026D308
			public void SetSize(int width, int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)width);
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)height);
			}

			// Token: 0x06004AF5 RID: 19189 RVA: 0x0026EF38 File Offset: 0x0026D338
			public void SetWidth(int width)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)width);
			}

			// Token: 0x06004AF6 RID: 19190 RVA: 0x0026EF5A File Offset: 0x0026D35A
			public void SetHeight(int height)
			{
				if (this.text == null)
				{
					return;
				}
				this.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, (float)height);
			}

			// Token: 0x06004AF7 RID: 19191 RVA: 0x0026EF7C File Offset: 0x0026D37C
			public void SetLabel(string label)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.text = label;
			}

			// Token: 0x06004AF8 RID: 19192 RVA: 0x0026EF9C File Offset: 0x0026D39C
			public void SetFontStyle(FontStyle style)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.fontStyle = style;
			}

			// Token: 0x06004AF9 RID: 19193 RVA: 0x0026EFBC File Offset: 0x0026D3BC
			public void SetTextAlignment(TextAnchor alignment)
			{
				if (this.text == null)
				{
					return;
				}
				this.text.alignment = alignment;
			}

			// Token: 0x06004AFA RID: 19194 RVA: 0x0026EFDC File Offset: 0x0026D3DC
			public void SetActive(bool state)
			{
				if (this.gameObject == null)
				{
					return;
				}
				this.gameObject.SetActive(state);
			}

			// Token: 0x06004AFB RID: 19195 RVA: 0x0026EFFC File Offset: 0x0026D3FC
			private bool Check()
			{
				bool result = true;
				if (this.text == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: Button is missing Text child component!");
					result = false;
				}
				this.gameObject = this.text.gameObject;
				this.rectTransform = this.text.GetComponent<RectTransform>();
				return result;
			}
		}

		// Token: 0x02000C14 RID: 3092
		[Serializable]
		public class MappingSet
		{
			// Token: 0x06004AFC RID: 19196 RVA: 0x0026F04B File Offset: 0x0026D44B
			public MappingSet()
			{
				this._mapCategoryId = -1;
				this._actionCategoryIds = new int[0];
				this._actionIds = new int[0];
				this._actionListMode = ControlMapper.MappingSet.ActionListMode.ActionCategory;
			}

			// Token: 0x06004AFD RID: 19197 RVA: 0x0026F079 File Offset: 0x0026D479
			private MappingSet(int mapCategoryId, ControlMapper.MappingSet.ActionListMode actionListMode, int[] actionCategoryIds, int[] actionIds)
			{
				this._mapCategoryId = mapCategoryId;
				this._actionListMode = actionListMode;
				this._actionCategoryIds = actionCategoryIds;
				this._actionIds = actionIds;
			}

			// Token: 0x170006E3 RID: 1763
			// (get) Token: 0x06004AFE RID: 19198 RVA: 0x0026F09E File Offset: 0x0026D49E
			public int mapCategoryId
			{
				get
				{
					return this._mapCategoryId;
				}
			}

			// Token: 0x170006E4 RID: 1764
			// (get) Token: 0x06004AFF RID: 19199 RVA: 0x0026F0A6 File Offset: 0x0026D4A6
			public ControlMapper.MappingSet.ActionListMode actionListMode
			{
				get
				{
					return this._actionListMode;
				}
			}

			// Token: 0x170006E5 RID: 1765
			// (get) Token: 0x06004B00 RID: 19200 RVA: 0x0026F0AE File Offset: 0x0026D4AE
			public IList<int> actionCategoryIds
			{
				get
				{
					if (this._actionCategoryIds == null)
					{
						return null;
					}
					if (this._actionCategoryIdsReadOnly == null)
					{
						this._actionCategoryIdsReadOnly = new ReadOnlyCollection<int>(this._actionCategoryIds);
					}
					return this._actionCategoryIdsReadOnly;
				}
			}

			// Token: 0x170006E6 RID: 1766
			// (get) Token: 0x06004B01 RID: 19201 RVA: 0x0026F0DF File Offset: 0x0026D4DF
			public IList<int> actionIds
			{
				get
				{
					if (this._actionIds == null)
					{
						return null;
					}
					if (this._actionIdsReadOnly == null)
					{
						this._actionIdsReadOnly = new ReadOnlyCollection<int>(this._actionIds);
					}
					return this._actionIds;
				}
			}

			// Token: 0x170006E7 RID: 1767
			// (get) Token: 0x06004B02 RID: 19202 RVA: 0x0026F110 File Offset: 0x0026D510
			public bool isValid
			{
				get
				{
					return this._mapCategoryId >= 0 && ReInput.mapping.GetMapCategory(this._mapCategoryId) != null;
				}
			}

			// Token: 0x170006E8 RID: 1768
			// (get) Token: 0x06004B03 RID: 19203 RVA: 0x0026F136 File Offset: 0x0026D536
			public static ControlMapper.MappingSet Default
			{
				get
				{
					return new ControlMapper.MappingSet(0, ControlMapper.MappingSet.ActionListMode.ActionCategory, new int[1], new int[0]);
				}
			}

			// Token: 0x0400502A RID: 20522
			[SerializeField]
			[Tooltip("The Map Category that will be displayed to the user for remapping.")]
			private int _mapCategoryId;

			// Token: 0x0400502B RID: 20523
			[SerializeField]
			[Tooltip("Choose whether you want to list Actions to display for this Map Category by individual Action or by all the Actions in an Action Category.")]
			private ControlMapper.MappingSet.ActionListMode _actionListMode;

			// Token: 0x0400502C RID: 20524
			[SerializeField]
			private int[] _actionCategoryIds;

			// Token: 0x0400502D RID: 20525
			[SerializeField]
			private int[] _actionIds;

			// Token: 0x0400502E RID: 20526
			private IList<int> _actionCategoryIdsReadOnly;

			// Token: 0x0400502F RID: 20527
			private IList<int> _actionIdsReadOnly;

			// Token: 0x02000C15 RID: 3093
			public enum ActionListMode
			{
				// Token: 0x04005031 RID: 20529
				ActionCategory,
				// Token: 0x04005032 RID: 20530
				Action
			}
		}

		// Token: 0x02000C16 RID: 3094
		[Serializable]
		public class InputBehaviorSettings
		{
			// Token: 0x170006E9 RID: 1769
			// (get) Token: 0x06004B05 RID: 19205 RVA: 0x0026F1AB File Offset: 0x0026D5AB
			public int inputBehaviorId
			{
				get
				{
					return this._inputBehaviorId;
				}
			}

			// Token: 0x170006EA RID: 1770
			// (get) Token: 0x06004B06 RID: 19206 RVA: 0x0026F1B3 File Offset: 0x0026D5B3
			public bool showJoystickAxisSensitivity
			{
				get
				{
					return this._showJoystickAxisSensitivity;
				}
			}

			// Token: 0x170006EB RID: 1771
			// (get) Token: 0x06004B07 RID: 19207 RVA: 0x0026F1BB File Offset: 0x0026D5BB
			public bool showMouseXYAxisSensitivity
			{
				get
				{
					return this._showMouseXYAxisSensitivity;
				}
			}

			// Token: 0x170006EC RID: 1772
			// (get) Token: 0x06004B08 RID: 19208 RVA: 0x0026F1C3 File Offset: 0x0026D5C3
			public string labelLanguageKey
			{
				get
				{
					return this._labelLanguageKey;
				}
			}

			// Token: 0x170006ED RID: 1773
			// (get) Token: 0x06004B09 RID: 19209 RVA: 0x0026F1CB File Offset: 0x0026D5CB
			public string joystickAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._joystickAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x170006EE RID: 1774
			// (get) Token: 0x06004B0A RID: 19210 RVA: 0x0026F1D3 File Offset: 0x0026D5D3
			public string mouseXYAxisSensitivityLabelLanguageKey
			{
				get
				{
					return this._mouseXYAxisSensitivityLabelLanguageKey;
				}
			}

			// Token: 0x170006EF RID: 1775
			// (get) Token: 0x06004B0B RID: 19211 RVA: 0x0026F1DB File Offset: 0x0026D5DB
			public Sprite joystickAxisSensitivityIcon
			{
				get
				{
					return this._joystickAxisSensitivityIcon;
				}
			}

			// Token: 0x170006F0 RID: 1776
			// (get) Token: 0x06004B0C RID: 19212 RVA: 0x0026F1E3 File Offset: 0x0026D5E3
			public Sprite mouseXYAxisSensitivityIcon
			{
				get
				{
					return this._mouseXYAxisSensitivityIcon;
				}
			}

			// Token: 0x170006F1 RID: 1777
			// (get) Token: 0x06004B0D RID: 19213 RVA: 0x0026F1EB File Offset: 0x0026D5EB
			public float joystickAxisSensitivityMin
			{
				get
				{
					return this._joystickAxisSensitivityMin;
				}
			}

			// Token: 0x170006F2 RID: 1778
			// (get) Token: 0x06004B0E RID: 19214 RVA: 0x0026F1F3 File Offset: 0x0026D5F3
			public float joystickAxisSensitivityMax
			{
				get
				{
					return this._joystickAxisSensitivityMax;
				}
			}

			// Token: 0x170006F3 RID: 1779
			// (get) Token: 0x06004B0F RID: 19215 RVA: 0x0026F1FB File Offset: 0x0026D5FB
			public float mouseXYAxisSensitivityMin
			{
				get
				{
					return this._mouseXYAxisSensitivityMin;
				}
			}

			// Token: 0x170006F4 RID: 1780
			// (get) Token: 0x06004B10 RID: 19216 RVA: 0x0026F203 File Offset: 0x0026D603
			public float mouseXYAxisSensitivityMax
			{
				get
				{
					return this._mouseXYAxisSensitivityMax;
				}
			}

			// Token: 0x170006F5 RID: 1781
			// (get) Token: 0x06004B11 RID: 19217 RVA: 0x0026F20B File Offset: 0x0026D60B
			public bool isValid
			{
				get
				{
					return this._inputBehaviorId >= 0 && (this._showJoystickAxisSensitivity || this._showMouseXYAxisSensitivity);
				}
			}

			// Token: 0x04005033 RID: 20531
			[SerializeField]
			[Tooltip("The Input Behavior that will be displayed to the user for modification.")]
			private int _inputBehaviorId = -1;

			// Token: 0x04005034 RID: 20532
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			private bool _showJoystickAxisSensitivity = true;

			// Token: 0x04005035 RID: 20533
			[SerializeField]
			[Tooltip("If checked, a slider will be displayed so the user can change this value.")]
			private bool _showMouseXYAxisSensitivity = true;

			// Token: 0x04005036 RID: 20534
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed as the title for the Input Behavior control set. Otherwise, the name field of the InputBehavior will be used.")]
			private string _labelLanguageKey = string.Empty;

			// Token: 0x04005037 RID: 20535
			[SerializeField]
			[Tooltip("If set to a non-blank value, this name will be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			private string _joystickAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x04005038 RID: 20536
			[SerializeField]
			[Tooltip("If set to a non-blank value, this key will be used to look up the name in Language to be displayed above the individual slider control. Otherwise, no name will be displayed.")]
			private string _mouseXYAxisSensitivityLabelLanguageKey = string.Empty;

			// Token: 0x04005039 RID: 20537
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			private Sprite _joystickAxisSensitivityIcon;

			// Token: 0x0400503A RID: 20538
			[SerializeField]
			[Tooltip("The icon to display next to the slider. Set to none for no icon.")]
			private Sprite _mouseXYAxisSensitivityIcon;

			// Token: 0x0400503B RID: 20539
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			private float _joystickAxisSensitivityMin;

			// Token: 0x0400503C RID: 20540
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			private float _joystickAxisSensitivityMax = 2f;

			// Token: 0x0400503D RID: 20541
			[SerializeField]
			[Tooltip("Minimum value the user is allowed to set for this property.")]
			private float _mouseXYAxisSensitivityMin;

			// Token: 0x0400503E RID: 20542
			[SerializeField]
			[Tooltip("Maximum value the user is allowed to set for this property.")]
			private float _mouseXYAxisSensitivityMax = 2f;
		}

		// Token: 0x02000C17 RID: 3095
		[Serializable]
		private class Prefabs
		{
			// Token: 0x170006F6 RID: 1782
			// (get) Token: 0x06004B13 RID: 19219 RVA: 0x0026F238 File Offset: 0x0026D638
			public GameObject button
			{
				get
				{
					return this._button;
				}
			}

			// Token: 0x170006F7 RID: 1783
			// (get) Token: 0x06004B14 RID: 19220 RVA: 0x0026F240 File Offset: 0x0026D640
			public GameObject playerButton
			{
				get
				{
					return this._playerButton;
				}
			}

			// Token: 0x170006F8 RID: 1784
			// (get) Token: 0x06004B15 RID: 19221 RVA: 0x0026F248 File Offset: 0x0026D648
			public GameObject fitButton
			{
				get
				{
					return this._fitButton;
				}
			}

			// Token: 0x170006F9 RID: 1785
			// (get) Token: 0x06004B16 RID: 19222 RVA: 0x0026F250 File Offset: 0x0026D650
			public GameObject inputGridLabel
			{
				get
				{
					return this._inputGridLabel;
				}
			}

			// Token: 0x170006FA RID: 1786
			// (get) Token: 0x06004B17 RID: 19223 RVA: 0x0026F258 File Offset: 0x0026D658
			public GameObject inputGridDeactivatedLabel
			{
				get
				{
					return this._inputGridDeactivatedLabel;
				}
			}

			// Token: 0x170006FB RID: 1787
			// (get) Token: 0x06004B18 RID: 19224 RVA: 0x0026F260 File Offset: 0x0026D660
			public GameObject inputGridHeaderLabel
			{
				get
				{
					return this._inputGridHeaderLabel;
				}
			}

			// Token: 0x170006FC RID: 1788
			// (get) Token: 0x06004B19 RID: 19225 RVA: 0x0026F268 File Offset: 0x0026D668
			public GameObject actionsHeaderLabel
			{
				get
				{
					return this._actionsHeaderLabel;
				}
			}

			// Token: 0x170006FD RID: 1789
			// (get) Token: 0x06004B1A RID: 19226 RVA: 0x0026F270 File Offset: 0x0026D670
			public GameObject inputGridFieldButton
			{
				get
				{
					return this._inputGridFieldButton;
				}
			}

			// Token: 0x170006FE RID: 1790
			// (get) Token: 0x06004B1B RID: 19227 RVA: 0x0026F278 File Offset: 0x0026D678
			public GameObject inputGridFieldInvertToggle
			{
				get
				{
					return this._inputGridFieldInvertToggle;
				}
			}

			// Token: 0x170006FF RID: 1791
			// (get) Token: 0x06004B1C RID: 19228 RVA: 0x0026F280 File Offset: 0x0026D680
			public GameObject window
			{
				get
				{
					return this._window;
				}
			}

			// Token: 0x17000700 RID: 1792
			// (get) Token: 0x06004B1D RID: 19229 RVA: 0x0026F288 File Offset: 0x0026D688
			public GameObject windowTitleText
			{
				get
				{
					return this._windowTitleText;
				}
			}

			// Token: 0x17000701 RID: 1793
			// (get) Token: 0x06004B1E RID: 19230 RVA: 0x0026F290 File Offset: 0x0026D690
			public GameObject windowContentText
			{
				get
				{
					return this._windowContentText;
				}
			}

			// Token: 0x17000702 RID: 1794
			// (get) Token: 0x06004B1F RID: 19231 RVA: 0x0026F298 File Offset: 0x0026D698
			public GameObject fader
			{
				get
				{
					return this._fader;
				}
			}

			// Token: 0x17000703 RID: 1795
			// (get) Token: 0x06004B20 RID: 19232 RVA: 0x0026F2A0 File Offset: 0x0026D6A0
			public GameObject calibrationWindow
			{
				get
				{
					return this._calibrationWindow;
				}
			}

			// Token: 0x17000704 RID: 1796
			// (get) Token: 0x06004B21 RID: 19233 RVA: 0x0026F2A8 File Offset: 0x0026D6A8
			public GameObject inputBehaviorsWindow
			{
				get
				{
					return this._inputBehaviorsWindow;
				}
			}

			// Token: 0x17000705 RID: 1797
			// (get) Token: 0x06004B22 RID: 19234 RVA: 0x0026F2B0 File Offset: 0x0026D6B0
			public GameObject centerStickGraphic
			{
				get
				{
					return this._centerStickGraphic;
				}
			}

			// Token: 0x17000706 RID: 1798
			// (get) Token: 0x06004B23 RID: 19235 RVA: 0x0026F2B8 File Offset: 0x0026D6B8
			public GameObject moveStickGraphic
			{
				get
				{
					return this._moveStickGraphic;
				}
			}

			// Token: 0x06004B24 RID: 19236 RVA: 0x0026F2C0 File Offset: 0x0026D6C0
			public bool Check()
			{
				return !(this._button == null) && !(this._fitButton == null) && !(this._inputGridLabel == null) && !(this._inputGridHeaderLabel == null) && !(this._inputGridFieldButton == null) && !(this._inputGridFieldInvertToggle == null) && !(this._window == null) && !(this._windowTitleText == null) && !(this._windowContentText == null) && !(this._fader == null) && !(this._calibrationWindow == null) && !(this._inputBehaviorsWindow == null);
			}

			// Token: 0x0400503F RID: 20543
			[SerializeField]
			private GameObject _button;

			// Token: 0x04005040 RID: 20544
			[SerializeField]
			private GameObject _playerButton;

			// Token: 0x04005041 RID: 20545
			[SerializeField]
			private GameObject _fitButton;

			// Token: 0x04005042 RID: 20546
			[SerializeField]
			private GameObject _inputGridLabel;

			// Token: 0x04005043 RID: 20547
			[SerializeField]
			private GameObject _inputGridDeactivatedLabel;

			// Token: 0x04005044 RID: 20548
			[SerializeField]
			private GameObject _inputGridHeaderLabel;

			// Token: 0x04005045 RID: 20549
			[SerializeField]
			private GameObject _actionsHeaderLabel;

			// Token: 0x04005046 RID: 20550
			[SerializeField]
			private GameObject _inputGridFieldButton;

			// Token: 0x04005047 RID: 20551
			[SerializeField]
			private GameObject _inputGridFieldInvertToggle;

			// Token: 0x04005048 RID: 20552
			[SerializeField]
			private GameObject _window;

			// Token: 0x04005049 RID: 20553
			[SerializeField]
			private GameObject _windowTitleText;

			// Token: 0x0400504A RID: 20554
			[SerializeField]
			private GameObject _windowContentText;

			// Token: 0x0400504B RID: 20555
			[SerializeField]
			private GameObject _fader;

			// Token: 0x0400504C RID: 20556
			[SerializeField]
			private GameObject _calibrationWindow;

			// Token: 0x0400504D RID: 20557
			[SerializeField]
			private GameObject _inputBehaviorsWindow;

			// Token: 0x0400504E RID: 20558
			[SerializeField]
			private GameObject _centerStickGraphic;

			// Token: 0x0400504F RID: 20559
			[SerializeField]
			private GameObject _moveStickGraphic;
		}

		// Token: 0x02000C18 RID: 3096
		[Serializable]
		private class References
		{
			// Token: 0x17000707 RID: 1799
			// (get) Token: 0x06004B26 RID: 19238 RVA: 0x0026F3A4 File Offset: 0x0026D7A4
			public Canvas canvas
			{
				get
				{
					return this._canvas;
				}
			}

			// Token: 0x17000708 RID: 1800
			// (get) Token: 0x06004B27 RID: 19239 RVA: 0x0026F3AC File Offset: 0x0026D7AC
			public CanvasGroup mainCanvasGroup
			{
				get
				{
					return this._mainCanvasGroup;
				}
			}

			// Token: 0x17000709 RID: 1801
			// (get) Token: 0x06004B28 RID: 19240 RVA: 0x0026F3B4 File Offset: 0x0026D7B4
			public Transform mainContent
			{
				get
				{
					return this._mainContent;
				}
			}

			// Token: 0x1700070A RID: 1802
			// (get) Token: 0x06004B29 RID: 19241 RVA: 0x0026F3BC File Offset: 0x0026D7BC
			public Transform mainContentInner
			{
				get
				{
					return this._mainContentInner;
				}
			}

			// Token: 0x1700070B RID: 1803
			// (get) Token: 0x06004B2A RID: 19242 RVA: 0x0026F3C4 File Offset: 0x0026D7C4
			public UIGroup playersGroup
			{
				get
				{
					return this._playersGroup;
				}
			}

			// Token: 0x1700070C RID: 1804
			// (get) Token: 0x06004B2B RID: 19243 RVA: 0x0026F3CC File Offset: 0x0026D7CC
			public Transform controllerGroup
			{
				get
				{
					return this._controllerGroup;
				}
			}

			// Token: 0x1700070D RID: 1805
			// (get) Token: 0x06004B2C RID: 19244 RVA: 0x0026F3D4 File Offset: 0x0026D7D4
			public Transform controllerGroupLabelGroup
			{
				get
				{
					return this._controllerGroupLabelGroup;
				}
			}

			// Token: 0x1700070E RID: 1806
			// (get) Token: 0x06004B2D RID: 19245 RVA: 0x0026F3DC File Offset: 0x0026D7DC
			public UIGroup controllerSettingsGroup
			{
				get
				{
					return this._controllerSettingsGroup;
				}
			}

			// Token: 0x1700070F RID: 1807
			// (get) Token: 0x06004B2E RID: 19246 RVA: 0x0026F3E4 File Offset: 0x0026D7E4
			public UIGroup assignedControllersGroup
			{
				get
				{
					return this._assignedControllersGroup;
				}
			}

			// Token: 0x17000710 RID: 1808
			// (get) Token: 0x06004B2F RID: 19247 RVA: 0x0026F3EC File Offset: 0x0026D7EC
			public Transform settingsAndMapCategoriesGroup
			{
				get
				{
					return this._settingsAndMapCategoriesGroup;
				}
			}

			// Token: 0x17000711 RID: 1809
			// (get) Token: 0x06004B30 RID: 19248 RVA: 0x0026F3F4 File Offset: 0x0026D7F4
			public UIGroup settingsGroup
			{
				get
				{
					return this._settingsGroup;
				}
			}

			// Token: 0x17000712 RID: 1810
			// (get) Token: 0x06004B31 RID: 19249 RVA: 0x0026F3FC File Offset: 0x0026D7FC
			public UIGroup mapCategoriesGroup
			{
				get
				{
					return this._mapCategoriesGroup;
				}
			}

			// Token: 0x17000713 RID: 1811
			// (get) Token: 0x06004B32 RID: 19250 RVA: 0x0026F404 File Offset: 0x0026D804
			public Transform inputGridGroup
			{
				get
				{
					return this._inputGridGroup;
				}
			}

			// Token: 0x17000714 RID: 1812
			// (get) Token: 0x06004B33 RID: 19251 RVA: 0x0026F40C File Offset: 0x0026D80C
			public Transform inputGridContainer
			{
				get
				{
					return this._inputGridContainer;
				}
			}

			// Token: 0x17000715 RID: 1813
			// (get) Token: 0x06004B34 RID: 19252 RVA: 0x0026F414 File Offset: 0x0026D814
			public Transform inputGridHeadersGroup
			{
				get
				{
					return this._inputGridHeadersGroup;
				}
			}

			// Token: 0x17000716 RID: 1814
			// (get) Token: 0x06004B35 RID: 19253 RVA: 0x0026F41C File Offset: 0x0026D81C
			public Transform inputGridInnerGroup
			{
				get
				{
					return this._inputGridInnerGroup;
				}
			}

			// Token: 0x17000717 RID: 1815
			// (get) Token: 0x06004B36 RID: 19254 RVA: 0x0026F424 File Offset: 0x0026D824
			public Transform actionsColumnHeadersGroup
			{
				get
				{
					return this._actionsColumnHeadersGroup;
				}
			}

			// Token: 0x17000718 RID: 1816
			// (get) Token: 0x06004B37 RID: 19255 RVA: 0x0026F42C File Offset: 0x0026D82C
			public Transform actionsColumn
			{
				get
				{
					return this._actionsColumn;
				}
			}

			// Token: 0x17000719 RID: 1817
			// (get) Token: 0x06004B38 RID: 19256 RVA: 0x0026F434 File Offset: 0x0026D834
			public Text controllerNameLabel
			{
				get
				{
					return this._controllerNameLabel;
				}
			}

			// Token: 0x1700071A RID: 1818
			// (get) Token: 0x06004B39 RID: 19257 RVA: 0x0026F43C File Offset: 0x0026D83C
			public Button removeControllerButton
			{
				get
				{
					return this._removeControllerButton;
				}
			}

			// Token: 0x1700071B RID: 1819
			// (get) Token: 0x06004B3A RID: 19258 RVA: 0x0026F444 File Offset: 0x0026D844
			public Button assignControllerButton
			{
				get
				{
					return this._assignControllerButton;
				}
			}

			// Token: 0x1700071C RID: 1820
			// (get) Token: 0x06004B3B RID: 19259 RVA: 0x0026F44C File Offset: 0x0026D84C
			public Button calibrateControllerButton
			{
				get
				{
					return this._calibrateControllerButton;
				}
			}

			// Token: 0x1700071D RID: 1821
			// (get) Token: 0x06004B3C RID: 19260 RVA: 0x0026F454 File Offset: 0x0026D854
			public Button doneButton
			{
				get
				{
					return this._doneButton;
				}
			}

			// Token: 0x1700071E RID: 1822
			// (get) Token: 0x06004B3D RID: 19261 RVA: 0x0026F45C File Offset: 0x0026D85C
			public Button restoreDefaultsButton
			{
				get
				{
					return this._restoreDefaultsButton;
				}
			}

			// Token: 0x1700071F RID: 1823
			// (get) Token: 0x06004B3E RID: 19262 RVA: 0x0026F464 File Offset: 0x0026D864
			public Selectable defaultSelection
			{
				get
				{
					return this._defaultSelection;
				}
			}

			// Token: 0x17000720 RID: 1824
			// (get) Token: 0x06004B3F RID: 19263 RVA: 0x0026F46C File Offset: 0x0026D86C
			public GameObject[] fixedSelectableUIElements
			{
				get
				{
					return this._fixedSelectableUIElements;
				}
			}

			// Token: 0x17000721 RID: 1825
			// (get) Token: 0x06004B40 RID: 19264 RVA: 0x0026F474 File Offset: 0x0026D874
			public Image mainBackgroundImage
			{
				get
				{
					return this._mainBackgroundImage;
				}
			}

			// Token: 0x17000722 RID: 1826
			// (get) Token: 0x06004B41 RID: 19265 RVA: 0x0026F47C File Offset: 0x0026D87C
			// (set) Token: 0x06004B42 RID: 19266 RVA: 0x0026F484 File Offset: 0x0026D884
			public LayoutElement inputGridLayoutElement { get; set; }

			// Token: 0x17000723 RID: 1827
			// (get) Token: 0x06004B43 RID: 19267 RVA: 0x0026F48D File Offset: 0x0026D88D
			// (set) Token: 0x06004B44 RID: 19268 RVA: 0x0026F495 File Offset: 0x0026D895
			public Transform inputGridActionColumn { get; set; }

			// Token: 0x17000724 RID: 1828
			// (get) Token: 0x06004B45 RID: 19269 RVA: 0x0026F49E File Offset: 0x0026D89E
			// (set) Token: 0x06004B46 RID: 19270 RVA: 0x0026F4A6 File Offset: 0x0026D8A6
			public Transform inputGridKeyboardColumn { get; set; }

			// Token: 0x17000725 RID: 1829
			// (get) Token: 0x06004B47 RID: 19271 RVA: 0x0026F4AF File Offset: 0x0026D8AF
			// (set) Token: 0x06004B48 RID: 19272 RVA: 0x0026F4B7 File Offset: 0x0026D8B7
			public Transform inputGridMouseColumn { get; set; }

			// Token: 0x17000726 RID: 1830
			// (get) Token: 0x06004B49 RID: 19273 RVA: 0x0026F4C0 File Offset: 0x0026D8C0
			// (set) Token: 0x06004B4A RID: 19274 RVA: 0x0026F4C8 File Offset: 0x0026D8C8
			public Transform inputGridControllerColumn { get; set; }

			// Token: 0x17000727 RID: 1831
			// (get) Token: 0x06004B4B RID: 19275 RVA: 0x0026F4D1 File Offset: 0x0026D8D1
			// (set) Token: 0x06004B4C RID: 19276 RVA: 0x0026F4D9 File Offset: 0x0026D8D9
			public Transform inputGridHeader1 { get; set; }

			// Token: 0x17000728 RID: 1832
			// (get) Token: 0x06004B4D RID: 19277 RVA: 0x0026F4E2 File Offset: 0x0026D8E2
			// (set) Token: 0x06004B4E RID: 19278 RVA: 0x0026F4EA File Offset: 0x0026D8EA
			public Transform inputGridHeader2 { get; set; }

			// Token: 0x17000729 RID: 1833
			// (get) Token: 0x06004B4F RID: 19279 RVA: 0x0026F4F3 File Offset: 0x0026D8F3
			// (set) Token: 0x06004B50 RID: 19280 RVA: 0x0026F4FB File Offset: 0x0026D8FB
			public Transform inputGridHeader3 { get; set; }

			// Token: 0x1700072A RID: 1834
			// (get) Token: 0x06004B51 RID: 19281 RVA: 0x0026F504 File Offset: 0x0026D904
			// (set) Token: 0x06004B52 RID: 19282 RVA: 0x0026F50C File Offset: 0x0026D90C
			public Transform inputGridHeader4 { get; set; }

			// Token: 0x06004B53 RID: 19283 RVA: 0x0026F518 File Offset: 0x0026D918
			public bool Check()
			{
				return !(this._canvas == null) && !(this._mainCanvasGroup == null) && !(this._mainContent == null) && !(this._mainContentInner == null) && !(this._playersGroup == null) && !(this._controllerGroup == null) && !(this._controllerGroupLabelGroup == null) && !(this._controllerSettingsGroup == null) && !(this._assignedControllersGroup == null) && !(this._settingsAndMapCategoriesGroup == null) && !(this._settingsGroup == null) && !(this._mapCategoriesGroup == null) && !(this._inputGridGroup == null) && !(this._inputGridContainer == null) && !(this._inputGridHeadersGroup == null) && !(this._inputGridInnerGroup == null) && !(this._controllerNameLabel == null) && !(this._removeControllerButton == null) && !(this._assignControllerButton == null) && !(this._calibrateControllerButton == null) && !(this._doneButton == null) && !(this._restoreDefaultsButton == null) && !(this._defaultSelection == null);
			}

			// Token: 0x04005050 RID: 20560
			[SerializeField]
			private Canvas _canvas;

			// Token: 0x04005051 RID: 20561
			[SerializeField]
			private CanvasGroup _mainCanvasGroup;

			// Token: 0x04005052 RID: 20562
			[SerializeField]
			private Transform _mainContent;

			// Token: 0x04005053 RID: 20563
			[SerializeField]
			private Transform _mainContentInner;

			// Token: 0x04005054 RID: 20564
			[SerializeField]
			private UIGroup _playersGroup;

			// Token: 0x04005055 RID: 20565
			[SerializeField]
			private Transform _controllerGroup;

			// Token: 0x04005056 RID: 20566
			[SerializeField]
			private Transform _controllerGroupLabelGroup;

			// Token: 0x04005057 RID: 20567
			[SerializeField]
			private UIGroup _controllerSettingsGroup;

			// Token: 0x04005058 RID: 20568
			[SerializeField]
			private UIGroup _assignedControllersGroup;

			// Token: 0x04005059 RID: 20569
			[SerializeField]
			private Transform _settingsAndMapCategoriesGroup;

			// Token: 0x0400505A RID: 20570
			[SerializeField]
			private UIGroup _settingsGroup;

			// Token: 0x0400505B RID: 20571
			[SerializeField]
			private UIGroup _mapCategoriesGroup;

			// Token: 0x0400505C RID: 20572
			[SerializeField]
			private Transform _inputGridGroup;

			// Token: 0x0400505D RID: 20573
			[SerializeField]
			private Transform _inputGridContainer;

			// Token: 0x0400505E RID: 20574
			[SerializeField]
			private Transform _inputGridHeadersGroup;

			// Token: 0x0400505F RID: 20575
			[SerializeField]
			private Transform _inputGridInnerGroup;

			// Token: 0x04005060 RID: 20576
			[SerializeField]
			private Transform _actionsColumnHeadersGroup;

			// Token: 0x04005061 RID: 20577
			[SerializeField]
			private Transform _actionsColumn;

			// Token: 0x04005062 RID: 20578
			[SerializeField]
			private Text _controllerNameLabel;

			// Token: 0x04005063 RID: 20579
			[SerializeField]
			private Button _removeControllerButton;

			// Token: 0x04005064 RID: 20580
			[SerializeField]
			private Button _assignControllerButton;

			// Token: 0x04005065 RID: 20581
			[SerializeField]
			private Button _calibrateControllerButton;

			// Token: 0x04005066 RID: 20582
			[SerializeField]
			private Button _doneButton;

			// Token: 0x04005067 RID: 20583
			[SerializeField]
			private Button _restoreDefaultsButton;

			// Token: 0x04005068 RID: 20584
			[SerializeField]
			private Selectable _defaultSelection;

			// Token: 0x04005069 RID: 20585
			[SerializeField]
			private GameObject[] _fixedSelectableUIElements;

			// Token: 0x0400506A RID: 20586
			[SerializeField]
			private Image _mainBackgroundImage;
		}

		// Token: 0x02000C19 RID: 3097
		private class InputActionSet
		{
			// Token: 0x06004B54 RID: 19284 RVA: 0x0026F6AF File Offset: 0x0026DAAF
			public InputActionSet(int actionId, AxisRange axisRange)
			{
				this._actionId = actionId;
				this._axisRange = axisRange;
			}

			// Token: 0x1700072B RID: 1835
			// (get) Token: 0x06004B55 RID: 19285 RVA: 0x0026F6C5 File Offset: 0x0026DAC5
			public int actionId
			{
				get
				{
					return this._actionId;
				}
			}

			// Token: 0x1700072C RID: 1836
			// (get) Token: 0x06004B56 RID: 19286 RVA: 0x0026F6CD File Offset: 0x0026DACD
			public AxisRange axisRange
			{
				get
				{
					return this._axisRange;
				}
			}

			// Token: 0x04005074 RID: 20596
			private int _actionId;

			// Token: 0x04005075 RID: 20597
			private AxisRange _axisRange;
		}

		// Token: 0x02000C1A RID: 3098
		private class InputMapping
		{
			// Token: 0x06004B57 RID: 19287 RVA: 0x0026F6D5 File Offset: 0x0026DAD5
			public InputMapping(string actionName, InputFieldInfo fieldInfo, ControllerMap map, ActionElementMap aem, ControllerType controllerType, int controllerId)
			{
				this.actionName = actionName;
				this.fieldInfo = fieldInfo;
				this.map = map;
				this.aem = aem;
				this.controllerType = controllerType;
				this.controllerId = controllerId;
			}

			// Token: 0x1700072D RID: 1837
			// (get) Token: 0x06004B58 RID: 19288 RVA: 0x0026F70A File Offset: 0x0026DB0A
			// (set) Token: 0x06004B59 RID: 19289 RVA: 0x0026F712 File Offset: 0x0026DB12
			public string actionName { get; private set; }

			// Token: 0x1700072E RID: 1838
			// (get) Token: 0x06004B5A RID: 19290 RVA: 0x0026F71B File Offset: 0x0026DB1B
			// (set) Token: 0x06004B5B RID: 19291 RVA: 0x0026F723 File Offset: 0x0026DB23
			public InputFieldInfo fieldInfo { get; private set; }

			// Token: 0x1700072F RID: 1839
			// (get) Token: 0x06004B5C RID: 19292 RVA: 0x0026F72C File Offset: 0x0026DB2C
			// (set) Token: 0x06004B5D RID: 19293 RVA: 0x0026F734 File Offset: 0x0026DB34
			public ControllerMap map { get; private set; }

			// Token: 0x17000730 RID: 1840
			// (get) Token: 0x06004B5E RID: 19294 RVA: 0x0026F73D File Offset: 0x0026DB3D
			// (set) Token: 0x06004B5F RID: 19295 RVA: 0x0026F745 File Offset: 0x0026DB45
			public ActionElementMap aem { get; private set; }

			// Token: 0x17000731 RID: 1841
			// (get) Token: 0x06004B60 RID: 19296 RVA: 0x0026F74E File Offset: 0x0026DB4E
			// (set) Token: 0x06004B61 RID: 19297 RVA: 0x0026F756 File Offset: 0x0026DB56
			public ControllerType controllerType { get; private set; }

			// Token: 0x17000732 RID: 1842
			// (get) Token: 0x06004B62 RID: 19298 RVA: 0x0026F75F File Offset: 0x0026DB5F
			// (set) Token: 0x06004B63 RID: 19299 RVA: 0x0026F767 File Offset: 0x0026DB67
			public int controllerId { get; private set; }

			// Token: 0x17000733 RID: 1843
			// (get) Token: 0x06004B64 RID: 19300 RVA: 0x0026F770 File Offset: 0x0026DB70
			// (set) Token: 0x06004B65 RID: 19301 RVA: 0x0026F778 File Offset: 0x0026DB78
			public ControllerPollingInfo pollingInfo { get; set; }

			// Token: 0x17000734 RID: 1844
			// (get) Token: 0x06004B66 RID: 19302 RVA: 0x0026F781 File Offset: 0x0026DB81
			// (set) Token: 0x06004B67 RID: 19303 RVA: 0x0026F789 File Offset: 0x0026DB89
			public ModifierKeyFlags modifierKeyFlags { get; set; }

			// Token: 0x17000735 RID: 1845
			// (get) Token: 0x06004B68 RID: 19304 RVA: 0x0026F794 File Offset: 0x0026DB94
			public AxisRange axisRange
			{
				get
				{
					AxisRange result = AxisRange.Positive;
					if (this.pollingInfo.elementType == ControllerElementType.Axis)
					{
						if (this.fieldInfo.axisRange == AxisRange.Full)
						{
							result = AxisRange.Full;
						}
						else
						{
							result = ((this.pollingInfo.axisPole != Pole.Positive) ? AxisRange.Negative : AxisRange.Positive);
						}
					}
					return result;
				}
			}

			// Token: 0x17000736 RID: 1846
			// (get) Token: 0x06004B69 RID: 19305 RVA: 0x0026F7EC File Offset: 0x0026DBEC
			public string elementName
			{
				get
				{
					if (this.controllerType == ControllerType.Keyboard && this.modifierKeyFlags != ModifierKeyFlags.None)
					{
						return string.Format("{0} + {1}", Keyboard.ModifierKeyFlagsToString(this.modifierKeyFlags), this.pollingInfo.elementIdentifierName);
					}
					string text = this.pollingInfo.elementIdentifierName;
					if (this.pollingInfo.elementType == ControllerElementType.Axis)
					{
						if (this.axisRange == AxisRange.Positive)
						{
							text = this.pollingInfo.elementIdentifier.positiveName;
						}
						else if (this.axisRange == AxisRange.Negative)
						{
							text = this.pollingInfo.elementIdentifier.negativeName;
						}
					}
					TranslationElement translationElement = Localization.Find(text);
					if (translationElement != null)
					{
						return translationElement.translations[(int)Localization.language].text;
					}
					return text;
				}
			}

			// Token: 0x06004B6A RID: 19306 RVA: 0x0026F8C3 File Offset: 0x0026DCC3
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo)
			{
				this.pollingInfo = pollingInfo;
				return this.ToElementAssignment();
			}

			// Token: 0x06004B6B RID: 19307 RVA: 0x0026F8D2 File Offset: 0x0026DCD2
			public ElementAssignment ToElementAssignment(ControllerPollingInfo pollingInfo, ModifierKeyFlags modifierKeyFlags)
			{
				this.pollingInfo = pollingInfo;
				this.modifierKeyFlags = modifierKeyFlags;
				return this.ToElementAssignment();
			}

			// Token: 0x06004B6C RID: 19308 RVA: 0x0026F8E8 File Offset: 0x0026DCE8
			public ElementAssignment ToElementAssignment()
			{
				return new ElementAssignment(this.controllerType, this.pollingInfo.elementType, this.pollingInfo.elementIdentifierId, this.axisRange, this.pollingInfo.keyboardKey, this.modifierKeyFlags, this.fieldInfo.actionId, (this.fieldInfo.axisRange != AxisRange.Negative) ? Pole.Positive : Pole.Negative, false, (this.aem == null) ? -1 : this.aem.id);
			}
		}

		// Token: 0x02000C1B RID: 3099
		private class AxisCalibrator
		{
			// Token: 0x06004B6D RID: 19309 RVA: 0x0026F978 File Offset: 0x0026DD78
			public AxisCalibrator(Joystick joystick, int axisIndex)
			{
				this.data = default(AxisCalibrationData);
				this.joystick = joystick;
				this.axisIndex = axisIndex;
				if (joystick != null && axisIndex >= 0 && joystick.axisCount > axisIndex)
				{
					this.axis = joystick.Axes[axisIndex];
					this.data = joystick.calibrationMap.GetAxis(axisIndex).GetData();
				}
				this.firstRun = true;
			}

			// Token: 0x17000737 RID: 1847
			// (get) Token: 0x06004B6E RID: 19310 RVA: 0x0026F9F1 File Offset: 0x0026DDF1
			public bool isValid
			{
				get
				{
					return this.axis != null;
				}
			}

			// Token: 0x06004B6F RID: 19311 RVA: 0x0026FA00 File Offset: 0x0026DE00
			public void RecordMinMax()
			{
				if (this.axis == null)
				{
					return;
				}
				float valueRaw = this.axis.valueRaw;
				if (this.firstRun || valueRaw < this.data.min)
				{
					this.data.min = valueRaw;
				}
				if (this.firstRun || valueRaw > this.data.max)
				{
					this.data.max = valueRaw;
				}
				this.firstRun = false;
			}

			// Token: 0x06004B70 RID: 19312 RVA: 0x0026FA7C File Offset: 0x0026DE7C
			public void RecordZero()
			{
				if (this.axis == null)
				{
					return;
				}
				this.data.zero = this.axis.valueRaw;
			}

			// Token: 0x06004B71 RID: 19313 RVA: 0x0026FAA0 File Offset: 0x0026DEA0
			public void Commit()
			{
				if (this.axis == null)
				{
					return;
				}
				AxisCalibration axisCalibration = this.joystick.calibrationMap.GetAxis(this.axisIndex);
				if (axisCalibration == null)
				{
					return;
				}
				if ((double)Mathf.Abs(this.data.max - this.data.min) < 0.1)
				{
					return;
				}
				axisCalibration.SetData(this.data);
			}

			// Token: 0x0400507E RID: 20606
			public AxisCalibrationData data;

			// Token: 0x0400507F RID: 20607
			public readonly Joystick joystick;

			// Token: 0x04005080 RID: 20608
			public readonly int axisIndex;

			// Token: 0x04005081 RID: 20609
			private Controller.Axis axis;

			// Token: 0x04005082 RID: 20610
			private bool firstRun;
		}

		// Token: 0x02000C1C RID: 3100
		private class IndexedDictionary<TKey, TValue>
		{
			// Token: 0x06004B72 RID: 19314 RVA: 0x0026FB0F File Offset: 0x0026DF0F
			public IndexedDictionary()
			{
				this.list = new List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry>();
			}

			// Token: 0x17000738 RID: 1848
			// (get) Token: 0x06004B73 RID: 19315 RVA: 0x0026FB22 File Offset: 0x0026DF22
			public int Count
			{
				get
				{
					return this.list.Count;
				}
			}

			// Token: 0x17000739 RID: 1849
			public TValue this[int index]
			{
				get
				{
					return this.list[index].value;
				}
			}

			// Token: 0x06004B75 RID: 19317 RVA: 0x0026FB44 File Offset: 0x0026DF44
			public TValue Get(TKey key)
			{
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					throw new Exception("Key does not exist!");
				}
				return this.list[num].value;
			}

			// Token: 0x06004B76 RID: 19318 RVA: 0x0026FB7C File Offset: 0x0026DF7C
			public bool TryGet(TKey key, out TValue value)
			{
				value = default(TValue);
				int num = this.IndexOfKey(key);
				if (num < 0)
				{
					return false;
				}
				value = this.list[num].value;
				return true;
			}

			// Token: 0x06004B77 RID: 19319 RVA: 0x0026FBC4 File Offset: 0x0026DFC4
			public void Add(TKey key, TValue value)
			{
				if (this.ContainsKey(key))
				{
					throw new Exception("Key " + key.ToString() + " is already in use!");
				}
				this.list.Add(new ControlMapper.IndexedDictionary<TKey, TValue>.Entry(key, value));
			}

			// Token: 0x06004B78 RID: 19320 RVA: 0x0026FC14 File Offset: 0x0026E014
			public int IndexOfKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return i;
					}
				}
				return -1;
			}

			// Token: 0x06004B79 RID: 19321 RVA: 0x0026FC64 File Offset: 0x0026E064
			public bool ContainsKey(TKey key)
			{
				int count = this.list.Count;
				for (int i = 0; i < count; i++)
				{
					if (EqualityComparer<TKey>.Default.Equals(this.list[i].key, key))
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x06004B7A RID: 19322 RVA: 0x0026FCB3 File Offset: 0x0026E0B3
			public void Clear()
			{
				this.list.Clear();
			}

			// Token: 0x04005083 RID: 20611
			private List<ControlMapper.IndexedDictionary<TKey, TValue>.Entry> list;

			// Token: 0x02000C1D RID: 3101
			private class Entry
			{
				// Token: 0x06004B7B RID: 19323 RVA: 0x0026FCC0 File Offset: 0x0026E0C0
				public Entry(TKey key, TValue value)
				{
					this.key = key;
					this.value = value;
				}

				// Token: 0x04005084 RID: 20612
				public TKey key;

				// Token: 0x04005085 RID: 20613
				public TValue value;
			}
		}

		// Token: 0x02000C1E RID: 3102
		private enum LayoutElementSizeType
		{
			// Token: 0x04005087 RID: 20615
			MinSize,
			// Token: 0x04005088 RID: 20616
			PreferredSize
		}

		// Token: 0x02000C1F RID: 3103
		private enum WindowType
		{
			// Token: 0x0400508A RID: 20618
			None,
			// Token: 0x0400508B RID: 20619
			ChooseJoystick,
			// Token: 0x0400508C RID: 20620
			JoystickAssignmentConflict,
			// Token: 0x0400508D RID: 20621
			ElementAssignment,
			// Token: 0x0400508E RID: 20622
			ElementAssignmentPrePolling,
			// Token: 0x0400508F RID: 20623
			ElementAssignmentPolling,
			// Token: 0x04005090 RID: 20624
			ElementAssignmentResult,
			// Token: 0x04005091 RID: 20625
			ElementAssignmentConflict,
			// Token: 0x04005092 RID: 20626
			Calibration,
			// Token: 0x04005093 RID: 20627
			CalibrateStep1,
			// Token: 0x04005094 RID: 20628
			CalibrateStep2
		}

		// Token: 0x02000C20 RID: 3104
		private class InputGrid
		{
			// Token: 0x06004B7C RID: 19324 RVA: 0x0026FCD6 File Offset: 0x0026E0D6
			public InputGrid()
			{
				this.list = new ControlMapper.InputGridEntryList();
				this.groups = new List<GameObject>();
			}

			// Token: 0x06004B7D RID: 19325 RVA: 0x0026FCF4 File Offset: 0x0026E0F4
			public void AddMapCategory(int mapCategoryId)
			{
				this.list.AddMapCategory(mapCategoryId);
			}

			// Token: 0x06004B7E RID: 19326 RVA: 0x0026FD02 File Offset: 0x0026E102
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.list.AddAction(mapCategoryId, action, axisRange);
			}

			// Token: 0x06004B7F RID: 19327 RVA: 0x0026FD12 File Offset: 0x0026E112
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.list.AddActionCategory(mapCategoryId, actionCategoryId);
			}

			// Token: 0x06004B80 RID: 19328 RVA: 0x0026FD21 File Offset: 0x0026E121
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				this.list.AddInputFieldSet(mapCategoryId, action, axisRange, controllerType, fieldSetContainer);
			}

			// Token: 0x06004B81 RID: 19329 RVA: 0x0026FD35 File Offset: 0x0026E135
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				this.list.AddInputField(mapCategoryId, action, axisRange, controllerType, fieldIndex, inputField);
			}

			// Token: 0x06004B82 RID: 19330 RVA: 0x0026FD4B File Offset: 0x0026E14B
			public void AddGroup(GameObject group)
			{
				this.groups.Add(group);
			}

			// Token: 0x06004B83 RID: 19331 RVA: 0x0026FD59 File Offset: 0x0026E159
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				this.list.AddActionLabel(mapCategoryId, actionId, axisRange, label);
			}

			// Token: 0x06004B84 RID: 19332 RVA: 0x0026FD6B File Offset: 0x0026E16B
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				this.list.AddActionCategoryLabel(mapCategoryId, actionCategoryId, label);
			}

			// Token: 0x06004B85 RID: 19333 RVA: 0x0026FD7B File Offset: 0x0026E17B
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.Contains(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x06004B86 RID: 19334 RVA: 0x0026FD8F File Offset: 0x0026E18F
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				return this.list.GetGUIInputField(mapCategoryId, actionId, axisRange, controllerType, fieldIndex);
			}

			// Token: 0x06004B87 RID: 19335 RVA: 0x0026FDA3 File Offset: 0x0026E1A3
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				return this.list.GetActionSets(mapCategoryId);
			}

			// Token: 0x06004B88 RID: 19336 RVA: 0x0026FDB1 File Offset: 0x0026E1B1
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				this.list.SetColumnHeight(mapCategoryId, height);
			}

			// Token: 0x06004B89 RID: 19337 RVA: 0x0026FDC0 File Offset: 0x0026E1C0
			public float GetColumnHeight(int mapCategoryId)
			{
				return this.list.GetColumnHeight(mapCategoryId);
			}

			// Token: 0x06004B8A RID: 19338 RVA: 0x0026FDCE File Offset: 0x0026E1CE
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				this.list.SetFieldsActive(mapCategoryId, state);
			}

			// Token: 0x06004B8B RID: 19339 RVA: 0x0026FDDD File Offset: 0x0026E1DD
			public void SetFieldLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				this.list.SetLabel(mapCategoryId, actionId, axisRange, controllerType, index, label);
			}

			// Token: 0x06004B8C RID: 19340 RVA: 0x0026FDF4 File Offset: 0x0026E1F4
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				this.list.PopulateField(mapCategoryId, actionId, axisRange, controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x06004B8D RID: 19341 RVA: 0x0026FE1B File Offset: 0x0026E21B
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				this.list.SetFixedFieldData(mapCategoryId, actionId, axisRange, controllerType, controllerId);
			}

			// Token: 0x06004B8E RID: 19342 RVA: 0x0026FE2F File Offset: 0x0026E22F
			public void InitializeFields(int mapCategoryId)
			{
				this.list.InitializeFields(mapCategoryId);
			}

			// Token: 0x06004B8F RID: 19343 RVA: 0x0026FE3D File Offset: 0x0026E23D
			public void Show(int mapCategoryId)
			{
				this.list.Show(mapCategoryId);
			}

			// Token: 0x06004B90 RID: 19344 RVA: 0x0026FE4B File Offset: 0x0026E24B
			public void HideAll()
			{
				this.list.HideAll();
			}

			// Token: 0x06004B91 RID: 19345 RVA: 0x0026FE58 File Offset: 0x0026E258
			public void ClearLabels(int mapCategoryId)
			{
				this.list.ClearLabels(mapCategoryId);
			}

			// Token: 0x06004B92 RID: 19346 RVA: 0x0026FE68 File Offset: 0x0026E268
			private void ClearGroups()
			{
				for (int i = 0; i < this.groups.Count; i++)
				{
					if (!(this.groups[i] == null))
					{
						UnityEngine.Object.Destroy(this.groups[i]);
					}
				}
			}

			// Token: 0x06004B93 RID: 19347 RVA: 0x0026FEBE File Offset: 0x0026E2BE
			public void ClearAll()
			{
				this.ClearGroups();
				this.list.Clear();
			}

			// Token: 0x04005095 RID: 20629
			private ControlMapper.InputGridEntryList list;

			// Token: 0x04005096 RID: 20630
			private List<GameObject> groups;
		}

		// Token: 0x02000C21 RID: 3105
		private class InputGridEntryList
		{
			// Token: 0x06004B94 RID: 19348 RVA: 0x0026FED1 File Offset: 0x0026E2D1
			public InputGridEntryList()
			{
				this.entries = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry>();
			}

			// Token: 0x06004B95 RID: 19349 RVA: 0x0026FEE4 File Offset: 0x0026E2E4
			public void AddMapCategory(int mapCategoryId)
			{
				if (mapCategoryId < 0)
				{
					return;
				}
				if (this.entries.ContainsKey(mapCategoryId))
				{
					return;
				}
				this.entries.Add(mapCategoryId, new ControlMapper.InputGridEntryList.MapCategoryEntry());
			}

			// Token: 0x06004B96 RID: 19350 RVA: 0x0026FF11 File Offset: 0x0026E311
			public void AddAction(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				this.AddActionEntry(mapCategoryId, action, axisRange);
			}

			// Token: 0x06004B97 RID: 19351 RVA: 0x0026FF20 File Offset: 0x0026E320
			private ControlMapper.InputGridEntryList.ActionEntry AddActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddAction(action, axisRange);
			}

			// Token: 0x06004B98 RID: 19352 RVA: 0x0026FF54 File Offset: 0x0026E354
			public void AddActionLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = mapCategoryEntry.GetActionEntry(actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetLabel(label);
			}

			// Token: 0x06004B99 RID: 19353 RVA: 0x0026FF8D File Offset: 0x0026E38D
			public void AddActionCategory(int mapCategoryId, int actionCategoryId)
			{
				this.AddActionCategoryEntry(mapCategoryId, actionCategoryId);
			}

			// Token: 0x06004B9A RID: 19354 RVA: 0x0026FF98 File Offset: 0x0026E398
			private ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategoryEntry(int mapCategoryId, int actionCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.AddActionCategory(actionCategoryId);
			}

			// Token: 0x06004B9B RID: 19355 RVA: 0x0026FFC4 File Offset: 0x0026E3C4
			public void AddActionCategoryLabel(int mapCategoryId, int actionCategoryId, ControlMapper.GUILabel label)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				ControlMapper.InputGridEntryList.ActionCategoryEntry actionCategoryEntry = mapCategoryEntry.GetActionCategoryEntry(actionCategoryId);
				if (actionCategoryEntry == null)
				{
					return;
				}
				actionCategoryEntry.SetLabel(label);
			}

			// Token: 0x06004B9C RID: 19356 RVA: 0x0026FFFC File Offset: 0x0026E3FC
			public void AddInputFieldSet(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, GameObject fieldSetContainer)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputFieldSet(controllerType, fieldSetContainer);
			}

			// Token: 0x06004B9D RID: 19357 RVA: 0x00270024 File Offset: 0x0026E424
			public void AddInputField(int mapCategoryId, InputAction action, AxisRange axisRange, ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, action, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.AddInputField(controllerType, fieldIndex, inputField);
			}

			// Token: 0x06004B9E RID: 19358 RVA: 0x0027004E File Offset: 0x0026E44E
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				return this.GetActionEntry(mapCategoryId, actionId, axisRange) != null;
			}

			// Token: 0x06004B9F RID: 19359 RVA: 0x00270060 File Offset: 0x0026E460
			public bool Contains(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				return actionEntry != null && actionEntry.Contains(controllerType, fieldIndex);
			}

			// Token: 0x06004BA0 RID: 19360 RVA: 0x0027008C File Offset: 0x0026E48C
			public void SetColumnHeight(int mapCategoryId, float height)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.columnHeight = height;
			}

			// Token: 0x06004BA1 RID: 19361 RVA: 0x002700B4 File Offset: 0x0026E4B4
			public float GetColumnHeight(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return 0f;
				}
				return mapCategoryEntry.columnHeight;
			}

			// Token: 0x06004BA2 RID: 19362 RVA: 0x002700E0 File Offset: 0x0026E4E0
			public ControlMapper.GUIInputField GetGUIInputField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int fieldIndex)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return null;
				}
				return actionEntry.GetGUIInputField(controllerType, fieldIndex);
			}

			// Token: 0x06004BA3 RID: 19363 RVA: 0x0027010C File Offset: 0x0026E50C
			private ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, int actionId, AxisRange axisRange)
			{
				if (actionId < 0)
				{
					return null;
				}
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return null;
				}
				return mapCategoryEntry.GetActionEntry(actionId, axisRange);
			}

			// Token: 0x06004BA4 RID: 19364 RVA: 0x00270141 File Offset: 0x0026E541
			private ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int mapCategoryId, InputAction action, AxisRange axisRange)
			{
				if (action == null)
				{
					return null;
				}
				return this.GetActionEntry(mapCategoryId, action.id, axisRange);
			}

			// Token: 0x06004BA5 RID: 19365 RVA: 0x0027015C File Offset: 0x0026E55C
			public IEnumerable<ControlMapper.InputActionSet> GetActionSets(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry entry;
				if (!this.entries.TryGet(mapCategoryId, out entry))
				{
					yield break;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> list = entry.actionList;
				int count = (list == null) ? 0 : list.Count;
				for (int i = 0; i < count; i++)
				{
					yield return list[i].actionSet;
				}
				yield break;
			}

			// Token: 0x06004BA6 RID: 19366 RVA: 0x00270188 File Offset: 0x0026E588
			public void SetFieldsActive(int mapCategoryId, bool state)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList == null) ? 0 : actionList.Count;
				for (int i = 0; i < num; i++)
				{
					actionList[i].SetFieldsActive(state);
				}
			}

			// Token: 0x06004BA7 RID: 19367 RVA: 0x002701E4 File Offset: 0x0026E5E4
			public void SetLabel(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int index, string label)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFieldLabel(controllerType, index, label);
			}

			// Token: 0x06004BA8 RID: 19368 RVA: 0x00270210 File Offset: 0x0026E610
			public void PopulateField(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.PopulateField(controllerType, controllerId, index, actionElementMapId, label, invert);
			}

			// Token: 0x06004BA9 RID: 19369 RVA: 0x00270240 File Offset: 0x0026E640
			public void SetFixedFieldData(int mapCategoryId, int actionId, AxisRange axisRange, ControllerType controllerType, int controllerId)
			{
				ControlMapper.InputGridEntryList.ActionEntry actionEntry = this.GetActionEntry(mapCategoryId, actionId, axisRange);
				if (actionEntry == null)
				{
					return;
				}
				actionEntry.SetFixedFieldData(controllerType, controllerId);
			}

			// Token: 0x06004BAA RID: 19370 RVA: 0x00270268 File Offset: 0x0026E668
			public void InitializeFields(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList == null) ? 0 : actionList.Count;
				for (int i = 0; i < num; i++)
				{
					actionList[i].Initialize();
				}
			}

			// Token: 0x06004BAB RID: 19371 RVA: 0x002702C4 File Offset: 0x0026E6C4
			public void Show(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				mapCategoryEntry.SetAllActive(true);
			}

			// Token: 0x06004BAC RID: 19372 RVA: 0x002702EC File Offset: 0x0026E6EC
			public void HideAll()
			{
				for (int i = 0; i < this.entries.Count; i++)
				{
					this.entries[i].SetAllActive(false);
				}
			}

			// Token: 0x06004BAD RID: 19373 RVA: 0x00270328 File Offset: 0x0026E728
			public void ClearLabels(int mapCategoryId)
			{
				ControlMapper.InputGridEntryList.MapCategoryEntry mapCategoryEntry;
				if (!this.entries.TryGet(mapCategoryId, out mapCategoryEntry))
				{
					return;
				}
				List<ControlMapper.InputGridEntryList.ActionEntry> actionList = mapCategoryEntry.actionList;
				int num = (actionList == null) ? 0 : actionList.Count;
				for (int i = 0; i < num; i++)
				{
					actionList[i].ClearLabels();
				}
			}

			// Token: 0x06004BAE RID: 19374 RVA: 0x00270381 File Offset: 0x0026E781
			public void Clear()
			{
				this.entries.Clear();
			}

			// Token: 0x04005097 RID: 20631
			private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.MapCategoryEntry> entries;

			// Token: 0x02000C22 RID: 3106
			private class MapCategoryEntry
			{
				// Token: 0x06004BAF RID: 19375 RVA: 0x0027038E File Offset: 0x0026E78E
				public MapCategoryEntry()
				{
					this._actionList = new List<ControlMapper.InputGridEntryList.ActionEntry>();
					this._actionCategoryList = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry>();
				}

				// Token: 0x1700073A RID: 1850
				// (get) Token: 0x06004BB0 RID: 19376 RVA: 0x002703AC File Offset: 0x0026E7AC
				public List<ControlMapper.InputGridEntryList.ActionEntry> actionList
				{
					get
					{
						return this._actionList;
					}
				}

				// Token: 0x1700073B RID: 1851
				// (get) Token: 0x06004BB1 RID: 19377 RVA: 0x002703B4 File Offset: 0x0026E7B4
				public ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> actionCategoryList
				{
					get
					{
						return this._actionCategoryList;
					}
				}

				// Token: 0x1700073C RID: 1852
				// (get) Token: 0x06004BB2 RID: 19378 RVA: 0x002703BC File Offset: 0x0026E7BC
				// (set) Token: 0x06004BB3 RID: 19379 RVA: 0x002703C4 File Offset: 0x0026E7C4
				public float columnHeight
				{
					get
					{
						return this._columnHeight;
					}
					set
					{
						this._columnHeight = value;
					}
				}

				// Token: 0x06004BB4 RID: 19380 RVA: 0x002703D0 File Offset: 0x0026E7D0
				public ControlMapper.InputGridEntryList.ActionEntry GetActionEntry(int actionId, AxisRange axisRange)
				{
					int num = this.IndexOfActionEntry(actionId, axisRange);
					if (num < 0)
					{
						return null;
					}
					return this._actionList[num];
				}

				// Token: 0x06004BB5 RID: 19381 RVA: 0x002703FC File Offset: 0x0026E7FC
				public int IndexOfActionEntry(int actionId, AxisRange axisRange)
				{
					int count = this._actionList.Count;
					for (int i = 0; i < count; i++)
					{
						if (this._actionList[i].Matches(actionId, axisRange))
						{
							return i;
						}
					}
					return -1;
				}

				// Token: 0x06004BB6 RID: 19382 RVA: 0x00270442 File Offset: 0x0026E842
				public bool ContainsActionEntry(int actionId, AxisRange axisRange)
				{
					return this.IndexOfActionEntry(actionId, axisRange) >= 0;
				}

				// Token: 0x06004BB7 RID: 19383 RVA: 0x00270454 File Offset: 0x0026E854
				public ControlMapper.InputGridEntryList.ActionEntry AddAction(InputAction action, AxisRange axisRange)
				{
					if (action == null)
					{
						return null;
					}
					if (this.ContainsActionEntry(action.id, axisRange))
					{
						return null;
					}
					this._actionList.Add(new ControlMapper.InputGridEntryList.ActionEntry(action, axisRange));
					return this._actionList[this._actionList.Count - 1];
				}

				// Token: 0x06004BB8 RID: 19384 RVA: 0x002704A7 File Offset: 0x0026E8A7
				public ControlMapper.InputGridEntryList.ActionCategoryEntry GetActionCategoryEntry(int actionCategoryId)
				{
					if (!this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x06004BB9 RID: 19385 RVA: 0x002704C8 File Offset: 0x0026E8C8
				public ControlMapper.InputGridEntryList.ActionCategoryEntry AddActionCategory(int actionCategoryId)
				{
					if (actionCategoryId < 0)
					{
						return null;
					}
					if (this._actionCategoryList.ContainsKey(actionCategoryId))
					{
						return null;
					}
					this._actionCategoryList.Add(actionCategoryId, new ControlMapper.InputGridEntryList.ActionCategoryEntry(actionCategoryId));
					return this._actionCategoryList.Get(actionCategoryId);
				}

				// Token: 0x06004BBA RID: 19386 RVA: 0x00270504 File Offset: 0x0026E904
				public void SetAllActive(bool state)
				{
					for (int i = 0; i < this._actionCategoryList.Count; i++)
					{
						this._actionCategoryList[i].SetActive(state);
					}
					for (int j = 0; j < this._actionList.Count; j++)
					{
						this._actionList[j].SetActive(state);
					}
				}

				// Token: 0x04005098 RID: 20632
				private List<ControlMapper.InputGridEntryList.ActionEntry> _actionList;

				// Token: 0x04005099 RID: 20633
				private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.ActionCategoryEntry> _actionCategoryList;

				// Token: 0x0400509A RID: 20634
				private float _columnHeight;
			}

			// Token: 0x02000C23 RID: 3107
			private class ActionEntry
			{
				// Token: 0x06004BBB RID: 19387 RVA: 0x0027056D File Offset: 0x0026E96D
				public ActionEntry(InputAction action, AxisRange axisRange)
				{
					this.action = action;
					this.axisRange = axisRange;
					this.actionSet = new ControlMapper.InputActionSet(action.id, axisRange);
					this.fieldSets = new ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet>();
				}

				// Token: 0x06004BBC RID: 19388 RVA: 0x002705A0 File Offset: 0x0026E9A0
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x06004BBD RID: 19389 RVA: 0x002705A9 File Offset: 0x0026E9A9
				public bool Matches(int actionId, AxisRange axisRange)
				{
					return this.action.id == actionId && this.axisRange == axisRange;
				}

				// Token: 0x06004BBE RID: 19390 RVA: 0x002705CD File Offset: 0x0026E9CD
				public void AddInputFieldSet(ControllerType controllerType, GameObject fieldSetContainer)
				{
					if (this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					this.fieldSets.Add((int)controllerType, new ControlMapper.InputGridEntryList.FieldSet(fieldSetContainer));
				}

				// Token: 0x06004BBF RID: 19391 RVA: 0x002705F4 File Offset: 0x0026E9F4
				public void AddInputField(ControllerType controllerType, int fieldIndex, ControlMapper.GUIInputField inputField)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int)controllerType);
					if (fieldSet.fields.ContainsKey(fieldIndex))
					{
						return;
					}
					fieldSet.fields.Add(fieldIndex, inputField);
				}

				// Token: 0x06004BC0 RID: 19392 RVA: 0x00270640 File Offset: 0x0026EA40
				public ControlMapper.GUIInputField GetGUIInputField(ControllerType controllerType, int fieldIndex)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return null;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(fieldIndex))
					{
						return null;
					}
					return this.fieldSets.Get((int)controllerType).fields.Get(fieldIndex);
				}

				// Token: 0x06004BC1 RID: 19393 RVA: 0x00270695 File Offset: 0x0026EA95
				public bool Contains(ControllerType controllerType, int fieldId)
				{
					return this.fieldSets.ContainsKey((int)controllerType) && this.fieldSets.Get((int)controllerType).fields.ContainsKey(fieldId);
				}

				// Token: 0x06004BC2 RID: 19394 RVA: 0x002706CC File Offset: 0x0026EACC
				public void SetFieldLabel(ControllerType controllerType, int index, string label)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(index))
					{
						return;
					}
					this.fieldSets.Get((int)controllerType).fields.Get(index).SetLabel(label);
				}

				// Token: 0x06004BC3 RID: 19395 RVA: 0x00270728 File Offset: 0x0026EB28
				public void PopulateField(ControllerType controllerType, int controllerId, int index, int actionElementMapId, string label, bool invert)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					if (!this.fieldSets.Get((int)controllerType).fields.ContainsKey(index))
					{
						return;
					}
					ControlMapper.GUIInputField guiinputField = this.fieldSets.Get((int)controllerType).fields.Get(index);
					guiinputField.SetLabel(label);
					guiinputField.actionElementMapId = actionElementMapId;
					guiinputField.controllerId = controllerId;
					if (guiinputField.hasToggle)
					{
						guiinputField.toggle.SetInteractible(true, false);
						guiinputField.toggle.SetToggleState(invert);
						guiinputField.toggle.actionElementMapId = actionElementMapId;
					}
				}

				// Token: 0x06004BC4 RID: 19396 RVA: 0x002707C8 File Offset: 0x0026EBC8
				public void SetFixedFieldData(ControllerType controllerType, int controllerId)
				{
					if (!this.fieldSets.ContainsKey((int)controllerType))
					{
						return;
					}
					ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets.Get((int)controllerType);
					int count = fieldSet.fields.Count;
					for (int i = 0; i < count; i++)
					{
						fieldSet.fields[i].controllerId = controllerId;
					}
				}

				// Token: 0x06004BC5 RID: 19397 RVA: 0x00270824 File Offset: 0x0026EC24
				public void Initialize()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							if (guiinputField.hasToggle)
							{
								guiinputField.toggle.SetInteractible(false, false);
								guiinputField.toggle.SetToggleState(false);
								guiinputField.toggle.actionElementMapId = -1;
							}
							guiinputField.SetLabel(string.Empty);
							guiinputField.actionElementMapId = -1;
							guiinputField.controllerId = -1;
						}
					}
				}

				// Token: 0x06004BC6 RID: 19398 RVA: 0x002708D8 File Offset: 0x0026ECD8
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
					int count = this.fieldSets.Count;
					for (int i = 0; i < count; i++)
					{
						this.fieldSets[i].groupContainer.SetActive(state);
					}
				}

				// Token: 0x06004BC7 RID: 19399 RVA: 0x00270934 File Offset: 0x0026ED34
				public void ClearLabels()
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							guiinputField.SetLabel(string.Empty);
						}
					}
				}

				// Token: 0x06004BC8 RID: 19400 RVA: 0x002709A4 File Offset: 0x0026EDA4
				public void SetFieldsActive(bool state)
				{
					for (int i = 0; i < this.fieldSets.Count; i++)
					{
						ControlMapper.InputGridEntryList.FieldSet fieldSet = this.fieldSets[i];
						int count = fieldSet.fields.Count;
						for (int j = 0; j < count; j++)
						{
							ControlMapper.GUIInputField guiinputField = fieldSet.fields[j];
							guiinputField.SetInteractible(state, false);
							if (guiinputField.hasToggle)
							{
								guiinputField.toggle.SetInteractible(state, false);
							}
						}
					}
				}

				// Token: 0x0400509B RID: 20635
				private ControlMapper.IndexedDictionary<int, ControlMapper.InputGridEntryList.FieldSet> fieldSets;

				// Token: 0x0400509C RID: 20636
				public ControlMapper.GUILabel label;

				// Token: 0x0400509D RID: 20637
				public readonly InputAction action;

				// Token: 0x0400509E RID: 20638
				public readonly AxisRange axisRange;

				// Token: 0x0400509F RID: 20639
				public readonly ControlMapper.InputActionSet actionSet;
			}

			// Token: 0x02000C24 RID: 3108
			private class FieldSet
			{
				// Token: 0x06004BC9 RID: 19401 RVA: 0x00270A29 File Offset: 0x0026EE29
				public FieldSet(GameObject groupContainer)
				{
					this.groupContainer = groupContainer;
					this.fields = new ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField>();
				}

				// Token: 0x040050A0 RID: 20640
				public readonly GameObject groupContainer;

				// Token: 0x040050A1 RID: 20641
				public readonly ControlMapper.IndexedDictionary<int, ControlMapper.GUIInputField> fields;
			}

			// Token: 0x02000C25 RID: 3109
			private class ActionCategoryEntry
			{
				// Token: 0x06004BCA RID: 19402 RVA: 0x00270A43 File Offset: 0x0026EE43
				public ActionCategoryEntry(int actionCategoryId)
				{
					this.actionCategoryId = actionCategoryId;
				}

				// Token: 0x06004BCB RID: 19403 RVA: 0x00270A52 File Offset: 0x0026EE52
				public void SetLabel(ControlMapper.GUILabel label)
				{
					this.label = label;
				}

				// Token: 0x06004BCC RID: 19404 RVA: 0x00270A5B File Offset: 0x0026EE5B
				public void SetActive(bool state)
				{
					if (this.label != null)
					{
						this.label.SetActive(state);
					}
				}

				// Token: 0x040050A2 RID: 20642
				public readonly int actionCategoryId;

				// Token: 0x040050A3 RID: 20643
				public ControlMapper.GUILabel label;
			}
		}

		// Token: 0x02000C26 RID: 3110
		private class WindowManager
		{
			// Token: 0x06004BCD RID: 19405 RVA: 0x00270BD8 File Offset: 0x0026EFD8
			public WindowManager(GameObject windowPrefab, GameObject faderPrefab, Transform parent)
			{
				this.windowPrefab = windowPrefab;
				this.parent = parent;
				this.windows = new List<Window>();
				this.fader = UnityEngine.Object.Instantiate<GameObject>(faderPrefab);
				this.fader.transform.SetParent(parent, false);
				this.fader.GetComponent<RectTransform>().localScale = Vector2.one;
				this.SetFaderActive(false);
			}

			// Token: 0x1700073D RID: 1853
			// (get) Token: 0x06004BCE RID: 19406 RVA: 0x00270C44 File Offset: 0x0026F044
			public bool isWindowOpen
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return true;
						}
					}
					return false;
				}
			}

			// Token: 0x1700073E RID: 1854
			// (get) Token: 0x06004BCF RID: 19407 RVA: 0x00270C90 File Offset: 0x0026F090
			public Window topWindow
			{
				get
				{
					for (int i = this.windows.Count - 1; i >= 0; i--)
					{
						if (!(this.windows[i] == null))
						{
							return this.windows[i];
						}
					}
					return null;
				}
			}

			// Token: 0x06004BD0 RID: 19408 RVA: 0x00270CE8 File Offset: 0x0026F0E8
			public Window OpenWindow(string name, int width, int height)
			{
				Window result = this.InstantiateWindow(name, width, height);
				this.UpdateFader();
				return result;
			}

			// Token: 0x06004BD1 RID: 19409 RVA: 0x00270D08 File Offset: 0x0026F108
			public Window OpenWindow(GameObject windowPrefab, string name)
			{
				if (windowPrefab == null)
				{
					UnityEngine.Debug.LogError("Rewired Control Mapper: Window Prefab is null!");
					return null;
				}
				Window result = this.InstantiateWindow(name, windowPrefab);
				this.UpdateFader();
				return result;
			}

			// Token: 0x06004BD2 RID: 19410 RVA: 0x00270D40 File Offset: 0x0026F140
			public void CloseTop()
			{
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
					this.windows.RemoveAt(i);
				}
				this.UpdateFader();
			}

			// Token: 0x06004BD3 RID: 19411 RVA: 0x00270DBC File Offset: 0x0026F1BC
			public void CloseWindow(int windowId)
			{
				this.CloseWindow(this.GetWindow(windowId));
			}

			// Token: 0x06004BD4 RID: 19412 RVA: 0x00270DCC File Offset: 0x0026F1CC
			public void CloseWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else if (!(this.windows[i] != window))
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
						break;
					}
				}
				this.UpdateFader();
				this.FocusTopWindow();
			}

			// Token: 0x06004BD5 RID: 19413 RVA: 0x00270E78 File Offset: 0x0026F278
			public void CloseAll()
			{
				this.SetFaderActive(false);
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (this.windows[i] == null)
					{
						this.windows.RemoveAt(i);
					}
					else
					{
						this.DestroyWindow(this.windows[i]);
						this.windows.RemoveAt(i);
					}
				}
				this.UpdateFader();
			}

			// Token: 0x06004BD6 RID: 19414 RVA: 0x00270EF8 File Offset: 0x0026F2F8
			public void CancelAll()
			{
				if (!this.isWindowOpen)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						this.windows[i].Cancel();
					}
				}
				this.CloseAll();
			}

			// Token: 0x06004BD7 RID: 19415 RVA: 0x00270F64 File Offset: 0x0026F364
			public Window GetWindow(int windowId)
			{
				if (windowId < 0)
				{
					return null;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						if (this.windows[i].id == windowId)
						{
							return this.windows[i];
						}
					}
				}
				return null;
			}

			// Token: 0x06004BD8 RID: 19416 RVA: 0x00270FDE File Offset: 0x0026F3DE
			public bool IsFocused(int windowId)
			{
				return windowId >= 0 && !(this.topWindow == null) && this.topWindow.id == windowId;
			}

			// Token: 0x06004BD9 RID: 19417 RVA: 0x0027100A File Offset: 0x0026F40A
			public void Focus(int windowId)
			{
				this.Focus(this.GetWindow(windowId));
			}

			// Token: 0x06004BDA RID: 19418 RVA: 0x00271019 File Offset: 0x0026F419
			public void Focus(Window window)
			{
				if (window == null)
				{
					return;
				}
				window.TakeInputFocus();
				this.DefocusOtherWindows(window.id);
			}

			// Token: 0x06004BDB RID: 19419 RVA: 0x0027103C File Offset: 0x0026F43C
			private void DefocusOtherWindows(int focusedWindowId)
			{
				if (focusedWindowId < 0)
				{
					return;
				}
				for (int i = this.windows.Count - 1; i >= 0; i--)
				{
					if (!(this.windows[i] == null))
					{
						if (this.windows[i].id != focusedWindowId)
						{
							this.windows[i].Disable();
						}
					}
				}
			}

			// Token: 0x06004BDC RID: 19420 RVA: 0x002710B8 File Offset: 0x0026F4B8
			private void UpdateFader()
			{
				if (!this.isWindowOpen)
				{
					this.SetFaderActive(false);
					return;
				}
				Transform x = this.topWindow.transform.parent;
				if (x == null)
				{
					return;
				}
				this.SetFaderActive(true);
				this.fader.transform.SetAsLastSibling();
				int siblingIndex = this.topWindow.transform.GetSiblingIndex();
				this.fader.transform.SetSiblingIndex(siblingIndex);
			}

			// Token: 0x06004BDD RID: 19421 RVA: 0x0027112F File Offset: 0x0026F52F
			private void FocusTopWindow()
			{
				if (this.topWindow == null)
				{
					return;
				}
				this.topWindow.TakeInputFocus();
			}

			// Token: 0x06004BDE RID: 19422 RVA: 0x0027114E File Offset: 0x0026F54E
			private void SetFaderActive(bool state)
			{
				this.fader.SetActive(state);
			}

			// Token: 0x06004BDF RID: 19423 RVA: 0x0027115C File Offset: 0x0026F55C
			private Window InstantiateWindow(string name, int width, int height)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(this.windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
					component.SetSize(width, height);
				}
				return component;
			}

			// Token: 0x06004BE0 RID: 19424 RVA: 0x002711DC File Offset: 0x0026F5DC
			private Window InstantiateWindow(string name, GameObject windowPrefab)
			{
				if (string.IsNullOrEmpty(name))
				{
					name = "Window";
				}
				if (windowPrefab == null)
				{
					return null;
				}
				GameObject gameObject = UITools.InstantiateGUIObject<Window>(windowPrefab, this.parent, name);
				if (gameObject == null)
				{
					return null;
				}
				Window component = gameObject.GetComponent<Window>();
				if (component != null)
				{
					component.Initialize(this.GetNewId(), new Func<int, bool>(this.IsFocused));
					this.windows.Add(component);
				}
				return component;
			}

			// Token: 0x06004BE1 RID: 19425 RVA: 0x0027125D File Offset: 0x0026F65D
			private void DestroyWindow(Window window)
			{
				if (window == null)
				{
					return;
				}
				UnityEngine.Object.Destroy(window.gameObject);
			}

			// Token: 0x06004BE2 RID: 19426 RVA: 0x00271278 File Offset: 0x0026F678
			private int GetNewId()
			{
				int result = this.idCounter;
				this.idCounter++;
				return result;
			}

			// Token: 0x06004BE3 RID: 19427 RVA: 0x0027129B File Offset: 0x0026F69B
			public void ClearCompletely()
			{
				this.CloseAll();
				if (this.fader != null)
				{
					UnityEngine.Object.Destroy(this.fader);
				}
			}

			// Token: 0x040050A4 RID: 20644
			private List<Window> windows;

			// Token: 0x040050A5 RID: 20645
			private GameObject windowPrefab;

			// Token: 0x040050A6 RID: 20646
			private Transform parent;

			// Token: 0x040050A7 RID: 20647
			private GameObject fader;

			// Token: 0x040050A8 RID: 20648
			private int idCounter;
		}
	}
}
