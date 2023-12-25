using System;
using System.Runtime.InteropServices;

// Token: 0x020009CF RID: 2511
public static class PlayStation4Utilities
{
	// Token: 0x06003AF5 RID: 15093
	[DllImport("GetParam")]
	private static extern int get_system_service_param(int param, out int value);

	// Token: 0x06003AF6 RID: 15094 RVA: 0x00212B84 File Offset: 0x00210F84
	public static int GetSystemServiceParam(int param)
	{
		int result;
		int num = PlayStation4Utilities.get_system_service_param(param, out result);
		if (num != PlayStation4Utilities.SCE_OK)
		{
			throw new Exception("Error getting param. Result code: " + num);
		}
		return result;
	}

	// Token: 0x040042B1 RID: 17073
	public static readonly int SCE_OK;

	// Token: 0x040042B2 RID: 17074
	public static readonly int SCE_SYSTEM_SERVICE_PARAM_ID_ENTER_BUTTON_ASSIGN = 1000;

	// Token: 0x040042B3 RID: 17075
	public static readonly int SCE_SYSTEM_PARAM_ENTER_BUTTON_ASSIGN_CIRCLE;

	// Token: 0x040042B4 RID: 17076
	public static readonly int SCE_SYSTEM_PARAM_ENTER_BUTTON_ASSIGN_CROSS = 1;
}
