using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

namespace StellarisTool
{

    public class EventFocusSystem
    {
        public long id;
        public int type;
    }
    public class GlobalDefine
    {

        public static Dictionary<string, string> ColonizablePlanetClass = new Dictionary<string, string>
        {
            {"pc_ai","pc_ai" },
            {"pc_alpine","pc_alpine" },
            {"pc_arctic","pc_arctic" },
            {"pc_arid","pc_arid" },
            {"pc_continental","pc_continental" },
            {"pc_desert","pc_desert" },
            {"pc_gaia","pc_gaia" },
            {"pc_hive","pc_hive" },
            {"pc_machine","pc_machine" },
            {"pc_nuked","pc_nuked" },
            {"pc_ocean","pc_ocean" },
            {"pc_savannah","pc_savannah" },
            {"pc_tropical","pc_tropical" },
            {"pc_tundra","pc_tundra" },
            {"pc_city","pc_machine" },
            {"pc_relic","pc_machine" },
        };

        public static Dictionary<string, string> Megastructure = new Dictionary<string, string>
        {
            {"ring_world_1","ring_world" },
            {"ring_world_2_intermediate","ring_world" },
            {"ring_world_2","ring_world" },
            {"ring_world_3_intermediate","ring_world" },
            {"ring_world_ruined","ring_world_ruined" },
            {"ring_world_restored","ring_world" },

            {"dyson_sphere_0","dyson_sphere" },
            {"dyson_sphere_1","dyson_sphere" },
            {"dyson_sphere_2","dyson_sphere" },
            {"dyson_sphere_3","dyson_sphere" },
            {"dyson_sphere_4","dyson_sphere" },
            {"dyson_sphere_5","dyson_sphere" },
            {"dyson_sphere_permanently_ruined","dyson_sphere_ruined" },
            {"dyson_sphere_ruined","dyson_sphere_ruined" },
            {"dyson_sphere_restored","dyson_sphere" },
            {"dyson_sphere_disco","dyson_sphere" },
            {"dyson_sphere_disco_restored","dyson_sphere" },

            {"spy_orb_0","spy_orb" },
            {"spy_orb_1","spy_orb" },
            {"spy_orb_2","spy_orb" },
            {"spy_orb_3","spy_orb" },
            {"spy_orb_4","spy_orb" },
            {"spy_orb_permanently_ruined","spy_orb_ruined" },
            {"spy_orb_ruined","spy_orb_ruined" },
            {"spy_orb_restored","spy_orb" },

            {"think_tank_0","think_tank" },
            {"think_tank_1","think_tank" },
            {"think_tank_2","think_tank" },
            {"think_tank_3","think_tank" },
            {"think_tank_4","think_tank" },
            {"think_tank_permanently_ruined","think_tank_ruined" },
            {"think_tank_ruined","think_tank_ruined" },
            {"think_tank_restored","think_tank" },

            {"matter_decompressor_0","matter_decompressor" },
            {"matter_decompressor_1","matter_decompressor" },
            {"matter_decompressor_2","matter_decompressor" },
            {"matter_decompressor_3","matter_decompressor" },
            {"matter_decompressor_4","matter_decompressor" },
            {"matter_decompressor_permanently_ruined","matter_decompressor_ruined" },
            {"matter_decompressor_ruined","matter_decompressor_ruined" },
            {"matter_decompressor_restored","matter_decompressor" },

            {"strategic_coordination_center_0","strategic_coordination_center" },
            {"strategic_coordination_center_1","strategic_coordination_center" },
            {"strategic_coordination_center_2","strategic_coordination_center" },
            {"strategic_coordination_center_3","strategic_coordination_center" },
            {"strategic_coordination_center_4","strategic_coordination_center" },
            {"strategic_coordination_center_permanently_ruined","strategic_coordination_center_ruined" },
            {"strategic_coordination_center_ruined","strategic_coordination_center_ruined" },
            {"strategic_coordination_center_restored","strategic_coordination_center" },


            {"mega_art_installation_0","mega_art_installation" },
            {"mega_art_installation_1","mega_art_installation" },
            {"mega_art_installation_2","mega_art_installation" },
            {"mega_art_installation_3","mega_art_installation" },
            {"mega_art_installation_4","mega_art_installation" },
            {"mega_art_installation_permanently_ruined","mega_art_installation_ruined" },
            {"mega_art_installation_ruined","mega_art_installation_ruined" },
            {"mega_art_installation_restored","mega_art_installation" },
            {"mega_art_installation_restored_2","mega_art_installation" },

            {"interstellar_assembly_0","interstellar_assembly" },
            {"interstellar_assembly_1","interstellar_assembly" },
            {"interstellar_assembly_2","interstellar_assembly" },
            {"interstellar_assembly_3","interstellar_assembly" },
            {"interstellar_assembly_4","interstellar_assembly" },
            {"interstellar_assembly_permanently_ruined","interstellar_assembly_ruined" },
            {"interstellar_assembly_ruined","interstellar_assembly_ruined" },
            {"interstellar_assembly_restored","interstellar_assembly" },

            {"mega_shipyard_0","mega_shipyard" },
            {"mega_shipyard_1","mega_shipyard" },
            {"mega_shipyard_2","mega_shipyard" },
            {"mega_shipyard_3","mega_shipyard" },
            {"mega_shipyard_4","mega_shipyard" },
            {"mega_shipyard_permanently_ruined","mega_shipyard_ruined" },
            {"mega_shipyard_ruined","mega_shipyard_ruined" },
            {"mega_shipyard_restored","mega_shipyard" },

            {"crisis_sphere_0","crisis_sphere" },
            {"crisis_sphere_1","crisis_sphere" },
            {"crisis_sphere_2","crisis_sphere" },
            {"crisis_sphere_3","crisis_sphere" },
            {"crisis_sphere_4","crisis_sphere" },
            {"crisis_sphere_ruined","crisis_sphere_ruined" },

            {"quantum_catapult_0","quantum_catapult" },
            {"quantum_catapult_1","quantum_catapult" },
            {"quantum_catapult_2","quantum_catapult" },
            {"quantum_catapult_3","quantum_catapult" },
            {"quantum_catapult_4","quantum_catapult" },
            {"quantum_catapult_permanently_ruined","quantum_catapult_ruined" },
            {"quantum_catapult_ruined","quantum_catapult_ruined" },
            {"quantum_catapult_restored","quantum_catapult" },
            {"quantum_catapult_ruined_slingshot","quantum_catapult" },
            {"quantum_catapult_restored_slingshot","quantum_catapult" },
            {"quantum_catapult_improved_slingshot","quantum_catapult" },

            //{"gateway_0","gateway_0" },
            //{"gateway_ruined","gateway_ruined" },
            //{"gateway_restored","gateway_ruined" },
            //{"gateway_final","gateway_ruined" },
            //{"gateway_derelict","gateway_ruined" },

        };

