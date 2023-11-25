﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Net;
using System.IO;
using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using HarmonyLib;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;
using MTM101BaldAPI.AssetManager;
using MTM101BaldAPI;
using MTM101BaldAPI.Registers;

namespace BBPBaldnana
{
    [BepInPlugin("mtm101.rulerp.bbplus.bbpbaldnana", "BB+ Banana Mayham", "2.0.0.0")]

    public class BaldiBananaMayham : BaseUnityPlugin
    {

        internal static ManualLogSource Log;

        public static Sprite BananaSmall;
        public static Sprite BananaLarge;
        public static Sprite BananaFloor;

        public static Sprite RipeSmall;
        public static Sprite RipeLarge;
        public static Sprite RipeSpray;

        public static Items ripeId;
        public static Items splitId;

        public static Sprite SplitSmall;
        public static Sprite SplitLarge;

        public static List<WeightedItemObject> NewItems = new List<WeightedItemObject>();
        public static List<WeightedItemObject> ShopItems = new List<WeightedItemObject>();

        public static ItemObject BSODAObj;
        public static ItemObject BananaObject;
        public static ItemObject RipeObject;
        public static ItemObject SplitObject;

        public static SoundObject SlipSound;
        public static SoundObject ShingSound;

        public static List<WeightedPosterObject> Posters = new List<WeightedPosterObject>();

