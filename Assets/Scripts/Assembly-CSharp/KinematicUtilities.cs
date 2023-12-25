using System;
using UnityEngine;

// Token: 0x02000394 RID: 916
public static class KinematicUtilities
{
	// Token: 0x06000B07 RID: 2823 RVA: 0x00081CB0 File Offset: 0x000800B0
	public static float CalculateAcceleration(float distance, float finalSpeed)
	{
		return 0.5f * finalSpeed * finalSpeed / distance;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x00081CBD File Offset: 0x000800BD
	public static float CalculateTimeToSpeed(float distance, float finalSpeed)
	{
		return 2f * distance / finalSpeed;
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x00081CC8 File Offset: 0x000800C8
	public static float CalculateTimeToTravelDistance(float distance, float speed)
	{
		return distance / speed;
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x00081CCD File Offset: 0x000800CD
	public static float CalculateVelocityFromZero(float distance, float time)
	{
		return 2f * distance / time;
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x00081CD8 File Offset: 0x000800D8
	public static float CalculateAccelerationFromZero(float distance, float time)
	{
		float num = time * time;
		return 2f * distance / num;
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x00081CF2 File Offset: 0x000800F2
	public static float CalculateTimeToChangeVelocity(float v1, float v2, float distance)
	{
		return 2f * distance / (v1 + v2);
	}

	// Token: 0x06000B0D RID: 2829 RVA: 0x00081CFF File Offset: 0x000800FF
	public static float CalculateInitialSpeedToReachApex(float apexHeight, float gravity)
	{
		return Mathf.Sqrt(2f * gravity * apexHeight);
	}

	// Token: 0x06000B0E RID: 2830 RVA: 0x00081D0F File Offset: 0x0008010F
	public static float CalculateDistanceTravelled(Vector2 initialVelocity, float startingHeight, float gravity)
	{
		return initialVelocity.x / gravity * (initialVelocity.y + Mathf.Sqrt(initialVelocity.y * initialVelocity.y + 2f * gravity * startingHeight));
	}

	// Token: 0x06000B0F RID: 2831 RVA: 0x00081D41 File Offset: 0x00080141
	public static float CalculateHorizontalSpeedToTravelDistance(float distance, float velocityY, float startingHeight, float gravity)
	{
		return distance * gravity / (velocityY + Mathf.Sqrt(velocityY * velocityY + 2f * gravity * startingHeight));
	}
}
