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
    [HarmonyPatch(typeof(LevelGenerator))]
    [HarmonyPatch("StartGenerate")]
    class MessWithLevelData
    {
        static bool Prefix(LevelGenerator __instance)
        {
            __instance.ld.items = BaldiBananaMayham.NewItems.ToArray();
            __instance.ld.shopItems = BaldiBananaMayham.ShopItems.ToArray();
            return true;
        }
    }
}