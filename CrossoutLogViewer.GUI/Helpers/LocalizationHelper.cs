/* 
 * All credit to GrumpyDev (Steven Robbins)
 */

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CrossoutLogView.GUI.Services;

namespace CrossoutLogView.GUI.Helpers
{
    /// <summary>
    ///     Helper class for binding to resource strings
    /// </summary>
    public class LocalisationHelper : INotifyPropertyChanged
    {
        private string _defaultManager;

        /// <summary>
        ///     Initializes a new instance of the LocalisationHelper class.
        /// </summary>
        public LocalisationHelper()
        {
            if (!DesignHelpers.IsInDesignModeStatic)
                // Refresh all the bindings when the locale changes
                ResourceManagerService.LocaleChanged += (s, e) => RaisePropertyChanged(string.Empty);
        }

        /// <summary>
        ///     Gets a resource string from the ResourceManager
        ///     You can bind to this property using the .[KEY] syntax e.g.:
        ///     {Binding Source={StaticResource localisation}, Path=.[MainScreenResources.IntroTextLine1]}
        /// </summary>
        /// <param name="key">Key to retrieve in the format [ManagerName].[ResourceKey]</param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (key is null)
                    throw new ArgumentNullException(nameof(key));
                var isValidKey = ValidKey(key);
                if (!isValidKey && string.IsNullOrEmpty(DefaultManager))
                    throw new ArgumentException(
                        "Key cannot be empty, and must be in the valid [ManagerName].[ResourceKey] format. Key = \"" +
                        key + "\"");
                if (DesignHelpers.IsInDesignModeStatic)
                    return key; //throw new Exception("Design mode is not supported.");
                if (isValidKey)
                    return ResourceManagerService.GetResourceString(GetManagerKey(key), GetResourceKey(key));
                return ResourceManagerService.GetResourceString(DefaultManager, key);
            }
        }

        /// <summary>
        ///     Gets or sets a string representing the default ResourceManager.
        ///     When set a resource string can be obtained without specifing a ManagerName, in that case the value of
        ///     DefaultManager is used as such.
        /// </summary>
        public string DefaultManager
        {
            get => _defaultManager;
            set
            {
                _defaultManager = value;
                RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Private Key Methods

        private bool ValidKey(string input)
        {
            return input.Contains(".");
        }

        private string GetManagerKey(string input)
        {
            return input.Split('.')[0];
        }

        private string GetResourceKey(string input)
        {
            return input.Substring(input.IndexOf('.') + 1);
        }

        #endregion
    }
}