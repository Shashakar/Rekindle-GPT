# Delivery report — Rekindle / Cinderhollow

This is a connected, playable Unity restoration prototype, with a saved production scene and an authored beginning, middle, and ending. It is not a production-art action RPG or a demonstrated 20–40 minute first session. The compact region and simple geometric presentation remain below the challenge's requested artistic and pacing standard.

The implementation contains four restoration sites (Hearth House, Mira's Forge, Community Garden, Dawn Beacon), three souls, 43 authored interactions, a forest road and dangerous approach, seven hostile encounters including the Ash Warden, and a ruined sanctuary. All content is created and saved through supported Unity Editor APIs. The runtime does not procedurally generate the map or dungeon.

Implemented and exercised in Unity:

- Cursor-first controller with Input System movement, turn/strafe, orbit/steering, zoom and camera collision. The automated walkthrough injects W through a test keyboard and observes real CharacterController movement. Other mouse/control combinations have source implementation and need broader hands-on review.
- Shared Character health, energy, death/revival; shared committed attacks with range/hostility validation, energy spending, cooldown/recovery, auto-attack, cleave, and evasive invulnerability. Player and hostile controllers compose the same rules.
- Melee and ranged hostile configurations, NavMesh approach, perception, leash/reset, and a boss with an every-third-attack rupture telegraph and recovery opening. A guardian and the boss were defeated through production attack commands in Play Mode.
- Four material types, depleted gathering nodes, shared inventory, atomic restoration costs, occupied-stack swapping, food consumption, and authored item/ability definition assets.
- Staged structures, wisp/spectre/restored inhabitants, forge-based weapon improvement, restored hearth healing, and a one-time garden harvest service. Service buttons have been integrated; individual services need additional hands-on coverage beyond the main restoration walkthrough.
- Boss rewards, corruption removal, roadside regrowth, return waystone, final settlement lighting and festival transformation, derived objectives, and a visible ending.
- Main menu, Continue, HUD, target information, six actions, inventory, restoration panels, pause/save/quit, controls, and notifications.
- Explicit versioned snapshots, stable IDs, repeated safe writes with backup, corruption recovery, and actual scene-reload/Continue restoration of the completed world, weapon, and absent boss.

Validation evidence:

- `Artifacts/rule-validation.txt`: 12 deterministic rule and authored-scene checks. A repeated-write defect was found and fixed; the failing result is preserved separately.
- `Artifacts/nunit-results.xml` and `nunit-summary.txt`: five Unity Test Framework EditMode tests.
- `Artifacts/runtime-validation.txt`: 18/18 integrated Play Mode milestones passed. The command walkthrough uses test teleports and temporary player invulnerability to reach expensive states. It does not prove natural combat balance, ordinary traversal of the entire route, or session length.
- `Artifacts/reload-result.txt`: the saved scene reopened with 12,285 baked navigation vertices present. Navigation is stored as a project asset.
- `Artifacts/01-arrival*.png`, `02-sanctuary*.png`, `03-rekindled*.png`: actual Unity camera/Game View captures. Camera renders omit UI; files ending in `-hud` capture the Game View.
- `Artifacts/build-result.txt`: final non-Development Windows build succeeded with zero errors and two warnings (104,286,248 reported bytes). The standalone process launched, loaded the authored scene, created its game window, and registered all seven agents on the saved NavMesh. Automated input/save smoke remains BLOCKED by foreground-window access in this desktop session; no successful standalone Begin, movement, first interaction, or save test is claimed. Initial Development attempts also encountered a firewall prompt; the final build omits the development player connection. See `Artifacts/windows-smoke.txt`, `windows-player.log`, and `unity-final.log`.

Partial implementations and known limitations:

- Custom geometric characters, roofs/walls, foliage, and props are coherent prototype substitutes. They are visibly primitive. Procedural limb and sword motion does not replace a production animation set. No imported rigged characters, authored animation controller set, cinematic, voice acting, soundtrack, or rich ambient soundscape is provided.
- UI uses IMGUI with colored item marks and scaled panels, rather than a production fantasy icon set or retained-mode UI. Inventory compacts its stack list and swaps occupied positions; drag-and-drop and placement into arbitrary empty slots are not implemented. Hotbar cooldown presentation and key rebinding UI are absent.
- Ranged attacks resolve after a windup with range checks and damage feedback; they have no simulated projectile or cover/line-of-sight system. Boss telegraphs are simple circles. Encounter variety and traversal need independent player evaluation.
- Residents return physically and provide services but have no work schedules or ambient animation. The restoration arc is deliberately fixed and small. Objective, dungeon, and building facts share a deterministic world aggregate rather than separate extensible authoring frameworks.
- The map has painted terrain, relief, paths, cedar trees, rock boundaries, river treatment, landmarks, and a handcrafted sanctuary. Much decoration is seeded around that fixed layout. Environmental storytelling and density need another art/level-design pass.
- The 20–40 minute target is not measured or established. A new-player usability test, unassisted combat run, performance capture, broad resolution/input testing, and release-quality Windows playthrough remain outstanding.

Not implemented: multiplayer, character creation, classes, crafting economy, advanced NPC simulation, multiple regions, a full campaign, or the other excluded features.

The architecture and tradeoffs are in `Docs/Architecture.md`; dependencies and licensing are in `Docs/Attribution.md`. The complete intended player walkthrough and build instructions are in `README.md`.

Windows build configuration uses Mono with Burst AOT disabled through the package's platform settings. Native optimization stalled in this environment; managed gameplay is retained. Performance remains unmeasured. The unused Unity AI Inference package was removed to avoid unrelated compute-shader compilation; other editor AI tools can still emit licensing/service errors.


Validation scope: the implemented systems listed above were exercised in Unity, with specific service and control combinations still awaiting hands-on coverage. Partial implementations are identified explicitly below the evidence list; excluded campaign and multiplayer systems are not implemented. Windows validation is recorded independently in Artifacts/windows-smoke.txt.

Standalone startup diagnostics confirmed every authored enemy agent bound to the persisted NavMesh (12,285 vertices). The initial native agent-enable warnings occur before registration completes; they did not indicate missing navigation at Start. See Artifacts/windows-navigation.txt.


Quality review: technical rules and integrated restoration are validated by the checks above; gameplay completion was tested with assistance. Experience quality still needs an unfamiliar player's unassisted run. Visual quality is a cohesive geometric prototype and does not meet the requested professional environment/art standard. The original challenge is therefore only partially fulfilled despite the playable Unity loop and successful Windows build.


