using HarmonyLib;
using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Diagnostics;
using UnityEngine;
using KMod;

namespace IceBox
{
    internal class Paches:UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            PUtil.InitLibrary(false);
            new POptions().RegisterOptions(this, typeof(Settings));
            Settings.Init(POptions.ReadSettings<Settings>());
        }

        [HarmonyPatch("Initialize")]
        [HarmonyPatch(typeof(Db))]
        public static class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                Utils.AddBuildingToTech("FinerDining", IceBoxConfig.ID);
                Utils.AddPlan("Food", "storage", IceBoxConfig.ID, "Refrigerator");
            }
        }
        [HarmonyPatch(typeof(Localization), "Initialize")]
        public class Localization_Initialize_Patch
        {
            public static void Postfix()
            {
                Utils.Translate(typeof(STRINGS));
            }
        }

        [HarmonyPatch(typeof(RefrigeratorConfig))]
        [HarmonyPatch("CreateBuildingDef")]
        public class RefrigeratorConfig_Power_Sttings
        {
            private static void Postfix(BuildingDef __result)
            {
                __result.EnergyConsumptionWhenActive = Settings.GetSettings().RefrigeratorPower;
                __result.SelfHeatKilowattsWhenActive = Settings.GetSettings().RefrigeratorSelfHeatKilowattsWhenActive;
            }
        }
        [HarmonyPatch(typeof(RefrigeratorConfig))]
        [HarmonyPatch("DoPostConfigureComplete")]
        internal class RefrigeratorConfig_Str_Sttings
        {
            private static void Postfix(GameObject go)
            {
                go.AddOrGet<Storage>().capacityKg = Settings.GetSettings().RefrigeratorStorage;
            }
        }
        [HarmonyPatch(typeof(RefrigeratorController.Def), (MethodType)3)]
        public class RefrigeratorController_Def
        {
            private static void Postfix(ref float ___simulatedInternalTemperature)
            {
                ___simulatedInternalTemperature = Settings.GetSettings().SimulatedInternalTemperature;
            }
        }
        [HarmonyPatch(typeof(Rottable), "AtmosphereQuality")]
        internal class Rottable_RotAtmosphereQuality
        {
            public static void Postfix(ref Rottable.RotAtmosphereQuality __result)
            {
                __result = (Rottable.RotAtmosphereQuality)1;
            }
        }
    }
}
