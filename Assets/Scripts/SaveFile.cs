using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StellarisTool;
using UnityEngine;

namespace StellarisTool
{


    public class SpecialOjbect
    {
        public static string 无限神机 = "guardian_sphere";
        public static string 以太龙 = "guardian_dragon";
        public static string 无尽堡垒 = "guardian_fortress";
        public static string 无畏舰 = "guardian_dreadnought";
        public static string 提杨凯长老 = "guardian_elderly_tiyanki";
        public static string 小行星蜂巢 = "guardians_hive_system";
        public static string 噬星虫 = "guardians_stellarite_system";
        public static string 位面异怪 = "guardians_horror_system";
        public static string 拾荒者 = "guardian_scavenger_bot";
        public static string 水晶之国 = "central_crystal_flag";
        public static string 天选民 = "chosen_system";
        public static string 避难所 = "sanctuary_system";
        public static string 极域警哨 = "surveillance_supercomputer_system";
        public static string 虚空文盲 = "hatchling_will_trigger";
        public static string 联邦之终 = "NAME_Unique_System_1";
        public static string 拉里昂内斯 = "NAME_Unique_System_2";
        public static string 泽沃克斯 = "NAME_Unique_System_3";
        public static string 芬拉克斯的抵抗 = "NAME_Unique_System_4";
        public static string 詹若克斯的休憩 = "NAME_Unique_System_5";
        public static string 阿法尔 = "NAME_Unique_System_6";
        public static string 缇杨娜之巢 = "NAME_Tiyana_Vek";
        public static string 缇云穴 = "NAME_Tiyun_Ort";
        public static string 爱神之槽 = "NAME_Amor_Alveo";
        public static string 温科沃特 = "NAME_Wenkwort";
        public static string 哈尔 = "NAME_Hauer";
        public static string 贺利托 = "NAME_Helito";
             


        public static string GetName(string specialName)
        {
            switch (specialName)
            {
                case "guardian_sphere":
                    return "无限神机";
                case "guardian_dragon":
                    return "以太龙";
                case "guardian_fortress":
                    return "无尽堡垒";
                case "guardian_dreadnought":
                    return "无畏舰";
                case "guardian_elderly_tiyanki":
                    return "提杨凯长老";
                case "guardians_hive_system":
                    return "小行星蜂巢";
                case "guardians_stellarite_system":
                    return "噬星虫";
                case "guardians_horror_system":
                    return "位面异怪";
                case "guardian_scavenger_bot":
                    return "拾荒者";
                case "central_crystal_flag":
                    return "水晶之国";
                case "chosen_system":
                    return "天选民";
                case "sanctuary_system":
                    return "避难所";
                case "surveillance_supercomputer_system":
                    return "极域警哨";
                case "hatchling_will_trigger":
                    return "虚空文盲";
                case "NAME_Unique_System_1":
                    return "联邦之终";
                case "NAME_Unique_System_2":
                    return "虚空文盲";
                case "NAME_Unique_System_3":
                    return "拉里昂内斯";
                case "NAME_Unique_System_4":
                    return "芬拉克斯的抵抗";
                case "NAME_Unique_System_5":
                    return "詹若克斯的休憩";
                case "NAME_Unique_System_6":
                    return "阿法尔";
                case "NAME_Tiyana_Vek":
                    return "缇杨娜之巢";
                case "NAME_Tiyun_Ort":
                    return "缇云穴";
                case "NAME_Amor_Alveo":
                    return "爱神之槽";
                case "NAME_Wenkwort":
                    return "温科沃特";
                case "NAME_Hauer":
                    return "哈尔";
                case "NAME_Helito":
                    return "贺利托";
                default:
                    return "NotFoundName";
            }
        }
    }
    public class StellarisSave
    {
        public StellarisGameState gameState;
        public StellarisGameMeta meta;


