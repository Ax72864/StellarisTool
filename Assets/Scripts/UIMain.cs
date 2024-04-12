using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StellarisTool
{
    public class UIMain : MonoBehaviour
    {
        public RectTransform node_buttons;
        public RectTransform node_loading;
        public Slider lod_loading;
        public Text lbl_loading;

        private bool InLoading = false;
        private bool NeedEnterMap = false;
        private bool ReturnMain = false;

        private void Start()
        {
            ShowMain();
        }

        public void GoLoad()
        {
            var filepath = SaveManager.WindowsOpenSelectFileDialog();
            if (!string.IsNullOrEmpty(filepath))
            {
                ShowLoading();
                NeedEnterMap = false;
                InLoading = true;
                loadingProgress = 0;
                SaveManager.LoadSaveFileAsync(filepath, OnLoadFileProgress, OnLoadFileComplete);
            }
        }

        public void GoSetting()
        {
            AssetManager.ChangeScene("scene_map");
        }

        public void GoExit()
        {
            Application.Quit();
        }

        public void ShowLoading()
        {
            node_buttons.SetActiveEx(false);
            node_loading.SetActiveEx(true);
        }

        public void ShowMain()
        {
            node_buttons.SetActiveEx(true);
            node_loading.SetActiveEx(false);
        }

        private float loadingProgress = 0f;
        public void OnLoadFileProgress(float progress)
        {
            loadingProgress = progress;
        }

        public void OnLoadFileComplete(StellarisSave save)
        {
            if (save != null)
            {
                NeedEnterMap = true;
            }
            else
            {
                ReturnMain = true;
            }
        }

        private void Update()
        {
            if (InLoading)
            {
                lod_loading.value = loadingProgress;
                lbl_loading.text = $"{(loadingProgress * 100).ToString("F2")}%";
            }
            if (NeedEnterMap)
            {
                AssetManager.ChangeScene("scene_map");
                NeedEnterMap = false;
            }
            if (ReturnMain)
            {
                ShowMain();
                ReturnMain = false;
                InLoading = false;
            }
        }
    }
}
