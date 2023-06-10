using System.ComponentModel;

public class Alumno : INotifyPropertyChanged
{
    private string carnet;
    public string Carnet
    {
        get { return carnet; }
        set
        {
            carnet = value;
            OnPropertyChanged("Carnet");
        }
    }

    private string nombre;
    public string Nombre
    {
        get { return nombre; }
        set
        {
            nombre = value;
            OnPropertyChanged("Nombre");
        }
    }

    private string telefono;
    public string Telefono
    {
        get { return telefono; }
        set
        {
            telefono = value;
            OnPropertyChanged("Telefono");
        }
    }

    private string grado;
    public string Grado
    {
        get { return grado; }
        set
        {
            grado = value;
            OnPropertyChanged("Grado");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