        //top flags
        public int gray_goo_stat = 0;//0:灰风 1:灰蛊风暴 2:L星龙 3:纳米帝国
        //in country type        
        //public long guardian_sphere = 0; //无限神机
        //public long guardian_dragon = 0; //以太龙
        //public long guardian_fortress = 0; //无尽堡垒
        //public long guardian_dreadnought = 0; //无畏舰
        //public long guardian_elderly_tiyanki = 0; //提杨凯长老
        //in galaxy_object flag
        public long[] LGates;
        public int fallen_machine_state = 0;//0:无 1:好 2:坏
        //public long guardians_hive_system = 0; //小行星蜂巢
        //public long guardians_stellarite_system = 0; //噬星虫
        //public long guardians_horror_system = 0; //位面异怪
        //public long guardian_scavenger_bot = 0; //拾荒者
        //public long central_crystal_flag = 0; //水晶之国
        //public long chosen_system = 0; //天选民

        //in planet flags
        //public long hatchling_will_trigger = 0; //虚空文盲

        //in Initializer
        //public bool sanctuary_system; //避难所
        //public bool surveillance_supercomputer_system; //极域警哨

        public Dictionary<long, string> CountryCapitals = new Dictionary<long, string>();
        public Dictionary<long, string> FallenEmpire = new Dictionary<long, string>();
        public Dictionary<long,long> WormholeLocation = new Dictionary<long, long>();
        public Dictionary<long,long> WormholePair = new Dictionary<long, long>();