        public static Dictionary<string, string> Hostile = new Dictionary<string, string>
        {
            {"marauder_system","劫掠" },
            {"hostile_system","危险" }
        };

    }
    public class StellarisGameMeta
    {
        public string version;
        public string version_control_revision;
        public string name;
        public string date;
        public string[] required_dlcs;
        public string player_portrait;
        public Flag flag;
        public int meta_fleets;
        public int meta_planets;
        public bool ironman;

    }
    public class StellarisGameState
    {
        public string version;
        public string version_control_revision;
        public string name;
        public string date;
        public string[] required_dlcs;
        //TODO non key dict to List
        public Player[] player;
        //TODO number dict
        //public Dictionary<int, SpyNetWork> spy_networks;
        //TODO
        public Nebula[] nebula;
        //TODO number dict
        public Species species_db;
        public Dictionary<long, Pop> pop;
        public Dictionary<long, GalacticObject> galactic_object;
        //public object starbase_mgr;
        public PlanetRoot planets;
        //public object astral_rifts;
        public Dictionary<long, Country> country;
        //public Federation[] federation;
        //public object truce;
        //public object trade_deal;
        //public object espionage_assets;
        //public object construction;
        //public Leader[] leaders;
        public Initializer system_initializer_counter;
        //public Ship[] ships;
        public Dictionary<long, Fleet> fleet;
        //public object fleet_template;
        public Army army;
        //public object deposit;
        //public object ground_combat;
        //public object war;
        public Debris debris;
        //public object missile;
        //public object strike_craft;
        //public object ambient_object;
        //public object orbital_line;
        //public object message;
        //public object player_event;
        //public object open_player_event_selection_history;
        //public object random_name_database;
        //public object name_list;
        public Galaxy galaxy;
        public float galaxy_radius;
        public Dictionary<string, object> flags;
        //public object saved_event_target;
        //public object ship_design;
        public Dictionary<long, Megastructure> megastructures;
        public Dictionary<long, Bypass> bypasses;
        //public object natural_wormholes;
        //public object sectors;
        //public object buildings;
        //public object archaeological_sites;
        //public object espionage_operations;
        //public object agreements;
        //public object global_ship_design;
        //public object clusters;
        //public object rim_galactic_objects;
        //public string used_color;
        //public object used_symbols;
        //public object used_species_names;
        //public object used_species_portrait;
        //public object market;
        //public object trade_routes_manager;
        //public object slave_market_manager;
        //public object galactic_community;
        //public object first_contacts;
        //public object situations;
        //public object council_positions;
        //public object automation_resources;
        public int tick;
        public int random_log_day;
        //TODOinner key
        public long last_created_species_ref;
        public long last_created_pop;
        public long last_created_country;
        public long last_refugee_country;
        public long last_created_system;
        public long last_created_fleet;
        public long last_created_ship;
        public long last_created_leader;
        public long last_created_army;
        public long last_created_design;
        public long last_created_ambient_object;
        public long last_event_id;
        public int camera_focus;
        public int additional_crisis_strength;
        public int random_seed;
        public int random_count;
        public bool show_structures_tab;
        public bool show_politics_tab;
    }

