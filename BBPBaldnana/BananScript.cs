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
using BBPlusNameAPI;
using System.Collections.Generic;

namespace BBPBaldnana
{
	public class ITM_Banan : Item
	{
		public override bool Use(PlayerManager pm)
		{
			UnityEngine.Debug.Log("CUSTOM ITEM BABY");
			Item bsod = GameObject.Instantiate(BaldiBananaMayham.BSODAObj.item);
			bsod.transform.name = "Banana NPCless";
			bsod.Use(pm);
			return true;
		}
	}
}
