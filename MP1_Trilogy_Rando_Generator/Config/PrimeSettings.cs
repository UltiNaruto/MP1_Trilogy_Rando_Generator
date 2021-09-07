using System;

namespace MP1_Trilogy_Rando_Generator.Config
{
    class PrimeSettings
    {
        public enum SensitivityEnum : UInt32 {
            Basic = 0,
            Standard = 1,
            Advanced = 2
        }

        public class Controller
        {
            public SensitivityEnum Sensitivity;
            public bool LockOnFreeAim { get; private set; }
            public bool Rumble { get; private set; }
            public bool SwapJumpFire { get; private set; }
            public bool SwapVisorAndBeam { get; private set; }
            public Controller(SensitivityEnum sensitivity, bool lockOnFreeAim, bool rumble, bool swapJumpFire, bool swapVisorAndBeamHyper)
            {
                Sensitivity = sensitivity;
                LockOnFreeAim = lockOnFreeAim;
                Rumble = rumble;
                SwapJumpFire = swapJumpFire;
                SwapVisorAndBeam = swapVisorAndBeamHyper;
            }

            public UInt32 GetControllerEnum()
            {
                UInt32 value = 0;
                if (LockOnFreeAim)
                    value |= 0x2;
                if (SwapVisorAndBeam)
                    value |= 0x4;
                if (SwapJumpFire)
                    value |= 0x8;
                if (Rumble)
                    value |= 0x40;
                return value;
            }

            public void Set(String setting, SensitivityEnum value)
            {
                if (setting == "Sensitivity")
                    Sensitivity = value;
            }

            public void Set(String setting, bool value)
            {
                if (setting == "Lock On/Free Aim")
                    LockOnFreeAim = value;
                if (setting == "Rumble")
                    Rumble = value;
                if (setting == "Swap Jump/Fire")
                    SwapJumpFire = value;
                if (setting == "Swap Visor and Beam")
                    SwapVisorAndBeam = value;
            }
        }
        public class Display {
            public UInt32 Brightness { get; private set; }
            // hint system is always off
            public bool BonusCreditMessages { get; private set; }
            public Display(UInt32 brightness, bool hudLag, bool bonusCreditMessages)
            {
                Brightness = brightness;
                BonusCreditMessages = bonusCreditMessages;
            }

            public void Set(String setting, UInt32 value)
            {
                if (setting == "Brightness")
                    Brightness = value;
            }

            public void Set(String setting, bool value)
            {
                if (setting == "Bonus Credit Messages")
                    BonusCreditMessages = value;
            }

            public UInt32 GetDisplayEnum2()
            {
                return (UInt32)(BonusCreditMessages ? 0x80 : 0);
            }
        }

        [Serializable]
        public class Visor
        {
            public UInt32 VisorOpacity { get; private set; }
            public UInt32 HelmetOpacity { get; private set; }
            public bool HudLag { get; private set; }
            public Visor(UInt32 visorOpacity, UInt32 helmetOpacity, bool hudLag)
            {
                VisorOpacity = visorOpacity;
                HelmetOpacity = helmetOpacity;
                HudLag = hudLag;
            }

            public void Set(String setting, UInt32 value)
            {
                if (setting == "Visor Opacity")
                    VisorOpacity = value;
                if (setting == "Helmet Opacity")
                    HelmetOpacity = value;
            }

            public void Set(String setting, bool value)
            {
                if (setting == "HUD Lag")
                    HudLag = value;
            }

            public UInt32 GetVisorEnum()
            {
                return (UInt32)(HudLag ? 0x80 : 0);
            }
        }

        [Serializable]
        public class Sound {
            public UInt32 SoundFXVolume { get; private set; }
            public UInt32 MusicVolume { get; private set; }
            public Sound(UInt32 soundFXVol, UInt32 musicVol)
            {
                SoundFXVolume = soundFXVol;
                MusicVolume = musicVol;
            }

            public void Set(String setting, UInt32 value)
            {
                if (setting == "Sound FX")
                    SoundFXVolume = value;
                if (setting == "Music")
                    MusicVolume = value;
            }
        }

        public Controller controller;
        public Display display;
        public Visor visor;
        public Sound sound;

        public PrimeSettings()
        {
            controller = new Controller(SensitivityEnum.Standard, true, true, false, false);
            display = new Display(50, true, true);
            visor = new Visor(100, 100, true);
            sound = new Sound(100, 100);
        }

        public void SetSensitivity(SensitivityEnum sensitivity)
        {
            controller.Set("Sensitivity", sensitivity);
        }

        public void SetLockOnFreeAim(bool state)
        {
            controller.Set("Lock On/Free Aim", state);
        }

        public void SetRumble(bool state)
        {
            controller.Set("Rumble", state);
        }

        public void SetSwapJumpFire(bool state)
        {
            controller.Set("Swap Jump/Fire", state);
        }

        public void SetSwapVisorAndBeam(bool state)
        {
            controller.Set("Swap Visor and Beam", state);
        }

        public void SetBrightness(UInt32 value)
        {
            display.Set("Brightness", value);
        }

        public void SetBonusCreditMessages(bool state)
        {
            display.Set("Bonus Credit Messages", state);
        }

        public void SetVisorOpacity(UInt32 value)
        {
            visor.Set("Visor Opacity", value);
        }

        public void SetHelmetOpacity(UInt32 value)
        {
            visor.Set("Helmet Opacity", value);
        }

        public void SetHudLag(bool state)
        {
            visor.Set("HUD Lag", state);
        }

        public void SetSoundFXVolume(UInt32 vol)
        {
            sound.Set("Sound FX", vol);
        }

        public void SetMusicVolume(UInt32 vol)
        {
            sound.Set("Music", vol);
        }
    }
}