/*
Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl/MCForge)
Dual-licensed under the Educational Community License, Version 2.0 and
the GNU General Public License, Version 3 (the "Licenses"); you may
not use this file except in compliance with the Licenses. You may
obtain a copy of the Licenses at
https://opensource.org/license/ecl-2-0/
https://www.gnu.org/licenses/gpl-3.0.html
Unless required by applicable law or agreed to in writing,
software distributed under the Licenses are distributed on an "AS IS"
BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
or implied. See the Licenses for the specific language governing
permissions and limitations under the Licenses.
 */
using System;
using System.Windows.Forms;
using Flames.Games;
using Flames.Modules.Games.Countdown;
using Flames.Modules.Games.CTF;
using Flames.Modules.Games.LS;
using Flames.Modules.Games.ZS;
using Flames.Modules.Games.TW;
namespace Flames.Gui
{
    public partial class PropertyWindow : Form
    {
        public GamesHelper LsHelper, ZsHelper, CtfHelper, TwHelper, CdHelper;
        public void LoadGameProps()
        {
            string[] allMaps = LevelInfo.AllMapNames();
            LoadZSSettings(allMaps);
            LoadCTFSettings(allMaps);
            LoadLSSettings(allMaps);
            LoadTWSettings(allMaps);
            LoadCDSettings(allMaps);
        }
        public void SaveGameProps()
        {
            SaveZSSettings();
            SaveCTFSettings();
            SaveLSSettings();
            SaveTWSettings();
            SaveCDSettings();
        }
        public GamesHelper GetGameHelper(IGame game)
        {
            // TODO: Find a better way of doing this
            if (game == ZSGame.Instance) 
            {
                return ZsHelper; 
            }
            if (game == CTFGame.Instance) 
            {
                return CtfHelper;
            }
            if (game == LSGame.Instance) 
            {
                return LsHelper;
            }
            if (game == TWGame.Instance) 
            {
                return TwHelper;
            }
            return null;
        }
        public void HandleMapsChanged(RoundsGame game)
        {
            GamesHelper helper = GetGameHelper(game);
            if (helper == null)
            {
                return;
            }
            RunOnUI_Async(() => helper.UpdateMaps());
        }
        public void HandleStateChanged(IGame game)
        {
            GamesHelper helper = GetGameHelper(game);
            if (helper == null)
            {
                return;
            }
            RunOnUI_Async(() => helper.UpdateButtons());
        }
        public void LoadZSSettings(string[] allMaps)
        {
            ZsHelper = new GamesHelper(
                ZSGame.Instance, Zs_cbStart, Zs_cbMap, Zs_cbMain,
                Zs_btnStart, Zs_btnStop, Zs_btnEnd,
                Zs_btnAdd, Zs_btnRemove, Zs_lstUsed, Zs_lstNotUsed);
            ZsHelper.Load(allMaps);
            ZSConfig cfg = ZSGame.Instance.Config;
            Zs_numInvHumanDur.Value = cfg.InvisibilityDuration;
            Zs_numInvHumanMax.Value = cfg.InvisibilityPotions;
            Zs_numInvZombieDur.Value = cfg.ZombieInvisibilityDuration;
            Zs_numInvZombieMax.Value = cfg.ZombieInvisibilityPotions;
            Zs_numReviveMax.Value = cfg.ReviveTimes;
            Zs_numReviveEff.Value = cfg.ReviveChance;
            Zs_numReviveLimit.Value = cfg.ReviveTooSlow;
            Zs_txtName.Text = cfg.ZombieName;
            Zs_txtModel.Text = cfg.ZombieModel;
        }
        public void SaveZSSettings()
        {
            try
            {
                ZSConfig cfg = ZSGame.Instance.Config;
                cfg.InvisibilityDuration = (int)Zs_numInvHumanDur.Value;
                cfg.InvisibilityPotions = (int)Zs_numInvHumanMax.Value;
                cfg.ZombieInvisibilityDuration = (int)Zs_numInvZombieDur.Value;
                cfg.ZombieInvisibilityPotions = (int)Zs_numInvZombieMax.Value;
                cfg.ReviveTimes = (int)Zs_numReviveMax.Value;
                cfg.ReviveChance = (int)Zs_numReviveEff.Value;
                cfg.ReviveTooSlow = (int)Zs_numReviveLimit.Value;
                cfg.ZombieName = Zs_txtName.Text.Trim();
                cfg.ZombieModel = Zs_txtModel.Text.Trim();
                if (cfg.ZombieModel.Length == 0)
                {
                    cfg.ZombieModel = "zombie";
                }
                ZsHelper.Save();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving ZS settings", ex);
            }
        }
        public void LoadCTFSettings(string[] allMaps)
        {
            CtfHelper = new GamesHelper(
                CTFGame.Instance, Ctf_cbStart, Ctf_cbMap, Ctf_cbMain,
                Ctf_btnStart, Ctf_btnStop, Ctf_btnEnd,
                Ctf_btnAdd, Ctf_btnRemove, Ctf_lstUsed, Ctf_lstNotUsed);
            CtfHelper.Load(allMaps);
        }
        public void SaveCTFSettings()
        {
            try
            {
                CtfHelper.Save();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving CTF settings", ex);
            }
        }
        public void LoadLSSettings(string[] allMaps)
        {
            LsHelper = new GamesHelper(
               LSGame.Instance, Ls_cbStart, Ls_cbMap, Ls_cbMain,
               Ls_btnStart, Ls_btnStop, Ls_btnEnd,
               Ls_btnAdd, Ls_btnRemove, Ls_lstUsed, Ls_lstNotUsed);
            LsHelper.Load(allMaps);

            LSConfig cfg = LSGame.Instance.Config;
            Ls_numMax.Value = cfg.MaxLives;
        }
        public void SaveLSSettings()
        {
            try
            {
                LSConfig cfg = LSGame.Instance.Config;
                cfg.MaxLives = (int)Ls_numMax.Value;
                LsHelper.Save();
                SaveLSMapSettings();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving Lava Survival settings", ex);
            }
        }
        public string LsCurMap;
        public LSMapConfig LsCurCfg;
        public void LsMapUse_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveLSMapSettings();
            if (Ls_lstUsed.SelectedIndex == -1)
            {
                Ls_grpMapSettings.Text = "Map settings";
                Ls_grpMapSettings.Enabled = false;
                LsCurCfg = null;
                return;
            }
            LsCurMap = Ls_lstUsed.SelectedItem.ToString();
            Ls_grpMapSettings.Text = "Map settings (" + LsCurMap + ")";
            Ls_grpMapSettings.Enabled = true;
            try
            {
                LsCurCfg = new LSMapConfig();
                LsCurCfg.Load(LsCurMap);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                LsCurCfg = null;
            }
            if (LsCurCfg == null)
            {
                return;
            }
            LSConfig cfg = LSGame.Instance.Config;
            Ls_numWater.Value = LsCurCfg.WaterChance;
            Ls_numFast.Value = LsCurCfg.FastChance;
            //Ls_numDestroy.Value = LsCurCfg.DestroyChance;
            Ls_numFloodUp.Value = LsCurCfg.FloodUpChance;
            Ls_numLayer.Value = LsCurCfg.LayerChance;
            Ls_numCount.Value = LsCurCfg.LayerCount;
            Ls_numHeight.Value = LsCurCfg.LayerHeight;
            Ls_numRound.Value = cfg.GetRoundTime(LsCurCfg);
            Ls_numFlood.Value = cfg.GetFloodTime(LsCurCfg);
            Ls_numLayerTime.Value = cfg.GetLayerInterval(LsCurCfg);
        }
        public void SaveLSMapSettings()
        {
            if (LsCurCfg == null)
            {
                return;
            }
            LSConfig cfg = LSGame.Instance.Config;

            LsCurCfg.WaterChance = (int)Ls_numWater.Value;
            LsCurCfg.FastChance = (int)Ls_numFast.Value;
            //LsCurCfg.DestroyChance = (int)Ls_numDestroy.Value;
            LsCurCfg.FloodUpChance = (int)Ls_numFloodUp.Value;
            LsCurCfg.LayerChance = (int)Ls_numLayer.Value;
            LsCurCfg.LayerCount = (int)Ls_numCount.Value;
            LsCurCfg.LayerHeight = (int)Ls_numHeight.Value;

            // TODO function for this
            if (Ls_numRound.Value != cfg.DefaultRoundTime)
            {
                LsCurCfg._RoundTime = Ls_numRound.Value;
            }
            if (Ls_numFlood.Value != cfg.DefaultFloodTime)
            {
                LsCurCfg._FloodTime = Ls_numFlood.Value;
            }
            if (Ls_numLayerTime.Value != cfg.DefaultLayerInterval)
            {
                LsCurCfg._LayerInterval = Ls_numLayerTime.Value;
            }
            LsCurCfg.Save(LsCurMap);
            LsHelper.UpdateMapConfig(LsCurMap);
        }
        public void LoadTWSettings(string[] allMaps)
        {
            TwHelper = new GamesHelper(
               TWGame.Instance, Tw_cbStart, Tw_cbMap, Tw_cbMain,
               Tw_btnStart, Tw_btnStop, Tw_btnEnd,
               Tw_btnAdd, Tw_btnRemove, Tw_lstUsed, Tw_lstNotUsed);
            TwHelper.Load(allMaps);
            TWConfig cfg = TWGame.Instance.Config;
            Tw_cmbDiff.SelectedIndex = (int)cfg.Difficulty;
            Tw_cmbMode.SelectedIndex = (int)cfg.Mode;
        }
        public void SaveTWSettings()
        {
            try
            {
                TWConfig cfg = TWGame.Instance.Config;
                if (Tw_cmbDiff.SelectedIndex >= 0)
                {
                    cfg.Difficulty = (TWDifficulty)Tw_cmbDiff.SelectedIndex;
                }
                if (Tw_cmbMode.SelectedIndex >= 0)
                {
                    cfg.Mode = (TWGameMode)Tw_cmbMode.SelectedIndex;
                }
                TwHelper.Save();
                SaveTWMapSettings();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving TNT wars settings", ex);
            }
        }
        public string TwCurMap;
        public TWMapConfig TwCurCfg;
        public void TwMapUse_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveTWMapSettings();
            if (Tw_lstUsed.SelectedIndex == -1)
            {
                Tw_grpMapSettings.Text = "Map settings";
                Tw_grpMapSettings.Enabled = false;
                TwCurCfg = null;
                return;
            }
            TwCurMap = Tw_lstUsed.SelectedItem.ToString();
            Tw_grpMapSettings.Text = "Map settings (" + TwCurMap + ")";
            Tw_grpMapSettings.Enabled = true;
            try
            {
                TwCurCfg = new TWMapConfig();
                TwCurCfg.Load(TwCurMap);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                TwCurCfg = null;
            }
            if (TwCurCfg == null)
            {
                return;
            }
            Tw_numScoreLimit.Value = TwCurCfg.ScoreRequired;
            Tw_numScorePerKill.Value = TwCurCfg.ScorePerKill;
            Tw_numScoreAssists.Value = TwCurCfg.AssistScore;
            Tw_numMultiKills.Value = TwCurCfg.MultiKillBonus;
            Tw_cbStreaks.Checked = TwCurCfg.Streaks;
            Tw_cbGrace.Checked = TwCurCfg.GracePeriod;
            Tw_numGrace.Value = TwCurCfg.GracePeriodTime;
            Tw_cbBalance.Checked = TwCurCfg.BalanceTeams;
            Tw_cbKills.Checked = TwCurCfg.TeamKills;
        }
        public void SaveTWMapSettings()
        {
            if (TwCurCfg == null)
            {
                return;
            }
            TwCurCfg.ScoreRequired = (int)Tw_numScoreLimit.Value;
            TwCurCfg.ScorePerKill = (int)Tw_numScorePerKill.Value;
            TwCurCfg.AssistScore = (int)Tw_numScoreAssists.Value;
            TwCurCfg.MultiKillBonus = (int)Tw_numMultiKills.Value;
            TwCurCfg.Streaks = Tw_cbStreaks.Checked;
            TwCurCfg.GracePeriod = Tw_cbGrace.Checked;
            TwCurCfg.GracePeriodTime = Tw_numGrace.Value;
            TwCurCfg.BalanceTeams = Tw_cbBalance.Checked;
            TwCurCfg.TeamKills = Tw_cbKills.Checked;
            TwCurCfg.Save(TwCurMap);
            TwHelper.UpdateMapConfig(TwCurMap);
        }
        public void LoadCDSettings(string[] allMaps)
        {
            CdHelper = new GamesHelper(
                CountdownGame.Instance, Cd_cbStart, Cd_cbMap, Cd_cbMain,
                Cd_btnStart, Cd_btnStop, Cd_btnEnd,
                Cd_btnAdd, Cd_btnRemove, Cd_lstUsed, Cd_lstNotUsed);
            CdHelper.Load(allMaps);
        }
        public void SaveCDSettings()
        {
            try
            {
                CdHelper.Save();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error saving Countdown settings", ex);
            }
        }
    }
}