using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace MP1_Trilogy_Rando_Generator.Config
{
    class AppSettings
    {
        public String prime1RandomizerPath;
        public bool disableSpringBall;
        public bool enableMapFromStart;
        public String outputPath;
        public String outputType;
        public PrimeSettings primeSettings;

        static AppSettings LoadDefaultSettings()
        {
            AppSettings appSettings = new AppSettings();
            appSettings.prime1RandomizerPath = "";
            appSettings.outputPath = "";
            appSettings.outputType = ".ciso";
            appSettings.primeSettings = new PrimeSettings();
            appSettings.SaveToJson();
            return appSettings;
        }

        public static AppSettings LoadFromJson()
        {
            AppSettings appSettings = null;
            String CurDir = Directory.GetCurrentDirectory();
            if (!File.Exists(CurDir+@"\settings.json"))
                return LoadDefaultSettings();

            appSettings = new AppSettings();

            try {
                dynamic json = JObject.Parse(File.ReadAllText(CurDir + @"\settings.json"));

                try {
                    appSettings.prime1RandomizerPath = json.prime1RandomizerPath;
                } catch {
                    appSettings.prime1RandomizerPath = "";
                }

                try {
                    appSettings.outputPath = json.outputPath;
                } catch {
                    appSettings.outputPath = "";
                }

                try {
                    appSettings.outputType = json.outputType;
                } catch {
                    appSettings.outputType = ".ciso";
                }

                try {
                    appSettings.disableSpringBall = json.disableSpringBall;
                } catch {
                    appSettings.disableSpringBall = false;
                }

                try {
                    appSettings.enableMapFromStart = json.enableMapFromStart;
                } catch {
                    appSettings.enableMapFromStart = false;
                }

                appSettings.primeSettings = new PrimeSettings();

                try {
                    appSettings.primeSettings.SetSensitivity((Config.PrimeSettings.SensitivityEnum)Convert.ToUInt32(json.primeSettings.controller.Sensitivity));
                } catch { }

                try {
                    appSettings.primeSettings.SetLockOnFreeAim(json.primeSettings.controller.LockOnFreeAim == "true");
                } catch { }

                try {
                    appSettings.primeSettings.SetRumble(json.primeSettings.controller.Rumble == "true");
                } catch { }

                try {
                    appSettings.primeSettings.SetSwapJumpFire(json.primeSettings.controller.SwapJumpFire == "true");
                } catch { }

                try {
                    appSettings.primeSettings.SetSwapVisorAndBeam(json.primeSettings.controller.SwapVisorAndBeamHyper == "true");
                } catch { }

                try {
                    appSettings.primeSettings.SetBrightness(Convert.ToUInt32(json.primeSettings.display.Brightness));
                } catch { }

                try {
                    appSettings.primeSettings.SetBonusCreditMessages(json.primeSettings.display.BonusCreditMessages == "true");
                } catch { }

                try {
                    appSettings.primeSettings.SetVisorOpacity(Convert.ToUInt32(json.primeSettings.visor.VisorOpacity));
                } catch { }

                try {
                    appSettings.primeSettings.SetHelmetOpacity(Convert.ToUInt32(json.primeSettings.visor.HelmetOpacity));
                } catch { }

                try {
                    appSettings.primeSettings.SetHudLag(json.primeSettings.visor.HudLag == "true");
                } catch { }

                try {
                    appSettings.primeSettings.SetSoundFXVolume(Convert.ToUInt32(json.primeSettings.sound.SoundFXVolume));
                } catch { }

                try {
                    appSettings.primeSettings.SetMusicVolume(Convert.ToUInt32(json.primeSettings.sound.MusicVolume));
                } catch { }

                return appSettings;
            } catch {
                return LoadDefaultSettings();
            }
        }

        public void SaveToJson()
        {
            String CurDir = Directory.GetCurrentDirectory();
            if (File.Exists(CurDir + @"\settings.json"))
                File.Delete(CurDir + @"\settings.json");

            File.WriteAllText(CurDir + @"\settings.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }
    }
}
