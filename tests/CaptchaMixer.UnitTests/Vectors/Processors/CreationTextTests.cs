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

namespace Kaspersky.CaptchaMixer.UnitTests.Vectors.Processors;

public class CreationTextTests
{
	[Test]
	public void VectorProcessors_CreationText_CaptchaChars_Test()
	{
		var layer = new VectorLayer("root");
		new CaptchaChars().Process(
			layer,
			new CaptchaContext(
				new CaptchaParameters("text", 200, 60),
				new[] { layer },
				Array.Empty<IRasterLayer>()));

		// text vectorization uses SkiaSharp's built-in functionality.
		// thus testing the vectors themselves makes no sense.
		layer.Objects.Should().HaveCount(4);
	}

	[Test]
	public void VectorProcessors_CreationText_SkeletonizedCaptchaChars_Test()
	{
		var layer = new VectorLayer("root");
		new SkeletonizedCaptchaChars().Process(
			layer,
			new CaptchaContext(
				new CaptchaParameters("text", 200, 60),
				new[] { layer },
				Array.Empty<IRasterLayer>()));

		// skeletonization is covered by vector path tests, no need to do it again
		layer.Objects.Should().HaveCount(4);
	}
}