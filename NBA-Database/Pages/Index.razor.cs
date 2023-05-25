using System.ComponentModel.Design;
using System.Net.Http.Json;

namespace NBA_Database.Pages;

//Para leer el archivo json 
public partial class Index
{
    //Metemos en una ra�z porque el tama�o no cambiar� de PlayersName
    private PlayersName[]? players;

    //private Paginacion paginacion;

    //Leemos y pasamos a players el cotnenido de json
    protected override async Task OnInitializedAsync()
    {
        //players = await Http.GetFromJsonAsync<PlayersName[]>("/assets/data/dataPrueba.json");
        players = await Http.GetFromJsonAsync<PlayersName[]>("/assets/data/datas.json");
    }

    public class PlayersName
    {
        //player_name
        public string Player_name { get; set; }
        //age
        public int Age { get; set; }
        //player_height
        public double Player_height { get; set; }
        //player_weight
        public double Player_weight { get; set; }
        //team_abbreviation
        public string Team_abbreviation { get; set; }
        //college
        public string College { get; set; }
        //country
        public string Country { get; set; }
        //draft_year
        public string Draft_year { get; set; }
        //gp
        public int Gp { get; set; }
        //pts
        public double Pts { get; set; }
        //reb
        public double Reb { get; set; }
        //ast
        public double Ast { get; set; }
        //season
        public string Season { get; set; }
        //Propiedad nueva aplicando una propiedad anterior con un m�todo para sacar solo dos decimales y en metros
        public string Player_heightM => CalcularDecimal(Player_height);
        //En este caso directamente meterlo en un string y ponerle dos decimales porque ya est� transformado el valor
        public string Player_weightG => $"{Player_weight:0.00}";

        //Saca en metros con 2 decimales
        public string CalcularDecimal(double num)
        {
            num /= 100;
            return $"{num:0.00}";
        }

        //public string DaImagenPais() => $"/assets/image/equipos/{Country}.png";
        public string DaImagenGeneral(string buscaBandera)
        {
            //Imagen de salida que la obtenemos en el foreach del Index.razor al llamar al m�todo
            string imagen = "";
            //Creamos array con las banderas que tenemos la imagen
            string[] imagenExiste = { "CHI", "LAC", "TOR", "USA" };
            try
            {
                //Recorremos el array de las fotos existentes
                for (int i = 0; i < imagenExiste.Length; i++)
                {
                    //Miramos si el valor del json que leemos lo contiene el array, si lo contiene lo muestra y sino muestra una predeterminada
                    if (imagenExiste.Contains(buscaBandera))
                    {
                        imagen = $"/assets/image/equiposNBA/{buscaBandera}.png";
                    }
                    else
                    {
                        imagen = $"/assets/image/mundo.png";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return imagen;
        }

///////////////////////////PAGINACI�N//////////////////////////////////////////////////////////////////////////
    }
    //Contabilizar
    private int contador = 0;
    private const int CANTMOSTRAR = 10;
    int LIMITECONTADOR;
    int contarPag = 0;

    public int ContarPag()
    {
        if(contador == 0)
        {
            contador = 10;
        }
        return contarPag = contador / 10;
    }

    public void Retroceder()
    {
        if (contador >= 1)
        {
            contador -= CANTMOSTRAR;
            //Para que salga el n�mero de p�ginas
            
        }
        AgregarEliminar(contador);
        //Console.WriteLine(contador);
    }
    public void Avanzar()
    {
        if (contador <= players.Length && contador <= LIMITECONTADOR) //Para que no cuente m�s de lo que deber�a contar al repartir
        {                                                             //el listado entre los que hay que mostrar
            contador += CANTMOSTRAR;
        }
        AgregarEliminar(contador);
        //Console.WriteLine(contador);
    }
    //Meter datos en nuevo array
    public PlayersName[] AgregarEliminar(int contador)
    {
        LIMITECONTADOR = players.Length/10; //El total de json lo dividimos entre 10
        //Se crea nuevo array para meter los nuevos datos con l�mite al que pongamos para no cargar toda la lista
        PlayersName[] playersNew = new PlayersName[CANTMOSTRAR];
        try
        {
            //LLENAMOS EL NUEVO ARRAY
            int newIndex = 0; //Creamos un contador para almacenar desde la 1� posici�n, ya que si ponemos contador o i que es el mismo
            //valor, no se rellena correctamente, porque contador empieza desde donde est�
            for (int i = contador; i < contador + CANTMOSTRAR; i++) //Empieza desde el contador que vaya (ej: contador += CANTMOSTRAR y
            {                                                       //CANTMOSTRAR es 10 u otro, entonces el valor que empieza 0 se pone 
                                                                    //en 10 cuando pulsamos bot�n hasta los pr�ximos 10, los 20 siguientes
                if (i < players.Length && players[i] != null) //Si el n�mero del for que comience es menor que el total del array json y 
                {                                             //cada elemento es diferente a nulo
                    playersNew[newIndex] = players[i];  //Metemos cada elemento a la posici�n nueva del array nuevo
                    newIndex++;     //Posici�n nueva, despu�s del 0 se incrementa en 1 para la pr�xima insercci�n
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return playersNew;
    }
}