// Exemplo de Serviço (DashboardService.cs)

using CadastroFuncionarios;
using System.Collections.Generic;
using System;
using System.Linq;

public class DashboardService
{
    private readonly List<Funcionario> _funcionarios; // Assume que este é o seu acesso a dados

    public DashboardService(List<Funcionario> funcionarios)
    {
        _funcionarios = funcionarios; // Injete seu contexto de dados aqui
    }

    public DashboardData GetDashboardData()
    {
        var dataAtual = new DateTime(2025, 10, 10); // Data de referência do seu print

        // 1. Cálculos de Idade e Tempo de Casa
        var dadosTemporais = _funcionarios.Select(f => new
        {
            IdadeAnos = (int)((dataAtual - f.DataNascimento).TotalDays / 365.25),
            TempoCasaDias = (dataAtual - f.DataAdmissao).TotalDays
        }).ToList();

        double mediaIdade = dadosTemporais.Average(d => d.IdadeAnos);
        double maiorTempoDias = dadosTemporais.Max(d => d.TempoCasaDias);

        // 2. Agrupamentos (Raça e Pagamento)
        var racaData = _funcionarios
            .GroupBy(f => f.Raca)
            .Select(g => new ContagemItem { Categoria = g.Key, Count = g.Count() })
            .ToList();

        var pagamentoData = _funcionarios
            .GroupBy(f => f.FormaPagamento)
            .Select(g => new ContagemItem { Categoria = g.Key, Count = g.Count() })
            .ToList();

        // 3. Admissões ao Longo do Tempo (Gráfico de Linha)
        var admissoesData = _funcionarios
            .GroupBy(f => new { f.DataAdmissao.Year, f.DataAdmissao.Month })
            .Select(g => new AdmissaoItem
            {
                MesAno = $"{g.Key.Month:00}/{g.Key.Year}", // Formato MM/AAAA
                Count = g.Count()
            })
            .OrderBy(a => a.MesAno)
            .ToList();


        return new DashboardData
        {
            TotalRegistros = _funcionarios.Count,
            IdadeMedia = $"{Math.Round(mediaIdade)} anos",
            MaiorTempoCasa = FormatTempoCasa(maiorTempoDias),
            DistribuicaoRaca = racaData,
            FormasPagamento = pagamentoData,
            AdmissoesPorMes = admissoesData
        };
    }

    // Função auxiliar para formatar o maior tempo de casa (aprox.)
    private string FormatTempoCasa(double totalDias)
    {
        var anos = (int)(totalDias / 365.25);
        var diasRestantes = totalDias % 365.25;
        var meses = (int)(diasRestantes / 30.44); // Média de dias no mês
        return $"{anos} anos e {meses} meses";
    }
}