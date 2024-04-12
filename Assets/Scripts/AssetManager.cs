using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using YooAsset;

namespace StellarisTool
{
    public class AssetManager
    {
        //private static AssetManager _instance;
        //public static AssetManager Instance {
        //    get {
        //        if (_instance == null)
        //        {
        //            _instance = new AssetManager();
        //        }
        //        return _instance;
        //    }
        //}
        public static AssetOperationHandle LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            var handler = YooAssets.LoadAssetAsync<T>(path);
            return handler;
        }

        public static Dictionary<string, Sprite> _cachedSprite = new Dictionary<string, Sprite>();
        public static Dictionary<string,GameObject> _cachedGameObject = new Dictionary<string, GameObject>();
        public static Dictionary<string, SubAssetsOperationHandle> _cachedAtlas = new Dictionary<string, SubAssetsOperationHandle>();
        //public static Dictionary<string, Sprite> _cachedAtlasSprite = new Dictionary<string, SubAssetsOperationHandle>();
        public static Dictionary<string, Sprite> starSprites = new Dictionary<string, Sprite>();
        //public static Dictionary<StarClass, string> starRes = new Dictionary<StarClass, string>()
        //{
        //    { StarClass.sc_b,"b_star"},
        //    { StarClass.sc_a,"a_star"},
        //    { StarClass.sc_f,"f_star"},
        //    { StarClass.sc_g,"g_star"},
        //    { StarClass.sc_k,"k_star"},
        //    { StarClass.sc_m,"m_star"},
        //    { StarClass.sc_m_giant,"sc_m_giant"},
        //    { StarClass.sc_t,"t_star"},
        //    { StarClass.sc_black_hole,"black_hole"},
        //    { StarClass.sc_neutron_star,"neutron_star"},
        //    { StarClass.sc_pulsar,"pulsar"},
        //    { StarClass.sc_binary_1,"a_binary_star"},
        //    { StarClass.sc_trinary_1,"a_trinary_star"},
        //};
        public static Dictionary<string, string> starRes = new Dictionary<string, string>()
        {
            { "sc_b","b_star"},
            { "sc_a","a_star"},
            { "sc_f","f_star"},
            { "sc_g","g_star"},
            { "sc_k","k_star"},
            { "sc_m","m_star"},
            { "sc_m_giant","sc_m_giant"},
            { "sc_t","t_star"},
            { "sc_black_hole","black_hole"},
            { "sc_neutron_star","neutron_star"},
            { "sc_pulsar","pulsar"},
            { "sc_binary","a_binary_star"},
            { "sc_trinary","a_trinary_star"},
            { "common","x_star"},
        };

        private static List<string> _preloadRes = new List<string>
        {
            "trail",
            "x_star",
            "b_star",
            "a_star",
            "f_star",
            "g_star",
            "k_star",
            "m_star",
            "sc_m_giant",
            "t_star",
            "black_hole",
            "neutron_star",
            "pulsar",
            "a_binary_star",
            "a_trinary_star",
            "missing",
        };

        private static List<string> _preloadGameObject = new List<string>
        {
            "InfoLine",
            "StarObj",
            "target_arrow",
            "target_circle",
            "target_special",
            "select_icon"
        };

        private static List<string> _preloadAtlas = new List<string>
        {
            "planet_type_icons",
            "ship_classes_medium",
        };


        public static IEnumerator Init()
        {

            Debug.Log($"[AssetManager]Start init...");
            foreach (var path in _preloadRes)
            {
                var handler = YooAssets.LoadAssetAsync<Sprite>(path);
                yield return handler;
                if (handler.IsDone)
                {
                    _cachedSprite.Add(path, handler.AssetObject as Sprite);
                }
            }

            Debug.Log($"[AssetManager]Preloaded {_cachedSprite.Count} sprite");
            starSprites["common"] = _cachedSprite["x_star"];

            foreach (var path in _preloadAtlas)
            {
                var handler = YooAssets.LoadSubAssetsAsync<Sprite>(path);
                yield return handler;
                if (handler.IsDone)
                {
                    _cachedAtlas.Add(path, handler);
                }
            }
            foreach (var kv in starRes)
            {
                starSprites[kv.Value] = _cachedSprite[kv.Value];
            }
            //foreach (StarClass sc in Enum.GetValues(typeof(StarClass)))
            //{
            //    if (starRes.ContainsKey(sc))
            //    {
            //        starSprites[sc] = _cachedSprite[starRes[sc]];
            //    }
            //    else
            //    {
            //        starSprites[sc] = _cachedSprite["x_star"];
            //    }
            //}

            foreach (var path in _preloadGameObject)
            {
                var handler = YooAssets.LoadAssetAsync<GameObject>(path);
                yield return handler;
                if (handler.IsDone)
                {
                    _cachedGameObject.Add(path, handler.AssetObject as GameObject);
                }
            }
            Debug.Log($"[AssetManager]Preloaded {_cachedGameObject.Count} Prefab");
            yield return null;

            Debug.Log($"[AssetManager]Init finish!");
        }

        public static Sprite GetSprite(string path)
        {
            if (_cachedSprite.ContainsKey(path))
            {
                return _cachedSprite[path];
            } else
            {
                if (YooAssets.GetAssetInfo(path)!= null)
                {
                    return YooAssets.LoadAssetSync<Sprite>(path).AssetObject as Sprite;
                }

                return _cachedSprite["missing"];
            }
        }
        public static Sprite GetStarSprite(string sc)
        {
            starRes.TryGetValue(sc, out var sckey);
            if (string.IsNullOrEmpty(sckey))
            {
                var sp = sc.Split('_');
                if (sp.Length > 1)
                {
                    sckey = $"{sp[0]}_{sp[1]}";
                }
            }

            if (string.IsNullOrEmpty(sckey))
            {
                sckey = starRes["common"];
            }
            starSprites.TryGetValue(sckey, out var sprite);
            if (sprite == null)
            {
                //Debug.Log($"sprite not found: {sc} =>{sckey}");
                sprite = starSprites["common"];
            }
            return sprite;
        }

        public static Sprite GetPlanetSprite(string pc)
        {
            //return GetSprite(pc);
            var tp = _cachedAtlas["planet_type_icons"].GetSubAssetObject<Sprite>(pc);
            if (tp != null)
            {
                return tp;
            }
            return _cachedAtlas["planet_type_icons"].GetSubAssetObject<Sprite>("pc_missing");
        }

        public static Sprite GetShipClassSprite(string sc)
        {
            var tp = _cachedAtlas["ship_classes_medium"].GetSubAssetObject<Sprite>(sc);
            if (tp != null)
            {
                return tp;
            }
            return _cachedAtlas["ship_classes_medium"].GetSubAssetObject<Sprite>("sc_missing");
        }


        public static GameObject GetObject(string path)
        {
            _cachedGameObject.TryGetValue(path, out var obj);
            if (obj == null)
            {
                if (YooAssets.GetAssetInfo(path) != null)
                {
                    obj = YooAssets.LoadAssetSync<GameObject>(path).AssetObject as GameObject;
                }
            }
            return UnityEngine.Object.Instantiate(obj);
        }

        public static void ChangeScene(string scene)
        {
            Boot.StartCoroutine(LoadScene(scene));
        }
        private static IEnumerator LoadScene(string path)
        {
            var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
            var package = YooAssets.TryGetPackage("MainPackage");
            var handler = package.LoadSceneAsync(path, sceneMode, false);
            yield return handler;
            Debug.Log($"Change to Scene {handler.SceneObject.name} success");
        }
    }
}
