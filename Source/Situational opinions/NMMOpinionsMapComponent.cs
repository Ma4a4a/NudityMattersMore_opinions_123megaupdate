using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using NudityMattersMore; // For NMM.PawnInteractionManager, NMM.PawnInteraction, NMM.PawnState, NMM.InteractionType
using System; 

namespace NudityMattersMore_opinions
{

    /// <summary>
    /// MapComponent responsible for tracking new interactions in NMM
    /// and generating corresponding situational opinions.
    /// This component will be loaded for each map.
    /// </summary>
    public class NMMOpinionsMapComponent : MapComponent
    {
        // Dictionary to keep track of the last processed InteractionLogs for each pawn.
        // This dictionary will not persist across game exits.
        // External dictionary key: thingIDNumber of the pawn we are tracking the InteractionLog for.

        // Value: ID of the last processed record (GameTick or something unique).
        // Use a nested dictionary to keep track of processed records for each pair of pawns,
        // to avoid processing the same interactions twice.
        // Key of outer dictionary: thingIDNumber of the log owner pawn
        // Internal dictionary key: Unique interaction key (e.g. "ObservedPawnID-InteractionType-PawnState")
        // Internal dictionary value: GameTick, to ensure we only process new entries.
        private Dictionary<int, Dictionary<string, int>> lastProcessedInteractionTicks = new Dictionary<int, Dictionary<string, int>>();


        // Check frequency (in ticks). 60 ticks = 1 second of real time.
        // Checking too often can cause load, checking too rarely can cause delays.
        private const int CheckFrequencyTicks = 120; // Every 2 seconds

        /// <summary>
        /// Map component constructor.
        /// </summary>
        public NMMOpinionsMapComponent(Map map) : base(map) { }

        /// <summary>
        /// Called every tick on the map.
        /// </summary>
        public override void MapComponentTick()
        {
            base.MapComponentTick();

            // Check with the given frequency
            if (Find.TickManager.TicksGame % CheckFrequencyTicks != 0)
            {
                return;
            }

            // Get a copy of all pawns on the map.
            // Use TryGetValue to safely access NMM static dictionaries,
            // since they may be uninitialized or changed.
            if (NudityMattersMore.PawnInteractionManager.InteractionLogs == null)
            {
                // ModLog.Warning("[NMM Opinions MapComponent] NMM.PawnInteractionManager.InteractionLogs is null. Skipping tick.");
                return;
            }

            // Loop through all the PawnInteractionLogs that NMM maintains for each pawn.
            foreach (var kvpLog in NudityMattersMore.PawnInteractionManager.InteractionLogs)
            {
                Pawn ownerPawn = Find.CurrentMap?.mapPawns?.AllPawns?.FirstOrDefault(p => p.thingIDNumber == kvpLog.Key);

                if (ownerPawn == null) continue; // Skip if pawn is not found on current map

                PawnInteractionLog pawnLog = kvpLog.Value;

                // Handle observer logs (when ownerPawn is observing someone)
                ProcessInteractionsList(ownerPawn, pawnLog.ObserverInteractions, true);


                // Processing observable logs (when ownerPawn is observed by someone)
                ProcessInteractionsList(ownerPawn, pawnLog.ObservedInteractions, false);
            }
        }

        /// <summary>
        /// Processes the list of interactions (ObserverInteractions or ObservedInteractions)
        /// for the given pawn and adds the corresponding entry to its opinion log.
        /// </summary>
        /// <param name="ownerPawn">The pawn that the log belongs to (owner).</param>
        /// <param name="interactions">The list of interactions (from NMM).</param>
        /// <param name="isObserverList">True if this is a list of ObserverInteractions (ownerPawn - observer), false for ObservedInteractions (ownerPawn - observable).</param>
        private void ProcessInteractionsList(Pawn ownerPawn, List<PawnInteraction> interactions, bool isObserverList)
        {
            if (interactions == null || !interactions.Any()) return;

            // Make sure we have a dictionary for this pawn
            if (!lastProcessedInteractionTicks.ContainsKey(ownerPawn.thingIDNumber))
            {
                lastProcessedInteractionTicks[ownerPawn.thingIDNumber] = new Dictionary<string, int>();
            }
            Dictionary<string, int> pawnLastTicks = lastProcessedInteractionTicks[ownerPawn.thingIDNumber];

            // Walk through interactions in reverse order to process the most recent ones first
            // (NMM adds new entries to the top of the list)
            for (int i = 0; i < interactions.Count; i++)
            {
                PawnInteraction interaction = interactions[i];

                if (interaction == null || interaction.Pawn == null) continue;

                Pawn otherPawn = interaction.Pawn; // "Another" pawn in the interaction

                // Generate a unique key for this particular interaction record
                // Include the ID of both pawns, the interaction type, and the state to distinguish between similar
                // but different events by participants or context.
                // The key is formed in such a way as to uniquely identify *this particular event*
                // from the perspective of the log owner.
                string interactionKey = $"{ownerPawn.thingIDNumber}_{(isObserverList ? "ObservedByMe" : "ObservedMe")}_{otherPawn.thingIDNumber}_{interaction.InteractionType}_{interaction.PawnState}";

                // If the record has already been processed (or it is an old record), skip
                if (pawnLastTicks.ContainsKey(interactionKey) && pawnLastTicks[interactionKey] >= interaction.GameTick)
                {
                    continue;
                }

                // This is a new record that needs to be processed
                try
                {
                    // Call SituationalOpinionHelper to generate and add an opinion to the owner log
                    SituationalOpinionHelper.GenerateAndAddSituationalOpinionEntry(
                        ownerPawn,          // The owner of the log whose memory we are updating
                        otherPawn,          // Another pawn in the interaction
                        interaction.InteractionType,
                        interaction.PawnState,
                        interaction.Aware,
                        isObserverList      // Is ownerPawn an observer in this interaction?
                    );
                    // ModLog.Message($"[NMM Opinions MapComponent] Processed new interaction: {interactionKey} for owner {ownerPawn.LabelShort}");
                }
                catch (Exception ex)
                {
                    Log.Error($"[NMM Opinions MapComponent] Error processing situational opinion for {ownerPawn.LabelShort} (Interaction: {interactionKey}): {ex.Message}");
                }

                // Update the last processed tick label for this entry
                pawnLastTicks[interactionKey] = interaction.GameTick;


                // Limit the number of entries we track in pawnLastTicks,
                // to prevent the dictionary from growing infinitely.
                // (Roughly, since there is no mechanism to completely delete old keys without persistence)
            }
        }

        /// <summary>
        /// Serialize/deserialize map component data.
        /// This method is left empty to prevent data from being saved.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            // All attempts to save lastProcessedInteractionTicks have been removed.
            // The data will now only exist in memory during the game.
        }
    }
}
