using Newtonsoft.Json;
using Safe.Core.Domain;
using Safe.Core.Services;
using Safe.Importer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Safe.Importer
{
    class Program
    {
        private class ConfigurationService : IConfigurationService
        {
            private readonly IConfiguration _configuration;

            public ConfigurationService(IConfiguration configuration)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            }

            public IConfiguration GetConfiguration() => _configuration;

            public void SaveConfiguration(IConfiguration configuration) { }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to the input file:");
            Console.Write("> ");

            var inputFilePath = Console.ReadLine();

            var json = File.ReadAllText(inputFilePath);

            var topics = JsonConvert.DeserializeObject<TopicDto[]>(json);

            var items = ToItems(topics, new string[0]);

            var container = new Container();

            foreach (var item in items)
            {
                container.Items.Add(item);
            }

            Console.WriteLine("Enter the path to the output file:");
            Console.Write("> ");

            var outputFilePath = Console.ReadLine();

            Console.WriteLine("Enter the password:");
            Console.Write("> ");

            var passwordText = Console.ReadLine();

            var password = new Password(passwordText);

            Console.WriteLine("Enter the key generation salt:");
            Console.Write("> ");

            var saltText = Console.ReadLine();

            var salt = Encoding.ASCII.GetBytes(saltText);

            var configuration = new Configuration
            {
                Salt = salt
            };

            var configurationService = new ConfigurationService(configuration);

            var encryptionService = new EncryptionService(configurationService);

            using(var stream = File.OpenWrite(outputFilePath))
            {
                encryptionService.Encrypt(password, container, stream);
            }
        }

        private static IEnumerable<Item> ToItems(TopicDto[] topics, string[] tags)
        {
            if (topics == null) yield break;

            foreach (var topic in topics)
            {
                var nextTags = tags.Concat(new[] { topic.Name }).ToArray();

                if(topic.Fields != null && topic.Fields.Length > 0)
                {
                    var item = new Item
                    {
                        Title = topic.Name,
                        Description = topic.Description,
                    };

                    foreach (var tag in tags)
                    {
                        item.Tags.Add(tag);
                    }

                    foreach (var field in topic.Fields)
                    {
                        item.Fields.Add(ToField(field));
                    }

                    yield return item;
                }

                foreach (var subItem in ToItems(topic.SubTopics, nextTags))
                {
                    yield return subItem;
                }
            }

        }

        private static Field ToField(FieldDto field)
        {
            switch(field.Type)
            {
                case FieldType.SingleLine:
                    return new SingleLineTextField
                    {
                        Label = field.Label,
                        Text = field.Data
                    };
                case FieldType.Multiline:
                    return new MultiLineTextField
                    {
                        Label = field.Label,
                        Text = field.Data
                    };
                case FieldType.Password:
                    return new PasswordField
                    {
                        Label = field.Label,
                        Text = field.Data
                    };
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
