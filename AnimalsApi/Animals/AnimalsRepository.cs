using Microsoft.Data.SqlClient;

namespace AnimalsApi.Animals;

public interface IAnimalsRepository
{
    IEnumerable<Animal> GetAnimals(string orderBy);
}

public class Animal
{
    public int IdAnimal { get; init; }
    public string Name { get; init;  }
    public string Description { get; init; }
    public string Category { get; init; }
    public string Area { get; init; }
}

public class AnimalsRepository : IAnimalsRepository
{
    private const string ConString = "Server=localhost\\SQLEXPRESS01;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";
    public IEnumerable<Animal> GetAnimals(string orderBy)
    {
        SanitizeOrderBy(orderBy);
        var res = new List<Animal>();
        using var con = new SqlConnection(ConString);
        var command = new SqlCommand();
        command.Connection = con;
        command.CommandText = $"SELECT * FROM Animal ORDER BY {orderBy}";
        con.Open();
        var dr = command.ExecuteReader();
        while (dr.Read())
        {
            res.Add(new Animal
            {
                IdAnimal = (int) dr["IdAnimal"],
                Name = dr["Name"].ToString()!,
                Description = dr["Description"].ToString()!,
                Category = dr["Category"].ToString()!,
                Area = dr["Area"].ToString()!,
            });
        }

        return res;
    }

    private static void SanitizeOrderBy(string orderBy)
    {
        if (new[] { "name", "description", "category", "area" }
            .Any(field => field.Equals(orderBy, StringComparison.InvariantCultureIgnoreCase))) return;
        throw new ArgumentException($"Incorrect query param orderBy: {orderBy}");
    }
}