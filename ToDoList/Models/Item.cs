using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{

this is the page your on start here category == stylist, and items = clients remember




    public class Client
    {
        private string _name;
        private DateTime _due_date;
        private int _id;

        public Item(string name, DateTime due_date, int id = 0)
        {
            _name = name;
            _id = id;
            _due_date = due_date;
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

        public static List<Clients> GetAll()
        {
            List<Clients> allClients = new List<Clients> { };
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
                bool descriptionEquality = (this.GetDescription() == newClient.GetDescription());
                bool dueDateEquality = (this.GetDueDate() == newClient.GetDueDate());
                return (idEquality && descriptionEquality && dueDateEquality);
            }
        }

        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO items (description, due_date) VALUES (@description, @due_date);";
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@description";
            description.Value = this._description;
            cmd.Parameters.Add(description);
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

        public void Edit(string newDescription, DateTime newDueDate)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"UPDATE items SET description = @newDescription, due_date = @dueDate WHERE id = @searchId;";
            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = _id;
            cmd.Parameters.Add(searchId);
            MySqlParameter description = new MySqlParameter();
            description.ParameterName = "@newDescription";
            description.Value = newDescription;
            cmd.Parameters.Add(description);
            MySqlParameter dueDate = new MySqlParameter();
            dueDate.ParameterName = "@dueDate";
            dueDate.Value = newDueDate; //newDueDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            cmd.Parameters.Add(dueDate);
            cmd.ExecuteNonQuery();
            _description = newDescription;
            _due_date = newDueDate;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public List<Category> GetCategories()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT categories.* FROM items
                JOIN categories_items ON (items.id = categories_items.item_id)
                JOIN categories ON (categories_items.category_id = categories.id)
                WHERE items.id = @ItemId;";
            MySqlParameter itemIdParameter = new MySqlParameter();
            itemIdParameter.ParameterName = "@ItemId";
            itemIdParameter.Value = _id;
            cmd.Parameters.Add(itemIdParameter);
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Category> categories = new List<Category> {};
            while(rdr.Read())
            {
                int thisCategoryId = rdr.GetInt32(0);
                string categoryName = rdr.GetString(1);
                Category foundCategory = new Category(categoryName, thisCategoryId);
                categories.Add(foundCategory);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return categories;
        }

        public void AddCategory(Category newCategory)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";
            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@CategoryId";
            category_id.Value = newCategory.GetId();
            cmd.Parameters.Add(category_id);
            MySqlParameter item_id = new MySqlParameter();
            item_id.ParameterName = "@ItemId";
            item_id.Value = _id;
            cmd.Parameters.Add(item_id);
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }
    }
}
