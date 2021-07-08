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
using System.Linq;

namespace BBPBaldnana
{
    [BepInPlugin("mtm101.rulerp.bbplus.bbpbaldnana", "BB+ Banana Mayham", "0.0.0.0")]

    public class BaldiBananaMayham : BaseUnityPlugin
    {

        public const string BananImage32x = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAtUExURbSqULCgJ77BONjNSNSxLuzNUPLZZd6+QfXldpB8I97DPPntm+O7OMecHgAAAKe0lG8AAAAPdFJOU///////////////////ANTcmKEAAAAJcEhZcwAADsIAAA7CARUoSoAAAAC/SURBVDhP5ZLRDoMgDEVBSkUG/f/P3b0Vp3FuPi87waT0HoomBrvhR4QQR3lNsDiN8hpccS98NSjENDZXBJOUVcfugmBzzil9MIRCJlpG64CqpmWdAMrZKEBlolA5ATMOzgOUkqoIBHOB8FBh5uTKnILU1vwavsqQVDOEtgombTPQ91mI89w4wAXrvbvDPh+CBvNVoMIxs38RXaR+/iXY0tFBRFBt8S6YxRgFqwsujPs/sgvOAkY5OAnv/IVg9gTtgzGDXe7FNAAAAABJRU5ErkJggg==";
        public static Sprite BananSprite32x;
        public const string BananImage128x = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAzUExURbCgJ7SqUL7BOJB8I9jNSN7DPOzNUN6+QfLZZfXldpR2YcecHntcR9SxLvntm+O7OAAAAPEFAmUAAAARdFJOU/////////////////////8AJa2ZYgAAAAlwSFlzAAAOwQAADsEBuJFr7QAAAo5JREFUeF7tme12qjAURLkB+ajlyvs/bZnMxCTIbXu7FI+rZ/9AjMnM1iDLrjbLk3EBF3ABF3ABF3ABF3ABF7Aj0DxJxYLAn5UQ2jYEjR2KBYFlCaFZ+c0CKP/dAtwEFwih6/jsSOwIED47EmsCx2+CJYHn3I4tCZCjN8GKQNelesCxY7AicFrpIxDoe44egRWBYWA5BbpuHDn+eGwJJIUxwlcejQ2BVF9eiI9S6LphwJc+/SloQwBfQsD6vh9HCkxTnHMnkIy3BiDw9oZRSwKxW2ASBO6jgDeTmCYcz5H3dysC6SKsoQCIKT8kp6Ca9fMcAhWsCMyzOiswVStXYtp/8lcoQgKnUwjzzHorArgM1SrGMT1iUYrg3O+g5itcP0242Z1O88r5PM+YaVMg1VOg3AjAFfuo7wasQx7rsQUAK6wIXC54absRWSVJpLBMzP+CXA5Yzno7AlCgROyL1BuRJUCpkc7VtiGlIJsNZb01AcBpXERQv4WVW9QpcNNRRATlxKYAbshyuKKVFTIoUHuEzziuBVdyea63JQCFzyUYDLbP90ivc27fDwMTy3prAlQAnEr4c6UO5FkNxhO3I/v19gSyAuEiZazk0LICj3VdDVNQfrmo5oo9gWXBjyX8XOCRSxGiNJFLy+py1iCYwDekigKLAkkhcbsRe3BOAheuTlUNFF9hUwCofYWLEcVYHNMHmz/ifbgWDIOCN9gVqDeCIUqt2Puob1HkDpYFgPojyrqi3hUNrJTnWIN/ASjqH1gXIG3bNAwDyt+QFVMxZirgE15DALQrjCUswTGflWjZl7yOQAIimabJR6Jp3+b1BO6NC7iAC7iAC7iAC7iAC7iAC7iAC7iAC7jAkwWW5QM+aJR+gYQbDQAAAABJRU5ErkJggg==";
        public static Sprite BananSprite128x;
        public const string BananFloorImage = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAABIUExURfLbnd+rWnBXQJVyTPfVa9rHTa+fJ7FvH29PLVQ+L/LTjfTZOt6hH/nSNvnUT/7jcPrnse/jyP7iTtORL8Z7E7SbTNO1igAAAEQmIkcAAAAYdFJOU///////////////////////////////AM0TLuoAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAQnSURBVHhe7ZiNkqIwEITJmchuSMJPQN//Ta97EjxdldurwrXqahqlIFnoj5lJgtuc3ywFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAFUAAF+E8BGvPrcDjUk229BMC6Y/vx+auebesFAL5z4XgEQj3f1u4AjY1djCQ41pZt7Q3gbYoxdoEEtWlbOwP0Fu6IQAxQbdvWvgD0jwkxoP8bAAbD+K96A4AEADmAOWIQa+u29gSQ+qN51VjbN7UnQJ8MfROLkPK1fVM7AnhWHwIwVYDO1o5N7QjQi39OaZoEIKbasakdASxKMMR5mqZOpoLuhwF6yUCYOwGgfhigjIE5xRKCtwHQvxBMP1uEGAPIgWRgKllI37r3bgCW/pj/JAD0n6baI3o6Ke0IEAEwx47PT4QbAOvccanHt9oRIMcUCkBB+AMwOLwkdY/fD/YCkASwBBj8AnCZidkG1bMv2g0gIwVpxkLE7IvWtCdWRHwSgH8AGJqm936oZ1+EZUgyEOAECUDpGcuY7Fw5vdP3AXxfVc9vxBEYEQBZiq9LoMEB7OOTBPwTwKkC9P3dRRwA+ApA4POiDGQa6lf/p0vzYwA8rbW2728ibtBQPl8Z+CKEACAD5WVECAzCT38BeJaAhwBigEvtxMnN9ivEaIGArRBciQ+PCkwhB3kjkTKIZkDXxAqMG/73ABaW5BYAQUh+ZTgZiYt8izfVpU4AAiKwAuCLv5KrERwE46luAAbvHEwnkBPCEkNuYtdkNEYYEnbSkWyaSgGkOITZGL6RFmWbTHLRubjlT4Dx4KzDHR3V0Q3CDreHPSjECW2VIQOCGIhVSmXiw0tQms9zzrm6Q5n+KYf8IMtXQq8xYn1RSgsckjH0tYgDcgErBj71TTMMI7okADSWKE1cB5GiGS9kjH8FyCGYXI2eqfEAyFgq1u3mV32ztBlmjAERxJHG6x6SQd+vRsjBqmxCzidpvRbnx+ulERE4LQfHH7PU4X7ZPOQWCYC7nYwgVGc+OMRySfMlz6fAKZll4bI5tKfWGDOePTUgeBeN2DwvYoKWFtbLssD/U+5yq3Zc2jljbPQSCvGmcJxQq/00u6syY27KuDRja/ICgJPFJgwsYN94IQBAXsYC8PF4qa5i79K2LesQ1cknXAchxktqb6t8jCif6PBqaDIfnwAeO/heR6AfemtaZIgAH9/7b06b2yzWZVXAgzoMfXM3yhkC+djib7w3CH/DxUymEOtwUUBmKAJ83if+mca55cYPxhx3D66VEQKCXAm8bxpG38KWs1UwhrZFf472lMVgAEO28GbqYW9mWJ/ucV8DMJqZC1MwmBvhLRPCE6fXADQmz1DLHY4ePPhFrwEQjWMz/r26XgjwPb0Z4Hz+DTSBfnMzwR0yAAAAAElFTkSuQmCC";
        public static Sprite BananFloorSprite;


