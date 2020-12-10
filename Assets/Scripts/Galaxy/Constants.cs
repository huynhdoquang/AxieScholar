namespace Net.HungryBug.Galaxy
{
    public static class RootNames
    {
        public const string Empty = "Empty";
        public const string Login = "[UIRoot] Login";
        public const string Welcome = "[UIRoot] Welcome";
        public const string Home = "[UIRoot] Home";
        public const string WorldMap = "[UIRoot] WorldMap";
        public const string BattleHud = "[UIRoot] BattleHud";
        public static string[] AllRoots = new string[] { Empty, Login, Welcome, Home, WorldMap, BattleHud };
    }

    /// <summary>
    /// Scenes.
    /// </summary>
    public static class SceneNames
    {
        public const string S_Demo = "s_demo";

        public const string S000_Boot = "s000_boot";
        public const string S001_Start = "s001_start";
        public const string S002_Login = "s002_login";
        public const string S003_Welcome = "s003_welcome";
        public const string S004_Loading = "s004_loading";
        public const string S005_Home = "s005_home";
        public const string SBattleHUD = "s_battle_hud";
        public const string SMineWarBattleHUD = "s_battle_minewar_hud";
        public const string SEnding = "s_ending";
        public const string SMineWar = "s_mine_war";

        public const string SBattleHUDReplay = "s_battle_hud_replay";
        public const string SMinewarBattleHUDReplay = "s_battle_minewar_hud_replay";

        public const string SBattleGD_GUI = "s_battle_gd_gui";
        public const string SBattleGD_Game = "s_battle_gd_game";

        public const string SWorlMap = "s_world_map_hud";

        public const string SBattlePreview = "s_battle_preview";
        public const string SWorlMapTerrain = "s_worldmap_terrain";

        public const string SGachaGrass = "gacha_grass";
        public const string SHomeGrass = "home_grass";

        //Opening Scene
        public const string SOpeningSceneIntro = "s_opening_intro";
        public const string sOpeningSceneOuttro = "s_opening_outtro";

        //Dungeon
        public const string SAbyss = "Abyss";
        public const string SAccessories = "Accessories";
        public const string SAura = "Aura";
        public const string SEssence = "Essence";
        public const string SExpertisms = "Expertisms";
        public const string SGuardian = "GuardianAngel";
        public const string SMight = "Might";
        public const string SRattleballs = "Rattleballs";
        public const string SShadow = "ShadowSlenderDemon";

        //splash scenes
        public const string SelectLoginOption = "Splash_01";
    }


    /// <summary>
    /// Dialogs.
    /// </summary>
    public static class DialogNames
    {
        public const string YesNo = "@[Dialog] YesNo";
        public const string Yes = "@[Dialog] Yes";
        public const string BattleIssue = "@[Dialog] GameIssue";
        public const string ShopActionHandler = "@[Dialog] ShopActionHandle";
        public const string RetryEndBattle = "@[Dialog] RetryEndBattle";
        public const string PermissionRequest = "@[Dialog] PermissionRequest";

        public const string MineWar_YesNo = "@[Dialog] MineWar_YesNo";
        public const string MineWar_Yes = "@[Dialog] MineWar_Yes";
    }

    public static class ButtonNames
    {
        public const string Yes = "@[Button] Yes";
        public const string No = "@[Button] No";
        public const string AppSetting = "@[Button] AppSetting";
    }
}
