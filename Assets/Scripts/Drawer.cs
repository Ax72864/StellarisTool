using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using LitFramework;
using UnityEngine.UI;
using TMPro;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace StellarisTool
{
    public class Drawer : MonoBehaviour
    {

        public Material starMat;
        public Material hyperlaneMat;
        public GameObject starRoot;
        public GameObject hyperlaneRoot;
        public GameObject upperRoot;
        public GameObject starPrefab;

        public UIRightPanel rightPanel;

        private StellarisSave saveData;

        private bool Initialed = false;
        public static Drawer Instance;

        public static Material starMaterial;
        public static Material hyperlaneMaterial;

        private int StarObjLayerMask = 8;
        private PhysicsRaycaster rayCaster;

        private Transform node_star_select;
        private IEnumerator Init()
        {
            //Debug.Log("Start Init...");
            //yield return InitializeYooAsset();
            //Debug.Log("YooAssets Init Done");
            starMaterial = starMat;
            hyperlaneMaterial = hyperlaneMat;
            saveData = SaveManager.saveData;
            Debug.Log("Load saveData success");
            yield return null;
            rightPanel.SetSaveData(saveData);
            starMap.Clear();
            InitStarSelectIcon();
            DrawStar();
            rayCaster = Camera.main.GetComponent<PhysicsRaycaster>();
            Initialed = true;
        }

        private void Awake()
        {
            Instance = this;
            StarObjLayerMask = LayerMask.NameToLayer("starobj");
            StartCoroutine(Init());

        }

        private void Start()
        {
            LitEventSystem.Global.Register<EventFocusSystem>(OnEventFocusSystem);
        }

        private void OnDestroy()
        {
            LitEventSystem.Global.UnRegister<EventFocusSystem>(OnEventFocusSystem);
        }

        public void InitStarSelectIcon()
        {
            var go = AssetManager.GetObject("select_icon");
            go.transform.SetParent(upperRoot.transform, false);
            node_star_select = go.transform;
            node_star_select.SetActiveEx(false);
        }

        public Dictionary<long, StarObj> starMap = new Dictionary<long, StarObj>();
        public void DrawStar()
        {
            starRoot.transform.ClearAllChildren();
            var stars = saveData.gameState.galactic_object;

            foreach (var kv in stars)
            {
                var star = kv.Value;
                var go = Instantiate(starPrefab,starRoot.transform);
                go.transform.localPosition = new Vector3(star.coordinate.x, star.coordinate.y, 0);
                go.name = star.name.ToString();
                go.transform.localScale = Vector3.one * 3f;
                go.transform.localRotation = Quaternion.Euler(0, 0, 180);
                var starComp = go.GetComponent<StarObj>();
                starComp.SetStar(kv.Key,star);
                starMap[kv.Key] = starComp;
            }

            foreach (var kv in starMap)
            {
                var srcStar = kv.Value;
                var hyperlanes = kv.Value.info.hyperlane;
                if (hyperlanes!= null)
                {
                    for (int i = 0; i < hyperlanes.Length; i++)
                    {
                        var targetStar = starMap[hyperlanes[i].to];
                        var distance = Vector3.Distance(srcStar.transform.position, targetStar.transform.position) / starRoot.transform.localScale.x;
                        var lane = new GameObject($"{srcStar.info.name}<==>{targetStar.info.name}");
                        Vector3 direction = targetStar.transform.localPosition - srcStar.transform.localPosition;
                        lane.transform.SetParent(hyperlaneRoot.transform, false);
                        lane.transform.localPosition = 0.5f * direction + new Vector3(srcStar.transform.localPosition.x, srcStar.transform.localPosition.y,0);
                        lane.transform.localScale = new Vector3(distance * 0.39f, 0.3f, 1);
                        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        lane.transform.localRotation = Quaternion.Euler(0, 0, angle);
                        var sr = lane.AddComponent<SpriteRenderer>();
                        sr.sprite = AssetManager.GetSprite("trail");
                        sr.material = hyperlaneMat;
                    }
                }
            }

            foreach (var kv in saveData.WormholeLocation)
            {
                var wormholeId = kv.Key;
                var linkedTo = saveData.WormholePair[wormholeId];
                var srcStar = starMap[kv.Value];
                var targetStar = starMap[saveData.WormholeLocation[linkedTo]];

                var distance = Vector3.Distance(srcStar.transform.position, targetStar.transform.position) / starRoot.transform.localScale.x;
                var lane = new GameObject($"{srcStar.info.name}<==OO==>{targetStar.info.name}");
                Vector3 direction = targetStar.transform.localPosition - srcStar.transform.localPosition;
                lane.transform.SetParent(hyperlaneRoot.transform, false);
                lane.transform.localPosition = 0.25f * direction + new Vector3(srcStar.transform.localPosition.x, srcStar.transform.localPosition.y, 0);
                //lane.transform.localPosition = new Vector3(srcStar.transform.localPosition.x, srcStar.transform.localPosition.y, 0);
                lane.transform.localScale = new Vector3(distance * 0.195f, 0.3f, 1);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                lane.transform.localRotation = Quaternion.Euler(0, 0, angle);
                var sr = lane.AddComponent<SpriteRenderer>();
                sr.sprite = AssetManager.GetSprite("trail_2");
                sr.material = hyperlaneMat;
            }

        }

        public void ClearAll()
        {
            foreach (var item in starMap)
            {
                item.Value.ClearAttach();
            }
        }
        public void HighLightSelf()
        {
            var country = saveData.GetCountry(0);
            HighLight(new long[] { country.starting_system },4);
        }
        public void HighLightCapitals()
        {
            var ids = new List<long>();
            var texts = new List<string>();
            foreach (var kv in saveData.CountryCapitals)
            {
                ids.Add(kv.Key);
                texts.Add(kv.Value);
            }
            HighLight(ids.ToArray(), 3, texts.ToArray());
        }

        public void HighLightFallenEmpire()
        {
            var ids = new List<long>();
            var texts = new List<string>();
            foreach (var kv in saveData.FallenEmpire)
            {
                ids.Add(kv.Key);
                texts.Add(kv.Value);
            }
            HighLight(ids.ToArray(), 5, texts.ToArray());
        }
        public void HighLightLGate()
        {
            Debug.Log($"L-Gate Ids:{string.Join(',', saveData.LGates)}");
            HighLight(saveData.LGates);
        }


        public void HighLightSpecialSystem()
        {
            var ids = new List<long>();
            var texts = new List<string>();

            foreach (var kv in saveData.SpecialSystem)
            {
                if (kv.Value > 0)
                {
                    ids.Add(kv.Value);
                    texts.Add(SpecialOjbect.GetName(kv.Key));
                }
            }
            HighLight(ids.ToArray(), 2,texts.ToArray());
        }

        public void HighLight(long[] objId,int type = 1, string[] texts = null)
        {
            for (int i = 0; i < objId.Length; i++)
            {
                if (starMap.TryGetValue(objId[i], out var star))
                {
                    if (type == 1)
                    {
                        star.Attach(AssetManager.GetObject("target_arrow"), true);
                    } else if (type == 2)
                    {
                        var go = AssetManager.GetObject("target_special");
                        go.GetComponentInChildren<TextMeshPro>().text = texts[i];
                        star.Attach(go, false);

                    } else if (type == 3)
                    {
                        var go = AssetManager.GetObject("target_circle");
                        go.GetComponentInChildren<TextMeshPro>().text = texts[i];
                        star.Attach(go, false);
                    }
                    else if (type == 4)
                    {
                        var go = AssetManager.GetObject("target_arrow");
                        go.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
                        star.Attach(go, true);
                        FocusCameraTo(objId[i]);
                    }
                    else if (type == 5)
                    {
                        var go = AssetManager.GetObject("target_arrow");
                        go.GetComponentInChildren<SpriteRenderer>().color = Color.red;
                        star.Attach(go, false);
                    }
                    else if (type == 6)
                    {
                        var go = AssetManager.GetObject("target_arrow");
                        go.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
                        star.Attach(go, false);
                    }
                    else if (type == 7)
                    {
                        var go = AssetManager.GetObject("target_arrow");
                        go.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
                        star.Attach(go, false);
                    }
                }
            }
        }
        public void FocusCameraTo(long id)
        {
            var cameraRoot = Camera.main.transform.parent;
            var target = starMap[id];
            var newPos = new Vector3(target.transform.position.x,cameraRoot.position.y, target.transform.position.z);
            Camera.main.transform.DOMove(newPos, 0.5f).SetEase(Ease.OutCubic);
        }

        public void ShowStarPreview(long id)
        {

        }

        public void GoBack()
        {
            AssetManager.ChangeScene("scene_main");
        }

        public void OnEventFocusSystem(EventFocusSystem e)
        {
            HighLight(new[] { e.id }, e.type);
            //FocusCameraTo(e.id);
        }

        List<RaycastResult> results = new List<RaycastResult>();

        public GameObject GetOverGameObject()
        {
            results.Clear();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray,out var hit,100f,1<<StarObjLayerMask))
            {
                return hit.collider.gameObject;
            }
            return null;
        }

        private void Update()
        {
            if (!Initialed || OrthoCameraController.Draging)
            {
                return;
            }

            var gameObj = GetOverGameObject();
            if (gameObj != null)
            {
                node_star_select.SetActiveEx(true);
                node_star_select.position = gameObj.transform.position;
            }
            else
            {
                node_star_select.SetActiveEx(false);
            }
            if (Input.GetMouseButtonDown(0))
            {
                var star = gameObj?.GetComponent<StarObj>();
                if (star != null)
                {
                    Debug.Log($"Star:{star.info.name}");
                    ShowStarPreview(star.id);
                }
            }
        }
    }
}
