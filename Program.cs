using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CodespaceJobCentre;

public static class Cor
{
    public static string Bold => "\x1b[1m";
    public static string Red => "\x1b[31m";
    public static string Green => "\x1b[32m";
    public static string Yellow => "\x1b[33m";
    public static string Reset => "\x1b[0m";
}

public class Senha
{
    public string Numero { get; set; }
    public string TipoServico { get; set; }
    public bool Atendida { get; set; }

    public Senha(string numero, string tipoServico)
    {
        Numero = numero;
        TipoServico = tipoServico;
        Atendida = false;
    }

    public void Atender() => Atendida = true;

}

public class Servico
{
    public string Nome { get; set; }
    public string Prefixo { get; set; }
    private Queue<Senha> fila = new Queue<Senha>();
    private int contadorSenhas = 0;

    public Servico(string nome, string prefixo)
    {
        Nome = nome;
        Prefixo = prefixo;
    }

    public void AtribuirSenha()
    {
        string numeroSenha;
        contadorSenhas++;
        numeroSenha = $"{Prefixo}{contadorSenhas:D3}";
        var senha = new Senha(numeroSenha, Nome);
        fila.Enqueue(senha);
        Console.WriteLine($"{Cor.Yellow}Senha atribuída: {senha.Numero} - {Nome}{Cor.Reset}");
    }

    public Senha ChamarProximaSenha()
    {
        if (fila.Count > 0)
        {
            Senha senha = fila.Dequeue();
            senha.Atender();
            return senha;
        }
        return null;
    }

    public int SenhasPendentes() => fila.Count;


    public int SenhasAtendidas() => fila.Count(s => s.Atendida);


    public string UltimaSenhaAtendida()
    {
        foreach (var senha in fila)
        {
            if (senha.Atendida)
                return senha.Numero;
        }
        return "Nenhuma";
    }

    public void ExibirFila()
    {
        Console.WriteLine($"Fila de {Nome}:");
        foreach (var senha in fila)
        {
            Console.WriteLine($"- {senha.Numero} ({(senha.Atendida ? "Atendida" : "Pendente")})");
        }
    }
}

public class GestorDeFilas
{
    private List<Servico> servicos;

    public GestorDeFilas()
    {
        servicos = new List<Servico>
        {
            new Servico("Inscrição/Atualização de Dados", "A"),
            new Servico("Apoio à Procura de Emprego", "B"),
            new Servico("Informações Gerais", "C")
        };
    }

    public void AtribuirSenha()
    {
        int escolha;
        Console.Clear();
        MostrarMenuSecundário();

        escolha = int.Parse(Console.ReadLine()) - 1;
        if (escolha >= 0 && escolha < servicos.Count)
        {
            servicos[escolha].AtribuirSenha();
        }
    }

    public void ChamarProximaSenha()
    {
        int escolha;
        Console.Clear();
        MostrarMenuSecundário();
        escolha = int.Parse(Console.ReadLine()) - 1;
        if (escolha >= 0 && escolha < servicos.Count)
        {
            Senha senha = servicos[escolha].ChamarProximaSenha();
            if (senha != null)
            {
                Console.WriteLine($"{Cor.Green}Atendendo: {senha.Numero} - {senha.TipoServico}{Cor.Reset}");
            }
            else
            {
                Console.WriteLine($"{Cor.Red}Não há senhas pendentes neste serviço.{Cor.Reset}");
            }
        }
    }
    public void MostrarMenuSecundário()
    {
        Console.WriteLine($"\n{Cor.Bold}--- Selecione o serviço ---{Cor.Reset}\n");
        for (int i = 0; i < servicos.Count; i++)
        {
            Console.WriteLine($"{Cor.Yellow}{i + 1}.{Cor.Reset} {servicos[i].Nome}");
        }
        Console.WriteLine($"{Cor.Red}0. Sair{Cor.Reset}");
        Console.Write($"{Cor.Bold}Escolha uma opção: {Cor.Reset} ");
    }

