using System.Globalization;

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<string, int> mapaDeEstados = new Dictionary<string, int>();
        try
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            StreamReader src = File.OpenText("estados.csv");
            StreamWriter dst = File.CreateText("insert_estados.sql");
            string? linha = src.ReadLine();
            string templateSQL = "INSERT INTO tb_estado VALUES ( %CODIGO%, '%NOME%', '%SIGLA%');";

            while ((linha = src.ReadLine()) != null) {
                string[] campos = linha.Split(";");
                string novoSQL = templateSQL.Replace("%CODIGO%", campos[0]).Replace("%NOME%", campos[1]).Replace("%SIGLA%", campos[2]);
                mapaDeEstados.Add(campos[2], int.Parse(campos[0]));
                dst.WriteLine(novoSQL);
            }
            dst.Close();
            src.Close();
        }
        catch (IOException ex)
        {
            Console.WriteLine("Erro!", ex);
        }

        //Gera o insert da cidade
        try
        {
            StreamReader src = File.OpenText("cidade.csv");
            StreamWriter dst = File.CreateText("insert_cidade.sql");
            string? linha = src.ReadLine();
            string templateSQL = "INSERT INTO tb_cidade VALUES ( null, '%NOME%', %ESTADO%);";

            while ((linha = src.ReadLine()) != null) {
                string[] campos = linha.Split(";");
                int codEstado = mapaDeEstados[(campos[3])];

                string novoSQL = templateSQL.Replace("%NOME%", campos[4]).Replace("%ESTADO%", codEstado.ToString());
                dst.WriteLine(novoSQL);
            }
            dst.Close();
            src.Close();
        }
        catch (IOException ex) 
        {
            Console.WriteLine("Erro!", ex);
        }
    }
}