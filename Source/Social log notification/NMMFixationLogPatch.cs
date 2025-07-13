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
            FillCache(DefsForPawnBeingObserved_Dynamic_Names, DefsForPawnBeingObserved_Dynamic);
            FillCache(DefsForObservingPawn_Dynamic_Names, DefsForObservingPawn_Dynamic);
            FillCache(DefsForPawnBeingObserved_Fallback_Names, DefsForPawnBeingObserved_Fallback);
            FillCache(DefsForObservingPawn_Fallback_Names, DefsForObservingPawn_Fallback);
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
            }
        }
    }

    [StaticConstructorOnStartup]
    public static class NMMFixationLogPatches
    {
        private static readonly bool IsEnabled;

        // Кулдаун для конкретного взаимодействия (инициатор, цель, тип)
        private static Dictionary<Tuple<Pawn, Pawn, InteractionType>, int> interactionCooldownExpiry = new Dictionary<Tuple<Pawn, Pawn, InteractionType>, int>();

        // Счетчик мнений, высказанных пешкой за один "проход" для ограничения спама
        private static Dictionary<Pawn, int> opinionsThisTick = new Dictionary<Pawn, int>();
        private static int lastTickProcessed = -1;

        private const int QueueProcessInterval = 60;
        private static Queue<Action> commentaryQueue = new Queue<Action>();

        public static DynamicLogTextInfo LastDynamicTextInfo = null;

        static NMMFixationLogPatches()
        {
            if (ModLister.GetActiveModWithIdentifier("JPT.speakup") == null)
            {
                IsEnabled = false;
                return;
            }

            IsEnabled = true;
            var harmony = new Harmony("shark510.nuditymattersmoreopinions.nmmo.fixationlog");
            harmony.Patch(AccessTools.Method(typeof(PawnInteractionManager), "ProcessInteraction"), postfix: new HarmonyMethod(typeof(NMMFixationLogPatches), nameof(ProcessInteraction_Postfix)));
            harmony.Patch(AccessTools.Method(typeof(TickManager), "DoSingleTick"), postfix: new HarmonyMethod(typeof(NMMFixationLogPatches), nameof(ProcessQueue_Postfix)));
        }

        public static void ProcessQueue_Postfix()
        {
            if (!IsEnabled) return;

            int currentTick = Find.TickManager.TicksGame;

            // Очищаем счетчик одновременных мнений в начале каждого нового цикла обработки
            if (currentTick > lastTickProcessed)
            {
                opinionsThisTick.Clear();
                lastTickProcessed = currentTick;
            }

            if (currentTick % QueueProcessInterval == 0 && commentaryQueue.Any())
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
            bool observerInSameNudeState = IsPawnInSameNudeState(observer, interactionType);

            // Новая проверка: если настройка выключена и оба в одинаковом состоянии, не комментируем
            if (!NudityMattersMore_opinions_Mod.settings.allowCommentOnSameState && observerInSameNudeState && IsPawnInSameNudeState(observed, interactionType))
            {
                // Пропускаем, если оба в одинаковом состоянии и это запрещено настройками
            }
            else if (!observerInSameNudeState)
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
                    TryQueueCommentary(observer, observed, interactionType, defObserver, rawTextObserver, observer, observed);
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
                    TryQueueCommentary(observed, observer, interactionType, defObserved, rawTextObserved, observer, observed);
                }
            }
        }

        private static void TryQueueCommentary(Pawn initiator, Pawn recipient, InteractionType interactionType, InteractionDef interactionDef, string rawOpinionText, Pawn originalObserver, Pawn originalObserved)
        {
            int currentTick = Find.TickManager.TicksGame;

            // Проверка на лимит одновременных мнений
            if (opinionsThisTick.TryGetValue(initiator, out int count) && count >= NudityMattersMore_opinions_Mod.settings.maxSimultaneousOpinions)
            {
                return;
            }

            // Проверка кулдауна для конкретного взаимодействия
            var key = Tuple.Create(initiator, recipient, interactionType);
            if (interactionCooldownExpiry.TryGetValue(key, out int expiryTick) && currentTick < expiryTick)
            {
                return;
            }

            if (initiator == null || recipient == null)
            {
                return;
            }
            // ИСПРАВЛЕНО: Передаем interactionType в очередь
            commentaryQueue.Enqueue(() => FireSingleCommentary(initiator, recipient, interactionType, interactionDef, rawOpinionText, originalObserver, originalObserved));
        }
        // ИСПРАВЛЕНО: Добавлен параметр interactionType
        private static void FireSingleCommentary(Pawn initiator, Pawn recipient, InteractionType interactionType, InteractionDef interactionDef, string rawOpinionText, Pawn originalObserver, Pawn originalObserved)
        {
            if (initiator == null || recipient == null || interactionDef == null)
            {
                return;
            }

            // Повторная проверка кулдаунов
            int currentTick = Find.TickManager.TicksGame;
            // ИСПРАВЛЕНО: Используем переданный interactionType для создания ключа
            var key = Tuple.Create(initiator, recipient, interactionType);
            if (interactionCooldownExpiry.TryGetValue(key, out int expiryTick) && currentTick < expiryTick)
            {
                return;
            }

            // Повторная проверка лимита одновременных мнений
            if (opinionsThisTick.TryGetValue(initiator, out int count) && count >= NudityMattersMore_opinions_Mod.settings.maxSimultaneousOpinions)
            {
                return;
            }

            DynamicLogTextInfo dynamicTextInfo = null;
            if (!string.IsNullOrEmpty(rawOpinionText))
            {
                dynamicTextInfo = new DynamicLogTextInfo
                {
                    RawText = rawOpinionText,
                    OriginalObserver = originalObserver,
                    OriginalObserved = originalObserved,
                    BodyPart = SituationalOpinionHelper.LastObservedBodyPartDef
                };
            }

            LastDynamicTextInfo = dynamicTextInfo;

            try
            {
                if (initiator.interactions.TryInteractWith(recipient, interactionDef))
                {
                    currentTick = Find.TickManager.TicksGame;

                    // Обновляем кулдаун для этого конкретного взаимодействия
                    int cooldownTicks = (int)(NudityMattersMore_opinions_Mod.settings.commentaryCooldownSeconds * 60);
                    interactionCooldownExpiry[key] = currentTick + cooldownTicks;

                    // Увеличиваем счетчик мнений за этот тик
                    if (opinionsThisTick.ContainsKey(initiator))
                    {
                        opinionsThisTick[initiator]++;
                    }
                    else
                    {
                        opinionsThisTick[initiator] = 1;
                    }
                }
            }
            finally
            {
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