    public void ConsultarEstatisticas()
    {
        Console.Clear();
        Console.WriteLine($"\n{Cor.Bold}--- Estatísticas ---{Cor.Reset}\n");
        foreach (var servico in servicos)
        {
            Console.WriteLine($"{Cor.Bold}{servico.Nome}:{Cor.Reset}");
            Console.WriteLine($"- Senhas atendidas: {servico.SenhasAtendidas()}");
            Console.WriteLine($"- Senhas pendentes: {servico.SenhasPendentes()}");
            Console.WriteLine($"- Última senha atendida: {servico.UltimaSenhaAtendida()}\n");
        }
    }

    public void ExportarEstatisticas(string caminhoArquivo)
    {
        try
        {
            List<string> linhas = new List<string>();
            foreach (var servico in servicos)
            {
                linhas.Add($"Serviço: {servico.Nome}");
                linhas.Add($"Senhas atendidas: {servico.SenhasAtendidas()}");
                linhas.Add($"Senhas pendentes: {servico.SenhasPendentes()}");
                linhas.Add($"Última senha atendida: {servico.UltimaSenhaAtendida()}\n");
            }

            File.WriteAllLines(caminhoArquivo, linhas);
            Console.WriteLine($"{Cor.Green}Estatísticas exportadas para {caminhoArquivo}{Cor.Reset}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Cor.Red}Erro ao exportar estatísticas: {ex.Message}{Cor.Reset}");
        }
    }
}

class Program
{
    static void Main()
    {
        var gestorDeFilas = new GestorDeFilas();
        bool continuar = true;

        while (continuar)
        {
            MostrarMenuPrincipal();
            MenuEscolha(ref continuar, gestorDeFilas);
        }
    }
    static void MenuEscolha(ref bool continuar, GestorDeFilas gestorDeFilas)
    {
        
        string op1;
        string op2;
        op1 = Console.ReadLine();
        switch (op1)
        {
            case "1":
                gestorDeFilas.AtribuirSenha();
                LimparTela();
                break;
            case "2":
                gestorDeFilas.ChamarProximaSenha();
                LimparTela();
                break;
            case "3":
                Console.Clear();
                Console.WriteLine($"\n{Cor.Bold}--- Estatísticas ---{Cor.Reset}\n");
                Console.WriteLine($"{Cor.Yellow}1.{Cor.Reset} Consultar");
                Console.WriteLine($"{Cor.Yellow}2.{Cor.Reset} Exportar");
                Console.WriteLine($"{Cor.Yellow}3.{Cor.Reset} Consultar e Exportar");
                Console.WriteLine($"{Cor.Red}0. Sair{Cor.Reset}");
                Console.Write($"{Cor.Bold}Escolha uma opção: {Cor.Reset} ");
                op2 = Console.ReadLine();
                switch (op2)
                {
                    case "1":
                        gestorDeFilas.ConsultarEstatisticas();
                        LimparTela();
                        break;
                    case "2":
                    
                        gestorDeFilas.ExportarEstatisticas("estatisticas.txt");
                        LimparTela();
                        break;
                    case "3":
                        gestorDeFilas.ExportarEstatisticas("estatisticas.txt");
                        gestorDeFilas.ConsultarEstatisticas();
                        LimparTela();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção indisponível");
                        LimparTela();
                        break;
                }
                break;
            case "0":
                gestorDeFilas.ExportarEstatisticas("estatisticas.txt");
                continuar = false;
                break;
            default:
                Console.WriteLine($"\n{Cor.Red}Opção inválida, tente novamente.{Cor.Reset}");
                LimparTela();
                break;
        }
        if (continuar) Console.Clear();
    }
    static void LimparTela()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
    static void MostrarMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine($"\n{Cor.Bold}--- Centro de emprego ---{Cor.Reset}\n");
        Console.WriteLine($"{Cor.Yellow}1.{Cor.Reset} Atribuir senha");
        Console.WriteLine($"{Cor.Yellow}2.{Cor.Reset} Chamar próxima senha");
        Console.WriteLine($"{Cor.Yellow}3.{Cor.Reset} Consultar estatísticas");
        Console.WriteLine($"{Cor.Red}0.{Cor.Reset} {Cor.Red}Sair{Cor.Reset}");
        Console.Write($"{Cor.Bold}Escolha uma opção: {Cor.Reset} ");
    }
}

