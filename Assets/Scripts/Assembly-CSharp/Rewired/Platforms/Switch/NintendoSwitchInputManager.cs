using System;
using System.Collections.Generic;
using Rewired.Data;
using Rewired.Utils.Interfaces;
using UnityEngine;

namespace Rewired.Platforms.Switch
{
	// Token: 0x02000C5B RID: 3163
	[AddComponentMenu("Rewired/Nintendo Switch Input Manager")]
	[RequireComponent(typeof(InputManager))]
	public sealed class NintendoSwitchInputManager : MonoBehaviour, IExternalInputManager
	{
		// Token: 0x06004E95 RID: 20117 RVA: 0x0027A2A7 File Offset: 0x002786A7
		object IExternalInputManager.Initialize(Platform platform, ConfigVars configVars)
		{
			return null;
		}

		// Token: 0x06004E96 RID: 20118 RVA: 0x0027A2AA File Offset: 0x002786AA
		void IExternalInputManager.Deinitialize()
		{
		}

		// Token: 0x040051E2 RID: 20962
		[SerializeField]
		private NintendoSwitchInputManager.UserData _userData = new NintendoSwitchInputManager.UserData();

		// Token: 0x02000C5C RID: 3164
		[Serializable]
		private class UserData : IKeyedData<int>
		{
			// Token: 0x170007F1 RID: 2033
			// (get) Token: 0x06004E98 RID: 20120 RVA: 0x0027A353 File Offset: 0x00278753
			// (set) Token: 0x06004E99 RID: 20121 RVA: 0x0027A35B File Offset: 0x0027875B
			public int allowedNpadStyles
			{
				get
				{
					return this._allowedNpadStyles;
				}
				set
				{
					this._allowedNpadStyles = value;
				}
			}

			// Token: 0x170007F2 RID: 2034
			// (get) Token: 0x06004E9A RID: 20122 RVA: 0x0027A364 File Offset: 0x00278764
			// (set) Token: 0x06004E9B RID: 20123 RVA: 0x0027A36C File Offset: 0x0027876C
			public int joyConGripStyle
			{
				get
				{
					return this._joyConGripStyle;
				}
				set
				{
					this._joyConGripStyle = value;
				}
			}

			// Token: 0x170007F3 RID: 2035
			// (get) Token: 0x06004E9C RID: 20124 RVA: 0x0027A375 File Offset: 0x00278775
			// (set) Token: 0x06004E9D RID: 20125 RVA: 0x0027A37D File Offset: 0x0027877D
			public bool adjustIMUsForGripStyle
			{
				get
				{
					return this._adjustIMUsForGripStyle;
				}
				set
				{
					this._adjustIMUsForGripStyle = value;
				}
			}

			// Token: 0x170007F4 RID: 2036
			// (get) Token: 0x06004E9E RID: 20126 RVA: 0x0027A386 File Offset: 0x00278786
			// (set) Token: 0x06004E9F RID: 20127 RVA: 0x0027A38E File Offset: 0x0027878E
			public int handheldActivationMode
			{
				get
				{
					return this._handheldActivationMode;
				}
				set
				{
					this._handheldActivationMode = value;
				}
			}

			// Token: 0x170007F5 RID: 2037
			// (get) Token: 0x06004EA0 RID: 20128 RVA: 0x0027A397 File Offset: 0x00278797
			// (set) Token: 0x06004EA1 RID: 20129 RVA: 0x0027A39F File Offset: 0x0027879F
			public bool assignJoysticksByNpadId
			{
				get
				{
					return this._assignJoysticksByNpadId;
				}
				set
				{
					this._assignJoysticksByNpadId = value;
				}
			}

			// Token: 0x170007F6 RID: 2038
			// (get) Token: 0x06004EA2 RID: 20130 RVA: 0x0027A3A8 File Offset: 0x002787A8
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo1
			{
				get
				{
					return this._npadNo1;
				}
			}

			// Token: 0x170007F7 RID: 2039
			// (get) Token: 0x06004EA3 RID: 20131 RVA: 0x0027A3B0 File Offset: 0x002787B0
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo2
			{
				get
				{
					return this._npadNo2;
				}
			}

			// Token: 0x170007F8 RID: 2040
			// (get) Token: 0x06004EA4 RID: 20132 RVA: 0x0027A3B8 File Offset: 0x002787B8
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo3
			{
				get
				{
					return this._npadNo3;
				}
			}

			// Token: 0x170007F9 RID: 2041
			// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x0027A3C0 File Offset: 0x002787C0
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo4
			{
				get
				{
					return this._npadNo4;
				}
			}

