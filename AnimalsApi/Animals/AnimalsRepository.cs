using Microsoft.Data.SqlClient;

namespace AnimalsApi.Animals;

public interface IAnimalsRepository
{
    IEnumerable<Animal> GetAnimals(string orderBy);
    void Add(Animal animal);
    void Delete(int idAnimal);
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

    public void Add(Animal animal)
    {
        AssertCorrectness(animal);
        using var con = new SqlConnection(ConString);
        var command = new SqlCommand();
        command.Connection = con;
        command.CommandText = $"insert into Animal (Name, Description, Category, Area) values ('{animal.Name}', '{animal.Description}', '{animal.Category}', '{animal.Area}')";
        con.Open();
        command.ExecuteNonQuery();
    }

    public void Delete(int idAnimal)
    {
        if (idAnimal == 0) throw new ArgumentException("incorrect id");
        using var con = new SqlConnection(ConString);
        var command = new SqlCommand();
        command.Connection = con;
        command.CommandText = $"delete from Animal where IdAnimal = {idAnimal}";
        con.Open();
        command.ExecuteNonQuery();
    }

    private static void AssertCorrectness(Animal? animal)
    {
        if (new[] { animal.Name, animal.Area, animal.Description, animal.Category }.Any(field =>
                String.IsNullOrEmpty(field))) throw new ArgumentException("missing field");
        
    }

    private static void SanitizeOrderBy(string orderBy)
    {
        if (new[] { "name", "description", "category", "area" }
            .Any(field => field.Equals(orderBy, StringComparison.InvariantCultureIgnoreCase))) return;
        throw new ArgumentException($"Incorrect query param orderBy: {orderBy}");
    }
}