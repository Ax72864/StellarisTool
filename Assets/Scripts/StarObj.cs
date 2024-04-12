using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using YooAsset;

namespace StellarisTool
{
    public class StarObj : MonoBehaviour
    {
        public SpriteRenderer baseSpr;
        public TextMeshPro text_name;

        public SpriteRenderer sp_mega;
        public SpriteRenderer sp_hostile;
        public SpriteRenderer sp_pc1;
        public TextMeshPro lbl_pc1;
        public SpriteRenderer sp_pc2;
        public TextMeshPro lbl_pc2;
        public SpriteRenderer sp_pc3;
        public TextMeshPro lbl_pc3;
        public SpriteRenderer sp_prestl;
        public TextMeshPro lbl_prestl;
        public SpriteRenderer sp_enclave;
        public TextMeshPro lbl_enclave;
        public SpriteRenderer sp_gate;
        public SpriteRenderer sp_lgate;
        public SpriteRenderer sp_wormhole;
        public Transform tag_root;
        public Transform attach_root;

        public long id;
        public GalacticObject info;

        public int colonizableCount = 0;
        public SpriteRenderer[] sp_pcs = new SpriteRenderer[3];
        public TextMeshPro[] lbl_pcs = new TextMeshPro[3];
        private void Awake()
        {
            sp_pcs[0] = sp_pc1;
            sp_pcs[1] = sp_pc2;
            sp_pcs[2] = sp_pc3;

            lbl_pcs[0] = lbl_pc1;
            lbl_pcs[1] = lbl_pc2;
            lbl_pcs[2] = lbl_pc3;

        }

        public void SetStar(long id,GalacticObject star)
        {
            this.id = id;
            info = star;
            baseSpr.material = Drawer.starMaterial;
            baseSpr.sprite = AssetManager.GetStarSprite(info.star_class);
            text_name.text = info.name.ToString();
            UpdateIcon();
        }


