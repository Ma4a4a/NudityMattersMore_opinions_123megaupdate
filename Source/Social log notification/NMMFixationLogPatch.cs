using HarmonyLib;
using RimWorld;
using Verse;
using NudityMattersMore;
using System.Collections.Generic;
using System;
using System.Linq;

namespace NudityMattersMore_opinions
{
    // --- КЛАСС КЭША ВЫНЕСЕН НАВЕРХ ДЛЯ ЛУЧШЕЙ ЧИТАЕМОСТИ ---
    [StaticConstructorOnStartup]
    public static class InteractionDefCache
    {
        // Финальные словари с готовыми InteractionDef
        public static readonly Dictionary<InteractionType, InteractionDef> DefsForPawnBeingObserved_Dynamic = new Dictionary<InteractionType, InteractionDef>();
        public static readonly Dictionary<InteractionType, InteractionDef> DefsForObservingPawn_Dynamic = new Dictionary<InteractionType, InteractionDef>();
        public static readonly Dictionary<InteractionType, InteractionDef> DefsForPawnBeingObserved_Fallback = new Dictionary<InteractionType, InteractionDef>();
        public static readonly Dictionary<InteractionType, InteractionDef> DefsForObservingPawn_Fallback = new Dictionary<InteractionType, InteractionDef>();

