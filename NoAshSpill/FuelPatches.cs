using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using OS.Items;
using UnityEngine;

namespace NoAshSpill
{
    /// <summary>
    /// <c>Fuel.SpawnRefuse</c> / <c>SpawnRefuseAll</c> put burnt leftovers (ash) into <c>RefuseSlot</c> with
    /// <c>SlotController.AddItem(..., dropper)</c>, which drops whatever doesn't fit on the ground next to the device.
    /// Those two call sites are redirected to <see cref="AddWithoutDrop"/>: same insert, overflow is discarded.
    /// Only the call sites inside <c>Fuel</c> change; every other <c>AddItem</c> caller is untouched.
    /// </summary>
    [HarmonyPatch(typeof(Fuel))]
    internal static class FuelPatches
    {
        private static readonly MethodInfo AddItemMethod = AccessTools.Method(typeof(SlotController), nameof(SlotController.AddItem),
            new[] { typeof(Item), typeof(int), typeof(int), typeof(object[]), typeof(GameObject), typeof(bool), typeof(int), typeof(ItemOperations.DropItemValues) });

        private static readonly MethodInfo ReplacementMethod = AccessTools.Method(typeof(FuelPatches), nameof(AddWithoutDrop));

        [HarmonyTranspiler]
        [HarmonyPatch("SpawnRefuse")]
        private static IEnumerable<CodeInstruction> SpawnRefuseTranspiler(IEnumerable<CodeInstruction> instructions) => Redirect(instructions, "SpawnRefuse");

        [HarmonyTranspiler]
        [HarmonyPatch("SpawnRefuseAll")]
        private static IEnumerable<CodeInstruction> SpawnRefuseAllTranspiler(IEnumerable<CodeInstruction> instructions) => Redirect(instructions, "SpawnRefuseAll");

        private static IEnumerable<CodeInstruction> Redirect(IEnumerable<CodeInstruction> instructions, string method)
        {
            int replaced = 0;
            foreach (CodeInstruction ins in instructions)
            {
                if (ins.Calls(AddItemMethod))
                {
                    replaced++;
                    yield return new CodeInstruction(System.Reflection.Emit.OpCodes.Call, ReplacementMethod);
                }
                else
                {
                    yield return ins;
                }
            }
            if (replaced == 0)
                Plugin.Log.LogWarning($"Fuel.{method}: SlotController.AddItem call not found, ash overflow is left as in vanilla (game updated?)");
        }

        /// <summary>
        /// Same arguments as <c>SlotController.AddItem</c>. Fills the slot as far as it goes and throws the rest away
        /// (<c>dropItems: false</c>); with the mod off it calls the original.
        /// </summary>
        private static bool AddWithoutDrop(SlotController slot, Item item, int amount, int owner, object[] meta, GameObject dropper,
            bool notify, int stackAmount, ItemOperations.DropItemValues dropItemValues)
        {
            if (!Plugin.On)
                return slot.AddItem(item, amount, owner, meta, dropper, notify, stackAmount, dropItemValues);
            return slot.AddItemAndReturnRemaining(item, amount, owner, meta, dropItems: false, dropper, notify, stackAmount, dropItemValues) == 0;
        }
    }
}
