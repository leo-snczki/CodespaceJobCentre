namespace CodespaceJobCentre;

public class Cor // Variáveis estáticas públicas que server para dar cor a interface.
{
    public static string Bold => "\x1b[1m";
    public static string Yellow => "\x1b[33m";
    public static string Red => "\x1b[31m";
    public static string Reset => "\x1b[0m"; // volta o texto ao normal.
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
        MostrarMenu();
    }
    static void MostrarMenu()
    {
        Console.WriteLine($"{Cor.Bold}Menu:{Cor.Reset}");
        Console.WriteLine($"{Cor.Yellow}1{Cor.Reset} - Atribuir senha");
        Console.WriteLine($"{Cor.Yellow}2{Cor.Reset} - Chamar próxima senha");
        Console.WriteLine($"{Cor.Yellow}3{Cor.Reset} - Consultar estatísticas");
        Console.WriteLine($"{Cor.Yellow}4{Cor.Reset} - Mostrar fila de senhas");
        Console.WriteLine($"{Cor.Yellow}5{Cor.Reset} - {Cor.Red}Sair{Cor.Reset}");
        Console.Write($"{Cor.Bold}Escolha uma opção:{Cor.Reset}");
    }
    static void LimparTela()
    {
        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
        Console.Clear();
    }
}