        public const string RipeImage32x = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsEAAA7BAbiRa+0AAAIXSURBVFhH3ZbPa9NgHMbfpI20qZMOyxTqdE7xOsdAYZdBLx70MvAmeBIPonjw6Fkm0z9AEO+6KXibenT4E0Fwwx/dLpsoldUF6rrZtGlMPt8EujKwwkZjHnj48BKaN+/zPm8aTW2/3IDt2nIuPWDXtGMPUBjN4b+p6wnsRAdCddSF+CdQun6CgT1rwYPPF6An5o7vKQiVGkvj785PPJ5LYU9+Qm4sE2BlTee88l3esHDiWAqz8y3Ni28HGoHTwxm867iJNVPHoeKbQNnV8IImTp/cjbXeBA7V9QS2803Im2/CvsfA+D0F91hrcNCuw7tXFuGDJ6vwv+wA57zdN+uO8r1a0/FSVVysu/iz1wffG70GDhWJDvgr6Fjnrtrw8KUkzBySGlmNGnQac/BX+Q5sOp9gsirX308swzfTKzA6CSwXLvpQiexlqJtHoErKSq0mULcXJYG5PgfmT8sa9Ky0um+kAp36M1irPoXvJj/Aj/d/QE9EF50Ebh3d60ON9QxDwzgFVbIAvjhy/a0te/5SjrVa6xdmhySR/aNSqUz/PHTdGfj6xmNYfFSCnqKTQCge/ey+HgZKyTf9tfwFqBln4KsaXzPqhVRBzcvClXtAbmUOrsPKt0mYML/C4sNZ6Kl1zmglEGrTe2E8F2yylgeuPgCX/D97TyvB6aiEv0pJJJXSNGzRVnNFM4F2bUrkH9TJvbudgFJ/APrGsIMrfdd+AAAAAElFTkSuQmCC";
        public static Sprite RipeSprite32x;
        public const string RipeImage128x = "iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAMAAAD04JH5AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAD8UExURQAAAEI6FOh7NuI+JfC6P8g7ElEUCP79XtyxLsthGlEVCPr9XuXiOdGRJFIfCub8X8DjOtrIMFMwDYf6nYzyR5rhO7zbNldIEoj7/XXzn23nSXTcOpTaN05ZF1tv+nzZ/of4/3b0+HPwy2bcY2TZOmjOMzdVFeJCZuVS0udc9edg+dZe+WI68zVQ8FCb8lKr4Gje0mXZjF7ORSpYF4skE8oxRM82i886zag34HIx4Tgs4SxJ4Tx43lSy3V/PsV7OaCZTGFQXDVQXIccyhcg30Y8w2VQq2Sgn2SdD2jFl0CNKWiZUPVEUHVEVOFAXWDETWBwRWA4QWA4YVQAAABKUcPMAAABUdFJOU///////////////////////////////////////////////////////////////////////////////////////////////////////////////AFP3ctEAAAAJcEhZcwAADsEAAA7BAbiRa+0AAAFWSURBVHhe7dTZThRRFIbRQkWZFBVkEBkdcWBGQRBUEBwAgX7/d+FmdZnYMaSTjpTlv246VXXO3t9VF40OK/7A5xYJSEAC6hvQhccEJCAB/1FAk70tfC4lIAEJqH/ANa7jdbk3AQlIQP0DbtDNTXz+VdJpxicgAQn4+wHGFrfooZc+HEtAAhJQ44B+BrjNHRxLQAISUOOAQe5yj/s4loAEJKBGAcYVQwzzgBFGcTwBCUjAPxjgWosxxnnIBI+YxLUEJCABFQho1xTTzDDLHI95wlOeYVz77E9AAhLQ+YDnvGCel7ziNW9YYJEllllhFevKP0CPCUhAAioQsMY6b3nHBpu8Z4ttPrDDLh+xLgEJSECFAvyWJZ/4zB77fOGAQ77yje/8wPhyX1MCEpCA6gQ0OVc64pgTfnLKGee4XjK+RQISkIDqBfzO/ba5fqkEJCABVxzQaFwAez2KBHEo54gAAAAASUVORK5CYII=";
        public static Sprite RipeSprite128x;

