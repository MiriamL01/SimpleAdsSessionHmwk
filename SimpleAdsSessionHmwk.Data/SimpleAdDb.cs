using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SimpleAdsSessionHmwk.Data
{
    public class SimpleAdDb
    {
        private readonly string _connectionString;

        public SimpleAdDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddSimpleAd(SimpleAd ad)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Ads (Name, PhoneNumber, Text, Date) " +
                "VALUES (@name, @number, @text, GETDATE()) " +
                "SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@name", ad.Name);
            cmd.Parameters.AddWithValue("@number", ad.PhoneNumber);
            cmd.Parameters.AddWithValue("@text", ad.Text);
            connection.Open();
            ad.Id = (int)(decimal)cmd.ExecuteScalar();
        }

        public List<SimpleAd> GetAllSimpleAds()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ads ORDER BY Date DESC";
            connection.Open();
            List<SimpleAd> ads = new();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ads.Add(CastAdFromReader(reader));
            }
            return ads;
        }

        public SimpleAd GetAdById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Ads WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
            {
                return null;
            }
            return CastAdFromReader(reader);
        }

        public SimpleAd CastAdFromReader(SqlDataReader reader)
        {
            return new SimpleAd
            {
                Id = reader.GetOrNull<int>("Id"),
                Name = reader.GetOrNull<string>("Name"),
                PhoneNumber = reader.GetOrNull<string>("PhoneNumber"),
                Text = reader.GetOrNull<string>("Text"),
                Date = reader.GetOrNull<DateTime>("Date")
            };
        }

        public void DeleteAd(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Ads WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            connection.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
