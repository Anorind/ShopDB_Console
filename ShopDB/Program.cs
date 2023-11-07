using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShopDB
{

    internal class Program
    {


        static string conn = ConfigurationManager.ConnectionStrings["ShopConn"].ConnectionString;
        static void Main(string[] args)
        {
            DataSet ShopDB = new DataSet("ShopDB");

            // Создаем таблицы
            DataTable Sales = new DataTable("Sales");
            DataTable Customers = new DataTable("Customers");
            DataTable Sellers = new DataTable("Sellers");

            // Добавляем таблицы в ShopDB
            ShopDB.Tables.AddRange(new DataTable[] { Sales, Customers, Sellers });

            // Создаем столбцы для каждой таблицы и устанавливаем PrimaryKey
            DataColumn SalesColumn = Sales.Columns.Add("ID", typeof(int));
            DataColumn ProductNameColumn = Sales.Columns.Add("ProductName", typeof(string));
            DataColumn CustomerIDColumnInSales = Sales.Columns.Add("CustomerID", typeof(int));
            DataColumn SellerIDColumnInSales = Sales.Columns.Add("SellerID", typeof(int));
            Sales.PrimaryKey = new DataColumn[] { SalesColumn };

            DataColumn CustomersColumn = Customers.Columns.Add("ID", typeof(int));
            DataColumn FirstNameColumnInCustomers = Customers.Columns.Add("FirstName", typeof(string));
            DataColumn LastNameColumnInCustomers = Customers.Columns.Add("LastName", typeof(string));
            Customers.PrimaryKey = new DataColumn[] { CustomersColumn };

            DataColumn SellersColumn = Sellers.Columns.Add("ID", typeof(int));
            DataColumn FirstNameColumnInSellers = Sellers.Columns.Add("FirstName", typeof(string));
            DataColumn LastNameColumnInSellers = Sellers.Columns.Add("LastName", typeof(string));
            Sellers.PrimaryKey = new DataColumn[] { SellersColumn };

            // Создаем ограничения ForeignKey
            ForeignKeyConstraint fkSales_Customers = new ForeignKeyConstraint("FK_Sales_Customers", CustomersColumn, CustomerIDColumnInSales);
            ForeignKeyConstraint fkSales_Sellers = new ForeignKeyConstraint("FK_Sales_Sellers", SellersColumn, SellerIDColumnInSales);

            // Добавляем ограничения ForeignKey в таблицу Sales
            Sales.Constraints.AddRange(new Constraint[] { fkSales_Customers, fkSales_Sellers });


            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();

                //CREATE TABLE Customers
                //(
                //    ID int PRIMARY KEY IDENTITY(1, 1),
                //    FirstName nvarchar(50) NOT NULL,
                //    LastName nvarchar(50) NOT NULL
                //);
                //string insertCustomer = "INSERT INTO Customers (FirstName, LastName) VALUES " +
                //                        "('John', 'Johns'), " +
                //                        "('Vinny', 'Johans'), " +
                //                        "('Leed', 'Bronx'), " +
                //                        "('Jony', 'Foster'), " +
                //                        "('Ivan', 'Ivanovich'), " +
                //                        "('Lera', 'Golovanchenko'), " +
                //                        "('Anastasia', 'Bitchevna'), " +
                //                        "('Irak', 'Iranovich')";
                //using (SqlCommand command = new SqlCommand(insertCustomer, connection))
                //{
                //    command.ExecuteNonQuery();
                //}


                //CREATE TABLE Sellers
                //(
                //    ID int PRIMARY KEY IDENTITY(1, 1),
                //    FirstName nvarchar(50) NOT NULL,
                //    LastName nvarchar(50) NOT NULL
                //);
                //string insertSellers = "INSERT INTO Sellers (FirstName, LastName) VALUES " +
                //                       "('Lev', 'Paskal'), " +
                //                       "('Lonny', 'Ment'), " +
                //                       "('Djimboo', 'Valerievich'), " +
                //                       "('Kody', 'Molot'), " +
                //                       "('Sveta', 'st.George'), " +
                //                       "('Jan', 'Clode'), " +
                //                       "('Anatoliy', 'Peregibko'), " +
                //                       "('Oleg', 'Tyagnybok')";
                //using (SqlCommand command = new SqlCommand(insertSellers, connection))
                //{
                //    command.ExecuteNonQuery();
                //}


                //CREATE TABLE Sales
                //(
                //    ID int PRIMARY KEY IDENTITY(1, 1),
                //    ProductName nvarchar(50) NOT NULL,
                //    CustomerID int FOREIGN KEY REFERENCES Customers(ID) NOT NULL,
                //    SellerID int FOREIGN KEY REFERENCES Sellers(ID) NOT NULL
                //);
                //string insertSales = "INSERT INTO Sales (ProductName, CustomerID, SellerID) " +
                //     "VALUES ('Banka', " +
                //     "(SELECT ID FROM Customers WHERE FirstName = 'John' AND LastName = 'Johns'), " +
                //     "(SELECT ID FROM Sellers WHERE FirstName = 'Lev' AND LastName = 'Paskal'))";
                //using (SqlCommand command = new SqlCommand(insertSales, connection))
                //{
                //    command.ExecuteNonQuery();
                //}

                ////Это я не мог разобраться почему команда Delete не удаляет ID и нашел такую команду для
                ////сброса ID
                ////string resetIdentity = "DBCC CHECKIDENT ('Sales', RESEED, 0)";
                ////using (SqlCommand command = new SqlCommand(resetIdentity, connection))
                ////{
                ////    command.ExecuteNonQuery();
                ////}




                // Создаю адаптеры данных для каждой таблицы
                SqlDataAdapter SalesAdapter = new SqlDataAdapter("SELECT * FROM Sales", connection);
                SqlDataAdapter CustomersAdapter = new SqlDataAdapter("SELECT * FROM Customers", connection);
                SqlDataAdapter SellersAdapter = new SqlDataAdapter("SELECT * FROM Sellers", connection);

                // Загружаю данные в таблицы
                CustomersAdapter.Fill(Customers);
                SellersAdapter.Fill(Sellers);
                SalesAdapter.Fill(Sales);

                // Вывожу данные на консоль
                Console.WriteLine("Sales:");
                foreach (DataRow row in Sales.Rows)
                {
                    Console.WriteLine($"ID: {row["ID"]}, ProductName: {row["ProductName"]}, CustomerID: {row["CustomerID"]}, SellerID: {row["SellerID"]}");
                }

                Console.WriteLine("\nCustomers:");
                foreach (DataRow row in Customers.Rows)
                {
                    Console.WriteLine($"ID: {row["ID"]}, FirstName: {row["FirstName"]}, LastName: {row["LastName"]}");
                }

                Console.WriteLine("\nSellers:");
                foreach (DataRow row in Sellers.Rows)
                {
                    Console.WriteLine($"ID: {row["ID"]}, FirstName: {row["FirstName"]}, LastName: {row["LastName"]}");
                }
            }
            Console.ReadLine();


        }
    }
}
