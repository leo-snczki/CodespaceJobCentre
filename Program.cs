using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CodespaceJobCentre;

public static class Interface
{
    public static string Bold => "\x1b[1m";
    public static string Red => "\x1b[31m";
    public static string Green => "\x1b[32m";
    public static string Yellow => "\x1b[33m";
    public static string OpcaoErro => $"\n{Interface.Red}Opção indisponível.{Interface.Reset}";
    public static string EscOp => $"{Interface.Bold}\nEscolha uma opção: {Interface.Reset} ";
    public static string Reset => "\x1b[0m";
    public static void LimparTela()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear();
    }
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

    private List<Senha> filaSenhasAtendidas = new List<Senha>();
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
        Console.Clear();
        fila.Enqueue(senha);
        Console.WriteLine($"\n{Interface.Yellow}Senha atribuída: {Interface.Bold}{senha.Numero} - {Nome}{Interface.Reset}");
    }

    public Senha ChamarProximaSenha()
    {
        if (fila.Count > 0)
        {
            Senha senha = fila.Dequeue();
            senha.Atender();
            filaSenhasAtendidas.Add(senha);
            return senha;
        }
        return null;
    }
    public int SenhasPendentes() => fila.Count;


    public int SenhasAtendidas() => filaSenhasAtendidas.Count;

    public string UltimaSenhaAtendida()
    {
        if (filaSenhasAtendidas.Count > 0)
            return filaSenhasAtendidas.Last().Numero;
        return "Nenhuma";
    }


    public string UltimaSenha()
    {
        if (fila.Count > 0)
            return fila.Last().Numero;
        return "Nenhuma";
    }


    public void ExibirSenha()
    {
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

        if (int.TryParse(Console.ReadLine(), out escolha) && escolha > 0 && escolha <= servicos.Count)
        {
            servicos[escolha - 1].AtribuirSenha();
            Interface.LimparTela();
        }
        else if (escolha == 0) return;

        else
        {
            Console.Clear();
            Console.WriteLine(Interface.OpcaoErro);
        }
    }

    public void ChamarProximaSenha()
    {
        int escolha;
        Console.Clear();
        MostrarMenuSecundário();

        if (int.TryParse(Console.ReadLine(), out escolha) && escolha > 0 && escolha <= servicos.Count)
        {
            Senha senha = servicos[escolha - 1].ChamarProximaSenha();
            if (senha != null)
            {
                Console.Clear();
                Console.WriteLine($"\n{Interface.Green}Atendido: {senha.Numero} - {senha.TipoServico}{Interface.Reset}");
                Interface.LimparTela();
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"\n{Interface.Red}Não há senhas pendentes neste serviço.{Interface.Reset}");
            }
        }
        else if (escolha == 0)
        {
            return;
        }

        else
        {
            Console.Clear();
            Console.WriteLine(Interface.OpcaoErro);
        }

    }
    public void MostrarMenuSecundário()
    {
        Console.WriteLine($"\n{Interface.Bold}--- Selecione o serviço ---{Interface.Reset}\n");
        for (int i = 0; i < servicos.Count; i++)
        {
            Console.WriteLine($"{Interface.Yellow}{i + 1}.{Interface.Reset} {servicos[i].Nome}");
        }
        Console.WriteLine($"{Interface.Red}0. Sair{Interface.Reset}");
        Console.Write(Interface.EscOp);
    }

    public void ConsultarEstatisticas()
    {
        Console.Clear();
        Console.WriteLine($"\n{Interface.Bold}--- Estatísticas ---{Interface.Reset}\n");
        foreach (var servico in servicos)
        {
            Console.WriteLine($"{Interface.Bold}{servico.Nome}:{Interface.Reset}");
            Console.WriteLine($"- Senhas atendidas: {servico.SenhasAtendidas()}");
            Console.WriteLine($"- Senhas pendentes: {servico.SenhasPendentes()}");
            Console.WriteLine($"- Última senha solicitada: {servico.UltimaSenha()}");
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
                linhas.Add($"Última senha: {servico.UltimaSenha()}");
                linhas.Add($"Última senha atendida: {servico.UltimaSenhaAtendida()}\n");
            }

            File.WriteAllLines(caminhoArquivo, linhas);
            Console.WriteLine($"{Interface.Green}Estatísticas exportadas para {caminhoArquivo}{Interface.Reset}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Interface.Red}Erro ao exportar estatísticas: {ex.Message}{Interface.Reset}");
        }
    }
    public void ExibirFila()
    {
        Console.Clear();
        Console.WriteLine($"\n{Interface.Bold}--- Fila Completa ---{Interface.Reset}");

        foreach (var servico in servicos)
        {
            // Exibir as senhas de cada serviço
            Console.WriteLine($"\n{Interface.Bold}{servico.Nome}:{Interface.Reset}");
            servico.ExibirSenha();
        }

        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
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
            MenuEscolha(ref continuar, ref gestorDeFilas);
        }
    }
    static void MenuEscolha(ref bool continuar, ref GestorDeFilas gestorDeFilas)
    {

        string op1;
        string op2;
        DateTime currentDate = DateTime.Now;
        op1 = Console.ReadLine();
        switch (op1)
        {
            case "1":
                gestorDeFilas.AtribuirSenha();
                break;
            case "2":
                gestorDeFilas.ChamarProximaSenha();
                break;
            case "3":
                Console.Clear();
                Console.WriteLine($"\n{Interface.Bold}--- Estatísticas ---{Interface.Reset}\n");
                Console.WriteLine($"{Interface.Yellow}1.{Interface.Reset} Consultar");
                Console.WriteLine($"{Interface.Yellow}2.{Interface.Reset} Exportar");
                Console.WriteLine($"{Interface.Yellow}3.{Interface.Reset} Consultar e Exportar");
                Console.WriteLine($"{Interface.Red}0. Sair{Interface.Reset}");
                Console.Write(Interface.EscOp);
                op2 = Console.ReadLine();
                switch (op2)
                {
                    case "1":
                        gestorDeFilas.ConsultarEstatisticas();
                        Interface.LimparTela();
                        break;
                    case "2":
                        ExportadorNome(gestorDeFilas);
                        Interface.LimparTela();
                        break;
                    case "3":
                        ExportadorNome(gestorDeFilas);
                        gestorDeFilas.ConsultarEstatisticas();
                        Interface.LimparTela();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine(Interface.OpcaoErro);
                        Interface.LimparTela();
                        break;
                }
                break;
            case "4":
                gestorDeFilas.ExibirFila();
                break;
            case "0":
                gestorDeFilas.ExportarEstatisticas("estatisticas-backup.txt");
                continuar = false;
                break;
            case "X":
                continuar = false;
                break;
            default:
                Console.WriteLine(Interface.OpcaoErro);
                Interface.LimparTela();
                break;
        }
        if (continuar) Console.Clear();
    }
    static void ExportadorNome(GestorDeFilas gestorDeFilas)
    {
        string nomeArquivo;
        Console.Clear();
        Console.WriteLine($"{Interface.Bold}0....{Interface.Reset} para voltar ao menu");
        Console.WriteLine($"{Interface.Bold}null.{Interface.Reset} para data atual");
        Console.Write($"\n{Interface.Bold}Digite o nome do arquivo para exportar as estatísticas: {Interface.Reset}");
        nomeArquivo = Console.ReadLine();
        if (nomeArquivo == "0")
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(nomeArquivo))
        {
            nomeArquivo = DateTime.Now.ToString("dd.MM.yyyy_H:mm");
        }
        gestorDeFilas.ExportarEstatisticas(nomeArquivo + ".txt");
    }
    static void MostrarMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine($"\n{Interface.Bold}--- Centro de emprego ---{Interface.Reset}\n");
        Console.WriteLine($"{Interface.Yellow}1.{Interface.Reset} Atribuir senha");
        Console.WriteLine($"{Interface.Yellow}2.{Interface.Reset} Chamar próxima senha");
        Console.WriteLine($"{Interface.Yellow}3.{Interface.Reset} Consultar estatísticas");
        Console.WriteLine($"{Interface.Yellow}4.{Interface.Reset} Exibir fila");
        Console.WriteLine($"{Interface.Red}0.{Interface.Reset} {Interface.Red}Sair{Interface.Reset}");
        Console.WriteLine($"{Interface.Red}{Interface.Bold}X. Sair sem backup{Interface.Reset}");
        Console.Write(Interface.EscOp);
    }
}
