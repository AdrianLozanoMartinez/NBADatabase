using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using static NBA_Database.Pages.Compare;
using static NBA_Database.Pages.Index;

namespace NBA_Database.Pages;

//Para leer el archivo json 
public partial class Index
{
    //Metemos en una ra�z porque el tama�o no cambiar� de PlayersName
    private List<PlayersName> players;
    private List<PlayersName> players2;

    //Leemos y pasamos a players el contenido de json
    protected override async Task OnInitializedAsync()
    {
        players = await Http.GetFromJsonAsync<List<PlayersName>>("/assets/data/datas.json");
        //CREAMOS NUEVA VARIABLE para reiniciar los datos antes de la b�squeda
        players2 = players;
        //LIMITECONTADOR = players.Count / 10; //El total de json lo dividimos entre 10
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
            try
            {
                switch (buscaBandera)
                {
                    case "Germany": imagen = "https://www.banderas-mundo.es/data/flags/w702/de.png"; break;
                    case "Argentina": imagen = "Argentina"; break;
                    case "Australia": imagen = "Australia"; break;
                    case "Bahamas": imagen = "Bahamas"; break;
                    case "Bosnia and Herzegovina": imagen = "Bosnia and Herzegovina"; break;
                    case "Brazil": imagen = "Brazil"; break;
                    case "Cameroon": imagen = "Cameroon"; break;
                    case "Canada": imagen = "Canada"; break;
                    case "China": imagen = "China"; break;
                    case "Croatia": imagen = "Croatia"; break;
                    case "Egypt": imagen = "Egypt"; break;
                    case "Slovenia": imagen = "Slovenia"; break;
                    case "Spain": imagen = "https://www.banderas-mundo.es/data/flags/w702/es.webp"; break;
                    case "France": imagen = "France"; break;
                    case "Georgia": imagen = "Georgia"; break;
                    case "Greece": imagen = "Greece"; break;
                    case "Haiti": imagen = "Haiti"; break;
                    case "Italy": imagen = "Italy"; break;
                    case "Latvia": imagen = "Latvia"; break;
                    case "Lithuania": imagen = "Lithuania"; break;
                    case "Mali": imagen = "Mali"; break;
                    case "New Zealand": imagen = "New Zealand"; break;
                    case "Poland": imagen = "Poland"; break;
                    case "Puerto Rico": imagen = "Puerto Rico"; break;
                    case "United Kingdom": imagen = "United Kingdom"; break;
                    case "Czech Republic": imagen = "Czech Republic"; break;
                    case "Democratic Republic of the Congo": imagen = "Democratic Republic of the Congo"; break;
                    case "Dominican Republic": imagen = "Dominican Republic"; break;
                    case "Russia": imagen = "Russia"; break;
                    case "Senegal": imagen = "Senegal"; break;
                    case "Serbia": imagen = "Serbia"; break;
                    case "South Sudan": imagen = "South Sudan"; break;
                    case "Sweden": imagen = "Sweden"; break;
                    case "Switzerland": imagen = "Switzerland"; break;
                    case "Turkey": imagen = "Turkey"; break;
                    case "Ukraine": imagen = "Ukraine"; break;
                    case "USA": imagen = "https://www.banderas-mundo.es/data/flags/w702/us.webp"; break;
                    default: imagen = $"/assets/image/mundo.png"; break;
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
    private int elementosPagina = 7; // N�mero de elementos por p�gina
    private int paginaActual = 1; // P�gina actual
    //Funci�n para mostrar y paginar
    public List<PlayersName> ConseguirJugadoresPaginacion()
    {
        int primerElemento = (paginaActual - 1) * elementosPagina; //A la p�gina actual le quito 1 para que al multiplicar
                                                       //por 1 se ponga 0 que al multiplicar te da 0, 1� posici�n
                                                       //Para obtener el siguiente grupo ser�a 2 que se suma abajo
                                                       //2 menos 1 da 1 que es la 2� pagina por el tama�o que son 7
                                                       //por lo que cogemos la siguiente posicion que empezamos que es
                                                       //el 7, ya que antes eran del 0 al 6 (7 posiciones)
                                                       //el siguiente del 7 al 13 (7 posiciones)
        return players.Skip(primerElemento).Take(elementosPagina).ToList();
        //Cogemos la lista, saltamos desde el inicio (0 al principio, luego desde la posicion 7)
        //Take -> Cogemos la cantidad de p�gina que queremos mostrar y pasamos a la lista para mostrar
    }

    public void Avanzar()
    {
        if (paginaActual < TotalPaginas()) //Si es menor a la catidad total de p�gina avanza 1
        {
            paginaActual++;
        }
    }

    public void Retroceder()
    {
        if (paginaActual > 1)
        {
            paginaActual--;
        }
    }
    //Cogemos la cantidad de p�ginas que se va a mostrar
    public int TotalPaginas()
    {
        return (int)Math.Ceiling((double)players.Count / elementosPagina); //En este caso 63
    }


    ///////////////////////////ORDENAR//////////////////////////////////////////////////////////////////////////
    bool pulsar = false;
    public List<PlayersName> OrdenarNombre()
    {
        //Controlamos con pulsar bool que cada vez le demos nos ordene de una u otra forma
        if (pulsar)
        {
            //Lo metemos en players que es la variable principal para que afecte directamente a la lista que tenemos creada y la ordene
            //directamente en la tabla
            players = players.OrderByDescending(x => x.Player_name).ToList(); //Ordenamos de Z a la A
            pulsar = false; //Cambiamos a true para que la pr�xima vez que le demos se ejecute el contrario, de la A a la Z
        }
        else
        {
            players = players.OrderBy(x => x.Player_name).ToList(); //Ordenamos de A a la Z
            pulsar = true; //Lo contrario del anterior, para que se ejecute luego de la Z a la A
        }
        return players; 
    }

    public List<PlayersName> OrdenarEdad()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Age).ToList(); 
            pulsar = false; 
        }
        else
        {
            players = players.OrderBy(x => x.Age).ToList(); 
            pulsar = true; 
        }
        return players;
    }