        public static List<WeightedItemObject> NewItems = new List<WeightedItemObject>();

        public static ItemObject BSODAObj;
        public static ItemObject BananaObject;
        public static ItemObject RipeObject;

        private static Texture TextureFromBase64(string base64)
        {
            byte[] array = Convert.FromBase64String(base64);
            Texture2D texture2D = new Texture2D(1, 1,TextureFormat.RGBA32,false);
            ImageConversion.LoadImage(texture2D, array);
            texture2D.filterMode = 0;
            return texture2D;
        }

        private static bool HasAlreadyLoaded = false;
        private static void Load()
        {
            if (HasAlreadyLoaded)
            {
                return;
            }
            HasAlreadyLoaded = true;
            string pathtoload = Path.Combine(Application.persistentDataPath, "Mods", "BBSlotReducer", "data.dat");
            if (File.Exists(pathtoload))
            {
                FileStream stream = File.OpenRead(pathtoload);
                BinaryReader reader = new BinaryReader(stream);
                try
                {
                    //SlotsPriv = reader.ReadByte();
                }
                catch (Exception E)
                {
                    UnityEngine.Debug.LogError(E.Message);
                }
                reader.Close();
            }
            else
            {
                Save();
            }
        }

        private static void Save()
        {
            string pathtosave = Path.Combine(Application.persistentDataPath, "Mods", "Baldnana");
            if (!Directory.Exists(pathtosave))
            {
                Directory.CreateDirectory(pathtosave);
            }
            DirectoryInfo fo = new DirectoryInfo(pathtosave);
            FileInfo[] filefo = fo.GetFiles("data.dat");
            if (filefo.Length != 0)
            {
                filefo[0].Delete();
            }
            FileStream stream = File.Create(Path.Combine(pathtosave, "data.dat"));
            BinaryWriter writer = new BinaryWriter(stream);

            //writer.Write((byte)SlotsPriv);

            writer.Close();
        }

