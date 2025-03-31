using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Godot.Fish_the_fishes.Scripts
{
    public class UserSettings
    {
        #region serializable instance
        private static UserSettings _instance = null;

        public bool competitiveMode
        {
            get
            {
                return GameManager.Instance.GetTree().Root.ContentScaleAspect == Window.ContentScaleAspectEnum.Keep;
            }
            set
            {
                GD.Print("setting competitive mode");
                GameManager.Instance.GetTree().Root.ContentScaleAspect = value ? Window.ContentScaleAspectEnum.Keep : Window.ContentScaleAspectEnum.Expand;
            }
        }

        private Dictionary<string, (bool isMuted, float volume)> _audioSettings;

        public UserSettings()
        {
            if (_instance != null)
                return;
            _audioSettings = new();
            _instance = this;
        }
        #endregion

        public static bool CompetitiveMode
        {
            get { return _instance.competitiveMode; }
            set { _instance.competitiveMode = value; }
        }

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
