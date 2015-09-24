using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;

namespace UnsignedAnnie
{
    class AnnieFunctions
    {
        public enum AttackSpell
        {
            Q,
            W,
            R,
            Ignite
        };

        //get enemy non last hit)
        public static Obj_AI_Base GetEnemy(float range, GameObjectType type)
        {
            return ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health).Where(a => a.IsEnemy
            && a.Type == type
            && a.Distance(Annie) <= range
            && !a.IsDead
            && !a.IsInvulnerable
            && a.IsValidTarget(range)).FirstOrDefault();
        }

        public static Obj_AI_Base GetEnemy(GameObjectType type, AttackSpell spell)
        {
            //ksing
            if (spell == AttackSpell.W)//w
            {
                return ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health).Where(a => a.IsEnemy
                && a.Type == type
                && a.Distance(Annie) <= Program.W.Range
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(Program.W.Range)
                && a.Health <= (AnnieCalcs.W(a) - 25)).FirstOrDefault();
            }
            else if (spell == AttackSpell.Q)//q
            {
                return ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health).Where(a => a.IsEnemy
                && a.Type == type
                && a.Distance(Annie) <= Program.Q.Range
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(Program.Q.Range)
                && a.Health <= (AnnieCalcs.Q(a) - 25)).FirstOrDefault();
            }
            else if (spell == AttackSpell.R)//r
            {
                return ObjectManager.Get<Obj_AI_Base>().Where(a => a.IsEnemy
                && a.Type == type
                && a.Distance(Annie.Pet) <= 200
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(200)).FirstOrDefault();
            }
            else//ignite
            {
                return ObjectManager.Get<Obj_AI_Base>().OrderBy(a => a.Health).Where(a => a.IsEnemy
                && a.Type == type
                && a.Distance(Annie) <= Program.Ignite.Range
                && !a.IsDead
                && !a.IsInvulnerable
                && a.IsValidTarget(Program.Ignite.Range)
                && a.Health <= (AnnieCalcs.Ignite(a) - 25)).FirstOrDefault();
            }
        }

        public static AIHeroClient Annie { get { return ObjectManager.Player; } }

        public static void LastHit()
        {
            bool QCHECK = Program.LastHit["LHQ"].Cast<CheckBox>().CurrentValue;
            bool QREADY = Program.Q.IsReady();
          
            if (QCHECK && QREADY)
            {
                Obj_AI_Minion enemy = (Obj_AI_Minion)GetEnemy(GameObjectType.obj_AI_Minion, AttackSpell.Q);

                if (enemy != null)
                    Program.Q.Cast(enemy);
            }
        }

        public static void LaneClear()
        {
            bool QCHECK = Program.LaneClear["LCQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = Program.LaneClear["LCW"].Cast<CheckBox>().CurrentValue;
            bool QREADY = Program.Q.IsReady();
            bool WREADY = Program.W.IsReady();

            if (QCHECK && QREADY)
            {
                Obj_AI_Minion enemy = (Obj_AI_Minion)GetEnemy(Program.Q.Range, GameObjectType.obj_AI_Minion);

                if (enemy != null)
                    Program.Q.Cast(enemy);
            }

            if (WCHECK && WREADY)
            {
                Obj_AI_Base enemy = GetBestWLocation(GameObjectType.obj_AI_Minion);

                if (enemy != null)
                    Program.W.Cast(enemy.Position);
            }

            if (Orbwalker.CanAutoAttack)
            {
                Obj_AI_Base enemy = (Obj_AI_Minion)GetEnemy(Annie.GetAutoAttackRange(), GameObjectType.obj_AI_Base);

                if (enemy != null)
                    Orbwalker.ForcedTarget = enemy;
            }
        }
       
        public static void Harrass()
        {
            bool QCHECK = Program.LastHit["HQ"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = Program.LastHit["HW"].Cast<CheckBox>().CurrentValue;
            bool QREADY = Program.Q.IsReady();
            bool WREADY = Program.W.IsReady();

            if (QCHECK && QREADY)
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(Program.Q.Range, GameObjectType.AIHeroClient);

                if (enemy != null)
                    Program.Q.Cast(enemy);
            }

            if (WCHECK && WREADY)
            {
                AIHeroClient enemy = (AIHeroClient)GetBestWLocation(GameObjectType.AIHeroClient);

                if (enemy != null)
                    Program.W.Cast(enemy.Position);
            }

            if (Orbwalker.CanAutoAttack)
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(Annie.GetAutoAttackRange(), GameObjectType.AIHeroClient);

                if (enemy != null)
                    Orbwalker.ForcedTarget = enemy;
            }
        }

        public static void Combo()
        {
            bool QCHECK = Program.ComboMenu["QU"].Cast<CheckBox>().CurrentValue;
            bool WCHECK = Program.ComboMenu["WU"].Cast<CheckBox>().CurrentValue;
            bool ECHECK = Program.ComboMenu["EU"].Cast<CheckBox>().CurrentValue;
            bool RCHECK = Program.ComboMenu["RU"].Cast<CheckBox>().CurrentValue;
            bool ItemsCHECK = Program.ComboMenu["IU"].Cast<CheckBox>().CurrentValue;
            bool IgniteCHECK = Program.ComboMenu["IgU"].Cast<CheckBox>().CurrentValue;
            bool QREADY = Program.Q.IsReady();
            bool WREADY = Program.W.IsReady();
            bool EREADY = Program.E.IsReady();
            bool RREADY = Program.R.IsReady();

            if (RCHECK && RREADY)
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(Program.R.Range, GameObjectType.AIHeroClient);

                if (enemy != null)
                    Program.R.Cast(enemy);
            }

            if (QCHECK && QREADY)
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(Program.W.Range, GameObjectType.AIHeroClient);

                if (enemy != null)
                    Program.Q.Cast(enemy);
            }

            if (WCHECK && WREADY)
            {
                AIHeroClient enemy = (AIHeroClient)GetBestWLocation(GameObjectType.AIHeroClient);

                if (enemy != null)
                    Program.W.Cast(enemy.Position);
            }

            if (ECHECK && EREADY)
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(Program.Q.Range, GameObjectType.AIHeroClient);

                if (enemy != null)
                    Program.E.Cast();
            }

            if (ItemsCHECK)
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(2500, GameObjectType.AIHeroClient);

                if (enemy != null)
                    UseItems();
            }

            if(GetEnemy(GameObjectType.AIHeroClient, AttackSpell.R) != null
                && Program.E.IsReady())
            {
                Program.E.Cast();
            }

            if (IgniteCHECK && Program.Ignite != null && Program.Ignite.IsReady())
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(Program.Ignite.Range, GameObjectType.AIHeroClient);

                if (enemy != null)
                    Program.Ignite.Cast(enemy);
            }

            if (Orbwalker.CanAutoAttack)
            {
                AIHeroClient enemy = (AIHeroClient)GetEnemy(Annie.GetAutoAttackRange(), GameObjectType.AIHeroClient);

                if (enemy != null)
                    Orbwalker.ForcedTarget = enemy;
            }
        }

        public static void StackMode()
        {
            if(Annie.IsInShopRange()
                && !Annie.HasBuff("pyromania_particle"))
            {
                if (Program.W.IsReady())
                    Program.W.Cast(Annie.Position);
                if (Program.E.IsReady())
                    Program.E.Cast();
            }
        }

        public static void Flee()
        {
            Orbwalker.MoveTo(Game.CursorPos);
        }

        public static void UseItems()
        {
            InventorySlot[] items = Annie.InventoryItems;

            foreach (InventorySlot item in items)
            {
                if (item.CanUseItem())
                {
                    if (item.Id == ItemId.Health_Potion
                        && Annie.Health <= (Annie.MaxHealth * 0.45)
                        && !Annie.IsRecalling()
                        && Annie.CountEnemiesInRange(2000) <= 1
                        && !Annie.IsInShopRange()
                        && !Annie.HasBuff("RegenerationPotion"))
                    {
                        item.Cast();
                    }
                    if (item.Id == ItemId.Mana_Potion
                        && Annie.Mana <= (Annie.MaxMana * 0.45)
                        && !Annie.IsRecalling()
                        && Annie.CountEnemiesInRange(2000) <= 1
                        && !Annie.IsInShopRange()
                        && !Annie.HasBuff("FlaskOfCrystalWater"))
                    {
                        item.Cast();
                    }
                    if (item.Id == ItemId.Crystalline_Flask
                        && (Annie.Health <= (Annie.MaxHealth * 0.45) || Annie.Mana <= (Annie.MaxMana * 0.45))
                        && !Annie.IsRecalling()
                        && Annie.CountEnemiesInRange(2000) <= 1
                        && !Annie.IsInShopRange()
                        && !Annie.HasBuff("ItemCrystalFlask"))
                    {
                        item.Cast();
                    }
                }
            }
        }
        
        public static Obj_AI_Base GetBestWLocation(GameObjectType type)
        {
            int numEnemiesInRange = 0;
            Obj_AI_Base enem = null;

            foreach (Obj_AI_Base enemy in ObjectManager.Get<Obj_AI_Base>()
                .OrderBy(a => a.Health)
                .Where(a => a.Distance(Annie) <= Program.W.Range
                && a.IsEnemy
                && a.Type == type
                && !a.IsDead
                && !a.IsInvulnerable))
            {
                int tempNumEnemies = 0;
                foreach (Obj_AI_Base enemy2 in ObjectManager.Get<Obj_AI_Base>()
                    .OrderBy(a => a.Health)
                    .Where(a => a.Distance(Annie) <= Program.W.Range
                    && a.IsEnemy
                    && !a.IsDead
                    && a.Type == type
                    && !a.IsInvulnerable))
                {
                    if (enemy != enemy2
                        && enemy2.Distance(enemy) <= 75)
                        tempNumEnemies++;
                }

                if (tempNumEnemies > numEnemiesInRange)
                {
                    enem = enemy;
                    numEnemiesInRange = tempNumEnemies;
                }
            }

            return enem;
        }
    }
}