    public class Megastructure
    {
        public string type;
        public Coordinate coordinate;
        public long owner;
        public long planet;
        //public Dictionary<int,long> orbitals;
        public long build_queue;
    }

    public class Bypass
    {
        public string type;
        public bool active;
        public Owner owner;
        public long lock_country;
        public long lock_remain_day;
        public bool is_manual_lock;
        public long linked_to;
        public long[] connections;
        public long[] active_connections;

    }

    public class Owner
    {
        public int type;
        public long id;
    }
    public class Initializer
    {
        public int[] count;
        public string[] initializer;
    }
    public class Galaxy{
        public string template;
        public string shape;
        public int num_empires;
        public int num_advanced_empires;
        public int num_fallen_empires;
        public int num_marauder_empires;
        public float habitability;
        public float primitive;
        public bool advanced_starts_near_player;
        public bool caravaneers_enabled;
        public bool xeno_compatibility_enabled;
        public float crises;
        public float technology;
        public float traditions;
        public float logistic_ceiling;
        public float growth_scale;
        public bool clustered;
        public bool random_empires;
        public bool random_fallen_empires;
        public bool random_marauder_empires;
        public bool random_advanced_empires;
        public float core_radius;
        // public int player_locations;
        // public int difficulty;
        // public int aggressiveness;
        // public int crisis_type;
        // public int scaling;
        // public int technology_difficulty_scale;
        // public bool lgate_enabled;
        public string name;
        public bool ironman;
        public float num_gateways;
        public float num_wormhole_pairs;
        public float num_hyperlanes;
        public float mid_game_start;
        public float end_game_start;
        public float victory_year;
        public float num_guaranteed_colonies;
        public bool difficulty_adjusted_ai_modifiers;
    }
    public class Debris {
    }
    public class Army{

    }

