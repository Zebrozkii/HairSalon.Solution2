using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Specialty
  {
    private int _id;
    private string _specialty;

    public Specialty(string specialty, int id = 0)
    {
      _id = id;
      _specialty = specialty;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetSpecialty()
    {
      return _specialty;
    }
    public static List<Specialty> GetAll()
    {
        List<Specialty> allSpecialties = new List<Specialty> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM specialty;";
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while (rdr.Read())
        {
            int specialtyId = rdr.GetInt32(0);
            string specialtyName = rdr.GetString(1);
            Specialty newSpecialty = new Specialty(specialtyName, specialtyId);
            allSpecialties.Add(newSpecialty);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allSpecialties;
    }

    public static void ClearAll()
    {
      List<Specialty> allSpecialties = new List<Specialty>{};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialty;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int specialtyId = rdr.GetInt32(0);
        string specialtyName = rdr.GetString(1);
        Specialty newSpecialty = new Specialty(specialtyName,specialtyId);
        allSpecialties.Add(newSpecialty);
      }
      conn.Close();
      if (conn != null)
      {
      conn.Dispose();
      }
    }
    public void Edit(string newSpecialty)
     {
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"UPDATE specialty SET name = @newName WHERE id = @searchId;";
       MySqlParameter searchId = new MySqlParameter();
       searchId.ParameterName = "@searchId";
       searchId.Value = _id;
       cmd.Parameters.Add(searchId);
       MySqlParameter specialty = new MySqlParameter();
       specialty.ParameterName = "@newName";
       specialty.Value = newSpecialty;
       cmd.Parameters.Add(specialty);
       cmd.ExecuteNonQuery();
       _specialty = newSpecialty;
       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }
     }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO specialty (specialty) values (@specialty);";
      MySqlParameter specialty = new MySqlParameter();
      specialty.ParameterName = "@specialty";
      specialty.Value = this._specialty;
      cmd.Parameters.Add(specialty);
      cmd.ExecuteNonQuery();
      _id = (int)cmd.LastInsertedId;
      conn.Close();
      if (conn!=null)
      {
      conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherSpecialty)
    {
      if(!(otherSpecialty is Specialty))
      {
        return false;
      }
      else
      {
        Specialty newSpecialty = (Specialty)otherSpecialty;
        bool idEquality = this.GetId().Equals(newSpecialty.GetId());
        bool nameEquality = this.GetSpecialty().Equals(newSpecialty.GetSpecialty());
        return (idEquality && nameEquality);
      }
    }
    public override int GetHashCode()
    {
        return this.GetId().GetHashCode();
    }
    public static Specialty Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialty WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int SpecialtyId = 0;
      string SpecialtyName = "";
      while (rdr.Read())
      {
        SpecialtyId = rdr.GetInt32(0);
        SpecialtyName = rdr.GetString(1);
      }
      Specialty newSpecialty = new Specialty(SpecialtyName, SpecialtyId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newSpecialty;
    }
    public void Delete(int id)
     {
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"DELETE FROM specialty WHERE id = @thisId;DELETE FROM stylists_specialties WHERE specialty_id = @thisId;";
       MySqlParameter thisId = new MySqlParameter();
       thisId.ParameterName = "@thisId";
       thisId.Value = id;
       cmd.Parameters.Add(thisId);
       cmd.ExecuteNonQuery();
       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }
     }
    public List<Stylist> GetStylists()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT stylist.* FROM specialty
            JOIN stylist_specialty ON (specialty.id = stylist_specialty.specialty_id)
            JOIN stylist ON (stylist_specialty.stylist_id = stylist.id)
            WHERE specialty.id = @stylistId;";
        MySqlParameter stylistId = new MySqlParameter();
        stylistId.ParameterName = "@stylistId";
        stylistId.Value = _id;
        cmd.Parameters.Add(stylistId);
        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        List<Stylist> stylists = new List<Stylist> {};
        while (rdr.Read())
        {
            int theStylistId = rdr.GetInt32(0);
            string stylistName = rdr.GetString(1);
            Stylist foundStylist = new Stylist(stylistName, theStylistId);
            stylists.Add(foundStylist);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return stylists;
      }
      public void AddStylist(Stylist newStylist)
      {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"INSERT INTO stylist_specialty (stylist_id, specialty_id) VALUES (@StylistId, @SpecialtyId);";
          MySqlParameter stylist_id = new MySqlParameter();
          stylist_id.ParameterName = "@StylistId";
          stylist_id.Value = newStylist.GetId();
          cmd.Parameters.Add(stylist_id);
          MySqlParameter specialty_id = new MySqlParameter();
          specialty_id.ParameterName = "@SpecialtyId";
          specialty_id.Value = _id;
          cmd.Parameters.Add(specialty_id);
          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
              conn.Dispose();
          }
      }

  }
}
