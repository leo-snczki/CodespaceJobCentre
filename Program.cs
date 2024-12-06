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
        Console.WriteLine($"\n{Cor.Yellow}Senha atribuída: {Cor.Bold}{senha.Numero} - {Nome}{Cor.Reset}");
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
                Console.Clear();
                Console.WriteLine($"\n{Cor.Green}Atendido: {senha.Numero} - {senha.TipoServico}{Cor.Reset}");
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
        Console.Write($"\n{Cor.Bold}Escolha uma opção: {Cor.Reset} ");
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
            Console.WriteLine($"{Cor.Green}Estatísticas exportadas para {caminhoArquivo}{Cor.Reset}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Cor.Red}Erro ao exportar estatísticas: {ex.Message}{Cor.Reset}");
        }
    }
    public void ExibirFila()
    {
        Console.Clear();
        Console.WriteLine($"\n{Cor.Bold}--- Fila Completa ---{Cor.Reset}");

        foreach (var servico in servicos)
        {
            // Exibir as senhas de cada serviço
            Console.WriteLine($"\n{Cor.Bold}{servico.Nome}:{Cor.Reset}");
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
    static void MenuEscolha(ref bool continuar,ref GestorDeFilas gestorDeFilas)
    {

        string op1;
        string op2;
        DateTime currentDate = DateTime.Now;
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
                Console.Write($"\n{Cor.Bold}Escolha uma opção: {Cor.Reset} ");
                op2 = Console.ReadLine();
                switch (op2)
                {
                    case "1":
                        gestorDeFilas.ConsultarEstatisticas();
                        LimparTela();
                        break;
                    case "2":
                        ExportadorNome(gestorDeFilas);
                        LimparTela();
                        break;
                    case "3":
                        ExportadorNome(gestorDeFilas);
                        gestorDeFilas.ConsultarEstatisticas();
                        LimparTela();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine($"\n{Cor.Red}Opção indisponível{Cor.Reset}");
                        LimparTela();
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
                Console.WriteLine($"\n{Cor.Red}Opção indisponível{Cor.Reset}");
                LimparTela();
                break;
        }
        if (continuar) Console.Clear();
    }
    static void ExportadorNome(GestorDeFilas gestorDeFilas)
    {
        string nomeArquivo;
        Console.Clear();
        Console.WriteLine($"{Cor.Bold}0....{Cor.Reset} para voltar ao menu");
        Console.WriteLine($"{Cor.Bold}null.{Cor.Reset} para data atual");
        Console.Write($"\n{Cor.Bold}Digite o nome do arquivo para exportar as estatísticas: {Cor.Reset}");
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
        Console.WriteLine($"{Cor.Yellow}4.{Cor.Reset} Exibir fila");
        Console.WriteLine($"{Cor.Red}0.{Cor.Reset} {Cor.Red}Sair{Cor.Reset}");
        Console.WriteLine($"{Cor.Red}{Cor.Bold}X. Sair sem backup{Cor.Reset}");
        Console.Write($"{Cor.Bold}\nEscolha uma opção: {Cor.Reset} ");
    }
}
