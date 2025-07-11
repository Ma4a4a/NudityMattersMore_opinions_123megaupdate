using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using NudityMattersMore; // Required for InteractionType

namespace NudityMattersMore_opinions
{
    // Class for storing all mod settings
    public class NudityMattersMore_opinions_ModSettings : ModSettings
    {
        // Opinion Generator Settings
        public bool enableSituationalOpinionGenerator = true;
        public float chanceOfGeneratedOpinion = 50f;

        // Setting to enable/disable comments from our patch
        public bool enableFixationLogComments = true;

        // Setting to enable/disable debug logs
        public bool enableDebugLogging = false;

        // Other settings
        public bool enableAllOpinionsFromAllPawns = true;
        public float chanceObserverOrObservedOpinion = 50f;
        public bool displayBothObserverAndObservedOpinion = false;
        public float chancePrivatePartDescription = 20f;

        // Dictionary to store the toggle state for each interaction commentary.
        public Dictionary<string, bool> enabledInteractionToggles;

        // A list of interactions that should be disabled by default and shown separately.
        public static readonly HashSet<InteractionType> SensitiveInteractions = new HashSet<InteractionType>
        {
            InteractionType.Sex,
            InteractionType.Rape,
            InteractionType.Raped,
            InteractionType.Masturbation
        };

        // Method for saving/loading settings
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref enableSituationalOpinionGenerator, "enableSituationalOpinionGenerator", true);
            Scribe_Values.Look(ref chanceOfGeneratedOpinion, "chanceOfGeneratedOpinion", 50f);
            Scribe_Values.Look(ref enableFixationLogComments, "enableFixationLogComments", true);
            Scribe_Values.Look(ref enableDebugLogging, "enableDebugLogging", false);
            Scribe_Values.Look(ref enableAllOpinionsFromAllPawns, "enableAllOpinionsFromAllPawns", true);
            Scribe_Values.Look(ref chanceObserverOrObservedOpinion, "chanceObserverOrObservedOpinion", 50f);
            Scribe_Values.Look(ref displayBothObserverAndObservedOpinion, "displayBothObserverAndObservedOpinion", false);
            Scribe_Values.Look(ref chancePrivatePartDescription, "chancePrivatePartDescription", 20f);

            // Save and load the dictionary of toggles.
            Scribe_Collections.Look(ref enabledInteractionToggles, "enabledInteractionToggles", LookMode.Value, LookMode.Value);

