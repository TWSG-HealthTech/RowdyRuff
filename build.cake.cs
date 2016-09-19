#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=Machine.Specifications.Runner.Console"
#tool "nuget:?package=secure-file"

using System.Diagnostics;
using IO = System.IO;
using System.Linq;

var target = Argument("target", "Default");
var buildConfiguration = "Release";
var platformTarget = PlatformTarget.MSIL;   //AnyCPU

Task("Build")
  .Does(() =>
{
  NuGetRestore("RowdyRuff.sln");
  MSBuild("RowdyRuff.sln", new MSBuildSettings {
    Configuration = buildConfiguration,
    PlatformTarget = platformTarget
  });
});

Task("Test")
  .Does(() =>
{
  var binFolders = IO.Directory.GetDirectories(IO.Directory.GetCurrentDirectory(), "*.Tests").SelectMany(d => IO.Directory.GetDirectories(d, "bin"));
  var testDlls = binFolders.SelectMany(f => IO.Directory.GetFiles(IO.Path.Combine(f, buildConfiguration), "*.Tests.dll", SearchOption.AllDirectories)).Select(p => "\"" + p + "\"");

  var startInfo = new ProcessStartInfo(IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "tools/Machine.Specifications.Runner.Console/tools/mspec-clr4.exe"))
  {
    UseShellExecute = false,
    RedirectStandardOutput = true,
    CreateNoWindow = true,
    Arguments = "--xml=\"./TestResult.xml\" " + string.Join(" ", testDlls)
  };

  var mspecProcess = Process.Start(startInfo);
  while (!mspecProcess.StandardOutput.EndOfStream) {
      string line = mspecProcess.StandardOutput.ReadLine();
      Console.WriteLine(line);
  }
  if (mspecProcess.ExitCode != 0)
  {
    throw new CakeException(string.Format("mSpec test failure... exit code: {0}", mspecProcess.ExitCode));
}
    });


    Task("Encrypt")
    .Does(() => {
        var startInfo = new ProcessStartInfo(IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "tools/secure-file/tools/secure-file"))
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            Arguments = "-encrypt ./RowdyRuff/appsettings.json -secret " + Argument<string>("secret", null)
        };

var proc = Process.Start(startInfo);
        while (!proc.StandardOutput.EndOfStream) {
            string line = proc.StandardOutput.ReadLine();
Console.WriteLine(line);
        }
        if (proc.ExitCode != 0)
        {
            throw new CakeException(string.Format("secure-file encryption failed... exit code: {0}", proc.ExitCode));
        }
    });

    Task("Decrypt")
    .Does(() => {
        var startInfo = new ProcessStartInfo(IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "tools/secure-file/tools/secure-file"))
        {
            UseShellExecute = false,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            Arguments = "-decrypt ./RowdyRuff/appsettings.json.enc -secret " + Argument<string>("secret", null)
        };

var proc = Process.Start(startInfo);
        while (!proc.StandardOutput.EndOfStream) {
            string line = proc.StandardOutput.ReadLine();
Console.WriteLine(line);
        }
        if (proc.ExitCode != 0)
        {
            throw new CakeException(string.Format("secure-file decryption failed... exit code: {0}", proc.ExitCode));
        }
    });

    Task("Default")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

    RunTarget(target);
