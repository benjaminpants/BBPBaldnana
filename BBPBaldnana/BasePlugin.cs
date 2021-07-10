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
        public const string RipeSprayImage = "iVBORw0KGgoAAAANSUhEUgAAAQAAAAEACAMAAABrrFhUAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAADAUExURQAAAEs1EfX9XhMTV+h7NuI+Jedg+fC6P8g7ElEUHVIYCoj6/of6ndyxLsthGuXiOdGRJIzyR8DjOtrIMHzZ/nPwy3Xzn2bcY23nSWje0prhO7zbNlNRFdZe+VSy3Xb0+GvWOJTaNy1VF1Cb8ltv+uJCZuVS0udc9Wo26jFN6VKr4GXZjF7ORYskE8oxRMs0iMw5z6g34Dgs4Tdv11/PsV7OaCdD2lMWLY8w2VQq2Sgn2SNKWiZUPVAXWDETWAAAAK6dbt4AAABAdFJOU////////////////////////////////////////////////////////////////////////////////////wDCe7FEAAAACXBIWXMAAA7BAAAOwQG4kWvtAAANEElEQVR4Xu2dCXvbNg/HPaf1lsg7mtpp0tXpkWxvs6M7urW72n7/b/UCIGT5kESQAGTF0j9+Hj5ybIr8iaIAGKImnwauEQCXg9UIgMvBagTA5WA1AuBysBoBcDlYjQC4HKxGAFwOViMALgerEQCXg9UIgMvBagTAZQeakHijNxoBcNmBJpPPPhsBDBkAIujfSdBlc6YBwJQ3+6EOAUynJ5MHDx4+7NcY6K4xsymMgACgT2OgMwBT0Mnk88+/+OL0rE9joKumTAsAMCEAZ6cDBDCd4/GfTr/88quvzmAE9Ock6AYAHP85HP81gJOBASjmcwbw9dfffDO4EVAUAODRo0cbAAY1AopZUUwLIIAAzs8fP0aDeFrwfw8ubwAz6P9ssby4eHK5BaA3Q8AXwAz7XyyKxdXFZILnwdOn354/e2Y1AoJpzRuZ8gQAvUetFovrq6s1gG8BwInNCDgEgIR9Uu8BQvjK8+Li4vLyxeXLlzgC5vwZnSwcbD8AePKDVqvwlSIAePHy6X0HINondvkM/l5BeXO7WBTF8+fL5ZMnL0Cv4E0bACnHo0mOAErd7gB49mpyZgTAIMKQTi/skzeaNJl8993338/+h6fA69d3dzc3P/xwdfXjjz/9RMd/bgPAIsKQ/FUZ9AAA+t8AgD+mk0mEIRWAEPpk8vPPb9788suvv/722+vXv/9+d/f27fX1H3/8+acd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";
        public static Sprite RipeSpraySprite;


        public const string SplitImage32x = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAeUExURbKUddasm9czQuni2NzLrZhuXbCflFlBOlQxKQAAAHWolOsAAAAKdFJOU////////////wCyzCzPAAAACXBIWXMAAA7DAAAOwwHHb6hkAAABAElEQVQ4T92Q4Y7EIAiEhzIgff8XvsFad5tscskm9+eIrTp8jgjOX+KPgHfxE/DQvgGe0heAHaa/X5sHsDQ7Dq2da/sC3C+JDdCcc7eBcE9rwiHQM2+LBZAyaInQ5B6DrEkswGSJCDlVRQRiGJnabwfQ+lSMGiLGqCStkwvwtvBkVY0hiyoA1u+9ATc3BimHC6jSJcO9ga6PRliIitAIs9KFBk+cqbwpCAzokC6v0hyGynlFpmmghf54AVrzLhKZ0VQ/UUP2JQk5e7mKzPmqUOndh/4kzMwCuvuWaoWiexEq5dI3oEqBhE4rO2Sw9A2o3chQuXqkit/xBnyO/wCc5w+MEh2SEfn5CQAAAABJRU5ErkJggg==";
        public static Sprite SplitSprite32x;
        public const string SplitImage128x = "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";
        public static Sprite SplitSprite128x;

        public static List<WeightedItemObject> NewItems = new List<WeightedItemObject>();
        public static List<WeightedItemObject> ShopItems = new List<WeightedItemObject>();

        public static ItemObject BSODAObj;
        public static ItemObject BananaObject;
        public static ItemObject RipeObject;
        public static ItemObject SplitObject;

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

            //FIX THIS TEXTURE LOADING CODE JESUS CHRIST
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
            tex = BaldiBananaMayham.TextureFromBase64(RipeSprayImage);
            RipeSpraySprite = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));


            tex = BaldiBananaMayham.TextureFromBase64(SplitImage32x);
            SplitSprite32x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
            tex = BaldiBananaMayham.TextureFromBase64(SplitImage128x);
            SplitSprite128x = Sprite.Create((Texture2D)tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));


            BananaObject = CreateObject("Banana", "Banana\nA good ol' fashioned Banana!\nDrop it and make people slip the opposite direction they are heading!\nThis can backfire and make them head towards you!\n(This is totally intentional and not a bug)", BananSprite32x, BananSprite128x, Items.NanaPeel, 50,25);
            BananaObject.item = new GameObject().AddComponent<ITM_Banan>();
            RipeObject = CreateObject("Ripe", "Ripe\nUse this rainbow Banana to spawn a hoard of rainbow colored Bananas to stop characters in their tracks!", RipeSprite32x, RipeSprite128x, Items.FidgetSpinner, 100, 100); //its a really bad idea to use these non-sense values but it could be causing issues
            RipeObject.item = new GameObject().AddComponent<ITM_RipeBanan>();
            SplitObject = CreateObject("Banana Split", "Banana Split\nThis yummy treat will increase your stamina for the rest of the floor!", SplitSprite32x, SplitSprite128x, Items.Football, 75, 25);
            SplitObject.item = new GameObject().AddComponent<ITM_BananSplit>();

            DontDestroyOnLoad(BananaObject.item);
            DontDestroyOnLoad(RipeObject.item);
            DontDestroyOnLoad(SplitObject.item);
            BSODAObj = Resources.FindObjectsOfTypeAll<ItemObject>().ToList().Find(x => x.itemType == Items.Bsoda);



            //item drops(REMINDER BEN TO FIX THIS CODE LATER AS ITS A MESS)
            WeightedItemObject banobj = new WeightedItemObject();
            banobj.selection = BananaObject;
            banobj.weight = 100;
            NewItems.Add(banobj);
            banobj = new WeightedItemObject();
            banobj.selection = Resources.FindObjectsOfTypeAll<ItemObject>().ToList().Find(x => x.itemType == Items.Quarter);
            banobj.weight = 5;
            NewItems.Add(banobj);
            banobj = new WeightedItemObject();
            banobj.selection = RipeObject;
            banobj.weight = 10;
            banobj = new WeightedItemObject();
            banobj.selection = SplitObject;
            banobj.weight = 35;
            NewItems.Add(banobj);

            banobj.selection = BananaObject;
            banobj.weight = 100;
            ShopItems.Add(banobj);
            banobj = new WeightedItemObject();
            banobj.selection = Resources.FindObjectsOfTypeAll<ItemObject>().ToList().Find(x => x.itemType == Items.Quarter);
            banobj.weight = 25;
            ShopItems.Add(banobj);
            banobj.selection = Resources.FindObjectsOfTypeAll<ItemObject>().ToList().Find(x => x.itemType == Items.GrapplingHook);
            banobj.weight = 10;
            ShopItems.Add(banobj);
            banobj.selection = Resources.FindObjectsOfTypeAll<ItemObject>().ToList().Find(x => x.itemType == Items.Teleporter);
            banobj.weight = 2;
            ShopItems.Add(banobj);
            banobj = new WeightedItemObject();
            banobj.selection = RipeObject;
            banobj.weight = 50;
            ShopItems.Add(banobj);
            banobj = new WeightedItemObject();
            banobj.selection = SplitObject;
            banobj.weight = 50;
            ShopItems.Add(banobj);


            harmony.PatchAll();
        }


        public static ItemObject CreateObject(string localizedtext, string desckey, Sprite smallicon, Sprite largeicon, Items type, int price, int cost)
        {
            ItemObject obj = ScriptableObject.CreateInstance<ItemObject>();
            obj.nameKey = localizedtext;
            obj.itemSpriteSmall = smallicon;
            obj.itemSpriteLarge = largeicon;
            obj.itemType = type;
            obj.descKey = desckey;
            obj.cost = cost;
            obj.price = price;

            return obj;
        }
    }
}
