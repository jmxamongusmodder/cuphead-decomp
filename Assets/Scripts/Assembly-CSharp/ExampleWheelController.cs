using System;
using UnityEngine;

// Token: 0x02000C03 RID: 3075
public class ExampleWheelController : MonoBehaviour
{
	// Token: 0x06004966 RID: 18790 RVA: 0x00265A5D File Offset: 0x00263E5D
	private void Start()
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		this.m_Rigidbody.maxAngularVelocity = 100f;
	}

	// Token: 0x06004967 RID: 18791 RVA: 0x00265A7C File Offset: 0x00263E7C
	private void Update()
	{
		if (Input.GetKey(KeyCode.UpArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(-1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			this.m_Rigidbody.AddRelativeTorque(new Vector3(1f * this.acceleration, 0f, 0f), ForceMode.Acceleration);
		}
		float value = -this.m_Rigidbody.angularVelocity.x / 100f;
		if (this.motionVectorRenderer)
		{
			this.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(value, -0.25f, 0.25f));
		}
	}

	// Token: 0x04004F7D RID: 20349
	public float acceleration;

	// Token: 0x04004F7E RID: 20350
	public Renderer motionVectorRenderer;

	// Token: 0x04004F7F RID: 20351
	private Rigidbody m_Rigidbody;

	// Token: 0x02000C04 RID: 3076
	private static class Uniforms
	{
		// Token: 0x04004F80 RID: 20352
		internal static readonly int _MotionAmount = Shader.PropertyToID("_MotionAmount");
	}
}
