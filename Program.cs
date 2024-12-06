using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CodespaceJobCentre;

public static class Interface// Classe estática para definir a interface visual (cores e estilos).
{
    public static string Bold => "\x1b[1m"; // Texto em negrito.
    public static string Red => "\x1b[31m"; // Texto vermelho.
    public static string Green => "\x1b[32m"; // Texto verde.
    public static string Yellow => "\x1b[33m"; // Texto amarelo.
    public static string OpcaoErro => $"\n{Interface.Red}Opção indisponível.{Interface.Reset}"; // Mensagem de erro.
    public static string EscOp => $"{Interface.Bold}\nEscolha uma opção: {Interface.Reset} "; // Esquema do prompt para o utilizador.
    public static string Reset => "\x1b[0m"; // Reset ao estilo padrão do texto.
    public static void LimparTela() // Aguarda interação do utilizador.
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        Console.ReadKey();
        Console.Clear(); // Limpa o ecrã.
    }
}


public class Senha // uma senha para atendimento
{
    public string Numero { get; set; } // Número da senha
    public string TipoServico { get; set; } // Tipo de serviço associado à senha
    public bool Atendida { get; set; } // Indica se a senha já foi atendida

    public Senha(string numero, string tipoServico)
    {
        Numero = numero;
        TipoServico = tipoServico;
        Atendida = false; // Inicialmente, a senha não foi atendida
    }

    public void Atender() => Atendida = true; // Marca a senha como atendida
}

public class Servico
{
    public string Nome { get; set; } // Nome do serviço.
    public string Prefixo { get; set; } // Prefixo usado para gerar o número da senha
    private Queue<Senha> filaSenhasPendentes = new Queue<Senha>(); // Queue para senhas Pendentes.
    private List<Senha> filaSenhasAtendidas = new List<Senha>(); // Lista para senhas Atendidas.
    private int contadorSenhas = 0; // Variável privada que será usada para contar o número em si da senha.

    public Servico(string nome, string prefixo)
    {
        Nome = nome;
        Prefixo = prefixo;
    }

    public void AtribuirSenha()
    {
        string numeroSenha;
        contadorSenhas++; // Aumenta o contador de senha.
        numeroSenha = $"{Prefixo}{contadorSenhas:D3}"; // Cria uma senha formatada com 3 espaços para números.
        var senha = new Senha(numeroSenha, Nome);
        Console.Clear();
        filaSenhasPendentes.Enqueue(senha); // Adiciona a nova senha à fila.
        Console.WriteLine($"\n{Interface.Yellow}Senha atribuída: {Interface.Bold}{senha.Numero} - {Nome}{Interface.Reset}"); // Confirmação da senha atribuída.
    }

    public Senha ChamarProximaSenha() // Método para chamar a próxima senha da fila e marcá-la como atendida.
    {
        if (filaSenhasPendentes.Count > 0) // Verifica se há senhas na fila.
        {
            Senha senha = filaSenhasPendentes.Dequeue(); // Retira senha da fila de senhas pendentes.
            senha.Atender(); // Marca a senha como atendida.
            filaSenhasAtendidas.Add(senha); // Adiciona à lista de senhas atendidas.
            return senha;
        }
        return null;
    }
    public int QuantidadeSenhasPendentes() => filaSenhasPendentes.Count; // Método para retornar a quantidade de senhas que ainda estão pendentes.

    public int QuantidadeSenhasAtendidas() => filaSenhasAtendidas.Count; // Método para retornar a quantidade de senhas que já foram atendidas.

    public string UltimaSenhaAtendida()
    {
        if (filaSenhasAtendidas.Count > 0)
            return filaSenhasAtendidas.Last().Numero; // Retorna o número da última senha atendida.
        return "Nenhuma"; // Caso não haja senhas, retorna "Nenhuma".
    }

    public string UltimaSenha()
    {
        if (filaSenhasPendentes.Count > 0)
            return filaSenhasPendentes.Last().Numero; // Retorna o número da última senha gerada.
        return "Nenhuma"; // Caso não haja senhas, retorna "Nenhuma".
    }

    public void ExibirSenhaPendentes() // Exibe o número de cada senha pendente.
    {
        foreach (var senha in filaSenhasPendentes) // Itera pela fila e demonstra as senhas pendentes.
        {
            Console.WriteLine($"- {senha.Numero}");
        }
    }
}