        // Временные словари с названиями. Имена исправлены, чтобы избежать дублирования.
        private static readonly Dictionary<InteractionType, string> DefsForPawnBeingObserved_Dynamic_Names = new Dictionary<InteractionType, string> { { InteractionType.Covering, "NMM_DynamicBubble_Covering_Observed" }, { InteractionType.Naked, "NMM_DynamicBubble_Naked_Observed" }, { InteractionType.Topless, "NMM_DynamicBubble_Topless_Observed" }, { InteractionType.Bottomless, "NMM_DynamicBubble_Bottomless_Observed" }, { InteractionType.Masturbation, "NMM_DynamicBubble_Masturbation_Observed" }, { InteractionType.Sex, "NMM_DynamicBubble_Sex_Observed" }, { InteractionType.Rape, "NMM_DynamicBubble_Rape_Observed" }, { InteractionType.Raped, "NMM_DynamicBubble_Raped_Observed" }, { InteractionType.Shower, "NMM_DynamicBubble_Shower_Observed" }, { InteractionType.Bath, "NMM_DynamicBubble_Bath_Observed" }, { InteractionType.Sauna, "NMM_DynamicBubble_Sauna_Observed" }, { InteractionType.Swimming, "NMM_DynamicBubble_Swimming_Observed" }, { InteractionType.HotTub, "NMM_DynamicBubble_HotTub_Observed" }, { InteractionType.NipSlip, "NMM_DynamicBubble_NipSlip_Observed" }, { InteractionType.BreastSlip, "NMM_DynamicBubble_BreastSlip_Observed" }, { InteractionType.AreolaSlip, "NMM_DynamicBubble_AreolaSlip_Observed" }, { InteractionType.Biopod, "NMM_DynamicBubble_Biopod_Observed" }, { InteractionType.Changing, "NMM_DynamicBubble_Changing_Observed" }, { InteractionType.WetShirt, "NMM_DynamicBubble_WetShirt_Observed" }, { InteractionType.Breastfeed, "NMM_DynamicBubble_Breastfeed_Observed" }, { InteractionType.SelfMilk, "NMM_DynamicBubble_SelfMilk_Observed" }, { InteractionType.Milk, "NMM_DynamicBubble_Milk_Observed" }, { InteractionType.MedicalFull, "NMM_DynamicBubble_MedicalFull_Observed" }, { InteractionType.MedicalTop, "NMM_DynamicBubble_MedicalTop_Observed" }, { InteractionType.MedicalBottom, "NMM_DynamicBubble_MedicalBottom_Observed" }, { InteractionType.MedicalFullSelf, "NMM_DynamicBubble_MedicalFullSelf_Observed" }, { InteractionType.MedicalTopSelf, "NMM_DynamicBubble_MedicalTopSelf_Observed" }, { InteractionType.MedicalBottomSelf, "NMM_DynamicBubble_MedicalBottomSelf_Observed" }, { InteractionType.Surgery, "NMM_DynamicBubble_Surgery_Observed" }, { InteractionType.HumanArtTop, "NMM_DynamicBubble_HumanArtTop_Observed" }, { InteractionType.HumanArtBottom, "NMM_DynamicBubble_HumanArtBottom_Observed" }, { InteractionType.HumanArtFull, "NMM_DynamicBubble_HumanArtFull_Observed" }, };
        private static readonly Dictionary<InteractionType, string> DefsForObservingPawn_Dynamic_Names = new Dictionary<InteractionType, string> { { InteractionType.Covering, "NMM_DynamicBubble_Covering_Observer" }, { InteractionType.Naked, "NMM_DynamicBubble_Naked_Observer" }, { InteractionType.Topless, "NMM_DynamicBubble_Topless_Observer" }, { InteractionType.Bottomless, "NMM_DynamicBubble_Bottomless_Observer" }, { InteractionType.Masturbation, "NMM_DynamicBubble_Masturbation_Observer" }, { InteractionType.Sex, "NMM_DynamicBubble_Sex_Observer" }, { InteractionType.Rape, "NMM_DynamicBubble_Rape_Observer" }, { InteractionType.Raped, "NMM_DynamicBubble_Raped_Observer" }, { InteractionType.Shower, "NMM_DynamicBubble_Shower_Observer" }, { InteractionType.Bath, "NMM_DynamicBubble_Bath_Observer" }, { InteractionType.Sauna, "NMM_DynamicBubble_Sauna_Observer" }, { InteractionType.Swimming, "NMM_DynamicBubble_Swimming_Observer" }, { InteractionType.HotTub, "NMM_DynamicBubble_HotTub_Observer" }, { InteractionType.NipSlip, "NMM_DynamicBubble_NipSlip_Observer" }, { InteractionType.BreastSlip, "NMM_DynamicBubble_BreastSlip_Observer" }, { InteractionType.AreolaSlip, "NMM_DynamicBubble_AreolaSlip_Observer" }, { InteractionType.Biopod, "NMM_DynamicBubble_Biopod_Observer" }, { InteractionType.Changing, "NMM_DynamicBubble_Changing_Observer" }, { InteractionType.WetShirt, "NMM_DynamicBubble_WetShirt_Observer" }, { InteractionType.Breastfeed, "NMM_DynamicBubble_Breastfeed_Observer" }, { InteractionType.SelfMilk, "NMM_DynamicBubble_SelfMilk_Observer" }, { InteractionType.Milk, "NMM_DynamicBubble_Milk_Observer" }, { InteractionType.MedicalFull, "NMM_DynamicBubble_MedicalFull_Observer" }, { InteractionType.MedicalTop, "NMM_DynamicBubble_MedicalTop_Observer" }, { InteractionType.MedicalBottom, "NMM_DynamicBubble_MedicalBottom_Observer" }, { InteractionType.MedicalFullSelf, "NMM_DynamicBubble_MedicalFullSelf_Observer" }, { InteractionType.MedicalTopSelf, "NMM_DynamicBubble_MedicalTopSelf_Observer" }, { InteractionType.MedicalBottomSelf, "NMM_DynamicBubble_MedicalBottomSelf_Observer" }, { InteractionType.Surgery, "NMM_DynamicBubble_Surgery_Observer" }, { InteractionType.HumanArtTop, "NMM_DynamicBubble_HumanArtTop_Observer" }, { InteractionType.HumanArtBottom, "NMM_DynamicBubble_HumanArtBottom_Observer" }, { InteractionType.HumanArtFull, "NMM_DynamicBubble_HumanArtFull_Observer" }, };
        private static readonly Dictionary<InteractionType, string> DefsForPawnBeingObserved_Fallback_Names = new Dictionary<InteractionType, string> { { InteractionType.Covering, "Covering_Observed_Interaction" }, { InteractionType.Naked, "Naked_Observed_Interaction" }, { InteractionType.Topless, "Topless_Observed_Interaction" }, { InteractionType.Bottomless, "Bottomless_Observed_Interaction" }, { InteractionType.Masturbation, "Masturbation_Observed_Interaction" }, { InteractionType.Sex, "Sex_Observed_Interaction" }, { InteractionType.Rape, "Rape_Observed_Interaction" }, { InteractionType.Raped, "Raped_Observed_Interaction" }, { InteractionType.Shower, "Shower_Observed_Interaction" }, { InteractionType.Bath, "Bath_Observed_Interaction" }, { InteractionType.Sauna, "Sauna_Observed_Interaction" }, { InteractionType.Swimming, "Swimming_Observed_Interaction" }, { InteractionType.HotTub, "HotTub_Observed_Interaction" }, { InteractionType.NipSlip, "NipSlip_Observed_Interaction" }, { InteractionType.BreastSlip, "BreastSlip_Observed_Interaction" }, { InteractionType.AreolaSlip, "AreolaSlip_Observed_Interaction" }, { InteractionType.Biopod, "Biopod_Observed_Interaction" }, { InteractionType.Changing, "Changing_Observed_Interaction" }, { InteractionType.WetShirt, "WetShirt_Observed_Interaction" }, { InteractionType.Breastfeed, "Breastfeed_Observed_Interaction" }, { InteractionType.SelfMilk, "SelfMilk_Observed_Interaction" }, { InteractionType.Milk, "Milk_Observed_Interaction" }, { InteractionType.MedicalFull, "MedicalFull_Observed_Interaction" }, { InteractionType.MedicalTop, "MedicalTop_Observed_Interaction" }, { InteractionType.MedicalBottom, "MedicalBottom_Observed_Interaction" }, { InteractionType.MedicalFullSelf, "MedicalFullSelf_Observed_Interaction" }, { InteractionType.MedicalTopSelf, "MedicalTopSelf_Observed_Interaction" }, { InteractionType.MedicalBottomSelf, "MedicalBottomSelf_Observed_Interaction" }, { InteractionType.Surgery, "Surgery_Observed_Interaction" }, { InteractionType.HumanArtTop, "HumanArtTop_Observed_Interaction" }, { InteractionType.HumanArtBottom, "HumanArtBottom_Observed_Interaction" }, { InteractionType.HumanArtFull, "HumanArtFull_Observed_Interaction" }, };
        private static readonly Dictionary<InteractionType, string> DefsForObservingPawn_Fallback_Names = new Dictionary<InteractionType, string> { { InteractionType.Covering, "Covering_wasObserved_Interaction" }, { InteractionType.Naked, "Naked_wasObserved_Interaction" }, { InteractionType.Topless, "Topless_wasObserved_Interaction" }, { InteractionType.Bottomless, "Bottomless_wasObserved_Interaction" }, { InteractionType.Masturbation, "Masturbation_wasObserved_Interaction" }, { InteractionType.Sex, "Sex_wasObserved_Interaction" }, { InteractionType.Rape, "Rape_wasObserved_Interaction" }, { InteractionType.Raped, "Raped_wasObserved_Interaction" }, { InteractionType.Shower, "Shower_wasObserved_Interaction" }, { InteractionType.Bath, "Bath_wasObserved_Interaction" }, { InteractionType.Sauna, "Sauna_wasObserved_Interaction" }, { InteractionType.Swimming, "Swimming_wasObserved_Interaction" }, { InteractionType.HotTub, "HotTub_wasObserved_Interaction" }, { InteractionType.NipSlip, "NipSlip_wasObserved_Interaction" }, { InteractionType.BreastSlip, "BreastSlip_wasObserved_Interaction" }, { InteractionType.AreolaSlip, "AreolaSlip_wasObserved_Interaction" }, { InteractionType.Biopod, "Biopod_wasObserved_Interaction" }, { InteractionType.Changing, "Changing_wasObserved_Interaction" }, { InteractionType.WetShirt, "WetShirt_wasObserved_Interaction" }, { InteractionType.Breastfeed, "Breastfeed_wasObserved_Interaction" }, { InteractionType.SelfMilk, "SelfMilk_wasObserved_Interaction" }, { InteractionType.Milk, "Milk_wasObserved_Interaction" }, { InteractionType.MedicalFull, "MedicalFull_wasObserved_Interaction" }, { InteractionType.MedicalTop, "MedicalTop_wasObserved_Interaction" }, { InteractionType.MedicalBottom, "MedicalBottom_wasObserved_Interaction" }, { InteractionType.MedicalFullSelf, "MedicalFullSelf_wasObserved_Interaction" }, { InteractionType.MedicalTopSelf, "MedicalTopSelf_wasObserved_Interaction" }, { InteractionType.MedicalBottomSelf, "MedicalBottomSelf_wasObserved_Interaction" }, { InteractionType.Surgery, "Surgery_wasObserved_Interaction" }, { InteractionType.HumanArtTop, "HumanArtTop_wasObserved_Interaction" }, { InteractionType.HumanArtBottom, "HumanArtBottom_wasObserved_Interaction" }, { InteractionType.HumanArtFull, "HumanArtFull_wasObserved_Interaction" }, };

