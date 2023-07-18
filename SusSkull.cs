using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UMM;

namespace SusSkull
{
    [UKPlugin("radsi.ultrakillsuskull", "SusKull", "1.1.1", "Replaces the skulls with among us", false, false)]
    public class amongusSkulls : UKMod
    {
        public static Dictionary<string, GameObject> allAmongus = new Dictionary<string, GameObject>();
		private Harmony harmony = new Harmony("radsi.ultrakillsuskull");
        public static AssetBundle amongusbundle;

		public override void OnModLoaded()
        {
            amongusbundle = AssetBundle.LoadFromMemory(Resource1.suskull);
            amongusbundle.LoadAllAssets();
			harmony.PatchAll();

            allAmongus.Add("AmongUsBlue", amongusbundle.LoadAsset<GameObject>("AmongUsBlueGO"));
            allAmongus.Add("AmongUsRed", amongusbundle.LoadAsset<GameObject>("AmongUsRedGO"));
            allAmongus.Add("AmongUsYellow", amongusbundle.LoadAsset<GameObject>("AmongUsYellowGO"));
            allAmongus.Add("AmongUsWhite", amongusbundle.LoadAsset<GameObject>("AmongUsWhiteGO"));
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002140 File Offset: 0x00000340
		public override void OnModUnload()
		{
			allAmongus.Clear();
			harmony.UnpatchSelf();
			harmony = null;
			amongusbundle.Unload(true);
			amongusbundle = null;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002170 File Offset: 0x00000370
		public static void CreateFumo(string amongusType, Transform masterSkull, Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Debug.Log("Swapping " + masterSkull.name + " to " + amongusType);
			GameObject gameObject = Object.Instantiate<GameObject>(allAmongus[amongusType], masterSkull);
			gameObject.active = true;
			gameObject.transform.localRotation = rotation;
			gameObject.transform.localPosition = position;
			gameObject.transform.localScale = scale;
		}

		// Token: 0x02000007 RID: 7
		[HarmonyPatch(typeof(Skull), "Start")]
		public static class amongufiyskull
		{
			// Token: 0x0600000D RID: 13 RVA: 0x00002284 File Offset: 0x00000484
			public static void Prefix(Skull __instance)
			{
				Renderer component = __instance.gameObject.GetComponent<Renderer>();
				if (component)
				{
					Vector3 position = new Vector3(0.05f, 0f, 0.2f);
					Quaternion rotation = Quaternion.Euler(0f, 20f, 0f);
					Vector3 scale = new Vector3(1.5f, 1.5f, 1.5f);
					ItemType itemType = __instance.GetComponent<ItemIdentifier>().itemType;
					string amongusType;
					if (itemType != ItemType.SkullBlue)
					{
						if (itemType != ItemType.SkullRed)
						{
							return;
						}
						amongusType = "AmongUsRed";
					}
					else
					{
						amongusType = "AmongUsBlue";
					}
					component.enabled = false;
					CreateFumo(amongusType, component.transform, position, rotation, scale);
				}
			}
		}

		// Token: 0x0200000A RID: 10
		[HarmonyPatch(typeof(Torch), "Start")]
		public static class amongufiytorch
		{
			// Token: 0x06000010 RID: 16 RVA: 0x00002470 File Offset: 0x00000670
			public static void Prefix(Torch __instance)
			{
				Renderer componentInChildren = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
				if (componentInChildren)
				{
					Vector3 position = new Vector3(0f, 0.1f, 0f);
					Quaternion rotation = Quaternion.Euler(270f, 270f, 0f);
					Vector3 scale = new Vector3(1f, 1f, 1f) * 2.75f;
					componentInChildren.enabled = false;
					CreateFumo("AmongUsYellow", componentInChildren.transform.parent.transform, position, rotation, scale);
				}
			}
		}

		// Token: 0x0200000B RID: 11
		[HarmonyPatch(typeof(Soap), "Start")]
		public static class amongufiysoap
		{
			// Token: 0x06000011 RID: 17 RVA: 0x00002500 File Offset: 0x00000700
			public static void Prefix(Soap __instance)
			{
				Renderer componentInChildren = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
				if (componentInChildren)
				{
					Vector3 position = new Vector3(0f, 0.1f, 0f);
					Quaternion rotation = Quaternion.Euler(270f, 270f, 0f);
					Vector3 scale = new Vector3(1f, 1f, 1f) * 2.75f;
					componentInChildren.enabled = false;
					CreateFumo("AmongUsWhite", componentInChildren.transform.parent.transform, position, rotation, scale);
				}
			}
		}
	}
}