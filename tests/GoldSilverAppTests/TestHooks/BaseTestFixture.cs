using Microsoft.Playwright;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GoldSilverApp.Automation.Core;

namespace GoldSilverApp.Automation.Tests.TestHooks
{
    public class BaseTestFixture
    {
        protected IBrowserContext Context { get; private set; } = null!;
        protected IPage Page { get; private set; } = null!;

        public TestContext TestContext { get; set; } = null!;

        [TestInitialize]
        public async Task TestSetup()
        {
            Context = await BrowserFactory.CreateContextAsync(
                GlobalTestFixture.Browser!,
                storageStatePath: "auth/storageState.json"
            );

            await Context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });

            Page = await Context.NewPageAsync();
        }

        [TestCleanup]
        public async Task TestTeardown()
        {
            var failed = TestContext.CurrentTestOutcome != UnitTestOutcome.Passed;

            if (failed)
            {
                var testMethod = GetType().GetMethod(TestContext.TestName!);
                var trace = testMethod?.GetCustomAttribute<RtmTraceAttribute>();
                var rtmId = trace?.TcId ?? TestContext.TestName;

                var tracePath = $"reports/traces/{rtmId}.zip";
                await Context.Tracing.StopAsync(new TracingStopOptions { Path = tracePath });
            }
            else
            {
                await Context.Tracing.StopAsync();
            }

            await Context.CloseAsync();
        }
    }
}