			// Token: 0x170007FA RID: 2042
			// (get) Token: 0x06004EA6 RID: 20134 RVA: 0x0027A3C8 File Offset: 0x002787C8
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo5
			{
				get
				{
					return this._npadNo5;
				}
			}

			// Token: 0x170007FB RID: 2043
			// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x0027A3D0 File Offset: 0x002787D0
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo6
			{
				get
				{
					return this._npadNo6;
				}
			}

			// Token: 0x170007FC RID: 2044
			// (get) Token: 0x06004EA8 RID: 20136 RVA: 0x0027A3D8 File Offset: 0x002787D8
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo7
			{
				get
				{
					return this._npadNo7;
				}
			}

			// Token: 0x170007FD RID: 2045
			// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x0027A3E0 File Offset: 0x002787E0
			private NintendoSwitchInputManager.NpadSettings_Internal npadNo8
			{
				get
				{
					return this._npadNo8;
				}
			}

			// Token: 0x170007FE RID: 2046
			// (get) Token: 0x06004EAA RID: 20138 RVA: 0x0027A3E8 File Offset: 0x002787E8
			private NintendoSwitchInputManager.NpadSettings_Internal npadHandheld
			{
				get
				{
					return this._npadHandheld;
				}
			}

			// Token: 0x170007FF RID: 2047
			// (get) Token: 0x06004EAB RID: 20139 RVA: 0x0027A3F0 File Offset: 0x002787F0
			public NintendoSwitchInputManager.DebugPadSettings_Internal debugPad
			{
				get
				{
					return this._debugPad;
				}
			}

			// Token: 0x17000800 RID: 2048
			// (get) Token: 0x06004EAC RID: 20140 RVA: 0x0027A3F8 File Offset: 0x002787F8
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					Dictionary<int, object[]> dictionary = new Dictionary<int, object[]>();
					dictionary.Add(0, new object[]
					{
						new Func<int>(this.get_allowedNpadStyles),
						new Action<int>(delegate(int x)
						{
							this.allowedNpadStyles = x;
						})
					});
					dictionary.Add(1, new object[]
					{
						new Func<int>(this.get_joyConGripStyle),
						new Action<int>(delegate(int x)
						{
							this.joyConGripStyle = x;
						})
					});
					dictionary.Add(2, new object[]
					{
						new Func<bool>(this.get_adjustIMUsForGripStyle),
						new Action<bool>(delegate(bool x)
						{
							this.adjustIMUsForGripStyle = x;
						})
					});
					dictionary.Add(3, new object[]
					{
						new Func<int>(this.get_handheldActivationMode),
						new Action<int>(delegate(int x)
						{
							this.handheldActivationMode = x;
						})
					});
					dictionary.Add(4, new object[]
					{
						new Func<bool>(this.get_assignJoysticksByNpadId),
						new Action<bool>(delegate(bool x)
						{
							this.assignJoysticksByNpadId = x;
						})
					});
					Dictionary<int, object[]> dictionary2 = dictionary;
					int key = 5;
					object[] array = new object[2];
					array[0] = new Func<object>(this.get_npadNo1);
					dictionary2.Add(key, array);
					Dictionary<int, object[]> dictionary3 = dictionary;
					int key2 = 6;
					object[] array2 = new object[2];
					array2[0] = new Func<object>(this.get_npadNo2);
					dictionary3.Add(key2, array2);
					Dictionary<int, object[]> dictionary4 = dictionary;
					int key3 = 7;
					object[] array3 = new object[2];
					array3[0] = new Func<object>(this.get_npadNo3);
					dictionary4.Add(key3, array3);
					Dictionary<int, object[]> dictionary5 = dictionary;
					int key4 = 8;
					object[] array4 = new object[2];
					array4[0] = new Func<object>(this.get_npadNo4);
					dictionary5.Add(key4, array4);
					Dictionary<int, object[]> dictionary6 = dictionary;
					int key5 = 9;
					object[] array5 = new object[2];
					array5[0] = new Func<object>(this.get_npadNo5);
					dictionary6.Add(key5, array5);
					Dictionary<int, object[]> dictionary7 = dictionary;
					int key6 = 10;
					object[] array6 = new object[2];
					array6[0] = new Func<object>(this.get_npadNo6);
					dictionary7.Add(key6, array6);
					Dictionary<int, object[]> dictionary8 = dictionary;
					int key7 = 11;
					object[] array7 = new object[2];
					array7[0] = new Func<object>(this.get_npadNo7);
					dictionary8.Add(key7, array7);
					Dictionary<int, object[]> dictionary9 = dictionary;
					int key8 = 12;
					object[] array8 = new object[2];
					array8[0] = new Func<object>(this.get_npadNo8);
					dictionary9.Add(key8, array8);
					Dictionary<int, object[]> dictionary10 = dictionary;
					int key9 = 13;
					object[] array9 = new object[2];
					array9[0] = new Func<object>(this.get_npadHandheld);
					dictionary10.Add(key9, array9);
					Dictionary<int, object[]> dictionary11 = dictionary;
					int key10 = 14;
					object[] array10 = new object[2];
					array10[0] = new Func<object>(this.get_debugPad);
					dictionary11.Add(key10, array10);
					return this.__delegates = dictionary;
				}
			}

