using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace PlaysFeed.DataAccess;

public class GamesDbContext : DbContext
{
    /// <summary>
    /// Games table
    /// </summary>
    public DbSet<Game> Games { get; set; }

    /// <summary>
    /// Teams table
    /// </summary>
    public DbSet<Team> Teams { get; set; }

    /// <summary>
    /// Sports table
    /// </summary>
    public DbSet<Sport> Sports { get; set; }

    /// <summary>
    /// Competitions table
    /// </summary>
    public DbSet<Competition> Competitions { get; set; }

    public GamesDbContext(DbContextOptions<GamesDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .HasKey(g => g.Id);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.Sport)
            .WithMany(s => s.Games)
            .HasForeignKey(g => g.SportId);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.Competition)
            .WithMany(c => c.Games)
            .HasForeignKey(g => g.CompetitionId);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.HomeTeam)
            .WithMany(t => t.HomeGames)
            .HasForeignKey(g => g.HomeTeamId);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.VisitingTeam)
            .WithMany(t => t.VisitingGames)
            .HasForeignKey(g => g.VisitingTeamId);

        modelBuilder.Entity<Team>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Team>()
            .HasOne(t => t.Sport)
            .WithMany(s => s.Teams)
            .HasForeignKey(t => t.SportId);

        modelBuilder.Entity<Competition>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Competition>()
            .HasOne(c => c.Sport)
            .WithMany(s => s.Competitions)
            .HasForeignKey(c => c.SportId);

        modelBuilder.Entity<Competition>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Sport>()
            .HasKey(s => s.Id);

        modelBuilder.Entity<Sport>()
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Team>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Team>()
            .Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}

/// <summary>
/// Represents a competition
/// </summary>
public class Competition
{
    /// <summary>
    /// Competition Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Competition name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Sport Id
    /// </summary>
    public int SportId { get; set; }

    public virtual Sport Sport { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();
}

/// <summary>
/// Represents a sport
/// </summary>
public class Sport
{
    /// <summary>
    /// Sport Id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sport name
    /// </summary>
    public string Name { get; set; }

    public virtual ICollection<Game> Games { get; set; } = new HashSet<Game>();
    public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    public virtual ICollection<Competition> Competitions { get; set; } = new HashSet<Competition>();
}

public class Team
{
    public int Id { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Sport Id
    /// </summary>
    public int SportId { get; set; }

    public virtual Sport Sport { get; set; }

    public virtual ICollection<Game> HomeGames { get; set; } = new HashSet<Game>();
    public virtual ICollection<Game> VisitingGames { get; set; } = new HashSet<Game>();
}

public class Game
{
    public int Id { get; set; }
    /// <summary>
    /// Sport Id
    /// </summary>
    public int SportId { get; set; }
    public virtual Sport Sport { get; set; }
    public int CompetitionId { get; set; }
    public virtual Competition Competition { get; set; }
    public int HomeTeamId { get; set; }
    public virtual Team HomeTeam { get; set; }
    public int VisitingTeamId { get; set; }
    public virtual Team VisitingTeam { get; set; }
    public DateTime? Date { get; set; }
    public string Identity { get; set; }
}