        static InteractionDefCache()
        {
            // Заполняем наши кэшированные словари настоящими Def-ами
            FillCache(DefsForPawnBeingObserved_Dynamic_Names, DefsForPawnBeingObserved_Dynamic);
            FillCache(DefsForObservingPawn_Dynamic_Names, DefsForObservingPawn_Dynamic);
            FillCache(DefsForPawnBeingObserved_Fallback_Names, DefsForPawnBeingObserved_Fallback);
            FillCache(DefsForObservingPawn_Fallback_Names, DefsForObservingPawn_Fallback);

            ModLog.Message("[NMM Opinions] InteractionDefCache: Caching complete.");
        }

        private static void FillCache(Dictionary<InteractionType, string> source, Dictionary<InteractionType, InteractionDef> destination)
        {
            foreach (var kvp in source)
            {
                InteractionDef def = DefDatabase<InteractionDef>.GetNamed(kvp.Value, false);
                if (def != null)
                {
                    destination[kvp.Key] = def;
                }
                else
                {
                    Log.Warning($"[NMM Opinions] InteractionDefCache: Could not find InteractionDef named '{kvp.Value}' for InteractionType '{kvp.Key}'.");
                }
            }
        }
    }

    [StaticConstructorOnStartup]
    public static class NMMFixationLogPatches
    {
        private static readonly bool IsEnabled;

