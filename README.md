# Rekindle — Cinderhollow

A Unity 6.3 / URP fantasy restoration prototype. Open this folder in **Unity 6000.3.10f1**. The game scene is `Assets/Rekindle/Content/Scenes/Cinderhollow.unity`. Press Play, then Begin journey. Generated scene assets are created by `Rekindle > Author vertical slice`; this replaces the generated scene, so save manual edits elsewhere before regenerating.

Windows build: `Builds/Windows/Rekindle.exe`. Keep the complete Windows folder together, including Rekindle_Data, MonoBleedingEdge, and UnityPlayer.dll. Rebuild through `Rekindle > Build Windows`. No purchased assets or external art downloads are needed.

Controls: W/S move, A/D turn, Q/E strafe; LMB drag orbits, RMB drag steers (A/D strafe while RMB is held); wheel zooms. Tab cycles nearby hostiles; right-click selects a hostile and starts auto-attack. F or right-click interacts nearby. 1 strike, 2 Ember Cleave, 3/Space evade, 4 wildfruit, 5/I inventory, 6 rest near the settlement hearth. Escape pauses. Click two occupied inventory slots to swap them.

Walkthrough:
1. Speak to Elian's golden wisp near the ruined Hearth House, west of the square.
2. Gather fallen heartwood and salvage stone around the settlement. Restore the house twice: 4 wood + 2 stone, then 3 wood. Elian transitions through wisp and spectre to a restored resident.
3. Stabilize Mira's Forge east of the square with 4 stone + 2 iron. Mira needs her personal hammer before she can fully return.
4. Gather wildfruit and restore the Community Garden with 2 wood + 2 food, then 2 wood. Wildfruit can also heal you in combat.
5. Follow the road east and north through Whisperwood. Ashbound close to melee; cinder seers attack from farther away. Tab target, close to melee reach, strike, and spend energy on Ember Cleave. Dodge telegraphed attacks and heal with wildfruit.
6. Enter the Sunken Sanctuary beneath the old lintel. Broken screens alter the approach to the final courtyard. Release its guardians, then defeat the Ash Warden. It uses two ordinary strikes followed by a wide, delayed rupture with a visible danger ring. Leave the ring during its windup and punish the recovery.
7. Victory releases Mira's hammer and the Dawn Ember, removes roadside corruption, restores dawn blooms, and activates a waystone in the sanctuary's northeast corner.
8. Use the waystone to return home. Give Mira her hammer through the forge restoration interaction. She returns; the Dawn Blade replaces your starting weapon and strengthens basic strikes.
9. Stabilize the Dawn Beacon with 4 stone + 2 iron, then offer the Dawn Ember. With the smith and garden restored, the valley warms, lanterns and festival cloth appear, and the final objective acknowledges the permanent transformation.

Progress autosaves after gathering, restoration, and combat rewards, plus quit. Continue restores building stages, dungeon kills, depleted nodes, inventory, position, and health. Saves live under Unity's persistent data directory (`AppData/LocalLow/Rekindle Studio/Rekindle/rekindle-v1.json` on Windows), with a previous-write backup. Begin journey creates a fresh in-memory journey; existing progress is replaced by its first autosave.

Validation tools: `Rekindle > Validate rules and scene`, `Rekindle > Run runtime walkthrough`, and Unity Test Runner's Rekindle.Tests EditMode suite. The automated runtime walkthrough uses test teleports and temporary invulnerability; it verifies integrated commands, not natural pacing or player combat skill. Evidence and actual validation status are in `Artifacts/` and `Docs/DeliveryReport.md`.

The Windows prototype uses Mono with Burst AOT disabled; native performance optimization is deferred. The runtime walkthrough menu also requests a Windows build after its automated Play Mode pass. Automated evidence is deliberately separate from an unassisted player playthrough.


Current handoff: Windows build succeeded (0 errors, 2 warnings). Standalone launch and NavMesh registration were observed; automated desktop input/save validation was blocked by window focus access. See Artifacts/windows-validation.txt. The automated completed journey was archived under Artifacts/CompletedJourney.json, leaving a fresh local profile. Visual presentation remains prototype quality; the 20-40 minute target has not been measured.
