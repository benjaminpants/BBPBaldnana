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
    [HarmonyPatch(typeof(ItemManager))]
    [HarmonyPatch("Update")]
    class DebugSlotPatch
    {
        static bool Prefix(ItemManager __instance)
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {
                __instance.AddItem(BaldiBananaMayham.BananaObject);
                __instance.AddItem(BaldiBananaMayham.RipeObject);
                __instance.AddItem(BaldiBananaMayham.SplitObject);
            }
            return true;
        }
    }
}