    public class Fleet{
        public Variable name;
        public long[] ships;
        public object combat;
        public bool station;
        public MovementManager movement_manager;
    }
    public class MovementManager
    {
        public Coordinate coordinate;
    }
    public class Ship{

    }
    public class Leader{

    }
    public class Federation{

    }
    public class Country{
        public Flag flag;
        public int color_index;
        public Variable name;
        public Variable adjective;
        //public object tech_status;
        //public string last_date_was_human;
        //public string last_date_war_lost;
        //public string last_date_at_war;
        //public object budget;
        //public object events;
        public bool track_all_situations;
        //public object terra_incognita;
        //public float military_power;
        //public float economy_power;
        //public int victory_rank;
        //public float victory_score;
        //public float tech_power;
        //public float immigration;
        //public float emigration;
        //public int fleet_size;
        //public int used_naval_capacity;
        //public int empire_size;
        //public float new_colonies;
        //public int sapient;
        public string graphical_culture;
        public string city_graphical_culture;
        //public int capital;
        //public int founder_species_ref;
        //public object ethos;
        //public object fleet_template_manager;
        //public object government;
        public string personality;
        public string next_election;
        public string government_date;
        //public object surveyed_deposit_holders;
        public Variable homeworld_name;
        //public int[] visited_objects;
        //public int[] intel_level;
        //public int[] highest_intel_level;
        public string[] default_planet_automation_settings;
        public Dictionary<string,object> flags;
        public Dictionary<string,object> variables;
        //public int[] sensor_range_fleets;
        public Dictionary<string,object> faction;
        public string name_list;
        //public string[] ship_names;
        //public int ruler;
        //public object control_groups;
        public Variable ship_prefix;
        //public object active_policies;
        //public string[] policy_flags;
        public long starting_system;
        public bool has_advisor;
        public int[] owned_leaders;
        public int[] owned_armies;
        public int[] owned_planets;
        public int[] controlled_planets;
        //public int[] ship_design_collection;
        public string type;
        //public object modules;
        public bool initialized;
        //public object espionage_manager;
        //public object intel_manager;
        public FleetsManager fleets_manager;
        //public string customization;
        public bool is_in_breach_of_any;
        //public int awareness;
        //public int old_awareness;
        //public string last_changed_country_type;
        public int[] hyperlane_systems;
        //public object sectors;
        //public float given_value;
        //public int num_upgraded_starbase;
        //public int starbase_capacity;
        //public int employable_pops;
        //public int[] owned_species_refs;
        //public object first_contact;
        //public object astral_actions_usage_states_array;

    }

    public class FleetsManager
    {
        public OwnedFleet[] owned_fleets;
    }

    public class OwnedFleet
    {
        public long fleet;
        public string ownership_status;
        public int lease_period;
        public long debtor;
        public long previous_owner;
    }
    public class PlanetRoot
    {
        public Dictionary<long, Planet> planet;
    }

    public class Variable
    {
        public string key;
        public Variable value;
        public Variable[] variables;


        static Dictionary<string, string> KeyMap = new Dictionary<string, string>()
        {
            {"shipclass_mining_station_name","%PLANET%" },
            {"PLANET_NAME_FORMAT","%PARENT% %NUMERAL%" },
            {"SUBPLANET_NAME_FORMAT","%PARENT%%NUMERAL%" },
            {"%ADJECTIVE%","%ADJECTIVE%" },
            {"STAR_NAME_1_OF_1","%NAME%" },
            {"STAR_NAME_1_OF_2","%NAME%" },
            {"STAR_NAME_2_OF_2","%NAME%" },
            {"STAR_NAME_1_OF_3","%NAME%" },
            {"STAR_NAME_2_OF_3","%NAME%" },
            {"STAR_NAME_3_OF_3","%NAME%" },
            {"STAR_NAME_1_OF_4","%NAME%" },
            {"STAR_NAME_2_OF_4","%NAME%" },
            {"STAR_NAME_3_OF_4","%NAME%" },
            {"STAR_NAME_4_OF_4","%NAME%" }
        };

