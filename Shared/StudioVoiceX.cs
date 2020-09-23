using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
#if HS2
using System;
#endif
using UnityEngine;


namespace StudioVoiceX
{
    [BepInProcess("StudioNEOV2")]
    public partial class StudioVoiceX : BaseUnityPlugin
    {
        public const string Version = "1.0.0";

        public static ConfigEntry<float> MinDistanceX;
        public static ConfigEntry<float> MaxDistanceX;

        private void Awake()
        {
            MinDistanceX = Config.Bind("Voice Distance Settings", "Min Distance Multiplier", 10f, new ConfigDescription("Adjusts minDistance of all voices in the scene", new AcceptableValueRange<float>(1, 100)));
            MaxDistanceX = Config.Bind("Voice Distance Settings", "Max Distance Multiplier", 1f, new ConfigDescription("Adjusts maxDistance of all voices in the scene", new AcceptableValueRange<float>(1, 100)));

            Harmony.CreateAndPatchAll(typeof(StudioVoiceX));
        }

        [HarmonyPostfix]
#if AI
        [HarmonyPatch(typeof(Manager.Voice), "Play")]

        public static void VoiceDistance_Patch(ref Transform __result)
        {
            AudioSource audioSource = __result.GetComponent<AudioSource>();
#endif
#if HS2
        [HarmonyPatch(typeof(Manager.Voice), "Play", new Type[] { typeof(Manager.Voice.Loader)
    })]

        public static void VoiceDistance_Patch(ref AudioSource __result)
        {
            AudioSource audioSource = __result;
#endif
            if (audioSource != null)
            {
                if (MinDistanceX.Value > 0) audioSource.minDistance *= MinDistanceX.Value;
                if (MaxDistanceX.Value > 0) audioSource.maxDistance *= MaxDistanceX.Value;
            }
        }
    }
}
