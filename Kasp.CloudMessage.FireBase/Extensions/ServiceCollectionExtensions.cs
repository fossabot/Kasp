using System;
using FcmSharp;
using FcmSharp.Http.Client;
using FcmSharp.Settings;
using Kasp.CloudMessage.FireBase.Data;
using Kasp.CloudMessage.FireBase.Models;
using Kasp.CloudMessage.FireBase.Services;
using Kasp.Db.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Kasp.CloudMessage.FireBase.Extensions {
	public static class ServiceCollectionExtensions {
		public static void AddFcm(this KaspDbServiceBuilder builder, string projectName, string filePath) {
			builder.Services.AddSingleton<IFcmClientSettings>(FileBasedFcmClientSettings.CreateFromFile(projectName, filePath));

			AddServices(builder);
		}

		private static void AddServices(KaspDbServiceBuilder builder) {
			var fcmConfig = builder.Configuration.GetSection("Fcm");

			if (fcmConfig == null)
				throw new NullReferenceException("you must define fcm config");

			builder.Services.Configure<FcmConfig>(fcmConfig);

			builder.Services.AddScoped(typeof(IFcmUserTokenRepository), typeof(FcmUserTokenRepository<>).MakeGenericType(builder.DbContextType));

			builder.Services.AddSingleton<IFcmHttpClient, FcmHttpClient>();
			builder.Services.AddSingleton<IFcmClient, FcmClient>();
			builder.Services.AddSingleton<FcmDeviceGroupService>();
			builder.Services.AddSingleton<IFcmService, FcmService>();

			builder.Services.AddHttpClient<FcmApiHttpClient>();
		}
	}
}