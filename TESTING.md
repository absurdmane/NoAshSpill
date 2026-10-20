# No Ash Spill — test plan

Build and install: `dotnet build -c Release -p:Deploy=true` in `NoAshSpill/`.
Log: `BepInEx\LogOutput.log` — look for `No Ash Spill 1.0.0 loaded` and no `call not found` warning.

- [ ] The game starts, the log has the line, no errors; the config has `[Mod] Enabled`.
- [ ] Stove/furnace with the ash slot already full of ash: burn wood until it goes out — no ash on the floor, the slot stays full.
- [ ] Ash slot partly full: it fills up to the stack limit, the rest vanishes, nothing on the floor.
- [ ] Ash slot empty: ash lands in the slot as in vanilla.
- [ ] Ash slot holds some other item: the new ash vanishes, nothing on the floor.
- [ ] Burning a big stack of wood at once (more ash than one stack): slot full, no spill.
- [ ] Other wood-burning devices (every kind you can find) behave the same.
- [ ] `[Mod] Enabled = false` while playing: overflow drops on the floor again.
- [ ] Save and load with a burning device; removing the mod leaves the save working.
