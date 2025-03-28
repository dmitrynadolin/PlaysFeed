using PlaysFeed.DataAccess;

public class DataSeeder
{
    private readonly GamesDbContext _db;

    public DataSeeder(GamesDbContext db)
    {
        _db = db;
    }

    public void Seed()
    {
        if (_db.Teams.Any())
        {
            return;
        }

        var sports = new[]
        {
            new Sport { Name = "Football" },
            new Sport { Name = "Basketball" },
            new Sport { Name = "Tennis" },
            new Sport { Name = "Golf" },
            new Sport { Name = "Baseball" }
        };

        _db.Sports.AddRange(sports);

        var competitions = new[]
        {
            new Competition { Name = "LIGAT HA'AL", Sport = sports[0] },
            new Competition { Name = "La Liga", Sport = sports[0] },
            new Competition { Name = "NBA", Sport = sports[1] },
            new Competition { Name = "Wimbledon", Sport = sports[2] },
            new Competition { Name = "The Masters", Sport = sports[3] },
            new Competition { Name = "MLB", Sport = sports[4] }
        };

        _db.Competitions.AddRange(competitions);

        var ligatHaalTeams = new[]
        {
            new Team { Name = "Hapoel Tel Aviv FC", Sport = sports[0] },
            new Team { Name = "Maccabi Tel Aviv FC", Sport = sports[0] },
            new Team { Name = "Hapoel Haifa FC", Sport = sports[0] },
            new Team { Name = "Maccabi Haifa FC", Sport = sports[0] },
            new Team { Name = "Beitar Jerusalem FC", Sport = sports[0] },
            new Team { Name = "Hapoel Beer Sheva FC", Sport = sports[0] },
            new Team { Name = "Bnei Yehuda FC", Sport = sports[0] },
            new Team { Name = "Hapoel Kfar Saba FC", Sport = sports[0] },
            new Team { Name = "Maccabi Petah Tikva FC", Sport = sports[0] },
            new Team { Name = "Hapoel Hadera FC", Sport = sports[0] },
            new Team { Name = "Maccabi Netanya FC", Sport = sports[0] },
            new Team { Name = "Hapoel Tel Aviv FC", Sport = sports[0] },
            new Team { Name = "Maccabi Tel Aviv FC", Sport = sports[0] },
            new Team { Name = "Hapoel Haifa FC", Sport = sports[0] },
            new Team { Name = "Maccabi Haifa FC", Sport = sports[0] },
            new Team { Name = "Beitar Jerusalem FC", Sport = sports[0] },
            new Team { Name = "Hapoel Beer Sheva FC", Sport = sports[0] },
            new Team { Name = "Bnei Yehuda FC", Sport = sports[0] },
            new Team { Name = "Hapoel Kfar Saba FC", Sport = sports[0] },
            new Team { Name = "Maccabi Petah Tikva FC", Sport = sports[0] },
            new Team { Name = "Hapoel Hadera FC", Sport = sports[0] },
            new Team { Name = "Maccabi Netanya FC", Sport = sports[0] }
        };

        _db.Teams.AddRange(ligatHaalTeams);

        _db.SaveChanges();
    }
}