public class GestorDeFilas
{
    private List<Servico> servicos; // Lista com os tipos de serviços.

    public GestorDeFilas()
    {
        servicos = new List<Servico> // fácil possibilidade de acrésimo de serviços no futuro.
        {
            new Servico("Inscrição/Atualização de Dados", "A"), // 0.
            new Servico("Apoio à Procura de Emprego", "B"), // 1.
            new Servico("Informações Gerais", "C") // 2.
        };
    }

    public void AtribuirSenha()
    {
        int escolha; // Variável da opcão que será escolhida pela utilizador.
        Console.Clear();
        MostrarMenuSecundário(); // Autoexplicativo.

        if (int.TryParse(Console.ReadLine(), out escolha) && escolha > 0 && escolha <= servicos.Count) // Lê a escolha do utilizador e verifica se é válida.
        {
            servicos[escolha - 1].AtribuirSenha();
            Interface.LimparTela();
        }
        else if (escolha == 0) return; // Se for 0, retorna ao menu principal.

        else // se for inválido, limpa o ecrã e mostra msg de erro.
        {
            Console.Clear();
            Console.WriteLine(Interface.OpcaoErro); // Demostra erro.
            Console.WriteLine("\nPressione qualquer tecla para continuar..."); // Para o utilizador conseguir ler o console com tempo.
            Console.ReadKey();
        }
    }