        public Dictionary<string, long> SpecialSystem = new Dictionary<string, long>()
        {
            {SpecialOjbect.无限神机  ,-1},
            {SpecialOjbect.以太龙    ,-1},
            {SpecialOjbect.无尽堡垒  ,-1},
            {SpecialOjbect.无畏舰    ,-1},
            {SpecialOjbect.提杨凯长老,-1},
            {SpecialOjbect.小行星蜂巢,-1},
            {SpecialOjbect.噬星虫    ,-1},
            {SpecialOjbect.位面异怪  ,-1},
            {SpecialOjbect.拾荒者    ,-1},
            {SpecialOjbect.水晶之国  ,-1},
            {SpecialOjbect.天选民    ,-1},
            {SpecialOjbect.避难所    ,-1},
            {SpecialOjbect.极域警哨  ,-1},
            {SpecialOjbect.虚空文盲  ,-1},
            {SpecialOjbect.联邦之终  ,-1},
            {SpecialOjbect.拉里昂内斯  ,-1},
            {SpecialOjbect.泽沃克斯  ,-1},
            {SpecialOjbect.芬拉克斯的抵抗  ,-1},
            {SpecialOjbect.詹若克斯的休憩  ,-1},
            {SpecialOjbect.阿法尔  ,-1},
            {SpecialOjbect.缇杨娜之巢  ,-1},
            {SpecialOjbect.缇云穴  ,-1},
            {SpecialOjbect.爱神之槽  ,-1},
            {SpecialOjbect.温科沃特  ,-1},
            {SpecialOjbect.哈尔  ,-1},
            {SpecialOjbect.贺利托  ,-1},
        };
        public string[] FindInCountryType = new string[]
        {
            SpecialOjbect.无限神机,
            SpecialOjbect.以太龙,
            SpecialOjbect.无尽堡垒,
            SpecialOjbect.提杨凯长老,
            SpecialOjbect.无畏舰,
            SpecialOjbect.拾荒者
        };
        public string[] FindInStarTag = new string[]
        {
            SpecialOjbect.小行星蜂巢,
            SpecialOjbect.极域警哨,
            SpecialOjbect.位面异怪,
            SpecialOjbect.水晶之国,
            SpecialOjbect.天选民
        };
        public string[] FindInStarName = new string[]
        {
            SpecialOjbect.联邦之终,
            SpecialOjbect.拉里昂内斯,
            SpecialOjbect.泽沃克斯,
            SpecialOjbect.芬拉克斯的抵抗,
            SpecialOjbect.詹若克斯的休憩,
            SpecialOjbect.阿法尔,
            SpecialOjbect.缇杨娜之巢,
            SpecialOjbect.缇云穴,
            SpecialOjbect.爱神之槽,
            SpecialOjbect.温科沃特,
            SpecialOjbect.哈尔,
            SpecialOjbect.贺利托,
        };
        public void ScanBaseInfo()
        {


            foreach (var initStr in gameState.system_initializer_counter.initializer)
            {
                if (initStr == "sanctuary_system")
                {
                    SpecialSystem[SpecialOjbect.避难所] = 0;
                }
                else if (initStr == "surveillance_supercomputer_system")
                {
                    SpecialSystem[SpecialOjbect.极域警哨] = 0;
                }
            }
            var planets = gameState.planets.planet;
            foreach (var kv in planets)
            {
                var planet = kv.Value;
                if (planet.flags != null)
                {
                    if (planet.flags.ContainsKey("hatchling_will_trigger"))
                    {
                        SpecialSystem[SpecialOjbect.虚空文盲] = planet.coordinate.origin;
                    }
                }
            }
            var countrys = gameState.country;
            foreach (var kv in countrys)
            {
                var country = kv.Value;

                for (int i = 0; i < FindInCountryType.Length; i++)
                {
                    if (SpecialSystem[FindInCountryType[i]] < 0 && country.type == FindInCountryType[i])
                    {
                        var fleetId = country.fleets_manager.owned_fleets[0].fleet;
                        var fleet = GetFleet(fleetId);
                        SpecialSystem[FindInCountryType[i]] = fleet.movement_manager.coordinate.origin;
                    }
                }
                if (country.type == "fallen_empire")
                {
                    var fleetId = country.fleets_manager.owned_fleets[0].fleet;
                    var fleet = GetFleet(fleetId);
                    FallenEmpire.Add(fleet.movement_manager.coordinate.origin, country.name.ToString());
                } else
                {
                    CountryCapitals[country.starting_system] = country.name.ToString();
                }
            }

            var stars = gameState.galactic_object;
            var lgateIds = new List<long>();
            foreach (var kv in stars)
            {
                var star = kv.Value;
                if (star.flags != null)
                {
                    if (star.flags.ContainsKey("lgate"))
                    {
                        lgateIds.Add(kv.Key);
                    }
                    if (star.flags.ContainsKey("fallen_machine_empire_awaken_1"))
                    {
                        fallen_machine_state = 1;
                    }
                    else if (star.flags.ContainsKey("fallen_machine_empire_awaken_2"))
                    {
                        fallen_machine_state = 2;
                    }
                    for (int i = 0; i < FindInStarTag.Length; i++)
                    {
                        if (SpecialSystem[FindInStarTag[i]] < 0 && star.flags.ContainsKey(FindInStarTag[i]))
                        {
                            SpecialSystem[FindInStarTag[i]] = kv.Key;
                        }
                    }
                }
                var idx = Array.IndexOf(FindInStarName, star.name.ToString());
                if (idx > -1)
                {
                    SpecialSystem[FindInStarName[idx]] = kv.Key;
                }
                if (star.bypasses != null)
                {
                    for (int i = 0; i < star.bypasses.Length; i++)
                    {
                        var bypassId = star.bypasses[i];
                        var bypass = gameState.bypasses[bypassId];
                        if (bypass.type == "wormhole")
                        {
                            WormholeLocation[bypassId] = kv.Key;
                            WormholePair[bypassId] = bypass.linked_to;
                        }
                    }
                }
            }

            LGates = lgateIds.ToArray();
        }

        public GalacticObject GetStar(long id)
        {
            gameState.galactic_object.TryGetValue(id, out var star);
            return star;
        }

        public Planet GetPlanet(long id)
        {
            gameState.planets.planet.TryGetValue(id, out var planet);
            return planet;
        }

        public Country GetCountry(long id)
        {
            gameState.country.TryGetValue(id, out var country);
            return country;
        }
        public Fleet GetFleet(long id)
        {
            gameState.fleet.TryGetValue(id, out var fleet);
            return fleet;
        }

        public Bypass GetBypass(long id)
        {
            gameState.bypasses.TryGetValue(id, out var bypass);
            return bypass;
        }
        public Megastructure GetMega(long id)
        {
            gameState.megastructures.TryGetValue(id, out var mega);
            return mega;
        }
    }


}
