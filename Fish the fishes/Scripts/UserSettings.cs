using System;
using System.Text.Json;

namespace Godot.Fish_the_fishes.Scripts
{
    public class UserSettings
    {
        #region serializable instance
        private static UserSettings _instance = null;

        private bool _competitiveMode = false;
        public bool competitiveMode
        {
            get { return _competitiveMode; }
            set
            {
                GameManager.Instance.GetTree().Root.ContentScaleAspect = value ? Window.ContentScaleAspectEnum.Keep : Window.ContentScaleAspectEnum.Expand;
                _competitiveMode = value;
            }
        }

        private bool _muteMaster = false;
        public bool muteMaster
        {
            get { return _muteMaster; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), value);
                _muteMaster = value;
            }
        }
        private float _masterVolume = 1;
        public float masterVolume
        {
            get { return _masterVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Mathf.LinearToDb(value));
                _masterVolume = value;
            }
        }
        private bool _muteMusic = false;
        public bool muteMusic
        {
            get { return _muteMusic; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), value);
                _muteMusic = value;
            }
        }
        private float _musicVolume = 1;
        public float musicVolume
        {
            get { return _musicVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(value));
                _musicVolume = value;
            }
        }
        private bool _muteSFX = false;
        public bool muteSFX
        {
            get { return _muteSFX; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), value);
                _muteSFX = value;
            }
        }
        private float _sfxVolume = 1;
        public float sfxVolume
        {
            get { return _sfxVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), Mathf.LinearToDb(value));
                _sfxVolume = value;
            }
        }

        private bool _muteAmbiance = false;
        public bool muteAmbiance
        {
            get { return _muteAmbiance; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Ambiance"), value);
                _muteAmbiance = value;
            }
        }
        private float _ambianceVolume = .1f;
        public float ambianceVolume
        {
            get { return _ambianceVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Ambiance"), Mathf.LinearToDb(value));
                _ambianceVolume = value;
            }
        }

        private bool _muteFishes = false;
        public bool muteFishes
        {
            get { return _muteFishes; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Fishes"), value);
                _muteFishes = value;
            }
        }
        private float _fishesVolume = 1;
        public float fishesVolume
        {
            get { return _fishesVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Fishes"), Mathf.LinearToDb(value));
                _fishesVolume = value;
            }
        }

        public UserSettings()
        {
            if (_instance != null)
                return;
            _instance = this;
        }
        #endregion

        public static bool CompetitiveMode
        {
            get { return _instance.competitiveMode; }
            set { _instance.competitiveMode = value; }
        }

        #region volume
        public static bool MuteMaster
        {
            get { return _instance.muteMaster; }
            set { _instance.muteMaster = value; }
        }
        public static float MasterVolume
        {
            get { return _instance.masterVolume; }
            set { _instance.masterVolume = value; }
        }
        public static bool MuteMusic
        {
            get { return _instance.muteMusic; }
            set { _instance.muteMusic = value; }
        }
        public static float MusicVolume
        {
            get { return _instance.musicVolume; }
            set { _instance.musicVolume = value; }
        }
        public static bool MuteSFX
        {
            get { return _instance.muteSFX; }
            set { _instance.muteSFX = value; }
        }
        public static float SFXVolume
        {
            get { return _instance.sfxVolume; }
            set { _instance.sfxVolume = value; }
        }
        public static bool MuteAmbiance
        {
            get { return _instance.muteAmbiance; }
            set { _instance.muteAmbiance = value; }
        }
        public static float AmbianceVolume
        {
            get { return _instance.ambianceVolume; }
            set { _instance.ambianceVolume = value; }
        }
        public static bool MuteFishes
        {
            get { return _instance.muteFishes; }
            set { _instance.muteFishes = value; }
        }
        public static float FishesVolume
        {
            get { return _instance.fishesVolume; }
            set { _instance.fishesVolume = value; }
        }
        #endregion

        public static void Reset()
        {
            _instance = null;
            new UserSettings();
        }
        public static string Serialize()
        {
            return JsonSerializer.Serialize(_instance);
        }
        public static bool Deserialize(string json)
        {
            try
            {
                _instance = JsonSerializer.Deserialize<UserSettings>(json);
                //PropertyInfo[] properties = typeof(UserSettings).GetProperties();

                //// This makes sure that we recover from corrupted data where any property is set to `null`
                //// by replacing it with a new empty object.
                //foreach (PropertyInfo property in properties)
                //{
                //    if (typeof(UserData).GetProperty(property.Name).GetValue(_instance) == null)
                //    {
                //        typeof(UserData).GetProperty(property.Name).SetValue(_instance, property.PropertyType.GetConstructor(new Type[] { }).Invoke(new object[] { }));
                //    }
                //}
                return true;
            }
            catch (Exception e)
            {
                GD.PrintErr(e);
                return false;
            }
        }
    }
}