			// Token: 0x06004EAD RID: 20141 RVA: 0x0027A61C File Offset: 0x00278A1C
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x06004EAE RID: 20142 RVA: 0x0027A67C File Offset: 0x00278A7C
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040051E3 RID: 20963
			[SerializeField]
			private int _allowedNpadStyles = -1;

			// Token: 0x040051E4 RID: 20964
			[SerializeField]
			private int _joyConGripStyle = 1;

			// Token: 0x040051E5 RID: 20965
			[SerializeField]
			private bool _adjustIMUsForGripStyle = true;

			// Token: 0x040051E6 RID: 20966
			[SerializeField]
			private int _handheldActivationMode;

			// Token: 0x040051E7 RID: 20967
			[SerializeField]
			private bool _assignJoysticksByNpadId = true;

			// Token: 0x040051E8 RID: 20968
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo1 = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x040051E9 RID: 20969
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo2 = new NintendoSwitchInputManager.NpadSettings_Internal(1);

			// Token: 0x040051EA RID: 20970
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo3 = new NintendoSwitchInputManager.NpadSettings_Internal(2);

			// Token: 0x040051EB RID: 20971
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo4 = new NintendoSwitchInputManager.NpadSettings_Internal(3);

			// Token: 0x040051EC RID: 20972
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo5 = new NintendoSwitchInputManager.NpadSettings_Internal(4);

			// Token: 0x040051ED RID: 20973
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo6 = new NintendoSwitchInputManager.NpadSettings_Internal(5);

			// Token: 0x040051EE RID: 20974
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo7 = new NintendoSwitchInputManager.NpadSettings_Internal(6);

			// Token: 0x040051EF RID: 20975
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadNo8 = new NintendoSwitchInputManager.NpadSettings_Internal(7);

			// Token: 0x040051F0 RID: 20976
			[SerializeField]
			private NintendoSwitchInputManager.NpadSettings_Internal _npadHandheld = new NintendoSwitchInputManager.NpadSettings_Internal(0);

			// Token: 0x040051F1 RID: 20977
			[SerializeField]
			private NintendoSwitchInputManager.DebugPadSettings_Internal _debugPad = new NintendoSwitchInputManager.DebugPadSettings_Internal(0);

			// Token: 0x040051F2 RID: 20978
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000C5D RID: 3165
		[Serializable]
		private sealed class NpadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x06004EB4 RID: 20148 RVA: 0x0027A6E4 File Offset: 0x00278AE4
			internal NpadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x17000801 RID: 2049
			// (get) Token: 0x06004EB5 RID: 20149 RVA: 0x0027A701 File Offset: 0x00278B01
			// (set) Token: 0x06004EB6 RID: 20150 RVA: 0x0027A709 File Offset: 0x00278B09
			private bool isAllowed
			{
				get
				{
					return this._isAllowed;
				}
				set
				{
					this._isAllowed = value;
				}
			}

			// Token: 0x17000802 RID: 2050
			// (get) Token: 0x06004EB7 RID: 20151 RVA: 0x0027A712 File Offset: 0x00278B12
			// (set) Token: 0x06004EB8 RID: 20152 RVA: 0x0027A71A File Offset: 0x00278B1A
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x17000803 RID: 2051
			// (get) Token: 0x06004EB9 RID: 20153 RVA: 0x0027A723 File Offset: 0x00278B23
			// (set) Token: 0x06004EBA RID: 20154 RVA: 0x0027A72B File Offset: 0x00278B2B
			private int joyConAssignmentMode
			{
				get
				{
					return this._joyConAssignmentMode;
				}
				set
				{
					this._joyConAssignmentMode = value;
				}
			}