        void Awake()
        {
            Harmony harmony = new Harmony("mtm101.rulerp.bbplus.bbpbaldnana");
            //NameMenuManager.AddPage("bbpbnoptions", "options");
            //NameMenuManager.AddToPage("options", new MenuFolder("tobbpbnoptions", "Banana Mayham", "bbpbnoptions"));

            //finally fixed this texture loading code :)
            BananaSmall = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this,"BananaSmall.png"), Vector2.one / 2f, 25f);
            BananaLarge = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this, "BananaLarge.png"), Vector2.one / 2f, 50f);
            BananaFloor = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this, "BananaFloor.png"), Vector2.one / 2f, 15f);

            SlipSound = ObjectCreatorHandlers.CreateSoundObject(AssetManager.AudioClipFromMod(this, "Slip.wav"), "Vfx_Slip", SoundType.Effect, Color.yellow);
            ShingSound = ObjectCreatorHandlers.CreateSoundObject(AssetManager.AudioClipFromMod(this, "Shing.wav"), "Vfx_Shing", SoundType.Effect, Color.cyan);

            RipeSmall = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this, "RipeSmall.png"), Vector2.one / 2f, 25f);
            RipeLarge = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this, "RipeLarge.png"), Vector2.one / 2f, 50f);
            RipeSpray = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this, "RipeSpray.png"), Vector2.one / 2f, 10f);


            SplitSmall = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this, "SplitSmall.png"), Vector2.one / 2f, 25f);
            SplitLarge = AssetManager.SpriteFromTexture2D(AssetManager.TextureFromMod(this, "SplitLarge.png"), Vector2.one / 2f, 50f);

            ripeId = EnumExtensions.ExtendEnum<Items>("RipeBanana");
            splitId = EnumExtensions.ExtendEnum<Items>("BananaSplit");
            
            GeneratorManagement.Register(this, GenerationModType.Override, (string name, int floorid, LevelObject obj) =>
            {
                obj.posters = obj.posters.AddRangeToArray(BaldiBananaMayham.Posters.ToArray()); //no reason not to add the posters
                if (name == "C2") return;
                obj.items = BaldiBananaMayham.NewItems.ToArray();
                obj.shopItems = BaldiBananaMayham.ShopItems.ToArray();
                obj.MarkAsNeverUnload();
            });

            MTM101BaldiDevAPI.SavesEnabled = false;

            Posters.Add(new WeightedPosterObject()
            {
                selection=ObjectCreatorHandlers.CreatePosterObject(AssetManager.TextureFromMod(this, "Posters", "BananaHole.png"), null, new PosterTextData[0]),
                weight=80
            });
            Posters.Add(new WeightedPosterObject()
            {
                selection = ObjectCreatorHandlers.CreatePosterObject(AssetManager.TextureFromMod(this, "Posters", "DONT_TRUST.png"), null, new PosterTextData[0]),
                weight = 10
            });
            Posters.Add(new WeightedPosterObject()
            {
                selection = ObjectCreatorHandlers.CreatePosterObject(AssetManager.TextureFromMod(this, "Posters", "DO_TRUST.png"), null, new PosterTextData[0]),
                weight = 30
            });
            Posters.Add(new WeightedPosterObject()
            {
                selection = ObjectCreatorHandlers.CreatePosterObject(AssetManager.TextureFromMod(this, "Posters", "YUMMY.png"), null, new PosterTextData[0]),
                weight = 40
            });
            Posters.Add(new WeightedPosterObject()
            {
                selection = ObjectCreatorHandlers.CreatePosterObject(AssetManager.TextureFromMod(this, "Posters", "BUY_RIPE.png"), null, new PosterTextData[0]),
                weight = 60
            });
            PosterObject tomPoster = ScriptableObject.CreateInstance<PosterObject>();
            tomPoster.textData = new PosterTextData[0];
            tomPoster.multiPosterArray = new PosterObject[]
            {
                ObjectCreatorHandlers.CreatePosterObject(AssetManager.TextureFromMod(this, "Posters", "yipee1.png"),null,new PosterTextData[0]),
                ObjectCreatorHandlers.CreatePosterObject(AssetManager.TextureFromMod(this, "Posters", "yipee2.png"),null,new PosterTextData[0])
            };
            Posters.Add(new WeightedPosterObject()
            {
                selection = tomPoster,
                weight = 60
            });

            harmony.PatchAll();
        }
    }

    [HarmonyPatch(typeof(NameManager))]
    [HarmonyPatch("Awake")]
    static class NameManagerAwakePatch
    {
        static void Prefix()
        {
            BaldiBananaMayham.BananaObject = ObjectCreatorHandlers.CreateItemObject("Itm_Banana", "Desc_Banana", BaldiBananaMayham.BananaSmall, BaldiBananaMayham.BananaLarge, Items.NanaPeel, 50, 25);
            BaldiBananaMayham.BananaObject.item = new GameObject().AddComponent<ITM_Banan>();
            BaldiBananaMayham.RipeObject = ObjectCreatorHandlers.CreateItemObject("Itm_Ripe", "Desc_Ripe", BaldiBananaMayham.RipeSmall, BaldiBananaMayham.RipeLarge, BaldiBananaMayham.ripeId, 100, 50);
            BaldiBananaMayham.RipeObject.item = new GameObject().AddComponent<ITM_RipeBanan>();
            BaldiBananaMayham.SplitObject = ObjectCreatorHandlers.CreateItemObject("Itm_Split", "Desc_Split", BaldiBananaMayham.SplitSmall, BaldiBananaMayham.SplitLarge, BaldiBananaMayham.splitId, 75, 35);
            BaldiBananaMayham.SplitObject.item = new GameObject().AddComponent<ITM_BananSplit>();

            GameObject.DontDestroyOnLoad(BaldiBananaMayham.BananaObject.item.gameObject);
            GameObject.DontDestroyOnLoad(BaldiBananaMayham.RipeObject.item.gameObject);
            GameObject.DontDestroyOnLoad(BaldiBananaMayham.SplitObject.item.gameObject);

            List<ItemObject> gameItems = Resources.FindObjectsOfTypeAll<ItemObject>().ToList();

            BaldiBananaMayham.BSODAObj = gameItems.Find(x => x.itemType == Items.Bsoda);

            ItemObject grapplingHook = gameItems.Where(x => x.itemType == Items.GrapplingHook).Where(x => ((ITM_GrapplingHook)x.item).uses == 4).First();

            //item drops(REMINDER BEN TO FIX THIS CODE LATER AS ITS A MESS)
            BaldiBananaMayham.NewItems.Add(new WeightedItemObject()
            {
                selection = BaldiBananaMayham.BananaObject,
                weight = 100
            });
            BaldiBananaMayham.NewItems.Add(new WeightedItemObject()
            {
                selection = gameItems.Find(x => x.itemType == Items.Quarter),
                weight = 20
            });
            BaldiBananaMayham.NewItems.Add(new WeightedItemObject()
            {
                selection = BaldiBananaMayham.RipeObject,
                weight = 10
            });
            BaldiBananaMayham.NewItems.Add(new WeightedItemObject()
            {
                selection = BaldiBananaMayham.SplitObject,
                weight = 40
            });

            BaldiBananaMayham.ShopItems.Add(new WeightedItemObject()
            {
                selection = BaldiBananaMayham.BananaObject,
                weight = 100
            });
            BaldiBananaMayham.ShopItems.Add(new WeightedItemObject()
            {
                selection = gameItems.Find(x => x.itemType == Items.Quarter),
                weight = 25
            });
            BaldiBananaMayham.ShopItems.Add(new WeightedItemObject()
            {
                selection = grapplingHook,
                weight = 25
            });
            BaldiBananaMayham.ShopItems.Add(new WeightedItemObject()
            {
                selection = gameItems.Find(x => x.itemType == Items.Teleporter),
                weight = 5
            });
            BaldiBananaMayham.ShopItems.Add(new WeightedItemObject()
            {
                selection = BaldiBananaMayham.RipeObject,
                weight = 30
            });
            BaldiBananaMayham.ShopItems.Add(new WeightedItemObject()
            {
                selection = BaldiBananaMayham.SplitObject,
                weight = 50
            });
        }
    }
}
