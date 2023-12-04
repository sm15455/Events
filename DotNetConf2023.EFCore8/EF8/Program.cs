using EF8;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

Seed();
Json();
Hierarchy();
ReparentHierarchy();
RawSqlQueries();
In();

void In()
{
    using var ctx = new EF8Context();
    var ids = new List<int>([1, 2]);
    var p = ctx.People.Where(c => ids.Contains(c.Id)).Count();
    Console.WriteLine(p);
}

void RawSqlQueries()
{
    using var ctx = new EF8Context();
    var people = ctx.Database.SqlQueryRaw<RawPerson>("select Id, Name from people").Where(c => c.Name.StartsWith("S"));
    foreach(var p in people)
    {
        Console.WriteLine(p.Name);
    }
}

void ReparentHierarchy()
{
    using var ctx = new EF8Context();
    var allDescendent = ctx.Halflings.AsNoTracking().Where(
                descendent => descendent.PathFromPatriarch.IsDescendantOf(
                    ctx.Halflings
                        .Single(
                            ancestor =>
                                ancestor.Name == "Ponto"
                                && descendent.Id != ancestor.Id)
                        .PathFromPatriarch))
            .OrderBy(descendent => descendent.PathFromPatriarch.GetLevel());
    Console.WriteLine($"Tutti i discendenti prima: {String.Join(",", allDescendent.Select(c => c.Name))}");

    var mungo = ctx.Halflings.First(c => c.Name == "Mungo");
    var ponto = ctx.Halflings.First(c => c.Name == "Ponto");

    var longoAndDescendents = ctx.Halflings.Where(
            descendent => descendent.PathFromPatriarch.IsDescendantOf(
                ctx.Halflings.Single(ancestor => ancestor.Name == "Longo").PathFromPatriarch))
        .ToList();

    foreach (var descendent in longoAndDescendents)
    {
        descendent.PathFromPatriarch
            = descendent.PathFromPatriarch.GetReparentedValue(
                mungo.PathFromPatriarch, ponto.PathFromPatriarch)!;
    }

    ctx.SaveChanges();
    ctx.ChangeTracker.Clear();

    allDescendent = ctx.Halflings.AsNoTracking().Where(
            descendent => descendent.PathFromPatriarch.IsDescendantOf(
                ctx.Halflings
                    .Single(
                        ancestor =>
                            ancestor.Name == "Ponto"
                            && descendent.Id != ancestor.Id)
                    .PathFromPatriarch))
        .OrderBy(descendent => descendent.PathFromPatriarch.GetLevel());
    Console.WriteLine($"Tutti i discendenti  dopo: {String.Join(",", allDescendent.Select(c => c.Name))}");

}

void Hierarchy()
{
    using var ctx = new EF8Context();
    var secondgeneration = ctx.Halflings.Where(halfling => halfling.PathFromPatriarch.GetLevel() == 2).Count();
    Console.WriteLine($"seconda generazione: {secondgeneration}");


    var ancestor = ctx.Halflings
            .SingleOrDefault(
                ancestor => ancestor.PathFromPatriarch == ctx.Halflings
                    .Single(descendent => descendent.Name == "Otho").PathFromPatriarch
                    .GetAncestor(1));

    Console.WriteLine($"seconda generazione: {ancestor.Name}");

    var ancestors = ctx.Halflings.Where(
            ancestor => ctx.Halflings
                .Single(
                    descendent =>
                        descendent.Name == "Otho"
                        && ancestor.Id != descendent.Id)
                .PathFromPatriarch.IsDescendantOf(ancestor.PathFromPatriarch))
        .OrderByDescending(ancestor => ancestor.PathFromPatriarch.GetLevel());

    Console.WriteLine($"avi: {String.Join(",", ancestors.Select(c => c.Name))}");


    var descendent = ctx.Halflings.Where(
            descendent => descendent.PathFromPatriarch.GetAncestor(1) == ctx.Halflings
            .Single(ancestor => ancestor.Name == "Polo").PathFromPatriarch);
    Console.WriteLine($"Discendenti: {String.Join(",", descendent.Select(c => c.Name))}");


    var allDescendent = ctx.Halflings.Where(
                descendent => descendent.PathFromPatriarch.IsDescendantOf(
                    ctx.Halflings
                        .Single(
                            ancestor =>
                                ancestor.Name == "Polo"
                                && descendent.Id != ancestor.Id)
                        .PathFromPatriarch))
            .OrderBy(descendent => descendent.PathFromPatriarch.GetLevel());
    Console.WriteLine($"Tutti i discendenti: {String.Join(",", allDescendent.Select(c => c.Name))}");
}

static void Json()
{
    using var ctx = new EF8Context();
    var count = ctx.People.Count(c => c.Contacts.Addresses.Count == 1);
    var milanesi = ctx.People.Where(c => c.Contacts.Addresses.Any(c => c.City == "Milano")).Count();
    var melfitani = ctx.People.Where(c => c.Contacts.Addresses[1].City == "Melfi").Count();
    var developers = ctx.People.Where(c => c.Positions.Contains("Developer")).Count();

    Console.WriteLine($"un solo indirizzo: {count}");
    Console.WriteLine($"milanesi: {milanesi}");
    Console.WriteLine($"melfitani: {melfitani}");
    Console.WriteLine($"developers: {developers}");

}

