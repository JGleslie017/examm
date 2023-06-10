using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace TuProyecto
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataAdapter adaptador;
        private DataTable tabla;

        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Alumno> alumnos;
        public ObservableCollection<Alumno> Alumnos
        {
            get { return alumnos; }
            set
            {
                alumnos = value;
                OnPropertyChanged("Alumnos");
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\leslie\source\repos\Examen Final\Examen Final\Database1.mdf;Integrated Security=True";
            conexion = new SqlConnection(connectionString);
            comando = new SqlCommand();
            comando.Connection = conexion;

            Alumnos = new ObservableCollection<Alumno>();

            DataContext = this;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string carnet = txtCarnet.Text;
            string nombre = txtNombre.Text;
            string telefono = txtTelefono.Text;
            string grado = txtGrado.Text;

            // Validar que todos los campos estén completos
            if (string.IsNullOrEmpty(carnet) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(telefono) || string.IsNullOrEmpty(grado))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }

            string consulta = "INSERT INTO Alumno (Carnet, Nombre, Telefono, Grado) VALUES (@Carnet, @Nombre, @Telefono, @Grado)";
            comando.CommandText = consulta;
            comando.Parameters.Clear();
            comando.Parameters.AddWithValue("@Carnet", carnet);
            comando.Parameters.AddWithValue("@Nombre", nombre);
            comando.Parameters.AddWithValue("@Telefono", telefono);
            comando.Parameters.AddWithValue("@Grado", grado);

            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
                MessageBox.Show("Registro insertado correctamente.");
                CargarDatos();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar el registro: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        private void LimpiarCampos()
        {
            txtCarnet.Text = "";
            txtNombre.Text = "";
            txtTelefono.Text = "";
            txtGrado.Text = "";
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Alumno alumnoSeleccionado)
            {
                string consulta = "DELETE FROM Alumno WHERE Carnet = @Carnet";
                comando.CommandText = consulta;
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@Carnet", alumnoSeleccionado.Carnet);

                try
                {
                    conexion.Open();
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Registro eliminado correctamente.");
                    CargarDatos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar el registro: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
            else
            {
                MessageBox.Show("Seleccione un registro para eliminar.");
            }
        }

        private void CargarDatos()
        {
            string consulta = "SELECT * FROM Alumno";
            adaptador = new SqlDataAdapter(consulta, conexion);
            tabla = new DataTable();
            adaptador.Fill(tabla);

            Alumnos.Clear();

        }
    }
}
