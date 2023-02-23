using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UMM;

namespace SusSkull
{
    [UKPlugin("radsi.ultrakillsuskull", "SusKull", "1.1.0", "Replaces the skulls with among us", false, false)]
    public class amongusSkulls : UKMod
    {
        public static Dictionary<string, GameObject> allAmongus = new Dictionary<string, GameObject>();
        Harmony amongus;

        public static AssetBundle amongusbundle;


        public override void OnModLoaded()
        {
            amongusbundle = AssetBundle.LoadFromMemory(Resource1.suskull);
            amongusbundle.LoadAllAssets();
            amongus = new Harmony("radsi.ultrakillsuskull");
            amongus.PatchAll();
            //9
            allAmongus.Add("AmongUsBlue", amongusbundle.LoadAsset<GameObject>("AmongUsBlueGO"));
            allAmongus.Add("AmongUsRed", amongusbundle.LoadAsset<GameObject>("AmongUsRedGO"));
            allAmongus.Add("AmongUsYellow", amongusbundle.LoadAsset<GameObject>("AmongUsYellowGO"));
            allAmongus.Add("AmongUsWhite", amongusbundle.LoadAsset<GameObject>("AmongUsWhiteGO"));
        }

        public override void OnModUnload()
        {
            allAmongus.Clear();
            amongus.UnpatchSelf();
            amongus = null;
            amongusbundle.Unload(true);
            amongusbundle = null;
        }

        [HarmonyPatch(typeof(Skull), "Start")]
        public static class amongusfiyskull
        {
            public static void Prefix(Skull __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponent<Renderer>();
                if (masterSkull)
                {
                    string amongusType;
                    Vector3 amongusposition = new Vector3(0, 0, 0);
                    Quaternion amongusrotation = Quaternion.Euler(0, 20, 270);
                    Vector3 amongusscale = new Vector3(1, 1, 1);
                    switch (__instance.GetComponent<ItemIdentifier>().itemType)
                    {
                        case ItemType.SkullBlue:
                            amongusType = "AmongUsBlue";
                            break;

                        case ItemType.SkullRed:
                            amongusType = "AmongUsRed";
                            break;

                        default:
                            amongusType = "AmongUsRed";
                            return;
                     }
                    masterSkull.enabled = false;
                    CreateAmongus(amongusType, masterSkull.transform, amongusposition, amongusrotation, amongusscale);
                }
            }
        }


        [HarmonyPatch(typeof(Torch), "Start")]
        public static class amongusfiytorch
        {
            public static void Prefix(Torch __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
                if (masterSkull)
                {
                    Vector3 amongusposition = new Vector3(0, 0, 0);
                    Quaternion amongusrotation = Quaternion.Euler(270, 270, 0);
                    Vector3 amongusscale = new Vector3(1.9f, 1.9f, 1.9f);
                    string amongusType = "AmongUsYellow";
                    masterSkull.enabled = false;
                    CreateAmongus(amongusType, masterSkull.transform.parent.transform, amongusposition, amongusrotation, amongusscale);
                }
            }
        }

        [HarmonyPatch(typeof(Soap), "Start")]
        public static class amongusfiysoap
        {
            public static void Prefix(Soap __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponentInChildren<MeshRenderer>();
                if (masterSkull)
                {
                    Vector3 amongusposition = new Vector3(0, 0, 0);
                    Quaternion amongusrotation = Quaternion.Euler(270, 270, 0);
                    Vector3 amongusscale = new Vector3(1.9f, 1.9f, 1.9f);
                    string amongusType = "AmongUsWhite";
                    masterSkull.enabled = false;
                    CreateAmongus(amongusType, masterSkull.transform.parent.transform, amongusposition, amongusrotation, amongusscale);
                }
            }
        }

        public static void CreateAmongus(string amongusType, Transform masterSkull, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Debug.Log("Swapping " + masterSkull.name + " to " + amongusType);
            GameObject _amongus = allAmongus[amongusType];
            GameObject Skullamongus = Instantiate(_amongus, masterSkull);
            Skullamongus.SetActive(true);
            Skullamongus.transform.localRotation = rotation;
            Skullamongus.transform.localPosition = position;
            Skullamongus.transform.localScale = scale;
            Renderer[] amongusmatter = Skullamongus.GetComponentsInChildren<Renderer>();
            foreach(Renderer ren in amongusmatter)
            {
                Material[] amongusmaterial = ren.materials;
                foreach(Material mat in amongusmaterial)
                {
                    mat.shader = Shader.Find("psx/unlit/unlit");
                }
            }
        }
    }
}
