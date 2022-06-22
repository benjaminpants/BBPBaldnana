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
using HarmonyLib; //god im hoping i got the right version of harmony
using BepInEx.Configuration;
using MTM101BaldAPI;
using System.Collections.Generic;

namespace BBPBaldnana
{
    public class ITM_Banan : Item
    {
        public override bool Use(PlayerManager pm)
        {
            Item bsod = GameObject.Instantiate(BaldiBananaMayham.BSODAObj.item);
            bsod.transform.name = "Banana NPCless";
            bsod.Use(pm);
            return true;
        }
    }

    public class ITM_RipeBanan : Item
    {
        public override bool Use(PlayerManager pm)
        {
            Item bsod = GameObject.Instantiate(BaldiBananaMayham.BSODAObj.item);
            bsod.transform.name = "GB RIPE";
            bsod.Use(pm);
            return true;
        }
    }

    public class ITM_BananSplit : Item
    {
        public override bool Use(PlayerManager pm)
        {
            pm.plm.staminaMax += 25f;
            return true;
        }
    }
}