            // If the dictionary is null after loading (e.g., first time running with this update), initialize it.
            if (Scribe.mode == LoadSaveMode.PostLoadInit && enabledInteractionToggles == null)
            {
                InitializeInteractionToggles();
            }
        }

        /// <summary>
        /// Initializes the dictionary with all toggleable interactions set to their default states.
        /// </summary>
        public void InitializeInteractionToggles()
        {
            enabledInteractionToggles = new Dictionary<string, bool>();
            foreach (InteractionType type in Enum.GetValues(typeof(InteractionType)))
            {
                // Set default value based on the SensitiveInteractions list.
                // If it's in the list, default to false, otherwise true.
                enabledInteractionToggles[type.ToString()] = !SensitiveInteractions.Contains(type);
            }
        }

        /// <summary>
        /// Checks if a specific interaction type is allowed to generate commentary.
        /// </summary>
        public bool IsInteractionEnabled(InteractionType type)
        {
            // Ensure the dictionary is initialized.
            if (enabledInteractionToggles == null)
            {
                InitializeInteractionToggles();
            }

            // Return the value from the dictionary, defaulting to false if not found.
            return enabledInteractionToggles.TryGetValue(type.ToString(), out bool enabled) && enabled;
        }

        // Method for resetting settings
        public void Reset()
        {
            enableSituationalOpinionGenerator = true;
            chanceOfGeneratedOpinion = 50f;
            enableFixationLogComments = true;
            enableDebugLogging = false;
            enableAllOpinionsFromAllPawns = true;
            chanceObserverOrObservedOpinion = 50f;
            displayBothObserverAndObservedOpinion = false;
            chancePrivatePartDescription = 20f;
            // Reset the toggles as well.
            InitializeInteractionToggles();
        }
    }

    // The main class of the mod, which will contain the settings and the settings window
    public class NudityMattersMore_opinions_Mod : Mod
    {
        public static NudityMattersMore_opinions_ModSettings settings;

        // Enum and variable for managing settings tabs
        private enum SettingsTab { General, Interactions }
        private SettingsTab _tab = SettingsTab.General;
        private Vector2 _scrollPosition = Vector2.zero;

        public NudityMattersMore_opinions_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<NudityMattersMore_opinions_ModSettings>();
        }

        // Method for creating the settings window
        public override void DoSettingsWindowContents(Rect inRect)
        {
            // The game draws the main window title automatically from SettingsCategory().
            // We will draw our tabs at the top right of the content area (inRect).

            // 1. Define the list of tabs.
            List<TabRecord> tabs = new List<TabRecord>
            {
                new TabRecord("General", () => _tab = SettingsTab.General, _tab == SettingsTab.General),
                new TabRecord("Interactions", () => _tab = SettingsTab.Interactions, _tab == SettingsTab.Interactions)
            };

            // 2. Define the area for the tabs at the top-right.
            float tabWidth = 120f;
            float totalTabsWidth = tabWidth * tabs.Count;
            Rect tabsRect = new Rect(inRect.x + inRect.width - totalTabsWidth, inRect.y, totalTabsWidth, 32f);

            // 3. Draw the tabs in their own rect. This will not overlap the main window title.
            TabDrawer.DrawTabs(tabsRect, tabs);

            // 4. Define the content area below the tab bar line.
            Rect contentRect = new Rect(inRect);
            contentRect.yMin = inRect.y + 32f;

            // 5. Draw the content for the selected tab.
            switch (_tab)
            {
                case SettingsTab.General:
                    DrawGeneralSettings(contentRect);
                    break;
                case SettingsTab.Interactions:
                    DrawInteractionsSettings(contentRect);
                    break;
            }
        }

        private void DrawGeneralSettings(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            // MODIFIED: Removed the redundant title from this tab.

            listing.Label("NMM Opinions Generator Settings");
            Text.Font = GameFont.Small;
            listing.Gap(8f);
            listing.CheckboxLabeled("Use situational opinion generator", ref settings.enableSituationalOpinionGenerator, "If disabled, only predefined opinions will be displayed. Default true.");
            Rect chanceGeneratedRect = listing.GetRect(30f);
            settings.chanceOfGeneratedOpinion = Widgets.HorizontalSlider(chanceGeneratedRect, settings.chanceOfGeneratedOpinion, 0f, 100f, true, $"Chance of generated opinion: {settings.chanceOfGeneratedOpinion:F0}%", "0% Generated opinions will not appear", "100% Will show only generated opinions", 1f);
            listing.Label("(If 0%, generator still will be used, if action is not predefined in xml.)");
            listing.Gap(8f);

            listing.Gap(24f);
            listing.GapLine();
            listing.Gap(12f);
            Text.Font = GameFont.Medium;
            listing.Label("Fixation Log Commentary Settings");
            Text.Font = GameFont.Small;
            listing.Gap(8f);
            bool speakUpActive = ModLister.GetActiveModWithIdentifier("JPT.speakup") != null;
            if (speakUpActive)
            {
                listing.CheckboxLabeled("Enable pawn commentary on observed actions", ref settings.enableFixationLogComments, "Enables pawns to comment on what they see (e.g., seeing someone in the shower). Requires the 'SpeakUp' mod. Default: true.");
            }
            else
            {
                GUI.contentColor = Color.gray;
                listing.Label("Enable pawn commentary on observed actions ('SpeakUp' mod not found, feature disabled)");
                GUI.contentColor = Color.white;
            }

            listing.Gap(24f);
            listing.GapLine();
            listing.Gap(12f);
            Text.Font = GameFont.Medium;
            listing.Label("Debugging");
            Text.Font = GameFont.Small;
            listing.Gap(8f);
            listing.CheckboxLabeled("Enable debug logging", ref settings.enableDebugLogging, "Shows detailed logs in the console for debugging purposes. It is recommended to keep this disabled during normal play. Default: false.");

            listing.Gap(24f);
            listing.GapLine();

            if (listing.ButtonText("Reset all settings to default"))
            {
                settings.Reset();
            }

            listing.End();
        }

        private void DrawInteractionsSettings(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            if (settings.enabledInteractionToggles == null) settings.InitializeInteractionToggles();

            // --- Sensitive Interactions Section ---
            listing.Gap(12f);
            Text.Font = GameFont.Medium;
            listing.Label("Problematic Interactions");
            Text.Font = GameFont.Small;
            listing.Gap(8f);

            Text.Font = GameFont.Tiny;
            GUI.color = Color.yellow;
            listing.Label("If these interactions enabled, descriptions from other mods may look incorrect.");
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
            listing.Gap(4f);

            foreach (InteractionType type in NudityMattersMore_opinions_ModSettings.SensitiveInteractions)
            {
                string key = type.ToString();
                bool value = settings.enabledInteractionToggles[key];
                listing.CheckboxLabeled(key, ref value, $"Enable/disable commentary for the '{key}' interaction.");
                settings.enabledInteractionToggles[key] = value;
            }

            // --- Other Commentaries Section ---
            listing.Gap(24f);
            listing.GapLine();
            listing.Gap(12f);
            Text.Font = GameFont.Medium;
            listing.Label("Other Commentaries");
            Text.Font = GameFont.Small;
            listing.Gap(8f);

            float scrollViewHeight = inRect.height - listing.CurHeight - 10f;
            Rect scrollViewOuterRect = listing.GetRect(scrollViewHeight);

            // MODIFIED: Filter out sensitive interactions for the main list
            var otherToggles = settings.enabledInteractionToggles.Keys
                .Where(k => !NudityMattersMore_opinions_ModSettings.SensitiveInteractions.Contains((InteractionType)Enum.Parse(typeof(InteractionType), k)))
                .OrderBy(k => k) // Sort alphabetically for consistency
                .ToList();

            Rect scrollViewInnerRect = new Rect(0, 0, scrollViewOuterRect.width - 16f, otherToggles.Count * 28f);

            Widgets.BeginScrollView(scrollViewOuterRect, ref _scrollPosition, scrollViewInnerRect);
            Listing_Standard scrollListing = new Listing_Standard();
            scrollListing.Begin(scrollViewInnerRect);

            foreach (string key in otherToggles)
            {
                bool value = settings.enabledInteractionToggles[key];
                scrollListing.CheckboxLabeled(key, ref value, $"Enable/disable commentary for the '{key}' interaction.");
                settings.enabledInteractionToggles[key] = value;
            }

            scrollListing.End();
            Widgets.EndScrollView();

            listing.End();
        }

        // MODIFIED: Restored the SettingsCategory override to make the mod appear in the settings list.
        public override string SettingsCategory()
        {
            return "Nudity Matters More: Opinions";
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }
    }
}
