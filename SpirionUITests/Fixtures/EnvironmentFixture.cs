using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SpirionUITests.Models;
using Newtonsoft.Json;
using Spirion.Automation.Framework;
using Environment = SpirionUITests.Models.Environment;

namespace SpirionUITests.Fixtures
{

    public class EnvironmentFixture
    {
        public EnvironmentFixture()
        {
            Environment = GetEnvironment();
        }

        public Environment Environment { get; set; }


        private Environment GetEnvironment()
        {
            string json = File.ReadAllText("EnvironmentData.json");
            var readJson = JsonConvert.DeserializeObject<EnvironmentModel>(json);

            var environment = readJson.Environments.FirstOrDefault();
            if (environment == null)
            {
                var message = $"Could not find configuration for Environment:." +
                              " \nPlease check the EnvironmentData.Json file in the solution to select a valid environment." +
                              " \nPlease set a valid environment in the environment data json.";
                Logger.LogError(message);
            }

            
            return environment;
        }
    }

}