using HelperSharp;
using SpecFlowHelper.Configuration;
using SpecFlowHelper.Integrations;
using SpecFlowHelper.Integrations.Browsers;
using SpecFlowHelper.Logging;
using SampleProject.Specs.Integrations.Environments;
using SpecFlowHelper.Steps;
using SpecFlowHelper.Steps.Strategies;
using SpecFlowHelper.Steps.Strategies.jQuery;
using TechTalk.SpecFlow;
using TestSharp;

namespace SampleProject.Specs.Integrations
{
    [Binding]
    public class SpecFlowHeperConfig
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            AppConfig.Environment = new SampleContinuousIntegrationEnvironment();
            AppConfig.BrowserKind = BrowserKind.Chrome;
            AppConfig.BrowserDriverFolder = VSProjectHelper.GetProjectFolderPath("SampleProject.Specs") + @"\Drivers";
            AppConfig.Configuration = "Debug";

            AppConfig.InitializeDBEnabled = false;

            AppConfig.WebApiEnabled = true;
            AppConfig.WebApiProjectFolderName = "SampleProject.WebApi";
            AppConfig.WebApiPort = 8001;

            AppConfig.WebAppProjectFolderName = "SampleProject.WebApp";
            AppConfig.WebAppPort = 8002;
            AppConfig.WebAppBaseUrl = "http://localhost:8002";

            AppConfig.JobsEnabled = true;
            AppConfig.JobsLogFileName = "SampleProject.BackgroundWorkerApp-log.txt";
            AppConfig.JobsProcessName = "SampleProject.BackgroundWorkerApp";
            AppConfig.JobsProjectFolderName = "SampleProject.BackgroundWorkerApp";
      
            StrategyFactory.Register<IComboboxStepsStrategy, ComboboxSteps>(new jQueryComboboxStepsStrategy());
        }
    }
}
