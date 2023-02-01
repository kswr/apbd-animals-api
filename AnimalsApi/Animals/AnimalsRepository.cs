using Microsoft.Data.SqlClient;

namespace AnimalsApi.Animals;

public interface IAnimalsRepository
{
    IEnumerable<Animal> GetAnimals();
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
    public IEnumerable<Animal> GetAnimals()
    {
        var res = new List<Animal>();
        using var con = new SqlConnection(ConString);
        var command = new SqlCommand();
        command.Connection = con;
        command.CommandText = "SELECT * FROM Animal";
        con.Open();
        var dr = command.ExecuteReader();
        while (dr.Read())
        {
            res.Add(new Animal
            {
                IdAnimal = dr.GetInt32(0),
                Name = dr.GetString(1),
                Description = dr.GetString(2),
                Category = dr.GetString(3),
                Area = dr.GetString(4),
            });
        }

        return res;
    }
}