        public string GetKey()
        {
            KeyMap.TryGetValue(key, out var result);
            if (result == null)
            {
                //Debug.Log($"No Key Map:{key}");
                return key;    
            } else
            {
                return result;
            }
        }
        public override string ToString()
        {
            if (value != null)
            {
                return value.ToString();
            }
            var result = GetKey();
            if (variables != null)
            {
                for (int i = 0; i < variables.Length; i++)
                {
                    if (int.TryParse(variables[i].GetKey(), out var id))
                    {
                        result = $"{result} {variables[i]}";
                    }
                }
                var groups = Regex.Matches(result, @"%(?<var>[^%]+)%");
                if (groups.Count == 0)
                {
                    return result;
                }
                for (int i = 0; i < groups.Count; i++)
                {
                    var g = groups[i];
                    var varkey = g.Result("${var}");
                    if (value != null)
                    {
                        if (value.key.ToLower() == varkey.ToLower())
                        {
                            result = result.Replace($"%{varkey}%", value.ToString());
                            break;
                        }
                    }
                    if (variables != null)
                    {

                        for (int j = 0; j < variables.Length; j++)
                        {
                            if (variables[j].key.ToLower() == varkey.ToLower())
                            {
                                result = result.Replace($"%{varkey}%", variables[j].ToString());
                                break;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }

    public class Planet{
        public Variable name;
        public string planet_class;
        public Coordinate coordinate;
        public float orbit;
        public int planet_size;
        //public int bombardment_damage;
        //public string last_bombardment;
        //public bool automated_development;
        public string kill_pop;
        //public int build_queue;
        //public int army_build_queue;
        public Dictionary<int,long> planet_orbitals;
        public int shipclass_orbital_station;
        //public int orbital_defence;
        public Dictionary<string,object> flags;
        public int entity;
        //public int surveyed_by;
        public bool prevent_anomaly;
        public int[] deposits;
        public long owner = -1;
        public long original_owner = -1;
        public long controller = -1;
        public long[] pop;
        public long[] buildings;
        //public object favorite_jobs;
        //public int stability;
        //public int migration;
        //public int crime;
        //public int amenities;
        //public int amenities_usage;
        //public int free_amenities;
        //public int free_housing;
        //public int total_housing;
        //public int housing_usage;
        //public int employable_pops;
        //public int num_sapient_pops;
        public bool recalc_pops;
        public string manual_designation_changed_date;
        public int ascension_tire;
        public bool[] auto_slots_taken;        
    }

    public enum GalacticObjectType{
        Star
    }
    public enum StarClass{
        sc_b,
        sc_a,
        sc_f,
        sc_g,
        sc_k,
        sc_m,
        sc_m_giant,
        sc_t,
        sc_black_hole,
        sc_neutron_star,
        sc_pulsar,
        sc_binary_1,
        sc_binary_2,
        sc_binary_3,
        sc_binary_4,
        sc_binary_5,
        sc_binary_6,
        sc_binary_7,
        sc_binary_8,
        sc_binary_9,
        sc_binary_10,
        sc_trinary_1,
        sc_trinary_2,
        sc_trinary_3,
        sc_trinary_4,
        sc_toxoid_star,
        sc_rift_star,
        //game config dosn't exist
        sc_s_giant,
        sc_g_giant,
        sc_g_super,
        sc_k_giant,
        sc_c_giant,
        sc_b_giant,
        sc_a_giant,
        sc_f_giant,
        sc_t_giant,
        sc_trinary_f_k_m,
        sc_binary_o_t,
        sc_binary_f_f,
        sc_binary_gg_k,
        sc_binary_g_g,
        sc_binary_a_a,
        sc_binary_f_y,
        sc_binary_g_k,
        sc_binary_mg_m,
        sc_binary_m_m,
        sc_trinary_g_g_g,
        sc_d,
        sc_o_super,
        sc_trinary_g_m_m,
        sc_o_hyper,
        sc_binary_a_k,
        sc_fu,
        sc_binary_f_d,
        sc_trinary_a_f_d,
        sc_trinary_g_k_k,
        sc_trinary_k_k_k,
        sc_binary_mg_mg,
        sc_trinary_b_m_m,
        sc_o,
        sc_collapsar,
        sc_binary_k_k,
        sc_trinary_a_t_y,
        sc_protostar,
        sc_binary_fs_a,
        sc_p_green,
        sc_trinary_a_a_a,
        sc_binary_kg_a,
        sc_binary_k_d,
        sc_trinary_m_m_m,
        sc_l,
        sc_paired_kg_gg_m_m,
        sc_m_hyper,
        sc_a_super,
        sc_trinary_b_f_g,
        sc_tt_orange,
        sc_w_red,
        sc_tt_red,
        sc_trinary_gg_mg_k,
        sc_nova_1,
        sc_y,
        sc_trinary_gg_f_m,
        sc_binary_pl_pl,
        sc_trinary_f_f_f,
        sc_binary_m_l,
        sc_ae,
        sc_p_purple,
        sc_trinary_gs_gg_m,
        sc_trinary_b_a_a,
        sc_microquasar_1,
        sc_binary_kg_gg,
        sc_trinary_m_t_y,
        sc_paired_a_a_f_f,
        sc_magnetar,
        sc_lbv_blue,
        sc_binary_k_y,
        sc_w_green,
        sc_binary_kg_g,
        sc_binary_gg_f,
        sc_lbv_green,
        sc_trinary_g_k_d,
        sc_binary_g_y,
        sc_binary_mg_g,
        sc_binary_b_y,
        sc_trinary_k_m_d,
        sc_binary_k_l,
        sc_binary_g_l,
        sc_trinary_mg_a_d,
        sc_trinary_b_k_m,
        sc_trinary_b_b_b,
        sc_nova_2,
        sc_trinary_b_t_y,
        sc_binary_b_f,
        sc_binary_f_l,
        sc_trinary_mg_g_k,
        sc_binary_g_t,
        sc_paired_g_g_k_k,
        sc_w_purple,
        sc_binary_kg_m,
        sc_m_super,
        sc_trinary_a_l_t,
        sc_tt_white,
        sc_binary_gg_g,
        sc_trinary_ks_kg_d,
        sc_microquasar_2,
        sc_k_super,
        sc_binary_k_m,
        sc_paired_g_k_m_m,
        sc_binary_a_g,
        sc_lbv_red,
        sc_p_red,
        sc_paired_g_k_k_t,
        sc_binary_b_a,
    }




    public enum PlanetClass{
        pc_desert,
        pc_arid,
        pc_savannah,
        pc_tropical,
        pc_continental,
        pc_ocean,
        pc_tundra,
        pc_arctic,
        pc_alpine,
        pc_gas_giant,
        pc_asteroid,
        pc_ice_asteroid,
        pc_rare_crystal_asteroid,
        pc_molten,
        pc_barren,
        pc_barren_cold,
        pc_toxic,
        pc_frozen,
        pc_nuked,
        pc_hive,
        pc_machine,
        pc_machine_broken,
        pc_shielded,
        pc_ai,
        pc_infested,
        pc_gaia,
        pc_cybrex,
        pc_b_star,
        pc_a_star,
        pc_f_star,
        pc_g_star,
        pc_k_star,
        pc_m_star,
        pc_m_giant_star,
        pc_t_star,
        pc_black_hole,
        pc_neutron_star,
        pc_pulsar,
        pc_ringworld_habitable,
        pc_ringworld_habitable_damaged,
        pc_ringworld_tech,
        pc_ringworld_tech_damaged,
        pc_ringworld_seam,
        pc_ringworld_seam_damaged,
        pc_shattered_ring_habitable,
        pc_habitat,
        pc_shrouded,
        pc_broken,
        pc_shattered,
        pc_toxoid_star,

        pc_rift_star,
        pc_astral_scar,
        pc_gray_goo,
        pc_egg_cracked,
        pc_crystal_habitat,
        pc_warden_guardian,
        pc_crystal_asteroid,
        pc_ringworld_shielded,
        pc_habitat_shielded,
        pc_city,
        pc_relic,
        pc_shattered_2,

        pc_c_giant_star,
        pc_b_giant_star,
        pc_a_giant_star,
        pc_g_giant_star,
        pc_f_giant_star,
        pc_k_giant_star,
        pc_t_giant_star,
        pc_s_giant_star,
        pc_c_super_star,
        pc_b_super_star,
        pc_a_super_star,
        pc_g_super_star,
        pc_f_super_star,
        pc_k_super_star,
        pc_t_super_star,
        pc_s_super_star,
        pc_o_star,
        pc_o_super_star,
        pc_tt_orange_star,
        pc_nova_2,
        pc_collapsar,
        pc_d_star,
        pc_magnetar,
        pc_l_star,
        pc_lbv_green_star,
        pc_microquasar_1,
        pc_protostar,
        pc_m_hyper_star,
        pc_w_green_star,
        pc_w_purple_star,
        pc_lbv_red_star,
        pc_y_star,
        pc_m_super_star,
        pc_lbv_blue_star,
        pc_tt_red_star,
        pc_o_hyper_star,
        pc_tt_white_star,
        pc_fu_star,
        pc_microquasar_2,
        pc_ae_star,
        pc_nova_1,
        pc_w_red_star,
    }

    public class Hyperlane{
        public int to;
        public float length;
    }
    public class AsteroidBelt{
        public AsteroidBeltType type;
        public float inner_radius;
    }
    public enum AsteroidBeltType{
        rocky_asteroid_belt,
        icy_asteroid_belt,
        crystal_asteroid_belt,
        debris_asteroid_belt,
        empty_asteroid_belt

    }
    public class TradeHub{
        public float collected;
    }
    public class TradePiracy{
        public int throughput;
        public int total;
        public int max;
        public int active;
        public int used;
    }

    public class GalacticObject{
        public Coordinate coordinate;
        public GalacticObjectType type;
        //TODO inner key
        public Variable name;
        //TODO to list
        public JToken planet;
        public long[] ambient_object;
        public string star_class;
        public Hyperlane[] hyperlane;
        //TODO non key dict to List
        public AsteroidBelt[] asteroid_belts;
        public int arm;
        public Dictionary<string,object> flags;
        public string initializer;
        public float inner_radius;
        public float outer_radius;
        public long[] starbases;
        public long[] bypasses;
        public long[] megastructures;
        //public TradeHub trade_hub;

        //public object trade_collection;
        //public TradePiracy trade_piracy;
        public long sector;
        public long[] colonies;
    }

    public class Species
    {
        public Traits traits;
        public int extra_trait_points;
        public string gender;
        public string name_list;
        public Variable name;
        public Variable plural;
        public Variable adjective;
        public string @class;
        public string portrait;
        public long home_planet;
        public string name_data;
    }

    public class Traits
    {
        public string[] trait;
    }

    public class Pop{
        public int species;
        //TODO 转成list inner key
        public Dictionary<string,string> ethos;
        public bool force_faction_evaluation;
        public bool enslaved;
        public string job;
        public string category;
        public int planet;
        public float crime;
        public float power;
        public float diplomatic_weight;
        public float happiness;
        public bool can_migrate;
        public bool can_vote;
        public bool has_random_ethics;
        public bool can_fill_drone_job;
        public bool can_fill_worker_job;
        public bool can_fill_specialist_job;
        public bool can_fill_ruler_job;
        public bool can_fill_precursor_job;
        public float amenities_usage;
        public float housing_usage;
        public float[] job_weights_cache;
    }

    public class Coordinate {
        public float x;
        public float y;
        public long origin;
        public bool randomized;
        public float visual_height;
    }

    public class Nebula{
        public Coordinate coordinate;
        
        //TODO
        public Variable name;
        public float radius;
        //TODO 转成List
        public int[] galactic_objects; 
    }
    public class SpyNetWork{
        public int owner;
        public int target;
        public long leader;
        public float power;
        public string formed;
    }

    public class Player{
        public string name;
        public int country;
    }


    public class Flag{
        public FlagRes icon;
        public FlagRes background;
        public string[] colors;
    }

    public class FlagRes{
        public string category;
        public string file;
    }
}