        // --- Cooldowns ---
        private const int GlobalCommentCooldownTicks = 300;
        private const int RecipientReactionCooldownTicks = 450;
        private const int InteractionCooldownTicks = 15000;

        private static Dictionary<Pawn, int> pawnGlobalCooldownExpiry = new Dictionary<Pawn, int>();
        private static Dictionary<Tuple<Pawn, Pawn>, int> lastObserverCommentTick = new Dictionary<Tuple<Pawn, Pawn>, int>();
        private static Dictionary<Tuple<Pawn, Pawn>, int> lastObservedCommentTick = new Dictionary<Tuple<Pawn, Pawn>, int>();

        private const int QueueProcessInterval = 60;
        private static Queue<Action> commentaryQueue = new Queue<Action>();

        public static DynamicLogTextInfo LastDynamicTextInfo = null;

        static NMMFixationLogPatches()
        {
            if (ModLister.GetActiveModWithIdentifier("JPT.speakup") == null)
            {
                ModLog.Message("[NMM Opinions - Fixation Log] 'SpeakUp' mod not found. Commentary feature will be disabled.");
                IsEnabled = false;
                return;
            }

            IsEnabled = true;
            var harmony = new Harmony("shark510.nuditymattersmoreopinions.nmmo.fixationlog");
            harmony.Patch(AccessTools.Method(typeof(PawnInteractionManager), "ProcessInteraction"), postfix: new HarmonyMethod(typeof(NMMFixationLogPatches), nameof(ProcessInteraction_Postfix)));
            harmony.Patch(AccessTools.Method(typeof(TickManager), "DoSingleTick"), postfix: new HarmonyMethod(typeof(NMMFixationLogPatches), nameof(ProcessQueue_Postfix)));
            ModLog.Message("[NMM Opinions - Fixation Log] Patches successfully applied. 'SpeakUp' mod found.");
        }

        public static void ProcessQueue_Postfix()
        {
            if (!IsEnabled) return;
            if (Find.TickManager.TicksGame % QueueProcessInterval == 0 && commentaryQueue.Any())
            {
                commentaryQueue.Dequeue().Invoke();
            }
        }

        public static void ProcessInteraction_Postfix(Pawn observer, Pawn observed, InteractionType interactionType, bool aware)
        {
            if (!IsEnabled || !NudityMattersMore_opinions_Mod.settings.enableFixationLogComments || observer == null || observed == null || observer == observed)
            {
                return;
            }

            if (!NudityMattersMore_opinions_Mod.settings.IsInteractionEnabled(interactionType))
            {
                return;
            }

            // --- ОБРАБОТКА ДЛЯ НАБЛЮДАТЕЛЯ (Observer) ---
            if (!IsPawnInSameNudeState(observer, interactionType))
            {
                string rawTextObserver = SituationalOpinionHelper.SelectOpinionTextForBubble(observer, observed, interactionType, PawnState.None, aware, false, OpinionPerspective.UsedForObserver);

                InteractionDef defObserver = null;
                if (!string.IsNullOrEmpty(rawTextObserver))
                {
                    InteractionDefCache.DefsForObservingPawn_Dynamic.TryGetValue(interactionType, out defObserver);
                }
                else
                {
                    InteractionDefCache.DefsForObservingPawn_Fallback.TryGetValue(interactionType, out defObserver);
                }

                if (defObserver != null)
                {
                    TryQueueCommentary(observer, observed, defObserver, rawTextObserver, lastObserverCommentTick, observer, observed);
                }
            }

            // --- ОБРАБОТКА ДЛЯ НАБЛЮДАЕМОГО (Observed) ---
            if (aware)
            {
                string rawTextObserved = SituationalOpinionHelper.SelectOpinionTextForBubble(observed, observer, interactionType, PawnState.None, aware, false, OpinionPerspective.UsedForObserved);

                InteractionDef defObserved = null;
                if (!string.IsNullOrEmpty(rawTextObserved))
                {
                    InteractionDefCache.DefsForPawnBeingObserved_Dynamic.TryGetValue(interactionType, out defObserved);
                }
                else
                {
                    InteractionDefCache.DefsForPawnBeingObserved_Fallback.TryGetValue(interactionType, out defObserved);
                }

                if (defObserved != null)
                {
                    TryQueueCommentary(observed, observer, defObserved, rawTextObserved, lastObservedCommentTick, observer, observed);
                }
            }
        }

