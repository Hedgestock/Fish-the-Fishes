using System;
using System.Text.Json;
using System.Threading;
using static Godot.Fish_the_fishes.Scripts.UserData;

namespace Godot.Fish_the_fishes.Scripts
{
    public class UserSettings
    {
        #region serializable instance
        private static UserSettings _instance = null;

        public bool _competitiveMode { get; set; }
        public bool _muteMaster { get; set; }
        public float _masterVolume { get; set; }
        public bool _muteMusic { get; set; }
        public float _musicVolume { get; set; }
        public bool _muteSFX { get; set; }
        public float _SFXVolume { get; set; }
        public bool _muteFishes { get; set; }
        public float _fishesVolume { get; set; }

        public UserSettings()
        {
            if (_instance != null)
                return;

            _competitiveMode = false;
            _muteMaster = false;
            _masterVolume = 1;
            _muteMusic = false;
            _musicVolume = 1;
            _muteSFX = false;
            _SFXVolume = 1;
            _muteFishes = false;
            _fishesVolume = 1;

            _instance = this;
        }
        #endregion

        public static bool CompetitiveMode
        {
            get { return _instance._competitiveMode; }
            set { _instance._competitiveMode = value; }
        }

        #region volume
        public static bool MuteMaster
        {
            get { return _instance._muteMaster; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), value);
                _instance._muteMaster = value;
            }
        }
        public static float MasterVolume
        {
            get { return _instance._masterVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), Mathf.LinearToDb(value));
                _instance._masterVolume = value;
            }
        }
        public static bool MuteMusic
        {
            get { return _instance._muteMusic; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), value);
                _instance._muteMusic = value;
            }
        }
        public static float MusicVolume
        {
            get { return _instance._musicVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), Mathf.LinearToDb(value));
                _instance._musicVolume = value;
            }
        }
        public static bool MuteSFX
        {
            get { return _instance._muteSFX; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("SFX"), value);
                _instance._muteSFX = value;
            }
        }
        public static float SFXVolume
        {
            get { return _instance._SFXVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("SFX"), Mathf.LinearToDb(value));
                _instance._SFXVolume = value;
            }
        }
        public static bool MuteFishes
        {
            get { return _instance._muteFishes; }
            set
            {
                AudioServer.SetBusMute(AudioServer.GetBusIndex("Fishes"), value);
                _instance._muteFishes = value;
            }
        }
        public static float FishesVolume
        {
            get { return _instance._fishesVolume; }
            set
            {
                AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Fishes"), Mathf.LinearToDb(value));
                _instance._fishesVolume = value;
            }
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
