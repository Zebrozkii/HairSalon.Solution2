using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{


    public class Client
    {
        private string _name;
        private DateTime _due_date;
        private int _id;

        public Client(string name, DateTime due_date, int id = 0)
        {
            _name = name;
            _id = id;
            _due_date = _due_date;
        }

        public string GetName()
        {
            return _name;
        }

        public void SetDescription(string newName)
        {
            _name = newName;
        }

        public DateTime GetDueDate()
        {
            return _due_date;
        }

        public int GetId()
        {
            return _id;
        }

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
                DateTime clientDueDate = rdr.GetDateTime(2);
                Client newClient = new Client(clientName, clientDueDate, clientId);
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
            cmd.CommandText = @"DELETE FROM clients WHERE id = @clientId; DELETE FROM stylist_clients WHERE clients_id = @clientId;";
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
            DateTime clientDueDate = Convert.ToDateTime("01/01/2000");
            while (rdr.Read())
            {
                clientId = rdr.GetInt32(0);
                clientName = rdr.GetString(1);
                clientDueDate = rdr.GetDateTime(2);
            }
            Client newClient = new Client(clientName, clientDueDate, clientId);
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
                bool nameEquality = (this.GetName() == newClient.GetDescription());
                bool dueDateEquality = (this.GetDueDate() == newClient.GetDueDate());
                return (idEquality && descriptionEquality && dueDateEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO clients (name, due_date) VALUES (@name, @due_date);";
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@name";
            name.Value = this._name;
            cmd.Parameters.Add(name);
            MySqlParameter dueDate = new MySqlParameter();
            dueDate.ParameterName = "@due_date";
            dueDate.Value = _due_date;//.ToString("yyyy-MM-dd HH:mm:ss.fff");
            cmd.Parameters.Add(dueDate);
            cmd.ExecuteNonQuery();
            _id = (int) cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void Edit(string newName, DateTime newDueDate)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE clients SET name = @newName, due_date = @dueDate WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter name = new MySqlParameter();
            name.ParameterName = "@newName";
            name.Value = newName;
            cmd.Parameters.Add(name);
            MySqlParameter dueDate = new MySqlParameter();
            dueDate.ParameterName = "@dueDate";
            dueDate.Value = newDueDate; //newDueDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            cmd.Parameters.Add(dueDate);
            cmd.ExecuteNonQuery();
            _name = newName;
            _due_date = newDueDate;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Stylist> GetCategories()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT stylist.* FROM clients
                JOIN stylist_clients ON (clients.id = stylist_clients.client_id)
                JOIN stylists ON (stylist_clients.stylist_id = stylist.id)
                WHERE clients.id = @ClientId;";
            MySqlParameter clientIdParameter = new MySqlParameter();
            clientIdParameter.ParameterName = "@ClientId";
            clientIdParameter.Value = _id;
            cmd.Parameters.Add(clientIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Stylist> stylist = new List<Stylist> {};
            while(rdr.Read())
            {
                int thisStylistId = rdr.GetInt32(0);
                string stylistName = rdr.GetString(1);
                Stylist foundStylist = new Stylist(stylistName, thisStylistId);
                stylist.Add(foundStylist);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return stylist;
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
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
