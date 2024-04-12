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
        public static string ������� = "guardian_sphere";
        public static string ��̫�� = "guardian_dragon";
        public static string �޾����� = "guardian_fortress";
        public static string ��η�� = "guardian_dreadnought";
        public static string ������� = "guardian_elderly_tiyanki";
        public static string С���Ƿ䳲 = "guardians_hive_system";
        public static string ���ǳ� = "guardians_stellarite_system";
        public static string λ����� = "guardians_horror_system";
        public static string ʰ���� = "guardian_scavenger_bot";
        public static string ˮ��֮�� = "central_crystal_flag";
        public static string ��ѡ�� = "chosen_system";
        public static string ������ = "sanctuary_system";
        public static string ������ = "surveillance_supercomputer_system";
        public static string �����ä = "hatchling_will_trigger";
        public static string ����֮�� = "NAME_Unique_System_1";
        public static string ���ﰺ��˹ = "NAME_Unique_System_2";
        public static string ���ֿ�˹ = "NAME_Unique_System_3";
        public static string ������˹�ĵֿ� = "NAME_Unique_System_4";
        public static string ղ����˹����� = "NAME_Unique_System_5";
        public static string ������ = "NAME_Unique_System_6";
        public static string �����֮�� = "NAME_Tiyana_Vek";
        public static string ���Ѩ = "NAME_Tiyun_Ort";
        public static string ����֮�� = "NAME_Amor_Alveo";
        public static string �¿����� = "NAME_Wenkwort";
        public static string ���� = "NAME_Hauer";
        public static string ������ = "NAME_Helito";
             


        public static string GetName(string specialName)
        {
            switch (specialName)
            {
                case "guardian_sphere":
                    return "�������";
                case "guardian_dragon":
                    return "��̫��";
                case "guardian_fortress":
                    return "�޾�����";
                case "guardian_dreadnought":
                    return "��η��";
                case "guardian_elderly_tiyanki":
                    return "�������";
                case "guardians_hive_system":
                    return "С���Ƿ䳲";
                case "guardians_stellarite_system":
                    return "���ǳ�";
                case "guardians_horror_system":
                    return "λ�����";
                case "guardian_scavenger_bot":
                    return "ʰ����";
                case "central_crystal_flag":
                    return "ˮ��֮��";
                case "chosen_system":
                    return "��ѡ��";
                case "sanctuary_system":
                    return "������";
                case "surveillance_supercomputer_system":
                    return "������";
                case "hatchling_will_trigger":
                    return "�����ä";
                case "NAME_Unique_System_1":
                    return "����֮��";
                case "NAME_Unique_System_2":
                    return "�����ä";
                case "NAME_Unique_System_3":
                    return "���ﰺ��˹";
                case "NAME_Unique_System_4":
                    return "������˹�ĵֿ�";
                case "NAME_Unique_System_5":
                    return "ղ����˹�����";
                case "NAME_Unique_System_6":
                    return "������";
                case "NAME_Tiyana_Vek":
                    return "�����֮��";
                case "NAME_Tiyun_Ort":
                    return "���Ѩ";
                case "NAME_Amor_Alveo":
                    return "����֮��";
                case "NAME_Wenkwort":
                    return "�¿�����";
                case "NAME_Hauer":
                    return "����";
                case "NAME_Helito":
                    return "������";
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
        public int gray_goo_stat = 0;//0:�ҷ� 1:�ҹƷ籩 2:L���� 3:���׵۹�
        //in country type        
        //public long guardian_sphere = 0; //�������
        //public long guardian_dragon = 0; //��̫��
        //public long guardian_fortress = 0; //�޾�����
        //public long guardian_dreadnought = 0; //��η��
        //public long guardian_elderly_tiyanki = 0; //�������
        //in galaxy_object flag
        public long[] LGates;
        public int fallen_machine_state = 0;//0:�� 1:�� 2:��
        //public long guardians_hive_system = 0; //С���Ƿ䳲
        //public long guardians_stellarite_system = 0; //���ǳ�
        //public long guardians_horror_system = 0; //λ�����
        //public long guardian_scavenger_bot = 0; //ʰ����
        //public long central_crystal_flag = 0; //ˮ��֮��
        //public long chosen_system = 0; //��ѡ��

        //in planet flags
        //public long hatchling_will_trigger = 0; //�����ä

        //in Initializer
        //public bool sanctuary_system; //������
        //public bool surveillance_supercomputer_system; //������

        public Dictionary<long, string> CountryCapitals = new Dictionary<long, string>();
        public Dictionary<long, string> FallenEmpire = new Dictionary<long, string>();
        public Dictionary<long,long> WormholeLocation = new Dictionary<long, long>();
        public Dictionary<long,long> WormholePair = new Dictionary<long, long>();

        public Dictionary<string, long> SpecialSystem = new Dictionary<string, long>()
        {
            {SpecialOjbect.�������  ,-1},
            {SpecialOjbect.��̫��    ,-1},
            {SpecialOjbect.�޾�����  ,-1},
            {SpecialOjbect.��η��    ,-1},
            {SpecialOjbect.�������,-1},
            {SpecialOjbect.С���Ƿ䳲,-1},
            {SpecialOjbect.���ǳ�    ,-1},
            {SpecialOjbect.λ�����  ,-1},
            {SpecialOjbect.ʰ����    ,-1},
            {SpecialOjbect.ˮ��֮��  ,-1},
            {SpecialOjbect.��ѡ��    ,-1},
            {SpecialOjbect.������    ,-1},
            {SpecialOjbect.������  ,-1},
            {SpecialOjbect.�����ä  ,-1},
            {SpecialOjbect.����֮��  ,-1},
            {SpecialOjbect.���ﰺ��˹  ,-1},
            {SpecialOjbect.���ֿ�˹  ,-1},
            {SpecialOjbect.������˹�ĵֿ�  ,-1},
            {SpecialOjbect.ղ����˹�����  ,-1},
            {SpecialOjbect.������  ,-1},
            {SpecialOjbect.�����֮��  ,-1},
            {SpecialOjbect.���Ѩ  ,-1},
            {SpecialOjbect.����֮��  ,-1},
            {SpecialOjbect.�¿�����  ,-1},
            {SpecialOjbect.����  ,-1},
            {SpecialOjbect.������  ,-1},
        };
        public string[] FindInCountryType = new string[]
        {
            SpecialOjbect.�������,
            SpecialOjbect.��̫��,
            SpecialOjbect.�޾�����,
            SpecialOjbect.�������,
            SpecialOjbect.��η��,
            SpecialOjbect.ʰ����
        };
        public string[] FindInStarTag = new string[]
        {
            SpecialOjbect.С���Ƿ䳲,
            SpecialOjbect.������,
            SpecialOjbect.λ�����,
            SpecialOjbect.ˮ��֮��,
            SpecialOjbect.��ѡ��
        };
        public string[] FindInStarName = new string[]
        {
            SpecialOjbect.����֮��,
            SpecialOjbect.���ﰺ��˹,
            SpecialOjbect.���ֿ�˹,
            SpecialOjbect.������˹�ĵֿ�,
            SpecialOjbect.ղ����˹�����,
            SpecialOjbect.������,
            SpecialOjbect.�����֮��,
            SpecialOjbect.���Ѩ,
            SpecialOjbect.����֮��,
            SpecialOjbect.�¿�����,
            SpecialOjbect.����,
            SpecialOjbect.������,
        };
        public void ScanBaseInfo()
        {


            foreach (var initStr in gameState.system_initializer_counter.initializer)
            {
                if (initStr == "sanctuary_system")
                {
                    SpecialSystem[SpecialOjbect.������] = 0;
                }
                else if (initStr == "surveillance_supercomputer_system")
                {
                    SpecialSystem[SpecialOjbect.������] = 0;
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
                        SpecialSystem[SpecialOjbect.�����ä] = planet.coordinate.origin;
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
