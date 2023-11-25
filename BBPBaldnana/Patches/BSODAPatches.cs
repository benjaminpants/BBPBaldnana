using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;
//BepInEx stuff
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using HarmonyLib; //god im hoping i got the right version of harmony
using BepInEx.Configuration;
using System.Collections.Generic;
using TMPro;
using HarmonyLib.Tools;

namespace BBPBaldnana
{


    [HarmonyPatch(typeof(ITM_BSODA))]
    [HarmonyPatch("Use")]
    class BsodaCustomUse
    {
        static bool Prefix(ITM_BSODA __instance, ref PlayerManager pm)
        {
            if (__instance.transform.name == "Banana NPCless")
            {
                FieldInfo speed = AccessTools.Field(typeof(ITM_BSODA), "speed");
                speed.SetValue(__instance, 0f);
                FieldInfo time = AccessTools.Field(typeof(ITM_BSODA), "time");
                time.SetValue(__instance, 600f); //makes the banana stay for 10 minutes, which is way longer then any BB+ game can reasonibly last, i could set this to 999999 but i'd rather have some form of auto-cleanup
                FieldInfo enviroment = AccessTools.Field(typeof(ITM_BSODA), "ec");
                enviroment.SetValue(__instance, Singleton<BaseGameManager>.Instance.Ec);
                __instance.transform.position = pm.transform.position;
                __instance.transform.rotation = Singleton<CoreGameManager>.Instance.GetCamera(pm.playerNumber).transform.rotation;
                //okay now that we've simulated the base behavior with a few tweaks time to add our custom stuff
                SpriteRenderer spr = __instance.gameObject.transform.GetComponentInChildren<SpriteRenderer>();
                spr.sprite = BaldiBananaMayham.BananaFloor;
                spr.transform.position -= new Vector3(0f, 3.0f, 0f);
                return false;
            }
            if (__instance.transform.name == "GB RIPE")
            {
                FieldInfo speed = AccessTools.Field(typeof(ITM_BSODA), "speed");
                speed.SetValue(__instance, 0f);
                FieldInfo time = AccessTools.Field(typeof(ITM_BSODA), "time");
                time.SetValue(__instance, 10f);
                FieldInfo enviroment = AccessTools.Field(typeof(ITM_BSODA), "ec");
                enviroment.SetValue(__instance, Singleton<BaseGameManager>.Instance.Ec);
                __instance.transform.position = pm.transform.position;
                //okay now that we've simulated the base behavior with a few tweaks time to add our custom stuff
                SpriteRenderer spr = __instance.gameObject.transform.GetComponentInChildren<SpriteRenderer>();
                spr.sprite = BaldiBananaMayham.RipeSpray;
                return false;
            }

            return true;
        }
    }


    [HarmonyPatch(typeof(ITM_BSODA))]
    [HarmonyPatch("OnTriggerEnter")]
    class BsodaCustomOnEnter
    {
        static bool Prefix(ITM_BSODA __instance, ref Collider other)
        {
            if (other == null) return true;
            if (other.tag == "NPC" && other.isTrigger)
            {
                if (__instance.name == "Banana NPCless")
                {
                    Navigator nav = other.GetComponent<Navigator>();
                    if (nav.Velocity.magnitude <= float.Epsilon)
                    {
                        return false;
                    }
                    AudioManager audMan = other.GetComponent<AudioManager>();
                    if (audMan != null)
                    {
                        audMan.PlaySingle(BaldiBananaMayham.SlipSound);
                    }
                    else
                    {
                        BaldiBananaMayham.Log.LogWarning(String.Format("{0} doesn't have AudioManager... strange...", other.gameObject.name));
                    }
                    FieldInfo speed = AccessTools.Field(typeof(ITM_BSODA), "speed");
                    speed.SetValue(__instance, 40f);
                    __instance.transform.rotation = Quaternion.LookRotation(nav.gameObject.transform.forward, nav.Velocity);
                    __instance.transform.rotation = Quaternion.Euler(__instance.transform.rotation.eulerAngles + (180f * Vector3.up));
                    __instance.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z);
                    __instance.name = "Banana has NPC";
                    FieldInfo time = AccessTools.Field(typeof(ITM_BSODA), "time");
                    time.SetValue(__instance, 15f);
                }
                else if (__instance.name == "Banana has NPC")
                {
                    return false;
                }
                else if (__instance.name == "GB RIPE")
                {
                    AudioManager audMan = other.GetComponent<AudioManager>();
                    if (audMan != null)
                    {
                        audMan.PlaySingle(BaldiBananaMayham.ShingSound);
                    }
                    else
                    {
                        BaldiBananaMayham.Log.LogWarning(String.Format("{0} doesn't have AudioManager... strange...", other.gameObject.name));
                    }
                    return true;
                }
            }
            return true;
        }
    }
}
