using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodespaceJobCentre;
public enum TipoServico
{
    InscricaoAtualizacao = 1, 
    ApoioProcuraEmprego,
    InformacoesGerais 
}
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
public class Gerenciador
{
    private List<Senha> fila = new List<Senha>();
    // Contador para gerar números únicos para o tipo.
    private int contadorSenhasD = 1; // Inscrição/Atualização de Dados.
    private int contadorSenhasE = 1; // Apoio à Procura de Emprego.
    private int contadorSenhasG = 1; // Informações Gerais.
}
class Program
{
    static void Main(string[] args)
    {
        bool continuar = true;
        string op;
        while (continuar)
        {
            MostrarMenu();
            op = Console.ReadLine();
            Console.Clear();
            SelecionarOp(ref continuar, op);
        }

    }
    static void MostrarMenu()
    {
        Console.WriteLine($"{Cor.Bold}Menu:{Cor.Reset}");
        Console.WriteLine($"{Cor.Yellow}1{Cor.Reset} - Atribuir senha");
        Console.WriteLine($"{Cor.Yellow}2{Cor.Reset} - Chamar próxima senha");
        Console.WriteLine($"{Cor.Yellow}3{Cor.Reset} - Consultar estatísticas");
        Console.WriteLine($"{Cor.Yellow}4{Cor.Reset} - Mostrar fila de senhas");
        Console.WriteLine($"{Cor.Yellow}5{Cor.Reset} - {Cor.Red}Sair{Cor.Reset}");
        Console.Write($"{Cor.Bold}Escolha uma opção:{Cor.Reset} ");
    }
    static void LimparTela()
    {
        Console.WriteLine("\nPressione qualquer tecla para voltar ao menu...");
        Console.ReadKey();
        Console.Clear();
    }

    static void SelecionarOp(ref bool continuar, string op)
    {
        switch (op)
        {
            case "1":
                break;
            case "2":
                break;
            case "3":
                break;
            case "4":
                break;
            case "5":
                // Vai mostrar stats antes de fechar e possibilidade de exportar para um txt.
                continuar = false;
                break;
            default:
                Console.WriteLine("\nOpção inválida. Tente novamente.");
                break;
        }
        if (continuar) LimparTela();
    }
    static void ExibirTiposServico()
    {
        Console.WriteLine("Escolha o tipo de serviço:");
        Console.WriteLine($"{Cor.Yellow}1{Cor.Reset} - Inscrição/Atualização de Dados");
        Console.WriteLine($"{Cor.Yellow}2{Cor.Reset} - Apoio à Procura de Emprego");
        Console.WriteLine($"{Cor.Yellow}3{Cor.Reset} - Informações Gerais");
        Console.Write($"{Cor.Bold}Escolha uma opção:{Cor.Reset} ");
    }
}
