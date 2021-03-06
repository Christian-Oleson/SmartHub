﻿using Microsoft.Extensions.Options;

namespace SmartHub.Domain.Common.Settings.Validators
{
    public class ApplicationSettingsValidation : IValidateOptions<ApplicationSettings>
    {
        public ValidateOptionsResult Validate(string name, ApplicationSettings options)
        {
            if (string.IsNullOrEmpty(options.FolderName))
            {
                return ValidateOptionsResult.Fail("SmartHub:FolderName must be defined in the appsettings.json file");
            }
            if (string.IsNullOrEmpty(options.PluginFolderName))
            {
                return ValidateOptionsResult.Fail("SmartHub:DefaultPluginFolderName must be defined in the appsettings.json file");

            }
            if (string.IsNullOrEmpty(options.ApplicationName))
            {
                return ValidateOptionsResult.Fail("SmartHub:ApplicationName must be defined in the appsettings.json file");

            }
            return ValidateOptionsResult.Success;
        }
    }
}