        public void UpdateIcon()
        {
            sp_mega.gameObject.SetActiveEx(false);
            sp_hostile.gameObject.SetActiveEx(false);
            sp_pc1.gameObject.SetActiveEx(false);
            sp_pc2.gameObject.SetActiveEx(false);
            sp_pc3.gameObject.SetActiveEx(false);
            sp_prestl.gameObject.SetActiveEx(false);
            sp_enclave.gameObject.SetActiveEx(false);
            sp_gate.gameObject.SetActiveEx(false);
            sp_lgate.gameObject.SetActiveEx(false);
            sp_wormhole.gameObject.SetActiveEx(false);

            if (info.bypasses != null)
            {
                foreach (var bypassId in info.bypasses)
                {
                    var bypass = SaveManager.saveData.GetBypass(bypassId);
                    if (bypass == null) continue;
                    if (bypass.type == "lgate")
                    {
                        sp_lgate.gameObject.SetActiveEx(true);
                    }
                    else if (bypass.type == "gateway")
                    {
                        sp_gate.gameObject.SetActiveEx(true);
                    }
                    else if (bypass.type == "wormhole")
                    {
                        sp_wormhole.gameObject.SetActive(true);
                    }
                }
            }

            if (info.megastructures != null)
            {

                foreach (var megaId in info.megastructures)
                {
                    var mega = SaveManager.saveData.GetMega(megaId);
                    if (GlobalDefine.Megastructure.ContainsKey(mega.type))
                    {
                        sp_mega.gameObject.SetActiveEx(true);
                        sp_mega.sprite = AssetManager.GetSprite(GlobalDefine.Megastructure[mega.type]);
                    }
                }
            }

            if (info.flags != null)
            {
                foreach (var kv in info.flags)
                {
                    var tag = kv.Key;
                    switch (tag)
                    {
                        case "guardians_artists_system":
                            lbl_enclave.gameObject.SetActiveEx(true);
                            lbl_enclave.text = "艺术家";
                            sp_enclave.gameObject.SetActiveEx(true);
                            sp_enclave.sprite = AssetManager.GetShipClassSprite("sc_artists");
                            break;
                        case "guardians_curators_system":
                            lbl_enclave.gameObject.SetActiveEx(true);
                            lbl_enclave.text = "策展人";
                            sp_enclave.gameObject.SetActiveEx(true);
                            sp_enclave.sprite = AssetManager.GetShipClassSprite("sc_curators");
                            break;
                        case "salvager_enclave_system":
                            lbl_enclave.gameObject.SetActiveEx(true);
                            lbl_enclave.text = "打捞者";
                            sp_enclave.gameObject.SetActiveEx(true);
                            sp_enclave.sprite = AssetManager.GetShipClassSprite("sc_salvager");
                            break;
                        case "shroudwalker_enclave_system":
                            lbl_enclave.gameObject.SetActiveEx(true);
                            lbl_enclave.text = "虚境导师";
                            sp_enclave.gameObject.SetActiveEx(true);
                            sp_enclave.sprite = AssetManager.GetShipClassSprite("sc_shroudwalker");
                            break;
                        case "guardians_traders_system":
                            lbl_enclave.gameObject.SetActiveEx(true);
                            lbl_enclave.text = "贸易城邦";
                            sp_enclave.gameObject.SetActiveEx(true);
                            sp_enclave.sprite = AssetManager.GetShipClassSprite("sc_traders");
                            break;
                        case "caravaneer_home":
                            lbl_enclave.gameObject.SetActiveEx(true);
                            lbl_enclave.text = "行商联盟";
                            sp_enclave.gameObject.SetActiveEx(true);
                            sp_enclave.sprite = AssetManager.GetShipClassSprite("sc_traders");
                            break;
                        default:
                            break;
                    }
                    if (GlobalDefine.Hostile.ContainsKey(tag))
                    {
                        sp_hostile.gameObject.SetActiveEx(true);
                    }
                }

            }

            if (info.planet != null)
            {
                try
                {
                    var planets = JsonConvert.DeserializeObject<long[]>(info.planet.ToString());
                    foreach (var planetId in planets)
                    {
                        
                        var planet = SaveManager.saveData.GetPlanet(planetId);
                        if (planet.flags!= null && planet.flags.ContainsKey("pre_ftl_default"))
                        {
                            sp_prestl.gameObject.SetActiveEx(true);
                            lbl_prestl.text = planet.planet_size.ToString();
                        }
                        else if (planet.owner < 0 && GlobalDefine.ColonizablePlanetClass.ContainsKey(planet.planet_class))
                        {
                            if (colonizableCount < 3)
                            {
                                sp_pcs[colonizableCount].gameObject.SetActiveEx(true);
                                sp_pcs[colonizableCount].sprite = AssetManager.GetPlanetSprite(GlobalDefine.ColonizablePlanetClass[planet.planet_class]);
                                lbl_pcs[colonizableCount].text = planet.planet_size.ToString();
                            }
                            colonizableCount++;
                        }
                    }
                }
                catch (System.Exception)
                {
                    var planetId = JsonConvert.DeserializeObject<long>(info.planet.ToString());
                    var planet = SaveManager.saveData.GetPlanet(planetId);
                    if (planet.flags!=null&&planet.flags.ContainsKey("pre_ftl_default"))
                    {
                        sp_prestl.gameObject.SetActiveEx(true);
                        lbl_prestl.text = planet.planet_size.ToString();
                    }
                    else if (planet.owner < 0 && GlobalDefine.ColonizablePlanetClass.ContainsKey(planet.planet_class))
                    {
                        if (colonizableCount < 3)
                        {
                            sp_pcs[colonizableCount].gameObject.SetActiveEx(true);
                            sp_pcs[colonizableCount].sprite = AssetManager.GetPlanetSprite(GlobalDefine.ColonizablePlanetClass[planet.planet_class]);
                            lbl_pcs[colonizableCount].text = planet.planet_size.ToString();
                        }
                        colonizableCount++;
                    }
                }
            }

        }

        public void Attach(GameObject go,bool clear = false)
        {
            if (clear)
            {
                attach_root.ClearAllChildren();
            }
            go.transform.SetParent(attach_root, false);
        }

        public void ClearAttach()
        {
            attach_root.ClearAllChildren();
        }

        //private void Update()
        //{
        //    text_name.transform.LookAt(Camera.main.transform);
        //}
    }
}