        private static void TryQueueCommentary(Pawn initiator, Pawn recipient, InteractionDef interactionDef, string rawOpinionText, Dictionary<Tuple<Pawn, Pawn>, int> specificCooldownDict, Pawn originalObserver, Pawn originalObserved)
        {
            int currentTick = Find.TickManager.TicksGame;

            if (pawnGlobalCooldownExpiry.TryGetValue(initiator, out int expiryTick) && currentTick < expiryTick) return;

            var pair = Tuple.Create(initiator, recipient);
            if (specificCooldownDict.TryGetValue(pair, out int lastTick) && currentTick - lastTick < InteractionCooldownTicks) return;

            if (initiator == null || recipient == null)
            {
                Log.Warning($"[NMM Opinions] Skipping commentary for null pawn. Initiator: {initiator?.LabelCap ?? "null"}, Recipient: {recipient?.LabelCap ?? "null"}");
                return;
            }

            commentaryQueue.Enqueue(() => FireSingleCommentary(initiator, recipient, interactionDef, rawOpinionText, pair, specificCooldownDict, originalObserver, originalObserved));
        }

        private static void FireSingleCommentary(Pawn initiator, Pawn recipient, InteractionDef interactionDef, string rawOpinionText, Tuple<Pawn, Pawn> pair, Dictionary<Tuple<Pawn, Pawn>, int> cooldownDict, Pawn originalObserver, Pawn originalObserved)
        {

            // Добавляем проверку на null для initiator и recipient в самом начале метода
            if (initiator == null || recipient == null)
            {
                Log.Warning($"[NMM Opinions] FireSingleCommentary skipped due to null pawn. Initiator: {initiator?.LabelCap ?? "null"}, Recipient: {recipient?.LabelCap ?? "null"}");
                return;
            }


            int currentTick = Find.TickManager.TicksGame;
            if (pawnGlobalCooldownExpiry.TryGetValue(initiator, out int expiryTick) && currentTick < expiryTick)
            {
                return;
            }

            if (interactionDef == null)
            {
                return;
            }

            // --- ИЗМЕНЕНИЕ ЛОГИКИ ---
            // Мы создаем информацию для динамической замены текста ТОЛЬКО если этот текст существует.
            // Если rawOpinionText пустой, значит мы используем стандартный (fallback) InteractionDef,
            // и наша система динамической замены не должна вмешиваться.
            if (!string.IsNullOrEmpty(rawOpinionText))
            {
                LastDynamicTextInfo = new DynamicLogTextInfo
                {
                    RawText = rawOpinionText,
                    OriginalObserver = originalObserver,
                    OriginalObserved = originalObserved,
                    BodyPart = SituationalOpinionHelper.LastObservedBodyPartDef
                };
            }
            else
            {
                // Для стандартных InteractionDef убеждаемся, что информация для замены пуста.
                LastDynamicTextInfo = null;
            }


            if (initiator.interactions.TryInteractWith(recipient, interactionDef))
            {
                currentTick = Find.TickManager.TicksGame;
                cooldownDict[pair] = currentTick;
                pawnGlobalCooldownExpiry[initiator] = currentTick + GlobalCommentCooldownTicks;
                pawnGlobalCooldownExpiry[recipient] = currentTick + RecipientReactionCooldownTicks;
            }
            else
            {
                // Если по какой-то причине взаимодействие не удалось, всегда очищаем информацию.
                LastDynamicTextInfo = null;
            }
        }

        private static bool IsPawnInSameNudeState(Pawn pawn, InteractionType interactionType)
        {
            if (pawn == null) return false;
            switch (interactionType)
            {
                case InteractionType.Covering:
                    return InfoHelper.IsCovering(pawn, out _);
                case InteractionType.Naked:
                    return InfoHelper.IsNaked(pawn);
                case InteractionType.Topless:
                    return InfoHelper.IsTopless(pawn) && InfoHelper.HasBreasts(pawn);
                case InteractionType.Bottomless:
                    return InfoHelper.IsBottomless(pawn);
                default:
                    return false;
            }
        }
    }
}
