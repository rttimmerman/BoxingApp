using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BoxingApp.Models;
using Newtonsoft.Json;
using Environment = Android.OS.Environment;

namespace BoxingApp.Helpers
{
    public static class SettingsHelper
    {
        private static string SettingsFolderName => "BoxingTimeSettings";

        private static string SettingsFilePath => Environment.ExternalStorageDirectory.AbsolutePath;

        public static bool IsProfileNameDuplicate(string name)
        {
            var filename = Path.Combine(SettingsFilePath, $"{SettingsFolderName}/{name}Settings.xml");

            return File.Exists(filename);
        }

        public static bool SaveProfileSettings(Profile profile)
        {
            try
            {
                CreateFolderIfItDoesNotExist();

                var filename = GetProfileFilePath(profile.Name);

                var serializedProfile = JsonConvert.SerializeObject(profile);

                using (var file = File.Open(filename, FileMode.Create))
                using (var strm = new StreamWriter(file))
                {
                    strm.Write(serializedProfile);
                }
                return true;
            }
            catch (Exception e)
            {
                //todo: logging
            }

            return false;
        }

        private static void CreateFolderIfItDoesNotExist()
        {
            var folder = Path.Combine(SettingsFilePath,SettingsFolderName);
            if (!Directory.Exists(folder))
            {
               Directory.CreateDirectory(folder);
            }
        }

        public static Profile GetProfileSettings(string profileName)
        {
            var filename = GetProfileFilePath(profileName);

            if (!File.Exists(filename))
            {
                var profile = Constants.DefaultProfile;
                profile.Name = profileName;

                SaveProfileSettings(profile);
            }

            using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (var sr = new StreamReader(fs))
                {
                    var profile = JsonConvert.DeserializeObject<Profile>(sr.ReadToEnd());
                    return profile;
                }
            }
        }

        private static string GetProfileFilePath(string profileName)
        {
            return Path.Combine(SettingsFilePath, $"{SettingsFolderName}/{profileName}.json");
        }

        public static bool DeleteProfile(string profileName)
        {
            try
            {
                var filename = GetProfileFilePath(profileName);
                File.Delete(filename);

                return true;
            }
            catch (Exception e)
            {
                //todo: logging
            }

            return false;
        }

        public static List<string> GetProfileNames()
        {
            try
            {
                var paths = Directory.GetFiles(Path.Combine(SettingsFilePath, SettingsFolderName));

                var profileNames = paths.Select(p=>Path.GetFileName(p)?.Replace(".json", "")).ToList();

                if (!profileNames.Contains(Constants.DefaultProfileName))
                {
                    profileNames.Add(Constants.DefaultProfileName);
                }

                return profileNames;
            }
            catch (Exception e)
            {
                //todo:logging
                throw;
            }
        }
    }
}