        /*public static byte Slots
        {
            get
            {
                Load();
                return SlotsPriv;
            }

            set
            {
                SlotsPriv = value;
                Save();
            }


        }*/

       

        void Awake()
        {
            Harmony harmony = new Harmony("mtm101.rulerp.bbplus.bbpbaldnana");
            NameMenuManager.AddPage("bbpbnoptions", "options");
            NameMenuManager.AddToPage("options", new Name_MenuFolder("tobbpbnoptions", "Banana Mayham", "bbpbnoptions"));
            Texture tex = BaldiBananaMayham.TextureFromBase64(BananImage32x);
            BananSprite32x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
            tex = BaldiBananaMayham.TextureFromBase64(BananImage128x);
            BananSprite128x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            tex = BaldiBananaMayham.TextureFromBase64(BananFloorImage);
            BananFloorSprite = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            tex = BaldiBananaMayham.TextureFromBase64(RipeImage32x);
            RipeSprite32x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
            tex = BaldiBananaMayham.TextureFromBase64(RipeImage128x);
            RipeSprite128x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            BananaObject = CreateObject("Banana", "A good ol' fashioned Banana!\nYou can't go wrong with a scrumptious snack like this!\nJust make sure to clean up the peels!", BananSprite32x, BananSprite128x, Items.NanaPeel, 50);
            BananaObject = CreateObject("Ripe", "A good ol' fashioned Banana!\nYou can't go wrong with a scrumptious snack like this!\nJust make sure to clean up the peels!", RipeSprite32x, RipeSprite128x, Items.NanaPeel, 100);
            BananaObject.item = new GameObject().AddComponent<ITM_Banan>();
            DontDestroyOnLoad(BananaObject.item);
            BSODAObj = Resources.FindObjectsOfTypeAll<ItemObject>().ToList().Find(x => x.itemType == Items.Bsoda);



            //item drops
            WeightedItemObject banobj = new WeightedItemObject();
            banobj.selection = BananaObject;
            banobj.weight = 100;
            NewItems.Add(banobj);
            banobj = new WeightedItemObject();
            banobj.selection = Resources.FindObjectsOfTypeAll<ItemObject>().ToList().Find(x => x.itemType == Items.Quarter);
            banobj.weight = 25;
            NewItems.Add(banobj);
            harmony.PatchAll();
        }


        public static ItemObject CreateObject(string localizedtext, string desckey, Sprite smallicon, Sprite largeicon, Items type, int price)
        {
            ItemObject obj = ScriptableObject.CreateInstance<ItemObject>();
            obj.nameKey = localizedtext;
            obj.itemSpriteSmall = smallicon;
            obj.itemSpriteLarge = largeicon;
            obj.itemType = type;
            obj.descKey = desckey;
            obj.cost = price;

            return obj;
        }
    }
}
