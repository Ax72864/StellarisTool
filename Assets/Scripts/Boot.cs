using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace StellarisTool
{
    public class Boot : MonoBehaviour
    {
        public static Boot Instance;
        private static MonoBehaviour _behaviour;

        public RectTransform node_buttons;
        public RectTransform node_loading;
        public Slider lod_loading;
        public Text lbl_loading;

        private static string DefaultWorkingDir;

        private IEnumerator InitializeYooAsset()
        {
            var packageName = "MainPackage";
            YooAssets.Initialize();
            var package = YooAssets.CreatePackage(packageName);
            YooAssets.SetDefaultPackage(package);
            Debug.Log("YooAssets Load Package");
#if UNITY_EDITOR
            InitializationOperation initializationOperation = null;
            var createParameters = new EditorSimulateModeParameters();
            createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
            initializationOperation = package.InitializeAsync(createParameters);
            yield return initializationOperation;
#else
            var initParameters = new OfflinePlayModeParameters();
            yield return package.InitializeAsync(initParameters);
#endif
        }
        private IEnumerator Init()
        {
            Debug.Log("[Boot]Start Init...");
            yield return InitializeYooAsset();
            Debug.Log("[Boot]YooAssets Init Done");
            yield return AssetManager.Init();
            Debug.Log("All resource initialed!");
            DontDestroyOnLoad(gameObject);
            DefaultWorkingDir = Directory.GetCurrentDirectory();
            Debug.Log($"DefaultWorkingDir:{DefaultWorkingDir}");
        }

        private void Awake()
        {
            if (Instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }
            _behaviour = this as MonoBehaviour;
            Instance = this;
            StartCoroutine(Init());
        }

        public static new Coroutine StartCoroutine(IEnumerator ie)
        {
            return _behaviour.StartCoroutine(ie);
        }
        public static new void StopCoroutine(IEnumerator ie)
        {
            _behaviour.StopCoroutine(ie);
        }
        public static new void StopAllCoroutines()
        {
            _behaviour.StopAllCoroutines();
        }
        private void OnApplicationQuit()
        {
            Debug.Log($"Reset DefaultWorkingDir : {Directory.GetCurrentDirectory()} => {DefaultWorkingDir}");
            Directory.SetCurrentDirectory(DefaultWorkingDir);
        }
    }
}