			// Token: 0x17000804 RID: 2052
			// (get) Token: 0x06004EBB RID: 20155 RVA: 0x0027A734 File Offset: 0x00278B34
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(this.get_isAllowed),
								new Action<bool>(delegate(bool x)
								{
									this.isAllowed = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(this.get_rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						},
						{
							2,
							new object[]
							{
								new Func<int>(this.get_joyConAssignmentMode),
								new Action<int>(delegate(int x)
								{
									this.joyConAssignmentMode = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x06004EBC RID: 20156 RVA: 0x0027A7E4 File Offset: 0x00278BE4
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x06004EBD RID: 20157 RVA: 0x0027A844 File Offset: 0x00278C44
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040051F3 RID: 20979
			[Tooltip("Determines whether this Npad id is allowed to be used by the system.")]
			[SerializeField]
			private bool _isAllowed = true;

			// Token: 0x040051F4 RID: 20980
			[Tooltip("The Rewired Player Id assigned to this Npad id.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x040051F5 RID: 20981
			[Tooltip("Determines how Joy-Cons should be handled.\n\nUnmodified: Joy-Con assignment mode will be left at the system default.\nDual: Joy-Cons pairs are handled as a single controller.\nSingle: Joy-Cons are handled as individual controllers.")]
			[SerializeField]
			private int _joyConAssignmentMode = -1;

			// Token: 0x040051F6 RID: 20982
			private Dictionary<int, object[]> __delegates;
		}

		// Token: 0x02000C5E RID: 3166
		[Serializable]
		private sealed class DebugPadSettings_Internal : IKeyedData<int>
		{
			// Token: 0x06004EC1 RID: 20161 RVA: 0x0027A89A File Offset: 0x00278C9A
			internal DebugPadSettings_Internal(int playerId)
			{
				this._rewiredPlayerId = playerId;
			}

			// Token: 0x17000805 RID: 2053
			// (get) Token: 0x06004EC2 RID: 20162 RVA: 0x0027A8A9 File Offset: 0x00278CA9
			// (set) Token: 0x06004EC3 RID: 20163 RVA: 0x0027A8B1 File Offset: 0x00278CB1
			private int rewiredPlayerId
			{
				get
				{
					return this._rewiredPlayerId;
				}
				set
				{
					this._rewiredPlayerId = value;
				}
			}

			// Token: 0x17000806 RID: 2054
			// (get) Token: 0x06004EC4 RID: 20164 RVA: 0x0027A8BA File Offset: 0x00278CBA
			// (set) Token: 0x06004EC5 RID: 20165 RVA: 0x0027A8C2 File Offset: 0x00278CC2
			private bool enabled
			{
				get
				{
					return this._enabled;
				}
				set
				{
					this._enabled = value;
				}
			}

			// Token: 0x17000807 RID: 2055
			// (get) Token: 0x06004EC6 RID: 20166 RVA: 0x0027A8CC File Offset: 0x00278CCC
			private Dictionary<int, object[]> delegates
			{
				get
				{
					if (this.__delegates != null)
					{
						return this.__delegates;
					}
					return this.__delegates = new Dictionary<int, object[]>
					{
						{
							0,
							new object[]
							{
								new Func<bool>(this.get_enabled),
								new Action<bool>(delegate(bool x)
								{
									this.enabled = x;
								})
							}
						},
						{
							1,
							new object[]
							{
								new Func<int>(this.get_rewiredPlayerId),
								new Action<int>(delegate(int x)
								{
									this.rewiredPlayerId = x;
								})
							}
						}
					};
				}
			}

			// Token: 0x06004EC7 RID: 20167 RVA: 0x0027A954 File Offset: 0x00278D54
			bool IKeyedData<int>.TryGetValue<T>(int key, out T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					value = default(T);
					return false;
				}
				Func<T> func = array[0] as Func<T>;
				if (func == null)
				{
					value = default(T);
					return false;
				}
				value = func();
				return true;
			}

			// Token: 0x06004EC8 RID: 20168 RVA: 0x0027A9B4 File Offset: 0x00278DB4
			bool IKeyedData<int>.TrySetValue<T>(int key, T value)
			{
				object[] array;
				if (!this.delegates.TryGetValue(key, out array))
				{
					return false;
				}
				Action<T> action = array[1] as Action<T>;
				if (action == null)
				{
					return false;
				}
				action(value);
				return true;
			}

			// Token: 0x040051F7 RID: 20983
			[Tooltip("Determines whether the Debug Pad will be enabled.")]
			[SerializeField]
			private bool _enabled;

			// Token: 0x040051F8 RID: 20984
			[Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")]
			[SerializeField]
			private int _rewiredPlayerId;

			// Token: 0x040051F9 RID: 20985
			private Dictionary<int, object[]> __delegates;
		}
	}
}
