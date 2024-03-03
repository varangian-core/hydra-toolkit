using System.Security.AccessControl;
using Microsoft.Extensions.Configuration;

namespace Hydra
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }

        static void Main(string[] args)
        {
            var projectDir = AppContext.BaseDirectory;
            
            //WARNING: These are the path locations for the python scripts. Please adjust, if paths are different.
            var commandQueryScriptPath = Path.Combine(projectDir, "python", "command_query.py");
            var launchCommandScriptPath = Path.Combine(projectDir, "python", "launch_command.py");
            
            var resultsDir = Path.Combine(projectDir, "QodanaResults");

            var inMemorySettings = new Dictionary<string, string>
            {
                { "PythonScriptPaths:CommandQueryScriptPath", commandQueryScriptPath },
                { "PythonScriptPaths:LaunchCommandScriptPath", launchCommandScriptPath },
                { "Qodana:ProjectDir", projectDir },
                { "Qodana:ResultsDir", resultsDir },
            };
            
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddInMemoryCollection(inMemorySettings)
                    .Build();

            Console.WriteLine("Hydra Command Line Tool");
            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                CommandHandler.HandleCommand(input, Configuration);

            }

        }
    }
}

