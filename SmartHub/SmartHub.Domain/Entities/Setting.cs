﻿using SmartHub.Domain.Common.Enums;

namespace SmartHub.Domain.Entities
{
	public class Setting : BaseEntity
	{
		/// <summary>
		/// Tells what kind of setting this is
		/// </summary>
		public SettingTypes Type { get; private set; }

		public bool IsActive { get; private set; }
		public bool IsDefault { get; private set; }
		public string WatchPathAbsolut { get; private set; }
		public string PluginPath { get; private set; }

		public string DownloadServerUrl { get; private set; }
		public string? Filepath { get; private set; } // logic will come with #139

		public string CreatorName { get; private set; }


		protected Setting()
		{
		}

		public Setting(string name, string description, bool isActive, string watchPathAbsolut, string pluginPath, string downloadServerUrl,
			string creator, SettingTypes type) :
			base(name, description)
		{
			IsActive = isActive;
			WatchPathAbsolut = watchPathAbsolut;
			PluginPath = pluginPath;
			IsDefault = type == SettingTypes.Default;
			DownloadServerUrl = downloadServerUrl;
			CreatorName = creator;
			Type = type;
		}
	}
}