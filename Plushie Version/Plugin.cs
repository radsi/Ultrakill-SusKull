using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace SusKull
{
    [BepInPlugin("radsi.suskull", "SusKull", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static Dictionary<string, GameObject> allAmongus = new Dictionary<string, GameObject>();
        public static AssetBundle amongusbundle;

        private void Awake()
        {
            amongusbundle = AssetBundle.LoadFromMemory(Resource1.suskull);
            amongusbundle.LoadAllAssets();
            new Harmony("radsi.suskull").PatchAll();
            allAmongus.Add("AmongUsBlue", amongusbundle.LoadAsset<GameObject>("AmongUsBlueGO"));
            allAmongus.Add("AmongUsRed", amongusbundle.LoadAsset<GameObject>("AmongUsRedGO"));
            allAmongus.Add("AmongUsYellow", amongusbundle.LoadAsset<GameObject>("AmongUsYellowGO"));
        }

        [HarmonyPatch(typeof(Skull), "Start")]
        public static class susify
        {
            public static void Prefix(Skull __instance)
            {
                Renderer masterSkull = __instance.gameObject.GetComponent<Renderer>();
                if (masterSkull)
                {
                    string amongusType;
                    switch (__instance.GetComponent<ItemIdentifier>().itemType)
                    {
                        case ItemType.SkullBlue:
                            amongusType = "AmongUsBlue";
                        break;

                        case ItemType.SkullRed:
                            amongusType = "AmongUsRed";
                            break;

                        case ItemType.Torch:
                            amongusType = "AmongUsYellow";
                            break;

                        default:
                            return;
                     }
                    masterSkull.enabled = false;
                    GameObject _fumo = allAmongus[amongusType];
                    GameObject SkullAmongus = GameObject.Instantiate(_fumo, masterSkull.transform);
                    SkullAmongus.transform.localRotation = Quaternion.Euler(90, 20, 0);
                    SkullAmongus.transform.localPosition = new Vector3(-3.7091f, -1f, 1.9f);
                }
            }
        }
    }
}
