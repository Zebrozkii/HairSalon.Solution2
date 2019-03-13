using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace HairSalon.Models
{


    public class Client
    {
        private string _name;
        private int _id;
        // private int _stylistId;

        public Client(string name, int id = 0)
        {
            _name = name;
            _id = id;
            // _stylistId = stylistId;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetDescription(string newName)
        {
            _name = newName;
        }

        public int GetId()
        {
            return _id;
        }
        // public int GetStylistId()
        // {
        //   return _stylistId;
        // }

        public static List<Client> GetAll()
        {
            List<Client> allClients = new List<Client> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM clients;";
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int clientId = rdr.GetInt32(0);
                string clientName = rdr.GetString(1);
                // int stylistId = rdr.GetInt32(2);
                Client newClient = new Client(clientName,clientId);
                allClients.Add(newClient);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allClients;
        }

        public static void ClearAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM clients;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM clients WHERE id = @clientId;";
            MySqlParameter clientIdParameter = new MySqlParameter();
            clientIdParameter.ParameterName = "@clientId";
            clientIdParameter.Value = this.GetId();
            cmd.Parameters.Add(clientIdParameter);
            cmd.ExecuteNonQuery();
            if (conn != null)
            {
                conn.Close();
            }
        }

        public static Client Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM clients WHERE id = (@searchId);";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int clientId = 0;
            string clientName = "";
            // int stylistId = 0;
            while (rdr.Read())
            {
                clientId = rdr.GetInt32(0);
                clientName = rdr.GetString(1);
                // stylistId = rdr.GetInt32(2);

            }
            Client newClient = new Client(clientName,clientId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return newClient;
        }

        public override bool Equals(System.Object otherClient)
        {
            if (!(otherClient is Client))
            {
                return false;
            }
            else
            {
                Client newClient = (Client)otherClient;
                bool idEquality = (this.GetId() == newClient.GetId());
                bool nameEquality = (this.GetName() == newClient.GetName());
                // bool StylistEquality = this.GetStylistId() == newClient.GetStylistId();
                return (idEquality && nameEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO clients (name) VALUES (@name);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            // // stylistId.ParameterName = "@GetStylistId";
            // // // stylistId.Value = this._stylistId;
            // // cmd.Parameters.Add(stylistId);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
        public List <Stylist> GetStylist()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT stylist.* FROM clients
          JOIN stylist_clients ON (clients.id = stylist_clients.clients_id)
          JOIN stylist ON (stylist_clients.stylist_id = stylist.id)
          WHERE client.id = @ClientId;";

          MySqlParameter clientId = new MySqlParameter();
          clientId.ParameterName = "@ClientId";
          cmd.Parameters.Add(clientId);
          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          List<Stylist> stylists = new List<Stylist> {};
          while(rdr.Read())
          {
            int stylistId = rdr.GetInt32(0);
            string stylistName = rdr.GetString(1);
            Stylist foundStylist = new Stylist(stylistName, stylistId);
            stylists.Add(foundStylist);
          }
          conn.Close();
          if(conn!=null)
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
          cmd.CommandText = @"INSERT INTO stylists_clients (stylist_id, client_id) VALUES (@StylistId, @ClientId);";
          MySqlParameter stylist_id = new MySqlParameter();
          stylist_id.ParameterName = "@StylistId";
          stylist_id.Value = newStylist.GetId();
          cmd.Parameters.Add(stylist_id);
          MySqlParameter client_id = new MySqlParameter();
          client_id.ParameterName = "@ClientId";
          client_id.Value = _id;
          cmd.Parameters.Add(client_id);
          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn!=null) {
            conn.Dispose();

          }
        }
        public void Edit(string newName)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE clients SET name = @newName WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newName";
            name.Value = newName;
            cmd.Parameters.Add(name);
            cmd.ExecuteNonQuery();
            _name = newName;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
      }
    }
