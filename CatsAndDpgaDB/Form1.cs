using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CatsAndDpgaDB
{
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;
        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["CatsAndDpgaDB.Properties.Settings.PetsConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulatePetsTable();
        }
        public void PopulatePetsTable()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM PetType", connection))
            {
                DataTable petTable = new DataTable();
                adapter.Fill(petTable);

                listPets.DisplayMember = "PetTypeName";
                listPets.ValueMember = "TypeId";
                listPets.DataSource = petTable;
            }
        }

        private void listPets_SelectedIndexChanged(object sender, EventArgs e)
        {
           PopulatePetNames();
        }

        private void PopulatePetNames()
        {
            string query = "SELECT Pet.Name FROM PetType INNER JOIN Pet ON Pet.TypeId= PetType.TypeId WHERE PetType.TypeId= @TypeId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                command.Parameters.AddWithValue("@TypeId", listPets.SelectedValue);
                DataTable petNameTable = new DataTable();
                adapter.Fill(petNameTable);

                listPetNames.DisplayMember = "Name";
                listPetNames.ValueMember = "Id";
                listPetNames.DataSource= petNameTable;

            } 
            
        }

        private void listPetNames_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