    public void ChamarProximaSenha()
    {
        int escolha; // Variável da opcão que será escolhida pela utilizador.
        Console.Clear();
        MostrarMenuSecundário(); // Autoexplicativo.

        if (int.TryParse(Console.ReadLine(), out escolha) && escolha > 0 && escolha <= servicos.Count)  // Lê a escolha do utilizador e verifica se é válida.
        {
            Senha senha = servicos[escolha - 1].ChamarProximaSenha(); // i - 1 devido aos indices da lista.
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
        else if (escolha == 0) return;  // Se não for válida, e se for 0, volta ao menu principal.

        else // Se não for válida, nem for 0, exibe messagem de erro.
        {
            Console.Clear();
            Console.WriteLine(Interface.OpcaoErro); // Demostra erro.
            Console.WriteLine("\nPressione qualquer tecla para continuar..."); // Para o utilizador conseguir ler o console com tempo.
            Console.ReadKey();
        }

    }
    public void MostrarMenuSecundário() // Exibe o menu dos serviços.
    {
        Console.WriteLine($"\n{Interface.Bold}--- Selecione o serviço ---{Interface.Reset}\n");
        for (int i = 0; i < servicos.Count; i++) // Exibe os serviços disponíveis com um loop for.
        {
            Console.WriteLine($"{Interface.Yellow}{i + 1}.{Interface.Reset} {servicos[i].Nome}"); // i + 1 devido aos indices da lista.
        }
        Console.WriteLine($"{Interface.Red}0. Sair{Interface.Reset}");
        Console.Write(Interface.EscOp); // Mostra no console, perguntando qual será a opção.
    }

    public void ConsultarEstatisticas() // Consulta local da execução do programa.
    {
        int totalPendentes = 0;
        int totalAtendidas = 0;
        string ultimaSenhaPendente = "Nenhuma";
        string ultimaSenhaAtendida = "Nenhuma";
        Console.Clear();
        Console.WriteLine($"\n{Interface.Bold}--- Estatísticas ---{Interface.Reset}\n");
        foreach (var tipoServico in servicos) // Itinera e mostra no console, o mesmo layout para cada serviço.
        {
            Console.WriteLine($"{Interface.Bold}{tipoServico.Nome}:{Interface.Reset}");
            Console.WriteLine($"- Senhas atendidas: {tipoServico.QuantidadeSenhasAtendidas()}");
            Console.WriteLine($"- Senhas pendentes: {tipoServico.QuantidadeSenhasPendentes()}");
            Console.WriteLine($"- Última senha solicitada: {tipoServico.UltimaSenha()}");
            Console.WriteLine($"- Última senha atendida: {tipoServico.UltimaSenhaAtendida()}\n");

            totalPendentes += tipoServico.QuantidadeSenhasPendentes();
            totalAtendidas += tipoServico.QuantidadeSenhasAtendidas();

            if (tipoServico.QuantidadeSenhasPendentes() > 0) // Atualiza a última senha pendente (se houver)
                ultimaSenhaPendente = tipoServico.UltimaSenha();

            if (tipoServico.QuantidadeSenhasAtendidas() > 0) // Atualiza a última senha pendente (se houver)
                ultimaSenhaAtendida = tipoServico.UltimaSenhaAtendida();
        }

        Console.WriteLine($"\n{Interface.Bold}--- Estatísticas Gerais ---{Interface.Reset}\n"); // Demonstra as estatísticas gerais.
        Console.WriteLine($"Total de senhas pendentes: {totalPendentes}");
        Console.WriteLine($"Total de senhas atendidas: {totalAtendidas}");
        Console.WriteLine($"Última senha pendente: {ultimaSenhaPendente}");
        Console.WriteLine($"Última senha atendida: {ultimaSenhaAtendida}\n");
    }
    
    public void MostrarMenuTerciário()
    {
        Console.WriteLine($"\n{Interface.Bold}--- Estatísticas ---{Interface.Reset}\n");
        Console.WriteLine($"{Interface.Yellow}1.{Interface.Reset} Consultar");
        Console.WriteLine($"{Interface.Yellow}2.{Interface.Reset} Exportar");
        Console.WriteLine($"{Interface.Yellow}3.{Interface.Reset} Consultar e Exportar");
        Console.WriteLine($"{Interface.Red}0. Sair{Interface.Reset}");
        Console.Write(Interface.EscOp); // Mostra no console, perguntando qual será a opção.
    }

    public void ExportarEstatisticas(string caminhoArquivo) // Autoexplicativo.
    {
        try // Facilidade de identificar erros de exportação.
        {
            int totalPendentes = 0;
            int totalAtendidas = 0;
            string ultimaSenhaPendente = "Nenhuma";
            string ultimaSenhaAtendida = "Nenhuma";
            List<string> linhas = new List<string>();
            foreach (var tipoServico in servicos) // Escreve o mesmo layout para cada serviço.
            {
                linhas.Add($"Serviço: {tipoServico.Nome}");
                linhas.Add($"Senhas atendidas: {tipoServico.QuantidadeSenhasAtendidas()}");
                linhas.Add($"Senhas pendentes: {tipoServico.QuantidadeSenhasPendentes()}");
                linhas.Add($"Última senha: {tipoServico.UltimaSenha()}");
                linhas.Add($"Última senha atendida: {tipoServico.UltimaSenhaAtendida()}\n");
                totalPendentes += tipoServico.QuantidadeSenhasPendentes();
                totalAtendidas += tipoServico.QuantidadeSenhasAtendidas();
            }

            linhas.Add($"\n{Interface.Bold}--- Estatísticas Gerais ---{Interface.Reset}\n"); // Demonstra as estatísticas gerais.
            linhas.Add($"Total de senhas pendentes: {totalPendentes}"); // Mostra o total de senhas pendentes no final da execução do programa.
            linhas.Add($"Total de senhas atendidas: {totalAtendidas}"); // Mostra o total de senhas atendidas no final da execução do programa.
            linhas.Add($"Última senha pendente: {ultimaSenhaPendente}");
            linhas.Add($"Última senha atendida: {ultimaSenhaAtendida}\n");


            File.WriteAllLines(caminhoArquivo, linhas); // Escreve as linhas no arquivo estabelecido.
            Console.WriteLine($"{Interface.Green}Estatísticas exportadas para {caminhoArquivo}{Interface.Reset}"); // Confirmação doa exportação.

        }
        catch (Exception ex)
        {
            Console.WriteLine($"{Interface.Red}Erro ao exportar estatísticas: {ex.Message}{Interface.Reset}"); // Explicação do erro de exportação.
        }
    }

    public void ExportadorNome()
    {
        string nomeArquivo; // Variável que será usada para guardar o nome do arquivo de texto que será exportado.
        Console.Clear();
        Console.WriteLine($"{Interface.Bold}0....{Interface.Reset} para voltar ao menu"); // 0 volta ao menu.
        Console.WriteLine($"{Interface.Bold}null.{Interface.Reset} para data atual"); // Null deixará o nome do arquivo com a data do momento.
        Console.Write($"\n{Interface.Bold}Digite o nome do arquivo para exportar as estatísticas: {Interface.Reset}");
        nomeArquivo = Console.ReadLine(); // Input do utilizador.
        if (nomeArquivo == "0") return; // volta ao menu principal.

        if (string.IsNullOrWhiteSpace(nomeArquivo)) // se for null ou algum espaço atoa, vai dar o nome do arquivo com a data.
        {
            nomeArquivo = DateTime.Now.ToString("dd-MM-yyyy_H-mm");
        }
        ExportarEstatisticas(nomeArquivo + ".txt"); // Exporta as estatísticas com o nome do arquivo mais .txt.
    }

    public void ExibirFila() // Autoexplicativo.
    {
        Console.Clear();
        Console.WriteLine($"\n{Interface.Bold}--- Fila Completa ---{Interface.Reset}");

        foreach (var servico in servicos) // itinera com um loop foreach a lista de "servicos".
        {
            Console.WriteLine($"\n{Interface.Bold}{servico.Nome}:{Interface.Reset}"); // Título do serviço.
            servico.ExibirSenhaPendentes(); // Exibir as senhas de cada serviço.
        }

        Console.WriteLine("\nPressione qualquer tecla para continuar..."); // Para o utilizador conseguir ler o console com tempo.
        Console.ReadKey();
    }
}

class Program
{
    static void Main()
    {
        var gestor = new GestorDeFilas(); // Instatiação da classe GestorDeFilas.
        bool continuar = true; // Execução do programa.

        while (continuar) // Ciclo principal do programa, onde o utilizador pode escolher o que fazer.
        {
            MostrarMenuPrincipal();
            MenuEscolha(ref continuar, ref gestor);
        }
    }
    static void MenuEscolha(ref bool continuar, ref GestorDeFilas gestor)
    {

        string op1, op2; // Declaração de duas váriaveis para opções.
        DateTime currentDate = DateTime.Now; // Variável para armazenar a hora atual para fins de estátistica.
        op1 = Console.ReadLine(); // Seleciona a opcão com base no menu principal.
        switch (op1)
        {
            case "1": // Cria uma senha para tal serviço.
                gestor.AtribuirSenha();
                break;
            case "2": // Atende uma senha para tal serviço.
                gestor.ChamarProximaSenha();
                break;
            case "3": // Tudo sobre as estatísticas, como menu e chamadas dos respectivos métodos.
                Console.Clear();
                gestor.MostrarMenuTerciário();
                op2 = Console.ReadLine();
                switch (op2)
                {
                    case "1": // Consulta.
                        gestor.ConsultarEstatisticas();
                        Interface.LimparTela();
                        break;
                    case "2": // Exportação.
                        gestor.ExportadorNome();
                        Interface.LimparTela();
                        break;
                    case "3": // Consulta e exportação.
                        gestor.ExportadorNome(); // Define o nome do arquivo que será exportado.
                        gestor.ConsultarEstatisticas();
                        Interface.LimparTela();
                        break;
                    case "0":
                        return; // Sai para menu.
                    default:
                        Console.WriteLine(Interface.OpcaoErro); // default demonstra erro.
                        Interface.LimparTela();
                        break;
                }
                break;
            case "4": // Exibe as senhas pendentes.
                gestor.ExibirFila();
                break;
            case "0": // Sai do programa COM backup das estatísticas.
                gestor.ExportarEstatisticas("estatisticas-backup.txt");
                continuar = false;
                break;
            case "X": // Sai do programa SEM backup das estatísticas.
                continuar = false;
                break;
            default:
                Console.Clear();
                Console.WriteLine(Interface.OpcaoErro); // default demonstra erro.
                Interface.LimparTela();
                break;
        }
        if (continuar) Console.Clear();
    }
    static void MostrarMenuPrincipal() // Menu de maior utilização do programa.
    {
        Console.Clear();
        Console.WriteLine($"\n{Interface.Bold}--- Centro de emprego ---{Interface.Reset}\n");
        Console.WriteLine($"{Interface.Yellow}1.{Interface.Reset} Atribuir senha");
        Console.WriteLine($"{Interface.Yellow}2.{Interface.Reset} Chamar próxima senha");
        Console.WriteLine($"{Interface.Yellow}3.{Interface.Reset} Consultar estatísticas");
        Console.WriteLine($"{Interface.Yellow}4.{Interface.Reset} Exibir fila");
        Console.WriteLine($"{Interface.Red}0.{Interface.Reset} {Interface.Red}Sair{Interface.Reset}");
        Console.WriteLine($"{Interface.Red}{Interface.Bold}X. Sair sem backup{Interface.Reset}");
        Console.Write(Interface.EscOp); // Mostra no console, perguntando qual será a opção.
    }
}