    public List<PlayersName> OrdenarAltura()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Player_heightM).ToList();
            pulsar = false;
        }
        else
        {
            players = players.OrderBy(x => x.Player_heightM).ToList();
            pulsar = true;
        }
        return players;
    }

    public List<PlayersName> OrdenarEquipo()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Team_abbreviation).ToList();
            pulsar = false;
        }
        else
        {
            players = players.OrderBy(x => x.Team_abbreviation).ToList();
            pulsar = true;
        }
        return players;
    }

    public List<PlayersName> OrdenarPais()
    {
        if (pulsar)
        {
            players = players.OrderByDescending(x => x.Country).ToList();
            pulsar = false;
        }
        else
        {
            players = players.OrderBy(x => x.Country).ToList();
            pulsar = true;
        }
        return players;
    }

    ///////////////////////////////////////////BUSCADOR///////////////////////////////////////////////////////////////////
    private string searchTerm = "";

    public void Buscar()
    {
        players = players2; //CREAMOS NUEVA VARIABLE para reiniciar los datos antes de la b�squeda
        players = players.Where(p => p.Player_name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        //private List<PlayersName> filteredPlayers => players.Where(p => p.Player_name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
        //IndexOf -> se comporta como contains, sino encuentra nada searchTerm devuelve -1 y por eso es >= 0 para que coja los resultados
        //y tiene StringComparison.OrdinalIgnoreCase para ignorarno distiga mayuscula/minuscula 
    }

    //////////////////////SELECCI�N 
    /////PA�SES
    public void CogePais(ChangeEventArgs e)
    {
        var seleccionar = e.Value.ToString();
        string pais = " ";

        switch (seleccionar)
        {
            case "Germany": pais = "Germany"; break;
            case "Argentina": pais = "Argentina"; break;
            case "Australia": pais = "Australia"; break;
            case "Bahamas": pais = "Bahamas"; break;
            case "Bosnia and Herzegovina": pais = "Bosnia and Herzegovina"; break;
            case "Brazil": pais = "Brazil"; break;
            case "Cameroon": pais = "Cameroon"; break;
            case "Canada": pais = "Canada"; break;
            case "China": pais = "China"; break;
            case "Croatia": pais = "Croatia"; break;
            case "Egypt": pais = "Egypt"; break;
            case "Slovenia": pais = "Slovenia"; break;
            case "Spain": pais = "Spain"; break;
            case "France": pais = "France"; break;
            case "Georgia": pais = "Georgia"; break;
            case "Greece": pais = "Greece"; break;
            case "Haiti": pais = "Haiti"; break;
            case "Italy": pais = "Italy"; break;
            case "Latvia": pais = "Latvia"; break;
            case "Lithuania": pais = "Lithuania"; break;
            case "Mali": pais = "Mali"; break;
            case "New Zealand": pais = "New Zealand"; break;
            case "Poland": pais = "Poland"; break;
            case "Puerto Rico": pais = "Puerto Rico"; break;
            case "United Kingdom": pais = "United Kingdom"; break;
            case "Czech Republic": pais = "Czech Republic"; break;
            case "Democratic Republic of the Congo": pais = "Democratic Republic of the Congo"; break;
            case "Dominican Republic": pais = "Dominican Republic"; break;
            case "Russia": pais = "Russia"; break;
            case "Senegal": pais = "Senegal"; break;
            case "Serbia": pais = "Serbia"; break;
            case "South Sudan": pais = "South Sudan"; break;
            case "Sweden": pais = "Sweden"; break;
            case "Switzerland": pais = "Switzerland"; break;
            case "Turkey": pais = "Turkey"; break;
            case "Ukraine": pais = "Ukraine"; break;
            case "USA": pais = "USA"; break;
            default: players = players2.Select(x => x).ToList(); break; // Restablece la lista original si no hay coincidencia
        }
        if (seleccionar == "todos") players = players2.Select(x => x).ToList();
        else players = players2.Where(x => x.Country == pais).ToList();
    }

    /////EQUIPO
    public void CogeEquipo(ChangeEventArgs e)
    {
        var seleccionar = e.Value.ToString();
        string equipo = " ";

        switch (seleccionar)
        {
            case "ATL": equipo = "ATL"; break;
            case "BOS": equipo = "BOS"; break;
            case "BKN": equipo = "BKN"; break;
            case "CHA": equipo = "CHA"; break;
            case "CHI": equipo = "CHI"; break;
            case "CLE": equipo = "CLE"; break;
            case "DAL": equipo = "DAL"; break;
            case "DEN": equipo = "DEN"; break;
            case "DET": equipo = "DET"; break;
            case "GSW": equipo = "GSW"; break;
            case "HOU": equipo = "HOU"; break;
            case "IND": equipo = "IND"; break;
            case "LAC": equipo = "LAC"; break;
            case "LAL": equipo = "LAL"; break;
            case "MEM": equipo = "MEM"; break;
            case "MIA": equipo = "MIA"; break;
            case "MIL": equipo = "MIL"; break;
            case "MIN": equipo = "MIN"; break;
            case "NOP": equipo = "NOP"; break;
            case "NYK": equipo = "NYK"; break;
            case "OKC": equipo = "OKC"; break;
            case "ORL": equipo = "ORL"; break;
            case "PHI": equipo = "PHI"; break;
            case "PHX": equipo = "PHX"; break;
            case "POR": equipo = "POR"; break;
            case "SAC": equipo = "SAC"; break;
            case "SAS": equipo = "SAS"; break;
            case "TOR": equipo = "TOR"; break;
            case "UTA": equipo = "UTA"; break;
            case "WAS": equipo = "WAS"; break;
            default: players = players2.Select(x => x).ToList(); break; // Restablece la lista original si no hay coincidencia
        }
        if (seleccionar == "todos") players = players2.Select(x => x).ToList();
        else players = players2.Where(x => x.Team_abbreviation == equipo).ToList();
    }
}