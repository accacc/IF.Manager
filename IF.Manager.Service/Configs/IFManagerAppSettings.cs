﻿
using IF.Configuration;
using IF.Core.Configuration;
using IF.Core.Database;
using IF.Core.Jwt;
using IF.Core.MongoDb;
using IF.Core.RabbitMQ;
using IF.Core.Sms;

namespace IF.Manager.Service
{


    public interface IIFManagerAppSettings : IAppSettingsCore
    {
        RabbitMQConnectionSettings RabbitMQConnection { get; set; }
        DatabaseSettings Database { get; set; }

        IFSmsSettings IFSms { get; set; }

        MongoConnectionSettings MongoConnection { get; set; }

        JwtSettings Jwt { get; set; }
    }

    public class IFManagerAppSettings : AppSettingsCore, IIFManagerAppSettings
    {
        public RabbitMQConnectionSettings RabbitMQConnection { get; set; }

        public IFSmsSettings IFSms { get; set; }

        public DatabaseSettings Database { get; set; }

        public MongoConnectionSettings MongoConnection { get; set; }

        public JwtSettings Jwt { get; set; }
    }
}
