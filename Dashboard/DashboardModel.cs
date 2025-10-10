// Models para estruturar a resposta do Dashboard

using System.Collections.Generic;

public class DashboardData
{
    public int TotalRegistros { get; set; }
    public string IdadeMedia { get; set; }
    public string MaiorTempoCasa { get; set; }
    public List<ContagemItem> DistribuicaoRaca { get; set; }
    public List<ContagemItem> FormasPagamento { get; set; }
    public List<AdmissaoItem> AdmissoesPorMes { get; set; }
}

public class ContagemItem
{
    public string Categoria { get; set; }
    public int Count { get; set; }
}

public class AdmissaoItem
{
    public string MesAno { get; set; }
    public int Count { get; set; }
}