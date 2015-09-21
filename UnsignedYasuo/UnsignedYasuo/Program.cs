using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace DarkRyze
{
    internal class Program
    {
        public static Menu ComboMenu, DrawingsMenu, KSMenu, LaneClear, LastHit, Harass, menu;
        public static Spell.Skillshot Q { get { return YasuoFunctions.GetQType(); } }
        public static Spell.SpellBase W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static AIHeroClient _Player { get { return ObjectManager.Player; } }
        public static int Mana { get { return (int) _Player.Mana; } }
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }
        
        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.ChampionName != "Yasuo")
                return;

            //Hacks.AntiAFK = true;
            Bootstrap.Init(null);
            
            //W = new Spell.Skillshot(SpellSlot.W, 400, SkillShotType.Linear);
            E = new Spell.Targeted(SpellSlot.E, 475);
            R = new Spell.Active(SpellSlot.R);// range 1200
            
            menu = MainMenu.AddMenu("Unsigned Yasuo", "UnsignedYasuo");

            ComboMenu = menu.AddSubMenu("Combo", "combomenu");
            
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("QU", new CheckBox("Use Q"));
            ComboMenu.Add("EU", new CheckBox("Use E"));
            ComboMenu.Add("RU", new CheckBox("Use R"));
            ComboMenu.Add("IU", new CheckBox("Use Items"));

            LaneClear = menu.AddSubMenu("Lane Clear", "laneclear");
            LaneClear.AddGroupLabel("Lane Clear Settings");
            LaneClear.Add("LCQ", new CheckBox("Use Q"));
            LaneClear.Add("LCE", new CheckBox("Use E"));
            LaneClear.Add("LCEQ", new CheckBox("Use EQ"));
            LaneClear.Add("LCEUT", new CheckBox("E Under Turret"));
            LaneClear.Add("LCI", new CheckBox("Use Items (Hydra/Timat)"));

            Harass = menu.AddSubMenu("Harass", "harass");
            Harass.AddGroupLabel("Harass Settings");
            Harass.Add("HQ", new CheckBox("Use Q"));
            Harass.Add("HE", new CheckBox("Use E"));
            Harass.Add("HEQ", new CheckBox("Use EQ"));

            LastHit = menu.AddSubMenu("Last Hit", "lasthitmenu");
            LastHit.AddGroupLabel("Last Hit Settings");
            LastHit.Add("LHQ", new CheckBox("Use Q"));
            LastHit.Add("LHE", new CheckBox("Use E"));
            LastHit.Add("LHEQ", new CheckBox("Use EQ"));
            LastHit.Add("LHEUT", new CheckBox("E Under Turret"));

            /*KSMenu = menu.AddSubMenu("Kill Steal", "ksmenu");

            KSMenu.AddGroupLabel("Kill Steal Settings");
            KSMenu.Add("EnableKS", new CheckBox("KS"));
            KSMenu.Add("KSQ", new CheckBox("KS with Q"));
            KSMenu.Add("KS3Q", new CheckBox("KS with 3rd Q"));
            KSMenu.Add("KSE", new CheckBox("KS with E"));
            KSMenu.Add("KSEQ", new CheckBox("KS with EQ"));*/

            DrawingsMenu = menu.AddSubMenu("Drawings", "drawingsmenu");

            DrawingsMenu.AddGroupLabel("Drawings Settings");
            DrawingsMenu.Add("DQ", new CheckBox("Draw Q"));
            DrawingsMenu.Add("DE", new CheckBox("Draw E"));

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Program.DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, Q.Range, System.Drawing.Color.BlueViolet);
            }

            if (Program.DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue)
            {
                Drawing.DrawCircle(_Player.Position, E.Range, System.Drawing.Color.BlueViolet);
            }

        }

        private static void Game_OnTick(EventArgs args)
        {
            YasuoFunctions.GetQType();
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                YasuoFunctions.Combo();
            }
            /*if (Program.KSMenu["EnableKS"].Cast<CheckBox>().CurrentValue)
            {
                YasuoFunctions.KS();
            }*/
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                YasuoFunctions.LastHit();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                YasuoFunctions.Harrass();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                YasuoFunctions.LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                YasuoFunctions.Flee();
            }
        }
    }
}
