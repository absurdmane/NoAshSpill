# No Ash Spill for Obenseuer

A BepInEx 6 (Unity Mono) plugin for **Obenseuer**. Ash from wood-burning devices no longer spills onto the floor when the ash slot is full.

In vanilla, when a stove or furnace burns out and its ash slot can't take all the ash, the rest drops on the ground around it.
With this mod the slot is filled as far as it goes and the extra ash is deleted. Nothing is written to the game's saves, so the mod is safe to add and remove.

## Install

1. Install [BepInEx 6 (Unity Mono)](https://builds.bepinex.dev/projects/bepinex_be) into the game folder.
2. Put `NoAshSpill.dll` into `BepInEx/plugins/NoAshSpill/`.
3. Start the game once; settings appear in `BepInEx/config/obenseuer.absurdmane.noashspill.cfg`.

## Configuration

| Section | Key | Default | Meaning |
|---|---|---|---|
| Mod | `Enabled` | `true` | off = everything as in vanilla; works without a restart |

## Building

Requirements: .NET SDK 6+ and the game with BepInEx 6 installed.

1. Copy `local.props.example` to `local.props` and set `GameDir` to your Obenseuer folder.
2. `dotnet build NoAshSpill -c Release` (add `-p:Deploy=true` to copy the DLL into the game's plugins folder).

## License

MIT © absurdmane, see [LICENSE](LICENSE).
