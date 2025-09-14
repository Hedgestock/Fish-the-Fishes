using Godot;

namespace WaffleStock
{
    public class UserSettings
    {
        public static bool CompetitiveMode
        {
            get
            {
                return GameManager.Instance.GetTree().Root.ContentScaleAspect == Window.ContentScaleAspectEnum.Keep;
            }
            set
            {
                GameManager.Instance.GetTree().Root.ContentScaleAspect = value ? Window.ContentScaleAspectEnum.Keep : Window.ContentScaleAspectEnum.Expand;
            }
        }

        public static bool Vibrations = true;
        public static bool WaterEffect = true;

        public static bool Save(string path)
        {
            ConfigFile config = new();

            config.SetValue(null, nameof(CompetitiveMode), CompetitiveMode);
            config.SetValue(null, nameof(Vibrations), Vibrations);
            config.SetValue(null, nameof(WaterEffect), WaterEffect);

            for (int bus = 0; bus < AudioServer.BusCount; bus++)
            {
                config.SetValue("Audio", AudioServer.GetBusName(bus) + "IsMuted", AudioServer.IsBusMute(bus));
                config.SetValue("Audio", AudioServer.GetBusName(bus) + "Volume", AudioServer.GetBusVolumeLinear(bus));
            }

            Error err = config.Save(path);

            if (err != Error.Ok)
            {
                GD.PrintErr(err);
                return false;
            }

            return true;
        }

        public static bool Load(string path)
        {
            ConfigFile config = new();

            Error err = config.Load(path);

            if (err != Error.Ok)
            {
                GD.PrintErr($"Error loading settings file, creating a fresh one: {err}");
            }

            CompetitiveMode = (bool)config.GetValue(null, nameof(CompetitiveMode), CompetitiveMode);
            Vibrations = (bool)config.GetValue(null, nameof(Vibrations), Vibrations);
            WaterEffect = (bool)config.GetValue(null, nameof(WaterEffect), WaterEffect);

            for (int bus = 0; bus < AudioServer.BusCount; bus++)
            {
                AudioServer.SetBusMute(bus, (bool)config.GetValue("Audio", AudioServer.GetBusName(bus) + "IsMuted", false));
                AudioServer.SetBusVolumeLinear(bus, (float)config.GetValue("Audio", AudioServer.GetBusName(bus) + "Volume", 1));
            }

            return true;
        }
    }
}
