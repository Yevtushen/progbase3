using Microsoft.Data.Sqlite;

namespace Progbase3
{
	public class ProductsRepository
	{
		private SqliteConnection connection;

		public ProductsRepository(SqliteConnection connection)
		{
			this.connection = connection;
		}

        static Product GetProduct(SqliteDataReader reader)
        {
            Product p = new Product()
            {
                id = int.Parse(reader.GetString(0)),
                name = reader.GetString(1),
                price = double.Parse(reader.GetString(2)),
                left = int.Parse(reader.GetString(3)),
                description = reader.GetString(4)
            };

            return p;
        }

        public Product GetById(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                Product p = GetProduct(reader);
                reader.Close();
                connection.Close();
                return p;
            }
            else
            {
                connection.Close();
                return null;
            }
        }

        public long Insert(Product p)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
    INSERT INTO orders (id, name, price, left, description) 
    VALUES ($id, $name, $price, $left, $description);
 
    SELECT last_insert_rowid();
";
            command.Parameters.AddWithValue("$id", p.id);
            command.Parameters.AddWithValue("$name", p.name);
            command.Parameters.AddWithValue("$price", p.price);
            command.Parameters.AddWithValue("$left", p.left);
            command.Parameters.AddWithValue("$description", p.description);

            long newId = (long)command.ExecuteScalar();
            /*if (newId == 0)
            {
                Console.WriteLine("Internet provider not added.");
            }
            else
            {
                Console.WriteLine("Internet provider added. New id is: " + newId);
            }*/
            connection.Close();
            return newId;
        }

        public int Delete(int id)
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);
            int nChanged = command.ExecuteNonQuery();
            connection.Close();
            return nChanged;
            /*if (nChanged == 0)
            {
                Console.WriteLine("Book NOT deleted.");
            }
            else
            {
                Console.WriteLine("Book deleted.");
            }
            */
        }
    }
}