void Seed()
{
    var ctx = new EF8Context();
    ctx.Database.EnsureDeleted();
    ctx.Database.EnsureCreated();
    var p1 = new Person
    {
        Name = $"Stefano Mostarda",
        BirthDate = new DateOnly(1979, 1, 1),
        Positions = ["Developer", "Team Leader"],
        Contacts = new Contact
        {
            Addresses = [new() { Street = "Via del corso", City = "Roma", State = "IT", ZipCode = "00100" }],
            Phones = ["123123123", "987987987"],
            Emails = ["stefano@aspitalia.com", "jedi@qualcosa.it"]
        }
    };

    var p2 = new Person
    {
        Name = $"Daniele bochicchio",
        BirthDate = new DateOnly(1980, 1, 1),
        Positions = ["CEO", "CTO"],
        Contacts = new Contact
        {
            Addresses = [
                new Address { Street = "Via Torino", City = "Milano", State = "IT", ZipCode = "11111" },
                new Address { Street = "Via dei gerani", City = "Melfi", State = "IT", ZipCode = "00000" },
            ],
            Phones = ["456567876", "745863475"],
            Emails = ["daniele@aspitalia.com", "daniele@altro.com"]
        }
    };

    var p3 = new Person
    {
        Name = $"Cristian civera",
        BirthDate = new DateOnly(1990, 1, 1),
        Positions = ["Software Architect", "Developer"],
        Contacts = new Contact
        {
            Addresses = [
                new Address { Street = "Via bomba 3", City = "Brescia", State = "IT", ZipCode = "121212" },
            ],
            Phones = ["32548568356", "834687534"],
            Emails = ["cristian@aspitalia.com", "cristian@boh.net"]
        }
    };

    ctx.AddRange(new[] { p1, p2, p3 });

    ctx.AddRange(
        new Halfling(HierarchyId.Parse("/"), "Balbo", 1167),
        new Halfling(HierarchyId.Parse("/1/"), "Mungo", 1207),
        new Halfling(HierarchyId.Parse("/2/"), "Pansy", 1212),
        new Halfling(HierarchyId.Parse("/3/"), "Ponto", 1216),
        new Halfling(HierarchyId.Parse("/4/"), "Largo", 1220),
        new Halfling(HierarchyId.Parse("/5/"), "Lily", 1222),
        new Halfling(HierarchyId.Parse("/1/1/"), "Bungo", 1246),
        new Halfling(HierarchyId.Parse("/1/2/"), "Belba", 1256),
        new Halfling(HierarchyId.Parse("/1/3/"), "Longo", 1260),
        new Halfling(HierarchyId.Parse("/1/4/"), "Linda", 1262),
        new Halfling(HierarchyId.Parse("/1/5/"), "Bingo", 1264),
        new Halfling(HierarchyId.Parse("/3/1/"), "Rosa", 1256),
        new Halfling(HierarchyId.Parse("/3/2/"), "Polo"),
        new Halfling(HierarchyId.Parse("/4/1/"), "Fosco", 1264),
        new Halfling(HierarchyId.Parse("/1/1/1/"), "Bilbo", 1290),
        new Halfling(HierarchyId.Parse("/1/3/1/"), "Otho", 1310),
        new Halfling(HierarchyId.Parse("/1/5/1/"), "Falco", 1303),
        new Halfling(HierarchyId.Parse("/3/2/1/"), "Posco", 1302),
        new Halfling(HierarchyId.Parse("/3/2/2/"), "Prisca", 1306),
        new Halfling(HierarchyId.Parse("/4/1/1/"), "Dora", 1302),
        new Halfling(HierarchyId.Parse("/4/1/2/"), "Drogo", 1308),
        new Halfling(HierarchyId.Parse("/4/1/3/"), "Dudo", 1311),
        new Halfling(HierarchyId.Parse("/1/3/1/1/"), "Lotho", 1310),
        new Halfling(HierarchyId.Parse("/1/5/1/1/"), "Poppy", 1344),
        new Halfling(HierarchyId.Parse("/3/2/1/1/"), "Ponto", 1346),
        new Halfling(HierarchyId.Parse("/3/2/1/2/"), "Porto", 1348),
        new Halfling(HierarchyId.Parse("/3/2/1/3/"), "Peony", 1350),
        new Halfling(HierarchyId.Parse("/4/1/2/1/"), "Frodo", 1368),
        new Halfling(HierarchyId.Parse("/4/1/3/1/"), "Daisy", 1350),
        new Halfling(HierarchyId.Parse("/3/2/1/1/1/"), "Angelica", 1381));

    ctx.SaveChanges();

}


class RawPerson(int idRawPerson, string Name)
{

    [Column("Id")]
    public int IdRawPerson { get; set; } = idRawPerson;
    public string Name { get; set; } = Name;
}