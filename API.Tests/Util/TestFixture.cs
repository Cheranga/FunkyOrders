using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace API.Tests.Util
{
    public class TestFixture : IDisposable
    {
        private readonly Process _funcHostProcess;

        public readonly HttpClient Client = new HttpClient();

        public TestFixture()
        {
            var dotnetExePath = Environment.ExpandEnvironmentVariables(ConfigurationHelper.Settings.DotNetExecutablePath);
            var functionHostPath = Environment.ExpandEnvironmentVariables(ConfigurationHelper.Settings.FunctionHostPath);
            var functionAppFolder = Path.GetRelativePath(Directory.GetCurrentDirectory(), ConfigurationHelper.Settings.FunctionApplicationPath);

            _funcHostProcess = new Process
            {
                StartInfo =
                {
                    FileName = dotnetExePath,
                    Arguments = $"\"{functionHostPath}\" start -p {Port}",
                    WorkingDirectory = functionAppFolder
                }
            };
            var success = _funcHostProcess.Start();
            if (!success)
            {
                throw new InvalidOperationException("Could not start Azure Functions host.");
            }

            Client.BaseAddress = new Uri($"http://localhost:{Port}");
        }

        public int Port { get; } = 7071;

        public virtual void Dispose()
        {
            if (!_funcHostProcess.HasExited)
            {
                _funcHostProcess.Kill();
            }

            _funcHostProcess.Dispose();
        }
    }
}