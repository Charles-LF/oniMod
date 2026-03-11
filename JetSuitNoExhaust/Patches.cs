using HarmonyLib;
using KMod;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace JetSuitNoExhaust
{
    public class NoJetSuitExhaustMod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
        }
    }

    [HarmonyPatch(typeof(JetSuitMonitor))]
    [HarmonyPatch("Emit")]
    public static class JetSuitMonitor_Emit_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == 0.25f)
                {
                    codes[i].operand = 0f;
                    break;
                }
            }
            return codes.AsEnumerable();
        }
    }
}
