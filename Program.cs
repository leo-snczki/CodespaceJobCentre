namespace CodespaceJobCentre;

public class Cor // Variáveis estáticas públicas que server para dar cor a interface.
{
    public static string bold = "\x1b[1m";
    public static string yellow = "\x1b[33m";
    public static string red = "\x1b[31m";
    public static string reset = "\x1b[0m"; // volta o texto ao normal.
}
public class Senha
{
    public string TipoServico { get; set; } // Inscrição/Atualização de Dados, apoio à Procura de Emprego ou informações Gerais.
    public string Numero { get; set; } // Será usado uma letra que terá numeros como sequência consoante o tipo do serviço.
    public bool Atendida { get; set; }

    public Senha(string numero, string tipoServico)
    {
        Numero = numero;
        TipoServico = tipoServico;
        Atendida = false; // todas as senhas são dadas como "não atendidas" no começo.
    }
    
    public override string ToString() => $"{Numero} ({TipoServico}) - {(Atendida ? "Atendida" : "Pendente")}";
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Console.WriteLine("Bye, World!");
    }
    static void MostrarMenu()
    {
            Console.WriteLine("Menu:");
            Console.WriteLine("1 - Atribuir senha");
            Console.WriteLine("2 - Chamar próxima senha");
            Console.WriteLine("3 - Consultar estatísticas");
            Console.WriteLine("4 - Mostrar fila de senhas");
            Console.WriteLine("5 - Sair");
            Console.Write("Escolha uma opção: ");
    }
}
