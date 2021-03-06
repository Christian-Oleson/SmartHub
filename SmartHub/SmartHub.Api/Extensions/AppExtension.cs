﻿using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Serilog;
using SmartHub.Api.Middleware;

namespace SmartHub.Api.Extensions
{
	public static class AppExtension
	{
		public static void ConfigureSwagger(this IApplicationBuilder app)
		{
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartHub API V0.5");
			});
		}

		public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<ExceptionMiddleware>();
		}

		public static void ConfigureHangfire(this IApplicationBuilder app)
		{
			app.UseHangfireDashboard();
		}

		public static void ShowLocalIpv4()
		{
			foreach (var ip in NetworkInterface.GetAllNetworkInterfaces()
				.Select(item => new
				{
					item,
					types = new List<NetworkInterfaceType>
					{
						NetworkInterfaceType.Ethernet, NetworkInterfaceType.Wireless80211
					}
				})
				.Where(t =>
					t.types.Contains(t.item.NetworkInterfaceType) &&
					t.item.OperationalStatus == OperationalStatus.Up)
				.SelectMany(t => t.item.GetIPProperties().UnicastAddresses, (t, ip) => new {t, ip})
				.Where(t => t.ip.Address.AddressFamily == AddressFamily.InterNetwork)
				.Where(t => t.ip.Address.ToString().Contains("192."))
				.Select(t => t.ip))
			{
				Log.ForContext(typeof(AppExtension)).Information($"Your server ip is : {ip.Address}");
			}
		}
	}
}
