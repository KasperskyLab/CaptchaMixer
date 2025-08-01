// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

class Build : NukeBuild
{
	public static int Main() => Execute<Build>(x => x.Pack);

	[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	[Parameter("Force rebuilding in " + nameof(Test) + " and " + nameof(Pack) + " targets")]
	readonly bool ForceBuild = false;

	[Solution(GenerateProjects = true)]
	readonly Solution Solution;

	AbsolutePath OutputDirectory => RootDirectory / "out";
	AbsolutePath SourceDirectory => RootDirectory / "src";

	Target Clean => _ => _
		.Before(Restore)
		.Executes(() =>
		{
			SourceDirectory.GlobDirectories("*/bin", "*/obj").DeleteDirectories();
			OutputDirectory.CreateOrCleanDirectory();
		});

	Target Restore => _ => _
		.Executes(() =>
		{
			DotNetTasks.DotNetRestore(s => s
				.SetProcessWorkingDirectory(RootDirectory));
		});

	Target Compile => _ => _
		.DependsOn(Restore)
		.Executes(() =>
		{
			DotNetTasks.DotNetBuild(s => s
				.SetConfiguration(Configuration)
				.SetProcessWorkingDirectory(RootDirectory));
		});

	Target Test => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{
			DotNetTasks.DotNetTest(s => s
				.SetConfiguration(Configuration)
				.SetProcessWorkingDirectory(RootDirectory)
				.SetNoBuild(!ForceBuild));
		});

	Target Pack => _ => _
		.DependsOn(Test)
		.Executes(() =>
		{
			var project = Solution.GetProject("CaptchaMixer");

			DotNetTasks.DotNetPack(s => s
				.SetConfiguration(Configuration)
				.SetProject(project.Path)
				.SetOutputDirectory(OutputDirectory)
				.SetProcessWorkingDirectory(RootDirectory)
				.SetNoBuild(!ForceBuild));
		});
}
