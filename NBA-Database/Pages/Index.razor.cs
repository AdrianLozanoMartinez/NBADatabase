using System.Net.Http.Json;

namespace NBA_Database.Pages
{
    //Para leer el archivo json 
    public partial class Index
    {
        //Metemos en una ra�z porque el tama�o no cambiar� de PlayersName
        private PlayersName[]? players;
        //Leemos y pasamos a players el cotnenido de json
        protected override async Task OnInitializedAsync()
        {
            players = await Http.GetFromJsonAsync<PlayersName[]>("/assets/data/dataPrueba.json");
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
            public int Draft_year { get; set; }
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

            //Obtener im�genes
            public string TeamImage => DaImagenGeneral(Team_abbreviation);

            public string CountryImage => DaImagenGeneral(Country);

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
        }
    }
}