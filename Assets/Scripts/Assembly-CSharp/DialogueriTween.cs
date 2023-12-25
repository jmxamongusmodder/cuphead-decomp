using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B83 RID: 2947
public class DialogueriTween : MonoBehaviour
{
	// Token: 0x060046DA RID: 18138 RVA: 0x0024FF0C File Offset: 0x0024E30C
	public static void Init(GameObject target)
	{
		DialogueriTween.MoveBy(target, Vector3.zero, 0f);
	}

	// Token: 0x060046DB RID: 18139 RVA: 0x0024FF20 File Offset: 0x0024E320
	public static void CameraFadeFrom(float amount, float time)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.CameraFadeFrom(DialogueriTween.Hash(new object[]
			{
				"amount",
				amount,
				"time",
				time
			}));
		}
		else
		{
			global::Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x060046DC RID: 18140 RVA: 0x0024FF7E File Offset: 0x0024E37E
	public static void CameraFadeFrom(Hashtable args)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.ColorFrom(DialogueriTween.cameraFade, args);
		}
		else
		{
			global::Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x060046DD RID: 18141 RVA: 0x0024FFAC File Offset: 0x0024E3AC
	public static void CameraFadeTo(float amount, float time)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.CameraFadeTo(DialogueriTween.Hash(new object[]
			{
				"amount",
				amount,
				"time",
				time
			}));
		}
		else
		{
			global::Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x060046DE RID: 18142 RVA: 0x0025000A File Offset: 0x0024E40A
	public static void CameraFadeTo(Hashtable args)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.ColorTo(DialogueriTween.cameraFade, args);
		}
		else
		{
			global::Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", null);
		}
	}

	// Token: 0x060046DF RID: 18143 RVA: 0x00250038 File Offset: 0x0024E438
	public static void ValueTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("onupdate") || !args.Contains("from") || !args.Contains("to"))
		{
			global::Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!", null);
			return;
		}
		args["type"] = "value";
		if (args["from"].GetType() == typeof(Vector2))
		{
			args["method"] = "vector2";
		}
		else if (args["from"].GetType() == typeof(Vector3))
		{
			args["method"] = "vector3";
		}
		else if (args["from"].GetType() == typeof(Rect))
		{
			args["method"] = "rect";
		}
		else if (args["from"].GetType() == typeof(float))
		{
			args["method"] = "float";
		}
		else
		{
			if (args["from"].GetType() != typeof(Color))
			{
				global::Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!", null);
				return;
			}
			args["method"] = "color";
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046E0 RID: 18144 RVA: 0x002501D2 File Offset: 0x0024E5D2
	public static void FadeFrom(GameObject target, float alpha, float time)
	{
		DialogueriTween.FadeFrom(target, DialogueriTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x060046E1 RID: 18145 RVA: 0x00250207 File Offset: 0x0024E607
	public static void FadeFrom(GameObject target, Hashtable args)
	{
		DialogueriTween.ColorFrom(target, args);
	}

	// Token: 0x060046E2 RID: 18146 RVA: 0x00250210 File Offset: 0x0024E610
	public static void FadeTo(GameObject target, float alpha, float time)
	{
		DialogueriTween.FadeTo(target, DialogueriTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x060046E3 RID: 18147 RVA: 0x00250245 File Offset: 0x0024E645
	public static void FadeTo(GameObject target, Hashtable args)
	{
		DialogueriTween.ColorTo(target, args);
	}

	// Token: 0x060046E4 RID: 18148 RVA: 0x0025024E File Offset: 0x0024E64E
	public static void ColorFrom(GameObject target, Color color, float time)
	{
		DialogueriTween.ColorFrom(target, DialogueriTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x060046E5 RID: 18149 RVA: 0x00250284 File Offset: 0x0024E684
	public static void ColorFrom(GameObject target, Hashtable args)
	{
		Color color = default(Color);
		Color color2 = default(Color);
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Hashtable hashtable = (Hashtable)args.Clone();
					hashtable["ischild"] = true;
					DialogueriTween.ColorFrom(transform.gameObject, hashtable);
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
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			color = (color2 = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			color = (color2 = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>())
		{
			color = (color2 = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			color = (color2 = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			color = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				color.r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				color.g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				color.b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				color.a = (float)args["a"];
			}
		}
		if (args.Contains("amount"))
		{
			color.a = (float)args["amount"];
			args.Remove("amount");
		}
		else if (args.Contains("alpha"))
		{
			color.a = (float)args["alpha"];
			args.Remove("alpha");
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.GetComponent<GUITexture>().color = color;
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.GetComponent<GUIText>().material.color = color;
		}
		else if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = color;
		}
		else if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = color;
		}
		args["color"] = color2;
		args["type"] = "color";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046E6 RID: 18150 RVA: 0x00250614 File Offset: 0x0024EA14
	public static void ColorTo(GameObject target, Color color, float time)
	{
		DialogueriTween.ColorTo(target, DialogueriTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x060046E7 RID: 18151 RVA: 0x0025064C File Offset: 0x0024EA4C
	public static void ColorTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					Hashtable hashtable = (Hashtable)args.Clone();
					hashtable["ischild"] = true;
					DialogueriTween.ColorTo(transform.gameObject, hashtable);
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
		}
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		args["type"] = "color";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046E8 RID: 18152 RVA: 0x0025074C File Offset: 0x0024EB4C
	public static void AudioFrom(GameObject target, float volume, float pitch, float time)
	{
		DialogueriTween.AudioFrom(target, DialogueriTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x060046E9 RID: 18153 RVA: 0x002507A0 File Offset: 0x0024EBA0
	public static void AudioFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				global::Debug.LogError("iTween Error: AudioFrom requires an AudioSource.", null);
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		Vector2 vector;
		Vector2 vector2;
		vector.x = (vector2.x = audioSource.volume);
		vector.y = (vector2.y = audioSource.pitch);
		if (args.Contains("volume"))
		{
			vector2.x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			vector2.y = (float)args["pitch"];
		}
		audioSource.volume = vector2.x;
		audioSource.pitch = vector2.y;
		args["volume"] = vector.x;
		args["pitch"] = vector.y;
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046EA RID: 18154 RVA: 0x0025091C File Offset: 0x0024ED1C
	public static void AudioTo(GameObject target, float volume, float pitch, float time)
	{
		DialogueriTween.AudioTo(target, DialogueriTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x060046EB RID: 18155 RVA: 0x00250970 File Offset: 0x0024ED70
	public static void AudioTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (!args.Contains("easetype"))
		{
			args.Add("easetype", DialogueriTween.EaseType.linear);
		}
		args["type"] = "audio";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046EC RID: 18156 RVA: 0x002509CE File Offset: 0x0024EDCE
	public static void Stab(GameObject target, AudioClip audioclip, float delay)
	{
		DialogueriTween.Stab(target, DialogueriTween.Hash(new object[]
		{
			"audioclip",
			audioclip,
			"delay",
			delay
		}));
	}

	// Token: 0x060046ED RID: 18157 RVA: 0x002509FE File Offset: 0x0024EDFE
	public static void Stab(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "stab";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046EE RID: 18158 RVA: 0x00250A1F File Offset: 0x0024EE1F
	public static void LookFrom(GameObject target, Vector3 looktarget, float time)
	{
		DialogueriTween.LookFrom(target, DialogueriTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x060046EF RID: 18159 RVA: 0x00250A54 File Offset: 0x0024EE54
	public static void LookFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		Vector3 eulerAngles = target.transform.eulerAngles;
		if (args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = target.transform;
			Transform target2 = (Transform)args["looktarget"];
			Vector3? vector = (Vector3?)args["up"];
			transform.LookAt(target2, (vector == null) ? DialogueriTween.Defaults.up : vector.Value);
		}
		else if (args["looktarget"].GetType() == typeof(Vector3))
		{
			Transform transform2 = target.transform;
			Vector3 worldPosition = (Vector3)args["looktarget"];
			Vector3? vector2 = (Vector3?)args["up"];
			transform2.LookAt(worldPosition, (vector2 == null) ? DialogueriTween.Defaults.up : vector2.Value);
		}
		if (args.Contains("axis"))
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			string text = (string)args["axis"];
			if (text != null)
			{
				if (!(text == "x"))
				{
					if (!(text == "y"))
					{
						if (text == "z")
						{
							eulerAngles2.x = eulerAngles.x;
							eulerAngles2.y = eulerAngles.y;
						}
					}
					else
					{
						eulerAngles2.x = eulerAngles.x;
						eulerAngles2.z = eulerAngles.z;
					}
				}
				else
				{
					eulerAngles2.y = eulerAngles.y;
					eulerAngles2.z = eulerAngles.z;
				}
			}
			target.transform.eulerAngles = eulerAngles2;
		}
		args["rotation"] = eulerAngles;
		args["type"] = "rotate";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046F0 RID: 18160 RVA: 0x00250C5E File Offset: 0x0024F05E
	public static void LookTo(GameObject target, Vector3 looktarget, float time)
	{
		DialogueriTween.LookTo(target, DialogueriTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x060046F1 RID: 18161 RVA: 0x00250C94 File Offset: 0x0024F094
	public static void LookTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("looktarget") && args["looktarget"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["looktarget"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
		}
		args["type"] = "look";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046F2 RID: 18162 RVA: 0x00250D91 File Offset: 0x0024F191
	public static void MoveTo(GameObject target, Vector3 position, float time)
	{
		DialogueriTween.MoveTo(target, DialogueriTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x060046F3 RID: 18163 RVA: 0x00250DC8 File Offset: 0x0024F1C8
	public static void MoveTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("position") && args["position"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["position"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "move";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046F4 RID: 18164 RVA: 0x00250F07 File Offset: 0x0024F307
	public static void MoveFrom(GameObject target, Vector3 position, float time)
	{
		DialogueriTween.MoveFrom(target, DialogueriTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x060046F5 RID: 18165 RVA: 0x00250F3C File Offset: 0x0024F33C
	public static void MoveFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		if (args.Contains("path"))
		{
			Vector3[] array2;
			if (args["path"].GetType() == typeof(Vector3[]))
			{
				Vector3[] array = (Vector3[])args["path"];
				array2 = new Vector3[array.Length];
				Array.Copy(array, array2, array.Length);
			}
			else
			{
				Transform[] array3 = (Transform[])args["path"];
				array2 = new Vector3[array3.Length];
				for (int i = 0; i < array3.Length; i++)
				{
					array2[i] = array3[i].position;
				}
			}
			if (array2[array2.Length - 1] != target.transform.position)
			{
				Vector3[] array4 = new Vector3[array2.Length + 1];
				Array.Copy(array2, array4, array2.Length);
				if (flag)
				{
					array4[array4.Length - 1] = target.transform.localPosition;
					target.transform.localPosition = array4[0];
				}
				else
				{
					array4[array4.Length - 1] = target.transform.position;
					target.transform.position = array4[0];
				}
				args["path"] = array4;
			}
			else
			{
				if (flag)
				{
					target.transform.localPosition = array2[0];
				}
				else
				{
					target.transform.position = array2[0];
				}
				args["path"] = array2;
			}
		}
		else
		{
			Vector3 vector2;
			Vector3 vector;
			if (flag)
			{
				vector = (vector2 = target.transform.localPosition);
			}
			else
			{
				vector = (vector2 = target.transform.position);
			}
			if (args.Contains("position"))
			{
				if (args["position"].GetType() == typeof(Transform))
				{
					Transform transform = (Transform)args["position"];
					vector = transform.position;
				}
				else if (args["position"].GetType() == typeof(Vector3))
				{
					vector = (Vector3)args["position"];
				}
			}
			else
			{
				if (args.Contains("x"))
				{
					vector.x = (float)args["x"];
				}
				if (args.Contains("y"))
				{
					vector.y = (float)args["y"];
				}
				if (args.Contains("z"))
				{
					vector.z = (float)args["z"];
				}
			}
			if (flag)
			{
				target.transform.localPosition = vector;
			}
			else
			{
				target.transform.position = vector;
			}
			args["position"] = vector2;
		}
		args["type"] = "move";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046F6 RID: 18166 RVA: 0x002512A8 File Offset: 0x0024F6A8
	public static void MoveAdd(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.MoveAdd(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060046F7 RID: 18167 RVA: 0x002512DD File Offset: 0x0024F6DD
	public static void MoveAdd(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "add";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046F8 RID: 18168 RVA: 0x0025130E File Offset: 0x0024F70E
	public static void MoveBy(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.MoveBy(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060046F9 RID: 18169 RVA: 0x00251343 File Offset: 0x0024F743
	public static void MoveBy(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "move";
		args["method"] = "by";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046FA RID: 18170 RVA: 0x00251374 File Offset: 0x0024F774
	public static void ScaleTo(GameObject target, Vector3 scale, float time)
	{
		DialogueriTween.ScaleTo(target, DialogueriTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x060046FB RID: 18171 RVA: 0x002513AC File Offset: 0x0024F7AC
	public static void ScaleTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("scale") && args["scale"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["scale"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "scale";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046FC RID: 18172 RVA: 0x002514EB File Offset: 0x0024F8EB
	public static void ScaleFrom(GameObject target, Vector3 scale, float time)
	{
		DialogueriTween.ScaleFrom(target, DialogueriTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x060046FD RID: 18173 RVA: 0x00251520 File Offset: 0x0024F920
	public static void ScaleFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		Vector3 localScale2;
		Vector3 localScale = localScale2 = target.transform.localScale;
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				localScale = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				localScale = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				localScale.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				localScale.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				localScale.z = (float)args["z"];
			}
		}
		target.transform.localScale = localScale;
		args["scale"] = localScale2;
		args["type"] = "scale";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x060046FE RID: 18174 RVA: 0x0025167D File Offset: 0x0024FA7D
	public static void ScaleAdd(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ScaleAdd(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x060046FF RID: 18175 RVA: 0x002516B2 File Offset: 0x0024FAB2
	public static void ScaleAdd(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "add";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004700 RID: 18176 RVA: 0x002516E3 File Offset: 0x0024FAE3
	public static void ScaleBy(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ScaleBy(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06004701 RID: 18177 RVA: 0x00251718 File Offset: 0x0024FB18
	public static void ScaleBy(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "scale";
		args["method"] = "by";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004702 RID: 18178 RVA: 0x00251749 File Offset: 0x0024FB49
	public static void RotateTo(GameObject target, Vector3 rotation, float time)
	{
		DialogueriTween.RotateTo(target, DialogueriTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06004703 RID: 18179 RVA: 0x00251780 File Offset: 0x0024FB80
	public static void RotateTo(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		if (args.Contains("rotation") && args["rotation"].GetType() == typeof(Transform))
		{
			Transform transform = (Transform)args["rotation"];
			args["position"] = new Vector3(transform.position.x, transform.position.y, transform.position.z);
			args["rotation"] = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
			args["scale"] = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
		}
		args["type"] = "rotate";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004704 RID: 18180 RVA: 0x002518BF File Offset: 0x0024FCBF
	public static void RotateFrom(GameObject target, Vector3 rotation, float time)
	{
		DialogueriTween.RotateFrom(target, DialogueriTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06004705 RID: 18181 RVA: 0x002518F4 File Offset: 0x0024FCF4
	public static void RotateFrom(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		Vector3 vector2;
		Vector3 vector;
		if (flag)
		{
			vector = (vector2 = target.transform.localEulerAngles);
		}
		else
		{
			vector = (vector2 = target.transform.eulerAngles);
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				vector = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				vector = (Vector3)args["rotation"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				vector.x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				vector.y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				vector.z = (float)args["z"];
			}
		}
		if (flag)
		{
			target.transform.localEulerAngles = vector;
		}
		else
		{
			target.transform.eulerAngles = vector;
		}
		args["rotation"] = vector2;
		args["type"] = "rotate";
		args["method"] = "to";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004706 RID: 18182 RVA: 0x00251AAD File Offset: 0x0024FEAD
	public static void RotateAdd(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.RotateAdd(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06004707 RID: 18183 RVA: 0x00251AE2 File Offset: 0x0024FEE2
	public static void RotateAdd(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "add";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004708 RID: 18184 RVA: 0x00251B13 File Offset: 0x0024FF13
	public static void RotateBy(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.RotateBy(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06004709 RID: 18185 RVA: 0x00251B48 File Offset: 0x0024FF48
	public static void RotateBy(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "rotate";
		args["method"] = "by";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x0600470A RID: 18186 RVA: 0x00251B79 File Offset: 0x0024FF79
	public static void ShakePosition(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ShakePosition(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600470B RID: 18187 RVA: 0x00251BAE File Offset: 0x0024FFAE
	public static void ShakePosition(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "position";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x0600470C RID: 18188 RVA: 0x00251BDF File Offset: 0x0024FFDF
	public static void ShakeScale(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ShakeScale(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600470D RID: 18189 RVA: 0x00251C14 File Offset: 0x00250014
	public static void ShakeScale(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "scale";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x0600470E RID: 18190 RVA: 0x00251C45 File Offset: 0x00250045
	public static void ShakeRotation(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.ShakeRotation(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x0600470F RID: 18191 RVA: 0x00251C7A File Offset: 0x0025007A
	public static void ShakeRotation(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "shake";
		args["method"] = "rotation";
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004710 RID: 18192 RVA: 0x00251CAB File Offset: 0x002500AB
	public static void PunchPosition(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.PunchPosition(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06004711 RID: 18193 RVA: 0x00251CE0 File Offset: 0x002500E0
	public static void PunchPosition(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "position";
		args["easetype"] = DialogueriTween.EaseType.punch;
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004712 RID: 18194 RVA: 0x00251D2E File Offset: 0x0025012E
	public static void PunchRotation(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.PunchRotation(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06004713 RID: 18195 RVA: 0x00251D64 File Offset: 0x00250164
	public static void PunchRotation(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "rotation";
		args["easetype"] = DialogueriTween.EaseType.punch;
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004714 RID: 18196 RVA: 0x00251DB2 File Offset: 0x002501B2
	public static void PunchScale(GameObject target, Vector3 amount, float time)
	{
		DialogueriTween.PunchScale(target, DialogueriTween.Hash(new object[]
		{
			"amount",
			amount,
			"time",
			time
		}));
	}

	// Token: 0x06004715 RID: 18197 RVA: 0x00251DE8 File Offset: 0x002501E8
	public static void PunchScale(GameObject target, Hashtable args)
	{
		args = DialogueriTween.CleanArgs(args);
		args["type"] = "punch";
		args["method"] = "scale";
		args["easetype"] = DialogueriTween.EaseType.punch;
		DialogueriTween.Launch(target, args);
	}

	// Token: 0x06004716 RID: 18198 RVA: 0x00251E38 File Offset: 0x00250238
	private void GenerateTargets()
	{
		string text = this.type;
		switch (text)
		{
		case "value":
		{
			string text2 = this.method;
			if (text2 != null)
			{
				if (!(text2 == "float"))
				{
					if (!(text2 == "vector2"))
					{
						if (!(text2 == "vector3"))
						{
							if (!(text2 == "color"))
							{
								if (text2 == "rect")
								{
									this.GenerateRectTargets();
									this.apply = new DialogueriTween.ApplyTween(this.ApplyRectTargets);
								}
							}
							else
							{
								this.GenerateColorTargets();
								this.apply = new DialogueriTween.ApplyTween(this.ApplyColorTargets);
							}
						}
						else
						{
							this.GenerateVector3Targets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyVector3Targets);
						}
					}
					else
					{
						this.GenerateVector2Targets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyVector2Targets);
					}
				}
				else
				{
					this.GenerateFloatTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyFloatTargets);
				}
			}
			break;
		}
		case "color":
		{
			string text3 = this.method;
			if (text3 != null)
			{
				if (text3 == "to")
				{
					this.GenerateColorToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyColorToTargets);
				}
			}
			break;
		}
		case "audio":
		{
			string text4 = this.method;
			if (text4 != null)
			{
				if (text4 == "to")
				{
					this.GenerateAudioToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyAudioToTargets);
				}
			}
			break;
		}
		case "move":
		{
			string text5 = this.method;
			if (text5 != null)
			{
				if (!(text5 == "to"))
				{
					if (text5 == "by" || text5 == "add")
					{
						this.GenerateMoveByTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyMoveByTargets);
					}
				}
				else if (this.tweenArguments.Contains("path"))
				{
					this.GenerateMoveToPathTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyMoveToPathTargets);
				}
				else
				{
					this.GenerateMoveToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyMoveToTargets);
				}
			}
			break;
		}
		case "scale":
		{
			string text6 = this.method;
			if (text6 != null)
			{
				if (!(text6 == "to"))
				{
					if (!(text6 == "by"))
					{
						if (text6 == "add")
						{
							this.GenerateScaleAddTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyScaleToTargets);
						}
					}
					else
					{
						this.GenerateScaleByTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyScaleToTargets);
					}
				}
				else
				{
					this.GenerateScaleToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyScaleToTargets);
				}
			}
			break;
		}
		case "rotate":
		{
			string text7 = this.method;
			if (text7 != null)
			{
				if (!(text7 == "to"))
				{
					if (!(text7 == "add"))
					{
						if (text7 == "by")
						{
							this.GenerateRotateByTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyRotateAddTargets);
						}
					}
					else
					{
						this.GenerateRotateAddTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyRotateAddTargets);
					}
				}
				else
				{
					this.GenerateRotateToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyRotateToTargets);
				}
			}
			break;
		}
		case "shake":
		{
			string text8 = this.method;
			if (text8 != null)
			{
				if (!(text8 == "position"))
				{
					if (!(text8 == "scale"))
					{
						if (text8 == "rotation")
						{
							this.GenerateShakeRotationTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyShakeRotationTargets);
						}
					}
					else
					{
						this.GenerateShakeScaleTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyShakeScaleTargets);
					}
				}
				else
				{
					this.GenerateShakePositionTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyShakePositionTargets);
				}
			}
			break;
		}
		case "punch":
		{
			string text9 = this.method;
			if (text9 != null)
			{
				if (!(text9 == "position"))
				{
					if (!(text9 == "rotation"))
					{
						if (text9 == "scale")
						{
							this.GeneratePunchScaleTargets();
							this.apply = new DialogueriTween.ApplyTween(this.ApplyPunchScaleTargets);
						}
					}
					else
					{
						this.GeneratePunchRotationTargets();
						this.apply = new DialogueriTween.ApplyTween(this.ApplyPunchRotationTargets);
					}
				}
				else
				{
					this.GeneratePunchPositionTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyPunchPositionTargets);
				}
			}
			break;
		}
		case "look":
		{
			string text10 = this.method;
			if (text10 != null)
			{
				if (text10 == "to")
				{
					this.GenerateLookToTargets();
					this.apply = new DialogueriTween.ApplyTween(this.ApplyLookToTargets);
				}
			}
			break;
		}
		case "stab":
			this.GenerateStabTargets();
			this.apply = new DialogueriTween.ApplyTween(this.ApplyStabTargets);
			break;
		}
	}

	// Token: 0x06004717 RID: 18199 RVA: 0x00252458 File Offset: 0x00250858
	private void GenerateRectTargets()
	{
		this.rects = new Rect[3];
		this.rects[0] = (Rect)this.tweenArguments["from"];
		this.rects[1] = (Rect)this.tweenArguments["to"];
	}

	// Token: 0x06004718 RID: 18200 RVA: 0x002524C0 File Offset: 0x002508C0
	private void GenerateColorTargets()
	{
		this.colors = new Color[1, 3];
		this.colors[0, 0] = (Color)this.tweenArguments["from"];
		this.colors[0, 1] = (Color)this.tweenArguments["to"];
	}

	// Token: 0x06004719 RID: 18201 RVA: 0x00252528 File Offset: 0x00250928
	private void GenerateVector3Targets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (Vector3)this.tweenArguments["from"];
		this.vector3s[1] = (Vector3)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600471A RID: 18202 RVA: 0x002525EC File Offset: 0x002509EC
	private void GenerateVector2Targets()
	{
		this.vector2s = new Vector2[3];
		this.vector2s[0] = (Vector2)this.tweenArguments["from"];
		this.vector2s[1] = (Vector2)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			Vector3 a = new Vector3(this.vector2s[0].x, this.vector2s[0].y, 0f);
			Vector3 b = new Vector3(this.vector2s[1].x, this.vector2s[1].y, 0f);
			float num = Math.Abs(Vector3.Distance(a, b));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600471B RID: 18203 RVA: 0x002526EC File Offset: 0x00250AEC
	private void GenerateFloatTargets()
	{
		this.floats = new float[3];
		this.floats[0] = (float)this.tweenArguments["from"];
		this.floats[1] = (float)this.tweenArguments["to"];
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(this.floats[0] - this.floats[1]);
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x0600471C RID: 18204 RVA: 0x00252788 File Offset: 0x00250B88
	private void GenerateColorToTargets()
	{
		if (base.GetComponent(typeof(GUITexture)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<GUITexture>().color);
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<GUIText>().material.color);
		}
		else if (base.GetComponent<Renderer>())
		{
			this.colors = new Color[base.GetComponent<Renderer>().materials.Length, 3];
			for (int i = 0; i < base.GetComponent<Renderer>().materials.Length; i++)
			{
				this.colors[i, 0] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
				this.colors[i, 1] = base.GetComponent<Renderer>().materials[i].GetColor(this.namedcolorvalue.ToString());
			}
		}
		else if (base.GetComponent<Light>())
		{
			this.colors = new Color[1, 3];
			this.colors[0, 0] = (this.colors[0, 1] = base.GetComponent<Light>().color);
		}
		else
		{
			this.colors = new Color[1, 3];
		}
		if (this.tweenArguments.Contains("color"))
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				this.colors[j, 1] = (Color)this.tweenArguments["color"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("r"))
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					this.colors[k, 1].r = (float)this.tweenArguments["r"];
				}
			}
			if (this.tweenArguments.Contains("g"))
			{
				for (int l = 0; l < this.colors.GetLength(0); l++)
				{
					this.colors[l, 1].g = (float)this.tweenArguments["g"];
				}
			}
			if (this.tweenArguments.Contains("b"))
			{
				for (int m = 0; m < this.colors.GetLength(0); m++)
				{
					this.colors[m, 1].b = (float)this.tweenArguments["b"];
				}
			}
			if (this.tweenArguments.Contains("a"))
			{
				for (int n = 0; n < this.colors.GetLength(0); n++)
				{
					this.colors[n, 1].a = (float)this.tweenArguments["a"];
				}
			}
		}
		if (this.tweenArguments.Contains("amount"))
		{
			for (int num = 0; num < this.colors.GetLength(0); num++)
			{
				this.colors[num, 1].a = (float)this.tweenArguments["amount"];
			}
		}
		else if (this.tweenArguments.Contains("alpha"))
		{
			for (int num2 = 0; num2 < this.colors.GetLength(0); num2++)
			{
				this.colors[num2, 1].a = (float)this.tweenArguments["alpha"];
			}
		}
	}

	// Token: 0x0600471D RID: 18205 RVA: 0x00252C00 File Offset: 0x00251000
	private void GenerateAudioToTargets()
	{
		this.vector2s = new Vector2[3];
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			global::Debug.LogError("iTween Error: AudioTo requires an AudioSource.", null);
			this.Dispose();
		}
		this.vector2s[0] = (this.vector2s[1] = new Vector2(this.audioSource.volume, this.audioSource.pitch));
		if (this.tweenArguments.Contains("volume"))
		{
			this.vector2s[1].x = (float)this.tweenArguments["volume"];
		}
		if (this.tweenArguments.Contains("pitch"))
		{
			this.vector2s[1].y = (float)this.tweenArguments["pitch"];
		}
	}

	// Token: 0x0600471E RID: 18206 RVA: 0x00252D40 File Offset: 0x00251140
	private void GenerateStabTargets()
	{
		if (this.tweenArguments.Contains("audiosource"))
		{
			this.audioSource = (AudioSource)this.tweenArguments["audiosource"];
		}
		else if (base.GetComponent(typeof(AudioSource)))
		{
			this.audioSource = base.GetComponent<AudioSource>();
		}
		else
		{
			base.gameObject.AddComponent(typeof(AudioSource));
			this.audioSource = base.GetComponent<AudioSource>();
			this.audioSource.playOnAwake = false;
		}
		this.audioSource.clip = (AudioClip)this.tweenArguments["audioclip"];
		if (this.tweenArguments.Contains("pitch"))
		{
			this.audioSource.pitch = (float)this.tweenArguments["pitch"];
		}
		if (this.tweenArguments.Contains("volume"))
		{
			this.audioSource.volume = (float)this.tweenArguments["volume"];
		}
		this.time = this.audioSource.clip.length / this.audioSource.pitch;
	}

	// Token: 0x0600471F RID: 18207 RVA: 0x00252E88 File Offset: 0x00251288
	private void GenerateLookToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("looktarget"))
		{
			if (this.tweenArguments["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = base.transform;
				Transform target = (Transform)this.tweenArguments["looktarget"];
				Vector3? vector = (Vector3?)this.tweenArguments["up"];
				transform.LookAt(target, (vector == null) ? DialogueriTween.Defaults.up : vector.Value);
			}
			else if (this.tweenArguments["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform2 = base.transform;
				Vector3 worldPosition = (Vector3)this.tweenArguments["looktarget"];
				Vector3? vector2 = (Vector3?)this.tweenArguments["up"];
				transform2.LookAt(worldPosition, (vector2 == null) ? DialogueriTween.Defaults.up : vector2.Value);
			}
		}
		else
		{
			global::Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!", null);
			this.Dispose();
		}
		this.vector3s[1] = base.transform.eulerAngles;
		base.transform.eulerAngles = this.vector3s[0];
		if (this.tweenArguments.Contains("axis"))
		{
			string text = (string)this.tweenArguments["axis"];
			if (text != null)
			{
				if (!(text == "x"))
				{
					if (!(text == "y"))
					{
						if (text == "z")
						{
							this.vector3s[1].x = this.vector3s[0].x;
							this.vector3s[1].y = this.vector3s[0].y;
						}
					}
					else
					{
						this.vector3s[1].x = this.vector3s[0].x;
						this.vector3s[1].z = this.vector3s[0].z;
					}
				}
				else
				{
					this.vector3s[1].y = this.vector3s[0].y;
					this.vector3s[1].z = this.vector3s[0].z;
				}
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004720 RID: 18208 RVA: 0x00253250 File Offset: 0x00251650
	private void GenerateMoveToPathTargets()
	{
		Vector3[] array2;
		if (this.tweenArguments["path"].GetType() == typeof(Vector3[]))
		{
			Vector3[] array = (Vector3[])this.tweenArguments["path"];
			if (array.Length == 1)
			{
				global::Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!", null);
				this.Dispose();
			}
			array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
		}
		else
		{
			Transform[] array3 = (Transform[])this.tweenArguments["path"];
			if (array3.Length == 1)
			{
				global::Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!", null);
				this.Dispose();
			}
			array2 = new Vector3[array3.Length];
			for (int i = 0; i < array3.Length; i++)
			{
				array2[i] = array3[i].position;
			}
		}
		bool flag;
		int num;
		if (base.transform.position != array2[0])
		{
			if (!this.tweenArguments.Contains("movetopath") || (bool)this.tweenArguments["movetopath"])
			{
				flag = true;
				num = 3;
			}
			else
			{
				flag = false;
				num = 2;
			}
		}
		else
		{
			flag = false;
			num = 2;
		}
		this.vector3s = new Vector3[array2.Length + num];
		if (flag)
		{
			this.vector3s[1] = base.transform.position;
			num = 2;
		}
		else
		{
			num = 1;
		}
		Array.Copy(array2, 0, this.vector3s, num, array2.Length);
		this.vector3s[0] = this.vector3s[1] + (this.vector3s[1] - this.vector3s[2]);
		this.vector3s[this.vector3s.Length - 1] = this.vector3s[this.vector3s.Length - 2] + (this.vector3s[this.vector3s.Length - 2] - this.vector3s[this.vector3s.Length - 3]);
		if (this.vector3s[1] == this.vector3s[this.vector3s.Length - 2])
		{
			Vector3[] array4 = new Vector3[this.vector3s.Length];
			Array.Copy(this.vector3s, array4, this.vector3s.Length);
			array4[0] = array4[array4.Length - 3];
			array4[array4.Length - 1] = array4[2];
			this.vector3s = new Vector3[array4.Length];
			Array.Copy(array4, this.vector3s, array4.Length);
		}
		this.path = new DialogueriTween.CRSpline(this.vector3s);
		if (this.tweenArguments.Contains("speed"))
		{
			float num2 = DialogueriTween.PathLength(this.vector3s);
			this.time = num2 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004721 RID: 18209 RVA: 0x002535B0 File Offset: 0x002519B0
	private void GenerateMoveToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localPosition);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.position);
		}
		if (this.tweenArguments.Contains("position"))
		{
			if (this.tweenArguments["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["position"];
				this.vector3s[1] = transform.position;
			}
			else if (this.tweenArguments["position"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["position"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004722 RID: 18210 RVA: 0x00253858 File Offset: 0x00251C58
	private void GenerateMoveByTargets()
	{
		this.vector3s = new Vector3[6];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.position));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = this.vector3s[0] + (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = this.vector3s[0].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = this.vector3s[0].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = this.vector3s[0].z + (float)this.tweenArguments["z"];
			}
		}
		base.transform.Translate(this.vector3s[1], this.space);
		this.vector3s[5] = base.transform.position;
		base.transform.position = this.vector3s[0];
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			this.tweenArguments["looktarget"] = this.vector3s[1];
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004723 RID: 18211 RVA: 0x00253B1C File Offset: 0x00251F1C
	private void GenerateScaleToTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("scale"))
		{
			if (this.tweenArguments["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["scale"];
				this.vector3s[1] = transform.localScale;
			}
			else if (this.tweenArguments["scale"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["scale"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004724 RID: 18212 RVA: 0x00253D30 File Offset: 0x00252130
	private void GenerateScaleByTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = Vector3.Scale(this.vector3s[1], (Vector3)this.tweenArguments["amount"]);
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004725 RID: 18213 RVA: 0x00253EF4 File Offset: 0x002522F4
	private void GenerateScaleAddTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = (this.vector3s[1] = base.transform.localScale);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004726 RID: 18214 RVA: 0x002540B0 File Offset: 0x002524B0
	private void GenerateRotateToTargets()
	{
		this.vector3s = new Vector3[3];
		if (this.isLocal)
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.localEulerAngles);
		}
		else
		{
			this.vector3s[0] = (this.vector3s[1] = base.transform.eulerAngles);
		}
		if (this.tweenArguments.Contains("rotation"))
		{
			if (this.tweenArguments["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)this.tweenArguments["rotation"];
				this.vector3s[1] = transform.eulerAngles;
			}
			else if (this.tweenArguments["rotation"].GetType() == typeof(Vector3))
			{
				this.vector3s[1] = (Vector3)this.tweenArguments["rotation"];
			}
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
		this.vector3s[1] = new Vector3(this.clerp(this.vector3s[0].x, this.vector3s[1].x, 1f), this.clerp(this.vector3s[0].y, this.vector3s[1].y, 1f), this.clerp(this.vector3s[0].z, this.vector3s[1].z, 1f));
		if (this.tweenArguments.Contains("speed"))
		{
			float num = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004727 RID: 18215 RVA: 0x002543A0 File Offset: 0x002527A0
	private void GenerateRotateAddTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004728 RID: 18216 RVA: 0x00254570 File Offset: 0x00252970
	private void GenerateRotateByTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = (this.vector3s[1] = (this.vector3s[3] = base.transform.eulerAngles));
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] += Vector3.Scale((Vector3)this.tweenArguments["amount"], new Vector3(360f, 360f, 360f));
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				Vector3[] array = this.vector3s;
				int num = 1;
				array[num].x = array[num].x + 360f * (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				Vector3[] array2 = this.vector3s;
				int num2 = 1;
				array2[num2].y = array2[num2].y + 360f * (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				Vector3[] array3 = this.vector3s;
				int num3 = 1;
				array3[num3].z = array3[num3].z + 360f * (float)this.tweenArguments["z"];
			}
		}
		if (this.tweenArguments.Contains("speed"))
		{
			float num4 = Math.Abs(Vector3.Distance(this.vector3s[0], this.vector3s[1]));
			this.time = num4 / (float)this.tweenArguments["speed"];
		}
	}

	// Token: 0x06004729 RID: 18217 RVA: 0x00254768 File Offset: 0x00252B68
	private void GenerateShakePositionTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[3] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x0600472A RID: 18218 RVA: 0x002548AC File Offset: 0x00252CAC
	private void GenerateShakeScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x0600472B RID: 18219 RVA: 0x002549D4 File Offset: 0x00252DD4
	private void GenerateShakeRotationTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.eulerAngles;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x0600472C RID: 18220 RVA: 0x00254AFC File Offset: 0x00252EFC
	private void GeneratePunchPositionTargets()
	{
		this.vector3s = new Vector3[5];
		this.vector3s[4] = base.transform.eulerAngles;
		this.vector3s[0] = base.transform.position;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x0600472D RID: 18221 RVA: 0x00254C68 File Offset: 0x00253068
	private void GeneratePunchRotationTargets()
	{
		this.vector3s = new Vector3[4];
		this.vector3s[0] = base.transform.eulerAngles;
		this.vector3s[1] = (this.vector3s[3] = Vector3.zero);
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x0600472E RID: 18222 RVA: 0x00254DB8 File Offset: 0x002531B8
	private void GeneratePunchScaleTargets()
	{
		this.vector3s = new Vector3[3];
		this.vector3s[0] = base.transform.localScale;
		this.vector3s[1] = Vector3.zero;
		if (this.tweenArguments.Contains("amount"))
		{
			this.vector3s[1] = (Vector3)this.tweenArguments["amount"];
		}
		else
		{
			if (this.tweenArguments.Contains("x"))
			{
				this.vector3s[1].x = (float)this.tweenArguments["x"];
			}
			if (this.tweenArguments.Contains("y"))
			{
				this.vector3s[1].y = (float)this.tweenArguments["y"];
			}
			if (this.tweenArguments.Contains("z"))
			{
				this.vector3s[1].z = (float)this.tweenArguments["z"];
			}
		}
	}

	// Token: 0x0600472F RID: 18223 RVA: 0x00254EF4 File Offset: 0x002532F4
	private void ApplyRectTargets()
	{
		this.rects[2].x = this.ease(this.rects[0].x, this.rects[1].x, this.percentage);
		this.rects[2].y = this.ease(this.rects[0].y, this.rects[1].y, this.percentage);
		this.rects[2].width = this.ease(this.rects[0].width, this.rects[1].width, this.percentage);
		this.rects[2].height = this.ease(this.rects[0].height, this.rects[1].height, this.percentage);
		this.tweenArguments["onupdateparams"] = this.rects[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.rects[1];
		}
	}

	// Token: 0x06004730 RID: 18224 RVA: 0x00255070 File Offset: 0x00253470
	private void ApplyColorTargets()
	{
		this.colors[0, 2].r = this.ease(this.colors[0, 0].r, this.colors[0, 1].r, this.percentage);
		this.colors[0, 2].g = this.ease(this.colors[0, 0].g, this.colors[0, 1].g, this.percentage);
		this.colors[0, 2].b = this.ease(this.colors[0, 0].b, this.colors[0, 1].b, this.percentage);
		this.colors[0, 2].a = this.ease(this.colors[0, 0].a, this.colors[0, 1].a, this.percentage);
		this.tweenArguments["onupdateparams"] = this.colors[0, 2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.colors[0, 1];
		}
	}

	// Token: 0x06004731 RID: 18225 RVA: 0x002551F0 File Offset: 0x002535F0
	private void ApplyVector3Targets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector3s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector3s[1];
		}
	}

	// Token: 0x06004732 RID: 18226 RVA: 0x00255328 File Offset: 0x00253728
	private void ApplyVector2Targets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.tweenArguments["onupdateparams"] = this.vector2s[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.vector2s[1];
		}
	}

	// Token: 0x06004733 RID: 18227 RVA: 0x0025541C File Offset: 0x0025381C
	private void ApplyFloatTargets()
	{
		this.floats[2] = this.ease(this.floats[0], this.floats[1], this.percentage);
		this.tweenArguments["onupdateparams"] = this.floats[2];
		if (this.percentage == 1f)
		{
			this.tweenArguments["onupdateparams"] = this.floats[1];
		}
	}

	// Token: 0x06004734 RID: 18228 RVA: 0x0025549C File Offset: 0x0025389C
	private void ApplyColorToTargets()
	{
		for (int i = 0; i < this.colors.GetLength(0); i++)
		{
			this.colors[i, 2].r = this.ease(this.colors[i, 0].r, this.colors[i, 1].r, this.percentage);
			this.colors[i, 2].g = this.ease(this.colors[i, 0].g, this.colors[i, 1].g, this.percentage);
			this.colors[i, 2].b = this.ease(this.colors[i, 0].b, this.colors[i, 1].b, this.percentage);
			this.colors[i, 2].a = this.ease(this.colors[i, 0].a, this.colors[i, 1].a, this.percentage);
		}
		if (base.GetComponent(typeof(GUITexture)))
		{
			base.GetComponent<GUITexture>().color = this.colors[0, 2];
		}
		else if (base.GetComponent(typeof(GUIText)))
		{
			base.GetComponent<GUIText>().material.color = this.colors[0, 2];
		}
		else if (base.GetComponent<Renderer>())
		{
			for (int j = 0; j < this.colors.GetLength(0); j++)
			{
				base.GetComponent<Renderer>().materials[j].SetColor(this.namedcolorvalue.ToString(), this.colors[j, 2]);
			}
		}
		else if (base.GetComponent<Light>())
		{
			base.GetComponent<Light>().color = this.colors[0, 2];
		}
		if (this.percentage == 1f)
		{
			if (base.GetComponent(typeof(GUITexture)))
			{
				base.GetComponent<GUITexture>().color = this.colors[0, 1];
			}
			else if (base.GetComponent(typeof(GUIText)))
			{
				base.GetComponent<GUIText>().material.color = this.colors[0, 1];
			}
			else if (base.GetComponent<Renderer>())
			{
				for (int k = 0; k < this.colors.GetLength(0); k++)
				{
					base.GetComponent<Renderer>().materials[k].SetColor(this.namedcolorvalue.ToString(), this.colors[k, 1]);
				}
			}
			else if (base.GetComponent<Light>())
			{
				base.GetComponent<Light>().color = this.colors[0, 1];
			}
		}
	}

	// Token: 0x06004735 RID: 18229 RVA: 0x002557EC File Offset: 0x00253BEC
	private void ApplyAudioToTargets()
	{
		this.vector2s[2].x = this.ease(this.vector2s[0].x, this.vector2s[1].x, this.percentage);
		this.vector2s[2].y = this.ease(this.vector2s[0].y, this.vector2s[1].y, this.percentage);
		this.audioSource.volume = this.vector2s[2].x;
		this.audioSource.pitch = this.vector2s[2].y;
		if (this.percentage == 1f)
		{
			this.audioSource.volume = this.vector2s[1].x;
			this.audioSource.pitch = this.vector2s[1].y;
		}
	}

	// Token: 0x06004736 RID: 18230 RVA: 0x00255901 File Offset: 0x00253D01
	private void ApplyStabTargets()
	{
	}

	// Token: 0x06004737 RID: 18231 RVA: 0x00255904 File Offset: 0x00253D04
	private void ApplyMoveToPathTargets()
	{
		this.preUpdate = base.transform.position;
		float value = this.ease(0f, 1f, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		else
		{
			base.transform.position = this.path.Interp(Mathf.Clamp(value, 0f, 1f));
		}
		if (this.tweenArguments.Contains("orienttopath") && (bool)this.tweenArguments["orienttopath"])
		{
			float num;
			if (this.tweenArguments.Contains("lookahead"))
			{
				num = (float)this.tweenArguments["lookahead"];
			}
			else
			{
				num = DialogueriTween.Defaults.lookAhead;
			}
			float value2 = this.ease(0f, 1f, Mathf.Min(1f, this.percentage + num));
			this.tweenArguments["looktarget"] = this.path.Interp(Mathf.Clamp(value2, 0f, 1f));
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06004738 RID: 18232 RVA: 0x00255A98 File Offset: 0x00253E98
	private void ApplyMoveToTargets()
	{
		this.preUpdate = base.transform.position;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localPosition = this.vector3s[2];
		}
		else
		{
			base.transform.position = this.vector3s[2];
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localPosition = this.vector3s[1];
			}
			else
			{
				base.transform.position = this.vector3s[1];
			}
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06004739 RID: 18233 RVA: 0x00255C60 File Offset: 0x00254060
	private void ApplyMoveByTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x0600473A RID: 18234 RVA: 0x00255E48 File Offset: 0x00254248
	private void ApplyScaleToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.localScale = this.vector3s[2];
		if (this.percentage == 1f)
		{
			base.transform.localScale = this.vector3s[1];
		}
	}

	// Token: 0x0600473B RID: 18235 RVA: 0x00255F6C File Offset: 0x0025436C
	private void ApplyLookToTargets()
	{
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
	}

	// Token: 0x0600473C RID: 18236 RVA: 0x00256098 File Offset: 0x00254498
	private void ApplyRotateToTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		if (this.isLocal)
		{
			base.transform.localRotation = Quaternion.Euler(this.vector3s[2]);
		}
		else
		{
			base.transform.rotation = Quaternion.Euler(this.vector3s[2]);
		}
		if (this.percentage == 1f)
		{
			if (this.isLocal)
			{
				base.transform.localRotation = Quaternion.Euler(this.vector3s[1]);
			}
			else
			{
				base.transform.rotation = Quaternion.Euler(this.vector3s[1]);
			}
		}
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x0600473D RID: 18237 RVA: 0x0025627C File Offset: 0x0025467C
	private void ApplyRotateAddTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		this.vector3s[2].x = this.ease(this.vector3s[0].x, this.vector3s[1].x, this.percentage);
		this.vector3s[2].y = this.ease(this.vector3s[0].y, this.vector3s[1].y, this.percentage);
		this.vector3s[2].z = this.ease(this.vector3s[0].z, this.vector3s[1].z, this.percentage);
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x0600473E RID: 18238 RVA: 0x00256404 File Offset: 0x00254804
	private void ApplyShakePositionTargets()
	{
		if (this.isLocal)
		{
			this.preUpdate = base.transform.localPosition;
		}
		else
		{
			this.preUpdate = base.transform.position;
		}
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[3];
		}
		if (this.percentage == 0f)
		{
			base.transform.Translate(this.vector3s[1], this.space);
		}
		if (this.isLocal)
		{
			base.transform.localPosition = this.vector3s[0];
		}
		else
		{
			base.transform.position = this.vector3s[0];
		}
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		if (this.isLocal)
		{
			base.transform.localPosition += this.vector3s[2];
		}
		else
		{
			base.transform.position += this.vector3s[2];
		}
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x0600473F RID: 18239 RVA: 0x00256684 File Offset: 0x00254A84
	private void ApplyShakeScaleTargets()
	{
		if (this.percentage == 0f)
		{
			base.transform.localScale = this.vector3s[1];
		}
		base.transform.localScale = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.localScale += this.vector3s[2];
	}

	// Token: 0x06004740 RID: 18240 RVA: 0x002567C4 File Offset: 0x00254BC4
	private void ApplyShakeRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.percentage == 0f)
		{
			base.transform.Rotate(this.vector3s[1], this.space);
		}
		base.transform.eulerAngles = this.vector3s[0];
		float num = 1f - this.percentage;
		this.vector3s[2].x = UnityEngine.Random.Range(-this.vector3s[1].x * num, this.vector3s[1].x * num);
		this.vector3s[2].y = UnityEngine.Random.Range(-this.vector3s[1].y * num, this.vector3s[1].y * num);
		this.vector3s[2].z = UnityEngine.Random.Range(-this.vector3s[1].z * num, this.vector3s[1].z * num);
		base.transform.Rotate(this.vector3s[2], this.space);
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06004741 RID: 18241 RVA: 0x0025695C File Offset: 0x00254D5C
	private void ApplyPunchPositionTargets()
	{
		this.preUpdate = base.transform.position;
		Vector3 eulerAngles = default(Vector3);
		if (this.tweenArguments.Contains("looktarget"))
		{
			eulerAngles = base.transform.eulerAngles;
			base.transform.eulerAngles = this.vector3s[4];
		}
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Translate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		if (this.tweenArguments.Contains("looktarget"))
		{
			base.transform.eulerAngles = eulerAngles;
		}
		this.postUpdate = base.transform.position;
		if (this.physics)
		{
			base.transform.position = this.preUpdate;
			base.GetComponent<Rigidbody>().MovePosition(this.postUpdate);
		}
	}

	// Token: 0x06004742 RID: 18242 RVA: 0x00256C50 File Offset: 0x00255050
	private void ApplyPunchRotationTargets()
	{
		this.preUpdate = base.transform.eulerAngles;
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.Rotate(this.vector3s[2] - this.vector3s[3], this.space);
		this.vector3s[3] = this.vector3s[2];
		this.postUpdate = base.transform.eulerAngles;
		if (this.physics)
		{
			base.transform.eulerAngles = this.preUpdate;
			base.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(this.postUpdate));
		}
	}

	// Token: 0x06004743 RID: 18243 RVA: 0x00256EE4 File Offset: 0x002552E4
	private void ApplyPunchScaleTargets()
	{
		if (this.vector3s[1].x > 0f)
		{
			this.vector3s[2].x = this.punch(this.vector3s[1].x, this.percentage);
		}
		else if (this.vector3s[1].x < 0f)
		{
			this.vector3s[2].x = -this.punch(Mathf.Abs(this.vector3s[1].x), this.percentage);
		}
		if (this.vector3s[1].y > 0f)
		{
			this.vector3s[2].y = this.punch(this.vector3s[1].y, this.percentage);
		}
		else if (this.vector3s[1].y < 0f)
		{
			this.vector3s[2].y = -this.punch(Mathf.Abs(this.vector3s[1].y), this.percentage);
		}
		if (this.vector3s[1].z > 0f)
		{
			this.vector3s[2].z = this.punch(this.vector3s[1].z, this.percentage);
		}
		else if (this.vector3s[1].z < 0f)
		{
			this.vector3s[2].z = -this.punch(Mathf.Abs(this.vector3s[1].z), this.percentage);
		}
		base.transform.localScale = this.vector3s[0] + this.vector3s[2];
	}

	// Token: 0x06004744 RID: 18244 RVA: 0x002570FC File Offset: 0x002554FC
	private IEnumerator TweenDelay()
	{
		this.delayStarted = Time.time;
		yield return new WaitForSeconds(this.delay);
		if (this.wasPaused)
		{
			this.wasPaused = false;
			this.TweenStart();
		}
		yield break;
	}

	// Token: 0x06004745 RID: 18245 RVA: 0x00257118 File Offset: 0x00255518
	private void TweenStart()
	{
		this.CallBack("onstart");
		if (!this.loop)
		{
			this.ConflictCheck();
			this.GenerateTargets();
		}
		if (this.type == "stab")
		{
			this.audioSource.PlayOneShot(this.audioSource.clip);
		}
		if (this.type == "move" || this.type == "scale" || this.type == "rotate" || this.type == "punch" || this.type == "shake" || this.type == "curve" || this.type == "look")
		{
			this.EnableKinematic();
		}
		this.isRunning = true;
	}

	// Token: 0x06004746 RID: 18246 RVA: 0x00257214 File Offset: 0x00255614
	private IEnumerator TweenRestart()
	{
		if (this.delay > 0f)
		{
			this.delayStarted = Time.time;
			yield return new WaitForSeconds(this.delay);
		}
		this.loop = true;
		this.TweenStart();
		yield break;
	}

	// Token: 0x06004747 RID: 18247 RVA: 0x0025722F File Offset: 0x0025562F
	private void TweenUpdate()
	{
		this.apply();
		this.CallBack("onupdate");
		this.UpdatePercentage();
	}

	// Token: 0x06004748 RID: 18248 RVA: 0x00257250 File Offset: 0x00255650
	private void TweenComplete()
	{
		this.isRunning = false;
		if (this.percentage > 0.5f)
		{
			this.percentage = 1f;
		}
		else
		{
			this.percentage = 0f;
		}
		this.apply();
		if (this.type == "value")
		{
			this.CallBack("onupdate");
		}
		if (this.loopType == DialogueriTween.LoopType.none)
		{
			this.Dispose();
		}
		else
		{
			this.TweenLoop();
		}
		this.CallBack("oncomplete");
	}

	// Token: 0x06004749 RID: 18249 RVA: 0x002572E4 File Offset: 0x002556E4
	private void TweenLoop()
	{
		this.DisableKinematic();
		DialogueriTween.LoopType loopType = this.loopType;
		if (loopType != DialogueriTween.LoopType.loop)
		{
			if (loopType == DialogueriTween.LoopType.pingPong)
			{
				this.reverse = !this.reverse;
				this.runningTime = 0f;
				base.StartCoroutine("TweenRestart");
			}
		}
		else
		{
			this.percentage = 0f;
			this.runningTime = 0f;
			this.apply();
			base.StartCoroutine("TweenRestart");
		}
	}

	// Token: 0x0600474A RID: 18250 RVA: 0x00257370 File Offset: 0x00255770
	public static Rect RectUpdate(Rect currentValue, Rect targetValue, float speed)
	{
		Rect result = new Rect(DialogueriTween.FloatUpdate(currentValue.x, targetValue.x, speed), DialogueriTween.FloatUpdate(currentValue.y, targetValue.y, speed), DialogueriTween.FloatUpdate(currentValue.width, targetValue.width, speed), DialogueriTween.FloatUpdate(currentValue.height, targetValue.height, speed));
		return result;
	}

	// Token: 0x0600474B RID: 18251 RVA: 0x002573D8 File Offset: 0x002557D8
	public static Vector3 Vector3Update(Vector3 currentValue, Vector3 targetValue, float speed)
	{
		Vector3 a = targetValue - currentValue;
		currentValue += a * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x0600474C RID: 18252 RVA: 0x00257408 File Offset: 0x00255808
	public static Vector2 Vector2Update(Vector2 currentValue, Vector2 targetValue, float speed)
	{
		Vector2 a = targetValue - currentValue;
		currentValue += a * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x0600474D RID: 18253 RVA: 0x00257438 File Offset: 0x00255838
	public static float FloatUpdate(float currentValue, float targetValue, float speed)
	{
		float num = targetValue - currentValue;
		currentValue += num * speed * Time.deltaTime;
		return currentValue;
	}

	// Token: 0x0600474E RID: 18254 RVA: 0x00257457 File Offset: 0x00255857
	public static void FadeUpdate(GameObject target, Hashtable args)
	{
		args["a"] = args["alpha"];
		DialogueriTween.ColorUpdate(target, args);
	}

	// Token: 0x0600474F RID: 18255 RVA: 0x00257476 File Offset: 0x00255876
	public static void FadeUpdate(GameObject target, float alpha, float time)
	{
		DialogueriTween.FadeUpdate(target, DialogueriTween.Hash(new object[]
		{
			"alpha",
			alpha,
			"time",
			time
		}));
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x002574AC File Offset: 0x002558AC
	public static void ColorUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Color[] array = new Color[4];
		if (!args.Contains("includechildren") || (bool)args["includechildren"])
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.ColorUpdate(transform.gameObject, args);
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
		}
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		if (target.GetComponent(typeof(GUITexture)))
		{
			array[0] = (array[1] = target.GetComponent<GUITexture>().color);
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			array[0] = (array[1] = target.GetComponent<GUIText>().material.color);
		}
		else if (target.GetComponent<Renderer>())
		{
			array[0] = (array[1] = target.GetComponent<Renderer>().material.color);
		}
		else if (target.GetComponent<Light>())
		{
			array[0] = (array[1] = target.GetComponent<Light>().color);
		}
		if (args.Contains("color"))
		{
			array[1] = (Color)args["color"];
		}
		else
		{
			if (args.Contains("r"))
			{
				array[1].r = (float)args["r"];
			}
			if (args.Contains("g"))
			{
				array[1].g = (float)args["g"];
			}
			if (args.Contains("b"))
			{
				array[1].b = (float)args["b"];
			}
			if (args.Contains("a"))
			{
				array[1].a = (float)args["a"];
			}
		}
		array[3].r = Mathf.SmoothDamp(array[0].r, array[1].r, ref array[2].r, num);
		array[3].g = Mathf.SmoothDamp(array[0].g, array[1].g, ref array[2].g, num);
		array[3].b = Mathf.SmoothDamp(array[0].b, array[1].b, ref array[2].b, num);
		array[3].a = Mathf.SmoothDamp(array[0].a, array[1].a, ref array[2].a, num);
		if (target.GetComponent(typeof(GUITexture)))
		{
			target.GetComponent<GUITexture>().color = array[3];
		}
		else if (target.GetComponent(typeof(GUIText)))
		{
			target.GetComponent<GUIText>().material.color = array[3];
		}
		else if (target.GetComponent<Renderer>())
		{
			target.GetComponent<Renderer>().material.color = array[3];
		}
		else if (target.GetComponent<Light>())
		{
			target.GetComponent<Light>().color = array[3];
		}
	}

	// Token: 0x06004751 RID: 18257 RVA: 0x00257910 File Offset: 0x00255D10
	public static void ColorUpdate(GameObject target, Color color, float time)
	{
		DialogueriTween.ColorUpdate(target, DialogueriTween.Hash(new object[]
		{
			"color",
			color,
			"time",
			time
		}));
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x00257948 File Offset: 0x00255D48
	public static void AudioUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector2[] array = new Vector2[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		AudioSource audioSource;
		if (args.Contains("audiosource"))
		{
			audioSource = (AudioSource)args["audiosource"];
		}
		else
		{
			if (!target.GetComponent(typeof(AudioSource)))
			{
				global::Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.", null);
				return;
			}
			audioSource = target.GetComponent<AudioSource>();
		}
		array[0] = (array[1] = new Vector2(audioSource.volume, audioSource.pitch));
		if (args.Contains("volume"))
		{
			array[1].x = (float)args["volume"];
		}
		if (args.Contains("pitch"))
		{
			array[1].y = (float)args["pitch"];
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		audioSource.volume = array[3].x;
		audioSource.pitch = array[3].y;
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x00257B04 File Offset: 0x00255F04
	public static void AudioUpdate(GameObject target, float volume, float pitch, float time)
	{
		DialogueriTween.AudioUpdate(target, DialogueriTween.Hash(new object[]
		{
			"volume",
			volume,
			"pitch",
			pitch,
			"time",
			time
		}));
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x00257B58 File Offset: 0x00255F58
	public static void RotateUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 eulerAngles = target.transform.eulerAngles;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = target.transform.localEulerAngles;
		}
		else
		{
			array[0] = target.transform.eulerAngles;
		}
		if (args.Contains("rotation"))
		{
			if (args["rotation"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["rotation"];
				array[1] = transform.eulerAngles;
			}
			else if (args["rotation"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["rotation"];
			}
		}
		array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
		if (flag)
		{
			target.transform.localEulerAngles = array[3];
		}
		else
		{
			target.transform.eulerAngles = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 eulerAngles2 = target.transform.eulerAngles;
			target.transform.eulerAngles = eulerAngles;
			target.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(eulerAngles2));
		}
	}

	// Token: 0x06004755 RID: 18261 RVA: 0x00257DC3 File Offset: 0x002561C3
	public static void RotateUpdate(GameObject target, Vector3 rotation, float time)
	{
		DialogueriTween.RotateUpdate(target, DialogueriTween.Hash(new object[]
		{
			"rotation",
			rotation,
			"time",
			time
		}));
	}

	// Token: 0x06004756 RID: 18262 RVA: 0x00257DF8 File Offset: 0x002561F8
	public static void ScaleUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		array[0] = (array[1] = target.transform.localScale);
		if (args.Contains("scale"))
		{
			if (args["scale"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["scale"];
				array[1] = transform.localScale;
			}
			else if (args["scale"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["scale"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		target.transform.localScale = array[3];
	}

	// Token: 0x06004757 RID: 18263 RVA: 0x00258041 File Offset: 0x00256441
	public static void ScaleUpdate(GameObject target, Vector3 scale, float time)
	{
		DialogueriTween.ScaleUpdate(target, DialogueriTween.Hash(new object[]
		{
			"scale",
			scale,
			"time",
			time
		}));
	}

	// Token: 0x06004758 RID: 18264 RVA: 0x00258078 File Offset: 0x00256478
	public static void MoveUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[4];
		Vector3 position = target.transform.position;
		float num;
		if (args.Contains("time"))
		{
			num = (float)args["time"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		bool flag;
		if (args.Contains("islocal"))
		{
			flag = (bool)args["islocal"];
		}
		else
		{
			flag = DialogueriTween.Defaults.isLocal;
		}
		if (flag)
		{
			array[0] = (array[1] = target.transform.localPosition);
		}
		else
		{
			array[0] = (array[1] = target.transform.position);
		}
		if (args.Contains("position"))
		{
			if (args["position"].GetType() == typeof(Transform))
			{
				Transform transform = (Transform)args["position"];
				array[1] = transform.position;
			}
			else if (args["position"].GetType() == typeof(Vector3))
			{
				array[1] = (Vector3)args["position"];
			}
		}
		else
		{
			if (args.Contains("x"))
			{
				array[1].x = (float)args["x"];
			}
			if (args.Contains("y"))
			{
				array[1].y = (float)args["y"];
			}
			if (args.Contains("z"))
			{
				array[1].z = (float)args["z"];
			}
		}
		array[3].x = Mathf.SmoothDamp(array[0].x, array[1].x, ref array[2].x, num);
		array[3].y = Mathf.SmoothDamp(array[0].y, array[1].y, ref array[2].y, num);
		array[3].z = Mathf.SmoothDamp(array[0].z, array[1].z, ref array[2].z, num);
		if (args.Contains("orienttopath") && (bool)args["orienttopath"])
		{
			args["looktarget"] = array[3];
		}
		if (args.Contains("looktarget"))
		{
			DialogueriTween.LookUpdate(target, args);
		}
		if (flag)
		{
			target.transform.localPosition = array[3];
		}
		else
		{
			target.transform.position = array[3];
		}
		if (target.GetComponent<Rigidbody>() != null)
		{
			Vector3 position2 = target.transform.position;
			target.transform.position = position;
			target.GetComponent<Rigidbody>().MovePosition(position2);
		}
	}

	// Token: 0x06004759 RID: 18265 RVA: 0x002583E1 File Offset: 0x002567E1
	public static void MoveUpdate(GameObject target, Vector3 position, float time)
	{
		DialogueriTween.MoveUpdate(target, DialogueriTween.Hash(new object[]
		{
			"position",
			position,
			"time",
			time
		}));
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x00258418 File Offset: 0x00256818
	public static void LookUpdate(GameObject target, Hashtable args)
	{
		DialogueriTween.CleanArgs(args);
		Vector3[] array = new Vector3[5];
		float num;
		if (args.Contains("looktime"))
		{
			num = (float)args["looktime"];
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else if (args.Contains("time"))
		{
			num = (float)args["time"] * 0.15f;
			num *= DialogueriTween.Defaults.updateTimePercentage;
		}
		else
		{
			num = DialogueriTween.Defaults.updateTime;
		}
		array[0] = target.transform.eulerAngles;
		if (args.Contains("looktarget"))
		{
			if (args["looktarget"].GetType() == typeof(Transform))
			{
				Transform transform = target.transform;
				Transform target2 = (Transform)args["looktarget"];
				Vector3? vector = (Vector3?)args["up"];
				transform.LookAt(target2, (vector == null) ? DialogueriTween.Defaults.up : vector.Value);
			}
			else if (args["looktarget"].GetType() == typeof(Vector3))
			{
				Transform transform2 = target.transform;
				Vector3 worldPosition = (Vector3)args["looktarget"];
				Vector3? vector2 = (Vector3?)args["up"];
				transform2.LookAt(worldPosition, (vector2 == null) ? DialogueriTween.Defaults.up : vector2.Value);
			}
			array[1] = target.transform.eulerAngles;
			target.transform.eulerAngles = array[0];
			array[3].x = Mathf.SmoothDampAngle(array[0].x, array[1].x, ref array[2].x, num);
			array[3].y = Mathf.SmoothDampAngle(array[0].y, array[1].y, ref array[2].y, num);
			array[3].z = Mathf.SmoothDampAngle(array[0].z, array[1].z, ref array[2].z, num);
			target.transform.eulerAngles = array[3];
			if (args.Contains("axis"))
			{
				array[4] = target.transform.eulerAngles;
				string text = (string)args["axis"];
				if (text != null)
				{
					if (!(text == "x"))
					{
						if (!(text == "y"))
						{
							if (text == "z")
							{
								array[4].x = array[0].x;
								array[4].y = array[0].y;
							}
						}
						else
						{
							array[4].x = array[0].x;
							array[4].z = array[0].z;
						}
					}
					else
					{
						array[4].y = array[0].y;
						array[4].z = array[0].z;
					}
				}
				target.transform.eulerAngles = array[4];
			}
			return;
		}
		global::Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!", null);
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x002587BD File Offset: 0x00256BBD
	public static void LookUpdate(GameObject target, Vector3 looktarget, float time)
	{
		DialogueriTween.LookUpdate(target, DialogueriTween.Hash(new object[]
		{
			"looktarget",
			looktarget,
			"time",
			time
		}));
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x002587F4 File Offset: 0x00256BF4
	public static float PathLength(Transform[] path)
	{
		Vector3[] array = new Vector3[path.Length];
		float num = 0f;
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		Vector3[] pts = DialogueriTween.PathControlPointGenerator(array);
		Vector3 a = DialogueriTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int j = 1; j <= num2; j++)
		{
			float t = (float)j / (float)num2;
			Vector3 vector = DialogueriTween.Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x00258890 File Offset: 0x00256C90
	public static float PathLength(Vector3[] path)
	{
		float num = 0f;
		Vector3[] pts = DialogueriTween.PathControlPointGenerator(path);
		Vector3 a = DialogueriTween.Interp(pts, 0f);
		int num2 = path.Length * 20;
		for (int i = 1; i <= num2; i++)
		{
			float t = (float)i / (float)num2;
			Vector3 vector = DialogueriTween.Interp(pts, t);
			num += Vector3.Distance(a, vector);
			a = vector;
		}
		return num;
	}

	// Token: 0x0600475E RID: 18270 RVA: 0x002588F4 File Offset: 0x00256CF4
	public static Texture2D CameraTexture(Color color)
	{
		Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
		Color[] array = new Color[Screen.width * Screen.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = color;
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x0600475F RID: 18271 RVA: 0x00258953 File Offset: 0x00256D53
	public static void PutOnPath(GameObject target, Vector3[] path, float percent)
	{
		target.transform.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06004760 RID: 18272 RVA: 0x0025896C File Offset: 0x00256D6C
	public static void PutOnPath(Transform target, Vector3[] path, float percent)
	{
		target.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x00258980 File Offset: 0x00256D80
	public static void PutOnPath(GameObject target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.transform.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x002589D8 File Offset: 0x00256DD8
	public static void PutOnPath(Transform target, Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		target.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06004763 RID: 18275 RVA: 0x00258A28 File Offset: 0x00256E28
	public static Vector3 PointOnPath(Transform[] path, float percent)
	{
		Vector3[] array = new Vector3[path.Length];
		for (int i = 0; i < path.Length; i++)
		{
			array[i] = path[i].position;
		}
		return DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent);
	}

	// Token: 0x06004764 RID: 18276 RVA: 0x00258A72 File Offset: 0x00256E72
	public static void DrawLine(Vector3[] line)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06004765 RID: 18277 RVA: 0x00258A8D File Offset: 0x00256E8D
	public static void DrawLine(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x06004766 RID: 18278 RVA: 0x00258AA4 File Offset: 0x00256EA4
	public static void DrawLine(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06004767 RID: 18279 RVA: 0x00258AFC File Offset: 0x00256EFC
	public static void DrawLine(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06004768 RID: 18280 RVA: 0x00258B4F File Offset: 0x00256F4F
	public static void DrawLineGizmos(Vector3[] line)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06004769 RID: 18281 RVA: 0x00258B6A File Offset: 0x00256F6A
	public static void DrawLineGizmos(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, color, "gizmos");
		}
	}

	// Token: 0x0600476A RID: 18282 RVA: 0x00258B84 File Offset: 0x00256F84
	public static void DrawLineGizmos(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x0600476B RID: 18283 RVA: 0x00258BDC File Offset: 0x00256FDC
	public static void DrawLineGizmos(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, color, "gizmos");
		}
	}

	// Token: 0x0600476C RID: 18284 RVA: 0x00258C2F File Offset: 0x0025702F
	public static void DrawLineHandles(Vector3[] line)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600476D RID: 18285 RVA: 0x00258C4A File Offset: 0x0025704A
	public static void DrawLineHandles(Vector3[] line, Color color)
	{
		if (line.Length > 0)
		{
			DialogueriTween.DrawLineHelper(line, color, "handles");
		}
	}

	// Token: 0x0600476E RID: 18286 RVA: 0x00258C64 File Offset: 0x00257064
	public static void DrawLineHandles(Transform[] line)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600476F RID: 18287 RVA: 0x00258CBC File Offset: 0x002570BC
	public static void DrawLineHandles(Transform[] line, Color color)
	{
		if (line.Length > 0)
		{
			Vector3[] array = new Vector3[line.Length];
			for (int i = 0; i < line.Length; i++)
			{
				array[i] = line[i].position;
			}
			DialogueriTween.DrawLineHelper(array, color, "handles");
		}
	}

	// Token: 0x06004770 RID: 18288 RVA: 0x00258D0F File Offset: 0x0025710F
	public static Vector3 PointOnPath(Vector3[] path, float percent)
	{
		return DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent);
	}

	// Token: 0x06004771 RID: 18289 RVA: 0x00258D1D File Offset: 0x0025711D
	public static void DrawPath(Vector3[] path)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06004772 RID: 18290 RVA: 0x00258D38 File Offset: 0x00257138
	public static void DrawPath(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06004773 RID: 18291 RVA: 0x00258D50 File Offset: 0x00257150
	public static void DrawPath(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06004774 RID: 18292 RVA: 0x00258DA8 File Offset: 0x002571A8
	public static void DrawPath(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06004775 RID: 18293 RVA: 0x00258DFB File Offset: 0x002571FB
	public static void DrawPathGizmos(Vector3[] path)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06004776 RID: 18294 RVA: 0x00258E16 File Offset: 0x00257216
	public static void DrawPathGizmos(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, color, "gizmos");
		}
	}

	// Token: 0x06004777 RID: 18295 RVA: 0x00258E30 File Offset: 0x00257230
	public static void DrawPathGizmos(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "gizmos");
		}
	}

	// Token: 0x06004778 RID: 18296 RVA: 0x00258E88 File Offset: 0x00257288
	public static void DrawPathGizmos(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, color, "gizmos");
		}
	}

	// Token: 0x06004779 RID: 18297 RVA: 0x00258EDB File Offset: 0x002572DB
	public static void DrawPathHandles(Vector3[] path)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600477A RID: 18298 RVA: 0x00258EF6 File Offset: 0x002572F6
	public static void DrawPathHandles(Vector3[] path, Color color)
	{
		if (path.Length > 0)
		{
			DialogueriTween.DrawPathHelper(path, color, "handles");
		}
	}

	// Token: 0x0600477B RID: 18299 RVA: 0x00258F10 File Offset: 0x00257310
	public static void DrawPathHandles(Transform[] path)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "handles");
		}
	}

	// Token: 0x0600477C RID: 18300 RVA: 0x00258F68 File Offset: 0x00257368
	public static void DrawPathHandles(Transform[] path, Color color)
	{
		if (path.Length > 0)
		{
			Vector3[] array = new Vector3[path.Length];
			for (int i = 0; i < path.Length; i++)
			{
				array[i] = path[i].position;
			}
			DialogueriTween.DrawPathHelper(array, color, "handles");
		}
	}

	// Token: 0x0600477D RID: 18301 RVA: 0x00258FBC File Offset: 0x002573BC
	public static void CameraFadeDepth(int depth)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.cameraFade.transform.position = new Vector3(DialogueriTween.cameraFade.transform.position.x, DialogueriTween.cameraFade.transform.position.y, (float)depth);
		}
	}

	// Token: 0x0600477E RID: 18302 RVA: 0x0025901C File Offset: 0x0025741C
	public static void CameraFadeDestroy()
	{
		if (DialogueriTween.cameraFade)
		{
			UnityEngine.Object.Destroy(DialogueriTween.cameraFade);
		}
	}

	// Token: 0x0600477F RID: 18303 RVA: 0x00259037 File Offset: 0x00257437
	public static void CameraFadeSwap(Texture2D texture)
	{
		if (DialogueriTween.cameraFade)
		{
			DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		}
	}

	// Token: 0x06004780 RID: 18304 RVA: 0x00259058 File Offset: 0x00257458
	public static GameObject CameraFadeAdd(Texture2D texture, int depth)
	{
		if (DialogueriTween.cameraFade)
		{
			return null;
		}
		DialogueriTween.cameraFade = new GameObject("iTween Camera Fade");
		DialogueriTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)depth);
		DialogueriTween.cameraFade.AddComponent<GUITexture>();
		DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		DialogueriTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return DialogueriTween.cameraFade;
	}

	// Token: 0x06004781 RID: 18305 RVA: 0x002590F0 File Offset: 0x002574F0
	public static GameObject CameraFadeAdd(Texture2D texture)
	{
		if (DialogueriTween.cameraFade)
		{
			return null;
		}
		DialogueriTween.cameraFade = new GameObject("iTween Camera Fade");
		DialogueriTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)DialogueriTween.Defaults.cameraFadeDepth);
		DialogueriTween.cameraFade.AddComponent<GUITexture>();
		DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = texture;
		DialogueriTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return DialogueriTween.cameraFade;
	}

	// Token: 0x06004782 RID: 18306 RVA: 0x0025918C File Offset: 0x0025758C
	public static GameObject CameraFadeAdd()
	{
		if (DialogueriTween.cameraFade)
		{
			return null;
		}
		DialogueriTween.cameraFade = new GameObject("iTween Camera Fade");
		DialogueriTween.cameraFade.transform.position = new Vector3(0.5f, 0.5f, (float)DialogueriTween.Defaults.cameraFadeDepth);
		DialogueriTween.cameraFade.AddComponent<GUITexture>();
		DialogueriTween.cameraFade.GetComponent<GUITexture>().texture = DialogueriTween.CameraTexture(Color.black);
		DialogueriTween.cameraFade.GetComponent<GUITexture>().color = new Color(0.5f, 0.5f, 0.5f, 0f);
		return DialogueriTween.cameraFade;
	}

	// Token: 0x06004783 RID: 18307 RVA: 0x00259230 File Offset: 0x00257630
	public static void Resume(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			dialogueriTween.enabled = true;
		}
	}

	// Token: 0x06004784 RID: 18308 RVA: 0x00259274 File Offset: 0x00257674
	public static void Resume(GameObject target, bool includechildren)
	{
		DialogueriTween.Resume(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Resume(transform.gameObject, true);
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
		}
	}

	// Token: 0x06004785 RID: 18309 RVA: 0x002592EC File Offset: 0x002576EC
	public static void Resume(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.enabled = true;
			}
		}
	}

	// Token: 0x06004786 RID: 18310 RVA: 0x0025936C File Offset: 0x0025776C
	public static void Resume(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.enabled = true;
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Resume(transform.gameObject, type, true);
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
		}
	}

	// Token: 0x06004787 RID: 18311 RVA: 0x00259458 File Offset: 0x00257858
	public static void Resume()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			DialogueriTween.Resume(target);
		}
	}

	// Token: 0x06004788 RID: 18312 RVA: 0x002594A8 File Offset: 0x002578A8
	public static void Resume(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.Resume((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06004789 RID: 18313 RVA: 0x00259534 File Offset: 0x00257934
	public static void Pause(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween.delay > 0f)
			{
				dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted;
				dialogueriTween.StopCoroutine("TweenDelay");
			}
			dialogueriTween.isPaused = true;
			dialogueriTween.enabled = false;
		}
	}

	// Token: 0x0600478A RID: 18314 RVA: 0x002595B4 File Offset: 0x002579B4
	public static void Pause(GameObject target, bool includechildren)
	{
		DialogueriTween.Pause(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Pause(transform.gameObject, true);
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
		}
	}

	// Token: 0x0600478B RID: 18315 RVA: 0x0025962C File Offset: 0x00257A2C
	public static void Pause(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (dialogueriTween.delay > 0f)
				{
					dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted;
					dialogueriTween.StopCoroutine("TweenDelay");
				}
				dialogueriTween.isPaused = true;
				dialogueriTween.enabled = false;
			}
		}
	}

	// Token: 0x0600478C RID: 18316 RVA: 0x002596E8 File Offset: 0x00257AE8
	public static void Pause(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				if (dialogueriTween.delay > 0f)
				{
					dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted;
					dialogueriTween.StopCoroutine("TweenDelay");
				}
				dialogueriTween.isPaused = true;
				dialogueriTween.enabled = false;
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Pause(transform.gameObject, type, true);
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
		}
	}

	// Token: 0x0600478D RID: 18317 RVA: 0x00259810 File Offset: 0x00257C10
	public static void Pause()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			DialogueriTween.Pause(target);
		}
	}

	// Token: 0x0600478E RID: 18318 RVA: 0x00259860 File Offset: 0x00257C60
	public static void Pause(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.Pause((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x002598EB File Offset: 0x00257CEB
	public static int Count()
	{
		return DialogueriTween.tweens.Count;
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x002598F8 File Offset: 0x00257CF8
	public static int Count(string type)
	{
		int num = 0;
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			string text = (string)hashtable["type"] + (string)hashtable["method"];
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06004791 RID: 18321 RVA: 0x00259984 File Offset: 0x00257D84
	public static int Count(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		return components.Length;
	}

	// Token: 0x06004792 RID: 18322 RVA: 0x002599A8 File Offset: 0x00257DA8
	public static int Count(GameObject target, string type)
	{
		int num = 0;
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x00259A2C File Offset: 0x00257E2C
	public static void Stop()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject target = (GameObject)hashtable["target"];
			DialogueriTween.Stop(target);
		}
		DialogueriTween.tweens.Clear();
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x00259A88 File Offset: 0x00257E88
	public static void Stop(string type)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.Stop((GameObject)arrayList[j], type);
		}
	}

	// Token: 0x06004795 RID: 18325 RVA: 0x00259B14 File Offset: 0x00257F14
	public static void StopByName(string name)
	{
		ArrayList arrayList = new ArrayList();
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			GameObject value = (GameObject)hashtable["target"];
			arrayList.Insert(arrayList.Count, value);
		}
		for (int j = 0; j < arrayList.Count; j++)
		{
			DialogueriTween.StopByName((GameObject)arrayList[j], name);
		}
	}

	// Token: 0x06004796 RID: 18326 RVA: 0x00259BA0 File Offset: 0x00257FA0
	public static void Stop(GameObject target)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			dialogueriTween.Dispose();
		}
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x00259BE4 File Offset: 0x00257FE4
	public static void Stop(GameObject target, bool includechildren)
	{
		DialogueriTween.Stop(target);
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Stop(transform.gameObject, true);
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
		}
	}

	// Token: 0x06004798 RID: 18328 RVA: 0x00259C5C File Offset: 0x0025805C
	public static void Stop(GameObject target, string type)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.Dispose();
			}
		}
	}

	// Token: 0x06004799 RID: 18329 RVA: 0x00259CDC File Offset: 0x002580DC
	public static void StopByName(GameObject target, string name)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween._name == name)
			{
				dialogueriTween.Dispose();
			}
		}
	}

	// Token: 0x0600479A RID: 18330 RVA: 0x00259D30 File Offset: 0x00258130
	public static void Stop(GameObject target, string type, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			string text = dialogueriTween.type + dialogueriTween.method;
			text = text.Substring(0, type.Length);
			if (text.ToLower() == type.ToLower())
			{
				dialogueriTween.Dispose();
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.Stop(transform.gameObject, type, true);
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
		}
	}

	// Token: 0x0600479B RID: 18331 RVA: 0x00259E1C File Offset: 0x0025821C
	public static void StopByName(GameObject target, string name, bool includechildren)
	{
		Component[] components = target.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween._name == name)
			{
				dialogueriTween.Dispose();
			}
		}
		if (includechildren)
		{
			IEnumerator enumerator = target.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					DialogueriTween.StopByName(transform.gameObject, name, true);
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
		}
	}

	// Token: 0x0600479C RID: 18332 RVA: 0x00259EDC File Offset: 0x002582DC
	public static Hashtable Hash(params object[] args)
	{
		Hashtable hashtable = new Hashtable(args.Length / 2);
		if (args.Length % 2 != 0)
		{
			global::Debug.LogError("Tween Error: Hash requires an even number of arguments!", null);
			return null;
		}
		for (int i = 0; i < args.Length - 1; i += 2)
		{
			hashtable.Add(args[i], args[i + 1]);
		}
		return hashtable;
	}

	// Token: 0x0600479D RID: 18333 RVA: 0x00259F30 File Offset: 0x00258330
	private void Awake()
	{
		this.RetrieveArgs();
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x0600479E RID: 18334 RVA: 0x00259F44 File Offset: 0x00258344
	private IEnumerator Start()
	{
		if (this.delay > 0f)
		{
			yield return base.StartCoroutine("TweenDelay");
		}
		this.TweenStart();
		yield break;
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x00259F60 File Offset: 0x00258360
	private void Update()
	{
		if (this.isRunning && !this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x00259FD8 File Offset: 0x002583D8
	private void FixedUpdate()
	{
		if (this.isRunning && this.physics)
		{
			if (!this.reverse)
			{
				if (this.percentage < 1f)
				{
					this.TweenUpdate();
				}
				else
				{
					this.TweenComplete();
				}
			}
			else if (this.percentage > 0f)
			{
				this.TweenUpdate();
			}
			else
			{
				this.TweenComplete();
			}
		}
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x0025A050 File Offset: 0x00258450
	private void LateUpdate()
	{
		if (this.tweenArguments.Contains("looktarget") && this.isRunning && (this.type == "move" || this.type == "shake" || this.type == "punch"))
		{
			DialogueriTween.LookUpdate(base.gameObject, this.tweenArguments);
		}
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x0025A0D0 File Offset: 0x002584D0
	private void OnEnable()
	{
		if (this.isRunning)
		{
			this.EnableKinematic();
		}
		if (this.isPaused)
		{
			this.isPaused = false;
			if (this.delay > 0f)
			{
				this.wasPaused = true;
				this.ResumeDelay();
			}
		}
	}

	// Token: 0x060047A3 RID: 18339 RVA: 0x0025A11D File Offset: 0x0025851D
	private void OnDisable()
	{
		this.DisableKinematic();
	}

	// Token: 0x060047A4 RID: 18340 RVA: 0x0025A128 File Offset: 0x00258528
	private static void DrawLineHelper(Vector3[] line, Color color, string method)
	{
		Gizmos.color = color;
		for (int i = 0; i < line.Length - 1; i++)
		{
			if (method == "gizmos")
			{
				Gizmos.DrawLine(line[i], line[i + 1]);
			}
			else if (method == "handles")
			{
				global::Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!", null);
			}
		}
	}

	// Token: 0x060047A5 RID: 18341 RVA: 0x0025A1A0 File Offset: 0x002585A0
	private static void DrawPathHelper(Vector3[] path, Color color, string method)
	{
		Vector3[] pts = DialogueriTween.PathControlPointGenerator(path);
		Vector3 to = DialogueriTween.Interp(pts, 0f);
		Gizmos.color = color;
		int num = path.Length * 20;
		for (int i = 1; i <= num; i++)
		{
			float t = (float)i / (float)num;
			Vector3 vector = DialogueriTween.Interp(pts, t);
			if (method == "gizmos")
			{
				Gizmos.DrawLine(vector, to);
			}
			else if (method == "handles")
			{
				global::Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!", null);
			}
			to = vector;
		}
	}

	// Token: 0x060047A6 RID: 18342 RVA: 0x0025A22C File Offset: 0x0025862C
	private static Vector3[] PathControlPointGenerator(Vector3[] path)
	{
		int num = 2;
		Vector3[] array = new Vector3[path.Length + num];
		Array.Copy(path, 0, array, 1, path.Length);
		array[0] = array[1] + (array[1] - array[2]);
		array[array.Length - 1] = array[array.Length - 2] + (array[array.Length - 2] - array[array.Length - 3]);
		if (array[1] == array[array.Length - 2])
		{
			Vector3[] array2 = new Vector3[array.Length];
			Array.Copy(array, array2, array.Length);
			array2[0] = array2[array2.Length - 3];
			array2[array2.Length - 1] = array2[2];
			array = new Vector3[array2.Length];
			Array.Copy(array2, array, array2.Length);
		}
		return array;
	}

	// Token: 0x060047A7 RID: 18343 RVA: 0x0025A360 File Offset: 0x00258760
	private static Vector3 Interp(Vector3[] pts, float t)
	{
		int num = pts.Length - 3;
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float num3 = t * (float)num - (float)num2;
		Vector3 a = pts[num2];
		Vector3 a2 = pts[num2 + 1];
		Vector3 vector = pts[num2 + 2];
		Vector3 b = pts[num2 + 3];
		return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
	}

	// Token: 0x060047A8 RID: 18344 RVA: 0x0025A478 File Offset: 0x00258878
	private static void Launch(GameObject target, Hashtable args)
	{
		if (!args.Contains("id"))
		{
			args["id"] = DialogueriTween.GenerateID();
		}
		if (!args.Contains("target"))
		{
			args["target"] = target;
		}
		DialogueriTween.tweens.Insert(0, args);
		target.AddComponent<DialogueriTween>();
	}

	// Token: 0x060047A9 RID: 18345 RVA: 0x0025A4D4 File Offset: 0x002588D4
	private static Hashtable CleanArgs(Hashtable args)
	{
		Hashtable hashtable = new Hashtable(args.Count);
		Hashtable hashtable2 = new Hashtable(args.Count);
		IDictionaryEnumerator enumerator = args.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value);
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
		IDictionaryEnumerator enumerator2 = hashtable.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
				if (dictionaryEntry2.Value.GetType() == typeof(int))
				{
					int num = (int)dictionaryEntry2.Value;
					float num2 = (float)num;
					args[dictionaryEntry2.Key] = num2;
				}
				if (dictionaryEntry2.Value.GetType() == typeof(double))
				{
					double num3 = (double)dictionaryEntry2.Value;
					float num4 = (float)num3;
					args[dictionaryEntry2.Key] = num4;
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		IDictionaryEnumerator enumerator3 = args.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj3;
				hashtable2.Add(dictionaryEntry3.Key.ToString().ToLower(), dictionaryEntry3.Value);
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator3 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		args = hashtable2;
		return args;
	}

	// Token: 0x060047AA RID: 18346 RVA: 0x0025A6A0 File Offset: 0x00258AA0
	private static string GenerateID()
	{
		int num = 15;
		char[] array = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8'
		};
		int max = array.Length - 1;
		string text = string.Empty;
		for (int i = 0; i < num; i++)
		{
			text += array[(int)Mathf.Floor((float)UnityEngine.Random.Range(0, max))];
		}
		return text;
	}

	// Token: 0x060047AB RID: 18347 RVA: 0x0025A704 File Offset: 0x00258B04
	private void RetrieveArgs()
	{
		IEnumerator enumerator = DialogueriTween.tweens.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Hashtable hashtable = (Hashtable)obj;
				if ((GameObject)hashtable["target"] == base.gameObject)
				{
					this.tweenArguments = hashtable;
					break;
				}
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
		this.id = (string)this.tweenArguments["id"];
		this.type = (string)this.tweenArguments["type"];
		this._name = (string)this.tweenArguments["name"];
		this.method = (string)this.tweenArguments["method"];
		if (this.tweenArguments.Contains("time"))
		{
			this.time = (float)this.tweenArguments["time"];
		}
		else
		{
			this.time = DialogueriTween.Defaults.time;
		}
		if (base.GetComponent<Rigidbody>() != null)
		{
			this.physics = true;
		}
		if (this.tweenArguments.Contains("delay"))
		{
			this.delay = (float)this.tweenArguments["delay"];
		}
		else
		{
			this.delay = DialogueriTween.Defaults.delay;
		}
		if (this.tweenArguments.Contains("namedcolorvalue"))
		{
			if (this.tweenArguments["namedcolorvalue"].GetType() == typeof(DialogueriTween.NamedValueColor))
			{
				this.namedcolorvalue = (DialogueriTween.NamedValueColor)this.tweenArguments["namedcolorvalue"];
			}
			else
			{
				try
				{
					this.namedcolorvalue = (DialogueriTween.NamedValueColor)Enum.Parse(typeof(DialogueriTween.NamedValueColor), (string)this.tweenArguments["namedcolorvalue"], true);
				}
				catch
				{
					this.namedcolorvalue = DialogueriTween.NamedValueColor._Color;
				}
			}
		}
		else
		{
			this.namedcolorvalue = DialogueriTween.Defaults.namedColorValue;
		}
		if (this.tweenArguments.Contains("looptype"))
		{
			if (this.tweenArguments["looptype"].GetType() == typeof(DialogueriTween.LoopType))
			{
				this.loopType = (DialogueriTween.LoopType)this.tweenArguments["looptype"];
			}
			else
			{
				try
				{
					this.loopType = (DialogueriTween.LoopType)Enum.Parse(typeof(DialogueriTween.LoopType), (string)this.tweenArguments["looptype"], true);
				}
				catch
				{
					this.loopType = DialogueriTween.LoopType.none;
				}
			}
		}
		else
		{
			this.loopType = DialogueriTween.LoopType.none;
		}
		if (this.tweenArguments.Contains("easetype"))
		{
			if (this.tweenArguments["easetype"].GetType() == typeof(DialogueriTween.EaseType))
			{
				this.easeType = (DialogueriTween.EaseType)this.tweenArguments["easetype"];
			}
			else
			{
				try
				{
					this.easeType = (DialogueriTween.EaseType)Enum.Parse(typeof(DialogueriTween.EaseType), (string)this.tweenArguments["easetype"], true);
				}
				catch
				{
					this.easeType = DialogueriTween.Defaults.easeType;
				}
			}
		}
		else
		{
			this.easeType = DialogueriTween.Defaults.easeType;
		}
		if (this.tweenArguments.Contains("space"))
		{
			if (this.tweenArguments["space"].GetType() == typeof(Space))
			{
				this.space = (Space)this.tweenArguments["space"];
			}
			else
			{
				try
				{
					this.space = (Space)Enum.Parse(typeof(Space), (string)this.tweenArguments["space"], true);
				}
				catch
				{
					this.space = DialogueriTween.Defaults.space;
				}
			}
		}
		else
		{
			this.space = DialogueriTween.Defaults.space;
		}
		if (this.tweenArguments.Contains("islocal"))
		{
			this.isLocal = (bool)this.tweenArguments["islocal"];
		}
		else
		{
			this.isLocal = DialogueriTween.Defaults.isLocal;
		}
		if (this.tweenArguments.Contains("ignoretimescale"))
		{
			this.useRealTime = (bool)this.tweenArguments["ignoretimescale"];
		}
		else
		{
			this.useRealTime = DialogueriTween.Defaults.useRealTime;
		}
		this.GetEasingFunction();
	}

	// Token: 0x060047AC RID: 18348 RVA: 0x0025ABF8 File Offset: 0x00258FF8
	private void GetEasingFunction()
	{
		switch (this.easeType)
		{
		case DialogueriTween.EaseType.easeInQuad:
			this.ease = new DialogueriTween.EasingFunction(this.easeInQuad);
			break;
		case DialogueriTween.EaseType.easeOutQuad:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutQuad);
			break;
		case DialogueriTween.EaseType.easeInOutQuad:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutQuad);
			break;
		case DialogueriTween.EaseType.easeInCubic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInCubic);
			break;
		case DialogueriTween.EaseType.easeOutCubic:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutCubic);
			break;
		case DialogueriTween.EaseType.easeInOutCubic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutCubic);
			break;
		case DialogueriTween.EaseType.easeInQuart:
			this.ease = new DialogueriTween.EasingFunction(this.easeInQuart);
			break;
		case DialogueriTween.EaseType.easeOutQuart:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutQuart);
			break;
		case DialogueriTween.EaseType.easeInOutQuart:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutQuart);
			break;
		case DialogueriTween.EaseType.easeInQuint:
			this.ease = new DialogueriTween.EasingFunction(this.easeInQuint);
			break;
		case DialogueriTween.EaseType.easeOutQuint:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutQuint);
			break;
		case DialogueriTween.EaseType.easeInOutQuint:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutQuint);
			break;
		case DialogueriTween.EaseType.easeInSine:
			this.ease = new DialogueriTween.EasingFunction(this.easeInSine);
			break;
		case DialogueriTween.EaseType.easeOutSine:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutSine);
			break;
		case DialogueriTween.EaseType.easeInOutSine:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutSine);
			break;
		case DialogueriTween.EaseType.easeInExpo:
			this.ease = new DialogueriTween.EasingFunction(this.easeInExpo);
			break;
		case DialogueriTween.EaseType.easeOutExpo:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutExpo);
			break;
		case DialogueriTween.EaseType.easeInOutExpo:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutExpo);
			break;
		case DialogueriTween.EaseType.easeInCirc:
			this.ease = new DialogueriTween.EasingFunction(this.easeInCirc);
			break;
		case DialogueriTween.EaseType.easeOutCirc:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutCirc);
			break;
		case DialogueriTween.EaseType.easeInOutCirc:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutCirc);
			break;
		case DialogueriTween.EaseType.linear:
			this.ease = new DialogueriTween.EasingFunction(this.linear);
			break;
		case DialogueriTween.EaseType.spring:
			this.ease = new DialogueriTween.EasingFunction(this.spring);
			break;
		case DialogueriTween.EaseType.easeInBounce:
			this.ease = new DialogueriTween.EasingFunction(this.easeInBounce);
			break;
		case DialogueriTween.EaseType.easeOutBounce:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutBounce);
			break;
		case DialogueriTween.EaseType.easeInOutBounce:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutBounce);
			break;
		case DialogueriTween.EaseType.easeInBack:
			this.ease = new DialogueriTween.EasingFunction(this.easeInBack);
			break;
		case DialogueriTween.EaseType.easeOutBack:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutBack);
			break;
		case DialogueriTween.EaseType.easeInOutBack:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutBack);
			break;
		case DialogueriTween.EaseType.easeInElastic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInElastic);
			break;
		case DialogueriTween.EaseType.easeOutElastic:
			this.ease = new DialogueriTween.EasingFunction(this.easeOutElastic);
			break;
		case DialogueriTween.EaseType.easeInOutElastic:
			this.ease = new DialogueriTween.EasingFunction(this.easeInOutElastic);
			break;
		}
	}

	// Token: 0x060047AD RID: 18349 RVA: 0x0025AF78 File Offset: 0x00259378
	private void UpdatePercentage()
	{
		if (this.useRealTime)
		{
			this.runningTime += Time.realtimeSinceStartup - this.lastRealTime;
		}
		else
		{
			this.runningTime += Time.deltaTime;
		}
		if (this.reverse)
		{
			this.percentage = 1f - this.runningTime / this.time;
		}
		else
		{
			this.percentage = this.runningTime / this.time;
		}
		this.lastRealTime = Time.realtimeSinceStartup;
	}

	// Token: 0x060047AE RID: 18350 RVA: 0x0025B008 File Offset: 0x00259408
	private void CallBack(string callbackType)
	{
		if (this.tweenArguments.Contains(callbackType) && !this.tweenArguments.Contains("ischild"))
		{
			GameObject gameObject;
			if (this.tweenArguments.Contains(callbackType + "target"))
			{
				gameObject = (GameObject)this.tweenArguments[callbackType + "target"];
			}
			else
			{
				gameObject = base.gameObject;
			}
			if (this.tweenArguments[callbackType].GetType() == typeof(string))
			{
				gameObject.SendMessage((string)this.tweenArguments[callbackType], this.tweenArguments[callbackType + "params"], SendMessageOptions.DontRequireReceiver);
			}
			else
			{
				global::Debug.LogError("iTween Error: Callback method references must be passed as a String!", null);
				UnityEngine.Object.Destroy(this);
			}
		}
	}

	// Token: 0x060047AF RID: 18351 RVA: 0x0025B0E4 File Offset: 0x002594E4
	private void Dispose()
	{
		for (int i = 0; i < DialogueriTween.tweens.Count; i++)
		{
			Hashtable hashtable = (Hashtable)DialogueriTween.tweens[i];
			if ((string)hashtable["id"] == this.id)
			{
				DialogueriTween.tweens.RemoveAt(i);
				break;
			}
		}
		UnityEngine.Object.Destroy(this);
	}

	// Token: 0x060047B0 RID: 18352 RVA: 0x0025B154 File Offset: 0x00259554
	private void ConflictCheck()
	{
		Component[] components = base.GetComponents(typeof(DialogueriTween));
		foreach (DialogueriTween dialogueriTween in components)
		{
			if (dialogueriTween.type == "value")
			{
				return;
			}
			if (dialogueriTween.isRunning && dialogueriTween.type == this.type)
			{
				if (dialogueriTween.method != this.method)
				{
					return;
				}
				if (dialogueriTween.tweenArguments.Count != this.tweenArguments.Count)
				{
					dialogueriTween.Dispose();
					return;
				}
				IDictionaryEnumerator enumerator = this.tweenArguments.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
						if (!dialogueriTween.tweenArguments.Contains(dictionaryEntry.Key))
						{
							dialogueriTween.Dispose();
							return;
						}
						if (!dialogueriTween.tweenArguments[dictionaryEntry.Key].Equals(this.tweenArguments[dictionaryEntry.Key]) && (string)dictionaryEntry.Key != "id")
						{
							dialogueriTween.Dispose();
							return;
						}
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
				this.Dispose();
			}
		}
	}

	// Token: 0x060047B1 RID: 18353 RVA: 0x0025B2D8 File Offset: 0x002596D8
	private void EnableKinematic()
	{
	}

	// Token: 0x060047B2 RID: 18354 RVA: 0x0025B2DA File Offset: 0x002596DA
	private void DisableKinematic()
	{
	}

	// Token: 0x060047B3 RID: 18355 RVA: 0x0025B2DC File Offset: 0x002596DC
	private void ResumeDelay()
	{
		base.StartCoroutine("TweenDelay");
	}

	// Token: 0x060047B4 RID: 18356 RVA: 0x0025B2EA File Offset: 0x002596EA
	private float linear(float start, float end, float value)
	{
		return Mathf.Lerp(start, end, value);
	}

	// Token: 0x060047B5 RID: 18357 RVA: 0x0025B2F4 File Offset: 0x002596F4
	private float clerp(float start, float end, float value)
	{
		float num = 0f;
		float num2 = 360f;
		float num3 = Mathf.Abs((num2 - num) / 2f);
		float result;
		if (end - start < -num3)
		{
			float num4 = (num2 - start + end) * value;
			result = start + num4;
		}
		else if (end - start > num3)
		{
			float num4 = -(num2 - end + start) * value;
			result = start + num4;
		}
		else
		{
			result = start + (end - start) * value;
		}
		return result;
	}

	// Token: 0x060047B6 RID: 18358 RVA: 0x0025B36C File Offset: 0x0025976C
	private float spring(float start, float end, float value)
	{
		value = Mathf.Clamp01(value);
		value = (Mathf.Sin(value * 3.1415927f * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + 1.2f * (1f - value));
		return start + (end - start) * value;
	}

	// Token: 0x060047B7 RID: 18359 RVA: 0x0025B3D0 File Offset: 0x002597D0
	private float easeInQuad(float start, float end, float value)
	{
		end -= start;
		return end * value * value + start;
	}

	// Token: 0x060047B8 RID: 18360 RVA: 0x0025B3DE File Offset: 0x002597DE
	private float easeOutQuad(float start, float end, float value)
	{
		end -= start;
		return -end * value * (value - 2f) + start;
	}

	// Token: 0x060047B9 RID: 18361 RVA: 0x0025B3F4 File Offset: 0x002597F4
	private float easeInOutQuad(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value + start;
		}
		value -= 1f;
		return -end / 2f * (value * (value - 2f) - 1f) + start;
	}

	// Token: 0x060047BA RID: 18362 RVA: 0x0025B44B File Offset: 0x0025984B
	private float easeInCubic(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value + start;
	}

	// Token: 0x060047BB RID: 18363 RVA: 0x0025B45B File Offset: 0x0025985B
	private float easeOutCubic(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value + 1f) + start;
	}

	// Token: 0x060047BC RID: 18364 RVA: 0x0025B47C File Offset: 0x0025987C
	private float easeInOutCubic(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value + 2f) + start;
	}

	// Token: 0x060047BD RID: 18365 RVA: 0x0025B4D0 File Offset: 0x002598D0
	private float easeInQuart(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value + start;
	}

	// Token: 0x060047BE RID: 18366 RVA: 0x0025B4E2 File Offset: 0x002598E2
	private float easeOutQuart(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return -end * (value * value * value * value - 1f) + start;
	}

	// Token: 0x060047BF RID: 18367 RVA: 0x0025B504 File Offset: 0x00259904
	private float easeInOutQuart(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value + start;
		}
		value -= 2f;
		return -end / 2f * (value * value * value * value - 2f) + start;
	}

	// Token: 0x060047C0 RID: 18368 RVA: 0x0025B55D File Offset: 0x0025995D
	private float easeInQuint(float start, float end, float value)
	{
		end -= start;
		return end * value * value * value * value * value + start;
	}

	// Token: 0x060047C1 RID: 18369 RVA: 0x0025B571 File Offset: 0x00259971
	private float easeOutQuint(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * (value * value * value * value * value + 1f) + start;
	}

	// Token: 0x060047C2 RID: 18370 RVA: 0x0025B594 File Offset: 0x00259994
	private float easeInOutQuint(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * value * value * value * value * value + start;
		}
		value -= 2f;
		return end / 2f * (value * value * value * value * value + 2f) + start;
	}

	// Token: 0x060047C3 RID: 18371 RVA: 0x0025B5F0 File Offset: 0x002599F0
	private float easeInSine(float start, float end, float value)
	{
		end -= start;
		return -end * Mathf.Cos(value / 1f * 1.5707964f) + end + start;
	}

	// Token: 0x060047C4 RID: 18372 RVA: 0x0025B610 File Offset: 0x00259A10
	private float easeOutSine(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Sin(value / 1f * 1.5707964f) + start;
	}

	// Token: 0x060047C5 RID: 18373 RVA: 0x0025B62D File Offset: 0x00259A2D
	private float easeInOutSine(float start, float end, float value)
	{
		end -= start;
		return -end / 2f * (Mathf.Cos(3.1415927f * value / 1f) - 1f) + start;
	}

	// Token: 0x060047C6 RID: 18374 RVA: 0x0025B657 File Offset: 0x00259A57
	private float easeInExpo(float start, float end, float value)
	{
		end -= start;
		return end * Mathf.Pow(2f, 10f * (value / 1f - 1f)) + start;
	}

	// Token: 0x060047C7 RID: 18375 RVA: 0x0025B67F File Offset: 0x00259A7F
	private float easeOutExpo(float start, float end, float value)
	{
		end -= start;
		return end * (-Mathf.Pow(2f, -10f * value / 1f) + 1f) + start;
	}

	// Token: 0x060047C8 RID: 18376 RVA: 0x0025B6A8 File Offset: 0x00259AA8
	private float easeInOutExpo(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return end / 2f * Mathf.Pow(2f, 10f * (value - 1f)) + start;
		}
		value -= 1f;
		return end / 2f * (-Mathf.Pow(2f, -10f * value) + 2f) + start;
	}

	// Token: 0x060047C9 RID: 18377 RVA: 0x0025B71B File Offset: 0x00259B1B
	private float easeInCirc(float start, float end, float value)
	{
		end -= start;
		return -end * (Mathf.Sqrt(1f - value * value) - 1f) + start;
	}

	// Token: 0x060047CA RID: 18378 RVA: 0x0025B73B File Offset: 0x00259B3B
	private float easeOutCirc(float start, float end, float value)
	{
		value -= 1f;
		end -= start;
		return end * Mathf.Sqrt(1f - value * value) + start;
	}

	// Token: 0x060047CB RID: 18379 RVA: 0x0025B760 File Offset: 0x00259B60
	private float easeInOutCirc(float start, float end, float value)
	{
		value /= 0.5f;
		end -= start;
		if (value < 1f)
		{
			return -end / 2f * (Mathf.Sqrt(1f - value * value) - 1f) + start;
		}
		value -= 2f;
		return end / 2f * (Mathf.Sqrt(1f - value * value) + 1f) + start;
	}

	// Token: 0x060047CC RID: 18380 RVA: 0x0025B7D0 File Offset: 0x00259BD0
	private float easeInBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		return end - this.easeOutBounce(0f, end, num - value) + start;
	}

	// Token: 0x060047CD RID: 18381 RVA: 0x0025B7FC File Offset: 0x00259BFC
	private float easeOutBounce(float start, float end, float value)
	{
		value /= 1f;
		end -= start;
		if (value < 0.36363637f)
		{
			return end * (7.5625f * value * value) + start;
		}
		if (value < 0.72727275f)
		{
			value -= 0.54545456f;
			return end * (7.5625f * value * value + 0.75f) + start;
		}
		if ((double)value < 0.9090909090909091)
		{
			value -= 0.8181818f;
			return end * (7.5625f * value * value + 0.9375f) + start;
		}
		value -= 0.95454544f;
		return end * (7.5625f * value * value + 0.984375f) + start;
	}

	// Token: 0x060047CE RID: 18382 RVA: 0x0025B8A4 File Offset: 0x00259CA4
	private float easeInOutBounce(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		if (value < num / 2f)
		{
			return this.easeInBounce(0f, end, value * 2f) * 0.5f + start;
		}
		return this.easeOutBounce(0f, end, value * 2f - num) * 0.5f + end * 0.5f + start;
	}

	// Token: 0x060047CF RID: 18383 RVA: 0x0025B90C File Offset: 0x00259D0C
	private float easeInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x060047D0 RID: 18384 RVA: 0x0025B940 File Offset: 0x00259D40
	private float easeOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value = value / 1f - 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x060047D1 RID: 18385 RVA: 0x0025B980 File Offset: 0x00259D80
	private float easeInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end / 2f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end / 2f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x060047D2 RID: 18386 RVA: 0x0025BA00 File Offset: 0x00259E00
	private float punch(float amplitude, float value)
	{
		if (value == 0f)
		{
			return 0f;
		}
		if (value == 1f)
		{
			return 0f;
		}
		float num = 0.3f;
		float num2 = num / 6.2831855f * Mathf.Asin(0f);
		return amplitude * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * 1f - num2) * 6.2831855f / num);
	}

	// Token: 0x060047D3 RID: 18387 RVA: 0x0025BA78 File Offset: 0x00259E78
	private float easeInElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return -(num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
	}

	// Token: 0x060047D4 RID: 18388 RVA: 0x0025BB30 File Offset: 0x00259F30
	private float easeOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num) == 1f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		return num3 * Mathf.Pow(2f, -10f * value) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) + end + start;
	}

	// Token: 0x060047D5 RID: 18389 RVA: 0x0025BBE0 File Offset: 0x00259FE0
	private float easeInOutElastic(float start, float end, float value)
	{
		end -= start;
		float num = 1f;
		float num2 = num * 0.3f;
		float num3 = 0f;
		if (value == 0f)
		{
			return start;
		}
		if ((value /= num / 2f) == 2f)
		{
			return start + end;
		}
		float num4;
		if (num3 == 0f || num3 < Mathf.Abs(end))
		{
			num3 = end;
			num4 = num2 / 4f;
		}
		else
		{
			num4 = num2 / 6.2831855f * Mathf.Asin(end / num3);
		}
		if (value < 1f)
		{
			return -0.5f * (num3 * Mathf.Pow(2f, 10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2)) + start;
		}
		return num3 * Mathf.Pow(2f, -10f * (value -= 1f)) * Mathf.Sin((value * num - num4) * 6.2831855f / num2) * 0.5f + end + start;
	}

	// Token: 0x04004CBA RID: 19642
	public static ArrayList tweens = new ArrayList();

	// Token: 0x04004CBB RID: 19643
	private static GameObject cameraFade;

	// Token: 0x04004CBC RID: 19644
	public string id;

	// Token: 0x04004CBD RID: 19645
	public string type;

	// Token: 0x04004CBE RID: 19646
	public string method;

	// Token: 0x04004CBF RID: 19647
	public DialogueriTween.EaseType easeType;

	// Token: 0x04004CC0 RID: 19648
	public float time;

	// Token: 0x04004CC1 RID: 19649
	public float delay;

	// Token: 0x04004CC2 RID: 19650
	public DialogueriTween.LoopType loopType;

	// Token: 0x04004CC3 RID: 19651
	public bool isRunning;

	// Token: 0x04004CC4 RID: 19652
	public bool isPaused;

	// Token: 0x04004CC5 RID: 19653
	public string _name;

	// Token: 0x04004CC6 RID: 19654
	private float runningTime;

	// Token: 0x04004CC7 RID: 19655
	private float percentage;

	// Token: 0x04004CC8 RID: 19656
	private float delayStarted;

	// Token: 0x04004CC9 RID: 19657
	private bool kinematic;

	// Token: 0x04004CCA RID: 19658
	private bool isLocal;

	// Token: 0x04004CCB RID: 19659
	private bool loop;

	// Token: 0x04004CCC RID: 19660
	private bool reverse;

	// Token: 0x04004CCD RID: 19661
	private bool wasPaused;

	// Token: 0x04004CCE RID: 19662
	private bool physics;

	// Token: 0x04004CCF RID: 19663
	private Hashtable tweenArguments;

	// Token: 0x04004CD0 RID: 19664
	private Space space;

	// Token: 0x04004CD1 RID: 19665
	private DialogueriTween.EasingFunction ease;

	// Token: 0x04004CD2 RID: 19666
	private DialogueriTween.ApplyTween apply;

	// Token: 0x04004CD3 RID: 19667
	private AudioSource audioSource;

	// Token: 0x04004CD4 RID: 19668
	private Vector3[] vector3s;

	// Token: 0x04004CD5 RID: 19669
	private Vector2[] vector2s;

	// Token: 0x04004CD6 RID: 19670
	private Color[,] colors;

	// Token: 0x04004CD7 RID: 19671
	private float[] floats;

	// Token: 0x04004CD8 RID: 19672
	private Rect[] rects;

	// Token: 0x04004CD9 RID: 19673
	private DialogueriTween.CRSpline path;

	// Token: 0x04004CDA RID: 19674
	private Vector3 preUpdate;

	// Token: 0x04004CDB RID: 19675
	private Vector3 postUpdate;

	// Token: 0x04004CDC RID: 19676
	private DialogueriTween.NamedValueColor namedcolorvalue;

	// Token: 0x04004CDD RID: 19677
	private float lastRealTime;

	// Token: 0x04004CDE RID: 19678
	private bool useRealTime;

	// Token: 0x02000B84 RID: 2948
	// (Invoke) Token: 0x060047D8 RID: 18392
	private delegate float EasingFunction(float start, float end, float value);

	// Token: 0x02000B85 RID: 2949
	// (Invoke) Token: 0x060047DC RID: 18396
	private delegate void ApplyTween();

	// Token: 0x02000B86 RID: 2950
	public enum EaseType
	{
		// Token: 0x04004CE1 RID: 19681
		easeInQuad,
		// Token: 0x04004CE2 RID: 19682
		easeOutQuad,
		// Token: 0x04004CE3 RID: 19683
		easeInOutQuad,
		// Token: 0x04004CE4 RID: 19684
		easeInCubic,
		// Token: 0x04004CE5 RID: 19685
		easeOutCubic,
		// Token: 0x04004CE6 RID: 19686
		easeInOutCubic,
		// Token: 0x04004CE7 RID: 19687
		easeInQuart,
		// Token: 0x04004CE8 RID: 19688
		easeOutQuart,
		// Token: 0x04004CE9 RID: 19689
		easeInOutQuart,
		// Token: 0x04004CEA RID: 19690
		easeInQuint,
		// Token: 0x04004CEB RID: 19691
		easeOutQuint,
		// Token: 0x04004CEC RID: 19692
		easeInOutQuint,
		// Token: 0x04004CED RID: 19693
		easeInSine,
		// Token: 0x04004CEE RID: 19694
		easeOutSine,
		// Token: 0x04004CEF RID: 19695
		easeInOutSine,
		// Token: 0x04004CF0 RID: 19696
		easeInExpo,
		// Token: 0x04004CF1 RID: 19697
		easeOutExpo,
		// Token: 0x04004CF2 RID: 19698
		easeInOutExpo,
		// Token: 0x04004CF3 RID: 19699
		easeInCirc,
		// Token: 0x04004CF4 RID: 19700
		easeOutCirc,
		// Token: 0x04004CF5 RID: 19701
		easeInOutCirc,
		// Token: 0x04004CF6 RID: 19702
		linear,
		// Token: 0x04004CF7 RID: 19703
		spring,
		// Token: 0x04004CF8 RID: 19704
		easeInBounce,
		// Token: 0x04004CF9 RID: 19705
		easeOutBounce,
		// Token: 0x04004CFA RID: 19706
		easeInOutBounce,
		// Token: 0x04004CFB RID: 19707
		easeInBack,
		// Token: 0x04004CFC RID: 19708
		easeOutBack,
		// Token: 0x04004CFD RID: 19709
		easeInOutBack,
		// Token: 0x04004CFE RID: 19710
		easeInElastic,
		// Token: 0x04004CFF RID: 19711
		easeOutElastic,
		// Token: 0x04004D00 RID: 19712
		easeInOutElastic,
		// Token: 0x04004D01 RID: 19713
		punch
	}

	// Token: 0x02000B87 RID: 2951
	public enum LoopType
	{
		// Token: 0x04004D03 RID: 19715
		none,
		// Token: 0x04004D04 RID: 19716
		loop,
		// Token: 0x04004D05 RID: 19717
		pingPong
	}

	// Token: 0x02000B88 RID: 2952
	public enum NamedValueColor
	{
		// Token: 0x04004D07 RID: 19719
		_Color,
		// Token: 0x04004D08 RID: 19720
		_SpecColor,
		// Token: 0x04004D09 RID: 19721
		_Emission,
		// Token: 0x04004D0A RID: 19722
		_ReflectColor
	}

	// Token: 0x02000B89 RID: 2953
	public static class Defaults
	{
		// Token: 0x04004D0B RID: 19723
		public static float time = 1f;

		// Token: 0x04004D0C RID: 19724
		public static float delay = 0f;

		// Token: 0x04004D0D RID: 19725
		public static DialogueriTween.NamedValueColor namedColorValue = DialogueriTween.NamedValueColor._Color;

		// Token: 0x04004D0E RID: 19726
		public static DialogueriTween.LoopType loopType = DialogueriTween.LoopType.none;

		// Token: 0x04004D0F RID: 19727
		public static DialogueriTween.EaseType easeType = DialogueriTween.EaseType.easeOutExpo;

		// Token: 0x04004D10 RID: 19728
		public static float lookSpeed = 3f;

		// Token: 0x04004D11 RID: 19729
		public static bool isLocal = false;

		// Token: 0x04004D12 RID: 19730
		public static Space space = Space.Self;

		// Token: 0x04004D13 RID: 19731
		public static bool orientToPath = false;

		// Token: 0x04004D14 RID: 19732
		public static Color color = Color.white;

		// Token: 0x04004D15 RID: 19733
		public static float updateTimePercentage = 0.05f;

		// Token: 0x04004D16 RID: 19734
		public static float updateTime = 1f * DialogueriTween.Defaults.updateTimePercentage;

		// Token: 0x04004D17 RID: 19735
		public static int cameraFadeDepth = 999999;

		// Token: 0x04004D18 RID: 19736
		public static float lookAhead = 0.05f;

		// Token: 0x04004D19 RID: 19737
		public static bool useRealTime = false;

		// Token: 0x04004D1A RID: 19738
		public static Vector3 up = Vector3.up;
	}

	// Token: 0x02000B8A RID: 2954
	private class CRSpline
	{
		// Token: 0x060047E0 RID: 18400 RVA: 0x0025BD8C File Offset: 0x0025A18C
		public CRSpline(params Vector3[] pts)
		{
			this.pts = new Vector3[pts.Length];
			Array.Copy(pts, this.pts, pts.Length);
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x0025BDB4 File Offset: 0x0025A1B4
		public Vector3 Interp(float t)
		{
			int num = this.pts.Length - 3;
			int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
			float num3 = t * (float)num - (float)num2;
			Vector3 a = this.pts[num2];
			Vector3 a2 = this.pts[num2 + 1];
			Vector3 vector = this.pts[num2 + 2];
			Vector3 b = this.pts[num2 + 3];
			return 0.5f * ((-a + 3f * a2 - 3f * vector + b) * (num3 * num3 * num3) + (2f * a - 5f * a2 + 4f * vector - b) * (num3 * num3) + (-a + vector) * num3 + 2f * a2);
		}

		// Token: 0x04004D1B RID: 19739
		public Vector3[] pts;
	}
}
