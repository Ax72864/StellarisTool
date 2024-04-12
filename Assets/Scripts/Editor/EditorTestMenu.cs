using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace StellarisTool
{
    public class EditorTestMenu : Editor
    {
        [MenuItem("Tools/Test/ParseSave")]
        public static void ParseSave()
        {
            //var v1 = new Variable();
            //v1.key = "%ADJECTIVE%";
            //v1.variables = new Variable[]{
            //    new Variable()
            //    {
            //        key = "adjective",
            //        value = new Variable()
            //        {
            //            key = "SPEC_kelsiote"
            //        }
            //    }            
            //};

            //Debug.Log(v1.ToString());
            //SaveManager.LoadSaveFile(Application.streamingAssetsPath+ "/Saves/ironman.sav");
            //if (uint.TryParse("4294967295",out var intValue))
            //{
            //    Debug.Log("intValue!");
            //}
            SaveManager.LoadSaveFileAsync(Application.streamingAssetsPath + "/Saves/Áù±Û1000.sav", null, (save) =>
            {
                foreach (var item in save.gameState.planets.planet)
                {
                    Debug.Log(item.Value.name.ToString());
                }
                foreach (var item in save.gameState.galactic_object)
                {
                    Debug.Log(item.Value.name.ToString());
                }
                foreach (var item in save.gameState.country)
                {
                    Debug.Log(item.Value.name.ToString());
                }
            });

            //var groups = Regex.Matches("%PARENT% %NUMERAL%", @"%(?<var>[^%]+)%");
            //foreach (Match group in groups)
            //{
            //    Debug.Log(group.Result("${var}"));
            //}

        }
    }
}
