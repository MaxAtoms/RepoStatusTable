using System.Collections.Generic;
using System.Linq;

namespace RepoStatusTable.UnitTests.Utilities;

public class ReposDirectoryUtilityTests
{
	private const string RepoPathA = "/repo/path/a";
	private const string RepoPathB = "/repo/path/b";
	private const string RepoPathC = "/repo/path/c";
	private const string RepoPathD = "/repo/path/d";

	[Test]
	public void GetRepoDirectories_WithEmptyOptions_ShouldReturnEmptyList()
	{
		var builder = new ReposDirectoryUtilityBuilder();
		var result = builder
			.WithReposOrderProvider()
			.Build()
			.GetRepoDirectories();

		Assert.AreEqual( Enumerable.Empty<string>(), result );
		builder.VerifyNoOtherCalls();
	}

	[Test]
	public void GetRepoDirectories_WithValidRepoDirs_ShouldReturnDirectoryList()
	{
		var builder = new ReposDirectoryUtilityBuilder()
			.WithRepoOptionsRepoDir( RepoPathA )
			.WithRepoOptionsRepoDir( RepoPathB )
			.WithRepoOptionsRepoDir( RepoPathC )
			.WithRepoOptionsRepoDir( RepoPathD )
			.WithFileSystemFacadeGetFullPathReturns( RepoPathA )
			.WithFileSystemFacadeGetFullPathReturns( RepoPathB )
			.WithFileSystemFacadeGetFullPathReturns( RepoPathC )
			.WithFileSystemFacadeGetFullPathReturns( RepoPathD )
			.WithFileSystemFacadeDirectoryExists( RepoPathA, true )
			.WithFileSystemFacadeDirectoryExists( RepoPathB, false )
			.WithFileSystemFacadeDirectoryExists( RepoPathC, true )
			.WithFileSystemFacadeDirectoryExists( RepoPathD, true )
			.WithVscFacadeIsValid( RepoPathA, true )
			.WithVscFacadeIsValid( RepoPathC, false )
			.WithVscFacadeIsValid( RepoPathD, true )
			.WithReposOrderProvider( RepoPathA, RepoPathD );

		var uut = builder.Build();

		var result = uut.GetRepoDirectories().ToList();

		var expected = new List<string>
		{
			RepoPathA,
			RepoPathD
		};

		Assert.AreEqual( expected, result );
	}

	[Test]
	public void GetRepoDirectories_WithValidRepoRoots_ShouldReturnContainingDirs()
	{
		var uut = new ReposDirectoryUtilityBuilder()
			.WithRepoOptionsRepoRoot( RepoPathA )
			.WithRepoOptionsRepoRoot( RepoPathB )
			.WithFileSystemFacadeGetDirectories( RepoPathA, new List<string> { "repoAA", "repoAB" } )
			.WithFileSystemFacadeGetDirectories( RepoPathB, new List<string> { "repoBA", "repoBB" } )
			.WithVscFacadeIsValid( "repoAA", true )
			.WithVscFacadeIsValid( "repoAB", false )
			.WithVscFacadeIsValid( "repoBA", false )
			.WithVscFacadeIsValid( "repoBB", true )
			.WithReposOrderProvider( "repoAA", "repoBB" )
			.Build();

		var result = uut.GetRepoDirectories().ToList();

		var expected = new List<string>
		{
			"repoAA",
			"repoBB"
		};

		Assert.AreEqual( expected, result );
	}
}