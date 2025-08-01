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

using Microsoft.Extensions.Caching.Memory;

namespace Kaspersky.CaptchaMixer;

/// <summary>
/// <see cref="IMemoryCache"/> wrapper for simple operations.
/// </summary>
public class MemoryCacheWrapper<TKey, TItem>
{
	private double _expiration;
	private readonly MemoryCacheOptions _options;
	private readonly Lazy<MemoryCache> _cacheLazy;

	internal MemoryCacheWrapper(
		double expiration,
		int sizeLimit,
		double scanFrequency)
	{
		_expiration = expiration;

		_options = new()
		{
			SizeLimit = sizeLimit,
			ExpirationScanFrequency = TimeSpan.FromSeconds(scanFrequency)
		};

		_cacheLazy = new(() => new(_options));
	}

	public void Configure(
		double expiration,
		int sizeLimit,
		double scanFrequency)
	{
		if (_cacheLazy.IsValueCreated)
			throw new InvalidOperationException("Cache cannot be configured after first usage");

		_expiration = expiration;
		_options.SizeLimit = sizeLimit;
		_options.ExpirationScanFrequency = TimeSpan.FromSeconds(scanFrequency);
	}

	internal TItem GetOrCreate(TKey key, Func<TKey, TItem> factory)
		=> _cacheLazy.Value.GetOrCreate(key, entry =>
		{
			entry.Size = 1;
			entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_expiration);
			return factory(key);
		});

	internal Task<TItem> GetOrCreateAsync(TKey key, Func<TKey, Task<TItem>> factory)
		=> _cacheLazy.Value.GetOrCreateAsync(key, entry =>
		{
			entry.Size = 1;
			entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_expiration);
			return factory(key);
		});
}
