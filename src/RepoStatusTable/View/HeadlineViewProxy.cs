using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RepoStatusTable.Options;

namespace RepoStatusTable.View;

public class HeadlineViewProxy : IHeadlineView
{
	private readonly HeadlineOptions _options;
	private readonly IHeadlineViewStrategy _strategy;

	public HeadlineViewProxy( IOptions<HeadlineOptions> options, IHeadlineViewStrategy strategy )
	{
		_options = options.Value;
		_strategy = strategy;
	}

	public Task RenderAsync()
	{
		return _options.Enable is true ? _strategy.RenderAsync() : Task.CompletedTask;
	}
}